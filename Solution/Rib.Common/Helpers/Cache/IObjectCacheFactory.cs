namespace Rib.Common.Helpers.Cache
{
    using System.Runtime.Caching;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ObjectCacheFactory))]
    public interface IObjectCacheFactory
    {
        [NotNull]
        ObjectCache Create();
    }
}