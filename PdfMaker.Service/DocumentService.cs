using Microsoft.AspNetCore.Http;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Extensions.Html;

namespace PdfMaker.Service
{
    public class DocumentService
    {
        public Document CreateDocument(string? title = "", string? author = "", string? subject = "")
        {
            var document = new Document();
            document.Info.Title = title;
            document.Info.Author = author;
            document.Info.Subject = subject;
            return document;
        }
        public Section AddSection(Document document)
        {
            var section = document.AddSection();
            return section;
        }
        public void AddHeader(Section section, string? headerHtmlContent)
        {
            section.Headers.Primary.AddParagraph().AddHtml(headerHtmlContent);
        }

        public void AddPictures(Section section, IFormFile[]? formPictures)
        {
            var paragraph = section.AddParagraph();
            foreach (var picture in formPictures)
            {
                if (picture != null && picture.Length > 0 && picture.ContentType == "image/png")
                {
                    var ms = new MemoryStream();
                    picture.CopyTo(ms);
                    ms.Position = 0;
                    paragraph.AddImage("base64:" + Convert.ToBase64String(ms.ToArray()));
                }
            }

        }

        public void AddBody(Section section, string? bodyHtmlContent)
        {
            section.AddParagraph().AddHtml(bodyHtmlContent);
        }

        public void AddFooter(Section section, string? footerHtmlContent)
        {
            section.Footers.Primary.AddParagraph().AddHtml(footerHtmlContent);
        }
                
    }
}