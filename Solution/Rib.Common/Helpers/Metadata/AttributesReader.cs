namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    internal class AttributesReader : IAttributesReader
    {
        [NotNull] private readonly ICacherFactory _cacherFactory;
        [NotNull] private readonly IAttributesReaderKeyFactory _attributesReaderKeyFactory;

        public AttributesReader([NotNull] ICacherFactory cacherFactory,
            [NotNull] IAttributesReaderKeyFactory attributesReaderKeyFactory)
        {
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            if (attributesReaderKeyFactory == null) throw new ArgumentNullException(nameof(attributesReaderKeyFactory));
            _cacherFactory = cacherFactory;
            _attributesReaderKeyFactory = attributesReaderKeyFactory;
        }

        /// <summary>
        /// Прочитать атрибуты
        /// </summary>
        /// <param name="attr">Тип атрибута</param>
        /// <param name="provider">Параметр</param>
        /// <returns>Атрибуты или пустая коллекция, если отсутствуют</returns>
        public IReadOnlyCollection<object> ReadMany(Type attr, ICustomAttributeProvider provider)
        {
            if (attr == null) throw new ArgumentNullException(nameof(attr));
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            var key = _attributesReaderKeyFactory.Create(attr, provider);
            var res = _cacherFactory.Create<IReadOnlyCollection<object>>().GetOrAdd(key, s => provider.GetCustomAttributes(attr, false));
            if (res == null)
            {
                throw new InvalidOperationException($"Get custom attributes returned null or null in cache for key {key}");
            }
            return res;
        }


        [BindTo(typeof(AttributesReaderKeyFactory))]
        internal interface IAttributesReaderKeyFactory
        {
            [NotNull]
            string Create([NotNull] Type attrType, [NotNull] ICustomAttributeProvider provider);
        }

        internal class AttributesReaderKeyFactory : IAttributesReaderKeyFactory
        {
            public string Create(Type attrType, ICustomAttributeProvider provider)
            {
                if (attrType == null) throw new ArgumentNullException(nameof(attrType));
                if (provider == null) throw new ArgumentNullException(nameof(provider));
                string name;
                var mi = provider as MemberInfo;
                var t = mi as Type;
                var p = provider as ParameterInfo;
                var a = provider as Assembly;
                var m = provider as System.Reflection.Module;
                if (t != null)
                {
                    name = t.FullName;
                }
                else if (p != null)
                {
                    name = $"{(p.Member.DeclaringType ?? p.Member.ReflectedType)?.FullName}|{p.Member.Name}|{p.Name}";
                }
                else if (mi != null)
                {
                    var decType = mi.DeclaringType ?? mi.ReflectedType;
                    name = $"{decType?.FullName}|{mi}";
                }
                else if (a != null)
                {
                    name = a.FullName;
                }
                else if (m != null)
                {
                    name = m.FullyQualifiedName;
                }
                else
                {
                    name = provider.ToString();
                }
                return $"{attrType.FullName}|{name}";
            }
        }
    }
}