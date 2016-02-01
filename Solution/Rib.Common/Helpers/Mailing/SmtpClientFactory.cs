namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;

    internal class SmtpClientFactory : ISmtpClientFactory
    {
        public SmtpClient Create()
        {
            return new SmtpClient();
        }
    }
}