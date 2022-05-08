using Microsoft.AspNetCore.Http;

namespace PdfMaker.Service
{
    public class CreateModel
    {
        public string? HeaderHtml { get; set; }
        public IFormFile? HeaderImage { get; set; }

        public string? BodyHtml { get; set; }
        public IFormFile[]? BodyImages { get; set; }

        public string? FooterHtml { get; set; }
        public IFormFile? FooterImage { get; set; }

        public HtmlStyle[]? HtmlStyles { get; set; }
        public string? ContactInfo { get; set; }
    }
}
