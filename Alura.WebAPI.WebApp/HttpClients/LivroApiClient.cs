using Alura.ListaLeitura.Modelos;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
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

        public async Task PostLivroAsync(LivroUpload livro)
        {
            HttpContent content = CreateMultiPartFormData(livro);
            var resposta = await _httpClient
                .PostAsync(_configuration.GetSection("GetAllLivros").Value, content);

            resposta.EnsureSuccessStatusCode();
        }

        public async Task PutLivroAsync(LivroUpload livro)
        {
            HttpContent content = CreateMultiPartFormData(livro);
            var resposta = await _httpClient
                .PutAsync(string.Format(_configuration.GetSection("GetByIdLivros").Value, livro.Id), content);

            resposta.EnsureSuccessStatusCode();
        }

        private HttpContent CreateMultiPartFormData(LivroUpload livro)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(livro.Titulo), "titulo");
            content.Add(new StringContent(livro.Subtitulo), "subtitulo");
            content.Add(new StringContent(livro.Resumo), "resumo");
            content.Add(new StringContent(livro.Autor), "autor");
            content.Add(new StringContent(livro.Lista.ParaString()), "lista");
            
            if(livro.Capa != null)
            {
                var imageContent = new ByteArrayContent(livro.Capa.ConvertToBytes());
                imageContent.Headers.Add("Content-Type", "image/png");
                content.Add(imageContent, "capa", "capa.png");
            }

            return content;
        }
    }
}
