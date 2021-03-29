using CRM.Models;
using CRM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM
{
    public partial class ManageRoles : System.Web.UI.Page
    {
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {
            RoleUtilities roleUtilities = new RoleUtilities();
            OptionUtilities optionUtilities = new OptionUtilities();
            if (Request.HttpMethod == "POST")
            {
                string roleName = rolename.Value;
                string groupOption = groupoptions.Value;
                string roleType = Request.Form["type"];
                string roleLink = rolelink.Value;
                if (!string.IsNullOrEmpty(roleName) || !string.IsNullOrEmpty(groupOption) || !string.IsNullOrEmpty(roleType))
                {
                    string result = roleUtilities.InsertRole(roleName, groupOption, roleType, roleLink);
                    if (result == "Success")
                    {
                        ClearFields();
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", $"{roleName} Successfully Added."));
                    }
                    else
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"An Error Occured Please Contact System Administrator."));
                    }
                }
                else
                {
                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Please fill up all Fields."));
                }
            }
            ListItem[] gOptions = optionUtilities.GetGroupOptions();
            groupoptions.Items.Clear();
            groupoptions.Items.AddRange(gOptions);
            sgroupoptions.Items.Clear();
            sgroupoptions.Items.AddRange(gOptions);
            editgroupoptions.Items.Clear();
            editgroupoptions.Items.AddRange(gOptions);
            ShowRoles(roleUtilities.GetRoles());
        }

        private void ShowRoles(List<RolesModel> roles)
        {
            StringBuilder roleInformations = new StringBuilder();
            foreach (var role in roles)
            {
                roleInformations.Append("<tr>")
                    .AppendFormat("<td>{0}</td>", role.Id)
                    .AppendFormat("<td>{0}</td>", role.Name)
                    .AppendFormat("<td>{0}</td>", role.GroupName)
                    .AppendFormat("<td>{0}</td>", role.Type)
                    .AppendFormat("<td>{0}</td>", role.RoleLink)
                    .AppendFormat($"<td><a class='btn btn-xs btn-info' onClick=\"GetRole('{Classes.encryption.Encryption(role.Id.ToString())}')\" data-toggle='modal' data-target='#EditRoleModal'>EDIT</a></td>");
                roleInformations.Append("</tr>");
            }
            rolesBody.InnerHtml = roleInformations.ToString();
        }

        private void ClearFields()
        {
            rolename.Value = string.Empty;
            groupoptions.Value = string.Empty;
            rolelink.Value = string.Empty;
        }

        private static string ShowRolesTable(List<RolesModel> roles)
        {
            StringBuilder roleInformations = new StringBuilder();
            if (roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    roleInformations.Append("<tr>")
                        .AppendFormat("<td>{0}</td>", role.Id)
                        .AppendFormat("<td>{0}</td>", role.Name)
                        .AppendFormat("<td>{0}</td>", role.GroupName)
                        .AppendFormat("<td>{0}</td>", role.Type)
                        .AppendFormat("<td>{0}</td>", role.RoleLink)
                        .AppendFormat($"<td><a class='btn btn-xs btn-info' onClick=\"GetRole('{Classes.encryption.Encryption(role.Id.ToString())}')\" data-toggle='modal' data-target='#EditRoleModal'>EDIT</a></td>");
                    roleInformations.Append("</tr>");
                }
            }
            else
            {
                roleInformations.Append("<tr>")
                        .AppendFormat("<td colspan='6' class='text-center'><h2>NO RESULT</h2></td>");
                roleInformations.Append("</tr>");
            }
            return roleInformations.ToString();
        }


        [WebMethod]
        public static string UpdateRoles(string parameter, string roleName, string groupOption, string roleType, string roleLink)
        {
            RoleUtilities roleUtilities = new RoleUtilities();
            string roleInformations = string.Empty;
            if (!string.IsNullOrEmpty(roleName) || !string.IsNullOrEmpty(groupOption) || !string.IsNullOrEmpty(roleType))
            {
                string id = Classes.encryption.Decrypt(parameter);
                string result = roleUtilities.UpdateRole(id, roleName, groupOption, roleType, roleLink);
                if(result == "Success")
                {
                    roleInformations =  ShowRolesTable(roleUtilities.GetRoles());
                }
            }
            return roleInformations;
        }

        [WebMethod]
        public static RolesModel GetRole(string parameter)
        {
            RoleUtilities roleUtilities = new RoleUtilities();
            string id = Classes.encryption.Decrypt(parameter);
            RolesModel roleModel = roleUtilities.GetRole(id);
            return roleModel;
        }

        [WebMethod]
        public static string SearchRole(string roleName, string groupOption, string roleType, string roleLink)
        {
            RoleUtilities roleUtilities = new RoleUtilities();
            return ShowRolesTable(roleUtilities.SearchRoles("", roleName, groupOption, roleType, roleLink));
        }
    }
}