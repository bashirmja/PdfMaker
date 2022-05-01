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

        public void AddHeader(Section section, string? html, IFormFile? image)
        {
            var imageParageraph = section.Headers.Primary.AddParagraph();
            imageParageraph.Format.Alignment = ParagraphAlignment.Right;

            var htmlParagheraph = section.AddParagraph();
            htmlParagheraph.Format.Alignment = ParagraphAlignment.Center;

            AddImageToParagraphHelper(image, imageParageraph);
            AddHtmlToParagraphHelper(html, htmlParagheraph);
        }

        public void AddBody(Section section, string? html, IFormFile[]? images)
        {
            var beforSpaceParagraph = section.AddParagraph();
            beforSpaceParagraph.AddLineBreak();
            beforSpaceParagraph.AddLineBreak();

            var imagesParagraph = section.AddParagraph();
            foreach (var image in images)
            {
                AddImageToParagraphHelper(image, imagesParagraph);
            }


            var htmlParagheraph = section.AddParagraph();
            htmlParagheraph.AddLineBreak();

            AddHtmlToParagraphHelper(html, htmlParagheraph);
        }

        public void AddFooter(Section section, string? html, IFormFile? image)
        {
            var htmlParagheraph = section.Footers.Primary.AddParagraph();

            var imageParageraph = section.Footers.Primary.AddParagraph();
            imageParageraph.Format.Alignment = ParagraphAlignment.Right;


            AddHtmlToParagraphHelper(html, htmlParagheraph);
            AddImageToParagraphHelper(image, imageParageraph);


        }
        private void AddImageToParagraphHelper(IFormFile? image, Paragraph paragraph)
        {
            if (image != null && image.Length > 0)
            {
                var ms = new MemoryStream();
                image.CopyTo(ms);
                ms.Position = 0;
                var readyImage = "base64:" + Convert.ToBase64String(ms.ToArray());
                paragraph.AddImage(readyImage);
                paragraph.AddText("        ");
            }
        }

        private void AddHtmlToParagraphHelper(string? html, Paragraph paragraph)
        {
            if (!string.IsNullOrEmpty(html))
            {
                paragraph.AddHtml(html);
            }
        }

    }
}