using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DisSagligiTakip.Helpers
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _configuration["Smtp:Host"] ?? "smtp.gmail.com";
            var smtpPortString = _configuration["Smtp:Port"];
            var smtpPort = int.TryParse(smtpPortString, out var port) ? port : 587;
            var smtpUser = _configuration["Smtp:User"] ?? string.Empty;
            var smtpPass = _configuration["Smtp:Password"] ?? string.Empty;
            var enableSslString = _configuration["Smtp:EnableSsl"];
            var enableSsl = bool.TryParse(enableSslString, out var ssl) ? ssl : true;

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = enableSsl
            };

            using var message = new MailMessage
            {
                From = new MailAddress(smtpUser, "Dis Sağlığı Takip Sistemi"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            await client.SendMailAsync(message);
        }
    }
}
