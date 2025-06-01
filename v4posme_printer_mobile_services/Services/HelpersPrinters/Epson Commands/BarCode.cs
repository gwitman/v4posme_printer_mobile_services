using v4posme_printer_mobile_services.Services.HelpersPrinters.Enums;
using v4posme_printer_mobile_services.Services.HelpersPrinters.Extensions;
using v4posme_printer_mobile_services.Services.HelpersPrinters.Interfaces.Command;
using PrinterExtensions = v4posme_printer_mobile_services.Services.HelpersPrinters.Extensions.PrinterExtensions;

namespace v4posme_printer_mobile_services.Services.HelpersPrinters.Epson_Commands
{
    public class BarCode : IBarCode
    {
        public byte[] Code128(string code, Positions printString = Positions.NotPrint)
        {
            return PrinterExtensions.AddLF(PrinterExtensions.AddBytes(
                PrinterExtensions.AddBytes(
                    PrinterExtensions.AddBytes(
                        PrinterExtensions.AddBytes(
                            PrinterExtensions.AddBytes(
                                PrinterExtensions.AddBytes(
                                    PrinterExtensions.AddBytes(new byte[] { 29, 119, 2 }, new byte[] { 29, 104, 100 }),
                                    new byte[] { 29, 102, 1 }), new byte[] { 29, 72, printString.ToByte() }),
                            new byte[] { 29, 107, 73 }), new[] { (byte)(code.Length + 2) }),
                    new[] { PrinterExtensions.ToByte('{'), PrinterExtensions.ToByte('C') }), code));
        }

        public byte[] Code128V2(byte[] code, Positions printString = Positions.NotPrint)
        {
            return PrinterExtensions.AddLF(PrinterExtensions.AddBytes(
                PrinterExtensions.AddBytes(
                    PrinterExtensions.AddBytes(
                        PrinterExtensions.AddBytes(
                            PrinterExtensions.AddBytes(
                                PrinterExtensions.AddBytes(
                                    PrinterExtensions.AddBytes(new byte[] { 29, 119, 2 }, new byte[] { 29, 104, 100 }),
                                    new byte[] { 29, 102, 1 }), new byte[] { 29, 72, printString.ToByte() }),
                            new byte[] { 29, 107, 73 }), new[] { (byte)(code.Length + 2) }),
                    new[] { PrinterExtensions.ToByte('{'), PrinterExtensions.ToByte('C') }), code));
        }

        public byte[] Code39(string code, Positions printString = Positions.NotPrint)
        {
            return PrinterExtensions.AddLF(PrinterExtensions.AddBytes(
                PrinterExtensions.AddBytes(
                    PrinterExtensions.AddBytes(
                        PrinterExtensions.AddBytes(
                            PrinterExtensions.AddBytes(
                                PrinterExtensions.AddBytes(new byte[] { 29, 119, 2 }, new byte[] { 29, 104, 50 }),
                                new byte[] { 29, 102, 0 }), new byte[] { 29, 72, printString.ToByte() }),
                        new byte[] { 29, 107, 4 }), code), new byte[] { 0 }));
        }

        public byte[] Code39CustomPosMe2px1p(string code, Positions printString = Positions.NotPrint)
        {
            return PrinterExtensions.AddLF(PrinterExtensions.AddBytes(
                PrinterExtensions.AddBytes(
                    PrinterExtensions.AddBytes(
                        PrinterExtensions.AddBytes(
                            PrinterExtensions.AddBytes(
                                PrinterExtensions.AddBytes(new byte[] { 29, 119, 2 }, new byte[] { 29, 104, 60 }),
                                new byte[] { 29, 102, 0 }), new byte[] { 29, 72, printString.ToByte() }),
                        new byte[] { 29, 107, 4 }), code), new byte[] { 0 }));
        }


        public byte[] Ean13(string code, Positions printString = Positions.NotPrint)
        {
            if (code.Trim().Length != 13)
                return new byte[0];

            return PrinterExtensions.AddLF(PrinterExtensions.AddBytes(
                PrinterExtensions.AddBytes(
                    PrinterExtensions.AddBytes(
                        PrinterExtensions.AddBytes(new byte[] { 29, 119, 2 }, new byte[] { 29, 104, 50 }),
                        new byte[] { 29, 72, printString.ToByte() }), new byte[] { 29, 107, 67, 12 }),
                code.Substring(0, 12)));
        }
    }
}