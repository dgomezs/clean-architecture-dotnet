namespace WebApi.Authorization
{
    public interface IAuthConfig
    {
        public string GetIssuer();
        public string GetAudience();
    }
}