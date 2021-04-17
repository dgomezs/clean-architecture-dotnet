using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Domain.Shared.Errors
{
    public class DomainException : Exception
    {
        public readonly Error MainError;
        private List<Error> _errors = new();

        public DomainException(Error mainError) : base(mainError.Message) =>
            MainError = mainError;

        public DomainException(string errorKey, string errorMessage) : base(errorMessage) =>
            MainError = new Error(errorKey, errorMessage);

        public DomainException(string errorKey, string errorMessage, IEnumerable<Error> errors) : base(
            errorMessage)
        {
            MainError = new Error(errorKey, errorMessage);
            _errors = errors.ToList();
        }

        public DomainException(string errorKey, IEnumerable<ValidationFailure> errors)
        {
            MainError = new ValidationError(errorKey);
            _errors = errors.Select(x => new ValidationError(x)).ToList<Error>();
        }

        public DomainException(string errorKey) =>
            MainError = new Error(errorKey);

        public DomainException(string errorKey, IEnumerable<Error> errors)
            => (MainError, _errors) = (new Error(errorKey), errors.ToList());

        public DomainException(Error error, Exception innerException) : base(
            error.Message, innerException) => MainError = error;


        public DomainException(Error mainError, IEnumerable<Error> errors) : base(mainError.Message)
            => (MainError, _errors) = (mainError, errors.ToList());


        public string ErrorKey => MainError.ErrorKey;

        public IEnumerable<Error> Errors => _errors;

        public override string Message => MainError.Message;

        public override IDictionary Data => MainError.Data;

        public DomainException PrefixErrors(string prefix)
        {
            _errors = _errors.Select(e => e with {PropertyName = $"{prefix}.{e.PropertyName}"}).ToList();
            return this;
        }
    }
}