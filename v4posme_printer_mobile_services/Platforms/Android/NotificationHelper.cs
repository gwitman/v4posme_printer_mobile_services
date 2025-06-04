using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using v4posme_printer_mobile_services.Services.SystemNames;

namespace v4posme_printer_mobile_services;

public class NotificationHelper
{
    public static Notification BuildNotification(Context context, string title, string message)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var manager = (NotificationManager)context.GetSystemService(Context.NotificationService)!;

            var channel = new NotificationChannel(Constantes.ChanelNotification, Constantes.PosmeWatcher, NotificationImportance.Default)
            {
                Description = Constantes.ChanelDescription
            };

            manager.CreateNotificationChannel(channel);
        }

        var builder = new NotificationCompat.Builder(context, Constantes.ChanelNotification)
            .SetContentTitle(title)
            .SetContentText(message)
            .SetSmallIcon(Resource.Drawable.posme) // asegúrate de tener un ícono válido
            .SetOngoing(true) // notificación persistente
            .SetPriority((int)NotificationPriority.High);

        return builder.Build();
    }
}