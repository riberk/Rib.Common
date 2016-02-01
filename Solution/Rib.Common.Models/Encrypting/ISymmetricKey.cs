namespace Rib.Common.Models.Encrypting
{
    using JetBrains.Annotations;

    public interface ISymmetricKey
    {
        [NotNull]
        byte[] Key { get; }

        [NotNull]
        byte[] IV { get; }
    }
}