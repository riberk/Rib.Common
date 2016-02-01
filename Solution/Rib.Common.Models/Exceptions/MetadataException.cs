namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    /// <summary>
    ///     Ошибка из-за неправильных метаданных
    /// </summary>
    public class MetadataException : RibCommonException
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="MetadataException" />.
        /// </summary>
        public MetadataException()
        {
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="MetadataException" /> с указанным сообщением об
        ///     ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку. </param>
        public MetadataException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="MetadataException" /> с указанным сообщением об ошибке
        ///     и ссылкой на внутреннее исключение, вызвавшее это исключение.
        /// </summary>
        /// <param name="message">Сообщение об ошибке с объяснением причин исключения. </param>
        /// <param name="innerException">
        ///     Исключение, которое является причиной текущего исключения.Если параметр
        ///     <paramref name="innerException" /> не является указателем null, текущее исключение выброшено в блоке catch,
        ///     обрабатывающем внутренние исключения.
        /// </param>
        public MetadataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="MetadataException" /> с сериализованными данными.
        /// </summary>
        /// <param name="info">Объект, содержащий сериализованные данные объекта. </param>
        /// <param name="context">Контекстные сведения об источнике или назначении. </param>
        protected MetadataException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}