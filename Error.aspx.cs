using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM
{
    public partial class Error : System.Web.UI.Page
    {
        public string ErrorString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Error"] != null)
                ErrorString = Session["Error"].ToString();
        }
    }
}