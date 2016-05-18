namespace Rib.Common.Metadata.ClientEnums
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;

    internal class ClientEnumAssembliesResolver : IClientEnumAssembliesResolver
    {
        [NotNull] private readonly IClientEnumAssemblyStore _clientEnumAssemblyStore;

        public ClientEnumAssembliesResolver([NotNull] IClientEnumAssemblyStore clientEnumAssemblyStore)
        {
            if (clientEnumAssemblyStore == null) throw new ArgumentNullException(nameof(clientEnumAssemblyStore));
            _clientEnumAssemblyStore = clientEnumAssemblyStore;
        }

        public IReadOnlyCollection<Assembly> Resolve()
        {
            return _clientEnumAssemblyStore.Assemblies;
        }
    }
}