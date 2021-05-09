using Bogus;

namespace FakeTestData
{
    public static class StringFakeData
    {
        public static string GenerateRandomString(int length)
        {
            return new Faker().Random.AlphaNumeric(length);
        }
    }
}