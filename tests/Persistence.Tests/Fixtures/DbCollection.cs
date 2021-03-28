using Xunit;

namespace Persistence.Tests.Fixtures
{
    [CollectionDefinition("DB")]
    public class DbCollection : ICollectionFixture<DbFixture>
    {
    }
}