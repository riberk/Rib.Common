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
            Bind<IRsaCryptoServiceProviderResolver, IRsaKeyCreator>().To<RsaCryptoServiceProviderResolver>().InSingletonScope();
        }
    }
}