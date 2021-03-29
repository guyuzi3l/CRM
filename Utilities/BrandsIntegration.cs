using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using System.Web.Script.Serialization;
using System.Collections;
using System.Net;
using CRM.Models;
using Npgsql;

namespace CRM.Utilities
{
    public class BrandsIntegration
    {
        public static string BrandMakeDeposit(string brand, string email, string status, string amount, string currency, string payment_referrence, string holder, string ccnum, string expiry, string note, string pin,decimal service_fee,string psp_name)
        {
            string result = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string api_url = $"https://payments.{brand}.com/cryptosites/instbtc/process-deposit.aspx";

            #region Build Request Parameters
            string parameters = string.Empty;
            Hashtable post_values = new Hashtable();

            post_values.Add("email", email);
            post_values.Add("status", status);
            post_values.Add("amount", amount);
            post_values.Add("currency", currency);
            post_values.Add("payment_referrence", payment_referrence);
            post_values.Add("cardholdername", holder);
            post_values.Add("cardLast4", ccnum);
            post_values.Add("cardexpiry", expiry);
            post_values.Add("note", note);
            post_values.Add("pin", pin);
            post_values.Add("psp_name", $"IBTC_{psp_name.Replace(" ","")}");
            post_values.Add("service-fee", service_fee);

            String post_string = "";
            foreach (DictionaryEntry field in post_values)
            {
                post_string += field.Key + "=" + field.Value + "&";
            }
            post_string = post_string.TrimEnd('&');
            parameters = post_string;
            #endregion

            #region Perform the Request
            var client = new RestClient(api_url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", parameters, ParameterType.RequestBody);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            IRestResponse response = client.Execute(request);
            #endregion

            return result;
        }


        public static string PushToBrand(Models.ClientModel Info, Classes.Instbtc.Models.TransactionModel Transaction, string PspStatus, object result, string note, string pin,string psp_name)
        {
            if (result.ToString() != "Internal Error" || result != null)
            {
                decimal OriginalDepositTransactionAmount = decimal.Parse(Transaction.Deposit_Amount.ToString());
                decimal OriginalBtcExchangeAmount = decimal.Parse(Transaction.Exchange_Amount.ToString());

                decimal DepositAmountDeductionFee = 0;
                decimal BTCExchangeAmountDeductionFee = 0;


                if (psp_name.ToLower() == "wire transfer")
                    Transaction.Psp_ID = 1;

                #region Get Right Deduction Fee
                if (Transaction.Psp_ID == 1)
                {
                    DepositAmountDeductionFee = (OriginalDepositTransactionAmount * Convert.ToDecimal(0.025));
                    BTCExchangeAmountDeductionFee = (OriginalBtcExchangeAmount * Convert.ToDecimal(0.025));
                }
                else
                {
                    DepositAmountDeductionFee = (OriginalDepositTransactionAmount * Convert.ToDecimal(0.05));
                    BTCExchangeAmountDeductionFee = (OriginalBtcExchangeAmount * Convert.ToDecimal(0.05));
                }
                #endregion

                decimal FinalDepositAmountToSend = (OriginalDepositTransactionAmount - DepositAmountDeductionFee);
                decimal FinalBTCAmountToSend = (OriginalBtcExchangeAmount - BTCExchangeAmountDeductionFee);

                #region If not Approved Send Api Request to Brand to record tx
                if (PspStatus.ToLower() != "approved")
                {
                    var s = BrandMakeDeposit(Info.Referral, Info.Email, PspStatus, FinalDepositAmountToSend.ToString(), Transaction.Deposit_Currency, Transaction.PaymentReference, Transaction.CardHolderName, Transaction.CardLast4, "", note, pin, DepositAmountDeductionFee,psp_name);
                }
                #endregion

                #region If Approved Continue the Process
                if (PspStatus.ToLower() == "approved")
                {
                    string btcReff = "";

                    #region Make the BTC transfer
                    var res = Classes.btcWallet.Utilitties.createSelfTransaction(double.Parse(FinalBTCAmountToSend.ToString()), "http://54.200.165.82:4000/");

                    #region Funds were transfered
                    if (res.error == null)
                        btcReff = res.txResponse.hash;
                    #endregion

                    #region No funds
                    else if (res.error != null)
                        Email.SendNoFundsNotification(Transaction.Client_ID.ToString(), res.error.message);
                    #endregion

                    #region if tx unconfirmed error persist
                    if (res.error != null) 
                    {
                        if (res.error.message.ToLower() == "tx exceeds maximum unconfirmed ancestors.")
                        {

                            #region do ZAP call
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            var client = new RestClient("http://54.200.165.82:4000/zap"); 
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/x-www-form-urlencoded");
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                            IRestResponse response = client.Execute(request);

                            var dss = ser.Deserialize<dynamic>(response.Content.ToString());
                            if (dss != null) 
                            {
                                var s = dss.TryGetValue("success", out dynamic sc) ? sc : null;
                                if ((bool)s)
                                {
                                    res = Classes.btcWallet.Utilitties.createSelfTransaction(double.Parse(FinalBTCAmountToSend.ToString()), "http://54.200.165.82:4000/");
                                    if (res.error == null)
                                        btcReff = res.txResponse.hash;
                                }
                                else 
                                {
                                    btcReff = res.txResponse.hash = "N/A";
                                }
                            }
                            #endregion
                           
                        }
                    }
                    #endregion

                    #endregion
                    if (Transaction.Psp_ID != 1)
                        EmailTemplateUtilities.SendNotificationDeposit(Info.Id.ToString(), FinalDepositAmountToSend.ToString(), FinalBTCAmountToSend.ToString(), Transaction);

                    #region Create Withdrawal Object
                    WithdrawalModel.MWithdrawal WithdrawalModel = new WithdrawalModel.MWithdrawal
                    {
                        UserId = Info.Id,
                        WalletId = "Not Applicable",
                        ClientName = string.Concat(Info.First_name, " ", Info.Last_name),
                        DocumentId = "Not Applicable",
                        DocuLink = "Not Applicable",
                        Amount = Transaction.Exchange_Amount,
                        Status = "Initial",
                        DocumentStatus = CRM.Models.WithdrawalModel.DocumentStatus.OUT_FOR_SIGNATURE,
                        CreatedDate = DateTime.UtcNow,
                        UsdConversion = 0,
                        ServiceFeeUsd = 0,
                        ServiceFee = BTCExchangeAmountDeductionFee,
                        refference_hash = btcReff,
                        transaction_currency = "BTC"
                    };
                    #endregion

                    #region Create Initial Wd Request
                    var wd_id = CreateWdRequestReturningId(WithdrawalModel);
                    #endregion

                    if (!string.IsNullOrEmpty(wd_id.ToString()))
                    {
                        using (var conn = Classes.DB.InstBTCDB("instbtc"))
                        {
                            bool updateResult = UpdateCreditedStatus(conn, wd_id.ToString(), PspStatus);
                            if (updateResult)
                            {
                                UpdateUsdValueAmount(conn, wd_id.ToString());
                                var withdrawalData = GetWithdrawal(conn, wd_id.ToString());
                                if (withdrawalData.TransactionId == -1 && withdrawalData.Status == "Approved")
                                {
                                    var wd_result = InsertTransaction(conn, wd_id.ToString());

                                    if (wd_result.ToString() != "Internal Error" && !string.IsNullOrEmpty(wd_result.ToString()))
                                    {
                                        var updateWithdrawalTransaction = UpdateTransactionId(conn, wd_id.ToString(), Convert.ToInt32(wd_result));

                                        if (updateWithdrawalTransaction)
                                        {
                                            var brand_deposit_res = BrandMakeDeposit(Info.Referral, Info.Email, PspStatus, FinalDepositAmountToSend.ToString(), Transaction.Deposit_Currency, Transaction.PaymentReference, Transaction.CardHolderName, Transaction.CardLast4, "", string.Concat("[Refference:", btcReff, "] [Service Fee: ", DepositAmountDeductionFee, "] ", note), pin, DepositAmountDeductionFee,psp_name);
                                        }
                                        else
                                        {
                                            //logs issues
                                        }

                                    }
                                }
                            }
                        }
                    }

                }
                #endregion

            }
            return "Success";
        }

        private static object CreateWdRequestReturningId(WithdrawalModel.MWithdrawal Model)
        {
            object result;
            string query = $"INSERT INTO withdrawal(id,wallet_id,user_id,client_name,document_id,credited_status,document_status,document_link,created_date,amount,usd_conversion,service_fee,service_fee_usd,refference_hash,transaction_currency) " +
               $"VALUES(default,'{Model.WalletId}',{Model.UserId},'{Model.ClientName}','','{"Initial"}',{1},'','{DateTime.UtcNow}',{Model.Amount},{Model.UsdConversion},{Model.ServiceFee},{Model.ServiceFeeUsd},'{Model.refference_hash}','{Model.transaction_currency}') returning id;";
            using (var con = Classes.DB.InstBTCDB("instbtc"))
            {
                try
                {
                    result = Classes.DB.SelectScalar(con, query);
                }
                catch (Exception ex)
                {
                    result = "Internal Error";
                }
            }
            return result;
        }

        private static bool UpdateCreditedStatus(NpgsqlConnection conn, string id, string status)
        {
            string query = "UPDATE withdrawal SET credited_status = @Status WHERE id = @Id;";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                var qResult = Classes.DB.Update(conn, query, cmd);
            }
            catch (Exception e)
            {
                string message = e.Message;
                return false;
            }

            return true;
        }

