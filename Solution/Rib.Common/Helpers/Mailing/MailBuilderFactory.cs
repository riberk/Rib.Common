namespace Rib.Common.Helpers.Mailing
{
    using System;
    using Rib.Common.Ninject;
    using JetBrains.Annotations;

    internal class MailBuilderFactory : IMailBuilderFactory
    {
        [NotNull] private readonly IResolver _kernel;

        public MailBuilderFactory([NotNull] IResolver kernel)
        {
            if (kernel == null) throw new ArgumentNullException(nameof(kernel));
            _kernel = kernel;
        }

        public IMailBuilder Create()
        {
            return _kernel.Get<IMailBuilder>();
        }
    }
}