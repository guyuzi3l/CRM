using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace CRM.Utilities
{
    public class EmailTransact
    {

        public void GetEmailDetails(string email, string subject, string message)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                mailMessage.From = new MailAddress("support@gwumkt.com");
                (mailMessage.To).Add(new MailAddress(email));
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = message;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential("support@gwumkt.com", "S1a123ss");
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                //LOG ERROR MESSAGE
                HttpContext.Current.Response.Write(ex.Message);
            }
        }
    }
}