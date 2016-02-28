namespace Rib.Common.Models.Exceptions
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    /// <summary>
    ///     Ошибка, возникающая из-за отсутствя атрибута
    /// </summary>
    public class AttributeNotFoundException : AttributeException
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="AttributeException" />.
        /// </summary>
        /// <param name="type">Тип атрибута</param>
        /// <param name="memberInfo">Член, на котором ожидался атрибут</param>
        public AttributeNotFoundException(Type type, ICustomAttributeProvider memberInfo) : base(type)
        {
            MemberInfo = memberInfo;
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="AttributeException" /> с указанным сообщением об
        ///     ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку. </param>
        /// <param name="type">Тип атрибута</param>
        /// <param name="memberInfo">Член, на котором ожидался атрибут</param>
        public AttributeNotFoundException(string message, Type type, ICustomAttributeProvider memberInfo) : base(message, type)
        {
            MemberInfo = memberInfo;
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
        /// <param name="memberInfo">Член, на котором ожидался атрибут</param>
        public AttributeNotFoundException(string message, Exception innerException, Type type, ICustomAttributeProvider memberInfo)
            : base(message, innerException, type)
        {
            MemberInfo = memberInfo;
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="AttributeException" /> с сериализованными данными.
        /// </summary>
        /// <param name="info">Объект, содержащий сериализованные данные объекта. </param>
        /// <param name="context">Контекстные сведения об источнике или назначении. </param>
        /// <param name="type">Тип атрибута</param>
        /// <param name="memberInfo">Член, на котором ожидался атрибут</param>
        protected AttributeNotFoundException([NotNull] SerializationInfo info, StreamingContext context, Type type, ICustomAttributeProvider memberInfo)
            : base(info, context, type)
        {
            MemberInfo = memberInfo;
        }

        /// <summary>
        ///     Член, на котором ожидался атрибут
        /// </summary>
        public ICustomAttributeProvider MemberInfo { get; }
    }
}