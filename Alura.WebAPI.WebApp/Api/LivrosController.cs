using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Alura.WebAPI.WebApp.Api
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly IRepository<Livro> _repository;

        public LivrosController(IRepository<Livro> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Livro model = _repository.Find(id);

            if (model is null)
                return NotFound();

            return Ok(model.ToModel());
        }

        [HttpGet()]
        public IActionResult Get()
        {
            List<LivroUpload> livros = _repository.All.Select(l => l.ToModel()).ToList();

            if (livros is null)
                return NotFound();

            return Ok(livros);
        }

        [HttpPost]
        public IActionResult Post([FromBody] LivroUpload livroUpload)
        {
            if (ModelState.IsValid)
            {
                var livro = livroUpload.ToLivro();

                _repository.Incluir(livro);

                string uri = Url.Action("Recuperar", new { id = livro.Id });


                return Created(uri, livro);
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
        public IActionResult Put(int id, [FromBody] LivroUpload model)
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