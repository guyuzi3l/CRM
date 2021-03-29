using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Npgsql;
using static CRM.Models.WithdrawalModel;

namespace CRM.Utilities
{
    public class WithdrawalUtilities
    {
        public static List<MWithdrawal> WithdrawalRequest(NpgsqlConnection con)
        {
            List<MWithdrawal> withdrawalRequest = new List<MWithdrawal>();
            string query = "SELECT id, wallet_id, user_id, client_name, document_id, credited_status, document_status, document_link, created_date, amount FROM withdrawal ORDER BY created_date DESC";

            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            DataTable dt = Classes.DB.Select(con, query, cmd);
            if (dt.Rows.Count > 0)
            {
                withdrawalRequest = dt.AsEnumerable().Select(x => new MWithdrawal()
                {
                    Id = x.Field<int>("id"),
                    WalletId = x.Field<string>("wallet_id"),
                    UserId = x.Field<int>("user_id"),
                    ClientName = x.Field<string>("client_name"),
                    DocumentId = x.Field<string>("document_id"),
                    Status = x.Field<string>("credited_status"),
                    DocumentStatus = (DocumentStatus)x.Field<int>("document_status"),
                    DocuLink = x.Field<string>("document_link"),
                    CreatedDate = x.Field<DateTime>("created_date"),
                    Amount = x.Field<decimal>("amount"),
                    ServiceFee = x.Field<decimal?>("service_fee")

                }).ToList();
            }
            return withdrawalRequest;
        }

        public static List<MWithdrawal> WithdrawalRequest(NpgsqlConnection con, NpgsqlCommand command, string addedquery, int offset = 0, int limit = 0)
        {
            List<MWithdrawal> withdrawalRequest = new List<MWithdrawal>();
            //string query = $"SELECT w.* FROM withdrawal w INNER JOIN clients c ON w.user_id = c.id WHERE 1=1 {addedquery} ORDER BY w.created_date DESC OFFSET {offset} LIMIT {limit} ";

            //string query = $"SELECT w.*, SUM(t.exchange_amount) as currentbalance,c.first_name FROM transactions t INNER JOIN clients c on t.client_id = c.id LEFT JOIN withdrawal w ON c.id = w.user_id WHERE t.exchange_currency = 'BTC' AND t.psp_status = 'Approved' AND t.client_id = c.id {addedquery} GROUP BY c.first_name, w.id, t.exchange_currency OFFSET {offset} LIMIT {limit} ";

            string query = $"SELECT w.*,e.html,e.signature,c.referral, " +
                $"(SELECT COALESCE(SUM(t.exchange_amount),0) FROM transactions t WHERE t.transaction_currency = 'BTC' AND t.psp_status = 'Approved' AND t.client_id = w.user_id) as btc_balance," +
                $"(SELECT COALESCE(SUM(t.exchange_amount),0) FROM transactions t WHERE t.transaction_currency = 'EUR' AND t.psp_status = 'Approved' AND t.client_id = w.user_id) as eur_balance " +
                $"FROM withdrawal w " +
                $"INNER JOIN clients c on c.id = w.user_id " +
                $"LEFT JOIN emails e on w.id = e.wd_id " +
                $"WHERE 1=1 {addedquery} " +
                $"ORDER BY w.created_date DESC OFFSET {offset} LIMIT {limit} ";


            //Get Given Parameters Collection
            NpgsqlParameterCollection parameters = command.Parameters;

            //Pass Parameter Collection into Current parameter
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            foreach(NpgsqlParameter parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.NpgsqlValue);
            }

            //Fetch Withdrawal Table
            DataTable dt = Classes.DB.Select(con, query, cmd);
            if (dt.Rows.Count > 0)
            {
                foreach (var item in dt.AsEnumerable())
                {
                    var i = item.Field<int?>("id");
                    if (i != null)
                    {
                        var wr = new MWithdrawal();

                        wr.Id = item.Field<int>("id");
                        wr.WalletId = item.Field<string>("wallet_id");
                        wr.referral = item.Field<string>("referral");
                        wr.UserId = item.Field<int>("user_id");
                        wr.ClientName = item.Field<string>("client_name");
                        wr.DocumentId = item.Field<string>("document_id");
                        wr.Status = item.Field<string>("credited_status");
                        wr.DocumentStatus = (DocumentStatus)item.Field<int>("document_status");
                        wr.DocuLink = item.Field<string>("document_link");
                        wr.CreatedDate = item.Field<DateTime>("created_date");
                        wr.Amount = item.Field<decimal>("amount");
                        wr.UsdConversion = item.Field<decimal?>("usd_conversion");
                        wr.ServiceFee = item.Field<decimal?>("service_fee");
                        wr.CurrentBalance = item.Field<decimal?>("btc_balance");
                        wr.EuroBalance = item.Field<decimal?>("eur_balance");
                        wr.refference_hash = !string.IsNullOrEmpty(item.Field<string>("refference_hash")) ? item.Field<string>("refference_hash"): "";
                        wr.transaction_currency = !string.IsNullOrEmpty(item.Field<string>("transaction_currency")) ? item.Field<string>("transaction_currency") : "";
                        wr.html = !string.IsNullOrEmpty(item.Field<string>("html")) ? item.Field<string>("html") : "";
                        wr.signature = !string.IsNullOrEmpty(item.Field<string>("signature")) ? item.Field<string>("signature") : "";
                        wr.is_sign = !string.IsNullOrEmpty(item.Field<string>("is_sign")) ? item.Field<string>("is_sign") : "NO";
                        withdrawalRequest.Add(wr);
                    }
                }
            }
            return withdrawalRequest;
            //return withdrawalRequest.OrderByDescending(o => o.CreatedDate).ToList();
        }

