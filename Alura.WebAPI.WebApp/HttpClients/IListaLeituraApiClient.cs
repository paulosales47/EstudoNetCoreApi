using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public interface IListaLeituraApiClient
    {
        Task<ListaLeitura.Modelos.ListaLeitura> GetListaLeituraAsync(TipoListaLeitura tipo);
    }
}