namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using Rib.Common.Models.Encrypting;
    using JetBrains.Annotations;

    public interface ISymmetricKeyFactory
    {
        [NotNull]
        ISymmetricKey Create();
    }
}