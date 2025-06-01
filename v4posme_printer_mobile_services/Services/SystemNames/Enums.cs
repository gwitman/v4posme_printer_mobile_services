﻿namespace v4posme_printer_mobile_services.Services.SystemNames
{
    public enum TypeTransaction
    {
        TransactionInvoiceBilling   = 19,
        TransactionShare            = 23,
        TransactionQueryMedical     = 35
    }

    public enum TypeQueryMedical
    {
        Entrada         = 518,
        Salida          = 519,
        ConsultaMedica  = 716,
        Visita          = 739
    }


    public enum TypePeriodPay
    {
        Mensual         = 190,
        Quincenal       = 189,
        Semanal         = 188,
        Diario          = 531,
        Catorcenal      = 2322,
        MesYMedio       = 203,
    }
    public enum TypeCurrency
    {
        Cordoba     = 1,
        Dolar       = 2,
    }
    public enum TypeTransactionCausal {
        Contado = 21,
        Credito = 22
    }
    public enum  TypeComponent
    {
        Itme        = 33,
        Customer    = 36
    }

    public enum TypePayment
    {
        TarjetaDebito=1,
        TarjetaCredito,
        Efectivo,
        Cheque,
        Monedero,
        Otros
    }
}
