namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;
    using JetBrains.Annotations;

    public interface IMailMessageFactory
    {
        [NotNull]
        MailMessage Create();
    }
}