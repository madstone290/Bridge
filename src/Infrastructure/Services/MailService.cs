using Bridge.Application.Common.Services;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;

namespace Bridge.Infrastructure.Services
{
    public class MailService : IMailService
    {
        public class Config
        {
            public string Server { get; set; } = string.Empty;
            public int Port { get; set; }
            public string SenderEmail { get; set; } = string.Empty;
            public string Account { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        private readonly Config _config;

        public MailService(Config config)
        {
            _config = config;
        }

        public async Task SendAsync(string emailTo, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.SenderEmail));
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.Server, _config.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.Account, _config.Password);
            var formText = await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
