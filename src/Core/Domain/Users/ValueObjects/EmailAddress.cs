namespace Domain.Users.ValueObjects
{
    public class EmailAddress
    {
        private EmailAddress(string email)
        {
            Email = email;
        }

        public string Email { get; }

        public static EmailAddress Create(string email)
        {
            return new EmailAddress(email);
        }
    }
}