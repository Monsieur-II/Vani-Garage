using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Vani.Services.Auth;

namespace Vani.Services;

public static class EmailSender
{
    public static void SendEmail(string recipient, string body, EmailConfig config)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("VaniGarage", config.Username));
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = "Confirm Email";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var client = new SmtpClient();
            client.Connect(config.Host, config.Port, false);
            client.Authenticate(config.Username, config.Password);
            client.Send(email);
            client.Disconnect(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in sending email: " + ex.Message);
            //throw;
        }
    }
}
