using System.Linq;
using Alura.ListaLeitura.Persistencia;
using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Alura.WebAPI.WebApp.HttpClients;

namespace Alura.ListaLeitura.WebApp.Controllers
{
    [Authorize]
    public class LivroController : Controller
    {
        private readonly IRepository<Livro> _repo;
        private readonly ILivroApiClient _apiClient;

        public LivroController(IRepository<Livro> repository, ILivroApiClient apiClient)
        {
            _repo = repository;
            _apiClient = apiClient;
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
            byte[] img = await _apiClient.GetCapaAsync(id);

            if (img != null)
            {
                return File(img, "image/png");
            }
            return File("~/images/capas/capa-vazia.png", "image/png");
        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {
            var model = await _apiClient.GetLivroAsync(id);
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
        public async Task<IActionResult> Remover(int id)
        {
            var model = await _apiClient.GetLivroAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            await _apiClient.DeleteLivroAsync(id);
            return RedirectToAction("Index", "Home");
        }
    }
}