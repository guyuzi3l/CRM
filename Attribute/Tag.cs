using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Attribute
{
    public class Tag
    {
        public string EntityName { get; set; }
        public string EntityValue { get; set; }
        public List<Dictionary<string,string>> Fields { get; set; }

        public Tag()
        {
            this.Fields = new List<Dictionary<string, string>>();
        }
    }
}