using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Unity;
using v4posme_maui.Models;
using v4posme_printer_mobile_services.Services.Repository;
using v4posme_printer_mobile_services.Services.SystemNames;

namespace v4posme_printer_mobile_services.ViewModels;

public class PosMeParameterViewModel : BaseViewModel
{
    private readonly IRepositoryTbParameterSystem repositoryTbParameterSystem;
    private TbParameterSystem posmeFindPrinter              = new();
    private TbParameterSystem posmeFindInterval             = new();
    private TbParameterSystem posmeFindPrefijo              = new();
    private TbParameterSystem posmeFindCantidadCopias       = new();
    private TbParameterSystem posmeFindShowNotificationScan = new();
    
    public ICommand RefreshCommand { get; }
    public ICommand SaveCommand { get; }

    public PosMeParameterViewModel()
    {
        Title = "Parametros";
        repositoryTbParameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();
        Task.Run(async () =>
        {
            var test = await repositoryTbParameterSystem.PosMeCount();
            Debug.WriteLine(test);
        });
        SaveCommand     = new Command(OnSaveParameters);
        RefreshCommand  = new Command(OnRefreshPage);
        LoadValuesDefault();
    }

    private void OnRefreshPage(object obj)
    {
        LoadValuesDefault();
        IsRefreshing = false;
    }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        LoadValuesDefault();
    }

    private async void LoadValuesDefault()
    {
        try
        {
            posmeFindPrinter = await repositoryTbParameterSystem.PosMeFindPrinter();
            if (!string.IsNullOrWhiteSpace(posmeFindPrinter.Value))
            {
                Printer = posmeFindPrinter.Value;
            }
            posmeFindInterval = await repositoryTbParameterSystem.PosMeFindInterval();
            if (!string.IsNullOrWhiteSpace(posmeFindInterval.Value))
            {
                IntervalTime = Convert.ToInt32(posmeFindInterval.Value);
            }
            
            posmeFindPrefijo = await repositoryTbParameterSystem.PosMeFindPrefijo();
            if (!string.IsNullOrWhiteSpace(posmeFindPrefijo.Value))
            {
                Prefijo = posmeFindPrefijo.Value;
            }

            posmeFindCantidadCopias = await repositoryTbParameterSystem.PosMeFindCantidadCopias();
            if (!string.IsNullOrWhiteSpace(posmeFindCantidadCopias.Value))
            {
                CantidadCopias = Convert.ToInt32(posmeFindCantidadCopias.Value);
            }
            posmeFindShowNotificationScan = await repositoryTbParameterSystem.PosMeFindShowNotificationScan();
            if (!string.IsNullOrWhiteSpace(posmeFindShowNotificationScan.Value))
            {
                ShowNotification = posmeFindShowNotificationScan.Value == "1";
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    private bool Validate()
    {
        
        PrinterHasError = string.IsNullOrWhiteSpace(Printer);
        return !( PrinterHasError);
    }

    private bool _showNotification;

    public bool ShowNotification
    {
        get => _showNotification; 
        set => SetProperty(ref _showNotification,value);
    }
    
    private bool _printerhasError;

    public bool PrinterHasError
    {
        get => _printerhasError;
        set => SetProperty(ref _printerhasError, value);
    }

    private bool _prefijoHasError;

    public bool PrefijoHasError
    {
        get => _prefijoHasError;
        set => SetProperty(ref _prefijoHasError, value);
    }

    private void OnSaveParameters(object obj)
    {
        try
        {
            if (Validate())
            {               
                posmeFindPrinter.Value        = Printer;
                posmeFindInterval.Value       = $"{IntervalTime}";
                posmeFindPrefijo.Value        = Prefijo;
                posmeFindCantidadCopias.Value = $"{CantidadCopias}";
                posmeFindShowNotificationScan.Value = ShowNotification ? "1" : "0";
                repositoryTbParameterSystem.PosMeUpdate(posmeFindPrinter);
                repositoryTbParameterSystem.PosMeUpdate(posmeFindInterval);
                repositoryTbParameterSystem.PosMeUpdate(posmeFindPrefijo);
                repositoryTbParameterSystem.PosMeUpdate(posmeFindCantidadCopias);
                repositoryTbParameterSystem.PosMeUpdate(posmeFindShowNotificationScan);

                Mensaje                     = Mensajes.MensajeParametrosGuardar;
                PopupBackgroundColor        = Colors.Green;
                LoadValuesDefault();
            }
            else
            {
                PopupBackgroundColor    = Colors.Red;
                Mensaje                 = Mensajes.MensajeEspecificarDatosGuardar;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            PopupBackgroundColor    = Colors.Red;
            Mensaje                 = ex.Message;
        }

        PopUpShow = true;
    }

    private int _cantidadCopias;

    public int CantidadCopias
    {
        get =>_cantidadCopias; 
        set =>SetProperty(ref _cantidadCopias, value);
    }
    private int _intervalTime;

    public int IntervalTime
    {
        get => _intervalTime;
        set => SetProperty(ref _intervalTime, value);
    }
    
    private string? _printer;

    public string? Printer
    {
        get => _printer;
        set => SetProperty(ref _printer, value);
    }

    private string? _prefijo;

    public string? Prefijo
    {
        get=>_prefijo; 
        set=>SetProperty(ref _prefijo, value);
    }
    
    private bool _isRefreshing;

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }
}