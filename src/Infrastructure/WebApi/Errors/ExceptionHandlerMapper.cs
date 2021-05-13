using System;
using System.Linq;
using System.Net;
using Application.Services.Shared.Errors;
using Domain.Shared.Errors;
using FluentValidation;
using LanguageExt;

namespace WebApi.Errors
{
    public static class ExceptionHandlerMapper
    {
        public static RestErrorResponse Map(Exception exception)
        {
            return exception switch
            {
                DomainException dException => HandleDomainException(dException),
                ValidationException vException => HandleFluentValidationException(vException),
                ArgumentNullException aException => HandleArgumentException(aException),
                { } ex => new RestErrorResponse(
                    (int) HttpStatusCode.InternalServerError,
                    ex.Message)
            };
        }


        private static RestErrorResponse HandleArgumentException(ArgumentException aException)
        {
            var paramName = aException.ParamName ?? "Param";
            return new RestErrorResponse((int) HttpStatusCode.BadRequest,
                $"NotNull{paramName}",
                new[]
                {
                    new Error("NotNullValidator", paramName, aException.Message)
                },
                aException.Message);
        }

        private static RestErrorResponse HandleFluentValidationException(ValidationException vException)
        {
            return vException.Errors.Select(x =>
                    new Error(x.ErrorCode, x.PropertyName, x.ErrorMessage)).ToSeq().Case switch
                {
                    EmptyCase<Error> => new RestErrorResponse(),
                    HeadCase<Error> headCase => new RestErrorResponse((int) HttpStatusCode.BadRequest,
                        headCase.Head.Code,
                        headCase.Head.Message),
                    HeadTailCase<Error> headTailCase => new RestErrorResponse((int) HttpStatusCode.BadRequest,
                        headTailCase.Head.Code,
                        headTailCase.Tail, headTailCase.Head.Message),
                    _ => new RestErrorResponse()
                };
        }

        private static HttpStatusCode GetStatusCode(Error appException)
        {
            return appException switch
            {
                EntityAlreadyExistsError => HttpStatusCode.Conflict,
                EntityNotFoundError => HttpStatusCode
                    .BadRequest, // TODO changing this to NotFound gives an error while copying content stream
                ValidationError => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };
        }


        private static RestErrorResponse HandleDomainException(DomainException vException)
        {
            return new
            (
                (int) GetStatusCode(vException.MainError),
                vException.ErrorKey,
                vException.Errors,
                vException.Message);
        }
    }
}