using Domain.Users.ValueObjects;
using FluentValidation;

namespace WebApi.Controllers.Users
{
    public record RestCreateUserRequest(string FirstName, string LastName, string Email)
    {
    }


    public class RestCreateUserRequestValidator : AbstractValidator<RestCreateUserRequest>
    {
        public RestCreateUserRequestValidator()
        
        {
        }
    }
}