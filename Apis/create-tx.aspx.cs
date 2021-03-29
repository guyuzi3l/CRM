using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace CRM.Apis
{
    public partial class create_tx : Classes.Exceptions
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string resp = string.Empty;

            if (Request.HttpMethod == "POST") 
            {
                #region GET POST Variables
                var wire_amount = Request.Form["wire_amount"];
                var wire_currency = Request.Form["wire_currency"];
                var wire_ref = Request.Form["wire_reff"];
                var custom_param = Request.Form["custom_param"];
                #endregion

                #region Get Client UserId via the Custom Parameters
                var ClientId = string.Empty;
                if (custom_param.Contains("-"))
                    ClientId = custom_param.Substring(0, custom_param.IndexOf("-"));
                #endregion

                if (!string.IsNullOrEmpty(ClientId)) 
                {
                    #region Creating the Transaction Object
                    Classes.Instbtc.Models.TransactionModel Transaction = new Classes.Instbtc.Models.TransactionModel
                    {
                        Psp_ID = decimal.Parse("1"),
                        Deposit_Currency = wire_currency,
                        Deposit_Amount = Convert.ToDecimal(wire_amount),
                        Exchange_Currency = "BTC",
                        Exchange_Amount = Convert.ToDecimal(Classes.Instbtc.Utilities.Conversion.GetBTCAmountRestSharp(wire_amount, wire_currency)),
                        Created_Date = DateTime.UtcNow,
                        Client_ID = Convert.ToDecimal(ClientId),
                        Psp_Status = "Approved",
                        Credited_Status = "Credited",
                        PaymentReference = wire_ref,
                        Notes = $"Approved [{wire_ref}]",
                        type = Classes.Instbtc.Models.TransactionType.DEPOSIT,
                        CardLast4 = "",
                        CardHolderName = "",
                        Transaction_Currency = "BTC"

                    };
                    #endregion

                    var TransactionCheck = Utilities.Transactions.CheckTransactionIfExist(Transaction.PaymentReference);
                    object result = new object();
                    if (string.IsNullOrEmpty(TransactionCheck))
                    {
                        result = Classes.Instbtc.Create.Transactions.CreateTransaction(Transaction)?.ToString() ?? null;

                        if (!string.IsNullOrEmpty(result.ToString()) && result.ToString() != "Internal Error")
                        {
                            #region Api Response
                            resp = ser.Serialize(new
                            {
                                success = true,
                                id = result.ToString()
                            });
                            Response.ContentType = "application/json";
                            Response.Write(resp);
                            Response.End();
                            #endregion
                        }
                    }
                    else
                    {
                        #region Api Response
                        resp = ser.Serialize(new
                        {
                            success = false,
                            msg = "failed to create transaction, transaction reff already exist!."
                        });
                        Response.ContentType = "application/json";
                        Response.Write(resp);
                        Response.End();
                        #endregion
                    }
                }
            }
        }
    }
}