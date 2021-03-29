using CRM.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM.Utilities
{
    public class Clients
    {
        public static List<ClientModel> GetClients(NpgsqlConnection conn, string orderby = "")
        {
            if (!string.IsNullOrEmpty(orderby))
                orderby = $"ORDER BY {orderby}";

            List<ClientModel> clients = new List<ClientModel>();
            string query = $"SELECT id,email,password,referral,country,phone_prefix,phone_number,utm_source,created_date,first_name,last_name,phone_verification,max_deposit, first_name::text || ' ' || last_name::text AS fullname, actual_client_ip FROM clients {orderby}";
            using (DataTable dt = Classes.DB.Select(conn, query))
            {
                if (dt.Rows.Count > 0)
                {
                    clients = dt.AsEnumerable().Select(x => new ClientModel()
                    {
                        Id = x.Field<int>("id"),
                        Email = x.Field<string>("email"),
                        Password = x.Field<string>("password"),
                        Country = x.Field<string>("country"),
                        Phone_prefix = x.Field<string>("phone_prefix"),
                        Phone_number = x.Field<string>("phone_number"),
                        Utm_source = x.Field<string>("utm_source"),
                        Created_date = x.Field<DateTime>("created_date"),
                        First_name = x.Field<string>("first_name"),
                        Last_name = x.Field<string>("last_name"),
                        Phone_verification = x.Field<string>("phone_verification"),
                        Max_deposit = x.Field<double>("max_deposit").ToString(),
                        client_ip = x.Field<string>("actual_client_ip"),
                        Referral = !string.IsNullOrEmpty(x.Field<string>("referral")) ? x.Field<string>("referral") : ""
                    }).ToList();
                }
            }
            return clients;
        }
        public static List<ClientModel> GetClients(NpgsqlConnection conn, string queryFilter, string orderby = "")
        {

            if (!string.IsNullOrEmpty(orderby))
                orderby = $"ORDER BY {orderby}";

            List<ClientModel> clients = new List<ClientModel>();
            string query = $"SELECT id,email,password,referral,country,phone_prefix,phone_number,utm_source,created_date,first_name,last_name,phone_verification,max_deposit, first_name::text || ' ' || last_name::text AS fullname, actual_client_ip FROM clients WHERE 1=1 {queryFilter} {orderby}";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            using (DataTable dt = Classes.DB.Select(conn, query, cmd))
            {
                if (dt.Rows.Count > 0)
                {
                    clients = dt.AsEnumerable().Select(x => new ClientModel()
                    {
                        Id = x.Field<int>("id"),
                        Email = x.Field<string>("email"),
                        Password = x.Field<string>("password"),
                        Country = x.Field<string>("country"),
                        Phone_prefix = x.Field<string>("phone_prefix"),
                        Phone_number = x.Field<string>("phone_number"),
                        Utm_source = x.Field<string>("utm_source"),
                        Created_date = x.Field<DateTime>("created_date"),
                        First_name = x.Field<string>("first_name"),
                        Last_name = x.Field<string>("last_name"),
                        Phone_verification = x.Field<string>("phone_verification"),
                        Max_deposit = x.Field<double>("max_deposit").ToString(),
                        client_ip = !string.IsNullOrEmpty(x.Field<string>("actual_client_ip"))? x.Field<string>("actual_client_ip") : "",
                        Referral = !string.IsNullOrEmpty(x.Field<string>("referral")) ? x.Field<string>("referral") : ""
                    }).ToList();
                }
            }
            return clients;
        }
        public static List<ClientModel> GetClients(NpgsqlConnection conn, NpgsqlCommand command, string addedquery = "", int offset = 0, int limit = 0, string orderby = "")
        {

            if (!string.IsNullOrEmpty(orderby))
                orderby = $"ORDER BY {orderby}";
            List<ClientModel> clients = new List<ClientModel>();
            string query = $"SELECT id,email,password,referral,country,phone_prefix,phone_number,utm_source,created_date,first_name,last_name,phone_verification,max_deposit, first_name::text || ' ' || last_name::text AS fullname, actual_client_ip FROM clients WHERE 1=1 {addedquery} {orderby}";

            //Get Given Parameters Collection
            NpgsqlParameterCollection parameters = command.Parameters;

            //Pass Parameter Collection into Current parameter
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            foreach (NpgsqlParameter parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.NpgsqlValue);
            }

            using (DataTable dt = Classes.DB.Select(conn, query, cmd))
            {
                if (dt.Rows.Count > 0)
                {
                    clients = dt.AsEnumerable().Select(x => new ClientModel()
                    {
                        Id = x.Field<int>("id"),
                        Email = x.Field<string>("email"),
                        Password = x.Field<string>("password"),
                        Country = x.Field<string>("country"),
                        Phone_prefix = x.Field<string>("phone_prefix"),
                        Phone_number = x.Field<string>("phone_number"),
                        Utm_source = x.Field<string>("utm_source"),
                        Created_date = x.Field<DateTime>("created_date"),
                        First_name = x.Field<string>("first_name"),
                        Last_name = x.Field<string>("last_name"),
                        Phone_verification = x.Field<string>("phone_verification"),
                        Max_deposit = x.Field<double>("max_deposit").ToString(),
                        client_ip = !string.IsNullOrEmpty(x.Field<string>("actual_client_ip")) ? x.Field<string>("actual_client_ip").ToString() : "",
                        Referral = !string.IsNullOrEmpty(x.Field<string>("referral")) ? x.Field<string>("referral") : ""
                    }).ToList();
                }
            }
            return clients;
        }

        public static string ClientsCount(NpgsqlConnection conn)
        {
            string query = "SELECT COUNT (*) FROM clients";
            var getClients = Classes.DB.SelectScalar(conn, query);
            string TotalClients = getClients != null ? getClients.ToString() : "0";
            return TotalClients;
        }

        public static ClientModel FindById(NpgsqlConnection conn, int id)
        {
            string query = "SELECT * FROM clients WHERE id = @Id";
            ClientModel clients = new ClientModel();
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            using (DataTable dt = Classes.DB.Select(conn, query, cmd))
            {
                if (dt.Rows.Count <= 0)
                    return null;

                var x = dt.Rows[0];

                clients.Id = x.Field<int>("id");
                clients.Email = x.Field<string>("email");
                clients.Password = x.Field<string>("password");
                clients.Country = x.Field<string>("country");
                clients.Phone_prefix = x.Field<string>("phone_prefix");
                clients.Phone_number = x.Field<string>("phone_number");
                clients.Utm_source = x.Field<string>("utm_source");
                clients.Created_date = x.Field<DateTime>("created_date");
                clients.First_name = x.Field<string>("first_name");
                clients.Last_name = x.Field<string>("last_name");
                clients.Phone_verification = x.Field<string>("phone_verification");
                clients.Max_deposit = x.Field<double>("max_deposit").ToString();
                clients.Referral = !string.IsNullOrEmpty(x.Field<string>("referral")) ? x.Field<string>("referral") : "";
                clients.dob = !string.IsNullOrEmpty(x.Field<DateTime?>("dob")?.ToString() ?? "") ? x.Field<DateTime?>("dob")?.ToString("dd-MM-yyyy") : "01/01/01";
                clients.actual_client_ip = !string.IsNullOrEmpty(x.Field<string>("actual_client_ip")) ? x.Field<string>("actual_client_ip") : "";
            }

            return clients;
        }

        public bool UpdateClients(NpgsqlConnection conn, ClientModel clients)
        {
            if (clients != null)
            {
                string query = String.Format($"UPDATE clients SET  email = @Email, country = @Country, phone_number = @PhoneNumber, first_name = @FirstName, last_name = @LastName,phone_prefix = @PhonePrefix,max_deposit=@maxDeposit, referral = @reff WHERE  id = @Id");
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", clients.Email);
                cmd.Parameters.AddWithValue("@Country", clients.Country);
                cmd.Parameters.AddWithValue("@PhoneNumber", clients.Phone_number);
                cmd.Parameters.AddWithValue("@FirstName", clients.First_name);
                cmd.Parameters.AddWithValue("@LastName", clients.Last_name);
                cmd.Parameters.AddWithValue("@PhonePrefix", clients.Phone_prefix);
                cmd.Parameters.AddWithValue("@reff", clients.Referral);
                cmd.Parameters.AddWithValue("@Id", clients.Id);

                double mv = default(double);

                if (double.TryParse(clients.Max_deposit, out mv))
                    cmd.Parameters.AddWithValue("@maxDeposit", mv);
                else
                    return false; // cant think of any better action here. so just cancelling the update if the given max deposit is not convertible to type double.
                
                string res = Classes.DB.Update(conn, query, cmd);
                if (string.Equals(res, "success", StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public static ClientModel GetClientsByDocId(NpgsqlConnection conn, string DocId)
        {
            string query = $"SELECT clients.* FROM clients INNER JOIN documents ON clients.id = documents.client_id WHERE documents.id=@DocId";
            ClientModel clients = new ClientModel();
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DocId", Convert.ToInt32(DocId));
            using (DataTable dt = Classes.DB.Select(conn, query, cmd))
            {
                if (dt.Rows.Count <= 0)
                    return null;

                var x = dt.Rows[0];

                clients.Id = x.Field<int>("id");
                clients.Email = x.Field<string>("email");
                clients.Password = x.Field<string>("password");
                clients.Country = x.Field<string>("country");
                clients.Phone_prefix = x.Field<string>("phone_prefix");
                clients.Phone_number = x.Field<string>("phone_number");
                clients.Utm_source = x.Field<string>("utm_source");
                clients.Created_date = x.Field<DateTime>("created_date");
                clients.First_name = x.Field<string>("first_name");
                clients.Last_name = x.Field<string>("last_name");
                clients.Phone_verification = x.Field<string>("phone_verification");
                clients.Max_deposit = x.Field<double> ("max_deposit").ToString();
            }

            return clients;
        }

        public static List<ClientModel> GetClientCase(NpgsqlConnection conn)
        {
            List<ClientModel> clients = new List<ClientModel>();
            string query = "SELECT c.* FROM clients c INNER JOIN transactions t ON c.id = t.client_id WHERE c.created_date < NOW() - INTERVAL '7 days'";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            using (DataTable dt = Classes.DB.Select(conn, query, cmd))
            {
                if (dt.Rows.Count > 0)
                {
                    clients = dt.AsEnumerable().Select(x => new ClientModel()
                    {
                        Id = x.Field<int>("id"),
                        Email = x.Field<string>("email"),
                        Password = x.Field<string>("password"),
                        Country = x.Field<string>("country"),
                        Phone_prefix = x.Field<string>("phone_prefix"),
                        Phone_number = x.Field<string>("phone_number"),
                        Utm_source = x.Field<string>("utm_source"),
                        Created_date = x.Field<DateTime>("created_date"),
                        First_name = x.Field<string>("first_name"),
                        Last_name = x.Field<string>("last_name"),
                        Phone_verification = x.Field<string>("phone_verification"),
                        Max_deposit = x.Field<double>("max_deposit").ToString()
                    }).ToList();
                }
            }
            return clients;
        }

        public static string ClientCaseCount(NpgsqlConnection conn)
        {
            string query = "SELECT COUNT (*) FROM clients WHERE created_date < NOW() - INTERVAL '7 days'";
            var getClients = Classes.DB.SelectScalar(conn, query);
            string TotalClients = getClients != null ? getClients.ToString() : "0";
            return TotalClients;
        }

        public static string GetUserBtcBalance(NpgsqlConnection conn, string ClientId)
        {
            string query = "SELECT SUM(exchange_amount) as btc_balance FROM public.transactions t ";
            query += "INNER JOIN public.clients c ON t.client_id = c.id ";
            query += "WHERE t.transaction_currency = 'BTC' AND t.psp_status = 'Approved' ";
            query += "AND t.client_id = @ClientId";

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClientId", Convert.ToInt32(ClientId));
            var data = Classes.DB.SelectScalar(conn, query, cmd)?.ToString();

            if (string.IsNullOrEmpty(data))
                data = "0.00000";

            if (decimal.Parse(data.ToString()) < 0)
            {
                data = "0.00000";
            }

            return data.ToString();
        }

        public static string GetUserEurBalance(NpgsqlConnection conn, string ClientId)
        {
            string query = "SELECT SUM(exchange_amount) as eur_balance FROM public.transactions t ";
            query += "INNER JOIN public.clients c ON t.client_id = c.id ";
            query += "WHERE t.transaction_currency = 'EUR' AND t.psp_status = 'Approved' ";
            query += "AND t.client_id = @ClientId";

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClientId", Convert.ToInt32(ClientId));
            var data = Classes.DB.SelectScalar(conn, query, cmd)?.ToString();

            if (string.IsNullOrEmpty(data))
                data = "0.00000";

            if (decimal.Parse(data.ToString()) < 0)
            {
                data = "0.00000";
            }

            return data.ToString();
        }

    }
}