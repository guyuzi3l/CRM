using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Attribute;

namespace CRM.Attribute
{
    public class TagGroup
    {
        public TagEntityAttribute Entity { get; set; }
        public Object Context { get; set; }
        public List<Tag> Tags { get; set; }

        public TagGroup()
        {
            this.Tags = new List<Tag>();
        }
    }
}