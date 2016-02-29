namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// Читает пользовательские атрибуты
    /// </summary>
    public interface IAttributesReader
    {
        /// <summary>
        /// Прочитать атрибуты
        /// </summary>
        /// <param name="attr">Тип атрибута</param>
        /// <param name="provider">Параметр</param>
        /// <returns>Атрибуты или пустая коллекция, если отсутствуют</returns>
        [NotNull, ItemNotNull]
        IReadOnlyCollection<object> ReadMany([NotNull] Type attr, [NotNull] ICustomAttributeProvider provider);
    }
}