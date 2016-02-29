namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    /// <summary>
    ///     ������ ��-�� ����������� ������������������� ��������
    /// </summary>
    public class AttributeException : MetadataException
    {
        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="AttributeException" />.
        /// </summary>
        /// <param name="type">��� ��������</param>
        public AttributeException(Type type)
        {
            Type = type;
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="AttributeException" /> � ��������� ���������� ��
        ///     ������.
        /// </summary>
        /// <param name="message">���������, ����������� ������. </param>
        /// <param name="type">��� ��������</param>
        public AttributeException(string message, Type type) : base(message)
        {
            Type = type;
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="AttributeException" /> � ��������� ���������� �� ������
        ///     � ������� �� ���������� ����������, ��������� ��� ����������.
        /// </summary>
        /// <param name="message">��������� �� ������ � ����������� ������ ����������. </param>
        /// <param name="innerException">
        ///     ����������, ������� �������� �������� �������� ����������.���� ��������
        ///     <paramref name="innerException" /> �� �������� ���������� null, ������� ���������� ��������� � ����� catch,
        ///     �������������� ���������� ����������.
        /// </param>
        /// <param name="type">��� ��������</param>
        public AttributeException(string message, Exception innerException, Type type) : base(message, innerException)
        {
            Type = type;
        }

        /// <summary>
        ///     ��� ��������
        /// </summary>
        public Type Type { get; }
    }
}