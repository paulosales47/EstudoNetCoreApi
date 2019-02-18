using System.Threading.Tasks;
using Alura.ListaLeitura.Seguranca;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public interface IAuthApiClient
    {
        Task<string> PostLoginAsync(LoginModel login);
    }
}