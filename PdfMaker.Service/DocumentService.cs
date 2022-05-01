using Microsoft.AspNetCore.Http;
using MigraDoc.DocumentObjectModel;
using System.Text.RegularExpressions;

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
            var paragraph = section.AddParagraph();
            ParsHtmlStringToAddToParagraph(headerHtmlContent ?? "", paragraph);

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
            var paragraph = section.AddParagraph();
            ParsHtmlStringToAddToParagraph(bodyHtmlContent ?? "", paragraph);
        }

        public void AddFooter(Section section, string? footerHtmlContent)
        {
            var paragraph = section.AddParagraph();
            ParsHtmlStringToAddToParagraph(footerHtmlContent ?? "", paragraph);
        }


        private void ParsHtmlStringToAddToParagraph(string htmlstring, Paragraph paragraph)
        {

            var lines = htmlstring.Split("<br/>");

            foreach (var line in lines)
            {

                var matches = Regex.Matches(line, @"(<\w+>.*?</\w+>)");

                foreach (Match match in matches)
                {
                    paragraph.AddText(match.Index + ")" + match.Value);
                    paragraph.AddLineBreak();
                }

                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
            }

        }
    }
}