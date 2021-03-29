using CRM.Models;
using CRM.Utilities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ToastrUtilities = CRM.Utilities.ToastrUtilities;

namespace CRM
{
    public partial class transaction_lists : System.Web.UI.Page
    {
        public struct AjaxResponse
        {
            public bool Success { get; set; }
            public int Transaction_id { get; set; }
        }

        public string TotalTransactions = String.Empty;
        public StringBuilder TransactionsInformations = new StringBuilder();

        protected ToastrUtilities toastrUtilities = new ToastrUtilities();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ShowTransactions();
            OptionUtilities optionUtilities = new OptionUtilities();
            pspName.Items.AddRange(optionUtilities.GetPSPOptions());
        }


        public void ShowTransactions()
        {
            List<Transactions> transactions = new List<Transactions>();
            using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
            {
                string pspName = Request.QueryString["pspName"];
                string pspStatus = Request.QueryString["pspStatus"];
                string creditedStatus = Request.QueryString["creditedStatus"];
                string txtClientId = Request.QueryString["txtClientId"];
                string txtClientEmail = Request.QueryString["txtClientEmail"];
                string txtClientName = Request.QueryString["txtClientName"];
                string txtCreatedDate = Request.QueryString["txtCreatedDate"];
                string txtReferral = Request.QueryString["txtReferral"];

                string query = getQuery(conn, pspName, pspStatus, creditedStatus, txtClientId, txtClientEmail, txtClientName, txtCreatedDate,txtReferral);

                transactions = Transactions.GetTransactions(conn, query);

                TotalTransactions = Transactions.TransactionCount(conn, query);
            }

            List<Transactions> filteredTransactions = this.ApplyPagination(TransactionListPager, transactions, 10, "p", this.TotalTransactions);

            foreach (var transaction in filteredTransactions)
            {
                string created_date = transaction.Created_date == DateTime.MinValue ? "NONE" : transaction.Created_date.ToString();

                TransactionsInformations.Append("<tr>")
                .AppendFormat(@"<td>
                    <div class=""dropdown"">
                        <button class=""btn btn-sm btn-info dropdown-toggle"" type=""button"" id=""dropdownMenu1"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""true"">
                            Change Status
                        </button>
                        <ul class=""dropdown-menu"" aria-labelledby=""dropdownMenu1"">
                            <li><a href=""#!"" class=""change-transaction-status"" data-transaction-id=""{0}"" data-client-id=""{1}"">Change Trasanction Status</a></li>
                            <li role=""separator"" class=""divider""></li>
                            <li><a href=""#!"" class=""change-credited-transaction-status"" data-transaction-id=""{0}"">Change Credited Status</a></li>
                        </ul>
                    </div>
                </td>", transaction.Id, transaction.Client_id)
                .AppendFormat("<td><a href='/client-detail.aspx?id={0}' target='_blank'>{0}</a></td>", transaction.Client_id)
                .AppendFormat("<td>{0}</td>", transaction.PaymentReference)
                .AppendFormat("<td>{0}</td>", transaction.PspName)
                .AppendFormat("<td>{0}</td>", transaction.Fullname)
                 .AppendFormat("<td>{0}</td>", transaction.Referal)
                .AppendFormat("<td>{0}</td>", transaction.Deposit_currency)
                .AppendFormat("<td>{0}</td>", transaction.Deposit_amount)
                .AppendFormat("<td>{0}</td>", transaction.Exchange_currency)
                .AppendFormat("<td>{0}</td>", transaction.Exchange_amount)
                .AppendFormat("<td style='text-align:center;'>{0}</td>", !string.IsNullOrEmpty(transaction.CardholderName) ? transaction.CardholderName : "N/A")
                .AppendFormat("<td style='text-align:center;'>{0}</td>", !string.IsNullOrEmpty(transaction.CardLast4)? transaction.CardLast4:"N/A")
                .AppendFormat("<td>{0}</td>", transaction.Psp_status)
                .AppendFormat("<td>{0}</td>", !string.IsNullOrEmpty(transaction.Notes) ? transaction.Notes : "N/A")
                .AppendFormat("<td>{0}</td>", !string.IsNullOrEmpty(transaction.Type.ToString()) ? transaction.Type.ToString() : "N/A")
                .AppendFormat("<td>{0}</td>", created_date);

                TransactionsInformations.Append("</tr>");
                transactionBody.InnerHtml = TransactionsInformations.ToString();
            }
        }

        [System.Web.Services.WebMethod]
        public static AjaxResponse ChangeTransactionStatus(int tid, string selectedStatus, string cid)
        {
            using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
            {
                bool updateResult = Transactions.UpdatePspStatus(conn, tid, selectedStatus);

                AjaxResponse res = new AjaxResponse() { Success = updateResult, Transaction_id = tid };

                if (res.Success && selectedStatus == "Approved")
                {
                    //Get Transaction Informations
                    Transactions transaction = new Transactions();
                    transaction = Transactions.GetTransaction(conn, tid.ToString());

                    //Get Client Informations
                    ClientModel clients = new ClientModel();
                    clients = Clients.FindById(conn, Convert.ToInt32(cid));

                    //Send Email To Client
                    var message = EmailTemplateUtilities.GetDepositTemplate(transaction);
                    EmailTemplateUtilities.SendEmail(clients.Email, message, "Get-Bitcoin Successful Deposit");
                }

                return res;
            }
        }

        [System.Web.Services.WebMethod]
        public static AjaxResponse ChangeCreditedTransactionStatus(int tid, string selectedStatus)
        {
            using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
            {
                bool updateResult = Transactions.UpdateCreditedStatus(conn, tid, selectedStatus);

                AjaxResponse res = new AjaxResponse() { Success = updateResult, Transaction_id = tid };
                return res;
            }
        }

        [System.Web.Services.WebMethod]
        public static AjaxResponse ChangeNoteAndReff(int tid,string note,string ref_hash)
        {
            ToastrUtilities toastrUtilities = new ToastrUtilities();

            using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
            {

                #region Added Query
                string added_query = string.Empty;
                if (!string.IsNullOrEmpty(note))
                {
                    added_query += $"notes='{note}',";
                }
                else
                {
                    added_query += "notes=notes,";
                }

                if (!string.IsNullOrEmpty(ref_hash))
                {
                    added_query += $"payment_reference='{ref_hash}',";
                }
                else
                {
                    added_query += "payment_reference=payment_reference,";
                }
                #endregion

                string query = $"UPDATE transactions SET {added_query.TrimEnd(',')} WHERE id = {tid}";

                if (!string.IsNullOrEmpty(tid.ToString()))
                {
                    var sss = Classes.DB.Update(conn, query);
                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", "Transaction Updated"));
                }

                AjaxResponse res = new AjaxResponse() { Success = true, Transaction_id = tid };
                return res;
            }
        }


        protected List<Transactions> ApplyPagination(PaginationUtil pager, List<Transactions> list, int pageSize, string qkey, string total)
        {
            pager.ItemCountPerPage = pageSize;
            pager.QueryKey = qkey;
            pager.CurrentPage = pager.ParseQueryString(Request.QueryString[pager.QueryKey]);
            pager.TotalItems = int.Parse(total);
            int offset = pager.GetRecordOffset();
            return list.Skip(offset).Take(pager.ItemCountPerPage).ToList<Transactions>();
        }


        private string getQuery(NpgsqlConnection conn, string pspName, string pspStatus, string creditedStatus, string txtClientId, string txtClientEmail, string txtClientName, string txtCreatedDate,string txtReferral)
        {
            string query = string.Empty;

            if (pspName != String.Empty && pspName != null)
                query += String.Format(" AND ps.name = '{0}'", pspName);

            if (pspStatus != String.Empty && pspStatus != null)
                query += String.Format(" AND tr.psp_status = '{0}'", pspStatus);

            if (txtClientId != String.Empty && txtClientId != null)
                query += String.Format(" AND tr.client_id = '{0}'", txtClientId);

            if (txtCreatedDate != String.Empty && txtCreatedDate != null)
                query += String.Format(" AND to_char(tr.created_date,'YYYY-MM-DD') LIKE '%{0}%'", txtCreatedDate);

            if (txtClientEmail != String.Empty && txtClientEmail != null)
                query += String.Format(" AND cl.email LIKE '%{0}%'", txtClientEmail);

            if (txtClientName != String.Empty && txtClientName != null)
                query += String.Format(" AND cl.first_name LIKE '%{0}%'", txtClientName);
            if (creditedStatus != String.Empty && creditedStatus != null)
                query += String.Format(" AND tr.credited_status = '{0}'", creditedStatus);

            if (txtReferral != String.Empty && txtReferral != null)
                query += String.Format(" AND cl.referral = '{0}'", txtReferral);

            return query;
        }
    }
}