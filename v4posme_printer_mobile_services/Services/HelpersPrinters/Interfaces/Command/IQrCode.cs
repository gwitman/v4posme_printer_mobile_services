using v4posme_printer_mobile_services.Services.HelpersPrinters.Enums;

namespace v4posme_printer_mobile_services.Services.HelpersPrinters.Interfaces.Command
{
    internal interface IQrCode
    {
        byte[] Print(string qrData);
        byte[] Print(string qrData, QrCodeSize qrCodeSize);
    }
}

