namespace Rib.Common.Helpers.Mailing
{
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(MailBuilderFactory))]
    public interface IMailBuilderFactory
    {
        [NotNull]
        IMailBuilder Create();
    }
}