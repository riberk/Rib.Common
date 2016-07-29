namespace Rib.Common.Helpers.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;

    public static class TypeExtensions
    {
        [NotNull]
        public static IEnumerable<FieldInfo> Constants([NotNull] this Type t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(f => f.IsLiteral && !f.IsInitOnly);
        }
    }
}