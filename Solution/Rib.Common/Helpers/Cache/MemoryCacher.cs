namespace Rib.Common.Helpers.Cache
{
    using System;
    using System.Runtime.Caching;
    using JetBrains.Annotations;

    public class MemoryCacher
    {
        [NotNull] protected static readonly object EmptyObject = new object();

        public static event EventHandler<CacheUpdatedEventArgs> CacheItemUpdated;

        protected static void OnCacheItemUpdated(object invoker, [NotNull] CacheUpdatedEventArgs args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var h = CacheItemUpdated;
            h?.Invoke(invoker, args);
        }

        [NotNull]
        public static string FullKey(string prefix, string key)
        {
            return $"{prefix}|{key}";
        }
    }

    internal class MemoryCacher<T> : MemoryCacher, ICacher<T>
            where T : class
    {
        [NotNull] private readonly ICachePolicyFactory _cacheItemPolicyFactory;
        [NotNull] private readonly IObjectCacheFactory _objectCacheFactory;
        private readonly CacheItemPolicy _policy;
        [NotNull] private readonly string _prefix;

        public MemoryCacher([NotNull] ICachePolicyFactory cacheItemPolicyFactory,
                            [NotNull] IObjectCacheFactory objectCacheFactory,
                            [NotNull] string prefix,
                            CacheItemPolicy policy = null)
        {
            if (cacheItemPolicyFactory == null) throw new ArgumentNullException(nameof(cacheItemPolicyFactory));
            if (objectCacheFactory == null) throw new ArgumentNullException(nameof(objectCacheFactory));
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));
            _cacheItemPolicyFactory = cacheItemPolicyFactory;
            _objectCacheFactory = objectCacheFactory;
            _prefix = prefix;
            _policy = policy;
        }

        public T GetOrAdd(string key, Func<string, T> valueFactory)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));
            var cacheKey = FullKey(_prefix, key);
            var cache = _objectCacheFactory.Create();
            var res = cache.Get(cacheKey);
            if (res == null)
            {
                var factoryResult = valueFactory(key) ?? EmptyObject;
                res = cache.AddOrGetExisting(cacheKey, factoryResult, _policy ?? _cacheItemPolicyFactory.Create<T>()) ?? factoryResult;
            }
            if (res == EmptyObject)
            {
                return null;
            }
            var typedRes = res as T;
            if (typedRes == null)
            {
                throw new InvalidCastException($"Не удалось привести {res.GetType()} к {typeof (T)}");
            }
            return typedRes;
        }

        public T AddOrUpdate(string key, Func<string, T> createValueFactory)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (createValueFactory == null) throw new ArgumentNullException(nameof(createValueFactory));
            Remove(key);
            var res = GetOrAdd(key, createValueFactory);
            OnCacheItemUpdated(this, new CacheUpdatedEventArgs(FullKey(_prefix, key)));
            return res;
        }

        public void Remove(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            _objectCacheFactory.Create().Remove(FullKey(_prefix, key));
        }
    }
}