using CRM.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM.Utilities
{
    public class RoleUtilities
    {
        public string InsertRole(string roleName, string groupOption, string roleType, string roleLink)
        {
            string result = string.Empty;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = "INSERT INTO roles (id,name,rolegroup,type,rolelink) VALUES(default,@Name,@GroupOption,@Type,@RoleLink)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", roleName);
                cmd.Parameters.AddWithValue("@GroupOption", Convert.ToInt32(groupOption));
                cmd.Parameters.AddWithValue("@Type", roleType);
                cmd.Parameters.AddWithValue("@RoleLink", roleLink);
                result = Classes.DB.Insert(conn, query, cmd);
            }
            return result;
        }

        public string UpdateRole(string id, string roleName, string groupOption, string roleType, string roleLink)
        {
            string result = string.Empty;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = "UPDATE roles SET name = @Name, rolegroup = @GroupOption, type = @Type, rolelink = @RoleLink WHERE id=@Id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", roleName);
                cmd.Parameters.AddWithValue("@GroupOption", Convert.ToInt32(groupOption));
                cmd.Parameters.AddWithValue("@Type", roleType);
                cmd.Parameters.AddWithValue("@RoleLink", roleLink);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                result = Classes.DB.Update(conn, query, cmd);
            }
            return result;
        }

        public bool VerifyRoleExists(string roleName)
        {
            bool result;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = "SELECT EXISTS(SELECT 1 FROM roles WHERE name=@Name)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", roleName);
                var data = Classes.DB.SelectScalar(conn, query, cmd);
                result = data != null ? Convert.ToBoolean(data) : false;
            }
            return result;
        }

        public List<RolesModel> GetRoles()
        {
            List<RolesModel> rolesModels = new List<RolesModel>();
            string query = "SELECT r.id, r.name, r.type, r.rolelink, rg.id as groupid, rg.name as groupname FROM roles r LEFT JOIN rolegroups as rg ON r.rolegroup = rg.id ORDER BY r.id";
            DataTable dt = new DataTable();
            using (var conn = Classes.DB.InstBTCDB("instbtc"))
            {
                dt = Classes.DB.Select(conn, query);
                if (dt.Rows.Count > 0)
                {
                    rolesModels = dt.AsEnumerable().Select(x => new RolesModel
                    {
                        Id = x.Field<int>("id"),
                        Name = x.Field<string>("name"),
                        GroupId = x.Field<int>("groupid"),
                        GroupName = x.Field<string>("groupname"),
                        Type = x.Field<string>("type"),
                        RoleLink = x.Field<string>("rolelink")
                    }).ToList();
                }
            }
            return rolesModels;
        }

        public List<RolesModel> SearchRoles(string id, string rolename, string groupoptions, string roletype, string rolelink)
        {
            List<RolesModel> rolesModels = new List<RolesModel>();
            string addedQuery = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                addedQuery += "AND r.id = @Id ";
            }
            if (!string.IsNullOrEmpty(rolename))
            {
                addedQuery += "AND r.name = @RoleName ";
            }
            if (!string.IsNullOrEmpty(groupoptions))
            {
                addedQuery += "AND r.rolegroup = @GroupOptions ";
            }
            if (!string.IsNullOrEmpty(roletype))
            {
                addedQuery += "AND r.type = @RoleType ";
            }
            if (!string.IsNullOrEmpty(rolelink))
            {
                addedQuery += "AND r.rolelink = @Rolelink ";
            }
            string query = "SELECT r.id, r.name, r.type, r.rolelink, rg.id as groupid, rg.name as groupname FROM roles r LEFT JOIN rolegroups as rg ON r.rolegroup = rg.id WHERE 1=1 " +
                            $"{addedQuery} " +
                           "ORDER BY r.id";
            DataTable dt = new DataTable();

            using (var conn = Classes.DB.InstBTCDB("instbtc"))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                if (!string.IsNullOrEmpty(id))
                {
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                }
                cmd.Parameters.AddWithValue("@RoleName", rolename);
                if (!string.IsNullOrEmpty(groupoptions))
                {
                    cmd.Parameters.AddWithValue("@GroupOptions", Convert.ToInt32(groupoptions));
                }
                cmd.Parameters.AddWithValue("@RoleType", roletype);
                cmd.Parameters.AddWithValue("@Rolelink", rolelink);

                dt = Classes.DB.Select(conn, query, cmd);
                if (dt.Rows.Count > 0)
                {
                    rolesModels = dt.AsEnumerable().Select(x => new RolesModel
                    {
                        Id = x.Field<int>("id"),
                        Name = x.Field<string>("name"),
                        GroupId = x.Field<int>("groupid"),
                        GroupName = x.Field<string>("groupname"),
                        Type = x.Field<string>("type"),
                        RoleLink = x.Field<string>("rolelink")
                    }).ToList();
                }
            }
            return rolesModels;
        }

        public List<RolesModel> GetRoles(string id)
        {
            List<RolesModel> rolesModels = new List<RolesModel>();
            string query = "SELECT r.id, r.name, r.type, r.rolelink, rg.id as groupid, rg.name as groupname FROM roles r LEFT JOIN rolegroups as rg ON r.rolegroup = rg.id WHERE r.id =@Id ORDER BY r.id";
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                dt = Classes.DB.Select(conn, query, cmd);
                if (dt.Rows.Count > 0)
                {
                    rolesModels = dt.AsEnumerable().Select(x => new RolesModel
                    {
                        Id = x.Field<int>("id"),
                        Name = x.Field<string>("name"),
                        GroupId = x.Field<int>("groupid"),
                        GroupName = x.Field<string>("groupname"),
                        Type = x.Field<string>("type"),
                        RoleLink = x.Field<string>("rolelink")
                    }).ToList();
                }
            }
            return rolesModels;
        }

        public RolesModel GetRole(string id)
        {
            RolesModel rolesModel = new RolesModel();
            string query = "SELECT r.id, r.name, r.type, r.rolelink, rg.id as groupid, rg.name as groupname FROM roles r LEFT JOIN rolegroups as rg ON r.rolegroup = rg.id WHERE r.id =@Id ORDER BY r.id";
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                var reader = Classes.DB.ExecuteReader(conn, query, cmd);
                if (reader.Read())
                {
                    rolesModel.Id = !String.IsNullOrEmpty(reader["id"].ToString()) ? Convert.ToInt32(reader["id"]) : 0;
                    rolesModel.Name = reader["name"].ToString();
                    rolesModel.GroupId = !String.IsNullOrEmpty(reader["groupid"].ToString()) ? Convert.ToInt32(reader["groupid"]) : 0;
                    rolesModel.GroupName = reader["groupname"].ToString();
                    rolesModel.Type = reader["type"].ToString();
                    rolesModel.RoleLink = reader["rolelink"].ToString();
                }
            }
            return rolesModel;
        }


    }
}