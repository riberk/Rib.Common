namespace Rib.Common.Helpers.Tmp
{
    using System;
    using JetBrains.Annotations;

    public interface ITmpFolder : IDisposable
    {
        [NotNull]
        string Path { get; }

        Guid Id { get; }
    }
}