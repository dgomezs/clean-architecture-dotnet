using System.Collections.Generic;
using System.Net;
using Domain.Shared.Errors;

namespace WebApi
{
    public record RestErrorResponse
    {
        private const string UnexpectedServerError = "UnexpectedServerError";

        public RestErrorResponse(int code, string errorKey,
            IEnumerable<Error> errors,
            string message) =>
            (Status, ErrorKey, Errors, Message) = (code, errorKey, errors, message);

        public RestErrorResponse(int code, string errorKey,
            string message) =>
            (Status, ErrorKey, Message) = (code, errorKey, message);


        public RestErrorResponse(int code, string message) =>
            (ErrorKey, Status, Message) = (UnexpectedServerError, code, message);

        public RestErrorResponse() =>
            (ErrorKey, Status, Message) = (UnexpectedServerError, (int) HttpStatusCode.InternalServerError,
                UnexpectedServerError);


        public string Message { get; }

        public int Status { get; }

        public IEnumerable<Error> Errors { get; } = new List<Error>();

        public string ErrorKey { get; }
    }
}