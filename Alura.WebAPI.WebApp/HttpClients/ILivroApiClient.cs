using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public interface ILivroApiClient
    {
        Task<byte[]> GetCapaAsync(int id);
        Task<LivroApi> GetLivroAsync(int id);
        Task DeleteLivroAsync(int id);
        Task PostLivroAsync(LivroUpload livro);
        Task PutLivroAsync(LivroUpload livro);
    }
}