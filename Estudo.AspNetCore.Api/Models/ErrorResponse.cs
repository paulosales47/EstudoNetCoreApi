using System;

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

        public int Code { get; set; }

        public string Message { get; set; }

        public string Target { get; set; }

        public Error InnerError { get; set; }
    }
}
