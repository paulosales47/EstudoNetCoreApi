using Alura.ListaLeitura.Persistencia;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leituras = Alura.ListaLeitura.Modelos.ListaLeitura;
using Alura.WebAPI.WebApp.HttpClients;

namespace Alura.ListaLeitura.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRepository<Livro> _repo;
        private readonly IListaLeituraApiClient _apiClient;

        public HomeController(IRepository<Livro> repository, IListaLeituraApiClient apiClient)
        {
            _repo = repository;
            _apiClient = apiClient;
        }

        private async Task<IEnumerable<LivroApi>> ListaDoTipo(TipoListaLeitura tipo)
        {
            var lista = await _apiClient.GetListaLeituraAsync(tipo);
            return lista.Livros;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                ParaLer = await ListaDoTipo(TipoListaLeitura.ParaLer),
                Lendo = await ListaDoTipo(TipoListaLeitura.Lendo),
                Lidos = await ListaDoTipo(TipoListaLeitura.Lidos)
            };
            return View(model);
        }
    }
}