using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Bulky.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        // access appsettings.json
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

            // Use SmtpClient to send the email
            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;  // Enable SSL for security

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername), // From address
                    Subject = subject,                    // Email subject
                    Body = htmlMessage,                   // Email body
                    IsBodyHtml = true                     // Set the email as HTML
                };

                mailMessage.To.Add(email);  // Add the recipient email

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);  // Send the email
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;  
                }
            }
        }
    }
}
