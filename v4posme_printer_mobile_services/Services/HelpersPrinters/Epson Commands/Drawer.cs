using v4posme_printer_mobile_services.Services.HelpersPrinters.Interfaces.Command;

namespace v4posme_printer_mobile_services.Services.HelpersPrinters.Epson_Commands
{
    public class Drawer : IDrawer
    {
        public byte[] Open()
        {
            return new byte[] { 27, 112, 0, 60, 120 };
        }
    }
}

