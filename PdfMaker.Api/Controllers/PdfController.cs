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

            var fileId = _pdfService.CreatePdf(model);
            return Ok(fileId);
        }

        [HttpGet("{id}", Name = "GetPdf")]
        public async Task<IActionResult> GetPdfAsync(string id)
        {
            _logger.Log(LogLevel.Information, "==> GetPdf called!");

            var pdf = await _pdfService.GetPdfAsync(id);
            if (pdf == null)
            {
                return NotFound();
            }
            else
            {
                return File(pdf, "application/pdf");
            }
        }

    }
}