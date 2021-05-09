using System.Collections.Generic;
using System.Linq;
using System.Net;
using Domain.Shared.Errors;

namespace WebApi.Errors
{
    public record RestErrorResponse
    {
        private const string UnexpectedServerError = "UnexpectedServerError";

        public RestErrorResponse(int code, string errorKey,
            IEnumerable<Error> errors,
            string message) =>
            (Status, Code, Errors, Message) = (code, errorKey,
                errors.Select(e => new RestError(e.Code, e.Message, e.PropertyName)), message);

        public RestErrorResponse(int code, string errorKey,
            string message) =>
            (Status, Code, Message) = (code, errorKey, message);


        public RestErrorResponse(int code, string message) =>
            (Code, Status, Message) = (UnexpectedServerError, code, message);

        public RestErrorResponse() =>
            (Code, Status, Message) = (UnexpectedServerError, (int) HttpStatusCode.InternalServerError,
                UnexpectedServerError);


        public string Message { get; }

        public int Status { get; }

        public IEnumerable<RestError> Errors { get; } = new List<RestError>();

        public string Code { get; }
    }

    public record RestError(string Code, string Message, string? Property);
}