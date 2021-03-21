using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Domain.Errors
{
    public class DomainValidationException : DomainException
    {
        public DomainValidationException(string errorKey, IEnumerable<ValidationFailure> errors) :
            base(errorKey, errors.Select(e =>
                new Error(e.ErrorCode, e.PropertyName, e.ErrorMessage)))
        {
        }

        public DomainValidationException(string errorKey, string message) : base(errorKey, message)
        {
        }

        public DomainValidationException PrefixErrors(string prefix)
        {
            _errors = _errors.Select(e => e with {PropertyName = $"{prefix}.{e.PropertyName}"}).ToList();
            return this;
        }
    }
}