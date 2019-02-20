using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Estudo.AspNetCore.Api.Models
{
    public class ErrorResponse
    {
        public static ErrorResponse Create(Exception exception)
        {
            return new ErrorResponse
            {
                Error = Error.Create(exception)
            };
        }

        public static ErrorResponse CreateFromModelState(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(m => m.Errors);

            return new ErrorResponse
            {
                Error = new Error
                {
                    Code = 400,
                    Message = "Verifique os parametros enviados a API e/ou consulte a documentação",
                    Details = Error.CreateFromModelErrors(errors)
                }
            };
        }

        public Error Error { get; private set; }

    }

    public class Error
    {
        public static Error Create(Exception exception)
        {
            if (exception is null)
                return null;

            return new Error
            {
                Code = exception.HResult,
                Message = exception.Message,
                Target = exception.Source,
                InnerError = Create(exception.InnerException)

            };
        }

        public static IEnumerable<string> CreateFromModelErrors(IEnumerable<ModelError> errors)
        {
            foreach (var error in errors)
            {
                yield return error.ErrorMessage;
            }
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public string Target { get; set; }

        public Error InnerError { get; set; }

        public IEnumerable<string> Details { get; set; }
    }
}
