﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Build.Framework;
using System.Net.Mail;

namespace LeaveManagementSystem.Web.Services
{
    /*_configuration is used to read values like SMTP server,port,credentials
    from appsettings.json*/
    public class EmailSender(IConfiguration _configuration):IEmailSender
    {
     

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromAddress=_configuration["EmailSettings:DefaultEmailAddress"];
            var smtpServer = _configuration["EmailSettings:Server"];
            var smtpPort = Convert.ToInt32(_configuration["EmailSettings:Port"]);
            var message=new MailMessage 
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(email));
            using var client = new SmtpClient(smtpServer, smtpPort);
            await client.SendMailAsync(message);
        }
        
    }


    
}

