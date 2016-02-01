namespace Rib.Common.Helpers.Cache
{
    using System.Runtime.Caching;
    using Rib.Common.Models.Metadata;
    using JetBrains.Annotations;

    [BindTo(typeof(MemoryCacherFactory))]
    public interface ICacherFactory
    {
        [NotNull]
        ICacher<T> Create<T>(string prefix = null, CacheItemPolicy policy = null) where T : class;
    }
}