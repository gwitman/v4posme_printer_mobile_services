using System;
using Android;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

// Para evitar que el filtrado por nombre consuma permisos de ubicación
[assembly: UsesPermission(Manifest.Permission.Bluetooth, MaxSdkVersion = 30)]
[assembly: UsesPermission(Manifest.Permission.BluetoothAdmin, MaxSdkVersion = 30)]
[assembly: UsesPermission(Manifest.Permission.AccessFineLocation, MaxSdkVersion = 30)]
[assembly: UsesPermission(Manifest.Permission.AccessCoarseLocation, MaxSdkVersion = 30)]
[assembly: UsesPermission(Manifest.Permission.ForegroundService, MaxSdkVersion = 30)]
namespace v4posme_printer_mobile_services;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}