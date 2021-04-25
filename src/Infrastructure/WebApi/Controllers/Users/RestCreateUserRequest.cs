namespace WebApi.Controllers.Users
{
    public record RestCreateUserRequest(string FirstName, string LastName, string Email)
    {
    }
}