using CRM.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.Models;
using Website.Utilities;
using System.IO;

namespace CRM
{
    public partial class send_power_of_attorney : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageChecker = Request.QueryString["page"];
            if (string.IsNullOrEmpty(pageChecker))
            {
                Session.Remove("ClientSearch");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            string query = string.Empty;

            if (Request.HttpMethod == "POST")
            {
                string clientId = Request.Form["txtClientId"];
                string clientEmail = Request.Form["txtClientEmail"];
                string clientName = Request.Form["txtClientName"];
                string phoneNumber = Request.Form["txtPhoneNumber"];

                ClientModelSearch clientModel = new ClientModelSearch() {
                    Id = clientId,
                    Email = clientEmail,
                    First_name = clientName,
                    Phone_number = phoneNumber
                };
                Session["ClientSearch"] = clientModel;
            }

            if (Session["ClientSearch"] != null)
            {
                ClientModelSearch clientModel = (ClientModelSearch)Session["ClientSearch"];

                //Get SQLCommand Parameters and Query Parameters
                KeyValuePair<NpgsqlCommand, string> pair = getParameter(clientModel);
                command = pair.Key;
                query = pair.Value;

                //Return Values to their Fields
                //ReturnFieldValues(sessionSearchModel);
            }

            using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
            {
                List<ClientModel> clientModels = new List<ClientModel>();
                clientModels = Utilities.Clients.GetClients(conn, command, query);
                clientsBody.InnerHtml = BuildHtmlTable(clientModels).ToString();
            }
        }

        private static StringBuilder BuildHtmlTable(List<ClientModel> clientModels)
        {
            StringBuilder body = new StringBuilder();
            foreach (var t in clientModels)
            {
                body.Append("<tr>")
                .Append($"<td>{t.Id}</td>")
                .Append($"<td>{t.First_name} {t.Last_name}</td>")
                .Append($"<td>{t.Email}</td>")
                .Append($"<td>{t.Phone_prefix}{t.Phone_number}</td>")
                .Append($"<td><button type='button' class='btn btn-sm btn-info' onClick=\"SendPOA('{Classes.encryption.Encryption(t.Id.ToString())}')\">Send Power of Attorney</button></td>")
                .Append("</tr>");
            }
            return body;
        }

        private static KeyValuePair<NpgsqlCommand, string> getParameter(ClientModelSearch clientModel)
        {
            ToastrUtilities toastrUtilities = new ToastrUtilities();
            string txtClientId = string.Empty, txtClientEmail = string.Empty, txtClientName = string.Empty, txtPhoneNumber = string.Empty;
            txtClientId = clientModel.Id;
            txtClientEmail = clientModel.Email;
            txtClientName = clientModel.First_name;
            txtPhoneNumber = clientModel.Phone_number;

            string query = string.Empty;

            NpgsqlCommand command = new NpgsqlCommand();
            if (!string.IsNullOrEmpty(txtClientId))
            {
                try
                {
                    command.Parameters.AddWithValue("@ClientId", Convert.ToInt32(txtClientId));
                    query += " AND id = @ClientId";
                }
                catch
                {
                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Invalid Client ID"));
                }
            }

            if (!string.IsNullOrEmpty(txtClientEmail))
            {
                command.Parameters.AddWithValue("@ClientEmail", txtClientEmail);
                query += " AND email = @ClientEmail";
            }

            if (!string.IsNullOrEmpty(txtClientName))
            {
                command.Parameters.AddWithValue("@ClientName", txtClientName);
                query += " AND first_name::text || ' ' || last_name::text = @ClientName";
            }

            if (!string.IsNullOrEmpty(txtPhoneNumber))
            {
                command.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber);
                query += " AND phone_prefix::text || phone_number::text = @PhoneNumber";
            }

            KeyValuePair<NpgsqlCommand, string> pair = new KeyValuePair<NpgsqlCommand, string>(command, query);

            return pair;
        }

        [WebMethod]
        public static void SendPowerOfAttorney(string CardHolderName, string Amount, string LastFourDigits, string Parameter)
        {
            string id = Classes.encryption.Decrypt(Parameter);
            ClientModel client = new ClientModel();

            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                client = Utilities.Clients.FindById(conn, Convert.ToInt32(id));
            }

            PowerOfAttorneyModel powerOfAttorneyModel = new PowerOfAttorneyModel()
            {
                NameOfCardHolder = CardHolderName,
                NameOfClient = client.First_name + " " + client.Last_name,
                Amount = Amount,
                Date = DateTime.UtcNow,
                LastFourDigits = LastFourDigits
            };

            Website.Models.Accounts.Account accounts = new Website.Models.Accounts.Account() {
                FirstName = client.First_name,
                LastName = client.Last_name
            };

            PowerOfAttorneyGeneratedForm powerOfAttorneyGeneratedForm = new PowerOfAttorneyGeneratedForm();
            MemoryStream memoryStream = new MemoryStream();
            memoryStream = powerOfAttorneyGeneratedForm.CreatePackage(powerOfAttorneyModel);
            var bytes = memoryStream.ToArray();
            MessagingUtilities.SendEmailWithAttachment(client.Email, EmailTemplateUtilities.SendPowerOfAttorneyTemplate(accounts), "Power of Attorney", bytes);
        }
    }
}