namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(MailMessageFactory))]
    public interface IMailMessageFactory
    {
        [NotNull]
        MailMessage Create();
    }
}