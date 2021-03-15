using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Domain.Errors
{
    public class DomainValidationException : DomainException
    {
        private IList<DomainValidationFailure> _errors;

        public IEnumerable<DomainValidationFailure> Errors => _errors;

        public DomainValidationException(string errorKey, IEnumerable<ValidationFailure> errors) :
            base(errorKey) =>
            _errors = errors.Select(e =>
                new DomainValidationFailure(e.ErrorCode, e.PropertyName, e.ErrorMessage)).ToList();

        public DomainValidationException(string errorKey, string errorMessage,
            IEnumerable<DomainValidationFailure> errors) : base(errorKey, errorMessage) =>
            _errors = errors.ToList();

        public DomainValidationException(string errorKey, string errorMessage,
            IEnumerable<ValidationFailure> errors) : base(errorKey, errorMessage) =>
            _errors = errors.Select(e =>
                new DomainValidationFailure(e.ErrorCode, e.PropertyName, e.ErrorMessage)).ToList();

        public DomainValidationException PrefixErrors(string prefix)
        {
            _errors = _errors.Select(e => e with {PropertyName = $"{prefix}.{e.PropertyName}"}).ToList();
            return this;
        }
    }
}