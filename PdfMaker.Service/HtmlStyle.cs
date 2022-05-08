namespace PdfMaker.Service
{
    public class HtmlStyle
    {
        public string? Title { get; set; }
        public string? FontFamily { get; set; }
        public double FontSize { get; set; }
        public string FontColor { get; set; }
        public bool FontIsBold { get; set; }
        public bool FontIsItalic { get; set; }
        public double ParagraphLineSpacing { get; set; }
        public double ParagraphSpaceBefor { get; set; }
        public double ParagraphSpaceAfter { get; set; }

    }

    public enum StyleTitles
    {
        Normal,
        Footer,
        Header,
        Heading1,
        Heading2,
        Heading3,
        Heading4,
        Heading5,
        Heading6,
        Hyperlink,

    }
}
