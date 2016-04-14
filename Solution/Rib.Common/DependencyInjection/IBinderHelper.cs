namespace Rib.Common.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Rib.Common.Models.Binding;

    public interface IBinderHelper
    {
        [NotNull, ItemNotNull]
        IEnumerable<IBindInfo> ReadFromTypes([NotNull] IReadOnlyCollection<Type> types);
    }
}