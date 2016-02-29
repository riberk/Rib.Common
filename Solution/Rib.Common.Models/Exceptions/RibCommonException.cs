namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    public class RibCommonException : ApplicationException
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="T:System.ApplicationException" />.
        /// </summary>
        public RibCommonException()
        {
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="T:System.ApplicationException" /> с указанным сообщением об
        ///     ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку. </param>
        public RibCommonException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="T:System.ApplicationException" /> с указанным сообщением об ошибке
        ///     и ссылкой на внутреннее исключение, вызвавшее это исключение.
        /// </summary>
        /// <param name="message">Сообщение об ошибке с объяснением причин исключения. </param>
        /// <param name="innerException">
        ///     Исключение, которое является причиной текущего исключения.Если параметр
        ///     <paramref name="innerException" /> не является указателем null, текущее исключение выброшено в блоке catch,
        ///     обрабатывающем внутренние исключения.
        /// </param>
        public RibCommonException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}