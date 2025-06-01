using System;
using System.Collections.Generic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using SQLite;
using v4posme_maui.Models;
using v4posme_printer_mobile_services.Services.SystemNames;

namespace v4posme_printer_mobile_services.Services;

public class DataBase
{
    public readonly SQLiteAsyncConnection Database;

    public DataBase()
    {
        Database ??= new SQLiteAsyncConnection(ConstantsSqlite.DatabasePath, ConstantsSqlite.Flags);
    }
 public async void Init()
    {
        try
        {
            await Database.CreateTableAsync<TbParameterSystem>();
            var countParameters = await Database.Table<TbParameterSystem>().CountAsync();
            if (countParameters == 0)
            {
                var parametrosDefault = new List<TbParameterSystem>
                {
                    new() { Name = Constantes.ParametroInterval, Description = "Intervalo del servicio", Value = "2000" },
                    new() { Name = Constantes.ParametroLogo, Description = "Logo de la aplicación", Value = "" },
                    new() { Name = Constantes.ParametroPrinter, Description = "Impresora", Value = "Printer" },
                    new() { Name = Constantes.ParametroPrefijo, Description = "Prefijo de archivo", Value = "posme_" },
                    new() { Name = Constantes.ParameterCodigoFactura, Description = "Número de factura", Value = "FAC-0001" },
                    new() { Name = Constantes.ParemeterEntityIDAutoIncrement, Description = "Auto incrementado", Value = "-1" },
                    new() { Name = Constantes.ParameterCodigoVisita, Description = "Número de visita", Value = "VST-0001" },
                    new() { Name = Constantes.CustomerOrderShare, Description = "Orden de clientes abonos", Value = "" },
                    new() { Name = Constantes.CustomerOrderCustomer, Description = "Orden de clientes en pantalla cliente", Value = "" },
                    new() { Name = Constantes.CustomerOrderInvoice, Description = "Orden de clientes en pantalla invoice", Value = "" },
                };
                await Database.InsertAllAsync(parametrosDefault);
            }
        }
        catch (Exception e)
        {
            await Toast.Make(e.Message, ToastDuration.Long).Show();
        }
    }
}