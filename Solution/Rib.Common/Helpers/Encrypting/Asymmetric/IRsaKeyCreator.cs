namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    public interface IRsaKeyCreator
    {
        void CreateIfNotExists(string containerName);
    }
}