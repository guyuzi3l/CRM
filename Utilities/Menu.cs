using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CRM.Utilities;
using CRM.Models;

namespace CRM.Utilities
{
    public class Menu
    {

        public static string GetCurrentPage()
        {
            return HttpContext.Current.Request.Path;
        }

        public static void CallDashboard()
        {
            Dictionary<string, string> icons = new Dictionary<string, string>()
            {
                { "admin","fa fa-user" },
                { "support","fa fa-phone" },
                { "finance","fa fa-credit-card" },
                { "accounts","fa fa-users" },
                { "roles","fa fa-database" }
            };

            string roles = "";
            Utilities.AccountUtilities.checkUserLogin();
            string username = Classes.Cookie.GetCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", false);
            roles = Utilities.AccountUtilities.getUserRoles(username);

            #region General Things
            HttpContext.Current.Response.Write("<aside class='main-sidebar'>");
            HttpContext.Current.Response.Write("<section class='sidebar'>");
            HttpContext.Current.Response.Write("<div class='user-panel'>");
            HttpContext.Current.Response.Write("<div class='pull-left image'><img src='/img/thumbnail.jpg' class='img-circle' alt='User Image'></div>");
            HttpContext.Current.Response.Write("<div class='pull-left info'><p>" + username + "</p></div>");
            HttpContext.Current.Response.Write("</div>");

            HttpContext.Current.Response.Write("<ul class='sidebar-menu tree' data-widget='tree'>");
            HttpContext.Current.Response.Write("<li class='header'>InstBTC MAIN NAVIGATION</li>");

            HttpContext.Current.Response.Write("<li class='active treeview'> <a href='/dashboard.aspx'> <i class='fa fa-dashboard'></i> <span>DASHBOARD</span> </a> </li>");
            #endregion

            RoleUtilities roleUtilities = new RoleUtilities();
            List<RolesModel> rolesModels = new List<RolesModel>();
            List<string> roleLists = roles.Split(',').ToList();
            rolesModels = roleUtilities.GetRoles().Where(x => x.Type == "TAB").ToList().Where(c => roleLists.Contains(c.Id.ToString())).ToList();
            string[] nestedPagesName = rolesModels.Select(c => c.RoleLink).Select(y => y.ToUpperInvariant()).ToArray();
            IEnumerable<RolesModel> roleGroup = rolesModels.GroupBy(x => x.GroupId).Select(y => y.First()).ToList();

            string currentPage = GetCurrentPage();
            string addedCssClass = default(string);
            string innerAddedCssClass = default(string);
            foreach (var tab in roleGroup)
            {
                if (nestedPagesName.Contains(currentPage.ToUpperInvariant()))
                {
                    //addedCssClass += "active menu-open";
                }

                System.Web.HttpContext.Current.Response.Write($"<li class='treeview {addedCssClass}'> <a href='#'> <i class='{ icons[tab.GroupName.ToLower()] }'></i> <span>{tab.GroupName.ToUpper()}</span> <i class='fa fa-angle-left pull-right'></i> </a>");
                HttpContext.Current.Response.Write("<ul class='treeview-menu'>");
                List<RolesModel> tabPrint = new List<RolesModel>();
                tabPrint = rolesModels.Where(c => c.GroupId == tab.GroupId).ToList();
                foreach(var innerTab in tabPrint)
                {
                    if (currentPage.ToUpper() == innerTab.RoleLink.ToUpper())
                    {
                        innerAddedCssClass = "active";
                    }
                    else
                    {
                        innerAddedCssClass = string.Empty;
                    }

                    System.Web.HttpContext.Current.Response.Write($"<li class='{innerAddedCssClass}'><a href='{innerTab.RoleLink}'><i class='fa fa-circle-o'></i> {innerTab.Name.ToUpper()}</a></li>");

                }
                System.Web.HttpContext.Current.Response.Write("</ul></li>");
            }
            //int mone = 0;
            // string currentPage = GetCurrentPage();
            #region ADMIN TAB
            //if (roles.IndexOf(",1.0,") != -1)
            //{
            //    mone++;
            //    string[] nestedPagesName = new string[] { "client-lists.aspx", "document-lists.aspx", "transaction-lists.aspx" };
            //    string addedCssClass = default(string);

            //    if (nestedPagesName.Contains(currentPage))
            //        addedCssClass += "active menu-open";

            //    System.Web.HttpContext.Current.Response.Write("<li class='treeview " + addedCssClass + "'> <a href='#'> <i class='fa fa-gears'></i> <span>ADMIN</span> <i class='fa fa-angle-left pull-right'></i> </a>");
            //    HttpContext.Current.Response.Write("<ul class='treeview-menu'>");
            //}

            //if (roles.IndexOf(",1.1,") != -1)
            //{
            //    mone++;
            //    string innerAddedCssClass = default(string);
            //    if (currentPage == "client-lists.aspx")
            //        innerAddedCssClass += "active";
            //    System.Web.HttpContext.Current.Response.Write("<li class='" + innerAddedCssClass + "'><a href='/client-lists.aspx'><i class='fa fa-circle-o'></i> CLIENT LIST</a></li>");
            //}

            //if (roles.IndexOf(",1.2,") != -1)
            //{
            //    mone++;
            //    string innerAddedCssClass = default(string);
            //    if (currentPage == "document-lists.aspx")
            //        innerAddedCssClass += "active";
            //    System.Web.HttpContext.Current.Response.Write("<li class='" + innerAddedCssClass + "'><a href='/document-lists.aspx'><i class='fa fa-circle-o'></i> DOCUMENT LIST</a></li>");
            //}

            //if (roles.IndexOf(",1.3,") != -1)
            //{
            //    mone++;
            //    string innerAddedCssClass = default(string);
            //    if (currentPage == "transaction-lists.aspx")
            //        innerAddedCssClass += "active";
            //    System.Web.HttpContext.Current.Response.Write("<li class='" + innerAddedCssClass + "'><a href='/transaction-lists.aspx'><i class='fa fa-circle-o'></i> TRANSACTION LIST</a></li>");
            //}

            //if (roles.IndexOf(",1.3,") != -1)
            //{
            //    mone++;
            //    string innerAddedCssClass = default(string);
            //    if (currentPage == "add-user.aspx")
            //        innerAddedCssClass += "active";
            //    System.Web.HttpContext.Current.Response.Write("<li class='" + innerAddedCssClass + "'><a href='/add-user.aspx'><i class='fa fa-circle-o'></i> ADD USER</a></li>");
            //}

            //if (roles.IndexOf(",1.3,") != -1)
            //{
            //    mone++;
            //    string innerAddedCssClass = default(string);
            //    if (currentPage == "AddGroup.aspx")
            //        innerAddedCssClass += "active";
            //    System.Web.HttpContext.Current.Response.Write("<li class='" + innerAddedCssClass + "'><a href='AddGroup.aspx'><i class='fa fa-circle-o'></i> ADD GROUP</a></li>");
            //}

            //if (roles.IndexOf(",1.3,") != -1)
            //{
            //    mone++;
            //    string innerAddedCssClass = default(string);
            //    if (currentPage == "AddRoles.aspx")
            //        innerAddedCssClass += "active";
            //    System.Web.HttpContext.Current.Response.Write("<li class='" + innerAddedCssClass + "'><a href='AddRoles.aspx'><i class='fa fa-circle-o'></i> ADD ROLES</a></li>");
            //}

            //if (mone != 0)
            //{
            //    System.Web.HttpContext.Current.Response.Write("</ul></li>");
            //}
            #endregion

            #region LOGOUT
            HttpContext.Current.Response.Write("<li class='treeview'><a href='/logout.aspx'><i class='fa fa-sign-out'></i> LOGOUT</a></li>");
            #endregion
            HttpContext.Current.Response.Write("</ul></section></aside>");
        }
    }
}