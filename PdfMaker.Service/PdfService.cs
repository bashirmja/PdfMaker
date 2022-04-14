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


            _documentService.AddPicturesToPage(model.FrontView, gfx, 100);
            _documentService.AddPicturesToPage(model.LeftView, gfx, 300);
            _documentService.AddPicturesToPage(model.TopView, gfx, 500);

            _documentService.AddTextToPage(model.ProductTitle, gfx);

            return SaveDocument(document);
        }

        private XGraphics GetGfxFromDocument(PdfDocument document)
        {
            var page = document.AddPage();
            page.Orientation = PageOrientation.Landscape;
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