namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using Rib.Common.Models.Metadata;
    using JetBrains.Annotations;

    /// <summary>
    ///     Читатель енумов
    /// </summary>
    public interface IEnumReader
    {
        /// <summary>
        ///     Прочитать забоксенное значение енума в модель
        /// </summary>
        /// <param name="e">Значение енума</param>
        /// <returns>Модель значения енума</returns>
        [NotNull]
        IEnumModel Read(Enum e);

        /// <summary>
        ///     Прочитать значение енума в модель
        /// </summary>
        /// <typeparam name="TEnum">Тип енума</typeparam>
        /// <param name="e">Значение енума</param>
        /// <returns>Generic-модель значения</returns>
        [NotNull]
        IEnumModel<TEnum> Read<TEnum>(TEnum e) where TEnum : struct;
    }
}