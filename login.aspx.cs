using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using CRM.Utilities;
namespace CRM
{
    public partial class login : System.Web.UI.Page
    {
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {
            string clientId = Classes.Cookie.GetCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", false);
            if (clientId == null)
            {
                if (Request.HttpMethod == "POST")
                {
                    using (var conn =Classes.DB.InstBTCDB("instbtc"))
                    {
                        string usernameInput = username.Value;
                        string passwordInput = password.Value;
                        string result = Utilities.AccountUtilities.LoginAccount(conn, usernameInput, passwordInput);
                        if (!string.IsNullOrEmpty(result))
                        {
                            toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", result));
                        }
                    };
                }
            }
            else
                Response.Redirect("/dashboard.aspx");
        }
    }
}