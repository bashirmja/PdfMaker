using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Globalization;

namespace PdfMaker.Service
{
    public class CreatePdf
    {
        public static MemoryStream CreatePdfFile(string test)
        {
            var document = new PdfDocument();
            document.Info.Title = "PDFsharp Clock Demo";
            document.Info.Author = "COBOD";
            document.Info.Subject = "Server time: " + document.Info.CreationDate.ToString("F", CultureInfo.InvariantCulture);

            var page = document.AddPage();

            var gfx = XGraphics.FromPdfPage(page);

            var font = new XFont("Verdana", 20, XFontStyle.Bold);

            gfx.DrawString(test, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

            var stream = new MemoryStream();
            document.Save(stream, false);

            return stream;
        }
    }
}