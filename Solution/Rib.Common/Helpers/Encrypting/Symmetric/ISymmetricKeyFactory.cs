namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using Rib.Common.Models.Encrypting;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(SymmetricKeyFactory))]
    public interface ISymmetricKeyFactory
    {
        [NotNull]
        ISymmetricKey Create();
    }
}