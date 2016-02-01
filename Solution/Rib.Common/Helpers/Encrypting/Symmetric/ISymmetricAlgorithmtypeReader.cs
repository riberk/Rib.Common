namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using Rib.Common.Models.Encrypting;

    public interface ISymmetricAlgorithmTypeReader
    {
        SymmetricAlgorithmType Read();
    }
}