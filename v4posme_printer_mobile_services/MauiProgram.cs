using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Unity;
using v4posme_printer_mobile_services.Services.Repository;
using v4posme_printer_mobile_services.Services.SystemNames;
using DevExpress.Maui;

namespace v4posme_printer_mobile_services;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseDevExpress(useLocalization: false)
            .UseDevExpressScheduler()
            .UseDevExpressDataGrid()
            .UseDevExpressCharts()
            .UseDevExpressEditors()
            .UseDevExpressCollectionView()
            .UseDevExpressControls()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        VariablesGlobales.UnityContainer.RegisterType<IRepositoryTbParameterSystem, RepositoryTbParameterSystem>();

        #if DEBUG
        builder.Logging.AddDebug();
        #endif

        return builder.Build();
    }
}