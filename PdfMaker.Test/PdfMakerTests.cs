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
            using var form = new MultipartFormDataContent();

            var logoFile = new StreamContent(File.OpenRead("./files/logo.png"));
            logoFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            form.Add(logoFile, name: "HeaderImage", fileName: "logo.png");

            var redFile = new StreamContent(File.OpenRead("./files/red.png"));
            redFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            form.Add(redFile, name: "BodyImages", fileName: "red.png");

            var blueFile = new StreamContent(File.OpenRead("./files/blue.png"));
            blueFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            form.Add(blueFile, name: "BodyImages", fileName: "blue.png");

            var greenFile = new StreamContent(File.OpenRead("./files/green.png"));
            greenFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            form.Add(greenFile, name: "BodyImages", fileName: "green.png");

            var footerFile = new StreamContent(File.OpenRead("./files/footer.png"));
            footerFile.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            form.Add(footerFile, name: "FooterImage", fileName: "footer.png");

            form.Add(new StringContent("<h1> This is heading 1 </h1><br/>  line2 "), name: "HeaderHtml");
            form.Add(new StringContent($"<a href='https://google.com'>Google</a><br/>{DateTime.Now}"), name: "FooterHtml");
            form.Add(new StringContent("{'name':'John', 'age':30, 'car':null}"), name: "ContactInfo");
            form.Add(new StringContent(
                " This is not heading <br/>" +
                "<h1> This is heading 1 </h1><br/>" +
                "<h2> This is heading 2 </h2><br/>" +
                "<h3> This is heading 3 </h3><br/>" +
                "<h4> This is heading 4 </h4><br/>" +
                "<h5> This is heading 5 </h5><br/>" +
                "<h6> This is heading 6 </h6><br/>"), name: "BodyHtml");

            var h1Style = new
            {
                Title = "Heading1",
                FontFamily = "Open Sans",
                FontSize = 20,
                FontColor = "#FF0000",
            };

            var h2Style = new
            {
                Title = "Heading2",
                FontFamily = "Open Sans",
                FontSize = 18,
                FontColor = "#FFFF00",
            };

            var footerStyle = new
            {
                Title = "Footer",
                FontFamily = "Open Sans",
                FontSize = 12,
                FontColor = "#0000FF",
            };

            var linkStyle = new
            {
                Title = "Hyperlink",
                FontFamily = "tahoma",
                FontSize = 20,
                FontColor = "#008000",
            };

            var headerStyle = new
            {
                Title = "Header",
                FontFamily = "Open Sans",
                FontSize = 12,
                FontColor = "#FF00FF",
            };

            var NormalStyle = new
            {
                Title = "Normal",
                FontFamily = "Open Sans",
                FontSize = 0,
                FontColor = "",
            };


            form.Add(new StringContent(System.Text.Json.JsonSerializer.Serialize(new[] { NormalStyle, footerStyle, headerStyle, h1Style, h2Style })), name: "HtmlStyles");

            var client = new HttpClient();
            //var baseAddress = "https://xx.azurewebsites.net";
            var baseAddress = "https://localhost:7131";
            client.BaseAddress = new Uri(baseAddress);
            var postResponse = await client.PostAsync("/pdf", form);
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