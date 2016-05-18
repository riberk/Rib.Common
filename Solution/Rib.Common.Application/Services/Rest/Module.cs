namespace Rib.Common.Application.Services.Rest
{
    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind(typeof (IRestGetService<,>)).To(typeof (RestGetService<,>)).InSingletonScope();
            Bind(typeof (IRestDeleteService<>)).To(typeof (RestDeleteService<>)).InSingletonScope();
            Bind(typeof (IRestCreateService<,,>)).To(typeof (RestCreateService<,,>)).InSingletonScope();
            Bind(typeof (IRestUpdateService<,>)).To(typeof (RestUpdateService<,>)).InSingletonScope();
            Bind(typeof (IRestService<,,,,>)).To(typeof (RestService<,,,,>)).InSingletonScope();
        }
    }
}