using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CRM.Utilities
{
    public class EmailTemplateUtilities
    {
        public static string GetRegistrationTemplate(CRM.Models.ClientModel account)
        {
            string bodyTemplate = $"<p><b>Hello {account.First_name} {account.Last_name}, </b><p>"
                + $" <p> You have successfully registered an account with InstBTC.io. Verify your account now and purchase Bitcoin anytime, anywhere! </p>"
                + $"<p>Use the following credentials below to log in to your account:</p>"
                + $"<p> <b>Email:</b> <a href='mailto:{account.Email}' style='color: #f7931a;'>{account.Email}</a><br />"
                + $"<b>Password: </b>{account.Password} <br>"
                + $"<b>Wallet Address: </b>{account.walletAddress}</p>"
                + $"<br> Regards, <br> "
                + $"<b style='color: #f7931a;'>InstBTC Team</b>";

            return generalEmailTemplate(bodyTemplate);
        }

        public static string GetApproveStatusTemplate(ClientModel clients, string status)
        {
            string bodyTemplate = $"<h3>Hello {clients.First_name} {clients.Last_name},</h3><br/>" +
                                  $"<p>Thank you for choosing InstBTC. The documents you have submitted have been found valid and your application has already been approved by the company’s review team. You can now access your account at InstBTC. </p>" +
                                  $"<br/><p>Should you have concerns or further questions regarding your account, please contact us via email at <a href=\"mailto:info@instbtc.io\" target=\"_blank\" style=\"color: #f7931a;\">info@instbtc.io</a>, Live Chat, or phone.</p><br/>" +
                                  $"<p><b>24/5 Live Support:</b> Chat with Us<br> <b>E-Mail:</b> <a href=\"mailto:info@instbtc.io\" target=\"_blank\" style=\"color: #f7931a;\">info@instbtc.io</a><br>" +
                                  $"<b>Phone:</b> + 3728804565</p> <p>Regards, <br> <b style=\"color: #f7931a;\">InstBTC Team</b></p> </font> <br> <br>";
            return generalEmailTemplate(bodyTemplate);
        }

        public static string GetDocumentDeclinedTemplate(ClientModel clients, string status)
        {
            string bodyTemplate = $"<h3>Hello {clients.First_name} {clients.Last_name},</h3> <p>Thank you for choosing InstBTC. Unfortunately, your application has been declined by our review team due to specific reasons such as inaccurate information, invalid documents, insufficient details, and erroneous input, among other things. " +
                                  $"</p> <p>To know the precise reason for the rejection of your application, please contact us via email at <a href=\"mailto:info@instbtc.io\" target=\"_blank\" style=\"color: #f7931a;\">info@instbtc.io</a>, Live Chat, or phone. </p> " +
                                  $"<p><b>24/5 Live Support:</b> Chat with Us<br> <b>E-Mail:</b> <a href=\"mailto:info@instbtc.io\" target=\"_blank\" style=\"color: #f7931a;\">info@instbtc.io</a><br> <b>Phone:</b> + 3728804565</p> <p>Regards, <br> <b style=\"color: #f7931a;\">InstBTC Team</b></p> </font> <br> <br>";
            return generalEmailTemplate(bodyTemplate);
        }

        public static string GetDepositTemplate(Transactions transaction)
        {
            string bodyTemplate = $"<h3>Hello {transaction.Fullname},</h3>" +
                                  $"<p>You have successfully deposited <b>{transaction.Deposit_amount} {transaction.Deposit_currency.ToUpper()}</b>, with reference number <b>{DateTime.Now.Year}{transaction.Id}</b>. </p>" +
                                  $"<p>Thank you for choosing InstBTC.io. For further inquiries or assistance, please feel free to contact <a href=\"mailto:info@instbtc.io\" target=\"_blank\" style=\"color: #f7931a;\">info@instbtc.io</a>.</p>" +
                                  $"<p>Regards, <br> <b style=\"color: #f7931a;\">InstBTC Team</b></p> </font> <br> <br>";
            return generalEmailTemplate(bodyTemplate);
        }

        private static string generalEmailTemplate(string bodyTemplate)
        {
            string generalTemplate = "<!DOCTYPE html> <html> <head> <meta http-equiv=\"Content-type\" content=\"text/html; charset=utf-8\" /> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1\" /> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /> <meta name=\"format-detection\" content=\"date=no\" /> <meta name=\"format-detection\" content=\"address=no\" /> <meta name=\"format-detection\" content=\"telephone=no\" /> <meta name=\"x-apple-disable-message-reformatting\" /> <link href=\"https://fonts.googleapis.com/css?family=Noto+Sans:400,400i,700,700i\" rel=\"stylesheet\" /> <title>InstBTC - Email Template</title> <style type=\"text/css\" media=\"screen\"> body { padding:0 !important; margin:0 !important; display:block !important; min-width:100% !important; width:100% !important; background:#f4f4f4; -webkit-text-size-adjust:none } a { color:#66c7ff; text-decoration:none } p { padding:0 !important; margin:0 !important } img { -ms-interpolation-mode: bicubic; /* Allow smoother rendering of resized image in Internet Explorer */ } .mcnPreviewText { display: none !important; } @media only screen and (max-device-width: 480px), only screen and (max-width: 480px) {.mobile-shell { width: 100% !important; min-width: 100% !important; } .bg { background-size: 100% auto !important; -webkit-background-size: 100% auto !important; } .text-header, .m-center { text-align: center !important; } .center { margin: 0 auto !important; } .container { padding: 20px 10px !important } .td { width: 100% !important; min-width: 100% !important; } .m-br-15 { height: 15px !important; } .p30-15 { padding: 30px 15px !important; } .p40 { padding: 20px !important; } .m-td, .m-hide { display: none !important; width: 0 !important; height: 0 !important; font-size: 0 !important; line-height: 0 !important; min-height: 0 !important; } .m-block { display: block !important; } .fluid-img img { width: 100% !important; max-width: 100% !important; height: auto !important; } .column, .column-top, .column-empty, .column-empty2, .column-dir-top { float: left !important; width: 100% !important; display: block !important; } .column-empty { padding-bottom: 10px !important; } .column-empty2 { padding-bottom: 20px !important; } .content-spacing { width: 15px !important; } } </style> </head> <body class=\"body\" style=\"padding:0 !important; margin:0 !important; display:block !important; min-width:100% !important; width:100% !important; background:#f4f4f4; -webkit-text-size-adjust:none;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#f4f4f4\"> <tr> <td align=\"center\" valign=\"top\"> <table width=\"650\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"mobile-shell\"> <tr> <td class=\"td container\" style=\"width:650px; min-width:650px; font-size:0pt; line-height:0pt; margin:0; font-weight:normal; padding:55px 0px;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"p30-15\" style=\"padding: 25px 30px 25px 30px;\" bgcolor=\"#ffffff\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <th class=\"column-top\" width=\"145\" style=\"font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal; vertical-align:top;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"img m-center\" style=\"font-size:0pt; line-height:0pt; text-align:left;\"><img src=\"https://instbtc.io/assets/images/logo-1.png\" width=\"200\" height=\"80\" border=\"0\" alt=\"\" /></td> </tr> </table> </th> <th class=\"column-empty\" width=\"1\" style=\"font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal; vertical-align:top;\"></th> <th class=\"column\" style=\"font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"text-header\" style=\"color:#999999; font-family:'Noto Sans', Arial,sans-serif; font-size:12px; line-height:16px; text-align:right; text-transform:uppercase;\"><a href=\"https://instbtc.io/\" target=\"_blank\" class=\"link2\" style=\"color:#999999; text-decoration:none;\"><span class=\"link2\" style=\"color:#999999; text-decoration:none;\">Visit website</span></a></td> </tr> </table> </th> </tr> </table> </td> </tr> </table> </td> </tr> </table> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td background=\"https://instbtc.io/assets/images/1.jpg\" bgcolor=\"#114490\" valign=\"top\" height=\"366\" class=\"bg\" style=\"background-size:cover !important; -webkit-background-size:cover !important; background-repeat:none;\"> <div> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"content-spacing\" width=\"30\" height=\"366\" style=\"font-size:0pt; line-height:0pt; text-align:left;\"></td> <td style=\"padding: 30px 0px;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"h1 center pb25\" style=\"color:#ffffff; font-family:'Noto Sans', Arial,sans-serif; font-size:40px; line-height:46px; text-align:center; padding-bottom:25px;\">Refine <b>World-Class</b> Innovation</td> </tr> <tr> <td class=\"text-center\" style=\"color:#ffffff; font-family:'Noto Sans', Arial,sans-serif; font-size:16px; line-height:30px; text-align:center; padding-bottom: 5px;\">Trade on InstBTC's exchange using only one account.</td> </tr> </table> </td> <td class=\"content-spacing\" width=\"30\" style=\"font-size:0pt; line-height:0pt; text-align:left;\"></td> </tr> </table> </div> </td> </tr> <tr> <td class=\"mp15\" style=\"padding: 20px 30px;\" bgcolor=\"#f6cb00\" align=\"center\"> <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <th class=\"column\" style=\"font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"h5 white\" style=\"font-family:'Noto Sans', Arial,sans-serif; font-size:16px; line-height:22px; text-align:left; font-weight:bold; color:#ffffff;\">Faster Transaction</td> </tr> </table> </th> <th class=\"column\" width=\"50\" style=\"font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal;\"></th> <th class=\"column\" style=\"font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"h5 white\" style=\"font-family:'Noto Sans', Arial,sans-serif; font-size:16px; line-height:22px; text-align:left; font-weight:bold; color:#ffffff;\">Well-Secured</td> </tr> </table> </th> <th class=\"column\" width=\"50\" style=\"font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal;\"></th> <th class=\"column\" style=\"font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"h5 white\" style=\"font-family:'Noto Sans', Arial,sans-serif; font-size:16px; line-height:22px; text-align:left; font-weight:bold; color:#ffffff;\">Experts Support</td> </tr> </table> </th> </tr> </table> </td> </tr> </table> </td> </tr> </table> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\"> <tr> <td class=\"p30-15\" style=\"padding: 50px 30px;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"text pb20\" style=\"color:#777777; font-family:'Noto Sans', Arial,sans-serif; font-size:14px; line-height:26px; text-align:left; padding-bottom:20px;\"> " + $" {bodyTemplate}" + " </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"p30-15\" style=\"padding: 50px 30px;\" bgcolor=\"#0e0e0e\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td align=\"center\" style=\"padding-bottom: 30px;\"> <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"img\" width=\"55\" style=\"font-size:0pt; line-height:0pt; text-align:left;\"><a href=\"https://instbtc.io/\"><img src=\"https://instbtc.io/assets/images/logo-2.png\" width=\"200\" height=\"70\" border=\"0\" alt=\"\" /></a></td> </tr> </table> </td> </tr> <tr> <td class=\"text-footer1 pb10\" style=\"color:#999999; font-family:'Noto Sans', Arial,sans-serif; font-size:12px; line-height:20px; text-align:center; padding-bottom:10px;\">The webiste is operated by BHPT OÜ in Estonia, with registered address at Harju maakond, Tallinn, Kesklinna linnaosa, Roosikrantsi tn 2-K541, 10119 Estonia, and licensed to provide services of exchanging a virtual currency as well as virtual currency wallet service, with registration number 14514010.</td> </tr> <tr> <td class=\"text-footer2 pb30\" style=\"color:#999999; font-family:'Noto Sans', Arial,sans-serif; font-size:12px; line-height:26px; text-align:center;\"><span style=\"color: #f6cb00;\">Location:</span> Harju country, Estonia</td> </tr> <tr> <td class=\"text-footer2 pb30\" style=\"color:#999999; font-family:'Noto Sans', Arial,sans-serif; font-size:12px; line-height:26px; text-align:center;\"\"><span style=\"color: #f6cb00;\">Email Address:</span> info@instbtc.io</td> </tr> <tr> <td class=\"text-footer2 pb30\" style=\"color:#999999; font-family:'Noto Sans', Arial,sans-serif; font-size:12px; line-height:26px; text-align:center;\"><span style=\"color: #f6cb00;\">Phone:</span> +372 880 4565</td> </tr> </table> </td> </tr> </table> <!-- END Footer --> </td> </tr> </table> </td> </tr> </table> </body> </html>";


            return generalTemplate;
        }

        public static void SendNotificationDeposit(string UserId, string amountUsd, string amountBtc, Classes.Instbtc.Models.TransactionModel Transaction)
        {
            #region Get Client Info Via UserId

            ClientModel Info = new ClientModel();
            using (var con = Classes.DB.InstBTCDB("instbtc")) 
            { 
                Info = Clients.FindById(con,int.Parse(UserId));
            }
            #endregion

            #region Build email Template
            string Template = $"<h3>Dear {Info.First_name} {Info.Last_name},</h3>"
                + $"<p>Your deposit of ${Transaction.Deposit_Amount} has been successfully recieved and the following amount will be available on your account.</p>"
                + $"<p>{amountBtc} BTC = ${amountUsd}</p>"
                + $"<p>As per your request the purchased bitcoin is available in your BTC wallet.</p>"
                + $"<p>Please don't hesitate to contact us should you have any questions. </p>"
                + $"<p>We look forward to doing business with you again soon!</p><br>"
                + $"<br>With regards,<br> <b style=\"color: #f7931a;\">Instbtc Team</b>";

            var body = generalEmailTemplate(Template);
            #endregion

            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            try
            {
                message.From = new MailAddress("info@instbtc.io");
                (message.To).Add(new MailAddress(Info.Email));

                //if(Transaction.Psp_ID == 1)
                //(message.Bcc).Add("deposits@zerocap.io");

                message.Subject = string.Concat("Deposit Confirmation/Purchase Notification ", "[", Transaction.PaymentReference, "]");
                message.IsBodyHtml = true;
                message.Body = body;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential("info@instbtc.io", "bG5dDe8kjycEqBHZ");
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
            }
            catch
            {
            }

        }

        public static void SendEmail(string email, string message, string subject, string mailCredentialEmail = "info@instbtc.io", string mailCredentialPassword = "urY9S>SW")
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                mailMessage.From = new MailAddress(mailCredentialEmail);
                (mailMessage.To).Add(new MailAddress(email));
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = message;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(mailCredentialEmail, mailCredentialPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
        }
    }
}