namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;
    using System.Threading.Tasks;

    public interface IMailSender
    {
        Task SendAsync(MailMessage mailMessage);
    }
}