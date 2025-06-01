namespace v4posme_printer_mobile_services.Services.HelpersPrinters.Interfaces.Command
{
    interface ILineHeight
    {
        byte[] Normal();
        byte[] SetLineHeight(byte height);
    }
}
