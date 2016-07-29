namespace Rib.Common.Application.ClientEnums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Metadata.ClientEnums;

    internal class ClientEnumInitializer : IClientEnumInitializer
    {
        [NotNull]
        private readonly IClientEnumAssemblyStore _store;

        public ClientEnumInitializer([NotNull] IClientEnumAssemblyStore store)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));
            _store = store;
        }

        public void Initialize(params Type[] types)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (types != null)
            {
                assemblies = assemblies.Union(types.Where(t => t != null).Select(t => t.Assembly));
            }
            foreach (var assembly in assemblies)
            {
                _store.Add(assembly);
            }
            _store.Close();
        }
    }
}