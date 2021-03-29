using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Utilities;
using CRM.Models;
using HelloSign;
using Npgsql;
using static CRM.Models.WithdrawalModel;
using Classes.Instbtc.Models;

namespace CRM
{
    public partial class withdrawal_request : System.Web.UI.Page
    {

        public struct AjaxResponse
        {
            public bool Success { get; set; }
            public int DocumentId { get; set; }
            public string Response { get; set; }
        }

        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        public string TotalRequest = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageChecker = Request.QueryString["page"];
            if (string.IsNullOrEmpty(pageChecker))
            {
                Session.Remove("WithdrawalSearch");
            }

            using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
            {

                NpgsqlCommand command = new NpgsqlCommand();
                string query = string.Empty;

                if (Request.HttpMethod == "POST")
                {
                    string status = txtStatus.Value;
                    string clientId = txtClientId.Value;
                    string clientEmail = txtClientEmail.Value;
                    string clientName = txtClientName.Value;
                    string walletId = txtWalletId.Value;
                    string documentId = txtWalletId.Value;
                    string createdDate = txtCreatedDate.Value;
                    string docStatus = txtDocStatus.Value;
                    string refferenceHash = txtHash.Value; 

                    WithdrawalSearchModel searchModel = new WithdrawalSearchModel()
                    {
                        Status = status,
                        ClientModel = new ClientModelSearch
                        {
                            Id = clientId,
                            Email = clientEmail
                        },
                        DocumentStatus = docStatus,
                        ClientName = clientName,
                        WalletId = walletId,
                        DocumentId = documentId,
                        CreatedDate = createdDate,
                        refference_hash = refferenceHash
                    };
                    Session["WithdrawalSearch"] = searchModel;
                }
                
                if (Session["WithdrawalSearch"] != null)
                {
                    WithdrawalSearchModel sessionSearchModel = (WithdrawalSearchModel)Session["WithdrawalSearch"];

                    //Get SQLCommand Parameters and Query Parameters
                    KeyValuePair<NpgsqlCommand, string> pair = getParameter(sessionSearchModel);
                    command = pair.Key;
                    query = pair.Value;
                }

                //Get Withdrawals Datas
                List<MWithdrawal> withdrawalRequest = new List<MWithdrawal>();
                withdrawalRequest = WithdrawalUtilities.WithdrawalRequest(conn, command, query);

                #region Pagination
                Dictionary<string, int> pagerDictionary = new Dictionary<string, int>();
                string paginationString = "page";
                DocListPager.initialize(paginationString, 10);
                TotalRequest = WithdrawalUtilities.GetWithdrawalCount(conn, command, query);
                pagerDictionary = DocListPager.paginate(TotalRequest);
                withdrawalRequest = WithdrawalUtilities.WithdrawalRequest(conn, command, query, pagerDictionary["offset"], pagerDictionary["limit"]);
                rowCount.InnerText = $"{TotalRequest} Records Found";
                resultBody.InnerHtml = BuildHtmlTable(withdrawalRequest).ToString();
                #endregion

                //Get Document Statuses Selection
                OptionUtilities optionUtilities = new OptionUtilities();
                txtDocStatus.Items.Clear();
                txtDocStatus.Items.AddRange(optionUtilities.GetDocumentStatusOptions());

                //Return Values to their Fields
                ReturnFieldValues();
            }
        }

        private void ReturnFieldValues(WithdrawalSearchModel sessionSearchModel = null)
        {
            if (sessionSearchModel is null)
            {
                sessionSearchModel = Session["WithdrawalSearch"] as WithdrawalSearchModel;
            }

            if (sessionSearchModel != null)
            {
                txtStatus.Value = sessionSearchModel.Status;
                txtClientId.Value = sessionSearchModel.ClientModel.Id;
                txtClientEmail.Value = sessionSearchModel.ClientModel.Email;
                txtClientName.Value = sessionSearchModel.ClientName;
                txtWalletId.Value = sessionSearchModel.WalletId;
                txtDocumentId.Value = sessionSearchModel.DocumentId;
                txtCreatedDate.Value = sessionSearchModel.CreatedDate;
                txtDocStatus.Value = sessionSearchModel.DocumentStatus;
            }
        }

