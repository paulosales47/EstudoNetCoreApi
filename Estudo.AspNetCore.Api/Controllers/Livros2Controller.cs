using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Estudo.AspNetCore.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Estudo.AspNetCore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Livros2Controller : ControllerBase
    {
        private readonly IRepository<Livro> _repository;

        public Livros2Controller(IRepository<Livro> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}", Name = "GetLivro2")]
        public IActionResult Get(int id)
        {
            Livro model = _repository.Find(id);

            if (model is null)
                return NotFound();

            return Ok(model.ToApi());
        }

        [HttpGet()]
        public IActionResult Get([FromQuery] LivroFiltro filtro, [FromQuery] LivroOrdem ordem, [FromQuery] LivroPaginacao paginacao)
        {
            Paginacao livroPaginado = _repository
                .All
                .Filtrar(filtro)
                .Ordenar(ordem)
                .Select(l => l.ToApi())
                .Paginar(paginacao);

            if (livroPaginado is null)
                return NotFound();

            return Ok(livroPaginado);
        }

        [HttpPost]
        public IActionResult Post([FromForm] LivroUpload livroUpload)
        {
            if (ModelState.IsValid)
            {
                var livro = livroUpload.ToLivro();

                _repository.Incluir(livro);

                return CreatedAtRoute(
                    routeName: "GetLivro2",
                    routeValues: new { id = livro.Id },
                    value: livro);
            }
            return BadRequest(ErrorResponse.CreateFromModelState(ModelState));
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