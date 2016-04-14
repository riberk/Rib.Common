namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    /// <summary>
    ///     ������ ����, �������������� �� ���������� �����
    /// </summary>
    [BindTo(typeof(EnumFieldReader))]
    public interface IEnumFieldReader
    {
        /// <summary>
        ///     ��������� ���� �� �������� �����
        /// </summary>
        /// <typeparam name="TEnum">��� �����</typeparam>
        /// <param name="e">�������� �����</param>
        /// <returns>����</returns>
        [NotNull]
        FieldInfo Field<TEnum>(TEnum e) where TEnum : struct;

        /// <summary>
        ///     ��������� ���� �� ������������ �������� �����
        /// </summary>
        /// <param name="e">�������� �����</param>
        /// <returns>����</returns>
        [NotNull]
        FieldInfo Field(Enum e);
    }
}