using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.ImageSharp.PixelFormats;
using System.Globalization;

namespace PdfMaker.Service
{
    public class CreatePdf
    {
        public MemoryStream CreatePdfFile(string text, List<MemoryStream> pics)
        {
            PdfDocument document = CreateDocument();
            var page = document.AddPage();
            page.Orientation = PageOrientation.Landscape;
            var gfx = XGraphics.FromPdfPage(page);


            for (int i = 0; i < pics.Count; i++)
            {
                AddPicturesToPage(pics[i], gfx, (i + 1) * 150);
            }

            AddTextToPage(text, gfx);


            return SaveNewdocument(document);
        }

        private MemoryStream SaveNewdocument(PdfDocument document)
        {
            var stream = new MemoryStream();
            document.Save(stream, false);
            return stream;
        }

        private PdfDocument CreateDocument()
        {
            var document = new PdfDocument();
            document.Info.Title = "PDFsharp Demo";
            document.Info.Author = "COBOD";
            document.Info.Subject = "Server time: " + document.Info.CreationDate.ToString("F", CultureInfo.InvariantCulture);
            return document;
        }

        private void AddTextToPage(string test, XGraphics gfx)
        {
            var font = new XFont("Verdana", 20, XFontStyle.Bold);
            gfx.DrawString(test, font, XBrushes.Black, new XRect(0, 0, 500, 1000), XStringFormats.Center);
        }

        private void AddPicturesToPage(MemoryStream pic, XGraphics gfx, int left)
        {
            ImageSource.ImageSourceImpl = new PdfSharpCore.Utils.ImageSharpImageSource<Rgba32>();
            var image = XImage.FromImageSource(ImageSource.FromStream(Guid.NewGuid().ToString("n").Substring(0, 8), () => pic));
            gfx.DrawImage(image, left, 100, 100, 100);
        }
    }
}