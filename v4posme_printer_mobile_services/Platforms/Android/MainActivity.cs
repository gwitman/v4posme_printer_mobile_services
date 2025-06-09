using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;
using v4posme_printer_mobile_services.Services.SystemNames;

namespace v4posme_printer_mobile_services;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            var packageName = PackageName;
            var pm = (PowerManager)GetSystemService(PowerService);
            if (!pm.IsIgnoringBatteryOptimizations(packageName))
            {
                var intent = new Intent(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
                intent.SetData(Android.Net.Uri.Parse(Constantes.Package + packageName));
                StartActivity(intent);
            }
        }

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            if (CheckSelfPermission(Android.Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                RequestPermissions(new[] { Android.Manifest.Permission.PostNotifications }, 1001);
            }
            var manager         = (NotificationManager)GetSystemService(NotificationService);
            var existingChannel = manager.GetNotificationChannel(Constantes.ChanelNotification);
            // Comprobar si el servicio aún se está ejecutando
            if (IsServiceRunning(typeof(PosmeWatcherService)))
            {
                // Reconstruir y volver a mostrar la notificación
                var notification    = NotificationHelper.BuildNotification(this,Constantes.TituloNotificacion, Constantes.ServicioEjecucion);
                var service         = (NotificationManager)GetSystemService(NotificationService);
                service.Notify(PosmeWatcherService.ServiceNotificationId, notification); // Mismo ID que usaste en StartForeground
            }
            else
            {
                var posmeWatcher = new Intent(this, typeof(PosmeWatcherService));
                StartForegroundService(posmeWatcher);
            }
            if (existingChannel == null)
            {
                var channel = new NotificationChannel(Constantes.ChanelNotification, Constantes.PosmeWatcher, NotificationImportance.Low);
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
        else
        {
            var posmeWatcher = new Intent(this, typeof(PosmeWatcherService));
            StartService(posmeWatcher);
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