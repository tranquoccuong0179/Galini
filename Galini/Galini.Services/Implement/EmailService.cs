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

        public async Task SendEmailAsync(string customerEmail, string subject, string otp, string username)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress("Harmon", _emailAddress);
            email.From.Add(new MailboxAddress("Harmon", _emailAddress));
            email.To.Add(MailboxAddress.Parse(customerEmail));
            email.Subject = subject;

            var builder = new BodyBuilder();
            var image = builder.LinkedResources.Add("wwwroot/images/logo.jpg");
            image.ContentId = "logo";

            var facebookIcon = builder.LinkedResources.Add("wwwroot/images/logo-facebook.png");
            facebookIcon.ContentId = "facebook";

            var instagramIcon = builder.LinkedResources.Add("wwwroot/images/logo-instagram.png");
            instagramIcon.ContentId = "instagram";

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
            background-color: #f4f4f4;
            color: #333;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            background: white;
            color: #333;
            padding: 20px;
            border-radius: 10px;
            max-width: 600px;
            margin: auto;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }}
        .logo {{
            width: 120px;
            margin: 0 auto 20px;
            display: block;
        }}
        .header {{
            font-size: 26px;
            font-weight: bold;
            color: #A259FF;
            margin-bottom: 20px;
            text-align: center;
        }}
        .otp {{
            font-size: 40px;
            font-weight: bold;
            color: #A259FF;
            margin: 20px 0;
            text-align: center;
        }}
        .note {{
            font-size: 14px;
            color: #888;
            margin-top: 20px;
            text-align: center;
        }}
        .footer {{
            font-size: 12px;
            color: #777;
            margin-top: 20px;
            text-align: center;
        }}
        .social-icons {{
            text-align: center;
            margin-top: 20px;
        }}
        .social-icons a{{
            display: inline-block;
        }}
        .social-icons img {{
            width: 40px;
            height: 40px;
            margin: 0 10px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <img src='cid:logo' alt='Logo' class='logo'>
        <div class='header'>Xác nhận mã OTP của bạn</div>
        <div class='content'>
            <p>Xin chào, {username}</p>
            <p>Bạn đã yêu cầu mã OTP để xác thực. Vui lòng sử dụng mã OTP bên dưới:</p>
            <div class='otp'>{otp}</div>
            <p class='note'>Xin lưu ý: Không chia sẻ mã này cho bất kỳ ai để đảm bảo an toàn tài khoản của bạn.</p>
        </div>
        <div class='footer'>
            <p>Nếu bạn không yêu cầu mã OTP, vui lòng bỏ qua email này hoặc liên hệ với chúng tôi để được hỗ trợ.</p>
            <hr />
            <div class='social-icons'>
                <a href='https://www.facebook.com/harmon.sharing' target='_blank'>
                    <img src='cid:facebook' alt='Facebook'>
                </a>
                <a href='https://www.instagram.com/weare_harmon_/' target='_blank'>
                    <img src='cid:instagram' alt='Instagram'>
                </a>
            </div>
            <hr />
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
