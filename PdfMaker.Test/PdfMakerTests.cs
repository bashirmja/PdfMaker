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

            var blueFile = new StreamContent(File.OpenRead("./files/blue.png"));
            blueFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(blueFile, name: "FrontView", fileName: "blue.png");

            var redFile = new StreamContent(File.OpenRead("./files/red.png"));
            redFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(redFile, name: "LeftView", fileName: "red.png");

            var greenFile = new StreamContent(File.OpenRead("./files/green.png"));
            greenFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(greenFile, name: "TopView", fileName: "green.png");


            multipartFormContent.Add(new StringContent(DateTime.Now.ToString()), name: "Title");
            multipartFormContent.Add(new StringContent("10"), name: "TotalFloors");


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