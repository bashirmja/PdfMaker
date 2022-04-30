using Microsoft.AspNetCore.Http;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.ImageSharp.PixelFormats;
using System.Globalization;

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

        public PdfDocument CreateDocument()
        {
            var document = new PdfDocument();
            document.Info.Title = "PDFsharp Demo";
            document.Info.Author = "COBOD";
            document.Info.Subject = "Server time: " + document.Info.CreationDate.ToString("F", CultureInfo.InvariantCulture);
            return document;
        }

        public XGraphics GetGfxFromDocument(PdfDocument document)
        {
            var page = document.AddPage();
            page.Orientation = PageOrientation.Portrait;
            return XGraphics.FromPdfPage(page);
        }



        public void FillDocument(PdfDocument document, ConfigModel model)
        {
            var gfx = GetGfxFromDocument(document);

            AddPicturesToPage(model.CompanyLogo, gfx, 100, 50, 450, 25);
            AddTextToPage(model.ProductTitle, gfx, XFontStyle.Bold, 200, 0);
            AddTextToPage(model.ConfigTitle, gfx, XFontStyle.Regular, 250, 25);

            var basePictures = 100;
            AddPicturesToPage(model.FrontView, gfx, 150, 150, 50, basePictures);
            AddPicturesToPage(model.LeftView, gfx, 150, 150, 225, basePictures);
            AddPicturesToPage(model.TopView, gfx, 150, 150, 400, basePictures);

            var baseWidth = 250;
            AddTextToPage(model.WidthTitle, gfx, XFontStyle.Bold, 50, baseWidth);
            AddTextToPage(model.WidthModules, gfx, XFontStyle.Regular, 150, baseWidth);
            AddTextToPage(model.WidthSize, gfx, XFontStyle.Regular, 250, baseWidth);

            var baseLenght = 275;
            AddTextToPage(model.LenghtTitle, gfx, XFontStyle.Bold, 50, baseLenght);
            AddTextToPage(model.LenghtModules, gfx, XFontStyle.Regular, 150, baseLenght);
            AddTextToPage(model.LenghtSize, gfx, XFontStyle.Regular, 250, baseLenght);

            var baseheight = 300;
            AddTextToPage(model.HeightTitle, gfx, XFontStyle.Bold, 50, baseheight);
            AddTextToPage(model.HeightModules, gfx, XFontStyle.Regular, 150, baseheight);
            AddTextToPage(model.HeightSize, gfx, XFontStyle.Regular, 250, baseheight);

            var floorCount = 350;
            AddTextToPage(model.FloorCount, gfx, XFontStyle.Regular, 50, floorCount);

            var baseFloor = 375;
            AddTextToPage(model.FloorAreaTitle, gfx, XFontStyle.Bold, 50, baseFloor);
            AddTextToPage(model.FloorAreaSize, gfx, XFontStyle.Regular, 150, baseFloor);

            var baseTotal = 400;
            AddTextToPage(model.TotalAreaTitle, gfx, XFontStyle.Bold, 50, baseTotal);
            AddTextToPage(model.TotalAreaSize, gfx, XFontStyle.Regular, 150, baseTotal);
            AddFooter(gfx);
        }

    }
}