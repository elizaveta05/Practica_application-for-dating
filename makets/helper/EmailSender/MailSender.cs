using makets.helper.EmailSender.Enums;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace makets.helper.EmailSender
{
    

    class MailSender : IMailSender
    {

        string smtpServer = string.Empty;
        int smtpPort = 0;
        SmtpCredentials? creds = null;
        SMTPRouting route = null;
        SMTPContent content = null;

        public bool SendMail()
        {
            try
            {
                MailMessage mailMessage = new MailMessage(route.fromEmail, route.toEmail, content.Subject, content.Content);

                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(creds.Username, creds.Password);
                smtpClient.EnableSsl = true;

                smtpClient.Send(mailMessage);

                Console.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                return false;
            }
        }

        public MailSender WithAuthorization(SmtpCredentials creds)
        {
            this.creds = creds;
            return this;
        }

        public MailSender UseContent(SMTPContent content)
        {
            this.content = content;
            return this;
        } 

        public MailSender SetRouting(SMTPRouting routing)
        {
            this.route = routing;
            return this;
        }

        public MailSender WithDefaultPort()
        {
            this.smtpPort = 587;
            return this;
        }

        public MailSender UseSmtp(SMTPEnum smtp)
        {
            this.smtpServer = smtp.GetDescription();
            return this;
        }

        

        public static MailSender CreateSender()
        {
            return new MailSender();
        }

        private MailSender()
        {

        }
    }
}
