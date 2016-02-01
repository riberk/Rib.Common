namespace Rib.Common.Helpers.Cache
{
    using System;
    using JetBrains.Annotations;

    public interface ICacher<T>
        where T : class
    {
        T GetOrAdd([NotNull] string key, [NotNull] Func<string, T> valueFactory);
        T AddOrUpdate(string key, Func<string, T> createValueFactory);
        void Remove(string key);
    }
}