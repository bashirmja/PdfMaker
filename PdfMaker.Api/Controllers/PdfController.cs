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
        public IActionResult Post(IFormFile[] uploadedfiles, string text)
        {
            _logger.Log(LogLevel.Information, "==>action called!");
            foreach (var item in uploadedfiles)
            {
                _logger.LogInformation("file uploaded : " + item.FileName);
            }

            var streamImages = new List<MemoryStream>();
            foreach (var file in uploadedfiles)
            {
                if (file.Length > 0 && file.ContentType == "image/png")
                {
                    var ms = new MemoryStream();
                    file.CopyTo(ms);
                    ms.Position = 0;
                    streamImages.Add(ms);
                }
            }

            var stream = CreatePdf.CreatePdfFile(text, streamImages);

            return File(stream, "application/octet-stream", "ConfigatorSettings.pdf");
        }
    }
}