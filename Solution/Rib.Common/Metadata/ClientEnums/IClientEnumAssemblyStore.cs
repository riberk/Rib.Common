namespace Rib.Common.Metadata.ClientEnums
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof (ClientEnumAssemblyStore))]
    public interface IClientEnumAssemblyStore
    {
        [NotNull]
        IReadOnlyCollection<Assembly> Assemblies { get; }

        bool IsClosed { get; }
        void Add([NotNull] Type assemblyType);
        void Add([NotNull] Assembly assembly);
        void Close();
    }
}