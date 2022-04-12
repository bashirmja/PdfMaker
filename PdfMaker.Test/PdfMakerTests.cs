using System;
using System.Net.Http;
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