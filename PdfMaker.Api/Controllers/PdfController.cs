using Microsoft.AspNetCore.Mvc;
using PdfMaker.Service;

namespace PdfMaker.Api.Controllers
{
    [ApiController]
    [Route("/pdf")]
    public class PdfController : ControllerBase
    {
        private readonly ILogger<PdfController> _logger;
        private readonly PdfService _pdfService;
        private const string PdfStorageFolder = @"/pdfs";

        public PdfController(ILogger<PdfController> logger, PdfService pdfService)
        {
            logger.LogInformation("==> PdfController called!");

            _logger = logger;
            _pdfService = pdfService;
        }

        [HttpPost("", Name = "CreatePdf")]
        public IActionResult CreatePdf([FromForm] ConfigModel model)
        {
            _logger.Log(LogLevel.Information, "==> CreatePdf called!");

            var document = _pdfService.CreatePdf(model);

            var fileName = Guid.NewGuid().ToString();

            _pdfService.SavePdf(document, PdfStorageFolder, fileName);

            return Ok(fileName);
        }

        [HttpGet("{id}", Name = "GetPdf")]
        public async Task<IActionResult> GetPdfAsync(string id)
        {
            _logger.Log(LogLevel.Information, "==> GetPdf called!");

            var pdf = await _pdfService.GetPdfAsync(PdfStorageFolder, id);

            return pdf == null
                ? NotFound()
                : File(pdf, "application/pdf");
        }

    }
}