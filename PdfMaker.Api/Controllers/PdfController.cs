using Microsoft.AspNetCore.Mvc;
using PdfMaker.Service;

namespace PdfMaker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly ILogger<PdfController> _logger;
        public PdfController(ILogger<PdfController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreatePdf")]
        public IActionResult Post(IFormFile[] files)
        {
            _logger.Log(LogLevel.Information, "==>action called!");
            foreach (var item in files)
            {
                _logger.LogInformation("file uploaded : " + item.FileName);
            }
            var stream = CreatePdf.CreatePdfFile("Hello, World!");

            return File(stream, "application/octet-stream", "ConfigatorSettings.pdf");
        }
    }
}