namespace v4posme_printer_mobile_services.Services.HelpersPrinters.Interfaces.Command
{
    internal interface IAlignment
    {
        byte[] Left();
        byte[] Right();
        byte[] Center();
        byte[] Avanza(int puntos);
    }
}

