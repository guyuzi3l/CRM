using System;
using System.Collections.Generic;
using System.Reflection;
using CRM.Attribute;
using System.Linq;
using System.Web;

namespace CRM.Utilities
{
    public class TagUtility
    {
        private List<CRM.Attribute.Tag> Tags;
        private List<CRM.Attribute.TagGroup> TagGroups;
        private string TagEntityAttributeClassName;
        private string TagFieldAttributeClassName;

        public TagUtility(Type entity, Type tagField)
        {
            this.TagEntityAttributeClassName = entity.Name;
            this.TagFieldAttributeClassName = tagField.Name;
            this.init();
        }
        
        private void init()
        {
            this.Tags = new List<CRM.Attribute.Tag>();
            this.TagGroups = new List<TagGroup>();

            Assembly myAssembly = Assembly.GetExecutingAssembly();

            foreach (Type type in myAssembly.GetTypes())
            {
                TypeInfo ti = type.GetTypeInfo();

                foreach (var attr in ti.GetCustomAttributes())
                {
                    if (attr.GetType().Name == this.TagEntityAttributeClassName)
                    {
                        if (attr is TagEntityAttribute tagEntity)
                        {
                            tagEntity.EntityType = type;
                            var currTag = new Attribute.Tag() { EntityName = tagEntity.Name, EntityValue = tagEntity.Tag };
                            PropertyInfo[] props = type.GetProperties();

                            foreach (var p in props)
                            {
                                foreach (var propAttr in p.GetCustomAttributes())
                                {
                                    if (propAttr.GetType().Name == this.TagFieldAttributeClassName)
                                    {
                                        if (propAttr is TagFieldAttribute tagField)
                                        {
                                            currTag.Fields.Add(new Dictionary<string, string>() {
                                                {"name", tagField.Name},
                                                {"tag", tagField.Tag},
                                                {"hashtag", $"[#{{{tagEntity.Tag}}}{{{tagField.Tag}}}#]"},
                                                {"propName",p.Name}
                                            });
                                        }

                                    }
                                }
                            }

                            var tg = this.TagGroups.Where((_tg) =>
                            {
                                return _tg.Entity == tagEntity;
                            }).FirstOrDefault();

                            if (tg != null)
                            {
                                tg.Tags.Add(currTag);
                            }
                            else
                            {
                                tg = new TagGroup(){ Entity = tagEntity };
                                tg.Tags.Add(currTag);
                                this.TagGroups.Add(tg);

                            }

                            this.Tags.Add(currTag);
                        }
                    }
                }
            }
        }

        public void SetSlot(string slot , Object context)
        {
            var tg = this.TagGroups.Where<TagGroup>((_tg) => {
                return _tg.Entity.Slot == slot;
            }).FirstOrDefault();

            if (tg == null)
                throw new ApplicationException($"There is no tag group with a slot {slot} defined.");

            if (!tg.Entity.EntityType.IsAssignableFrom(context.GetType()))
                throw new ApplicationException($"Injected object is not assignable to {tg.Entity.EntityType.Name}.");

            tg.Context = context;
        }

        public List<CRM.Attribute.Tag> GetTags()
        {
            var tagList = new List<Tag>();
            this.TagGroups.ForEach((tg) =>
            {
                tg.Tags.ForEach((t) =>
                {
                    tagList.Add(t);
                });
            });
            
            return tagList;
        }

        public string Parse(string text)
        {
            this.TagGroups.ForEach((tg) => {
                tg.Tags.ForEach((t) => {

                    t.Fields.ForEach((f) =>
                    {
                        if (tg.Context != null)
                        {
                            var rep = tg.Context.GetType().GetProperty(f["propName"]).GetValue(tg.Context, null);
                            if (rep != null)
                                text = text.Replace(f["hashtag"], rep.ToString());
                        }
                    });
                });
            });
            return text;
        }
    }
}