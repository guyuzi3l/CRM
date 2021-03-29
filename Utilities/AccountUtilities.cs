using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CRM.Utilities
{
    public class AccountUtilities
    {
        public static void checkUserLogin()
        {
            if (Classes.Cookie.GetCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", false) == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("/login.aspx");
            }
        }

        public static string getUserRole(string username)
        {
            string role = string.Empty;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = "SELECT roles FROM users WHERE username=@Username";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                var checkRole = Classes.DB.SelectScalar(conn, query, cmd);
                string verifyRole = checkRole != null ? checkRole.ToString() : "";
                role = "," + verifyRole + ",";
            };
            return role;
        }

        public static string getUserRoles(string username)
        {
            string role = string.Empty;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = "SELECT roles FROM users WHERE username=@Username";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                var checkRole = Classes.DB.SelectScalar(conn, query, cmd);
                string verifyRole = checkRole != null ? checkRole.ToString() : "";
                role =  verifyRole;
            };
            return role;
        }

        public static string getUserId()
        {
            string id = string.Empty;
            string username = Classes.Cookie.GetCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", false);
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = "SELECT id FROM users WHERE username=@Username";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                var checkId = Classes.DB.SelectScalar(conn, query, cmd);
                string verifyId = checkId != null ? checkId.ToString() : "";
                id = "," + verifyId + ",";
            };
            return id;
        }

        public static string LoginAccount(NpgsqlConnection conn, string username, string password)
        {
            string result = string.Empty;
            string query = "SELECT username FROM users WHERE username=@Username AND password=@Password";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);
            var data = Classes.DB.SelectScalar(conn, query, cmd);
            if (data != null)
            {
                Classes.Cookie.CreateCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", data.ToString());
                HttpContext.Current.Response.Redirect("/dashboard.aspx");
            }
            else
            {
                result = "Invalid Username or Password";
            }
            return result;
        }

        public static string ExtracInitials(string name)
        {
            //Remove all punctuation, separator chars, control chars, and numbers (unicode style regexes)
            string initials = Regex.Replace(name, @"[\p{P}\p{S}\p{C}\p{N}]+", "");

            //Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
            initials = Regex.Replace(initials, @"\p{Z}+", " ");

            //Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
            initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);

            //Extract up to 2 initials from the remaining cleaned name.
            initials = Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))?(?:(\p{L})\p{L}*)?)?$", "$1$2").Trim();

            if (initials.Length > 2)
            {
                //When everything failed, just grab the first two letters of what we have left.
                initials = initials.Substring(0, 2);
            }

            return initials.ToUpperInvariant();
        }
    }
}