namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    public class ActionForbiddenException : RibCommonLogicException
    {
        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" />.
        /// </summary>
        public ActionForbiddenException()
        {
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" /> � ��������� ���������� ��
        ///     ������.
        /// </summary>
        /// <param name="message">���������, ����������� ������. </param>
        public ActionForbiddenException(string message) : base(message)
        {
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" /> � ��������� ���������� �� ������
        ///     � ������� �� ���������� ����������, ��������� ��� ����������.
        /// </summary>
        /// <param name="message">��������� �� ������ � ����������� ������ ����������. </param>
        /// <param name="innerException">
        ///     ����������, ������� �������� �������� �������� ����������.���� ��������
        ///     <paramref name="innerException" /> �� �������� ���������� null, ������� ���������� ��������� � ����� catch,
        ///     �������������� ���������� ����������.
        /// </param>
        public ActionForbiddenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" /> � ���������������� �������.
        /// </summary>
        /// <param name="info">������, ���������� ��������������� ������ �������. </param>
        /// <param name="context">����������� �������� �� ��������� ��� ����������. </param>
        protected ActionForbiddenException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}