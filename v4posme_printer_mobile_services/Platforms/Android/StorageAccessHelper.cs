using Android.Content;
using Android.Provider;
using v4posme_printer_mobile_services.Services.SystemNames;

namespace v4posme_printer_mobile_services;

public class StorageAccessHelper
{
    public static void RequestManageExternalStoragePermission()
    {
        var uri         = Android.Net.Uri.Parse(Constantes.Package + Android.App.Application.Context.PackageName);
        var intent      = new Intent(Settings.ActionManageAppAllFilesAccessPermission, uri);
        intent.AddFlags(ActivityFlags.NewTask);
        Android.App.Application.Context.StartActivity(intent);
    }
}