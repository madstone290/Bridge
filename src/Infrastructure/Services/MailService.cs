using Bridge.Application.Common.Services;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Bridge.Infrastructure.Services
{
    public class MailService : IMailService
    {
        public class Config
        {
            [Required(ErrorMessage = "메일 서비스 포트가 유효하지 않습니다")]
            public string Server { get; set; } = string.Empty;
            
            [Range(1,65535, ErrorMessage = "메일 서비스 포트가 유효하지 않습니다")]
            public int Port { get; set; }
            
            [Required(ErrorMessage = "메일 서비스 송신이메일이 유효하지 않습니다")]
            public string SenderEmail { get; set; } = string.Empty;
            
            [Required(ErrorMessage = "메일 서비스 계정이 유효하지 않습니다")]
            public string Account { get; set; } = string.Empty;

            [Required(ErrorMessage = "메일 서비스 비밀번호가 유효하지 않습니다")]
            public string Password { get; set; } = string.Empty;
        }

        private readonly Config _config;

        public MailService(IOptions<Config> configOptions)
        {
            _config = configOptions.Value;
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
