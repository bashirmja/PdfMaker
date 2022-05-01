using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace PdfMaker.Test
{
    public class PdfMakerTests
    {
        [Fact]
        public async Task CreatePDFAsync()
        {
            using var multipartFormContent = new MultipartFormDataContent();

            var redFile = new StreamContent(File.OpenRead("./files/red.png"));
            redFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(redFile, name: "FormPictures", fileName: "red.png");
            
            var blueFile = new StreamContent(File.OpenRead("./files/blue.png"));
            blueFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(blueFile, name: "FormPictures", fileName: "blue.png");


            multipartFormContent.Add(new StringContent("<p>this is <b>Header</b><br/>subheader</p>"), name: "HeaderHtmlContent");
            multipartFormContent.Add(new StringContent(
                "<p>this is text1 <b>bold1</b>text1<br/>this is text2 <b>bold1</b>text2</p>" +
                "<p>this is text5 <b>bold1</b>text1<br/>this is text5 <b>bold5</b>text5</p>"), name: "BodyHtmlContent");
            multipartFormContent.Add(new StringContent("<p>sdfgsdfg<b>footer</b>sdgdg<br/>sghfgh<b>subfooter</b>gdhsgh</P>"), name: "FooterHtmlContent");

            var client = new HttpClient();
            var baseAddress = "https://localhost:7131";
            client.BaseAddress = new Uri(baseAddress);

            var postResponse = await client.PostAsync("/pdf", multipartFormContent);
            string fileId = await postResponse.Content.ReadAsStringAsync();

            Process.Start(new ProcessStartInfo(baseAddress + "/pdf/" + fileId) { UseShellExecute = true });

            Assert.NotEmpty(fileId);
        }

        [Fact]
        public void TrueTest()
        {
            Assert.True(true);
        }
    }
}