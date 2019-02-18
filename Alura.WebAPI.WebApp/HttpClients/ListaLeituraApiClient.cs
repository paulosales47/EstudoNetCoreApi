using Alura.ListaLeitura.Modelos;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Leituras = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public class ListaLeituraApiClient : IListaLeituraApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ListaLeituraApiClient(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("Configuracao");
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("UriAPI").Value);
        }

        public async Task<Leituras> GetListaLeituraAsync(TipoListaLeitura tipo)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", "");
            
            HttpResponseMessage resposta = await _httpClient
                .GetAsync(string.Format(_configuration.GetSection("GetByIdListaLeitura").Value, tipo.ParaString()));

            resposta.EnsureSuccessStatusCode();

            return await resposta.Content.ReadAsAsync<Leituras>();
        }
    }
}
