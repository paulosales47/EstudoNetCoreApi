using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Estudo.AspNetCore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly IRepository<Livro> _repository;

        public LivrosController(IRepository<Livro> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}", Name = "GetLivro")]
        public IActionResult Get(int id)
        {
            Livro model = _repository.Find(id);

            if (model is null)
                return NotFound();

            return Ok(model.ToApi());
        }

        [HttpGet()]
        public IActionResult Get()
        {
            List<LivroApi> livros = _repository.All.Select(l => l.ToApi()).ToList();

            if (livros is null)
                return NotFound();

            return Ok(livros);
        }

        [HttpPost]
        public IActionResult Post([FromForm] LivroUpload livroUpload)
        {
            if (ModelState.IsValid)
            {
                var livro = livroUpload.ToLivro();

                _repository.Incluir(livro);

                return CreatedAtRoute(
                    routeName: "GetLivro",
                    routeValues: new { id = livro.Id },
                    value: livro);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Excluir(new Livro { Id = id});

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] LivroUpload model)
        {
            model.Id = id;

            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                if (model.Capa == null)
                {
                    livro.ImagemCapa = _repository.All
                        .Where(l => l.Id == livro.Id)
                        .Select(l => l.ImagemCapa)
                        .FirstOrDefault();
                }
                _repository.Alterar(livro);

                return Ok();
            }
            return BadRequest();
        }
    }
}