using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CS4540_A2.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var task = new Task(() =>
            {
                SmtpClient smtpClient = new SmtpClient("smtp.utah.edu", 25);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("u1193853@utah.edu", "LOT");
                mail.To.Add(new MailAddress(email));
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true; smtpClient.Send(mail);
            });

            task.Start();
            return task;
        }
    }

}
