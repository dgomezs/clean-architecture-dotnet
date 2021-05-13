using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestCategories
{
    public class IntegrationTestDiscoverer : ITraitDiscoverer
    {
        internal const string DiscovererTypeName =
            DiscovererUtil.AssemblyName + "." + nameof(IntegrationTestDiscoverer);

        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>("Category", "IntegrationTest");
        }
    }
}