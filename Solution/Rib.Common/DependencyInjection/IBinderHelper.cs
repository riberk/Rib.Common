namespace Rib.Common.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Rib.Common.Models.Binding;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(BinderHelper))]
    public interface IBinderHelper
    {
        [NotNull, ItemNotNull]
        IEnumerable<IBindInfo> ReadFromTypes([NotNull] IReadOnlyCollection<Type> types, string defaultScope = BindingScope.Transient);
    }
}