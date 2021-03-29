using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CRM.Models;

namespace CRM.Utilities
{
    public class GroupUtilities
    {
        public string InsertGroup(string groupName)
        {
            string result = string.Empty;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = "INSERT INTO rolegroups (id,name) VALUES(default,@Name)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", groupName);
                result = Classes.DB.Insert(conn, query, cmd);
            }
            return result;
        }

        public bool VerifyGroupExists(string groupName)
        {
            bool result;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = "SELECT EXISTS(SELECT 1 FROM rolegroups WHERE name=@Name)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", groupName);
                var data = Classes.DB.SelectScalar(conn, query, cmd);
                result = data != null ? Convert.ToBoolean(data) : false;
            }
            return result;
        }

        public List<KeyValuePair<int, string>> GetGroupOptions()
        {
            string query = "SELECT id, name FROM rolegroups ORDER BY id";
            DataTable dt = new DataTable();
            using (var conn = Classes.DB.InstBTCDB("instbtc"))
            {
                dt = Classes.DB.Select(conn, query);
            };

            List<KeyValuePair<int, string>> roles = new List<KeyValuePair<int, string>>();
            if (dt.Rows.Count > 0)
            {
                roles = dt.AsEnumerable().Select(x => new KeyValuePair<int, string>(x.Field<int>("id"), x.Field<string>("name"))).ToList();
            }

            return roles;
        }

        public List<GroupsModel> GetGroups()
        {
            List<GroupsModel> groupsModels = new List<GroupsModel>();
            string query = "SELECT * FROM rolegroups ORDER BY id";
            DataTable dt = new DataTable();
            using (var conn = Classes.DB.InstBTCDB("instbtc"))
            {
                dt = Classes.DB.Select(conn, query);
                groupsModels = dt.AsEnumerable().Select(x => new GroupsModel
                {
                    Id = x.Field<int>("id"),
                    Name = x.Field<string>("name")
                }).ToList();
            }
            return groupsModels;
        }
    }
}