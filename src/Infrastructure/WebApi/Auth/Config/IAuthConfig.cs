namespace WebApi.Auth.Config
{
    public interface IAuthConfig
    {
        public string GetIssuer();
        public string GetAudience();
        
    }
}