        public static MWithdrawal GetWithdrawal(NpgsqlConnection conn, string id)
        {
            MWithdrawal withdrawal = new MWithdrawal();
            string query = "SELECT id, wallet_id, transaction_id, user_id, client_name, document_id, credited_status, document_status, document_link, created_date, amount, usd_conversion, service_fee,refference_hash,transaction_currency FROM withdrawal WHERE id=@Id ORDER BY created_date DESC";
            NpgsqlCommand command = new NpgsqlCommand(query, conn);
            command.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            using (NpgsqlDataReader reader = Classes.DB.ExecuteReader(conn, query, command))
            {
                if (reader.Read())
                {
                    withdrawal.Id = Convert.ToInt32(reader["id"]);
                    withdrawal.WalletId = reader["wallet_id"].ToString();
                    withdrawal.UserId = Convert.ToInt32(reader["user_id"]);
                    withdrawal.ClientName = reader["client_name"].ToString();
                    withdrawal.DocumentId = reader["document_id"].ToString();
                    withdrawal.Status = reader["credited_status"].ToString();
                    withdrawal.DocumentStatus = (DocumentStatus)reader["document_status"];
                    withdrawal.DocuLink = reader["document_link"].ToString();
                    withdrawal.CreatedDate = Convert.ToDateTime(reader["created_date"]);
                    withdrawal.Amount = Convert.ToDecimal(reader["amount"]);
                    withdrawal.ServiceFee = Convert.ToDecimal(reader["service_fee"]);
                    withdrawal.TransactionId = !string.IsNullOrEmpty(reader["transaction_id"].ToString()) ? Convert.ToInt32(reader["transaction_id"]) : -1;
                    withdrawal.UsdConversion = !string.IsNullOrEmpty(reader["usd_conversion"].ToString()) ? Convert.ToDecimal(reader["usd_conversion"]) : 0;
                    withdrawal.refference_hash = !string.IsNullOrEmpty(reader["refference_hash"].ToString()) ? reader["refference_hash"].ToString() : "";
                    withdrawal.transaction_currency = !string.IsNullOrEmpty(reader["transaction_currency"].ToString()) ? reader["transaction_currency"].ToString() : "";
                }
            }

            return withdrawal;
        }

        public static string GetWithdrawalCount(NpgsqlConnection conn, NpgsqlCommand command, string addedquery)
        {
            string query = $"SELECT COUNT (*) FROM withdrawal w INNER JOIN clients c ON w.user_id = c.id WHERE 1=1 {addedquery}";

            //Get Given Parameters Collection
            NpgsqlParameterCollection parameters = command.Parameters;

            //Pass Parameter Collection into Current parameter
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            foreach (NpgsqlParameter parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.NpgsqlValue);
            }

            //GetCount
            var res = Classes.DB.SelectScalar(conn, query, cmd);
            string TotalWithdrawals = res != null ? res.ToString() : "0";
            return TotalWithdrawals;
        }

        public static bool UpdateCreditedStatus(NpgsqlConnection conn, string id, string status, string ref_hash)
        {
            string query = "UPDATE withdrawal SET credited_status = @Status, refference_hash = @Reference WHERE id = @Id;";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Reference", ref_hash);
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

        public static bool UpdateTransactionId(NpgsqlConnection conn, string id, int transactionId)
        {
            string query = "UPDATE withdrawal SET transaction_id = @TransactionId WHERE id = @Id;";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TransactionId", transactionId);
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

        public static bool UpdateUsdValueAmount(NpgsqlConnection conn, string id)
        {

            MWithdrawal withdrawal = new MWithdrawal();
            withdrawal = WithdrawalUtilities.GetWithdrawal(conn, id);

            #region Make the convertion to usd
            decimal? WithAmount = withdrawal.Amount - withdrawal.ServiceFee;
            withdrawal.UsdConversion = Convert.ToDecimal(Classes.Instbtc.Utilities.Conversion.GetCurrencyAmount(WithAmount.ToString(), "USD"));
            withdrawal.ServiceFeeUsd = Convert.ToDecimal(Classes.Instbtc.Utilities.Conversion.GetCurrencyAmount(withdrawal.ServiceFee.ToString(), "USD"));
            #endregion
            string query = "UPDATE withdrawal SET usd_conversion = @usdAmount, service_fee_usd = @serviceFeeUsd WHERE id = @Id;";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@usdAmount", withdrawal.UsdConversion);
                cmd.Parameters.AddWithValue("@serviceFeeUsd", withdrawal.ServiceFeeUsd);
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
    }
}