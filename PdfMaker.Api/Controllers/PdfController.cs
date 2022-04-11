using Microsoft.AspNetCore.Mvc;

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
        public void Post()
        {
            _logger.Log(LogLevel.Information, "--action called!");

        }
    }
}