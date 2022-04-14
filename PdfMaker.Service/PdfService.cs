using Microsoft.AspNetCore.StaticFiles;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Globalization;

namespace PdfMaker.Service
{
    public class PdfService
    {
        private readonly DocumentService _documentService;
        public PdfService(DocumentService documentService)
        {
            _documentService = documentService;
        }

        public async Task<PdfDto?> GetPdfAsync(string id)
        {
            var filePath = $@".\pdfs\{id}.pdf";

            if (!File.Exists(filePath))
            {
                return null;
            }
            else
            {
                if (!new FileExtensionContentTypeProvider().TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                var content = await File.ReadAllBytesAsync(filePath);
                return PdfDto.CreateInstance(content, contentType, filePath);
            }
        }

        public string CreatePdf(ConfigModel model)
        {
            var document = CreateDocument();
            var gfx = GetGfxFromDocument(document);

            _documentService.AddPicturesToPage(model.CompanyLogo, gfx, 100, 50, 450, 25);
            _documentService.AddTextToPage(model.ProductTitle, gfx, XFontStyle.Bold, 200, 0);
            _documentService.AddTextToPage(model.ConfigTitle, gfx, XFontStyle.Regular, 250, 25);

            var basePictures = 100;
            _documentService.AddPicturesToPage(model.FrontView, gfx, 150, 150, 50, basePictures);
            _documentService.AddPicturesToPage(model.LeftView, gfx, 150, 150, 225, basePictures);
            _documentService.AddPicturesToPage(model.TopView, gfx, 150, 150, 400, basePictures);

            var baseWidth = 250;
            _documentService.AddTextToPage(model.WidthTitle, gfx, XFontStyle.Bold, 50, baseWidth);
            _documentService.AddTextToPage(model.WidthModules, gfx, XFontStyle.Regular, 150, baseWidth);
            _documentService.AddTextToPage(model.WidthSize, gfx, XFontStyle.Regular, 250, baseWidth);

            var baseLenght = 275;
            _documentService.AddTextToPage(model.LenghtTitle, gfx, XFontStyle.Bold, 50, baseLenght);
            _documentService.AddTextToPage(model.LenghtModules, gfx, XFontStyle.Regular, 150, baseLenght);
            _documentService.AddTextToPage(model.LenghtSize, gfx, XFontStyle.Regular, 250, baseLenght);

            var baseheight = 300;
            _documentService.AddTextToPage(model.HeightTitle, gfx, XFontStyle.Bold, 50, baseheight);
            _documentService.AddTextToPage(model.HeightModules, gfx, XFontStyle.Regular, 150, baseheight);
            _documentService.AddTextToPage(model.HeightSize, gfx, XFontStyle.Regular, 250, baseheight);

            var floorCount = 350;
            _documentService.AddTextToPage(model.FloorCount, gfx, XFontStyle.Regular, 50, floorCount);

            var baseFloor = 375;
            _documentService.AddTextToPage(model.FloorAreaTitle, gfx, XFontStyle.Bold, 50, baseFloor);
            _documentService.AddTextToPage(model.FloorAreaSize, gfx, XFontStyle.Regular, 150, baseFloor);

            var baseTotal = 400;
            _documentService.AddTextToPage(model.TotalAreaTitle, gfx, XFontStyle.Bold, 50, baseTotal);
            _documentService.AddTextToPage(model.TotalAreaSize, gfx, XFontStyle.Regular, 150, baseTotal);


            return SaveDocument(document);
        }

        private XGraphics GetGfxFromDocument(PdfDocument document)
        {
            var page = document.AddPage();
            page.Orientation = PageOrientation.Portrait;
            return XGraphics.FromPdfPage(page);
        }

        private string SaveDocument(PdfDocument document)
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
    }
}