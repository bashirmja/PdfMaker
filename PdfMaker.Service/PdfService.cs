

using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace PdfMaker.Service
{
    public class PdfService
    {
        private readonly DocumentService _documentService;
        public PdfService(DocumentService documentService)
        {
            _documentService = documentService;
        }

        public async Task<byte[]?> GetPdfAsync(string path, string fileName)
        {
            var filePath = $@"{path}/{fileName}.pdf";

            return File.Exists(filePath)
                ? await File.ReadAllBytesAsync(filePath)
                : null;
        }

        public Document CreatePdf(CreateModel model)
        {
            var document = _documentService.CreateDocument();
            var section = _documentService.AddSection(document);
            _documentService.AddHeader(section, model.HeaderHtml,model.HeaderImage);
            _documentService.AddBody(section, model.BodyHtml,model.BodyImages);
            _documentService.AddFooter(section, model.FooterHtml, model.FooterImage);
            return document;
        }

        public void SavePdf(Document document, string path, string fileName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Directory.CreateDirectory(path);

            var pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save($@"{path}/{fileName}.pdf");
        }

    }
}