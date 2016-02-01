namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<ISymmetricAlgorithmFactory>().To<SymmetricAlgorithmFactory>().InSingletonScope();
            Bind<ISymmetricCryptoService>().To<SymmetricCryptoService>().InSingletonScope();
            Bind<ISymmetricKeyFactory>().To<SymmetricKeyFactory>().InSingletonScope();
        }
    }
}