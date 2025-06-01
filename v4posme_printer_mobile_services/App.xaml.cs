using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using v4posme_printer_mobile_services.Services;

namespace v4posme_printer_mobile_services;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        var dataBase = new DataBase();
        dataBase.Init();
        UserAppTheme = AppTheme.Light;


    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    protected override async void OnStart()
    {
        try
        {
            base.OnStart();
            var permissionsGranted = await PermissionsService.CheckAndRequestPermissionsAsync();
            while (!permissionsGranted)
            {
                permissionsGranted = await PermissionsService.CheckAndRequestPermissionsAsync();
                if (!permissionsGranted)
                {
                    await Current?.MainPage?.DisplayAlert(
                        "Advertencia","No permissions granted",
                        "OK")!;
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}