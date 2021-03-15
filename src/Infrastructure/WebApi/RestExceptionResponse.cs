using System.Collections.Generic;
using Domain.Errors;

namespace WebApi
{
    public record RestExceptionResponse
    {
        public RestExceptionResponse(int code, string errorKey,
            IEnumerable<DomainValidationFailure> errors,
            string message) =>
            (Status, ErrorKey, Errors, Message) = (code, errorKey, errors, message);

        public RestExceptionResponse(int code,
            IEnumerable<DomainValidationFailure> errors,
            string message) =>
            (Status, Errors, Message) = (code, errors, message);

        public RestExceptionResponse(int code, string errorKey,
            string message) =>
            (Status, ErrorKey, Message) = (code, errorKey, message);

        public RestExceptionResponse(int code, string message) =>
            (Status, Message) = (code, message);

        public string Message { get; }

        public int Status { get; }

        public IEnumerable<DomainValidationFailure>? Errors { get; }

        public string? ErrorKey { get; }
    }
}