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

            multipartFormContent.Add(new StringContent("This <b>Header</b> is on top <br/>subheader"), name: "HeaderHtml");

            var logoFile = new StreamContent(File.OpenRead("./files/logo.png"));
            logoFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(logoFile, name: "HeaderImage", fileName: "logo.png");

            multipartFormContent.Add(new StringContent(
                "this is text1 text1 <b>-1</b><br/>" +
                "this is text2 bold1<b>-2</b><br/>" +
                "text2 this is text5 text1<b>-3</b><br/>" +
                "this is text5 bold5text5<b>-4</b>"), name: "BodyHtml");

            var redFile = new StreamContent(File.OpenRead("./files/red.png"));
            redFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(redFile, name: "BodyImages", fileName: "red.png");

            var blueFile = new StreamContent(File.OpenRead("./files/blue.png"));
            blueFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(blueFile, name: "BodyImages", fileName: "blue.png");

            var greenFile = new StreamContent(File.OpenRead("./files/green.png"));
            greenFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(greenFile, name: "BodyImages", fileName: "green.png");


            multipartFormContent.Add(new StringContent($"{DateTime.Now}"), name: "FooterHtml");
            var footerFile = new StreamContent(File.OpenRead("./files/footer.png"));
            footerFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(footerFile, name: "FooterImage", fileName: "footer.png");

            multipartFormContent.Add(new StringContent("{'name':'John', 'age':30, 'car':null}"), name: "ContactInfo");

            var client = new HttpClient();
            //var baseAddress = "https://xx.azurewebsites.net";
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