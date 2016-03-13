namespace Rib.Common.Models.Metadata
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Class)]
    public class BindFromAttribute : Attribute
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BindFromAttribute"/>.
        /// </summary>
        public BindFromAttribute([NotNull] params Type[] from) : this(null, from)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BindFromAttribute"/>.
        /// </summary>
        public BindFromAttribute(string name, [NotNull] params Type[] from)
        {
            if (@from == null) throw new ArgumentNullException(nameof(@from));
            From = from;
            Name = name;
        }

        public IReadOnlyCollection<Type> From { get; }

        public string Name { get; }

        public string Scope { get; set; }
    }
}