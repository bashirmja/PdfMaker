using Microsoft.AspNetCore.Http;

namespace PdfMaker.Service
{
    public class CreateModel
    {
        public IFormFile[]? FormPictures { get; set; }
        public string? HeaderHtmlContent { get; set; }
        public string? BodyHtmlContent { get; set; }
        public string? FooterHtmlContent { get; set; }
    }
}
