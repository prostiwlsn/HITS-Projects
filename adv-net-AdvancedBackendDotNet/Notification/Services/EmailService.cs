using System.Net.Mail;

namespace Notification.Services
{
    public class EmailService
    {
        private SmtpClient _smtpClient;
        private IConfiguration _configuration;
        public EmailService(SmtpClient smtpClient, IConfiguration configuration)
        {
            _smtpClient = smtpClient;
            _configuration = configuration;
        }

        public async Task SendEmail(string to, string text, string topic = "noreply")
        {
            string from = _configuration.GetSection("Smtp:Address").Value;

            MailMessage message = new MailMessage(from, to);
            message.Subject = topic;
            message.Body = text;

            try
            {
                _smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
