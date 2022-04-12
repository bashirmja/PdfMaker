namespace PdfMaker.Api
{
    public class ConfigModel
    {
        public string? Title { get; set; }
        public int TotalFloors { get; set; }
        public IFormFile? TopView { get; set; }
        public IFormFile? LeftView { get; set; }
        public IFormFile? FrontView { get; set; }
    }
}
