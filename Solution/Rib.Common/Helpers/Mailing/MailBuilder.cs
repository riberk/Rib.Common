namespace Rib.Common.Helpers.Mailing
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    internal class MailBuilder : IMailBuilder
    {
        [NotNull] private readonly IMailSender _mailSender;

        [NotNull] private readonly MailMessage _message;

        public MailBuilder([NotNull] IMailSender mailSender,
                           [NotNull] IMailMessageFactory mailMessageFactory)
        {
            if (mailSender == null) throw new ArgumentNullException(nameof(mailSender));
            if (mailMessageFactory == null) throw new ArgumentNullException(nameof(mailMessageFactory));
            _mailSender = mailSender;
            _message = mailMessageFactory.Create();
        }

        public IMailBuilder Body(string body)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));
            _message.Body = body;
            return this;
        }

        public IMailBuilder From(MailAddress @from)
        {
            if (@from == null) throw new ArgumentNullException(nameof(@from));
            _message.From = from;
            return this;
        }

        public IMailBuilder From(string @from)
        {
            if (@from == null) throw new ArgumentNullException(nameof(@from));
            _message.From = new MailAddress(from);
            return this;
        }

        public IMailBuilder AddTo(string to)
        {
            if (to == null) throw new ArgumentNullException(nameof(to));
            _message.To.Add(new MailAddress(to));
            return this;
        }

        public IMailBuilder AddTo(params string[] to)
        {
            if (to == null) throw new ArgumentNullException(nameof(to));
            foreach (var s in to)
            {
                AddTo(s);
            }
            return this;
        }

        public IMailBuilder AddTo(MailAddress to)
        {
            if (to == null) throw new ArgumentNullException(nameof(to));
            _message.To.Add(to);
            return this;
        }

        public IMailBuilder AddTo(params MailAddress[] to)
        {
            if (to == null) throw new ArgumentNullException(nameof(to));
            foreach (var s in to)
            {
                AddTo(s);
            }
            return this;
        }

        public IMailBuilder Subject([NotNull] string subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            _message.Subject = subject;
            return this;
        }

        public IMailBuilder AddAttachment(Attachment attachment)
        {
            if (attachment == null) throw new ArgumentNullException(nameof(attachment));
            _message.Attachments.Add(attachment);
            return this;
        }

        public IMailBuilder AddAttachments(params Attachment[] attachments)
        {
            if (attachments == null) throw new ArgumentNullException(nameof(attachments));
            foreach (var a in attachments)
            {
                _message.Attachments.Add(a);
            }
            return this;
        }

        public Task SendAsync()
        {
            return _mailSender.SendAsync(_message);
        }

        public MailMessage ToMessage()
        {
            return _message;
        }
    }
}