        private static WithdrawalModel.MWithdrawal GetWithdrawal(NpgsqlConnection conn, string id)
        {
            WithdrawalModel.MWithdrawal withdrawal = new WithdrawalModel.MWithdrawal();
            string query = "SELECT id, wallet_id, transaction_id, user_id, client_name, document_id, credited_status, document_status, document_link, created_date, amount, usd_conversion, service_fee,refference_hash,transaction_currency FROM withdrawal WHERE id=@Id ORDER BY created_date DESC";
            NpgsqlCommand command = new NpgsqlCommand(query, conn);
            command.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            using (NpgsqlDataReader reader = Classes.DB.ExecuteReader(conn, query, command))
            {
                if (reader.Read())
                {
                    withdrawal.Id = Convert.ToInt32(reader["id"]);
                    withdrawal.WalletId = reader["wallet_id"].ToString();
                    withdrawal.UserId = Convert.ToInt32(reader["user_id"]);
                    withdrawal.ClientName = reader["client_name"].ToString();
                    withdrawal.DocumentId = reader["document_id"].ToString();
                    withdrawal.Status = reader["credited_status"].ToString();
                    withdrawal.DocumentStatus = (WithdrawalModel.DocumentStatus)reader["document_status"];
                    withdrawal.DocuLink = reader["document_link"].ToString();
                    withdrawal.CreatedDate = Convert.ToDateTime(reader["created_date"]);
                    withdrawal.Amount = Convert.ToDecimal(reader["amount"]);
                    withdrawal.ServiceFee = Convert.ToDecimal(reader["service_fee"]);
                    withdrawal.TransactionId = !string.IsNullOrEmpty(reader["transaction_id"].ToString()) ? Convert.ToInt32(reader["transaction_id"]) : -1;
                    withdrawal.UsdConversion = !string.IsNullOrEmpty(reader["usd_conversion"].ToString()) ? Convert.ToDecimal(reader["usd_conversion"]) : 0;
                    withdrawal.refference_hash = !string.IsNullOrEmpty(reader["refference_hash"].ToString()) ? reader["refference_hash"].ToString() : "";
                    withdrawal.transaction_currency = !string.IsNullOrEmpty(reader["transaction_currency"].ToString()) ? reader["transaction_currency"].ToString() : "";
                }
            }

            return withdrawal;
        }

