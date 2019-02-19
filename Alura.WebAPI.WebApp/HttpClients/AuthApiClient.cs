using Alura.ListaLeitura.Seguranca;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public class AuthApiClient: IAuthApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private LoginResult _loginResult;

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

            _loginResult = new LoginResult
            {
                Succeeded = resposta.IsSuccessStatusCode,
                Token = await resposta.Content.ReadAsStringAsync()

            };
        }

        public async Task<LoginResult> GetToken(LoginModel login)
        {
            if (_loginResult is null)
            {
                await PostLoginAsync(login);

                return _loginResult;
            }
            else
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var token = jwtHandler.ReadJwtToken(_loginResult.Token);
                var expireDate = token.ValidTo;

                if (expireDate < DateTime.Now.AddMinutes(1))
                {
                    return _loginResult;
                }
                else
                {
                    //IMPLEMENTAR REFRESH TOKEN
                    throw new TimeoutException("O token de acesso expirou, por favor logue-se novamente");
                }

            }
        }
    }

    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
    }
}
