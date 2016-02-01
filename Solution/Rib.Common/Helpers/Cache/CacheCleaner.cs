namespace Rib.Common.Helpers.Cache
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;

    public class CacheCleaner : ICacheCleaner
    {
        [NotNull] private readonly IObjectCacheFactory _objectCacheFactory;

        public CacheCleaner([NotNull] IObjectCacheFactory objectCacheFactory)
        {
            if (objectCacheFactory == null) throw new ArgumentNullException(nameof(objectCacheFactory));
            _objectCacheFactory = objectCacheFactory;
        }

        public void Clean()
        {
            var cache = _objectCacheFactory.Create();
            cache.AsParallel().Select(x => cache.Remove(x.Key)).ToList();
        }

        /// <summary>
        ///     Очистить итем кеша по имени
        /// </summary>
        /// <param name="fullName">Полный ключ итема</param>
        public void Clean(string fullName)
        {
            if (fullName == null) throw new ArgumentNullException(nameof(fullName));
            var cache = _objectCacheFactory.Create();
            cache.Remove(fullName);
        }
    }
}