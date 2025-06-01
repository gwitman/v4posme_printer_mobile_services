﻿using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Maui.Controls;
using v4posme_printer_mobile_services.ViewModels;

namespace v4posme_printer_mobile_services.Services
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null, false);
        }

        public async Task NavigateToAsync<TViewModel>(bool isAbsoluteRoute) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null, isAbsoluteRoute);
        }

        public async Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        async Task InternalNavigateToAsync(Type viewModelType, object? parameter, bool isAbsoluteRoute = false)
        {
            var viewName = viewModelType.FullName!.Replace("ViewModels", "Views").Replace("ViewModel", "Page");
            var absolutePrefix = isAbsoluteRoute ? "///" : string.Empty;
            if (parameter != null)
            {
                await Shell.Current.GoToAsync($"{absolutePrefix}{viewName}?id={HttpUtility.UrlEncode(parameter.ToString())}");
            }
            else
            {
                await Shell.Current.GoToAsync($"{absolutePrefix}{viewName}");
            }
        }
    }
}