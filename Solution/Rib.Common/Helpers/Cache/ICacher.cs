namespace Rib.Common.Helpers.Cache
{
    using System;
    using JetBrains.Annotations;

    public interface ICacher<T>
        where T : class
    {
        CacheTryGetResult TryGet([NotNull] string key, out T value);
        T GetOrAdd([NotNull] string key, [NotNull] Func<string, T> valueFactory);
        T AddOrUpdate(string key, Func<string, T> createValueFactory);
        void Remove(string key);
    }
}