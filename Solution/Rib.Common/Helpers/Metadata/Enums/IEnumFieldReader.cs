namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    ///     ������ ����, �������������� �� ���������� �����
    /// </summary>
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