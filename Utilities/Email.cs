using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using CRM.Models;

namespace CRM.Utilities
{
    public class Email
    {
        public static void SendNoFundsNotification(string UserId, string bridgeMessage)
        {
            ClientModel info = new ClientModel();
            using (var conn = Classes.DB.InstBTCDB("instbtc"))
            {
                info = Clients.FindById(conn, int.Parse(UserId));
            }

            string body = "<b>Brand: </b>Instbtc.io<br>"
                                                  + "<b>Client Name:</b> " + string.Concat(info.First_name, " ", info.Last_name) + "<br>"
                                                  + "<b>Clients Email:</b> " + info.Email + "<br>"
                                                  + "<b>BTC Bridge Message:</b> " + bridgeMessage + "<br>";
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            try
            {
                message.From = new MailAddress("info@instbtc.io");
                (message.To).Add(new MailAddress("crypto@vvxm.com"));
                message.Subject = string.Concat("InstBTC - No Funds for bridge ", "[", UserId, "]");
                message.IsBodyHtml = true;
                message.Body = body;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential("info@instbtc.io", "urY9S>Ss");
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
            }
            catch
            {
            }
        }
    }
}