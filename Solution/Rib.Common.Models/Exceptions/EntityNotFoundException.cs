namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    public class EntityNotFoundException : RibCommonLogicException
    {
        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" />.
        /// </summary>
        public EntityNotFoundException()
        {
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" /> � ��������� ���������� ��
        ///     ������.
        /// </summary>
        /// <param name="message">���������, ����������� ������. </param>
        public EntityNotFoundException(string message) : base(message)
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
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:System.ApplicationException" /> � ���������������� �������.
        /// </summary>
        /// <param name="info">������, ���������� ��������������� ������ �������. </param>
        /// <param name="context">����������� �������� �� ��������� ��� ����������. </param>
        protected EntityNotFoundException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}