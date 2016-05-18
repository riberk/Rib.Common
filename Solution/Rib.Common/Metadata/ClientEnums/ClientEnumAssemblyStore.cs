namespace Rib.Common.Metadata.ClientEnums
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;

    internal class ClientEnumAssemblyStore : IClientEnumAssemblyStore
    {
        [NotNull] private readonly HashSet<Assembly> _assemblies;

        public ClientEnumAssemblyStore()
        {
            _assemblies = new HashSet<Assembly>();
        }

        public void Add(Type assemblyType)
        {
            Add(assemblyType.Assembly);
        }

        public void Add(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            EnsureUnclosed();
            _assemblies.Add(assembly);
        }

        public IReadOnlyCollection<Assembly> Assemblies
        {
            get
            {
                EnsureClosed();
                return _assemblies;
            }
        }

        public bool IsClosed { get; private set; }

        public void Close()
        {
            IsClosed = true;
        }

        private void EnsureUnclosed()
        {
            if (IsClosed)
            {
                throw new InvalidOperationException("Store closed");
            }
        }

        private void EnsureClosed()
        {
            if (!IsClosed)
            {
                throw new InvalidOperationException("Store is unclosed. Call close");
            }
        }
    }
}