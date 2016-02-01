namespace Rib.Common.Helpers.Cache
{
    using System.Runtime.Caching;
    using JetBrains.Annotations;

    public interface IObjectCacheFactory
    {
        [NotNull]
        ObjectCache Create();
    }
}