        private static bool UpdateTransactionId(NpgsqlConnection conn, string id, int transactionId)
        {
            string query = "UPDATE withdrawal SET transaction_id = @TransactionId WHERE id = @Id;";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TransactionId", transactionId);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                var qResult = Classes.DB.Update(conn, query, cmd);
            }
            catch (Exception e)
            {
                string message = e.Message;
                return false;
            }

            return true;
        }

        private static bool UpdateUsdValueAmount(NpgsqlConnection conn, string id)
        {

            WithdrawalModel.MWithdrawal withdrawal = new WithdrawalModel.MWithdrawal();
            withdrawal = GetWithdrawal(conn, id);

            #region Make the convertion to usd
            decimal? WithAmount = withdrawal.Amount - withdrawal.ServiceFee;
            withdrawal.UsdConversion = Convert.ToDecimal(Classes.Instbtc.Utilities.Conversion.GetCurrencyAmount(WithAmount.ToString(), "USD"));
            withdrawal.ServiceFeeUsd = Convert.ToDecimal(Classes.Instbtc.Utilities.Conversion.GetCurrencyAmount(withdrawal.ServiceFee.ToString(), "USD"));
            #endregion

            string query = "UPDATE withdrawal SET usd_conversion = @usdAmount, service_fee_usd = @serviceFeeUsd WHERE id = @Id;";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@usdAmount", withdrawal.UsdConversion);
                cmd.Parameters.AddWithValue("@serviceFeeUsd", withdrawal.ServiceFeeUsd);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                var qResult = Classes.DB.Update(conn, query, cmd);
            }
            catch (Exception e)
            {
                string message = e.Message;
                return false;
            }

            return true;
        }

        private static object InsertTransaction(NpgsqlConnection conn, string id)
        {
            WithdrawalModel.MWithdrawal withdrawal = new WithdrawalModel.MWithdrawal();
            withdrawal = GetWithdrawal(conn, id);
            Classes.Instbtc.Models.TransactionModel transaction = new Classes.Instbtc.Models.TransactionModel();

            if (withdrawal.transaction_currency == "BTC")
            {
                #region Build Transaction Object BTC Wds
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
                    type = Classes.Instbtc.Models.TransactionType.WITHDRAWAL,
                    Transaction_Currency = withdrawal.transaction_currency
                };
                #endregion
            }

            else if (withdrawal.transaction_currency == "EUR")
            {
                #region Build Transaction Object EUR Wds
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
                    type = Classes.Instbtc.Models.TransactionType.WITHDRAWAL,
                    Transaction_Currency = withdrawal.transaction_currency
                };
                #endregion
            }

            object res;
            res = Classes.Instbtc.Create.Transactions.CreateTransaction(transaction);
            return res;
        }
    }
}