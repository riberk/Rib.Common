namespace Rib.Common.Helpers.Configuration.SettingsManagers
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IConfigurationReader, IConfigurationWriter, IConfigurationManager>().To<ConfigurationManager>().InSingletonScope();
        }
    }
}