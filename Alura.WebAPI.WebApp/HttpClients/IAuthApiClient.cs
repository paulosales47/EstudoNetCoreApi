using Alura.ListaLeitura.Seguranca;
using System.Threading.Tasks;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public interface IAuthApiClient
    {
        Task<LoginResult> GetToken(LoginModel login);
    }
}