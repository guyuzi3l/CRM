using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class TagEntityAttribute : System.Attribute
    {
        public String Name { get; set; }
        public String Tag { get; set; }
        public String Slot { get; set; }
        public Type EntityType { get; set; }

        public TagEntityAttribute(string name, string tag, string slot )
        {
            this.Name = name;
            this.Tag = tag;
            this.Slot = slot;
        }
    }
}