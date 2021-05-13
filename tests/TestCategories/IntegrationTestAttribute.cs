using System;
using Xunit.Sdk;

namespace TestCategories
{
    [TraitDiscoverer(IntegrationTestDiscoverer.DiscovererTypeName, DiscovererUtil.AssemblyName)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class IntegrationTestAttribute : Attribute, ITraitAttribute
    {
    }
}