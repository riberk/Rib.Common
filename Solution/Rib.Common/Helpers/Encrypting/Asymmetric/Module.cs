namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IAsymmetricCryptoService>().To<AsymmetricCryptoService>().InSingletonScope();
            Bind<IMaxBlockLengthResolver>().To<MaxBlockLengthResolver>().InSingletonScope();
            Bind<IRsaCryptoServiceProviderResolver, IRsaKeyCreator>().To<RsaCryptoServiceProviderResolver>().InSingletonScope();
        }
    }
}