namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class ConfigurationItemsHelper : IConfigurationItemsHelper
    {
        public IEnumerable<Type> GroupedTypes(Type t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t.GetNestedTypes().Where(x => x != null && x.IsAbstract && x.IsSealed);
        }

        public IEnumerable<FieldInfo> Items(Type t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(fi => fi != null && fi.IsInitOnly && typeof (ConfigurationItem).IsAssignableFrom(fi.FieldType));
        }

        public FieldInfo Item(Type t, string name)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            if (name == null) throw new ArgumentNullException(nameof(name));
            var f = t.GetField(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            if (!IsReadOnlyField(f))
            {
                throw new InvalidOperationException($"Field {name} on type {t} could not be found or it is not readonly static field");
            }
            return f;
        }

        private static bool IsReadOnlyField(FieldInfo fi)
        {
            return fi != null && fi.IsInitOnly && typeof (ConfigurationItem).IsAssignableFrom(fi.FieldType);
        }
    }
}