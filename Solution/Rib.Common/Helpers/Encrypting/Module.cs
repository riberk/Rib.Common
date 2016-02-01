namespace Rib.Common.Helpers.Encrypting
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IByteArraySplitter>().To<ByteArraySplitter>().InSingletonScope();
        }
    }
}