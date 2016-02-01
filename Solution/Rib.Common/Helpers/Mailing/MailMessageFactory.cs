namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;

    internal class MailMessageFactory : IMailMessageFactory
    {
        public MailMessage Create()
        {
            return new MailMessage
            {
                IsBodyHtml = true
            };
        }
    }
}