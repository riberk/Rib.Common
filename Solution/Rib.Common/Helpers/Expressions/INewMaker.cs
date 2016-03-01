namespace Rib.Common.Helpers.Expressions
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(NewMaker))]
    public interface INewMaker
    {
        [NotNull]
        Func<T> Create<T>();
    }
}