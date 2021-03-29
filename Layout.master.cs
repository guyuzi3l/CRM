using CRM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}