using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using CRM.Models;
using CRM.Utilities;


namespace CRM
{
    public partial class CreateTransaction : Classes.Exceptions
    {
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {

            string username = Classes.Cookie.GetCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", false);
            var roles = Utilities.AccountUtilities.getUserRoles(username);
            RoleUtilities roleUtilities = new RoleUtilities();
            List<RolesModel> rolesModels = new List<RolesModel>();
            List<string> roleLists = roles.Split(',').ToList();

            if (roleLists.Find(f => f.ToLower() == "12") == null)
            {
                Response.Redirect("/dashboard.aspx");
            }



            if (Request.HttpMethod == "POST")
            {
                #region Assign Post Variables
                string ClientId = Request.Form["clientId"];
                string PaymentReferrence = Request.Form["pspRef"];
                string PaymentStatus = Request.Form["pspStatus"];
                string CreditedStatus = Request.Form["creditedStatus"];
                string PspId = Request.Form["ctl00$ctl00$MainContent$LMainContent$pspId"];
                string Amount = Request.Form["amount"];
                string Currency = Request.Form["currency"];
                string Note = Request.Form["note"];
                string CardLast4 = Request.Form["cardLast4"];
                string CardExpiry = Request.Form["cardExpiry"];
                string CardHolderName = Request.Form["cardHolder"];
                string TradingAccountId = Request.Form["PIN"];
                #endregion

                string PIN = !string.IsNullOrEmpty(TradingAccountId) ? TradingAccountId : string.Empty;

                #region Get Client Info
                ClientModel Info = new ClientModel();
                using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
                {
                    Info = Clients.FindById(conn, int.Parse(ClientId));
                }
                #endregion

                if (Info != null)
                {
                    #region Creating the Transaction Object
                    Classes.Instbtc.Models.TransactionModel Transaction = new Classes.Instbtc.Models.TransactionModel
                    {
                        Psp_ID = decimal.Parse(PspId),
                        Deposit_Currency = Currency,
                        Deposit_Amount = Convert.ToDecimal(Amount),
                        Exchange_Currency = "BTC",
                        Exchange_Amount = Convert.ToDecimal(Classes.Instbtc.Utilities.Conversion.GetBTCAmountRestSharp(Amount, Currency)),
                        Created_Date = DateTime.UtcNow,
                        Client_ID = Convert.ToDecimal(ClientId),
                        Psp_Status = PaymentStatus,
                        Credited_Status = CreditedStatus,
                        PaymentReference = PaymentReferrence,
                        Notes = Note,
                        type = Classes.Instbtc.Models.TransactionType.DEPOSIT,
                        CardLast4 = !string.IsNullOrEmpty(CardLast4) ? CardLast4 : "",
                        CardHolderName = !string.IsNullOrEmpty(CardHolderName) ? CardHolderName : "",
                        Transaction_Currency = "BTC"

                    };
                    #endregion

                    #region Create The Transaction
                    var TransactionCheck = Utilities.Transactions.CheckTransactionIfExist(Transaction.PaymentReference);
                    object result = new object();
                    if (string.IsNullOrEmpty(TransactionCheck))
                    {
                        result = Classes.Instbtc.Create.Transactions.CreateTransaction(Transaction);
                    }
                    else
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Cant Create Transaction, Existing Detected!"));
                        Response.Redirect("/transaction-lists.aspx");
                    }
                    
                    #endregion

                    if (result.ToString() != "Internal Error" || result != null)
                    {
                        decimal OriginalDepositTransactionAmount = decimal.Parse(Transaction.Deposit_Amount.ToString());
                        decimal OriginalBtcExchangeAmount = decimal.Parse(Transaction.Exchange_Amount.ToString());

                        EmailTemplateUtilities.SendNotificationDeposit(Info.Id.ToString(), OriginalDepositTransactionAmount.ToString(), OriginalBtcExchangeAmount.ToString(), Transaction);

                        //if (Info.Referral.ToLower() == "lblv" || Info.Referral.ToLower() == "tradershome" || Info.Referral.ToLower() == "profitix" || Info.Referral.ToLower() == "vlom" || Info.Referral.ToLower() == "uptos" || Info.Referral.ToLower() == "fundiza" || Info.Referral.ToLower() == "kiplar" || Info.Referral.ToLower() == "investigram" || Info.Referral.ToLower() == "commercewealth")
                        //{
                        //    try
                        //    {
                        //        var pspList = PspUtilities.GetPspById(Transaction.Psp_ID.ToString());
                        //        var pspName = pspList.Where(w => w.Id == int.Parse(Transaction.Psp_ID.ToString()))?.FirstOrDefault()?.Name;
                        //        pspName = pspName.Replace("USD ", "").Replace("EUR ", "").Replace("AUD ", "");
                        //        var res = BrandsIntegration.PushToBrand(Info, Transaction, PaymentStatus, result, $"[{pspName}] " + Note, PIN, pspName);
                        //        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", $"{res}"));
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"{ex.Message.ToString()}"));
                        //    }
                        //}
                        //else 
                        //{
                        //    decimal OriginalDepositTransactionAmount = decimal.Parse(Transaction.Deposit_Amount.ToString());
                        //    decimal OriginalBtcExchangeAmount = decimal.Parse(Transaction.Exchange_Amount.ToString());

                        //    EmailTemplateUtilities.SendNotificationDeposit(Info.Id.ToString(), OriginalDepositTransactionAmount.ToString(), OriginalBtcExchangeAmount.ToString(), Transaction);
                        //}
                    }
                    else
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"An Error Occured Cant Create Transaction"));
                    }
                }
                else
                {
                    //Redirect Invalid Client Id;
                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"No Client With this Id: {ClientId}"));
                    Response.Redirect("/transaction-lists.aspx");
                }

            }

            OptionUtilities optionUtilities = new OptionUtilities();
            pspId.Items.AddRange(optionUtilities.GetPSPOptions());
        }
    }
}