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
                EmptyCase<Error> => throw new DomainException(ErrorCodes.UnexpectedError),
                HeadCase<Error> headCase => throw new DomainException(headCase.Head),
                HeadTailCase<Error> headTailCase => throw new DomainException(headTailCase.Head,
                    headTailCase.Tail),
                _ => throw new DomainException(ErrorCodes.UnexpectedError)
            });
        }
    }
}