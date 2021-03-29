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
    public partial class SendTransaction : Classes.Instbtc.Exceptions
    {
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        public string SendTransactionResponse { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Classes.Cookie.GetCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", false);
            var roles = Utilities.AccountUtilities.getUserRoles(username);
            RoleUtilities roleUtilities = new RoleUtilities();
            List<RolesModel> rolesModels = new List<RolesModel>();
            List<string> roleLists = roles.Split(',').ToList();

            if (roleLists.Find(f => f.ToLower() == "15") == null) 
            {
                Response.Redirect("/dashboard.aspx");
            }


            if (Request.HttpMethod == "POST") 
            {
                #region Get Post Variable
                string amount = Request.Form["amount"];
                string currency = Request.Form["currency"];
                #endregion

                if (!string.IsNullOrEmpty(amount) && !string.IsNullOrEmpty(currency)) 
                {
                    try 
                    {
                        #region Proceed with Transfer with Desired Currency and Amount
                        var res = Classes.btcWallet.Utilitties.createSelfTransaction(double.Parse(amount), currency, "http://54.200.165.82:4000/");
                        #endregion

                        #region Show the Hash on the End User
                        SendTransactionResponse = res.txResponse.hash;
                        #endregion

                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", $"Process Successfully."));


                    }
                    catch (Exception ex)
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"{ex.Message.ToString()}"));
                    }
                }
            }
        }
    }
}