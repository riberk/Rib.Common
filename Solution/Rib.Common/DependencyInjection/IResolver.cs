namespace Rib.Common.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

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

        IEnumerable<object> GetAll(Type type);
    }
}