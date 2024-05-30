using System.Net;
using MailKit.Net.Smtp;
using MimeKit;

namespace Emailing;

public interface IEmailSender
{
    Task<string> SendEmailAsync(EmailParams emailParams, ClientParams clientParams, CancellationToken ct);
}

public class EmailSender : IEmailSender 
{
    public async Task<string> SendEmailAsync(EmailParams emailParams, ClientParams clientParams, CancellationToken ct)
    {
        var message = new MimeMessage();
        message.Sender = MailboxAddress.Parse(emailParams.FromEmail);
        message.To.AddRange(emailParams.ToList.Select(MailboxAddress.Parse));

        var builder = new BodyBuilder
        {
            HtmlBody = emailParams.HtmlBody,
            TextBody = emailParams.TextBody,
        };

        message.Body = builder.ToMessageBody();
        message.Subject = emailParams.Subject;

        using var client = new SmtpClient();
        await client.ConnectAsync(clientParams.SmtpHost, clientParams.SmtpPort, cancellationToken: ct);
        await client.AuthenticateAsync(new NetworkCredential(clientParams.Username, clientParams.UserPassword), ct);
        
        var messageId = await client.SendAsync(message, ct);
        
        return messageId;
    }
}