using v4posme_printer_mobile_services.Services.HelpersPrinters.Enums;

namespace v4posme_printer_mobile_services.Services.HelpersPrinters.Interfaces.Command
{
    internal interface IFontMode
    {
        byte[] Bold(string value);
        byte[] Bold(PrinterModeState state);
        byte[] Underline(string value);
        byte[] Underline(PrinterModeState state);
        byte[] Expanded(string value);
        byte[] Expanded(PrinterModeState state);
        byte[] Condensed(string value);
        byte[] Condensed(PrinterModeState state);
        byte[] Font(string value, Enums.Fonts state);
        byte[] Font(Enums.Fonts state);
    }
}

