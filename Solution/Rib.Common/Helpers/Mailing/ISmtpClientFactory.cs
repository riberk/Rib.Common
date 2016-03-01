namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(SmtpClientFactory))]
    public interface ISmtpClientFactory
    {
        [NotNull]
        SmtpClient Create();
    }
}