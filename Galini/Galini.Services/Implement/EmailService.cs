using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Azure.Messaging;
using StackExchange.Redis;
using MailKit.Security;

namespace Galini.Services.Implement
{
    public class EmailService : IEmailService
    {
        private readonly string _emailAddress;
        private readonly string _appPassword;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailAddress = emailSettings.Value.EmailAddress;
            _appPassword = emailSettings.Value.AppPassword;
        }

        public async Task SendEmailAsync(string customerEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress("Harmon", _emailAddress);
            email.From.Add(new MailboxAddress("Harmon", _emailAddress));
            email.To.Add(MailboxAddress.Parse(customerEmail));
            email.Subject = subject;

            var builder = new BodyBuilder();
            var image = builder.LinkedResources.Add("wwwroot/images/logo.jpg");
            image.ContentId = "logo";
            string htmlContent = $@"
    <!DOCTYPE html>
    <html lang='vi'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Email</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #A259FF;
                color: white;
                text-align: center;
                padding: 20px;
            }}
            .container {{
                background: white;
                color: #A259FF;
                padding: 20px;
                border-radius: 10px;
                max-width: 600px;
                margin: auto;
            }}
            .logo {{
                width: 100px;
                margin-bottom: 20px;
            }}
            .content {{
                font-size: 16px;
                line-height: 1.6;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <img src='cid:logo' alt='Logo' class='logo'>
            <div class='content'>
                <p>Xin chào,</p>
                <p>{message}</p>
                <p>Chúc bạn một ngày tốt lành!</p>
            </div>
        </div>
    </body>
    </html>";
            builder.HtmlBody = htmlContent;
            email.Body = builder.ToMessageBody();

            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            
            smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(_emailAddress, _appPassword);
            await smtpClient.SendAsync(email);
        }
    }
}
