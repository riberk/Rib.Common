namespace Rib.Common.Application.Wrappers
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;
    using TsSoft.ContextWrapper;

    [BindTo(typeof(WrappersFactory))]
    public interface IWrappersFactory
    {
        [NotNull]
        IItemWrapper Get([NotNull] Type type);
    }
}