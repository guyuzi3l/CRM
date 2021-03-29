using CRM.Models;
using CRM.Utilities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.Utilities;
using static CRM.Models.WithdrawalModel;
using System.Data;
using System.Web.Services;
using ToastrUtilities = CRM.Utilities.ToastrUtilities;

namespace CRM
{
    public partial class client_detail : System.Web.UI.Page
    {
        public ClientModel Client { get; set; }
        public List<Documents> ClientDocuments { get; set; }
        public List<Transactions> ClientTransactions { get; set; }
        public List<MWithdrawal> ClientWithdrawals { get; set; }
        public bool IsUpdateSuccess { get; set; }

        protected string BtcBalance = string.Empty;
        protected string EurBalance = string.Empty;
        protected TransactionUtilities transactionUtilities = new TransactionUtilities();
        protected List<KeyValuePair<string, decimal>> CurrencyBalance = new List<KeyValuePair<string, decimal>>();

        public StringBuilder ClientDocsBuilder = new StringBuilder();


        public struct AjaxResponse
        {
            public bool Success { get; set; }
            public int DocumentId { get; set; }
            public string Response { get; set; }
        }


        protected ToastrUtilities toastrUtilities = new ToastrUtilities();

        protected void Page_Load(object sender, EventArgs e)
        {
            int clientId;

            this.IsUpdateSuccess = (Session["update_success"] != null) ? (bool)(Session["update_success"]) : false;

            if (int.TryParse(Request.QueryString["id"], out clientId) == false)
                this.Redirect();

            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                this.Client = Clients.FindById(conn, clientId);

                BtcBalance = CRM.Utilities.Transactions.GetUserBtcBalance(conn, clientId.ToString());
                EurBalance = CRM.Utilities.Transactions.GetUserEurBalance(conn, clientId.ToString());
                CurrencyBalance = Classes.Instbtc.Utilities.Conversion.GetCurrencies(BtcBalance);

                if (this.Client != null)
                {
                    this.ClientDocuments = Documents.GetDocuments(conn, $" AND dc.client_id = {this.Client.Id}");
                    this.ClientTransactions = Transactions.GetTransactions(conn, $"AND tr.client_id = { this.Client.Id}");
                }

                NpgsqlCommand command = new NpgsqlCommand();
                List<MWithdrawal> withdrawalRequest = new List<MWithdrawal>();
                string query = $"SELECT w.*,e.html,e.signature FROM withdrawal w INNER JOIN clients c ON w.user_id = c.id LEFT JOIN emails e on e.wd_id = w.id WHERE w.user_id = {clientId} ORDER BY w.created_date DESC";

                withdrawalRequest = WithdrawalRequest(conn, command, query);
                witdrawalBody.InnerHtml = BuildHtmlTable(withdrawalRequest).ToString();
            }

            if (this.Client == null)
                this.Redirect();
            Session["update_success"] = false;

            if (Request.HttpMethod == "POST")
            {
                Session["update_success"] = false;

                if (this.UpdateClient(Client))
                {
                    Session["update_success"] = true;
                    Response.Redirect(HttpContext.Current.Request.Url.PathAndQuery);
                }
            }

            this.ShowDocs();
            this.ShowTransactions();
        }

