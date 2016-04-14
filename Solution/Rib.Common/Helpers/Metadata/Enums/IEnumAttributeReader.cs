namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    /// <summary>
    ///     Помогает решать задачи, связанные с перечислениями
    /// </summary>
    [BindTo(typeof(EnumAttributeReader))]
    public interface IEnumAttributeReader
    {
        /// <summary>
        ///     Получить атрибут с забоксированного значения перечисления
        /// </summary>
        /// <typeparam name="TAttribute">Тип атрибута, который следует получить</typeparam>
        /// <param name="e">Значение енума</param>
        /// <returns>Атрибут или null при его отсктствии</returns>
        [CanBeNull]
        TAttribute Attribute<TAttribute>(Enum e) where TAttribute : Attribute;

        /// <summary>
        ///     Получить атрибут со значения перечисления
        /// </summary>
        /// <typeparam name="TAttribute">Тип атрибута, который следует получить</typeparam>
        /// <typeparam name="TEnum">Тип перечисления</typeparam>
        /// <param name="e">Значение енума</param>
        /// <returns>Атрибут или null при его отсктствии</returns>
        [CanBeNull]
        TAttribute Attribute<TAttribute, TEnum>(TEnum e) where TAttribute : Attribute where TEnum : struct;

        /// <summary>
        ///     Получить атрибут с забоксированного значения перечисления
        ///     если атрибут не прописан - упасть
        /// </summary>
        /// <typeparam name="TAttribute">Тип атрибута, который следует получить</typeparam>
        /// <param name="e">Значение енума</param>
        /// <returns>Атрибут</returns>
        [NotNull]
        TAttribute AttributeSafe<TAttribute>([NotNull] Enum e) where TAttribute : Attribute;

        /// <summary>
        ///     Получить атрибут со значения перечисления
        ///     если атрибут не прописан - упасть
        /// </summary>
        /// <typeparam name="TAttribute">Тип атрибута, который следует получить</typeparam>
        /// <typeparam name="TEnum">Тип перечисления</typeparam>
        /// <param name="e">Значение енума</param>
        /// <returns>Атрибут</returns>
        [NotNull]
        TAttribute AttributeSafe<TAttribute, TEnum>(TEnum e) where TAttribute : Attribute where TEnum : struct;
    }
}