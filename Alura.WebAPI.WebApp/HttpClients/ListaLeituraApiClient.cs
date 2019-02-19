using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
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
        private readonly IHttpContextAccessor _accessor;

        public ListaLeituraApiClient(IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _configuration = configuration.GetSection("Configuracao");
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("UriAPI").Value);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                (
                    "Bearer"
                    , _accessor.HttpContext.User.Claims.First(c => c.Type.Equals("Token")).Value
                );

        }


        public async Task<Leituras> GetListaLeituraAsync(TipoListaLeitura tipo)
        {
            HttpResponseMessage resposta = await _httpClient
                .GetAsync(string.Format(_configuration.GetSection("GetByIdListaLeitura").Value, tipo.ParaString()));

            resposta.EnsureSuccessStatusCode();

            return await resposta.Content.ReadAsAsync<Leituras>();
        }
    }
}
