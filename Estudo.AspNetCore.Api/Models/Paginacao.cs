using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Estudo.AspNetCore.Api.Models
{
    public class Paginacao
    {
        public int Total { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanhoPagina { get; set; }
        public int NumeroPagina { get; set; }
        public IList<LivroApi> Resultado { get; set; }
        public string Anterior { get; set; }
        public string Proximo { get; set; }
    }

    public static class LivroPaginadoExtensions
    {
        public static Paginacao Paginar(this IQueryable<LivroApi> query, LivroPaginacao livroPaginacao)
        {
            int totalItens = query.Count();
            int totalPaginas = (int)Math.Ceiling(totalItens / Convert.ToDouble(livroPaginacao.Tamanho));

            return new Paginacao
            {
                Total = totalItens,
                TotalPaginas = totalPaginas,
                NumeroPagina = livroPaginacao.Pagina,
                TamanhoPagina = livroPaginacao.Tamanho,
                Resultado = query
                    .Skip(livroPaginacao.Tamanho * (livroPaginacao.Pagina - 1))
                    .Take(livroPaginacao.Tamanho).ToList(),
                Anterior = livroPaginacao.Pagina > 1 ? 
                    $"livros?tamanho={livroPaginacao.Pagina-1}&pagina={livroPaginacao.Tamanho}": string.Empty,
                Proximo = livroPaginacao.Pagina < totalPaginas ?
                    $"livros?tamanho={livroPaginacao.Pagina + 1}&pagina={livroPaginacao.Tamanho}" : string.Empty,
            };
        }
    }
}
