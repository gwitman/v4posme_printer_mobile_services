using System;
using Android.App;
using Android.Content;
using Android.OS;
using System.IO;
using System.Threading.Tasks;
using AndroidX.Core.App;
using SkiaSharp;
using Unity;
using v4posme_printer_mobile_services.Services.HelpersPrinters;
using v4posme_printer_mobile_services.Services.Repository;
using v4posme_printer_mobile_services.Services.SystemNames;
using Debug = System.Diagnostics.Debug;
using Environment = Android.OS.Environment;

namespace v4posme_printer_mobile_services;

[Service(
    Name        = "com.v4posme.service.PosmeWatcherService", 
    Exported    = true, 
    Enabled     = true, 
    ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync)]
public class PosmeWatcherService : Service
{
    private System.Timers.Timer? _timer;
    private bool _isScanning;
    public const int ServiceNotificationId                                      = 1001;
    private readonly IRepositoryTbParameterSystem _repositoryTbParameterSystem  = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();

    public override void OnCreate()
    {
        try
        {
            base.OnCreate();
            CreateNotificationChannel();
            Debug.WriteLine("Servicio PosmeWatcher creado correctamente");
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error en OnCreate: {e.Message}");
        }
    }

    public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
    {
        try
        {
            var notification = BuildNotification(Constantes.TituloNotificacion, Constantes.ServicioEjecucion);
            StartForeground(ServiceNotificationId, notification, Android.Content.PM.ForegroundService.TypeDataSync);

            if (!CheckPermissions())
            {
                ShowPermissionErrorNotification();
                StopSelf();
                return StartCommandResult.NotSticky;
            }

            InitializeTimer();            
            return StartCommandResult.Sticky;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error en OnStartCommand: {ex.Message}");
            return StartCommandResult.NotSticky;
        }
    }

    private bool CheckPermissions()
    {
        try
        {
            // Verifica múltiples permisos necesarios
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                if (!Environment.IsExternalStorageManager)
                {
                    Debug.WriteLine("Se requiere permiso de gestión de almacenamiento");
                    return false;
                }
            }
            else
            {
                // Para versiones anteriores, verificar otros permisos necesarios
                // Puedes agregar verificaciones adicionales aquí
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error verificando permisos: {ex.Message}");
            return false;
        }
    }

    private void ShowPermissionErrorNotification()
    {
        var notification = new NotificationCompat.Builder(this, Constantes.ChanelNotification)
            .SetContentTitle(Constantes.PermisoRequeridoTitulo)
            .SetContentText(Constantes.PermisoRequeridoText)
            .SetSmallIcon(Resource.Drawable.posme) // Asegúrate de tener este recurso
            .SetPriority(NotificationCompat.PriorityHigh)
            .Build();

        var notificationManager = NotificationManagerCompat.From(this);
        notificationManager.Notify(ServiceNotificationId + 1, notification);
    }

