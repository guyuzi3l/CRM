using CRM.Attribute;
using CRM.Utilities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public static List<Tag> All()
        {
            var tags = new List<Tag>();
            var tagUtil = new TagUtility(typeof(TagEntityAttribute), typeof(TagFieldAttribute));
            var rawTags = tagUtil.GetTags();

            for (int index = 0; index < rawTags.Count; index++)
            {
                for (int _index = 0; _index < rawTags[index].Fields.Count; _index++)
                {
                    tags.Add(new Tag() {
                        Id = index + _index,
                        Name = $"{rawTags[index].EntityName} - {rawTags[index].Fields[_index]["name"]}",
                        Value = rawTags[index].Fields[_index]["hashtag"]
                    });
                }
            }
            
            return tags;
        }
    }
}