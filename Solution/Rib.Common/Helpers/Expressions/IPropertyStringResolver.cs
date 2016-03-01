namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    /// <summary>
    ///     Получатель свойств из строкового пути
    /// </summary>
    [BindTo(typeof(PropertyStringResolver))]
    public interface IPropertyStringResolver
    {
        /// <summary>
        ///     Получить свойства на основе пути к конечному свойству
        /// </summary>
        /// <param name="currentType">Тип</param>
        /// <param name="propPath">Путь к конечному свойству вида paramName.Prop1.Prop2.Prop3</param>
        /// <returns>Перечисление <see cref="PropertyInfo" /></returns>
        [NotNull]
        IEnumerable<PropertyInfo> GetProperties([NotNull] Type currentType, string propPath);
    }
}