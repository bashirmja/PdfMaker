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
            var pngHeader = new MediaTypeHeaderValue("image/png");

            var blueFile = new StreamContent(File.OpenRead("./files/blue.png"));
            blueFile.Headers.ContentType = pngHeader;

            var redFile = new StreamContent(File.OpenRead("./files/red.png"));
            redFile.Headers.ContentType = pngHeader;

            var greenFile = new StreamContent(File.OpenRead("./files/green.png"));
            greenFile.Headers.ContentType = pngHeader;

            var logoFile = new StreamContent(File.OpenRead("./files/logo.png"));
            logoFile.Headers.ContentType = pngHeader;

            multipartFormContent.Add(redFile, name: "LeftView", fileName: "red.png");
            multipartFormContent.Add(blueFile, name: "FrontView", fileName: "blue.png");
            multipartFormContent.Add(greenFile, name: "TopView", fileName: "green.png");
            multipartFormContent.Add(logoFile, name: "CompanyLogo", fileName: "logo.png");

            multipartFormContent.Add(new StringContent("COBOD Configurator"), name: "ProductTitle");
            multipartFormContent.Add(new StringContent("BOD2 4-8-2"), name: "ConfigTitle");

            multipartFormContent.Add(new StringContent("Width(X)"), name: "WidthTitle");
            multipartFormContent.Add(new StringContent("4 Modules"), name: "WidthModules");
            multipartFormContent.Add(new StringContent("9.6m"), name: "WidthSize");

            multipartFormContent.Add(new StringContent("Lenght(Y)"), name: "LenghtTitle");
            multipartFormContent.Add(new StringContent("8 Modules"), name: "LenghtModules");
            multipartFormContent.Add(new StringContent("19.1m"), name: "LenghtSize");

            multipartFormContent.Add(new StringContent("Height(Z)"), name: "HeightTitle");
            multipartFormContent.Add(new StringContent("2 Modules"), name: "HeightModules");
            multipartFormContent.Add(new StringContent("3.5m"), name: "HeightSize");

            multipartFormContent.Add(new StringContent("1 Floor"), name: "FloorCount");
            multipartFormContent.Add(new StringContent("Floor Area"), name: "FloorAreaTitle");
            multipartFormContent.Add(new StringContent("183m2"), name: "FloorAreaSize");
            multipartFormContent.Add(new StringContent("Total Area"), name: "TotalAreatitle");
            multipartFormContent.Add(new StringContent("183m2"), name: "TotalAreaSize");

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