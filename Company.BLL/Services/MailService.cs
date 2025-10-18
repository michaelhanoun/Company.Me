using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.BLL.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Company.BLL.Services
{
    public class MailService(IOptions<MailSettings> _options) : IEmailSender
    {
        public async Task<bool> SendEmail(EmailMessage message)
        {
            try
            {
                var mail = new MimeMessage();
                mail.Subject = message.Subject;
                mail.From.Add(new MailboxAddress(_options.Value.DisplayName,_options.Value.SenderEmail));
                mail.To.Add(MailboxAddress.Parse(message.To));
                var builder = new BodyBuilder();
                builder.TextBody = message.Body;
                mail.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_options.Value.SmtpClientServer, _options.Value.SmtpClientPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_options.Value.SenderEmail, _options.Value.SenderPassword);
                await smtp.SendAsync(mail);
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
    }
}