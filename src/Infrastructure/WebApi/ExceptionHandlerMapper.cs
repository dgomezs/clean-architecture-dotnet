using System;
using System.Linq;
using System.Net;
using Application.Services.Errors;
using Domain.Errors;
using FluentValidation;
using WebApi;

namespace App
{
    public class ExceptionHandlerMapper
    {
        public static RestExceptionResponse Map(Exception exception)
        {
            return exception switch
            {
                DomainValidationException vException => HandleDomainValidationException(vException),
                DomainException dException => HandleDomainException(dException),
                ValidationException vException => HandleFluentValidationException(vException),
                ArgumentNullException aException => HandleArgumentException(aException),
                { } ex => new RestExceptionResponse(
                    (int) HttpStatusCode.InternalServerError,
                    ex.Message)
            };
        }


        private static RestExceptionResponse HandleArgumentException(ArgumentException aException)
        {
            var paramName = aException.ParamName ?? "Param";
            return new RestExceptionResponse((int) HttpStatusCode.BadRequest,
                $"NotNull{paramName}",
                new[]
                {
                    new Error("NotNullValidator", paramName, aException.Message)
                },
                aException.Message);
        }

        private static RestExceptionResponse HandleFluentValidationException(ValidationException vException)
        {
            return new((int) HttpStatusCode.BadRequest,
                vException.Errors.Select(x =>
                    new Error(x.ErrorCode, x.PropertyName, x.ErrorMessage)),
                vException.Message);
        }

        private static HttpStatusCode GetStatusCode(DomainException appException)
        {
            return appException switch
            {
                EntityExistsException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };
        }

        private static RestExceptionResponse HandleDomainValidationException(
            DomainValidationException vException)
        {
            return new((int) HttpStatusCode.BadRequest,
                vException.ErrorKey,
                vException.Errors,
                vException.Message);
        }

        private static RestExceptionResponse HandleDomainException(DomainException vException)
        {
            return new
            (
                (int) GetStatusCode(vException),
                vException.ErrorKey,
                vException.Errors,
                vException.Message);
        }
    }
}