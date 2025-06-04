using Android.Graphics.Pdf;
using Android.OS;
using System;
using System.IO;
using Android.Content;
using Java.IO;
using Microsoft.Maui.Controls.PlatformConfiguration;
using v4posme_printer_mobile_services.Services.SystemNames;
using Debug = System.Diagnostics.Debug;

namespace v4posme_printer_mobile_services;

public static class PdfRendererHelper
{
    public static string[] ConvertPdfToBase64(Java.IO.File file, int dpi = 300)
    {
        try
        {
            var parcelFileDescriptor = ParcelFileDescriptor.Open(file, ParcelFileMode.ReadOnly);
            if (parcelFileDescriptor is null)
            {
                throw new Exception(Constantes.ErrorArchivo);
            }
            using var renderer  = new PdfRenderer(parcelFileDescriptor);
            var base64Images    = new string[renderer.PageCount];

            for (var i = 0; i < renderer.PageCount; i++)
            {
                using var page = renderer.OpenPage(i);

                var width   = page.Width * dpi / 72;
                var height  = page.Height * dpi / 72;

                using var bitmap    = Android.Graphics.Bitmap.CreateBitmap(width, height, Android.Graphics.Bitmap.Config.Argb8888);
                using (var canvas   = new Android.Graphics.Canvas(bitmap))
                {
                    canvas.DrawColor(Android.Graphics.Color.White);
                    var matrix = new Android.Graphics.Matrix();
                    matrix.PostScale(width / (float)page.Width, height / (float)page.Height);
                    page.Render(bitmap, null, matrix, PdfRenderMode.ForDisplay);
                }

                using var stream = new MemoryStream();
                bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, stream);
                base64Images[i] = Convert.ToBase64String(stream.ToArray());

                bitmap.Recycle();
            }
            renderer.Close();
            return base64Images;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return [];
        }
    }
}
