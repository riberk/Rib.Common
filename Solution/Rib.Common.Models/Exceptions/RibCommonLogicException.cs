namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    public class RibCommonLogicException : RibCommonException
    {
        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" />.
        /// </summary>
        public RibCommonLogicException()
        {
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" /> � ��������� ���������� ��
        ///     ������.
        /// </summary>
        /// <param name="message">���������, ����������� ������. </param>
        public RibCommonLogicException(string message) : base(message)
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
        public RibCommonLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" /> � ���������������� �������.
        /// </summary>
        /// <param name="info">������, ���������� ��������������� ������ �������. </param>
        /// <param name="context">����������� �������� �� ��������� ��� ����������. </param>
        protected RibCommonLogicException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}