using Microsoft.AspNetCore.Http;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using PdfSharpCore.Drawing;
using SixLabors.ImageSharp.PixelFormats;

namespace PdfMaker.Service
{
    public class DocumentService
    {
        public void AddTextToPage(string test, XGraphics gfx)
        {
            var font = new XFont("Verdana", 20, XFontStyle.Bold);
            gfx.DrawString(test, font, XBrushes.Black, new XRect(0, 0, 500, 1000), XStringFormats.Center);
        }

        public void AddPicturesToPage(IFormFile? file, XGraphics gfx, int left)
        {
            if (file != null && file.Length > 0 && file.ContentType == "image/png")
            {
                var ms = new MemoryStream();
                file.CopyTo(ms);
                ms.Position = 0;
                ImageSource.ImageSourceImpl = new PdfSharpCore.Utils.ImageSharpImageSource<Rgba32>();
                var image = XImage.FromImageSource(ImageSource.FromStream(Guid.NewGuid().ToString("n").Substring(0, 8), () => ms));
                gfx.DrawImage(image, left, 100, 100, 100);
            }
        }

    }
}