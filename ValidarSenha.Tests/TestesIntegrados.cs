using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using ValidarSenha.Core.Model;

namespace ValidarSenha.Tests
{
    public class TestesIntegrados : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;

        public TestesIntegrados(CustomWebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Add("Authorization", "basic");
            client.Timeout = new TimeSpan(0, 1, 0);

        }

        [Fact]
        public async Task ExecutaValidacaoComSucesso()
        {
            var httpResponse = await client.GetAsync("/ValidarSenha/validar/Abcdef@1234&5");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var responseAPI = JsonConvert.DeserializeObject<SenhaResponse>(stringResponse);
            Assert.True(responseAPI.senhaValida == true);

        }

        [Fact]
        public async Task ExecutaValidacaoComErroNaValidacao()
        {
            var httpResponse = await client.GetAsync("/ValidarSenha/validar/AbcdAf@1234&5");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var responseAPI = JsonConvert.DeserializeObject<SenhaResponse>(stringResponse);
            Assert.True(responseAPI.senhaValida == false);
        }

    }
}
