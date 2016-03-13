namespace Rib.Common.Models.Binding
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    internal class BindInfo : IBindInfo
    {
        /// <summary>
        ///     »нициализирует новый экземпл€р класса <see cref="BindInfo" />.
        /// </summary>
        public BindInfo([NotNull] BindingScope scope, [NotNull] IReadOnlyCollection<Type> @from, [NotNull] Type to, string name = null)
        {
            if (scope == null) throw new ArgumentNullException(nameof(scope));
            if (@from == null) throw new ArgumentNullException(nameof(@from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            Scope = scope;
            From = @from;
            To = to;
            Name = name;
        }

        [NotNull]
        public BindingScope Scope { get; }

        [NotNull]
        public IReadOnlyCollection<Type> From { get; }

        [NotNull]
        public Type To { get; }

        public string Name { get; }
    }
}