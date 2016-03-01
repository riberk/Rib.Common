namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(MailBuilder))]
    public interface IMailBuilder
    {
        [NotNull]
        IMailBuilder Body([NotNull] string body);

        [NotNull]
        IMailBuilder From([NotNull] MailAddress from);

        [NotNull]
        IMailBuilder From([NotNull] string from);

        [NotNull]
        IMailBuilder AddTo([NotNull] string to);

        [NotNull]
        IMailBuilder AddTo([NotNull, ItemNotNull] params string[] to);

        [NotNull]
        IMailBuilder AddTo([NotNull] MailAddress to);

        [NotNull]
        IMailBuilder AddTo([NotNull, ItemNotNull] params MailAddress[] to);

        [NotNull]
        IMailBuilder Subject(string subject);

        [NotNull]
        IMailBuilder AddAttachment([NotNull] Attachment attachment);

        [NotNull]
        IMailBuilder AddAttachments([NotNull] params Attachment[] attachments);

        [NotNull]
        Task SendAsync();

        [NotNull]
        MailMessage ToMessage();
    }
}