        private static StringBuilder BuildHtmlTable(List<MWithdrawal> withdrawalRequest)
        {
            StringBuilder body = new StringBuilder();
            foreach (var t in withdrawalRequest)
            {
                string id = Classes.encryption.Encryption(t.Id.ToString());
                body.AppendFormat("<tr>")
                .AppendFormat("<tr>")
                    .AppendFormat(@"<td><div class=""dropdown"">
                                        <button class=""btn btn-sm btn-info dropdown-toggle"" type=""button"" id=""dropdownMenu1"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""true"">
                                            Options
                                        </button>
                                        <ul class=""dropdown-menu"" aria-labelledby=""dropdownMenu1"">");

                if (t.Status != "Approved")
                {
                    body.AppendFormat(@"<li><a href=""#!"" class=""class-change-status"" data-document-id=""{0}"">Change Status</a></li>", id)
                    .AppendFormat(@"<li role=""separator"" class=""divider""></li>");
                }

                body.AppendFormat(@"<li><a href='{0}'>{1}</a></li>
                                        </ul>
                                    </div></td>", !string.IsNullOrEmpty(t.DocumentId.ToString()) ? "/hello-documents.aspx?doc_id=" + t.DocumentId.ToString() + "&name="+t.ClientName : "#", string.IsNullOrEmpty(t.DocumentId)? "Document Not Available": "Download Document" )
                .AppendFormat("<td style='text-align:center'><a href='{0}'>CLICK HERE</a></td>", string.IsNullOrEmpty(t.DocuLink) ? "#" : t.DocuLink)
                .AppendFormat("<td>{0}</td>", t.Id)
                .AppendFormat("<td style='text-align:center'>{0} Wallet</td>", t.transaction_currency)
                .AppendFormat("<td>{0}</td>", t.ClientName)
                .AppendFormat("<td style='text-align:center'>{0}</td>", t.UserId)
                .AppendFormat("<td>{0}</td>", t.WalletId)
                .AppendFormat("<td>{0}</td>", (t.Amount - (t.ServiceFee ?? Convert.ToDecimal(0.00))))
                .AppendFormat("<td>{0}</td>", t.UsdConversion)
                .AppendFormat("<td>{0}</td>", t.refference_hash)
                .AppendFormat("<td style='text-align:center'>{0}</td>", t.Status)
                .AppendFormat("<td>{0}</td>", t.DocumentStatus.ToString().Replace("_", " "))
                .AppendFormat("<td>{0}</td>", t.Amount)
                .AppendFormat("<td>{0}</td>", t.ServiceFee ?? Convert.ToDecimal(0.00))
                .AppendFormat("<td>{0}</td>", t.CreatedDate)
                .AppendFormat("<td>{0}</td>", t.is_sign)
                .AppendFormat("<td>{0}</td>", $"<button onclick='viewFile({t.Id})'>View File</button>")
                .AppendFormat("<td>{0}</td>", t.CurrentBalance)
                .AppendFormat("<td>{0}</td>",t.EuroBalance)
                .AppendFormat("<td>{0}</td>", t.referral)
                .AppendFormat("</tr>");
            }
            return body;

        }

        public bool DownloadDocument(MWithdrawal withdrawal)
        {
            try
            {
                //Extract Initals of Client
                string clientInitials = AccountUtilities.ExtracInitials(withdrawal.ClientName);
                string dateTimeNow = withdrawal.CreatedDate.ToString("yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

                //Get File From Hellosign
                Client client = new Client("bbb7087eb89dd0c11f91cf037366f6e85eca26b38f84ea7a80a2566e66f52e8d");
                byte[] downloadDocument = client.DownloadSignatureRequestFiles(withdrawal.DocumentId, SignatureRequest.FileType.PDF);

                //Transmit File to client
                Response.Clear();
                Response.AddHeader("Content-Disposition", $"attachment; filename={dateTimeNow}{clientInitials}{withdrawal.Id}.pdf");
                Response.AddHeader("Refresh", "3; /withdrawal-request.aspx");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(downloadDocument);
                Response.AddHeader("Refresh", "3; url=index.html");
                Response.Flush();
                Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return true;
            }
            catch (Exception ex)
            {
                toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Document Not Found"));
                return false;
            }
        }

        private static KeyValuePair<NpgsqlCommand, string> getParameter(WithdrawalSearchModel sessionSearchModel)
        {
            ToastrUtilities toastrUtilities = new ToastrUtilities();
            
            string txtStatus = sessionSearchModel?.Status ?? string.Empty;
            string txtClientId = sessionSearchModel?.ClientModel?.Id ?? string.Empty;
            string txtClientEmail = sessionSearchModel?.ClientModel?.Email ?? string.Empty;
            string txtClientName = sessionSearchModel?.ClientName ?? string.Empty;
            string txtWalletId = sessionSearchModel?.WalletId ?? string.Empty;
            string txtDocumentId = sessionSearchModel?.DocumentId ?? string.Empty;
            string txtCreatedDate = sessionSearchModel?.CreatedDate ?? string.Empty;
            string txtDocStatus = sessionSearchModel?.DocumentStatus ?? string.Empty;
            string txtHash = sessionSearchModel?.refference_hash ?? string.Empty;

            string query = string.Empty;

            NpgsqlCommand command = new NpgsqlCommand();

            if (!string.IsNullOrEmpty(txtStatus))
            {
                command.Parameters.AddWithValue("@Status", txtStatus);
                query += " AND w.credited_status = @Status";
            }

            if (!string.IsNullOrEmpty(txtHash))
            {
                command.Parameters.AddWithValue("@refference_hash", txtHash);
                query += " AND w.refference_hash = @refference_hash";
            }

            if (!string.IsNullOrEmpty(txtDocStatus))
            {
                if (int.TryParse(txtDocStatus, out int ds))
                {
                    command.Parameters.AddWithValue("@docstatus", ds);
                    query += " AND w.document_status = @docstatus";
                }
            }

            if (!string.IsNullOrEmpty(txtClientId))
            {
                if (int.TryParse(txtClientId, out int cid))
                {
                    command.Parameters.AddWithValue("@ClientId", cid);
                    query += " AND w.user_id = @ClientId";
                }
                else
                {
                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Invalid Client ID"));
                }
            }

            if (!string.IsNullOrEmpty(txtClientEmail))
            {
                command.Parameters.AddWithValue("@ClientEmail", txtClientEmail);
                query += " AND c.email = @ClientEmail";
            }

            if (!string.IsNullOrEmpty(txtClientName))
            {
                command.Parameters.AddWithValue("@ClientName", txtClientName);
                query += " AND w.client_name = @ClientName";
            }

            if (!string.IsNullOrEmpty(txtWalletId))
            {
                command.Parameters.AddWithValue("@WalletId", txtWalletId);
                query += " AND w.wallet_id = @WalletId";
            }

            if (!string.IsNullOrEmpty(txtDocumentId))
            {
                if (int.TryParse(txtDocumentId, out int did))
                {
                    command.Parameters.AddWithValue("@DocumentId", did);
                    query += " AND w.document_id = @DocumentId";
                }
                else
                {
                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Invalid Document ID"));
                }
            }

            if (!string.IsNullOrEmpty(txtCreatedDate))
            {
                if (DateTime.TryParse(txtCreatedDate, out DateTime cd))
                {
                    command.Parameters.AddWithValue("@CreatedDate", cd);
                    query += " AND w.created_date::date = @CreatedDate";
                }
                else
                {
                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Invalid Date"));
                }
            }


            KeyValuePair<NpgsqlCommand, string> pair = new KeyValuePair<NpgsqlCommand, string>(command, query);

            return pair;
        }

        [WebMethod]
        public static AjaxResponse ChangeStatus(string id, string selectedStatus, string ref_hash)
        {
            string DecryptedId = Classes.encryption.Decrypt(id);
            
                using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
                {
                    //Update Status
                    bool updateResult = WithdrawalUtilities.UpdateCreditedStatus(conn, DecryptedId, selectedStatus,ref_hash);
                    if (updateResult)
                    {
                        if (selectedStatus == "Approved")
                        {
                            WithdrawalUtilities.UpdateUsdValueAmount(conn, DecryptedId);


                            var withdrawalData = WithdrawalUtilities.GetWithdrawal(conn, DecryptedId);

                            if (withdrawalData.TransactionId == -1 && withdrawalData.Status == "Approved")
                            {
                                var result = InsertTransaction(conn, DecryptedId, createTransferRecord: false);
                                if (result.ToString() != "Internal Error" && !string.IsNullOrEmpty(result.ToString()))
                                {
                                    var updateWithdrawalTransaction = WithdrawalUtilities.UpdateTransactionId(conn, DecryptedId, Convert.ToInt32(result));

                                    ToastrUtilities toastrUtilities = new ToastrUtilities();

                                    if (updateWithdrawalTransaction)
                                    {
                                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", "Successfully Updated Status"));
                                    }
                                    else
                                    {
                                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Encountered Error Update Status"));
                                    }
                                }
                            }


                        }
                    }

                    //Set Response
                    AjaxResponse res = new AjaxResponse() { Success = updateResult, DocumentId = Convert.ToInt32(DecryptedId) };
                    return res;
                }

        }

        [WebMethod]
        public static AjaxResponse ProcessWithdrawalRequest(string id, string selectedStatus, string ref_hash)
        {
            string DecryptedId = Classes.encryption.Decrypt(id);
            using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
            {
                var withdrawalData = WithdrawalUtilities.GetWithdrawal(conn, DecryptedId);
                bool updateResult = WithdrawalUtilities.UpdateCreditedStatus(conn, DecryptedId, selectedStatus, ref_hash);

                if (updateResult)
                {
                    if (selectedStatus == "Approved")
                    {
                        #region BTC Wallet Withdrawal Handler
                        if (withdrawalData.transaction_currency == "BTC")
                        {
                            WithdrawalUtilities.UpdateUsdValueAmount(conn, DecryptedId);
                            withdrawalData = WithdrawalUtilities.GetWithdrawal(conn, DecryptedId);

                            if (withdrawalData.TransactionId == -1 && withdrawalData.Status == "Approved")
                            {
                                var result = InsertTransaction(conn, DecryptedId, createTransferRecord: false);
                                if (result.ToString() != "Internal Error" && !string.IsNullOrEmpty(result.ToString()))
                                {
                                    var updateWithdrawalTransaction = WithdrawalUtilities.UpdateTransactionId(conn, DecryptedId, Convert.ToInt32(result));

                                    ToastrUtilities toastrUtilities = new ToastrUtilities();

                                    if (updateWithdrawalTransaction)
                                    {
                                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", "Successfully Updated Status"));
                                    }
                                    else
                                    {
                                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Encountered Error Update Status"));
                                    }
                                }
                            }
                        }
                        #endregion

                        #region EUR Wallet WithdrawalHandler
                        if (withdrawalData.transaction_currency == "EUR")
                        {
                            withdrawalData = WithdrawalUtilities.GetWithdrawal(conn, DecryptedId);
                            if (withdrawalData.TransactionId == -1 && withdrawalData.Status == "Approved")
                            {
                                var result = InsertTransaction(conn, DecryptedId, createTransferRecord: false);
                                if (result.ToString() != "Internal Error" && !string.IsNullOrEmpty(result.ToString()))
                                {
                                    var updateWithdrawalTransaction = WithdrawalUtilities.UpdateTransactionId(conn, DecryptedId, Convert.ToInt32(result));

                                    ToastrUtilities toastrUtilities = new ToastrUtilities();

                                    if (updateWithdrawalTransaction)
                                    {
                                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", "Successfully Updated Status"));
                                    }
                                    else
                                    {
                                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Encountered Error Update Status"));
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                AjaxResponse res = new AjaxResponse() { Success = updateResult, DocumentId = Convert.ToInt32(DecryptedId) };
                return res;
            }
          
        }

        public static object InsertTransaction(NpgsqlConnection conn, string id, bool createTransferRecord = false) 
        {
            MWithdrawal withdrawal = new MWithdrawal();
            withdrawal = WithdrawalUtilities.GetWithdrawal(conn, id);
            Classes.Instbtc.Models.TransactionModel transaction = new Classes.Instbtc.Models.TransactionModel();

            if (withdrawal.transaction_currency == "BTC")
            {
                #region Transaction Object BTC Wds
                transaction = new Classes.Instbtc.Models.TransactionModel()
                {
                    Psp_ID = 2,
                    Deposit_Currency = "USD",
                    Deposit_Amount = (decimal)withdrawal.UsdConversion * -1,
                    Exchange_Currency = "BTC",
                    Exchange_Amount = withdrawal.Amount * -1,
                    Created_Date = DateTime.UtcNow,
                    Client_ID = withdrawal.UserId,
                    Psp_Status = "Approved",
                    Credited_Status = "Credited",
                    type = TransactionType.WITHDRAWAL,
                    Transaction_Currency = withdrawal.transaction_currency
                };
                #endregion
            }

            else if (withdrawal.transaction_currency == "EUR")
            {
                #region Transaction Object BTC Wds
                transaction = new Classes.Instbtc.Models.TransactionModel()
                {
                    Psp_ID = 2,
                    Deposit_Currency = "EUR",
                    Deposit_Amount = withdrawal.Amount * -1,
                    Exchange_Currency = "EUR",
                    Exchange_Amount = withdrawal.Amount * -1,
                    Created_Date = DateTime.UtcNow,
                    Client_ID = withdrawal.UserId,
                    Psp_Status = "Approved",
                    Credited_Status = "Credited",
                    type = TransactionType.WITHDRAWAL,
                    Transaction_Currency = withdrawal.transaction_currency
                };
                #endregion
            }

            object res;

            if (createTransferRecord is false)
            {
                res = Classes.Instbtc.Create.Transactions.CreateTransaction(transaction);
                
            }
            else
            {
                res = Classes.Instbtc.Create.Transactions.CreateTransaction(transaction);

                if (res.ToString() != "Internal Error" && !string.IsNullOrEmpty(res.ToString()))
                {
                    bool isCreateTransferSuccess = TransferUtility.CreateTransfer((int) res, transaction);
                    if (isCreateTransferSuccess)
                        return res;
                    else
                        res = "Error";
                }
            }
            
            return res;
        }

        [WebMethod]
        public static string GetDocuFile(string id) 
        {
            string file = string.Empty;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc")) 
            {
                string query = $"SELECT signature FROM emails WHERE wd_id=@id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", int.Parse(id));
                file = Classes.DB.SelectScalar(conn, query, cmd)?.ToString() ?? "Sorry, No Signed Document as of the moment";
            }

            return file;
        }
    }
}