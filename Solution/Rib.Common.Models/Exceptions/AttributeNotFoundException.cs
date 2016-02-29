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
        /// <param name="memberInfo">����, �� ������� �������� �������</param>
        public AttributeNotFoundException(Type type, ICustomAttributeProvider memberInfo) : base(type)
        {
            MemberInfo = memberInfo;
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="AttributeException" /> � ��������� ���������� ��
        ///     ������.
        /// </summary>
        /// <param name="message">���������, ����������� ������. </param>
        /// <param name="type">��� ��������</param>
        /// <param name="memberInfo">����, �� ������� �������� �������</param>
        public AttributeNotFoundException(string message, Type type, ICustomAttributeProvider memberInfo) : base(message, type)
        {
            MemberInfo = memberInfo;
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
        /// <param name="memberInfo">����, �� ������� �������� �������</param>
        public AttributeNotFoundException(string message, Exception innerException, Type type, ICustomAttributeProvider memberInfo)
            : base(message, innerException, type)
        {
            MemberInfo = memberInfo;
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="AttributeException" /> � ���������������� �������.
        /// </summary>
        /// <param name="info">������, ���������� ��������������� ������ �������. </param>
        /// <param name="context">����������� �������� �� ��������� ��� ����������. </param>
        /// <param name="type">��� ��������</param>
        /// <param name="memberInfo">����, �� ������� �������� �������</param>
        protected AttributeNotFoundException([NotNull] SerializationInfo info, StreamingContext context, Type type, ICustomAttributeProvider memberInfo)
            : base(info, context, type)
        {
            MemberInfo = memberInfo;
        }

        /// <summary>
        ///     ����, �� ������� �������� �������
        /// </summary>
        public ICustomAttributeProvider MemberInfo { get; }
    }
}