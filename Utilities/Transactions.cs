using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Data;

namespace CRM.Utilities
{
    public class Transactions
    {
        public int Id { get; set; }
        public decimal Client_id { get; set; }
        public decimal Psp_id { get; set; }
        public string Deposit_currency { get; set; }
        public decimal Deposit_amount { get; set; }
        public string Exchange_currency { get; set; }
        public decimal Exchange_amount { get; set; }
        public string Psp_status { get; set; }
        public string Credited_status { get; set; }
        public DateTime Created_date { get; set; }
        public StringBuilder Attached_doc { get; set; }
        public string Fullname { get; set; }
        public string PspName { get; set; }
        public string PaymentReference { get; set; }
        public string CardLast4 { get; set; }
        public string CardholderName { get; set; }
        public string Notes { get; set; }
        public string Referal { get; set; }
        public Classes.Instbtc.Models.TransactionType Type { get; set; }

        public static List<Transactions> GetTransactions(NpgsqlConnection conn, string queryFilter)
        {
            List<Transactions> transactions = new List<Transactions>();

            string query = "SELECT tr.id, cl.first_name::text || ' ' || cl.last_name AS fullname,cl.referral, tr.deposit_currency, tr.deposit_amount, tr.exchange_currency, tr.exchange_amount, tr.psp_status, tr.credited_status, tr.card_last4, tr.cardholder_name, tr.notes, tr.type, tr.created_date, tr.client_id, ps.name, tr.payment_reference  FROM clients cl INNER JOIN transactions tr on cl.id = tr.client_id INNER JOIN psp ps ON tr.psp_id = ps.id WHERE cl.id IS NOT NULL AND type = 0 " + queryFilter + " ORDER BY tr.created_date DESC";
            using (DataTable dt = Classes.DB.Select(conn, query))
            {
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        transactions = dt.AsEnumerable().Select(x => new Transactions()
                        {
                            Id = x.Field<int>("id"),
                            Client_id = x.Field<decimal>("client_id"),
                            Fullname = x.Field<string>("fullname"),
                            PspName = x.Field<string>("name"),
                            Deposit_currency = x.Field<string>("deposit_currency"),
                            Deposit_amount = x.Field<decimal>("deposit_amount"),
                            Exchange_currency = x.Field<string>("exchange_currency"),
                            Exchange_amount = x.Field<decimal>("exchange_amount"),
                            Psp_status = x.Field<string>("psp_status"),
                            Credited_status = x.Field<string>("credited_status"),
                            Created_date = x.Field<DateTime>("created_date"),
                            PaymentReference = x.Field<string>("payment_reference"),
                            CardLast4 = x.Field<string>("card_last4"),
                            CardholderName = x.Field<string>("cardholder_name"),
                            Notes = x.Field<string>("notes"),
                            Type = x.Field<int?>("type").HasValue ? x.Field<Classes.Instbtc.Models.TransactionType>("type") : Classes.Instbtc.Models.TransactionType.UNDEFINED,
                            Referal = x.Field<string>("referral")
                        }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return transactions;
        }

        public static bool UpdatePspStatus(NpgsqlConnection conn, int id, string status)
        {
            string query = "UPDATE transactions SET psp_status = @Status WHERE id = @Id;";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                Classes.DB.Update(conn, query, cmd);
            }
            catch (Exception e)
            {
                string message = e.Message;
                return false;
            }

            return true;
        }

        public static bool UpdateCreditedStatus(NpgsqlConnection conn, int id, string status)
        {
            string query = "UPDATE transactions SET credited_status = @Status WHERE id = @Id;";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                var qResult = Classes.DB.Update(conn, query, cmd);
            }
            catch (Exception e)
            {
                string message = e.Message;
                return false;
            }

            return true;
        }

        public static string TransactionCount(NpgsqlConnection conn, string parameter = "")
        {
            string query = $"SELECT COUNT(*) FROM clients cl INNER JOIN transactions tr on cl.id = tr.client_id INNER JOIN psp ps ON tr.psp_id = ps.id WHERE 1=1 {parameter}";
            var getTransactions = Classes.DB.SelectScalar(conn, query);
            string TotalTransactions = getTransactions != null ? getTransactions.ToString() : "0";
            return TotalTransactions;
        }

        public static Transactions GetTransaction(NpgsqlConnection conn, string id)
        {
            Transactions transactions = new Transactions();
            string query = "SELECT  tr.id, cl.first_name::text || ' ' || cl.last_name AS fullname, tr.deposit_currency, tr.deposit_amount, tr.exchange_currency, tr.exchange_amount, tr.psp_status, tr.credited_status, tr.created_date, tr.client_id, ps.name as pspname FROM clients cl INNER JOIN transactions tr on cl.id = tr.client_id INNER JOIN psp ps ON tr.psp_id = ps.id WHERE tr.id=@Id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            using (var reader = Classes.DB.ExecuteReader(conn, query, cmd))
            {
                if (reader.Read())
                {
                    transactions.Id = Convert.ToInt32(reader["id"]);
                    transactions.Client_id = Convert.ToDecimal(reader["client_id"]);
                    transactions.Deposit_currency = reader["deposit_currency"].ToString();
                    transactions.Deposit_amount = Convert.ToDecimal(reader["deposit_amount"]);
                    transactions.Exchange_currency = reader["exchange_currency"].ToString();
                    transactions.Exchange_amount = Convert.ToDecimal(reader["exchange_amount"]);
                    transactions.Psp_status = reader["psp_status"].ToString();
                    transactions.Credited_status = reader["credited_status"].ToString();
                    transactions.Created_date = Convert.ToDateTime(reader["created_date"]);
                    transactions.Fullname = reader["fullname"].ToString();
                    transactions.PspName = reader["pspname"].ToString();
                }
            }
            return transactions;
        }

        public string GetCreditedStatusLabel()
        {
            return this.Credited_status.ToString().Replace("_", " ").ToUpper();
        }

        public static string GetPspStatusValue(string e)
        {
            return (string)e;
        }

        public static string GetCreditedStatusValue(string e)
        {
            return (string)e;
        }

        public static string GetEnumLabel(Enum e)
        {
            string[] rawString = e.ToString().Replace('_', ' ').Split();
            List<string> modString = new List<string>();

            foreach (var each in rawString)
                modString.Add(each);

            return String.Join(" ", modString);
        }

        public static string CheckTransactionIfExist(string PaymentReference)
        {
            string result = string.Empty;
            string query = string.Format("SELECT id FROM transactions WHERE payment_reference = '{0}'", PaymentReference);
            using (var con = Classes.DB.InstBTCDB("instbtc"))
            {
                try
                {
                    var dt = Classes.DB.Select(con, query);
                    if (dt.Rows.Count > 0)
                    {
                        result = dt.Rows[0]["id"].ToString();
                    }
                }
                catch { result = "Internal Error"; }
            }
            return result;
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


        //public enum CreditedStatus 
        //{
        //    Credited = 1,
        //    NotCredited = 2
        //};

        //public enum PspStatus 
        //{ 
        //    Approved = 1,
        //    Pending = 2,
        //    Rejected = 3,
        //};

    }
}