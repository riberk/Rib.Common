namespace Rib.Common.Models.Metadata
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Атрибут для биндинга интерфейса в класс
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class BindToAttribute : Attribute
    {
        public BindToAttribute([NotNull] Type toType) : this(toType, null)
        {
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="T:System.Attribute" />.
        /// </summary>
        public BindToAttribute([NotNull] Type toType, string name)
        {
            if (toType == null) throw new ArgumentNullException(nameof(toType));
            ToType = toType;
            Name = name;
        }

        [NotNull]
        public Type ToType { get; }

        public string Name { get; }
    }
}