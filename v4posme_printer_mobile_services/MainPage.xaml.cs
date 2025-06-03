using System;
using System.Diagnostics;
#if ANDROID
using Android.Content;
using Android.OS;
#endif
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;
using v4posme_printer_mobile_services.Services;
namespace v4posme_printer_mobile_services;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        try
        {
            #if ANDROID
            if (!Android.OS.Environment.IsExternalStorageManager)
            {
                StorageAccessHelper.RequestManageExternalStoragePermission();
            }

            var context = Android.App.Application.Context;
            var intent  = new Intent(context, typeof(PosmeWatcherService));
            context.StartForegroundService(intent);
            #endif
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    private async void OnStopClicked(object? sender, EventArgs e)
    {
        try
        {
            #if ANDROID
            var intent = new Android.Content.Intent(Android.App.Application.Context, typeof(PosmeWatcherService));
            Android.App.Application.Context.StopService(intent);
            #endif
            await DisplayAlert("Servicio detenido", "La vigilancia de archivos ha sido detenida.", "OK");
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception.Message);
        }
    }
}