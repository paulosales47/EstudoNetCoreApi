using Alura.ListaLeitura.Modelos;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public class LivroApiClient : ILivroApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public LivroApiClient(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("Configuracao");
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("UriAPI").Value);
        }

        public async Task<LivroApi> GetLivroAsync(int id)
        {
            HttpResponseMessage resposta = await _httpClient
                .GetAsync(string.Format(_configuration.GetSection("GetByIdLivros").Value, id));

            resposta.EnsureSuccessStatusCode();

            return await resposta.Content.ReadAsAsync<LivroApi>();
        }

        public async Task<byte[]> GetCapaAsync(int id)
        {
            HttpResponseMessage resposta = await _httpClient
                .GetAsync(string.Format(_configuration.GetSection("GetByIdImagemCapa").Value, id));

            resposta.EnsureSuccessStatusCode();

            return await resposta.Content.ReadAsByteArrayAsync();
        }

        public async Task DeleteLivroAsync(int id)
        {
            var resposta =  await _httpClient
                .DeleteAsync(string.Format(_configuration.GetSection("GetByIdLivros").Value, id));

            resposta.EnsureSuccessStatusCode();
        }
    }
}
