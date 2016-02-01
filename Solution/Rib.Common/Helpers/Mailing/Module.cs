namespace Rib.Common.Helpers.Mailing
{
    using global::Ninject.Modules;

    public class Module : NinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IMailBuilderFactory>().To<MailBuilderFactory>().InSingletonScope();
            Bind<IMailSender>().To<MailSender>().InSingletonScope();
            Bind<IMailBuilder>().To<MailBuilder>().InTransientScope();
            Bind<ISmtpClientFactory>().To<SmtpClientFactory>().InSingletonScope();
            Bind<IMailMessageFactory>().To<MailMessageFactory>().InSingletonScope();
        }
    }
}