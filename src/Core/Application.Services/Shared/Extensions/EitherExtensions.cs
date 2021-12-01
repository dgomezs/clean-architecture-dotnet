using System.Threading.Tasks;
using Domain.Shared.Errors;
using LanguageExt;

namespace Application.Services.Shared.Extensions
{
    public static class EitherExtensions
    {
        public static T ToThrowException<T>(this Either<Error, T> result)
        {
            return result.Match(r => r, e => throw new DomainException(e));
        }

        public static async Task<T> ToThrowException<T>(this Task<Either<Error, T>> task)
        {
            var result = await task;
            return ToThrowException(result);
        }

        public static T ToThrowException<T>(this Validation<Error, T> result)
        {
            return result.Match(r => r, errors => errors.Case switch
            {
                null => throw new DomainException(ErrorCodes.UnexpectedError),
                Error head => throw new DomainException(head),
                (Error head, Seq<Error> tail) => throw new DomainException(head, tail),
                _ => throw new DomainException(ErrorCodes.UnexpectedError)
            });
        }
    }
}