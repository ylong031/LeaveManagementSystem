using Microsoft.Extensions.Configuration;
using System.Net.Mail;
namespace LeaveManagementSystem.Application.Services.Email
{
    /*It uses constructor injection to receive an IConfiguration object (_configuration),
    which allows it to access settings from the configuration file
    _configuration is used to read values like SMTP server,port,credentials
    from appsettings.json*/
    public class EmailSender(IConfiguration _configuration) : IEmailSender
    {


        /*If your class (like EmailSender) inherits from IEmailSender, you must implement the SendEmailAsync method.*/
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            /*Reads SMTP settings from the configuration:
            fromAddress: The sender's email address.
            smtpServer: The SMTP server address.
            smtpPort: The SMTP server port(converted to integer).
            
            (The name that you put in appsetting.json must match the name you put here,for example,EmailSettings:Server )
             */

            var fromAddress = _configuration["EmailSettings:DefaultEmailAddress"];
            var smtpServer = _configuration["EmailSettings:Server"];
            var smtpPort = Convert.ToInt32(_configuration["EmailSettings:Port"]);

            /*Creates a new MailMessage object and sets its properties:
            From: Sender’s address.
            To: Adds the recipient’s address.
            Subject: Sets the subject.
            Body: The actual message.
            IsBodyHtml: Allows HTML formatting in the email body.*/

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email));

            /*
            SmtpClient is used to connect to the SMTP server.
            Papercut SMTP is acting as the server.
            Your application (using SmtpClient) is the client.
            
            The email is sent asynchronously with SendMailAsync. 
             */
            using var client = new SmtpClient(smtpServer, smtpPort);
            await client.SendMailAsync(message);
        }

    }



}

