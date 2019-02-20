using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Estudo.AspNetCore.Api.Models
{
    public class LivroOrdem
    {
        public string Ordenacao { get; set; }
    }

    public static class LivroOrdemExtensions
    {
        public static IQueryable<Livro> Ordenar(this IQueryable<Livro> query, LivroOrdem ordem)
        {
            if (ordem is null)
                return query;

            query = query.OrderBy(ordem.Ordenacao);

            return query;
        }
    }
}
