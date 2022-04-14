using Microsoft.AspNetCore.Mvc;
using PdfMaker.Service;

namespace PdfMaker.Api.Controllers
{
    [ApiController]
    [Route("/pdf")]
    public class PdfController : ControllerBase
    {
        private readonly ILogger<PdfController> _logger;
        public PdfController(ILogger<PdfController> logger)
        {
            _logger = logger;
            _logger.LogInformation("==> Request hits pdf constractor!");
        }

        [HttpPost("", Name = "CreatePdf")]
        public IActionResult CreatePdf([FromForm] ConfigModel model)
        {
            _logger.Log(LogLevel.Information, "==> CreatePdf called!");


            var fileId = new PdfService().CreatePdf(model);

            _logger.Log(LogLevel.Information, $"==> Created fileId:{fileId}");
            return Ok(fileId);
        }

        [HttpGet("{id}", Name = "GetPdf")]
        public async Task<IActionResult> GetPdfAsync(string id)
        {
            _logger.Log(LogLevel.Information, "==> GetPdf called!");

            var file = await new PdfService().GetPdfAsync(id);

            if (file != null)
            {
                _logger.Log(LogLevel.Information, $"==> file {id} exist");
                return File(file.FileContent, file.FileContentType, Path.GetFileName(file.FilePath));
            }
            else
            {
                _logger.Log(LogLevel.Information, $"==> file {id} not found");
                return NotFound();
            }
        }

    }
}