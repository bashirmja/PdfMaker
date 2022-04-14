using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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
            _logger.LogInformation("==> Request hits constractor!");
        }

        [HttpPost("", Name = "CreatePdf")]
        public IActionResult CreatePdf([FromForm] ConfigSettings model)
        {
            _logger.Log(LogLevel.Information, "==> CreatePdf called!");


            var fileId = new CreatePdf().CreatePdfFile(model);

            _logger.Log(LogLevel.Information, $"==> fileId={fileId}");
            return Ok(fileId);
        }

        [HttpGet("{id}", Name = "GetPdf")]
        public async Task<IActionResult> GetPdfAsync(string id)
        {
            _logger.Log(LogLevel.Information, "==> ReadPdf called!");

            var provider = new FileExtensionContentTypeProvider();
            var filePath = $@".\pdfs\{id}.pdf";
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }

    }
}