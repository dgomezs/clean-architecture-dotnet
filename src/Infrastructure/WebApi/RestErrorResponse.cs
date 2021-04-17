using System.Collections.Generic;
using Domain.Shared.Errors;

namespace WebApi
{
    public record RestErrorResponse
    {
        public RestErrorResponse(int code, string errorKey,
            IEnumerable<Error> errors,
            string message) =>
            (Status, ErrorKey, Errors, Message) = (code, errorKey, errors, message);

        public RestErrorResponse(int code,
            IEnumerable<Error> errors,
            string message) =>
            (Status, Errors, Message) = (code, errors, message);

        public RestErrorResponse(int code, string errorKey,
            string message) =>
            (Status, ErrorKey, Message) = (code, errorKey, message);

        public RestErrorResponse(int code, string message) =>
            (Status, Message) = (code, message);

        public string Message { get; }

        public int Status { get; }

        public IEnumerable<Error> Errors { get; } = new List<Error>();

        public string? ErrorKey { get; }
    }
}