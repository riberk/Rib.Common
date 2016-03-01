namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    /// <summary>
    /// Читает пользовательские атрибуты
    /// </summary>
    [BindTo(typeof(AttributesReader))]
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