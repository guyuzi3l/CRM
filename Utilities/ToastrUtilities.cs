using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace CRM.Utilities
{
    public class ToastrUtilities
    {
        public KeyValuePair<string, string>? GetToast()
        {
            if (this.SessionHasKey("toast"))
            {
                var kvp = (KeyValuePair<string, string>)HttpContext.Current.Session["toast"];
                HttpContext.Current.Session["toast"] = null;
                return kvp;
            }
            return null;
        }

        public bool SessionHasKey(string key)
        {
            return HttpContext.Current.Session[key] != null;
        }

        public Object SessionPop(string key)
        {
            var ret = HttpContext.Current.Session[key];
            HttpContext.Current.Session[key] = null;
            return ret;
        }

        public void SessionPush(string key, Object val)
        {
            HttpContext.Current.Session[key] = val;
        }
    }
}