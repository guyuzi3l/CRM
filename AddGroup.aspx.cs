using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Utilities;
using CRM.Models;
using Npgsql;
using System.Text;

namespace CRM
{
    public partial class AddGroup : System.Web.UI.Page
    {
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {
            GroupUtilities groupUtilities = new GroupUtilities();
            ShowGroups(groupUtilities.GetGroups());
            if (Request.HttpMethod == "POST")
            {
                string groupName = groupname.Value;
                if (!string.IsNullOrEmpty(groupName))
                {
                    if (!groupUtilities.VerifyGroupExists(groupName))
                    {
                        string result = groupUtilities.InsertGroup(groupName);
                        if (result == "Success")
                        {
                            toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", $"{groupName} Successfully Added."));
                        }
                    }
                    else
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"{groupName} Already Exists!"));
                    }
                }
                else
                {
                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", "Please Enter Group Name."));
                }
            }
        }

        private void ShowGroups(List<GroupsModel> groups)
        {
            StringBuilder groupInformations = new StringBuilder();
            foreach (var group in groups)
            {
                groupInformations.Append("<tr>")
                    .AppendFormat("<td>{0}</td>", group.Id)
                    .AppendFormat("<td>{0}</td>", group.Name);
                groupInformations.Append("</tr>");
                groupsBody.InnerHtml = groupInformations.ToString();
            }
        }

    }
}