using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Errors
{
    public class DomainException : Exception
    {
        private readonly Error _mainError;
        private List<Error> _errors = new();

        public DomainException(Error mainError) : base(mainError.Message) =>
            _mainError = mainError;

        public DomainException(string errorKey, string errorMessage) : base(errorMessage) =>
            _mainError = new Error(errorKey, errorMessage);

        public DomainException(string errorKey, string errorMessage, IEnumerable<Error> errors) : base(
            errorMessage)
        {
            _mainError = new Error(errorKey, errorMessage);
            _errors = errors.ToList();
        }

        public DomainException(string errorKey) =>
            _mainError = new Error(errorKey);

        public DomainException(string errorKey, IEnumerable<Error> errors)
            => (_mainError, _errors) = (new Error(errorKey), errors.ToList());

        public DomainException(Error error, Exception innerException) : base(
            error.Message, innerException) => _mainError = error;


        public DomainException(Error mainError, IEnumerable<Error> errors) : base(mainError.Message)
            => (_mainError, _errors) = (mainError, errors.ToList());


        public string ErrorKey => _mainError.ErrorCode;

        public IEnumerable<Error> Errors => _errors;

        public override string Message => _mainError.Message;

        public DomainException PrefixErrors(string prefix)
        {
            _errors = _errors.Select(e => e with {PropertyName = $"{prefix}.{e.PropertyName}"}).ToList();
            return this;
        }
    }
}