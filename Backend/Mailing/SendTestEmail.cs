using MailKit.Net.Smtp;
using MimeKit;

namespace Mailing;

public class SendTestEmail
{
    public void Send()
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Joey Tribbiani", "joey@friends.com"));
        message.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "chandler@friends.com"));
        message.Subject = "How you doin'?";

        message.Body = new TextPart("plain")
        {
            Text = @"Hey Chandler, just wanted to let you know that Monica and I were going to go play some paintball, you in? -- Joey"
        };

        using var client = new SmtpClient();
        client.Connect("localhost", 1025, false);

        // Note: only needed if the SMTP server requires authentication
        //client.Authenticate("joey", "password");

        client.Send(message);
        client.Disconnect(true);
    }
}