    private void InitializeTimer()
    {
        try
        {
            _timer?.Stop();
            _timer?.Dispose();

            Task.Run(async () =>
            {
                var interval    = await _repositoryTbParameterSystem.PosMeFindInterval();
                _timer          = new System.Timers.Timer(Convert.ToInt32(interval.Value))
                {
                    AutoReset = true
                };
                _timer.Elapsed += async (s, e) => await ScanDownloadsAsync();
                _timer.Start();
                Debug.WriteLine($"Temporizador iniciado con intervalo de {interval.Value}ms");
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error inicializando temporizador: {ex.Message}");
        }
    }

    private Notification BuildNotification(string title, string text)
    {
        return NotificationHelper.BuildNotification(this, title, text);
    }

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(
                Constantes.ChanelNotification,
                Constantes.BuscandoArchivos,
                NotificationImportance.High)
            {
                Description = Constantes.ChanelDescription,
                LockscreenVisibility = NotificationVisibility.Public
            };
            
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }

    private async Task ScanDownloadsAsync()
    {
        if (_isScanning) return;
    
        _isScanning         = true;
        Debug.WriteLine("Iniciando escaneo de archivos...");
        int filesProcessed  = 0;

        try
        {
            var directoryDownloads = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads);
            if (directoryDownloads == null || !directoryDownloads.Exists())
            {
                Debug.WriteLine("No se pudo acceder al directorio de descargas");
                return;
            }

            var downloadPath        = directoryDownloads.AbsolutePath;
            var parameterPrefijo    = await _repositoryTbParameterSystem.PosMeFindPrefijo();
            var parameterPrinter    = await _repositoryTbParameterSystem.PosMeFindPrinter();
            var parameterCopias     = await _repositoryTbParameterSystem.PosMeFindCantidadCopias();

            if (string.IsNullOrWhiteSpace(parameterPrinter.Value))
            {
                Debug.WriteLine("No se ha configurado la impresora");
                return;
            }

            var pdfFiles = Directory.GetFiles(downloadPath, $"{parameterPrefijo.Value}*.pdf");
            if (pdfFiles.Length == 0)
            {
                Debug.WriteLine("No se encontraron archivos para imprimir");
                return;
            }

            var printer = new Printer(parameterPrinter.Value);
            var copias  = Convert.ToInt32(parameterCopias.Value);

            foreach (var file in pdfFiles)
            {
                if (await ProcessFileAsync(file, downloadPath, parameterPrefijo.Value, printer, copias))
                {
                    filesProcessed++;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error durante el escaneo: {ex.Message}");
            UpdateNotification(Constantes.ErrorNotification, $"Error: {ex.Message}");
        }
        finally
        {
            _isScanning = false;
            Debug.WriteLine("Escaneo completado");
        
            // Mostrar notificación de resultado
            var showNotificationParameter   = await _repositoryTbParameterSystem.PosMeFindShowNotificationScan();
            var show = showNotificationParameter.Value == "1";
            if (show)
            {
                ShowScanCompleteNotification(filesProcessed);
            }
        }
    }
    
    private void ShowScanCompleteNotification(int filesProcessed)
    {
        var notification = new NotificationCompat.Builder(this, Constantes.ChanelNotification)
            .SetContentTitle(Constantes.EscaneoCompletadoTitulo)
            .SetContentText(filesProcessed > 0 
                ? Constantes.ArchivosProcesados.Replace("{filesProcessed}", $"{filesProcessed}") 
                : Constantes.ArchivosNoEncontrados)
            .SetSmallIcon(Resource.Drawable.posme)
            .SetPriority(NotificationCompat.PriorityDefault)
            .SetAutoCancel(true)
            .SetDefaults((int)NotificationDefaults.Vibrate)
            .Build();

        var notificationManager = NotificationManagerCompat.From(this);
        notificationManager.Notify(ServiceNotificationId + 2, notification); // ID diferente
    }

    private async Task<bool> ProcessFileAsync(string filePath, string downloadPath, string prefix, Printer printer, int copies)
    {
        try
        {
            var filename        = Path.GetFileName(filePath);
            Debug.WriteLine($"Procesando archivo: {filename}");

            using var pathFile = new Java.IO.File(filePath);
            if (!pathFile.Exists())
            {
                Debug.WriteLine($"Archivo no existe: {pathFile.AbsolutePath}");
                return false;
            }

            var readBase64 = PdfRendererHelper.ConvertPdfToBase64(pathFile);
            if (readBase64 == null || readBase64.Length == 0)
            {
                Debug.WriteLine($"No se pudo convertir el PDF a imágenes: {filename}");
                return false;
            }

            // Renombrar archivo para marcarlo como procesado
            var newFilename     = filename.Replace(prefix, "impreso_");
            var newPath         = Path.Combine(downloadPath, newFilename);
        
            if (File.Exists(newPath))
            {
                Debug.WriteLine($"El archivo ya fue procesado anteriormente: {newFilename}");
                return false;
            }

            try
            {
                File.Move(filePath, newPath);
                Debug.WriteLine($"Archivo renombrado: {filename} → {newFilename}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al renombrar {filename}: {ex.Message}");
                return false;
            }

            // Procesar imágenes
            foreach (var imageBase64 in readBase64)
            {
                if (!string.IsNullOrWhiteSpace(imageBase64))
                {
                    await PrintImageAsync(imageBase64, printer, copies);
                }
            }

            return true; // Indica que el archivo fue procesado exitosamente
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error procesando archivo {filePath}: {ex.Message}");
            return false;
        }
    }

    private async Task PrintImageAsync(string base64Image, Printer printer, int copies)
    {
        try
        {
            await Task.Run(() =>
            {
                var imageData       = Convert.FromBase64String(base64Image);
                using var bitmap    = SKBitmap.Decode(imageData);
                if (bitmap == null)
                {
                    Debug.WriteLine("No se pudo decodificar la imagen desde Base64");
                    return;
                }

                printer.Image(bitmap);
                
                for (var i = 0; i < copies; i++)
                {
                    printer.Print();
                    Debug.WriteLine($"Imprimiendo copia {i + 1} de {copies}");
                }
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al imprimir: {ex.Message}");
        }
    }

    private void UpdateNotification(string title, string text)
    {
        try
        {
            var notification        = BuildNotification(title, text);
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(ServiceNotificationId, notification);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error actualizando notificación: {ex.Message}");
        }
    }

    public override IBinder? OnBind(Intent? intent) => null;

    public override void OnDestroy()
    {
        try
        {
            _timer?.Stop();
            _timer?.Dispose();
            Debug.WriteLine("Servicio PosmeWatcher detenido");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error en OnDestroy: {ex.Message}");
        }
        finally
        {
            base.OnDestroy();
        }
    }
}