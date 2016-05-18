namespace Rib.Common.Models.Metadata
{
    using System;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class ContextPropertyAttribute : Attribute
    {
        [NotNull] public readonly Type ResolverType;

        public ContextPropertyAttribute([NotNull] Type resolverType)
        {
            if (resolverType == null) throw new ArgumentNullException(nameof(resolverType));
            ResolverType = resolverType;
        }
    }
}