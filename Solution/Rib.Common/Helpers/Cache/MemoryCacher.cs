namespace Rib.Common.Helpers.Cache
{
    using System;
    using System.Runtime.Caching;
    using JetBrains.Annotations;

    public class MemoryCacher
    {
        [NotNull] protected static readonly object EmptyObject = new object();

        public static event EventHandler<CacheEventArgs> CacheItemRemoved;
        public static event EventHandler<CacheEventArgs> CacheItemAdded;

        protected static void OnCacheItemAdd(object invoker, [NotNull] CacheEventArgs args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var h = CacheItemAdded;
            h?.Invoke(invoker, args);
        }


        protected static void OnCacheItemRemove(object invoker, [NotNull] CacheEventArgs args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var h = CacheItemRemoved;
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

        public CacheTryGetResult TryGet(string key, out T value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var internalResult = GetResult(key);
            value = default(T);
            if (internalResult.Value == null)
            {
                return CacheTryGetResult.NotFound;
            }
            if (internalResult.Value == EmptyObject)
            {
                return CacheTryGetResult.Empty;
            }
            value = Cast(internalResult.Value);
            return CacheTryGetResult.Found;
        }

        public T GetOrAdd(string key, Func<string, T> valueFactory)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));
            var internalResult = GetResult(key);
            if (internalResult.Value == null)
            {
                var factoryResult = valueFactory(key) ?? EmptyObject;
                internalResult.Value =
                    internalResult.Cache.AddOrGetExisting(internalResult.FullKey, factoryResult, _policy ?? _cacheItemPolicyFactory.Create<T>()) ??
                    factoryResult;
                OnCacheItemAdd(this, new CacheEventArgs(internalResult.FullKey));
            }
            return internalResult.Value == EmptyObject ? null : Cast(internalResult.Value);
        }

        [NotNull]
        private static T Cast([NotNull] object value)
        {
            var typedRes = value as T;
            if (typedRes == null)
            {
                throw new InvalidCastException($"Не удалось привести {value.GetType()} к {typeof(T)}");
            }
            return typedRes;
        }

        private InternalCacheResult GetResult(string key)
        {
            var cacheKey = FullKey(_prefix, key);
            var cache = _objectCacheFactory.Create();
            var res = cache.Get(cacheKey);
            return new InternalCacheResult(cacheKey, res, cache);
        }

        public void Remove(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var fullKey = FullKey(_prefix, key);
            _objectCacheFactory.Create().Remove(fullKey);
            OnCacheItemRemove(this, new CacheEventArgs(fullKey));
        }

        private struct InternalCacheResult
        {
            /// <summary>
            /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
            /// </summary>
            public InternalCacheResult([NotNull] string fullKey, object value, [NotNull] ObjectCache cache)
            {
                FullKey = fullKey;
                Value = value;
                Cache = cache;
            }

            [NotNull]
            public string FullKey { get; }

            public object Value { get; set; }

            [NotNull]
            public ObjectCache Cache { get; }
        }
    }
}