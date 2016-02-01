namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    public class BitclickInvalidDataException : RibCommonLogicException
    {
        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" />.
        /// </summary>
        public BitclickInvalidDataException()
        {
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" /> � ��������� ���������� ��
        ///     ������.
        /// </summary>
        /// <param name="message">���������, ����������� ������. </param>
        public BitclickInvalidDataException(string message) : base(message)
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
        public BitclickInvalidDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" /> � ���������������� �������.
        /// </summary>
        /// <param name="info">������, ���������� ��������������� ������ �������. </param>
        /// <param name="context">����������� �������� �� ��������� ��� ����������. </param>
        protected BitclickInvalidDataException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}