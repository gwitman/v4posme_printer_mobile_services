namespace v4posme_printer_mobile_services.Services.HelpersPrinters.Interfaces.Command
{
    internal interface IPaperCut
    {
        byte[] Full();
        byte[] Partial();
    }
}

