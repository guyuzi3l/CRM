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
    public partial class create_conversion : Classes.Exceptions
    {
        protected string BitcoinBalance { get; set; }
        protected string EuroBalance { get; set; }
        protected string Result { get; set; }
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                #region Get POST Variable
                string client_id = Request.Form["clientId"];
                string from_currency = Request.Form["from_currency"];
                string to_currency = Request.Form["to_currency"];
                string credited_status = Request.Form["credited_status"];
                string desired_amount = Request.Form["amount"];
                #endregion


                using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
                {
                    ClientModel account = new ClientModel();
                    account = Clients.FindById(conn, int.Parse(client_id));

                    if (account == null)
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Invalid Client Id: {client_id}"));
                        return;
                    }

                    BitcoinBalance = Clients.GetUserBtcBalance(conn, account.Id.ToString());
                    EuroBalance = Clients.GetUserEurBalance(conn, account.Id.ToString());

                    #region Validating the Convert Params, should not be the same
                    if (from_currency == to_currency)
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Invalid Conversion Parameters"));
                        return;
                    }
                    #endregion

                    decimal exchange_amount = 0;
                    string transaction_currency = string.Empty;
                    string exchange_currency = string.Empty;


                    if (Convert.ToDecimal(BitcoinBalance) != 0 || Convert.ToDecimal(EuroBalance) != 0)
                    {
                        if (from_currency == "BTC" && to_currency == "EUR")
                        {
                            #region Check BTC Balance is Enough to Proceed with conversion
                            if (decimal.Parse(desired_amount) > decimal.Parse(BitcoinBalance))
                            {
                                toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Unable to perform request, Client not enough Balance."));
                                return;
                            }
                            if (decimal.Parse(desired_amount) <= 0)
                            {
                                toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Unable to perform request, Client not enough Balance."));
                                return;
                            }
                            #endregion

                            exchange_amount = decimal.Parse(Classes.Instbtc.Utilities.Conversion.KrakenBtcToEurConversion(desired_amount));
                            exchange_currency = "EUR";
                            transaction_currency = "BTC";
                        }
                        else if (from_currency == "EUR" && to_currency == "BTC")
                        {
                            #region Check EUR Balance is Enough to Proceed with conversion
                            if (decimal.Parse(desired_amount) > decimal.Parse(EuroBalance))
                            {
                                toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Unable to perform request, Client not enough Balance."));
                                return;
                            }
                            if (decimal.Parse(desired_amount) <= 0)
                            {
                                toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Unable to perform request, Client not enough Balance."));
                                return;
                            }
                            #endregion

                            exchange_amount = decimal.Parse(Classes.Instbtc.Utilities.Conversion.KrakenEurToBtcConversion(desired_amount));
                            exchange_currency = "BTC";
                            transaction_currency = "EUR";
                        }
                        else
                        {
                            toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Invalid Conversion Parameters"));
                            return;
                        }

                        #region Build Conversion Request Model
                        ConversionReqModel Model = new ConversionReqModel
                        {
                            client_name = $"{account.First_name} {account.Last_name}",
                            amount = Convert.ToDecimal(Convert.ToDouble(desired_amount)),
                            currency = transaction_currency,
                            created_date = DateTime.UtcNow,
                            transaction_id = null,
                            transaction_currency = transaction_currency,
                            credited_status = credited_status,
                            client_id = account.Id,
                            conversion_amount = exchange_amount,
                            conversion_currency = exchange_currency
                        };
                        #endregion

                        string cv_id = ConversionReqUtilities.CreateConversionRequest(Model);
                        Result = "Conversion Request Created";
                        if (!string.IsNullOrEmpty(cv_id))
                        {
                            if (credited_status.ToLower() == "approved")
                            {
                                Result = Utilities.ConversionReqUtilities.ProcessConversionRequest(cv_id, credited_status);
                            }
                        }
                        if(Result.Contains("Not Enough"))
                            toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"{Result}"));
                        else
                            toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", $"{Result}"));

                        Response.Redirect("/conversion-request.aspx");
                    }
                    else
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Unable to perform request, Client not enough Balance."));
                        return;
                    }

                }

            }
        }
    }
}