namespace Rib.Common.DependencyInjection
{
    using System.Collections.Generic;
    using Rib.Common.Models.Binding;

    public interface IBinder
    {
        void Bind(IEnumerable<IBindInfo> bindings);
    }
}