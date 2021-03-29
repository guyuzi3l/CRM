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
    public partial class ManageUser : System.Web.UI.Page
    {
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        protected RoleUtilities roleUtilities = new RoleUtilities();
        protected List<RolesModel> rolesModels = new List<RolesModel>();
        protected IEnumerable<RolesModel> roleGroup;
        protected List<RolesModel> rolePrint = new List<RolesModel>();
        protected void Page_Load(object sender, EventArgs e)
        {
            UserUtility userUtility = new UserUtility();
            if (Request.HttpMethod == "POST")
            {
                string strRoles = Request.Form["Roles"];
                string strUsername = Request.Form["username"];
                string strPassword = Request.Form["password"];
                string strEmail = Request.Form["email"];

                UserModel userModel = new UserModel
                {
                    Username = strUsername,
                    Password = strPassword,
                    Email = strEmail,
                    Roles = strRoles
                };

                if (!string.IsNullOrEmpty(userModel.Username) || !string.IsNullOrEmpty(userModel.Password) || !string.IsNullOrEmpty(userModel.Email))
                {
                    if (userUtility.InsertUser(userModel) == "Success")
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", $"{strUsername.ToUpper()} Successfully Added."));
                    }
                    else
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"An Error Occured Please Contact System Administrator."));
                    }
                }
                else
                {
                    toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Please fill up all Fields!"));
                }

            }
            rolesModels = roleUtilities.GetRoles();
            roleGroup = rolesModels.GroupBy(x => x.GroupId).Select(y => y.First()).ToList();
            usersBody.InnerHtml = ShowUsers(userUtility.GetUsers()).ToString();
        }

        private StringBuilder ShowUsers(List<UserModel> users)
        {
            StringBuilder usersInformations = new StringBuilder();
            foreach (var user in users)
            {
                usersInformations.Append("<tr>")
                    .AppendFormat("<td>{0}</td>", user.Id)
                    .AppendFormat("<td>{0}</td>", user.Username)
                    .AppendFormat("<td>{0}</td>", user.Email)
                    .AppendFormat($"<td><a class='btn btn-xs btn-info' onClick=\"GetUser('{Classes.encryption.Encryption(user.Id.ToString())}')\">EDIT</a></td>");
                usersInformations.Append("</tr>");
            }
            return usersInformations;
        }

        private static string ShowUsersTable(List<UserModel> users)
        {
            StringBuilder usersInformations = new StringBuilder();
            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    usersInformations.Append("<tr>")
                    .AppendFormat("<td>{0}</td>", user.Id)
                    .AppendFormat("<td>{0}</td>", user.Username)
                    .AppendFormat("<td>{0}</td>", user.Email)
                    .AppendFormat($"<td><a class='btn btn-xs btn-info' onClick=\"GetUser('{Classes.encryption.Encryption(user.Id.ToString())}')\">EDIT</a></td>");
                    usersInformations.Append("</tr>");
                }
            }
            else
            {
                usersInformations.Append("<tr>")
                        .AppendFormat("<td colspan='4' class='text-center'><h2>NO RESULT</h2></td>");
                usersInformations.Append("</tr>");
            }
            return usersInformations.ToString();
        }

        [WebMethod]
        public static Dictionary<Object, Object> GetUser(string parameter)
        {
            Dictionary<Object, Object> dict = new Dictionary<Object, Object>();
            UserUtility userUtility = new UserUtility();
            string id = Classes.encryption.Decrypt(parameter);
            UserModel userModel = userUtility.GetUser(id);
            dict["UserModel"] = userModel;
            dict["ListRoles"] = userModel.Roles.Split(',').ToList();

            return dict;
        }

        [WebMethod]
        public static string UpdateUser(string parameter, string username, string password, string email, string roles)
        {
            UserUtility userUtility = new UserUtility();
            StringBuilder userInformations = new StringBuilder();
            if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(email))
            {
                string id = Classes.encryption.Decrypt(parameter);
                string result = userUtility.UpdateUser(id, username, password, email, roles);
                if (result == "Success")
                {
                    List<UserModel> users = userUtility.GetUsers();
                    foreach (var user in users)
                    {
                        userInformations.Append("<tr>")
                            .AppendFormat("<td>{0}</td>", user.Id)
                            .AppendFormat("<td>{0}</td>", user.Username)
                            .AppendFormat("<td>{0}</td>", user.Email)
                            .AppendFormat($"<td><a class='btn btn-xs btn-info' onClick=\"GetUser('{Classes.encryption.Encryption(user.Id.ToString())}')\">EDIT</a></td>");
                        userInformations.Append("</tr>");
                    }
                }
            }
            return userInformations.ToString();
        }

        [WebMethod]
        public static string SearchUser(string userName, string eMail)
        {
            UserUtility userUtility = new UserUtility();
            return ShowUsersTable(userUtility.SearchUsers("", userName, eMail));
        }
    }
}