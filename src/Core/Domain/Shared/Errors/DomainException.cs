﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using FluentValidation.Results;
using LanguageExt;

namespace Domain.Shared.Errors
{
    public class DomainException : Exception
    {
        public readonly Error MainError;
        private List<Error> _errors = new();

        public DomainException(Error mainError) : base(mainError.Message) =>
            MainError = Guard.Against.Null(mainError, nameof(mainError));

        public DomainException(string errorKey, IEnumerable<ValidationFailure> errors)
        {
            MainError = new ValidationError(errorKey);
            _errors = Guard.Against.Null(errors, nameof(errors))
                .Select(x => new ValidationError(x)).ToList<Error>();
        }

        public DomainException() => MainError = new Error(ErrorCodes.UnexpectedError);

        public DomainException(string errorKey) =>
            MainError = new Error(errorKey);

        public DomainException(Error mainError, IEnumerable<Error> errors) : base(mainError.Message)
            => (MainError, _errors) = (mainError, errors.ToList());


        public string ErrorKey => MainError.Code;

        public IEnumerable<Error> Errors => _errors;

        public override string Message => MainError.Message;

        public override IDictionary Data => MainError.Data;


        public static DomainException FromSeqErrors(Seq<Error> errors)
        {
            return errors.Case switch
            {
                null => new DomainException(),
                Error e => new DomainException(e),
                (Error e, Seq<Error> rest) => throw new DomainException(e, rest),
                _ => new DomainException()
            };
        }

        public DomainException PrefixErrors(string prefix)
        {
            _errors = _errors.Select(e => e with {PropertyName = $"{prefix}.{e.PropertyName}"}).ToList();
            return this;
        }
    }
}