using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string clientId = Classes.Cookie.GetCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", false);
            //Website.Utilities.SecurityUtilities.CheckLoginStatus(clientId, "/login.aspx");
            Classes.Cookie.DeleteCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y");
            Response.Redirect("/login.aspx");
        }
    }
}