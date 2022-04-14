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
        public int WidthModules { get; set; }
        public int WidthSize { get; set; }

        public string? LenghtTitle { get; set; }
        public int LenghtModules { get; set; }
        public int LenghtSize { get; set; }

        public string? HeightTitle { get; set; }
        public int HeightModules { get; set; }
        public int HeightSize { get; set; }


        public int FloorCount { get; set; }
        public int FloorArea { get; set; }
        public int TotalArea { get; set; }
    }
}
