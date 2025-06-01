using SkiaSharp;

namespace v4posme_printer_mobile_services.Services.HelpersPrinters.Interfaces.Command
{
    internal interface IImage
    {
        byte[] Print(SKBitmap image);
    }
}
