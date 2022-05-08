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
        private const string PdfStorageFolder = @"./GeneratedPdfFiles/";
        private const string ContactStorageFolder = @"./ContactInfos/";

        public PdfController
        (
            ILogger<PdfController> logger,
            PdfService pdfService
        )
        {
            logger.LogInformation("==> PdfController called!");

            _logger = logger;
            _pdfService = pdfService;
        }

        [HttpPost("", Name = "CreatePdf")]
        public IActionResult CreatePdf([FromForm] CreateModel model)
        {
            _logger.Log(LogLevel.Information, "==> CreatePdf called!");

            model.HtmlStyles = System.Text.Json.JsonSerializer.Deserialize<HtmlStyle[]>(Request.Form["HtmlStyles"]);

            var document = _pdfService.CreatePdf(model);

            var fileName = Guid.NewGuid().ToString();

            _pdfService.SavePdf(document, PdfStorageFolder, fileName + ".pdf");
            _pdfService.SaveContactInfo(model.ContactInfo ?? "", ContactStorageFolder, "contacs.txt");

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