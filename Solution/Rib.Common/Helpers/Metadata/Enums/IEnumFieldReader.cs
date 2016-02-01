namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    ///     Читает поля, сопоставленные со значениями енума
    /// </summary>
    public interface IEnumFieldReader
    {
        /// <summary>
        ///     Прочитать поле по значению енума
        /// </summary>
        /// <typeparam name="TEnum">Тип енума</typeparam>
        /// <param name="e">Значение енума</param>
        /// <returns>Поле</returns>
        [NotNull]
        FieldInfo Field<TEnum>(TEnum e) where TEnum : struct;

        /// <summary>
        ///     Прочитать поле по забоксенному значению енума
        /// </summary>
        /// <param name="e">Значение енума</param>
        /// <returns>Поле</returns>
        [NotNull]
        FieldInfo Field(Enum e);
    }
}