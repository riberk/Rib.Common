namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    /// <summary>
    ///     Ошибка из-за неправильно сконфигурированного атрибута
    /// </summary>
    public class AttributeException : MetadataException
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="AttributeException" />.
        /// </summary>
        /// <param name="type">Тип атрибута</param>
        public AttributeException(Type type)
        {
            Type = type;
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="AttributeException" /> с указанным сообщением об
        ///     ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку. </param>
        /// <param name="type">Тип атрибута</param>
        public AttributeException(string message, Type type) : base(message)
        {
            Type = type;
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="AttributeException" /> с указанным сообщением об ошибке
        ///     и ссылкой на внутреннее исключение, вызвавшее это исключение.
        /// </summary>
        /// <param name="message">Сообщение об ошибке с объяснением причин исключения. </param>
        /// <param name="innerException">
        ///     Исключение, которое является причиной текущего исключения.Если параметр
        ///     <paramref name="innerException" /> не является указателем null, текущее исключение выброшено в блоке catch,
        ///     обрабатывающем внутренние исключения.
        /// </param>
        /// <param name="type">Тип атрибута</param>
        public AttributeException(string message, Exception innerException, Type type) : base(message, innerException)
        {
            Type = type;
        }

        /// <summary>
        ///     Тип атрибута
        /// </summary>
        public Type Type { get; }
    }
}