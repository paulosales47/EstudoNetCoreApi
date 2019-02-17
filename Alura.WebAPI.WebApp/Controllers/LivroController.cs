using System.Linq;
using Alura.ListaLeitura.Persistencia;
using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.WebApp.Controllers
{
    [Authorize]
    public class LivroController : Controller
    {
        private readonly IRepository<Livro> _repo;
        private readonly IConfiguration _configuration;

        public LivroController(IRepository<Livro> repository, IConfiguration configuration)
        {
            _repo = repository;
            _configuration = configuration.GetSection("Configuracao");
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return View(new LivroUpload());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Novo(LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                _repo.Incluir(model.ToLivro());
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ImagemCapa(int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configuration.GetSection("UriAPI").Value);

            HttpResponseMessage resposta = await httpClient
                .GetAsync(string.Format(_configuration.GetSection("GetByIdImagemCapa").Value, id));

            resposta.EnsureSuccessStatusCode();

            byte[] img = await resposta.Content.ReadAsByteArrayAsync();

            if (img != null)
            {
                return File(img, "image/png");
            }
            return File("~/images/capas/capa-vazia.png", "image/png");
        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configuration.GetSection("UriAPI").Value);

            HttpResponseMessage resposta = await httpClient
                .GetAsync(string.Format(_configuration.GetSection("GetByIdLivros").Value, id));

            resposta.EnsureSuccessStatusCode();

            var model = await resposta.Content.ReadAsAsync<LivroApi>();
            if (model == null)
            {
                return NotFound();
            }
            return View(model.ToUpload());
        }

        [HttpGet]
        public IActionResult DetalhesJson(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return Json(model.ToModel());
        }

        [HttpGet]
        public ActionResult<Livro> DetalhesJson2(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return model;
        }

        [HttpGet]
        public Livro DetalhesJson3(int id)
        {
            var model = _repo.Find(id);
            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detalhes(LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                if (model.Capa == null)
                {
                    livro.ImagemCapa = _repo.All
                        .Where(l => l.Id == livro.Id)
                        .Select(l => l.ImagemCapa)
                        .FirstOrDefault();
                }
                _repo.Alterar(livro);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remover(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            _repo.Excluir(model);
            return RedirectToAction("Index", "Home");
        }
    }
}