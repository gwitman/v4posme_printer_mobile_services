using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

namespace v4posme_printer_mobile_services;

public class NotificationHelper
{
    private const string ChannelId = "posme_channel";

    public static Notification BuildNotification(Context context, string title, string message)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var manager = (NotificationManager)context.GetSystemService(Context.NotificationService)!;

            var channel = new NotificationChannel(ChannelId, "Posme Watcher", NotificationImportance.Default)
            {
                Description = "Notificaciones del servicio de impresión"
            };

            manager.CreateNotificationChannel(channel);
        }

        var builder = new NotificationCompat.Builder(context, ChannelId)
            .SetContentTitle(title)
            .SetContentText(message)
            .SetSmallIcon(Resource.Drawable.posme) // asegúrate de tener un ícono válido
            .SetOngoing(true) // notificación persistente
            .SetPriority((int)NotificationPriority.High);

        return builder.Build();
    }
}