namespace Rib.Common.Helpers.Cache
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IObjectCacheFactory>().To<ObjectCacheFactory>().InSingletonScope();
            Bind<ICacheCleaner>().To<CacheCleaner>().InSingletonScope();
        }
    }
}