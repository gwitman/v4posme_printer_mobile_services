using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if ANDROID
using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
#endif

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using v4posme_printer_mobile_services.Services.SystemNames;

namespace v4posme_printer_mobile_services.Services;

public class PermissionsService
{
    public static async Task<bool> RequestLocationPermissionAsync()
    {
        // Verifica el estado del permiso FINE_LOCATION
        var fineLocationStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (fineLocationStatus != PermissionStatus.Granted)
        {
            // Solicita el permiso
            fineLocationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        // Verifica si el permiso fue otorgado
        return fineLocationStatus == PermissionStatus.Granted;
    }

    public static async Task<bool> CheckAndRequestPermissionsAsync()
    {
        var permissions = new Permissions.BasePlatformPermission[]
        {
            new Permissions.LocationWhenInUse(),
            new Permissions.LocationAlways(),
            new Permissions.Camera(),
            new Permissions.StorageRead(),
            new Permissions.StorageWrite(),
        };

        var allPermissionsGranted = true;

        foreach (var permission in permissions)
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
                if (status != PermissionStatus.Granted)
                {
                    allPermissionsGranted = false;
                    await Application.Current.MainPage.DisplayAlert(Constantes.PermisoDenegado, permission.GetType().Name, "OK");
                }
            }
        }

        #if ANDROID
        var bluetoothOk         = await CheckBluetoothPermissionsAndroid12Async();
        allPermissionsGranted   &= bluetoothOk;
        #endif

        return allPermissionsGranted;
    }
    public static Task<bool> CheckBluetoothPermissionsAndroid12Async()
    {
        #if ANDROID
        var context                 = Android.App.Application.Context;
        var permissionsToRequest    = new List<string>();

        if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.BluetoothConnect) != Permission.Granted)
            permissionsToRequest.Add(Manifest.Permission.BluetoothConnect);

        if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.BluetoothScan) != Permission.Granted)
            permissionsToRequest.Add(Manifest.Permission.BluetoothScan);

        if (permissionsToRequest.Any())
        {
            var activity = Platform.CurrentActivity;
            ActivityCompat.RequestPermissions(activity, permissionsToRequest.ToArray(), 1001);
            return Task.FromResult(false); // Aún no tienes permisos
        }

        #endif
        return Task.FromResult(true);
    }


}