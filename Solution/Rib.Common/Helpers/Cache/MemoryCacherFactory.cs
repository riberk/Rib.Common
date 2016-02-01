namespace Rib.Common.Helpers.Cache
{
    using System;
    using System.Runtime.Caching;
    using JetBrains.Annotations;

    internal class MemoryCacherFactory : ICacherFactory
    {
        [NotNull] private readonly ICachePolicyFactory _cachePolicyFactory;
        [NotNull] private readonly IObjectCacheFactory _objectCacheFactory;

        public MemoryCacherFactory([NotNull] ICachePolicyFactory cachePolicyFactory, [NotNull] IObjectCacheFactory objectCacheFactory)
        {
            if (cachePolicyFactory == null) throw new ArgumentNullException(nameof(cachePolicyFactory));
            if (objectCacheFactory == null) throw new ArgumentNullException(nameof(objectCacheFactory));
            _cachePolicyFactory = cachePolicyFactory;
            _objectCacheFactory = objectCacheFactory;
        }

        public ICacher<T> Create<T>(string prefix = null, CacheItemPolicy policy = null) where T : class
        {
            if (prefix == null)
            {
                prefix = $"{typeof (T).FullName}";
            }
            return new MemoryCacher<T>(_cachePolicyFactory, _objectCacheFactory, prefix, policy);
        }
    }
}