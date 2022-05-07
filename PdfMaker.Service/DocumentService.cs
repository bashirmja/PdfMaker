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
            section.PageSetup = document.DefaultPageSetup.Clone();

            section.PageSetup.HeaderDistance = "5mm";
            section.PageSetup.FooterDistance = "5mm";
            section.PageSetup.TopMargin = "25mm";
            section.PageSetup.BottomMargin = "25mm";
            section.PageSetup.LeftMargin = "15mm";
            section.PageSetup.RightMargin = "15mm";
            return section;
        }

        public void AddHeader(Section section, string? html, IFormFile? image)
        {
            var HeaderImageParageraph = section.Headers.Primary.AddParagraph();
            HeaderImageParageraph.Format.Alignment = ParagraphAlignment.Right;

            var HeaderHtmlParagheraph = section.AddParagraph();
            HeaderHtmlParagheraph.Format.Alignment = ParagraphAlignment.Center;

            AddImageToParagraphHelper(image, HeaderImageParageraph);
            AddHtmlToParagraphHelper(html, HeaderHtmlParagheraph);
        }

        public void AddBody(Section section, string? html, IFormFile[]? images)
        {
            var beforSpaceParagraph = section.AddParagraph();
            beforSpaceParagraph.AddLineBreak();
            beforSpaceParagraph.AddLineBreak();

            if (images != null)
            {
                var imagesParagraph = section.AddParagraph();
                for (int i = 0; i < images.Length; i++)
                {
                    AddImageToParagraphHelper(images[i], imagesParagraph, images.Length - 1 != i ? 10 : 0);
                }
            }

            var htmlParagheraph = section.AddParagraph();
            htmlParagheraph.AddLineBreak();

            AddHtmlToParagraphHelper(html, htmlParagheraph);
        }

        public void AddFooter(Section section, string? html, IFormFile? image)
        {

            var footerImageParageraph = section.Footers.Primary.AddParagraph();
            footerImageParageraph.Format.Alignment = ParagraphAlignment.Right;

            var footerHtmlParagheraph = section.Footers.Primary.AddParagraph();
            footerHtmlParagheraph.Format.Alignment = ParagraphAlignment.Right;

            AddImageToParagraphHelper(image, footerImageParageraph);
            AddHtmlToParagraphHelper(html, footerHtmlParagheraph);
        }

        private void AddImageToParagraphHelper(IFormFile? image, Paragraph paragraph, int spaceAfter = 0)
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

        private void AddHtmlToParagraphHelper(string? html, Paragraph paragraph)
        {
            if (!string.IsNullOrEmpty(html))
            {
                paragraph.AddHtml(html);
            }
        }

    }
}