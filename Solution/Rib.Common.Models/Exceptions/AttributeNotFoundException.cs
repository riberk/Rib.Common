namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    /// <summary>
    ///     ќшибка, возникающа€ из-за отсутств€ атрибута
    /// </summary>
    public class AttributeNotFoundException : AttributeException
    {
        /// <summary>
        ///     »нициализирует новый экземпл€р класса <see cref="AttributeException" />.
        /// </summary>
        /// <param name="type">“ип атрибута</param>
        /// <param name="provider">„лен, на котором ожидалс€ атрибут</param>
        public AttributeNotFoundException(Type type, ICustomAttributeProvider provider) : base(type)
        {
            Provider = provider;
        }

        /// <summary>
        ///     »нициализирует новый экземпл€р класса <see cref="AttributeException" /> с указанным сообщением об
        ///     ошибке.
        /// </summary>
        /// <param name="message">—ообщение, описывающее ошибку. </param>
        /// <param name="type">“ип атрибута</param>
        /// <param name="provider">„лен, на котором ожидалс€ атрибут</param>
        public AttributeNotFoundException(string message, Type type, ICustomAttributeProvider provider) : base(message, type)
        {
            Provider = provider;
        }

        /// <summary>
        ///     »нициализирует новый экземпл€р класса <see cref="AttributeException" /> с указанным сообщением об ошибке
        ///     и ссылкой на внутреннее исключение, вызвавшее это исключение.
        /// </summary>
        /// <param name="message">—ообщение об ошибке с объ€снением причин исключени€. </param>
        /// <param name="innerException">
        ///     »сключение, которое €вл€етс€ причиной текущего исключени€.≈сли параметр
        ///     <paramref name="innerException" /> не €вл€етс€ указателем null, текущее исключение выброшено в блоке catch,
        ///     обрабатывающем внутренние исключени€.
        /// </param>
        /// <param name="type">“ип атрибута</param>
        /// <param name="provider">„лен, на котором ожидалс€ атрибут</param>
        public AttributeNotFoundException(string message, Exception innerException, Type type, ICustomAttributeProvider provider)
            : base(message, innerException, type)
        {
            Provider = provider;
        }

        /// <summary>
        ///     „лен, на котором ожидалс€ атрибут
        /// </summary>
        public ICustomAttributeProvider Provider { get; }
    }
}