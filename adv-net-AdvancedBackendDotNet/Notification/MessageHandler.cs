using Common.Messages;
using EasyNetQ;
using Notification.Services;

namespace Notification
{
    public static class MessageHandler
    {
        public static async Task HandleMessage(this WebApplication app, SendEmailMessage msg)
        {
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var client = scope.ServiceProvider.GetRequiredService<EmailService>();

                    await client.SendEmail(msg.To, msg.Message, msg.Topic);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static async Task SetupListener(this IBus bus, WebApplication app)
        {
            bus.PubSub.Subscribe<SendEmailMessage>("send_email_id", msg => app.HandleMessage(msg));
        }
    }
}
