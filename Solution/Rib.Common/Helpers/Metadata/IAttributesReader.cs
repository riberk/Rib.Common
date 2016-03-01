namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    /// <summary>
    /// ������ ���������������� ��������
    /// </summary>
    [BindTo(typeof(AttributesReader))]
    public interface IAttributesReader
    {
        /// <summary>
        /// ��������� ��������
        /// </summary>
        /// <param name="attr">��� ��������</param>
        /// <param name="provider">��������</param>
        /// <returns>�������� ��� ������ ���������, ���� �����������</returns>
        [NotNull, ItemNotNull]
        IReadOnlyCollection<object> ReadMany([NotNull] Type attr, [NotNull] ICustomAttributeProvider provider);
    }
}