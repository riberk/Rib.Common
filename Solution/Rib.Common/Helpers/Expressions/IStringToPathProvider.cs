namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    ///     ѕреобразователь строковых значений в свойства дл€ типа
    /// </summary>
    public interface IStringToPathProvider
    {
        /// <summary>
        ///     ѕолучить <see cref="PropertyInfo" /> дл€ типа,
        ///     у которых имена совпадают со строками в перечислении в виде Prop1.Prop2.Prop3
        ///     (т.е. свойства достаютс€ последовательно из типов)
        /// </summary>
        /// <param name="currentType">“ип</param>
        /// <param name="propNames">ѕеречисление имен свойств</param>
        /// <returns>ѕеречисление <see cref="PropertyInfo" /></returns>
        [NotNull, ItemNotNull]
        IEnumerable<PropertyInfo> GetProperties([NotNull] Type currentType, [NotNull] IEnumerable<string> propNames);
    }
}