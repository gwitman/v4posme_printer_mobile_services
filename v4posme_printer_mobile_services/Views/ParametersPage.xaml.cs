using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using v4posme_printer_mobile_services.ViewModels;

namespace v4posme_printer_mobile_services.Views;

public partial class ParametersPage : ContentPage
{
    private readonly PosMeParameterViewModel viewModel;
    
    public ParametersPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new PosMeParameterViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing(Navigation);
    }
    
    private void RefreshView_OnRefreshing(object? sender, EventArgs e)
    {
        OnAppearing();
    }

    private void ClosePopup_Clicked(object? sender, EventArgs e)
    {
        Popup.IsOpen = false;
    }
}