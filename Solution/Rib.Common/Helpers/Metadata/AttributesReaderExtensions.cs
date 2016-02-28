namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Rib.Common.Models.Exceptions;
    using JetBrains.Annotations;

    public static class AttributesReaderExtensions
    {
        [NotNull]
        public static T ReadSafe<T>([NotNull] this IAttributesReader attributesReader, [NotNull] ICustomAttributeProvider t) where T : Attribute
        {
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            if (t == null) throw new ArgumentNullException(nameof(t));
            var res = attributesReader.Read<T>(t);
            if (res == null)
            {
                var type = typeof (T);
                throw new AttributeNotFoundException($"�� ������� ����������� ��������� {type} �� {t}", type, t);
            }
            return res;
        }

        /// <summary>
        /// ��������� ���� �������
        /// </summary>
        /// <typeparam name="T">��� ��������</typeparam>
        /// <param name="reader">����� ���������</param>
        /// <param name="t">��������</param>
        /// <returns>������� ��� null � ������ ��� ����������</returns>
        public static T Read<T>([NotNull] this IAttributesReader reader, [NotNull] ICustomAttributeProvider t) where T : Attribute
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var res = reader.Read(typeof (T), t);
            return res != null ? Cast<T>(res): null;
        }

        [NotNull]
        private static T Cast<T>([NotNull] object t) where T : Attribute
        {
            var typedAttr = t as T;
            if (typedAttr == null)
            {
                throw new InvalidCastException($"Impossible cast {t.GetType()} to {typeof(T)}");
            }
            return typedAttr;
        }

        /// <summary>
        /// ��������� ��������
        /// </summary>
        /// <typeparam name="T">��� ��������</typeparam>
        /// <param name="reader"></param>
        /// <param name="t">��������</param>
        /// <returns>�������� ��� ������ ���������, ���� �����������</returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<T> ReadMany<T>([NotNull] this IAttributesReader reader, [NotNull] ICustomAttributeProvider t)
            where T : Attribute
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            return reader.ReadMany(typeof (T), t).Select(Cast<T>);
        }

        /// <summary>
        /// ��������� ���� �������
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attr">��� ��������</param>
        /// <param name="provider">��������</param>
        /// <returns>������� ��� null � ������ ��� ����������</returns>
        public static object Read([NotNull] this IAttributesReader reader, [NotNull] Type attr, [NotNull] ICustomAttributeProvider provider)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            return reader.ReadMany(attr, provider).SingleOrDefault();
        }
    }
}