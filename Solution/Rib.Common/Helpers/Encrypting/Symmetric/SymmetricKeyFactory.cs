namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System;
    using Rib.Common.Models.Encrypting;
    using JetBrains.Annotations;

    public class SymmetricKeyFactory : ISymmetricKeyFactory
    {
        [NotNull] private readonly ISymmetricAlgorithmFactory _symmetricAlgorithmFactory;
        [NotNull] private readonly ISymmetricAlgorithmTypeReader _symmetricAlgorithmTypeReader;

        public SymmetricKeyFactory([NotNull] ISymmetricAlgorithmTypeReader symmetricAlgorithmTypeReader,
            [NotNull] ISymmetricAlgorithmFactory symmetricAlgorithmFactory)
        {
            if (symmetricAlgorithmTypeReader == null) throw new ArgumentNullException(nameof(symmetricAlgorithmTypeReader));
            if (symmetricAlgorithmFactory == null) throw new ArgumentNullException(nameof(symmetricAlgorithmFactory));
            _symmetricAlgorithmTypeReader = symmetricAlgorithmTypeReader;
            _symmetricAlgorithmFactory = symmetricAlgorithmFactory;
        }

        public ISymmetricKey Create()
        {
            using (var sa = _symmetricAlgorithmFactory.Create(_symmetricAlgorithmTypeReader.Read()))
            {
                sa.GenerateIV();
                sa.GenerateKey();
                return new SymmetricKey(sa.Key, sa.IV);
            }
        }
    }
}