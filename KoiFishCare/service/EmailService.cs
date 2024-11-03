using System;
using System.Net.Mail;
using System.Threading.Tasks;
using KoiFishCare.Interfaces;

namespace KoiFishCare.service
{
    // Định nghĩa class EmailService
    public class EmailService : IEmailService
    {
        // Phương thức gửi email
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                using var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential(
                        "nhatnhse182399@fpt.edu.vn",
                        "qwaw ihgq wulv fyzq"  // App Password của bạn
                    ),
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("nhatnhse182399@fpt.edu.vn", "KoiFishCare"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw;
            }
        }
    }
}
