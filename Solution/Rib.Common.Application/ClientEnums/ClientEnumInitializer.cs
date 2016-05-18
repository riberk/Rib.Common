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

        public void Initialize(params object[] obj)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (obj != null)
            {
                assemblies = assemblies.Union(obj.Where(o => o != null).Select(o => o.GetType().Assembly));
            }
            foreach (var assembly in assemblies)
            {
                _store.Add(assembly);
            }
            _store.Close();
        }
    }
}