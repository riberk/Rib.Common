namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IConfigurationItemResolver>().To<ConfigurationItemResolver>().InSingletonScope();
            Bind<IConfigurationItemsHelper>().To<ConfigurationItemsHelper>().InSingletonScope();
            Bind<IConfigurationItemsReader>().To<ConfigurationItemsReader>().InSingletonScope();
        }
    }
}