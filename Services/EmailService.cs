using ContactPro.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ContactPro.Services
{
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public EmailService (IOptions<MailSettings> mailSettings)
        {

            _mailSettings = mailSettings.Value;
        }
  
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string? emailSender = _mailSettings.Email ?? Environment.GetEnvironmentVariable("Email");


            MimeMessage newEmail = new();

            newEmail.From.Add(MailboxAddress.Parse(emailSender));

            // YES
            // newEmail.From.Add(MailboxAddress.Parse(emailSender));
            // Works, hard coded - newEmail.From.Add(MailboxAddress.Parse("red@devore.digital"));

            // NOPE
            // Doesn't work, Maybe for Google - newEmail.Sender = MailboxAddress.Parse(emailSender);
            // newEmail.Sender(MailboxAddress.Parse(emailSender));



            foreach (string emailAddress in email.Split(";"))
            {
                newEmail.To.Add(MailboxAddress.Parse(emailAddress));
            }
            newEmail.Subject = subject;

            BodyBuilder emailBody = new();

            emailBody.HtmlBody = htmlMessage;

            newEmail.Body = emailBody.ToMessageBody();

            // Log into SMTP 

            using SmtpClient smtpClient = new();

            try
            {
                string? host = _mailSettings.MailHost ?? Environment.GetEnvironmentVariable("MailHost");
                int port = _mailSettings.MailPort !=0 ? _mailSettings.MailPort : int.Parse(Environment.GetEnvironmentVariable("MailPort")!);
                string? password = _mailSettings.MailPassword ?? Environment.GetEnvironmentVariable("MailPassword"); ;

                await smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(emailSender, password);

                await smtpClient.SendAsync(newEmail);
                await smtpClient.DisconnectAsync(true);



            }
            catch(Exception ex)
            {
                string error = ex.Message;
                throw;

            }




        }
    }
}
