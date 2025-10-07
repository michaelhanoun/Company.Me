using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.BLL.Models;
using Microsoft.Extensions.Configuration;

namespace Company.BLL.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmail(EmailMessage message)
        {
            try
            {
                var senderEmail = _configuration["EmailSettings:senderEmail"];

                var senderPassword = _configuration["EmailSettings:senderPassword"];
                MailAddress address = new MailAddress(senderEmail);
                MailMessage mail = new MailMessage();
                mail.To.Add(message.To);
                mail.Subject = message.Subject;
                mail.Body = $"<html><html>{message.Body}</html></html>";
                mail.IsBodyHtml = true;
                mail.From = address;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = true;
                smtpClient.Host = _configuration["EmailSettings:SmtpClientServer"];
                smtpClient.Port = int.Parse(_configuration["EmailSettings:SmtpClientPort"]);
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                await smtpClient.SendMailAsync(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
