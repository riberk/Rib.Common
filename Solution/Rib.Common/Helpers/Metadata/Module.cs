namespace Rib.Common.Helpers.Metadata
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IAttributesReader>().To<AttributesReader>().InSingletonScope();
            Bind<IAssemblyInfoRetriever>().To<AssemblyInfoRetriever>().InSingletonScope();
        }
    }
}