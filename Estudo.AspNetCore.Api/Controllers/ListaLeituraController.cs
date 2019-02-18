using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Leituras = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Estudo.AspNetCore.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ListaLeituraController : ControllerBase
    {
        private readonly IRepository<Livro> _repository;

        public ListaLeituraController(IRepository<Livro> repository)
        {
            _repository = repository;
        }

        public IActionResult Get()
        {
            Leituras paraLer = CriarLeituras(TipoListaLeitura.ParaLer);
            Leituras lendo = CriarLeituras(TipoListaLeitura.Lendo);
            Leituras lidos = CriarLeituras(TipoListaLeitura.Lidos);

            var colecao = new List<Leituras> { paraLer, lendo, lidos };

            return Ok(colecao);
        }

        [HttpGet("{tipo}")]
        public IActionResult Get(TipoListaLeitura tipo)
        {
            Leituras listaLeitura = CriarLeituras(tipo);

            return Ok(listaLeitura);
        }

        private Leituras CriarLeituras(TipoListaLeitura tipo)
        {
            return new Leituras
            {
                Tipo = tipo.ParaString()
                ,
                Livros = _repository.All.Where(l => l.Lista == tipo).Select(l => l.ToApi()).ToList()
            };
        }

        [HttpGet("{id}/capa")]
        public IActionResult Get(int id)
        {
            byte[] img = _repository.All
            .Where(l => l.Id == id)
            .Select(l => l.ImagemCapa)
            .FirstOrDefault();
            if (img != null)
            {
                return File(img, "image/png");
            }
            return File("~/images/capas/capa-vazia.png", "image/png");
        }


    }
}