using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.ImageSharp.PixelFormats;
using System.Globalization;

namespace PdfMaker.Service
{
    public class CreatePdf
    {
        public static MemoryStream CreatePdfFile(string text, List<MemoryStream> pics)
        {
            PdfDocument document = CreateDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);


            if (pics.Any()) AddPicturesToPage(pics.First(), gfx);

            AddTextToPage(text, gfx);


            return SaveNewdocument(document);
        }

        private static MemoryStream SaveNewdocument(PdfDocument document)
        {
            var stream = new MemoryStream();
            document.Save(stream, false);
            return stream;
        }

        private static PdfDocument CreateDocument()
        {
            var document = new PdfDocument();
            document.Info.Title = "PDFsharp Demo";
            document.Info.Author = "COBOD";
            document.Info.Subject = "Server time: " + document.Info.CreationDate.ToString("F", CultureInfo.InvariantCulture);
            return document;
        }

        private static void AddTextToPage(string test, XGraphics gfx)
        {
            var font = new XFont("Verdana", 20, XFontStyle.Bold);
            gfx.DrawString(test, font, XBrushes.Black, new XRect(0, 0, 500, 1000), XStringFormats.Center);
        }

        private static void AddPicturesToPage(MemoryStream pic, XGraphics gfx)
        {
            ImageSource.ImageSourceImpl = new PdfSharpCore.Utils.ImageSharpImageSource<Rgba32>();
            var image = XImage.FromImageSource(ImageSource.FromStream("", () => pic));
            gfx.DrawImage(image, 0, 0, 100, 100);
        }
    }
}