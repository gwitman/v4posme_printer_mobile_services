using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;

namespace v4posme_printer_mobile_services;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            if (CheckSelfPermission(Android.Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                RequestPermissions(new[] { Android.Manifest.Permission.PostNotifications }, 1001);
            }
            var manager = (NotificationManager)GetSystemService(NotificationService);

            var channelId = "posme_channel";
            var existingChannel = manager.GetNotificationChannel(channelId);
            // Comprobar si el servicio aún se está ejecutando
            if (IsServiceRunning(typeof(PosmeWatcherService)))
            {
                // Reconstruir y volver a mostrar la notificación
                var notification = NotificationHelper.BuildNotification(this,"Scanning Download file", "Servicio de impresión directa posme");
                var service = (NotificationManager)GetSystemService(NotificationService);
                service.Notify(PosmeWatcherService.ServiceNotificationId, notification); // Mismo ID que usaste en StartForeground
            }
            if (existingChannel == null)
            {
                var channel = new NotificationChannel(channelId, "Posme Watcher", NotificationImportance.Low);
                manager.CreateNotificationChannel(channel);
            }
            else if (existingChannel.Importance == NotificationImportance.None)
            {
                // El canal está bloqueado → abrir ajustes de notificaciones
                var intent = new Intent(Android.Provider.Settings.ActionAppNotificationSettings);
                intent.PutExtra(Android.Provider.Settings.ExtraAppPackage, PackageName);
                StartActivity(intent);
            }
        }
    }

    private bool IsServiceRunning(Type serviceType)
    {
        var manager = (ActivityManager)GetSystemService(ActivityService);
        foreach (var service in manager.GetRunningServices(int.MaxValue))
        {
            if (service.Service.ClassName.Equals(serviceType.FullName))
                return true;
        }
        return false;
    }

}