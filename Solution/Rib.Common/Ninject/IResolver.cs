namespace Rib.Common.Ninject
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(NinjectResolver))]
    public interface IResolver
    {
        [NotNull]
        T Get<T>(string name = null);

        [CanBeNull]
        T TryGet<T>(string name = null);

        [NotNull]
        object Get(Type t, string name = null);

        [CanBeNull]
        object TryGet(Type t, string name = null);
    }
}