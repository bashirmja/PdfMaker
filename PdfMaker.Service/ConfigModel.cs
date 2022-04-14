using Microsoft.AspNetCore.Http;

namespace PdfMaker.Service
{
    public class ConfigModel
    {
        public IFormFile? TopView { get; set; }
        public IFormFile? LeftView { get; set; }
        public IFormFile? FrontView { get; set; }
        public IFormFile? CompanyLogo { get; set; }

        public string? ProductTitle { get; set; }
        public string? ConfigTitle { get; set; }

        public string? WidthTitle { get; set; }
        public string? WidthModules { get; set; }
        public string? WidthSize { get; set; }

        public string? LenghtTitle { get; set; }
        public string? LenghtModules { get; set; }
        public string? LenghtSize { get; set; }

        public string? HeightTitle { get; set; }
        public string? HeightModules { get; set; }
        public string? HeightSize { get; set; }


        public string? FloorCount { get; set; }

        public string? FloorAreaTitle { get; set; }
        public string? FloorAreaSize { get; set; }

        public string? TotalAreaTitle { get; set; }
        public string? TotalAreaSize { get; set; }
    }
}
