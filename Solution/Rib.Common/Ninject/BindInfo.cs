namespace Rib.Common.Ninject
{
    using System;
    using System.Collections.Generic;

    internal class BindInfo : IBindInfo
    {
        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="BindInfo"/>.
        /// </summary>
        public BindInfo(BindingScope scope, IReadOnlyCollection<Type> @from, Type to, string name = null)
        {
            Scope = scope;
            From = @from;
            To = to;
            Name = name;
        }

        public BindingScope Scope { get; }
        public IReadOnlyCollection<Type> From { get; }
        public Type To { get; }
        public string Name { get; }
    }
}