using Microsoft.AspNetCore.Mvc;
using PdfMaker.Service;

namespace PdfMaker.Api.Controllers
{
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly ILogger<PdfController> _logger;
        public PdfController(ILogger<PdfController> logger)
        {
            _logger = logger;
            _logger.LogInformation("==>Request hits constractor!");
        }

        [HttpPost("createpdf", Name = "CreatePdf")]
        public IActionResult CreatePost([FromForm] ConfigModel model)
        {
            _logger.Log(LogLevel.Information, "==>CreatePdf called!");

            var uploadedfiles = new List<IFormFile>();
            if (model.TopView != null) uploadedfiles.Add(model.TopView);
            if (model.LeftView != null) uploadedfiles.Add(model.LeftView);
            if (model.FrontView != null) uploadedfiles.Add(model.FrontView);

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

            var stream = new CreatePdf().CreatePdfFile(model.Title ?? "", streamImages);
            return File(stream, "application/octet-stream", "ConfigatorSettings.pdf");
        }

        [HttpGet("TestGet", Name = "TestGet")]
        public IActionResult TestGet()
        {
            _logger.Log(LogLevel.Information, "==>TestGet works");
            return Ok("GetOK");
        }

        [HttpPost("testpost", Name = "TestPost")]
        public IActionResult TestPost(ConfigModel model)
        {
            _logger.Log(LogLevel.Information, $"==>TestPost works {model.Title}");
            return Ok(model.Title);
        }

    }
}