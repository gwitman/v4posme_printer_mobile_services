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

            var downloadPath = directoryDownloads.AbsolutePath;
            var parameterPrefijo = await repositoryTbParameterSystem.PosMeFindPrefijo();
            var pdfFiles = Directory.GetFiles(downloadPath, $"{parameterPrefijo.Value}*.pdf");
            var parameterPrinter = await repositoryTbParameterSystem.PosMeFindPrinter();

            if (string.IsNullOrWhiteSpace(parameterPrinter.Value))
            {
                return;
            }

            var printer = new Printer(parameterPrinter.Value);
            foreach (var file in pdfFiles)
            {
                var filename = Path.GetFileName(file);
                try
                {
                    var pathFile = new Java.IO.File(file);
                    Debug.WriteLine(pathFile.ToURI());
                    var readBase64 = PdfRendererHelper.ConvertPdfToBase64(pathFile);
                    foreach (var read in readBase64)
                    {
                        var image = Convert.FromBase64String(read);
                        printer.Image(SKBitmap.Decode(image));
                        printer.Print();
                    }

                    // Quitar el prefijo "posme_" del nombre del archivo
                    var newFilename = filename.Replace("posme_", "impreso_");
                    var newPath = Path.Combine(downloadPath, newFilename);
                    File.Move(file, newPath);
                    System.Diagnostics.Debug.WriteLine($"Renombrado: {filename} → {newFilename}");
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

    public override IBinder? OnBind(Intent? intent) => null;

    public override void OnDestroy()
    {
        base.OnDestroy();
        timer?.Stop();
        timer?.Dispose();
    }
}