using Microsoft.AspNetCore.Http;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Extensions.Html;

namespace PdfMaker.Service
{
    public class ParagraphService
    {
        public void AddImageToParagraphHelper(IFormFile? image, Paragraph paragraph, int spaceAfter = 0)
        {
            if (image != null && image.Length > 0)
            {
                var ms = new MemoryStream();
                image.CopyTo(ms);
                ms.Position = 0;
                var readyImage = "base64:" + Convert.ToBase64String(ms.ToArray());
                paragraph.AddImage(readyImage);
                paragraph.AddText(new string(' ', spaceAfter));
            }
        }

        public void AddHtmlToParagraphHelper(string? html, Paragraph paragraph)
        {
            if (!string.IsNullOrEmpty(html))
            {
                paragraph.AddHtml(html);
            }
        }

    }
}