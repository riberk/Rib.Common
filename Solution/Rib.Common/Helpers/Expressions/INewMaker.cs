namespace Rib.Common.Helpers.Expressions
{
    using System;
    using JetBrains.Annotations;

    public interface INewMaker
    {
        [NotNull]
        Func<T> Create<T>();
    }
}