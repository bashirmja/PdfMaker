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
            var paragraph = section.Headers.Primary.AddParagraph();
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
            var paragraph = section.Footers.Primary.AddParagraph();
            ParsHtmlStringToAddToParagraph(footerHtmlContent ?? "", paragraph);
        }


        private void ParsHtmlStringToAddToParagraph(string htmlstring, Paragraph paragraph)
        {

            var pTagMatches = Regex.Matches(htmlstring, @"<(p|P)>.*?</(p|P)>");
            foreach (Match pTagMatch in pTagMatches)
            {
                var pTagValue = Regex.Match(pTagMatch.Value, "(?<=<(p|P)>).*?(?=</(p|P)>)").Value;
                var lines = pTagValue.Split("<br/>");

                foreach (var line in lines)
                {

                    var boldlineEndParts = line.Split("</b>");
                    foreach (var boldlineEndPart in boldlineEndParts)
                    {
                        if (!boldlineEndPart.Contains("<b>"))
                        {
                            paragraph.AddText(boldlineEndPart);
                            continue;
                        }

                        var boldlineStartParts = boldlineEndPart.Split("<b>");

                        paragraph.AddText(boldlineStartParts[0]);
                        paragraph.AddFormattedText(boldlineStartParts[1].Replace("</b>", ""), TextFormat.Bold);
                    }
                        paragraph.AddLineBreak();
                }

            }
        }
    }
}