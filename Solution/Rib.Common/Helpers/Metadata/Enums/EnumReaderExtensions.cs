namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rib.Common.Models.Metadata;
    using JetBrains.Annotations;

    /// <summary>
    ///     ������ ���������� ��� ������ ������
    /// </summary>
    public static class EnumReaderExtensions
    {
        /// <summary>
        ///     ������ ��� �������� ����� � generic-������
        /// </summary>
        /// <typeparam name="TEnum">��� �����</typeparam>
        /// <param name="enumReader">�������� ������</param>
        /// <returns>��� �������� �����</returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<IEnumModel<TEnum>> ReadMany<TEnum>([NotNull] this IEnumReader enumReader) where TEnum : struct
        {
            if (enumReader == null) throw new ArgumentNullException(nameof(enumReader));
            return Enum.GetValues(typeof (TEnum)).Cast<TEnum>().Select(enumReader.Read);
        }

        /// <summary>
        ///     ������ ��� �������� ����� � ������
        /// </summary>
        /// <param name="enumReader">�������� ������</param>
        /// <param name="enumType">��� �����</param>
        /// <returns>��� �������� �����</returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<IEnumModel> ReadMany([NotNull] this IEnumReader enumReader, [NotNull] Type enumType)
        {
            if (enumReader == null) throw new ArgumentNullException(nameof(enumReader));
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));
            return Enum.GetValues(enumType).Cast<Enum>().Select(enumReader.Read);
        }
    }
}