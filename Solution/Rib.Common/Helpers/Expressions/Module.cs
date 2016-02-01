namespace Rib.Common.Helpers.Expressions
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IPathConvertRemover>().To<PathConvertRemover>().InSingletonScope();
            Bind<IPropertyStringResolver>().To<PropertyStringResolver>().InSingletonScope();
            Bind<IStringToPathProvider>().To<StringToPathProvider>().InSingletonScope();
            Bind<INewMaker>().To<NewMaker>().InSingletonScope();
            Bind<IPropertyHelper>().To<PropertyHelper>().InSingletonScope();
        }
    }
}