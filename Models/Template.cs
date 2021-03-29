using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public string Body { get; set; }

        public enum TemplateType {Email , Sms};

        public bool Create()
        {
            var res = false;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = $"INSERT INTO templates (name, description, body, type) VALUES (@name, @desc, @body, @type)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", this.Name);
                cmd.Parameters.AddWithValue("@desc", this.Description);
                cmd.Parameters.AddWithValue("@body", this.Body);
                cmd.Parameters.AddWithValue("@type", this.Type);
                var result = Classes.DB.Update(conn, query, cmd);
                res = result == "Success";
            }

            return res;
        }

        public bool Update()
        {
            var res = false;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = $"UPDATE templates SET name = @name, description = @desc, body = @body, type = @type WHERE id=@id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", this.Id);
                cmd.Parameters.AddWithValue("@name", this.Name);
                cmd.Parameters.AddWithValue("@desc", this.Description);
                cmd.Parameters.AddWithValue("@body", this.Body);
                cmd.Parameters.AddWithValue("@type", this.Type);
                var result = Classes.DB.Update(conn, query, cmd);
                res = result == "Success";
            }
            return res;
        }

        public static List<Template> All()
        {
            var templates = new List<Template>();
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = $"SELECT * FROM templates";
                using (DataTable dt = Classes.DB.Select(conn, query))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        templates = dt.AsEnumerable().Select(x => new Template()
                        {
                            Id = x.Field<int>("id"),
                            Name = x.Field<string>("name"),
                            Description = x.Field<string>("description"),
                            Body = x.Field<string>("body")
                        }).ToList<Template>();
                    }
                }
            }

            return templates;
        }
    }
}