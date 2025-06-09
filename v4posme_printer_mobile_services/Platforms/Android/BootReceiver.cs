using Android.App;
using Android.Content;
using Android.OS;
using Debug = System.Diagnostics.Debug;

namespace v4posme_printer_mobile_services;

[BroadcastReceiver(Enabled = true, Exported = true)]
[IntentFilter(new[] { Intent.ActionBootCompleted })]
public class BootReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        Debug.WriteLine("Test de BootReceiver");
        if (intent.Action == Intent.ActionBootCompleted)
        {
            var serviceIntent = new Intent(context, typeof(PosmeWatcherService));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                context.StartForegroundService(serviceIntent);
            else
                context.StartService(serviceIntent);
        }
    }
}