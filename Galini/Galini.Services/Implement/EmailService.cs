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
            email.Sender = MailboxAddress.Parse(_emailAddress);
            email.To.Add(MailboxAddress.Parse(customerEmail));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = message;
            email.Body = builder.ToMessageBody();

            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            
            smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(_emailAddress, _appPassword);
            await smtpClient.SendAsync(email);
            }
    }
}
