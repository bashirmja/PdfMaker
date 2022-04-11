using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace PdfMaker.Service
{
    public class CreatePdf
    {
        public CreatePdf()
        {
            var document = new PdfDocument();

            var page = document.AddPage();

            var gfx = XGraphics.FromPdfPage(page);

            var font = new XFont("Verdana", 20, XFontStyle.Bold);

            gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
        }
    }
}