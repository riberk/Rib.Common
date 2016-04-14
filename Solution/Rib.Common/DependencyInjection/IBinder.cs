namespace Rib.Common.DependencyInjection
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Rib.Common.Models.Binding;

    public interface IBinder
    {
        void Bind([NotNull, ItemNotNull] IEnumerable<IBindInfo> bindings);
    }
}