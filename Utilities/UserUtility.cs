using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;
using System.Data;

namespace CRM.Utilities
{
    public class UserUtility
    {
        public string InsertUser(UserModel user)
        {
            string result = string.Empty;
            using (var conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = "INSERT INTO users (id,username,password,email,roles) VALUES(default,@Username,@Password,@Email,@Roles)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Roles", user.Roles);
                result = Classes.DB.Insert(conn, query, cmd);
            }
            return result;
        }

        public string UpdateUser(string id, string username, string password, string email, string roles)
        {
            string result = string.Empty;
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(password))
                {
                    query = "UPDATE users SET username = @Username, email = @Email, roles = @Roles WHERE id=@Id";
                }
                else
                {
                    query = "UPDATE users SET username = @Username, password = @Password, email = @Email, roles = @Roles WHERE id=@Id";
                }
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Roles", roles);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                result = Classes.DB.Update(conn, query, cmd);
            }
            return result;
        }

        public List<UserModel> GetUsers()
        {
            List<UserModel> userModels = new List<UserModel>();
            string query = "SELECT * FROM users ORDER BY id";
            DataTable dt = new DataTable();
            using (var conn = Classes.DB.InstBTCDB("instbtc"))
            {
                dt = Classes.DB.Select(conn, query);
                if (dt.Rows.Count > 0)
                {
                    userModels = dt.AsEnumerable().Select(x => new UserModel
                    {
                        Id = x.Field<int>("id"),
                        Username = x.Field<string>("username"),
                        Password = x.Field<string>("password"),
                        Email = x.Field<string>("email"),
                        Roles = x.Field<string>("roles"),
                    }).ToList();
                }
            }
            return userModels;
        }

        public UserModel GetUser(string id)
        {
            UserModel userModel = new UserModel();
            string query = "SELECT * FROM users WHERE id=@Id";
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                var reader = Classes.DB.ExecuteReader(conn, query, cmd);
                if (reader.Read())
                {
                    userModel.Id = !String.IsNullOrEmpty(reader["id"].ToString()) ? Convert.ToInt32(reader["id"]) : 0;
                    userModel.Username = reader["username"].ToString();
                    userModel.Password = reader["password"].ToString();
                    userModel.Email = reader["email"].ToString();
                    userModel.Roles = reader["roles"].ToString();
                }
            }
            return userModel;
        }

        public List<UserModel> SearchUsers(string id, string username, string email)
        {
            List<UserModel> userModel = new List<UserModel>();
            string addedQuery = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                addedQuery += "AND id = @Id ";
            }
            if (!string.IsNullOrEmpty(username))
            {
                addedQuery += "AND username = @Username ";
            }
            if (!string.IsNullOrEmpty(email))
            {
                addedQuery += "AND email = @Email ";
            }
            string query = "SELECT * FROM users WHERE 1=1 " +
                            $"{addedQuery} " +
                           "ORDER BY id";
            DataTable dt = new DataTable();

            using (var conn = Classes.DB.InstBTCDB("instbtc"))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                if (!string.IsNullOrEmpty(id))
                {
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                }
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);

                dt = Classes.DB.Select(conn, query, cmd);
                if (dt.Rows.Count > 0)
                {
                    userModel = dt.AsEnumerable().Select(x => new UserModel
                    {
                        Id = x.Field<int>("id"),
                        Username = x.Field<string>("username"),
                        Password = x.Field<string>("password"),
                        Email = x.Field<string>("email"),
                        Roles = x.Field<string>("roles")
                    }).ToList();
                }
            }
            return userModel;
        }
    }
}