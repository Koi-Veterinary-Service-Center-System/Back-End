using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using KoiFishCare.Models;
using KoiFishCare.Interfaces;
using System.Net.Mail;


namespace KoiFishCare.service
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("nhatnhse182399@fpt.edu.vn", "qwaw ihgq wulv fyzq"),
                UseDefaultCredentials = false,
                EnableSsl = true,
            };
            var mailMessage = new MailMessage("nhatnhse182399@fpt.edu.vn", email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}