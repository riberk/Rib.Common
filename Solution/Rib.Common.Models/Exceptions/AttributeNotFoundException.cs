namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    /// <summary>
    ///     ������, ����������� ��-�� ��������� ��������
    /// </summary>
    public class AttributeNotFoundException : AttributeException
    {
        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="AttributeException" />.
        /// </summary>
        /// <param name="type">��� ��������</param>
        /// <param name="provider">����, �� ������� �������� �������</param>
        public AttributeNotFoundException(Type type, ICustomAttributeProvider provider) : base(type)
        {
            Provider = provider;
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="AttributeException" /> � ��������� ���������� ��
        ///     ������.
        /// </summary>
        /// <param name="message">���������, ����������� ������. </param>
        /// <param name="type">��� ��������</param>
        /// <param name="provider">����, �� ������� �������� �������</param>
        public AttributeNotFoundException(string message, Type type, ICustomAttributeProvider provider) : base(message, type)
        {
            Provider = provider;
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
        /// <param name="provider">����, �� ������� �������� �������</param>
        public AttributeNotFoundException(string message, Exception innerException, Type type, ICustomAttributeProvider provider)
            : base(message, innerException, type)
        {
            Provider = provider;
        }

        /// <summary>
        ///     ����, �� ������� �������� �������
        /// </summary>
        public ICustomAttributeProvider Provider { get; }
    }
}