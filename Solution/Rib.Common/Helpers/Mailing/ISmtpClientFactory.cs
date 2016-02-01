namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;
    using JetBrains.Annotations;

    public interface ISmtpClientFactory
    {
        [NotNull]
        SmtpClient Create();
    }
}