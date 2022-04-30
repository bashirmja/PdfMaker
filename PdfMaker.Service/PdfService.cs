using PdfSharpCore.Pdf;

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

        public PdfDocument CreatePdf(ConfigModel model)
        {
            var document = _documentService.CreateDocument();
            _documentService.FillDocument(document, model);
            return document;
        }

        public void SavePdf(PdfDocument document, string path, string fileName)
        {
            Directory.CreateDirectory(path);
            document.Save($@"{path}/{fileName}.pdf");
        }

    }
}