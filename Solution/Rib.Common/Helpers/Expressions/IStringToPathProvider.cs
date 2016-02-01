namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    ///     ��������������� ��������� �������� � �������� ��� ����
    /// </summary>
    public interface IStringToPathProvider
    {
        /// <summary>
        ///     �������� <see cref="PropertyInfo" /> ��� ����,
        ///     � ������� ����� ��������� �� �������� � ������������ � ���� Prop1.Prop2.Prop3
        ///     (�.�. �������� ��������� ��������������� �� �����)
        /// </summary>
        /// <param name="currentType">���</param>
        /// <param name="propNames">������������ ���� �������</param>
        /// <returns>������������ <see cref="PropertyInfo" /></returns>
        [NotNull, ItemNotNull]
        IEnumerable<PropertyInfo> GetProperties([NotNull] Type currentType, [NotNull] IEnumerable<string> propNames);
    }
}