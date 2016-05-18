namespace Rib.Common.Application.Wrappers
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public interface IWrapperTypesResolver
    {
        [NotNull, ItemNotNull]
        IReadOnlyCollection<Type> Resolve();
    }
}