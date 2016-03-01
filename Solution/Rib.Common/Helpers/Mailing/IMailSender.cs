namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(MailSender))]
    public interface IMailSender
    {
        Task SendAsync(MailMessage mailMessage);
    }
}