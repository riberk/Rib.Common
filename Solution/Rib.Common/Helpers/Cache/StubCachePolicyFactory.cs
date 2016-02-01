namespace Rib.Common.Helpers.Cache
{
    using System.Runtime.Caching;
    using JetBrains.Annotations;

    internal class StubCachePolicyFactory : ICachePolicyFactory
    {
        [NotNull] private static readonly CacheItemPolicy Policy = new CacheItemPolicy();

        public CacheItemPolicy Create<T>() where T : class
        {
            return Policy;
        }
    }
}