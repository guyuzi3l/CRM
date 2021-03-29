using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM
{
    public partial class xhr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e){}

        [WebMethod]
        public static List<Models.Tag> GetTags()
        {
            return Models.Tag.All();
        }

        [WebMethod]
        public static List<Models.Template> GetTemplates()
        {
            return Models.Template.All();
        }

        [WebMethod]
        public static List<KeyValuePair<string, int>> GetTemplateTypes()
        {
            List<KeyValuePair<string, int>> ttypes = new List<KeyValuePair<string, int>>();

            foreach (var type in Enum.GetValues(typeof(Models.Template.TemplateType)))
            {
                ttypes.Add(new KeyValuePair<string, int>(Enum.GetName(typeof(Models.Template.TemplateType), type), (int)type));
            }

            return ttypes;
        }

        [WebMethod]
        public static Dictionary<string,Object> CreateTemplate(String name, string description , int type, string body )
        {
            var dick = new Dictionary<string, Object>()
            {
                { "success", true },
                { "message", "" },
            };

            Models.Template t = new Models.Template()
            {
                Name = name,
                Description = description,
                Type = type,
                Body = body
            };

            var res = t.Create();

            if (res == true)
                dick["success"] = true;
            else
            {
                dick["success"] = false;
                dick["message"] = res;
            }
            return dick;
        }

        [WebMethod]
        public static Dictionary<string, Object> UpdateTemplate(int id, String name, string description, int type, string body)
        {
            var dick = new Dictionary<string, Object>()
            {
                { "success", true },
                { "message", "" },
            };
            
            Models.Template t = new Models.Template()
            {
                Id = id,
                Name = name,
                Description = description,
                Type = type,
                Body = body
            };

            var res = t.Update();

            if (res == true)
                dick["success"] = true;
            else
            {
                dick["success"] = false;
                dick["message"] = res;
            }
            return dick;
        }
    }
}