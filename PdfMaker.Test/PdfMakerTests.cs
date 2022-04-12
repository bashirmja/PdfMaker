using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace PdfMaker.Test
{
    public class PdfMakerTests
    {
        private readonly Uri BaseAddress = new Uri("https://localhost:7131");

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


            multipartFormContent.Add(new StringContent("Hello!"), name: "Title");
            multipartFormContent.Add(new StringContent("10"), name: "TotalFloors");


            var client = new HttpClient();
            client.BaseAddress = BaseAddress;
            var httpResponse = await client.PostAsync("/createpdf", multipartFormContent);

            string resultContent = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal("Hello", resultContent);
        }



        [Fact]
        public async Task TestPostAsync()
        {
            var payload = new
            {
                Title = "COBOD"
            };
            var stringPayload = JsonSerializer.Serialize(payload);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.BaseAddress = BaseAddress;
            var httpResponse = await client.PostAsync("/testpost", httpContent);

            string resultContent = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);
            Assert.Equal("COBOD", resultContent);
        }


        [Fact]
        public async Task TestGetAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = BaseAddress;
            var result = await client.GetAsync("/testget");
            string resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal("GetOK", resultContent);
        }
    }
}