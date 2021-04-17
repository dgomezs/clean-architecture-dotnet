namespace Domain.Users.ValueObjects
{
    public record PersonName
    {
        private PersonName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string LastName { get; }

        public string FirstName { get; }

        public static PersonName Create(string firstName, string lastName)
        {
            return new(firstName, lastName);
        }
    }
}