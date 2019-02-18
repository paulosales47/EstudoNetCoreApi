using Alura.ListaLeitura.Seguranca;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public class AuthApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private string _token;
        
        public AuthApiClient(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("Configuracao");
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("UriAutenticacao").Value);
        }

        private async Task PostLoginAsync(LoginModel login)
        {
            var resposta = await _httpClient
                .PostAsJsonAsync(_configuration.GetSection("Login").Value, login);

            resposta.EnsureSuccessStatusCode();
            _token = await resposta.Content.ReadAsStringAsync();
        }

        private async Task<string> RecuperaToken()
        {
            if (string.IsNullOrEmpty(_token))
            {
                await PostLoginAsync(null);
                return _token;
            }
            else
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var token = jwtHandler.ReadJwtToken(_token);
                var expireDate = token.ValidTo;

                if(expireDate < DateTime.Now.AddMinutes(1))
                {
                    
                }

                return "";
            }
        }

    }
}
