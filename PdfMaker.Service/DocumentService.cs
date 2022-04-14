using Microsoft.AspNetCore.Http;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using PdfSharpCore.Drawing;
using SixLabors.ImageSharp.PixelFormats;

namespace PdfMaker.Service
{
    public class DocumentService
    {
        public void AddPicturesToPage(IFormFile? file, XGraphics gfx, int width, int height, int left, int top)
        {
            if (file != null && file.Length > 0 && file.ContentType == "image/png")
            {
                var ms = new MemoryStream();
                file.CopyTo(ms);
                ms.Position = 0;
                ImageSource.ImageSourceImpl = new PdfSharpCore.Utils.ImageSharpImageSource<Rgba32>();
                var image = XImage.FromImageSource(ImageSource.FromStream(Guid.NewGuid().ToString("n").Substring(0, 8), () => ms));
                gfx.DrawImage(image, left, top, width, height);
            }
        }

        public void AddTextToPage(string? text, XGraphics gfx, XFontStyle style, int left, int top)
        {
            var font = new XFont("Verdana", 16, style);
            gfx.DrawString(text ?? "", font, XBrushes.Black, new XRect(left, top, 25, 100), XStringFormats.CenterLeft);
        }

        public void AddFooter(XGraphics gfx)
        {
            var font = new XFont("Verdana", 16, XFontStyle.Italic);
            var box = new XRect(0, gfx.PageSize.Height - 50, gfx.PageSize.Width, 50);
            var text = $"COBOD International A/S - { DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
            gfx.DrawString(text, font, XBrushes.Blue, box, XStringFormats.TopCenter);
        }

    }
}