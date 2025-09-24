using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace PARS.Inhouse.Systems.Application.Services.Anypoint
{
    public class BrevoEmailSenderOptions
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
    }

    public class BrevoEmailSender
    {
        private readonly BrevoEmailSenderOptions _options;

        public BrevoEmailSender(IOptions<BrevoEmailSenderOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("", "WMO Suporte");
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.Subject = subject;
                mailMessage.Body = htmlMessage;
                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient("smtp-relay.brevo.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("", "");

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
        public string GenerateRandomPassword(int length = 12)
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string specials = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            var allChars = new List<char>();
            var random = new Random();

            allChars.Add(upperCase[random.Next(upperCase.Length)]);
            allChars.Add(lowerCase[random.Next(lowerCase.Length)]);
            allChars.Add(numbers[random.Next(numbers.Length)]);
            allChars.Add(specials[random.Next(specials.Length)]);

            var chars = upperCase + lowerCase + numbers + specials;
            for (int i = 4; i < length; i++)
            {
                allChars.Add(chars[random.Next(chars.Length)]);
            }

            var shuffled = allChars.OrderBy(c => random.Next()).ToArray();

            return new string(shuffled);
        }
    }
}
