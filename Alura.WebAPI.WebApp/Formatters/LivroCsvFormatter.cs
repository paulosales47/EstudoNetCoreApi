using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.WebAPI.WebApp.Formatters
{
    public class LivroCsvFormatter : TextOutputFormatter
    {
        public LivroCsvFormatter()
        {
            this.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            this.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            string livroCsv = string.Empty;

            if(context.Object is LivroApi)
            {
                var livro = context.Object as LivroApi;
                livroCsv = $"{livro.Titulo}, {livro.Subtitulo}, {livro.Autor}, {livro.Lista}";
            }
            if(context.Object is IEnumerable<LivroApi>)
            {
                foreach (var livro in context.Object as IEnumerable<LivroApi>)
                {
                    livroCsv += $"{livro.Titulo}, {livro.Subtitulo}, {livro.Autor}, {livro.Lista}\n";
                }
            }


            using (var escritor = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                return escritor.WriteAsync(livroCsv);
            }
        }
    }
}
