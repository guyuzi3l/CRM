using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace CRM.Utilities
{
    public class Templates
    {
        public static void getHeader()
        {
            StringBuilder header = new StringBuilder();
            string username = Classes.Cookie.GetCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", false);
            if (username == null)
                HttpContext.Current.Response.Redirect("/login.aspx");

            header.Append("<header class='main-header'>")
                    .Append("<a href='/dashboard.aspx' class='logo'> <span class='logo-mini'> InstBTC</span>")
                    .Append("<span class='logo-lg'> <img src='/img/logo-1.png' style='width: 200px; height:45px;'></span> </a>")

                .Append("<nav class='navbar navbar-static-top' role='navigation'>")
                .Append("<a href='#' class='sidebar-toggle' data-toggle='offcanvas' role='button'> <span class='sr-only'>Toggle navigation</span> </a>")
                    .Append("<div class='navbar-custom-menu'>")
                        .Append("<ul class='nav navbar-nav'>")
                            .Append("<li class='dropdown user user-menu'>")
                            .Append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'> <img src='/img/thumbnail.jpg' class='user-image'> <span class='hidden-xs'> Welcome: " + username + "</span> </a>")
                                .Append("<ul class='dropdown-menu'>")
                                    .Append("<li class='user-header'> <img src='/img/thumbnail.jpg' class='img-circle'")
                                    .Append("<p>" + username + " <small></small> </p> </li>")
                                    .Append("<li class='user-footer'>")
                                    .Append("<div class='pull-left'> <a href='#' class='btn btn-default btn-flat'><i class='fa fa-user'></i> Profile</a> </div>")
                                    .Append("<div class='pull-right'> <a href='/logout.aspx' class='btn btn-default btn-flat'><i class='fa fa-sign-out'></i> Sign out</a> </div>")
                                    .Append("<li>")
                                .Append("</ul>")
                            .Append("</li>")
                    .Append("<li> <a href='#' data-toggle='control-sidebar'><i class='fa fa-gears'></i></a> </li>")
                    .Append("</ul>")
                    .Append("</div>")
                    .Append("</nav>")
                    .Append("<link rel='stylesheet' href='/css/toastr/toastr.min.css' />")
                .Append("</header>")

                .Append("<aside class='control-sidebar control-sidebar-dark'> <!-- Create the tabs --> <ul class='nav nav-tabs nav-justified control-sidebar-tabs'> </ul> <!-- Tab panes --> <div class='tab-content'> <!-- Home tab content --> <div class='tab-pane' id='control-sidebar-home-tab'> <!-- /.control-sidebar-menu --> </div> </div> </aside>");
            //.Append("<aside class='control-sidebar control-sidebar-dark'> <!-- Create the tabs --> <ul class='nav nav-tabs nav-justified control-sidebar-tabs'> <li><a href='#control-sidebar-home-tab' data-toggle='tab'><i class='fa fa-home'></i>Choose Template</a></li> </ul> <!-- Tab panes --> <div class='tab-content'> <!-- Home tab content --> <div class='tab-pane active' id='control-sidebar-home-tab'> <ul class='control-sidebar-menu'> <li> <a href='javascript::;'> <i class='menu-icon fa fa-file-code-o bg-red'></i> <div class='menu-info'> <h4 class='control-sidebar-subheading'>Red Theme</h4> <p>Lorem ipsum dolor ipsum</p> </div> </a> </li> <li> <a href='javascript::;'> <i class='menu-icon fa fa-file-code-o bg-yellow'></i> <div class='menu-info'> <h4 class='control-sidebar-subheading'>Yellow Theme</h4> <p>Lorem ipsum dolor ipsum</p> </div> </a> </li> <li> <a href='javascript::;'> <i class='menu-icon fa fa-file-code-o bg-light-blue'></i> <div class='menu-info'> <h4 class='control-sidebar-subheading'>Light Blue Theme</h4> <p>Lorem ipsum dolor ipsum</p> </div> </a> </li> <li> <a href='javascript::;'> <i class='menu-icon fa fa-file-code-o bg-green'></i> <div class='menu-info'> <h4 class='control-sidebar-subheading'>Green Theme</h4> <p>Lorem ipsum dolor ipsum</p> </div> </a> </li> </ul> <!-- /.control-sidebar-menu --> </div> <!-- /.tab-pane --> </div> </aside>");
            HttpContext.Current.Response.Write(header);
        }

        public static StringBuilder footer;
        public static StringBuilder getFooter()
        {
            var userId = Utilities.AccountUtilities.getUserId();

            footer = new StringBuilder();
            footer.Append(" <footer class='main-footer'>")
                  .Append("<p class='text-right'><strong>Copyright &copy; 2018 <a href='/dashboard.aspx'>InstBTC</a>.</strong> All rights reserved.</p>")
                  .Append("</footer>")
                //Start of Loader on Page
                  .Append("<div id='loader'> <div class='spinner'> <div class='dot1'></div> <div class='dot2'></div> </div> </div>")

                  //Start of Loader when uploading
                  .Append("<div id='loader-container'> <div class='spinner'> <div class='dot1'></div> <div class='dot2'></div> </div> </div>")
                  .Append("<script type='text/javascript'> $('form').submit(function () { $('#loader-container').css('display', 'block'); return true; }); </script>")

                  .Append("<script src='/script/script-loader.js'></script>")
                  .Append("<script src='/script/toastr/toastr.min.js'></script>");

            return footer;
        }
    }
}