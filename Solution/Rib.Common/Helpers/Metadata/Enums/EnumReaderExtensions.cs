namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rib.Common.Models.Metadata;
    using JetBrains.Annotations;

    /// <summary>
    ///     Методы расшиерний для чтения енумов
    /// </summary>
    public static class EnumReaderExtensions
    {
        /// <summary>
        ///     Читает все значения енума в generic-модели
        /// </summary>
        /// <typeparam name="TEnum">Тип енума</typeparam>
        /// <param name="enumReader">Читатель енумов</param>
        /// <returns>Все значения енума</returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<IEnumModel<TEnum>> ReadMany<TEnum>([NotNull] this IEnumReader enumReader) where TEnum : struct
        {
            if (enumReader == null) throw new ArgumentNullException(nameof(enumReader));
            return Enum.GetValues(typeof (TEnum)).Cast<TEnum>().Select(enumReader.Read);
        }

        /// <summary>
        ///     Читает все значения енума в модели
        /// </summary>
        /// <param name="enumReader">Читатель енумов</param>
        /// <param name="enumType">Тип енума</param>
        /// <returns>Все значения енума</returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<IEnumModel> ReadMany([NotNull] this IEnumReader enumReader, [NotNull] Type enumType)
        {
            if (enumReader == null) throw new ArgumentNullException(nameof(enumReader));
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));
            return Enum.GetValues(enumType).Cast<Enum>().Select(enumReader.Read);
        }
    }
}