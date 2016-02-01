namespace Rib.Common.Helpers.Metadata.Enums
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IEnumAttributeReader>().To<EnumAttributeReader>().InSingletonScope();
            Bind<IEnumFieldReader>().To<EnumFieldReader>().InSingletonScope();
            Bind<IEnumReader>().To<EnumReader>().InSingletonScope();
        }
    }
}