namespace Rib.Common.Helpers.Cache
{
    using System.Runtime.Caching;

    internal class ObjectCacheFactory : IObjectCacheFactory
    {
        public ObjectCache Create()
        {
            return MemoryCache.Default;
        }
    }
}