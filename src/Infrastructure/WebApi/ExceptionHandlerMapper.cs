﻿using System;
using System.Linq;
using System.Net;
using Application.Services.Errors;
using Domain.Errors;
using FluentValidation;

namespace WebApi
{
    public class ExceptionHandlerMapper
    {
        public static RestErrorResponse Map(Exception exception)
        {
            return exception switch
            {
                DomainValidationException vException => HandleDomainValidationException(vException),
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

        private static RestErrorResponse HandleDomainValidationException(
            DomainValidationException vException)
        {
            return new((int) HttpStatusCode.BadRequest,
                vException.ErrorKey,
                vException.Errors,
                vException.Message);
        }

        private static RestErrorResponse HandleDomainException(DomainException vException)
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