        private void ShowTransactions()
        {
            StringBuilder TransactionsInformations = new StringBuilder();

            if (this.ClientTransactions.Count > 0)
            {
                foreach (var transaction in this.ClientTransactions)
                {
                    string created_date = transaction.Created_date == DateTime.MinValue ? "NONE" : transaction.Created_date.ToString();

                    TransactionsInformations.Append("<tr>")
                    .AppendFormat(@"<td class=""hideable"">
                    <div class=""dropdown"">
                        <button class=""btn btn-sm btn-info dropdown-toggle"" type=""button"" id=""dropdownMenu1"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""true"">
                            Options
                        </button>
                        <ul class=""dropdown-menu"" aria-labelledby=""dropdownMenu1"">
                            <li><a href=""#!"" class=""change-transaction-status"" data-transaction-id=""{0}"" data-client-id=""{1}"">Change Trasanction Status</a></li>
                            <li role=""separator"" class=""divider""></li>
                            <li><a href=""#!"" class=""change-credited-transaction-status"" data-transaction-id=""{0}"">Change Credited Status</a></li>
                            <li role=""separator"" class=""divider""></li>
                            <li><a href=""#!"" class=""change-transaction-note-ref"" data-transaction-id=""{0}"">Edit Transaction Reff and Note</a></li>
                        </ul>
                    </div>
                </td>", transaction.Id, transaction.Client_id)
                .AppendFormat("<td>{0}</td>", transaction.Client_id)
                .AppendFormat("<td>{0}</td>", transaction.PaymentReference)
                .AppendFormat("<td>{0}</td>", transaction.PspName)
                .AppendFormat("<td>{0}</td>", transaction.Fullname)
                .AppendFormat("<td>{0}</td>", transaction.Deposit_currency)
                .AppendFormat("<td>{0}</td>", transaction.Deposit_amount)
                .AppendFormat("<td>{0}</td>", transaction.Exchange_currency)
                .AppendFormat("<td>{0}</td>", transaction.Exchange_amount)
                .AppendFormat("<td style='text-align:center;'>{0}</td>", !string.IsNullOrEmpty(transaction.CardholderName) ? transaction.CardholderName : "N/A")
                .AppendFormat("<td style='text-align:center;'>{0}</td>", !string.IsNullOrEmpty(transaction.CardLast4) ? transaction.CardLast4 : "N/A")
                .AppendFormat("<td>{0}</td>", transaction.Psp_status)
                .AppendFormat("<td>{0}</td>", !string.IsNullOrEmpty(transaction.Notes) ? transaction.Notes : "N/A")
                .AppendFormat("<td>{0}</td>", !string.IsNullOrEmpty(transaction.Type.ToString()) ? transaction.Type.ToString() : "N/A")
                .AppendFormat("<td>{0}</td>", created_date);

                    TransactionsInformations.Append("</tr>");
                    transactionBody.InnerHtml = TransactionsInformations.ToString();
                }
            }
            else
            {
                transactionBody.InnerHtml = "<tr><td colspan='10'><p class='text-danger text-center'>No results found.</p></td></tr>";
            }
        }

        private static StringBuilder BuildHtmlTable(List<MWithdrawal> withdrawalRequest)
        {
            StringBuilder body = new StringBuilder();
            if (withdrawalRequest.Count > 0)
            {
                foreach (var t in withdrawalRequest)
                {
                    string id = Classes.encryption.Encryption(t.Id.ToString());
                    body.AppendFormat("<tr>")
                    .AppendFormat("<tr>")
                        .AppendFormat(@"<td class=""hideable-1""><div class=""dropdown"">
                                        <button class=""btn btn-sm btn-info dropdown-toggle"" type=""button"" id=""dropdownMenu1"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""true"">
                                            Options
                                        </button>
                                        <ul class=""dropdown-menu"" aria-labelledby=""dropdownMenu1"">");

                    if (t.Status != "Approved")
                    {
                        body.AppendFormat(@"<li><a href=""#!"" class=""class-change-status"" data-document-id=""{0}"">Change Status</a></li>", id)
                        .AppendFormat(@"<li role=""separator"" class=""divider""></li>");
                    }

                    body.AppendFormat(@"<li><a href=""#!"" class=""class-change-ref"" data-wd-id=""{0}"">Edit Referrence</a></li>", id)
                        .AppendFormat(@"<li role=""separator"" class=""divider""></li>");

                    body.AppendFormat(@"<li><a href='{0}'>{1}</a></li>
                                        </ul>
                                    </div></td>", !string.IsNullOrEmpty(t.DocumentId.ToString()) ? "/hello-documents.aspx?doc_id=" + t.DocumentId.ToString() + "&name=" + t.ClientName : "#", string.IsNullOrEmpty(t.DocumentId) ? "Document Not Available" : "Download Document")
                    .AppendFormat("<td class='hideable-2' style='text-align:center'><a href='{0}'>CLICK HERE</a></td>", string.IsNullOrEmpty(t.DocuLink) ? "#" : t.DocuLink)
                    .AppendFormat("<td>{0}</td>", t.Id)
                    .AppendFormat("<td>{0} Wallet</td>", t.transaction_currency)
                    .AppendFormat("<td>{0}</td>", t.ClientName)
                    .AppendFormat("<td style='text-align:center'>{0}</td>", t.UserId)
                    .AppendFormat("<td class='hideable-3'>{0}</td>", t.WalletId)
                    .AppendFormat("<td>{0}</td>", (t.Amount - (t.ServiceFee ?? Convert.ToDecimal(0.00))))
                    .AppendFormat("<td>{0}</td>", t.UsdConversion)
                    .AppendFormat("<td style='text-align:center'>{0}</td>", t.Status)
                    .AppendFormat("<td>{0}</td>", t.DocumentStatus.ToString().Replace("_", " "))
                    .AppendFormat("<td>{0}</td>", t.Amount)
                    .AppendFormat("<td>{0}</td>", t.refference_hash)
                    .AppendFormat("<td>{0}</td>", t.ServiceFee ?? Convert.ToDecimal(0.00))
                    .AppendFormat("<td>{0}</td>", t.CreatedDate)
                    .AppendFormat("<td class='hideable-4'>{0}</td>", t.is_sign)
                    .AppendFormat("<td class='hideable-5'>{0}</td>", $"<button onclick='viewFile({t.Id})'>View File</button>")
                    .AppendFormat("</tr>");
                }
            }
            else
            {
                body.AppendFormat("<tr><td colspan='10'><p class='text-danger text-center'>No results found.</p></td></tr>");
            }
            return body;

        }

        private void ShowDocs()
        {
            foreach (var doc in this.ClientDocuments)
            {
                this.ClientDocsBuilder.Append("<tr>")
                    .AppendFormat("<td>{0}</td>", doc.Id)
                    .AppendFormat("<td>{0}</td>", doc.Type)
                    .AppendFormat("<td>{0}</td>", doc.Status)
                    .AppendFormat("<td>{0}</td>", doc.CreatedDate)
                    .AppendFormat("<td>{0}</td>", doc.UpdateDate)
                    .AppendFormat("<td>{0}</td>", doc.ExpiryDate)
                    .AppendFormat("<td>{0}</td>", doc.SubType)
                    .AppendFormat("<td>{0}</td>", doc.Note)
                     .AppendFormat("<td>{0}</td>", doc.CardLastFour)
                    .AppendFormat("<td><a href='/manage-documents.aspx?id={0}' class='btn btn-sm btn-info'>Manage</a></td>", doc.Id);
                this.ClientDocsBuilder.Append("</tr>");
                documentsBody.InnerHtml = this.ClientDocsBuilder.ToString();
            }
        }

        protected void Redirect()
        {
            Response.Redirect("/client-lists.aspx");
        }

        private bool UpdateClient(ClientModel Client)
        {
            string LoginUserName = Classes.encryption.Decrypt(Classes.Cookie.GetCookie("username", false));
            Client.First_name = Request.Form["c_first_name"];
            Client.Last_name = Request.Form["c_last_name"];
            Client.Phone_number = Request.Form["c_phone_number"];
            Client.Id = Convert.ToInt32(Request.Form["id"]);
            Client.Email = String.IsNullOrEmpty(Request.Form["c_email_add"]) ? null : Classes.encryption.EncryptSecure(Request.Form["c_email_add"], LoginUserName, Client.Id.ToString(), "email");
            Client.EmailHash = String.IsNullOrEmpty(Request.Form["c_email_add"]) ? null : Classes.encryption.Hash(Request.Form["c_email_add"]);
            Client.Phone_prefix = Request.Form["c_phone_prefix"];
            Client.Country = Request.Form["c_country"];
            Client.Max_deposit = Request.Form["c_max_deposit"];
            Client.Referral = Request.Form["c_referral"];

            bool returnValue = false;

            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                if (!string.IsNullOrEmpty(Client.First_name) && !string.IsNullOrEmpty(Client.Last_name) && !string.IsNullOrEmpty(Client.Phone_number) && !string.IsNullOrEmpty(Client.Email) && !string.IsNullOrEmpty(Client.Country) && Client.Id > 0)
                {
                    Clients clientClass = new Clients();
                    returnValue = clientClass.UpdateClients(conn, Client);
                }
            }
            return returnValue;
        }

        public static List<MWithdrawal> WithdrawalRequest(NpgsqlConnection con, NpgsqlCommand command, string query)
        {
            List<MWithdrawal> withdrawalRequest = new List<MWithdrawal>();

            //Get Given Parameters Collection
            NpgsqlParameterCollection parameters = command.Parameters;

            //Pass Parameter Collection into Current parameter
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            foreach (NpgsqlParameter parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.NpgsqlValue);
            }

            //Fetch Withdrawal Table
            DataTable dt = Classes.DB.Select(con, query, cmd);
            if (dt.Rows.Count > 0)
            {
                foreach (var item in dt.AsEnumerable())
                {
                    var i = item.Field<int?>("id");
                    if (i != null)
                    {
                        var wr = new MWithdrawal();

                        wr.Id = item.Field<int>("id");
                        wr.WalletId = item.Field<string>("wallet_id");
                        wr.UserId = item.Field<int>("user_id");
                        wr.ClientName = item.Field<string>("client_name");
                        wr.DocumentId = item.Field<string>("document_id");
                        wr.Status = item.Field<string>("credited_status");
                        wr.DocumentStatus = (DocumentStatus)item.Field<int>("document_status");
                        wr.DocuLink = item.Field<string>("document_link");
                        wr.CreatedDate = item.Field<DateTime>("created_date");
                        wr.Amount = item.Field<decimal>("amount");
                        wr.UsdConversion = item.Field<decimal?>("usd_conversion");
                        wr.ServiceFee = item.Field<decimal?>("service_fee");
                        wr.refference_hash = !string.IsNullOrEmpty(item.Field<string>("refference_hash")) ? item.Field<string>("refference_hash") : "";
                        wr.transaction_currency = !string.IsNullOrEmpty(item.Field<string>("transaction_currency")) ? item.Field<string>("transaction_currency") : "";
                        wr.html = !string.IsNullOrEmpty(item.Field<string>("html")) ? item.Field<string>("html") : "";
                        wr.signature = !string.IsNullOrEmpty(item.Field<string>("signature")) ? item.Field<string>("signature") : "";
                        wr.is_sign = !string.IsNullOrEmpty(item.Field<string>("is_sign")) ? item.Field<string>("is_sign") : "NO";
                        withdrawalRequest.Add(wr);
                    }
                }
            }
            return withdrawalRequest.OrderByDescending(o => o.CreatedDate).ToList(); ;
        }

        [WebMethod]
        public static AjaxResponse EditReference(string id, string ref_hash)
        {
            AjaxResponse res = new AjaxResponse();
            var WithdrawalId = Classes.encryption.Decrypt(id);
            ToastrUtilities toastrUtilities = new ToastrUtilities();

            if (!string.IsNullOrEmpty(WithdrawalId) && !string.IsNullOrEmpty(ref_hash))
            {
                string UpdateQuery = "UPDATE withdrawal SET refference_hash = @Reference WHERE id = @Id; ";
                using (var con = Classes.DB.InstBTCDB("instbtc"))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(UpdateQuery, con);
                    cmd.Parameters.AddWithValue("@Reference", ref_hash);
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(WithdrawalId));
                    var qResult = Classes.DB.Update(con, UpdateQuery, cmd);

                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", "Successfully Updated Referrence"));
                    res = new AjaxResponse() { Success = true, DocumentId = int.Parse(WithdrawalId) };
                }
            }
            else
            {
                toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Failed to Update Referrence"));
                res = new AjaxResponse() { Success = false, DocumentId = 0 };
            }



            return res;
        }

    }
}