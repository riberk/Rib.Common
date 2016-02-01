namespace Rib.Common.Helpers.Mailing
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    internal class MailSender : IMailSender
    {
        [NotNull] private readonly ISmtpClientFactory _smtpClientFactory;

        public MailSender([NotNull] ISmtpClientFactory smtpClientFactory)
        {
            if (smtpClientFactory == null) throw new ArgumentNullException(nameof(smtpClientFactory));
            _smtpClientFactory = smtpClientFactory;
        }

        public async Task SendAsync([NotNull] MailMessage mailMessage)
        {
            if (mailMessage == null) throw new ArgumentNullException(nameof(mailMessage));
            using (var smtpClient = _smtpClientFactory.Create())
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}