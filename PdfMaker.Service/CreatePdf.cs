using Microsoft.AspNetCore.Http;
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
        public string CreatePdfFile(ConfigSettings model)
        {
            var images = GetImages(model);

            var document = CreateDocument();
            var page = document.AddPage();
            page.Orientation = PageOrientation.Landscape;
            var gfx = XGraphics.FromPdfPage(page);


            for (int i = 0; i < images.Count; i++)
            {
                AddPicturesToPage(images[i], gfx, (i + 1) * 150);
            }

            AddTextToPage(model.ProductTitle ?? "", gfx);


            return SaveNewdocument(document);
        }

        private static List<MemoryStream> GetImages(ConfigSettings model)
        {
            var uploadedfiles = new List<IFormFile>();
            if (model.TopView != null) uploadedfiles.Add(model.TopView);
            if (model.LeftView != null) uploadedfiles.Add(model.LeftView);
            if (model.FrontView != null) uploadedfiles.Add(model.FrontView);

            var images = new List<MemoryStream>();
            foreach (var file in uploadedfiles)
            {
                if (file.Length > 0 && file.ContentType == "image/png")
                {
                    var ms = new MemoryStream();
                    file.CopyTo(ms);
                    ms.Position = 0;
                    images.Add(ms);
                }
            }

            return images;
        }

        private string SaveNewdocument(PdfDocument document)
        {
            var fileId = Guid.NewGuid();
            var folderName = @".\pdfs";
            var filepath = $@"{folderName}\{fileId}.pdf";

            Directory.CreateDirectory(folderName);
            document.Save(filepath);
            return fileId.ToString();
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