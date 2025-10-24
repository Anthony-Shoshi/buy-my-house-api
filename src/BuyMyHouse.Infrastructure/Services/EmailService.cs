using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace BuyMyHouse.Infrastructure.Services;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        string host = _config["SMTP_HOST"]!;
        int port = int.Parse(_config["SMTP_PORT"]!);
        string user = _config["SMTP_USER"]!;
        string pass = _config["SMTP_PASS"]!;
        string from = _config["SMTP_FROM"]!;

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, pass),
            EnableSsl = true
        };

        using var message = new MailMessage(from, to, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}