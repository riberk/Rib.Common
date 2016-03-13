namespace Rib.Common.Ninject
{
    using System;
    using JetBrains.Annotations;

    public class BindingScope : IEquatable<BindingScope>
    {
        public const string Singleton = "Singleton";

        public const string Thread = "Thread";

        public const string Transient = "Transient";

        [NotNull]
        public static readonly BindingScope SingletonScope = new BindingScope(Singleton);

        [NotNull]
        public static readonly BindingScope ThreadScope = new BindingScope(Thread);

        [NotNull]
        public static readonly BindingScope TransientScope = new BindingScope(Transient);

        internal string Scope { get; }

        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Object"/>.
        /// </summary>
        public BindingScope([NotNull] string scope)
        {
            if (scope == null) throw new ArgumentNullException(nameof(scope));
            Scope = scope;
        }

        /// <summary>
        /// ”казывает, равен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <returns>
        /// true, если текущий объект равен параметру <paramref name="other"/>, в противном случае†Ч false.
        /// </returns>
        /// <param name="other">ќбъект, который требуетс€ сравнить с данным объектом.</param>
        public bool Equals(BindingScope other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Scope, other.Scope);
        }

        /// <summary>
        /// ќпредел€ет, равен ли заданный объект текущему объекту.
        /// </summary>
        /// <returns>
        /// «начение true, если указанный объект равен текущему объекту; в противном случае Ч значение false.
        /// </returns>
        /// <param name="obj">ќбъект, который требуетс€ сравнить с текущим объектом. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BindingScope) obj);
        }

        /// <summary>
        /// —лужит хэш-функцией по умолчанию. 
        /// </summary>
        /// <returns>
        /// ’эш-код дл€ текущего объекта.
        /// </returns>
        public override int GetHashCode()
        {
            return (Scope != null ? Scope.GetHashCode() : 0);
        }

        public static bool operator ==(BindingScope left, BindingScope right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BindingScope left, BindingScope right)
        {
            return !Equals(left, right);
        }
    }
}