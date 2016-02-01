namespace Rib.Common.Helpers.Cache
{
    using System.Runtime.Caching;
    using Rib.Common.Models.Metadata;
    using JetBrains.Annotations;

    [BindTo(typeof(StubCachePolicyFactory))]
    public interface ICachePolicyFactory
    {
        [NotNull]
        CacheItemPolicy Create<T>() where T : class;
    }
}