using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Attribute
{
    public class TagFieldAttribute : System.Attribute
    {
        public string Name {get;set;}
        public string Tag { get; set; }

        public TagFieldAttribute(string name, string tag)
        {
            this.Name = name;
            this.Tag = tag;
        }
    }
}