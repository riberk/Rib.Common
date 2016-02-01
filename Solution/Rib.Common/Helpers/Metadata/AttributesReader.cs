namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using JetBrains.Annotations;

    internal class AttributesReader : IAttributesReader
    {
        [NotNull] private readonly ICacherFactory _cacherFactory;

        public AttributesReader([NotNull] ICacherFactory cacherFactory)
        {
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _cacherFactory = cacherFactory;
        }

        public T Read<T>(MemberInfo t) where T : Attribute
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return _cacherFactory.Create<T>().GetOrAdd(Key<T>(t), s => t.GetCustomAttribute<T>());
        }

        public IReadOnlyCollection<T> ReadMany<T>(MemberInfo t) where T : Attribute
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return _cacherFactory.Create<IReadOnlyCollection<T>>().GetOrAdd(Key<IReadOnlyCollection<T>>(t), s => t.GetCustomAttributes<T>().ToArray());
        }

        [NotNull]
        private static string Key<T>([NotNull] MemberInfo mi)
        {
            string name;
            var t = mi as Type;
            if (t != null)
            {
                name = t.FullName;
            }
            else
            {
                var decType = mi?.DeclaringType ?? mi.ReflectedType;
                name = $"{decType?.FullName}|{mi.Name}";
            }
            return $"{typeof (T).FullName}|{name}";
        }
    }
}