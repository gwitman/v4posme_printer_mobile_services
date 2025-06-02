using System;
using Android.App;
using Android.Content;
using Android.OS;
using System.IO;
using AndroidX.Core.App;
using SkiaSharp;
using Unity;
using v4posme_printer_mobile_services.Services.HelpersPrinters;
using v4posme_printer_mobile_services.Services.Repository;
using v4posme_printer_mobile_services.Services.SystemNames;
using Debug = System.Diagnostics.Debug;
using Environment = Android.OS.Environment;

namespace v4posme_printer_mobile_services;

[Service(Name = "com.v4posme.service.PosmeWatcherService", Exported = true, ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync)]
public class PosmeWatcherService : Service
{
    private System.Timers.Timer? timer;
    public const int ServiceNotificationId = 1001;
    private readonly IRepositoryTbParameterSystem repositoryTbParameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();

    public override void OnCreate()
    {
        try
        {
            base.OnCreate();
            CreateNotificationChannel();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message);
        }
    }

    public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
    {
        var notification = BuildNotification("Scanning Download file", "Servicio de impresión directa posme");

        // Solo necesitas una llamada a StartForeground
        StartForeground(ServiceNotificationId, notification);

        // Verifica permisos antes de continuar
        if (!Environment.IsExternalStorageManager)
        {
            StopSelf(); // detener servicio si no hay permisos
            return StartCommandResult.NotSticky;
        }

        timer?.Stop();
        timer = new System.Timers.Timer(Convert.ToInt32(repositoryTbParameterSystem.PosMeFindInterval().Result.Value));
        timer.Elapsed += (s, e) => ScanDownloads();
        timer.Start();

        return StartCommandResult.Sticky;
    }


    public Notification BuildNotification(string title, string text)
    {
        return NotificationHelper.BuildNotification(this, title, text);
    }

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(
                "posme_channel",
                "Buscando archivos posMe a imprimir",
                NotificationImportance.High)
            {
                Description = "Canal para servicio de impresión"
            };
            
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }

    private async void ScanDownloads()
    {
        try
        {
            var directoryDownloads = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads);
            if (directoryDownloads is null)
            {
                return;
            }

            var downloadPath        = directoryDownloads.AbsolutePath;
            var parameterPrefijo    = await repositoryTbParameterSystem.PosMeFindPrefijo();
            var pdfFiles            = Directory.GetFiles(downloadPath, $"{parameterPrefijo.Value}*.pdf");
            var parameterPrinter    = await repositoryTbParameterSystem.PosMeFindPrinter();
            var parameterCopias     = await repositoryTbParameterSystem.PosMeFindCantidadCopias();
            var copias              = Convert.ToInt32(parameterCopias.Value);
            if (string.IsNullOrWhiteSpace(parameterPrinter.Value))
            {
                return;
            }

            var printer = new Printer(parameterPrinter.Value);
            foreach (var file in pdfFiles)
            {
                var filename = Path.GetFileName(file);
                if (!filename.Contains(parameterPrefijo.Value!, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                try
                {
                    using var pathFile = new Java.IO.File(file);
                    if (!pathFile.Exists())
                    {
                        System.Diagnostics.Debug.WriteLine($"Archivo original no existe: {pathFile.AbsolutePath}");
                        return;
                    }
                    var readBase64 = PdfRendererHelper.ConvertPdfToBase64(pathFile);
                    // Quitar el prefijo "posme_" del nombre del archivo
                    var newFilename = filename.Replace(parameterPrefijo.Value, "impreso_");
                    var newPath     = Path.Combine(downloadPath, newFilename);
                    var renamed     = pathFile.RenameTo(new Java.IO.File(newPath));
                    if (renamed)
                    {
                        System.Diagnostics.Debug.WriteLine($"Renombrado: {filename} → {newFilename}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al renombrar: {filename} → {newFilename}");
                        return;
                    }
                        
                    switch (readBase64.Length)
                    {
                        case 1:
                            ProcessImage(readBase64[0], printer, copias);
                            break;
                        case > 1:
                        {
                            foreach (var read in readBase64)
                            {
                                ProcessImage(read, printer, copias);
                            }

                            break;
                        }
                        default:
                            System.Diagnostics.Debug.WriteLine($"No hay datos que leer en el archivo {file}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al renombrar {filename}: {ex.Message}");
                }
            }
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Error al escanear {e.Message}");
        }
    }

    private void ProcessImage(string base64Image, Printer printer, int copies = 1)
    {
        if (string.IsNullOrWhiteSpace(base64Image))
            throw new ArgumentException("La cadena Base64 no puede estar vacía", nameof(base64Image));
    
        if (printer == null)
            throw new ArgumentNullException(nameof(printer));
    
        if (copies < 1)
            throw new ArgumentOutOfRangeException(nameof(copies), "El número de copias debe ser al menos 1");

        try
        {
            var imageData       = Convert.FromBase64String(base64Image);
            using var bitmap    = SKBitmap.Decode(imageData);
            if (bitmap == null)
                throw new InvalidOperationException("No se pudo decodificar la imagen desde Base64");
            
            printer.Image(bitmap);
            
            for (var i = 0; i < (copies * 2) ; i++)
            {
                printer.Print();
                System.Diagnostics.Debug.WriteLine($"Imprimiendo archivo {i} procesado");
            }
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException("La cadena Base64 no tiene un formato válido", ex);
        }
    }

    public override IBinder? OnBind(Intent? intent) => null;

    public override void OnDestroy()
    {
        base.OnDestroy();
        timer?.Stop();
        timer?.Dispose();
    }
}