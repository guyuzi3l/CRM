using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace CRM.Utilities
{
    public class ConversionReqUtilities
    {
        public static string CreateConversionRequest(Models.ConversionReqModel m)
        {
            string query = "INSERT INTO conversion_requests(id,client_name,amount,currency,created_date,transaction_id,transaction_currency,credited_status,client_id,conversion_amount,conversion_currency) ";
            query += $"VALUES(default,@client_name,@amount,@currency,@created_date,@transaction_id,@transaction_currency,@credited_status,@client_id,@conversion_amount,@conversion_currency) RETURNING id";

            string conversion_id = string.Empty;

            using (var con = Classes.DB.InstBTCDB("instbtc"))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@client_name", m.client_name);
                cmd.Parameters.AddWithValue("@amount", m.amount);
                cmd.Parameters.AddWithValue("@currency", m.currency);
                cmd.Parameters.AddWithValue("@created_date", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@transaction_id", m.transaction_id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@transaction_currency", m.transaction_currency);
                cmd.Parameters.AddWithValue("@credited_status", m.credited_status);
                cmd.Parameters.AddWithValue("@client_id", m.client_id);
                cmd.Parameters.AddWithValue("@conversion_amount", m.conversion_amount);
                cmd.Parameters.AddWithValue("@conversion_currency", m.conversion_currency);

                conversion_id = Classes.DB.SelectScalar(con, query, cmd)?.ToString();
            }

            return conversion_id;
        }

        public static List<Models.ConversionReqModel> GetConversionRequestbyClientID(NpgsqlConnection conn, string ClientId)
        {
            List<Models.ConversionReqModel> Request = new List<Models.ConversionReqModel>();
            string query = $"SELECT * FROM conversion_requests WHERE client_id = @ClientId ORDER BY created_date DESC";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClientId", int.Parse(ClientId));
            var dt = Classes.DB.Select(conn, query, cmd);

            if (dt.Rows.Count > 0)
            {
                Request = dt.AsEnumerable().Select(s => new Models.ConversionReqModel
                {
                    client_id = s.Field<int>("client_id"),
                    id = s.Field<int>("id"),
                    transaction_id = s.Field<int?>("transaction_id"),
                    client_name = s.Field<string>("client_name"),
                    amount = Decimal.Parse(s.Field<double>("amount").ToString()),
                    currency = s.Field<string>("currency"),
                    conversion_amount = Decimal.Parse(s.Field<double>("conversion_amount").ToString()),
                    conversion_currency = s.Field<string>("conversion_currency"),
                    credited_status = s.Field<string>("credited_status"),
                    created_date = s.Field<DateTime>("created_Date")
                }).ToList();
            }

            return Request;
        }


        public static List<Models.ConversionReqModel> GetConversionRequest(NpgsqlConnection conn, string addedQuery, int offset = 0, int limit = 0)
        {
            List<Models.ConversionReqModel> Request = new List<Models.ConversionReqModel>();
            string query = $"SELECT cv.*," +
                $"(SELECT SUM(t.exchange_amount) FROM transactions t WHERE t.transaction_currency = 'BTC' AND t.psp_status = 'Approved' AND t.client_id = cv.client_id) as btc_balance," +
                $"(SELECT SUM(t.exchange_amount) FROM transactions t WHERE t.transaction_currency = 'EUR' AND t.psp_status = 'Approved' AND t.client_id = cv.client_id) as eur_balance," +
                $"cl.email " +
                $"FROM conversion_requests cv LEFT JOIN clients cl ON cv.client_id = cl.id " +
                $"WHERE 1=1 {addedQuery} " +
                $"ORDER BY cv.created_date DESC " +
                $"OFFSET {offset} LIMIT {limit}";

            var dt = Classes.DB.Select(conn, query);

            if (dt.Rows.Count > 0)
            {
                Request = dt.AsEnumerable().Select(s => new Models.ConversionReqModel
                {
                    client_id = s.Field<int>("client_id"),
                    id = s.Field<int>("id"),
                    transaction_id = s.Field<int?>("transaction_id"),
                    client_name = s.Field<string>("client_name"),
                    amount = Decimal.Parse(s.Field<double>("amount").ToString()),
                    currency = s.Field<string>("currency"),
                    conversion_amount = Decimal.Parse(s.Field<double>("conversion_amount").ToString()),
                    conversion_currency = s.Field<string>("conversion_currency"),
                    credited_status = s.Field<string>("credited_status"),
                    created_date = s.Field<DateTime>("created_Date"),
                    email = s.Field<string>("email"),
                    transaction_currency = s.Field<string>("transaction_currency"),
                    eur_balance = s.Field<decimal>("eur_balance"),
                    btc_balance = s.Field<decimal>("btc_balance"),
                }).ToList();
            }

            return Request;
        }

        public static List<Models.ConversionReqModel> GetConversionRequest(NpgsqlConnection conn,object addedQuery)
        {
            List<Models.ConversionReqModel> Request = new List<Models.ConversionReqModel>();
            string query = $"SELECT cv.*," +
                $"(SELECT SUM(t.exchange_amount) FROM transactions t WHERE t.transaction_currency = 'BTC' AND t.psp_status = 'Approved' AND t.client_id = cv.client_id) as btc_balance," +
                $"(SELECT SUM(t.exchange_amount) FROM transactions t WHERE t.transaction_currency = 'EUR' AND t.psp_status = 'Approved' AND t.client_id = cv.client_id) as eur_balance," +
                $"cl.email " +
                $"FROM conversion_requests cv LEFT JOIN clients cl ON cv.client_id = cl.id " +
                $"WHERE 1=1 {addedQuery?.ToString() ?? string.Empty} " +
                $"ORDER BY cv.created_date DESC ";
                

            var dt = Classes.DB.Select(conn, query);

            if (dt.Rows.Count > 0)
            {
                Request = dt.AsEnumerable().Select(s => new Models.ConversionReqModel
                {
                    client_id = s.Field<int>("client_id"),
                    id = s.Field<int>("id"),
                    transaction_id = s.Field<int?>("transaction_id"),
                    client_name = s.Field<string>("client_name"),
                    amount = Decimal.Parse(s.Field<double>("amount").ToString()),
                    currency = s.Field<string>("currency"),
                    conversion_amount = Decimal.Parse(s.Field<double>("conversion_amount").ToString()),
                    conversion_currency = s.Field<string>("conversion_currency"),
                    credited_status = s.Field<string>("credited_status"),
                    created_date = s.Field<DateTime>("created_Date"),
                    email = s.Field<string>("email"),
                    transaction_currency = s.Field<string>("transaction_currency"),
                    eur_balance = s.Field<decimal>("eur_balance"),
                    btc_balance = s.Field<decimal>("btc_balance"),
                }).ToList();
            }

            return Request;
        }

        public static List<Models.ConversionReqModel> GetConversionRequest(NpgsqlConnection conn, int offset = 0, int limit = 0)
        {
            List<Models.ConversionReqModel> Request = new List<Models.ConversionReqModel>();
            string query = $"SELECT * FROM conversion_requests ORDER BY created_date DESC OFFSET {offset} LIMIT {limit}";

            var dt = Classes.DB.Select(conn, query);

            if (dt.Rows.Count > 0)
            {
                Request = dt.AsEnumerable().Select(s => new Models.ConversionReqModel
                {
                    client_id = s.Field<int>("client_id"),
                    id = s.Field<int>("id"),
                    transaction_id = s.Field<int?>("transaction_id"),
                    client_name = s.Field<string>("client_name"),
                    amount = Decimal.Parse(s.Field<double>("amount").ToString()),
                    currency = s.Field<string>("currency"),
                    conversion_amount = Decimal.Parse(s.Field<double>("conversion_amount").ToString()),
                    conversion_currency = s.Field<string>("conversion_currency"),
                    credited_status = s.Field<string>("credited_status"),
                    created_date = s.Field<DateTime>("created_Date")
                }).ToList();
            }

            return Request;
        }

        public static List<Models.ConversionReqModel> GetConversionRequestById(NpgsqlConnection conn, string conversion_id)
        {
            List<Models.ConversionReqModel> Request = new List<Models.ConversionReqModel>();
            string query = $"SELECT * FROM conversion_requests WHERE id = @id ORDER BY created_date DESC";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", int.Parse(conversion_id));
            var dt = Classes.DB.Select(conn, query, cmd);

            if (dt.Rows.Count > 0)
            {
                Request = dt.AsEnumerable().Select(s => new Models.ConversionReqModel
                {
                    client_id = s.Field<int>("client_id"),
                    id = s.Field<int>("id"),
                    transaction_id = s.Field<int?>("transaction_id"),
                    client_name = s.Field<string>("client_name"),
                    amount = Decimal.Parse(s.Field<double>("amount").ToString()),
                    currency = s.Field<string>("currency"),
                    conversion_amount = Decimal.Parse(s.Field<double>("conversion_amount").ToString()),
                    conversion_currency = s.Field<string>("conversion_currency"),
                    credited_status = s.Field<string>("credited_status"),
                    created_date = s.Field<DateTime>("created_Date"),
                    transaction_currency = s.Field<string>("transaction_currency")
                }).ToList();
            }

            return Request;
        }

        public static  void UpdateConversionRequest(NpgsqlConnection conn, string query, NpgsqlCommand cmd)
        {
            Classes.DB.Update(conn, query, cmd);
        }

        public static string ProcessConversionRequest(string conversion_id,string status)
        {
            List<Models.ConversionReqModel> RequestInfo = new List<Models.ConversionReqModel>();
            Classes.Instbtc.Models.TransactionModel Transaction = new Classes.Instbtc.Models.TransactionModel();

            using (var con = Classes.DB.InstBTCDB("instbtc"))
            {
                RequestInfo = GetConversionRequestById(con, conversion_id);

                if (RequestInfo.Count > 0)
                {

                    var ClientBitcoinBalance = Clients.GetUserBtcBalance(con, RequestInfo.FirstOrDefault().client_id.ToString());
                    var ClientEuroBalance = Clients.GetUserEurBalance(con, RequestInfo.FirstOrDefault().client_id.ToString());

                    #region Hold The Params
                    decimal from_amount = RequestInfo.FirstOrDefault().amount;
                    string from_currency = RequestInfo.FirstOrDefault().currency;
                    decimal to_amount = RequestInfo.FirstOrDefault().conversion_amount;
                    string to_currency = RequestInfo.FirstOrDefault().conversion_currency;
                    string transaction_currency = RequestInfo.FirstOrDefault().transaction_currency;
                    int client_id = RequestInfo.FirstOrDefault().client_id;
                    #endregion

                    #region Update Conversion Request Status
                    string query = $"UPDATE conversion_requests SET credited_status = @credited_status WHERE id=@id";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@credited_status", status);
                    cmd.Parameters.AddWithValue("@id", int.Parse(conversion_id));
                    UpdateConversionRequest(con, query, cmd);
                    #endregion

                    if (transaction_currency == "BTC" && status == "Approved")
                    {
                        #region Check if Bitcoin Balance is still Enough
                        if (from_amount > Convert.ToDecimal(ClientBitcoinBalance))
                        {
                            return "Not Enough Bitcoin Balance";
                        }
                        #endregion

                        #region Add Funds on EUR Wallet Balance

                        #region Create Transaction Object
                        Transaction = new Classes.Instbtc.Models.TransactionModel
                        {
                            Psp_ID = 11,
                            Deposit_Currency = from_currency,
                            Deposit_Amount = from_amount,
                            Exchange_Currency = to_currency,
                            Exchange_Amount = to_amount,
                            Created_Date = DateTime.UtcNow,
                            Client_ID = Convert.ToDecimal(client_id),
                            Psp_Status = status,
                            Credited_Status = "Credited",
                            PaymentReference = "",
                            Notes = $"Converted {from_amount} {from_currency}, {to_currency} Wallet recieved {to_amount} {to_currency}.",
                            type = Classes.Instbtc.Models.TransactionType.CONVERSION,
                            CardLast4 = "",
                            CardHolderName = "",
                            Transaction_Currency = "EUR"
                        };
                        #endregion

                        #region Create Transaction
                        var eur_trans_id = Classes.Instbtc.Create.Transactions.CreateTransaction(Transaction)?.ToString();
                        #endregion

                        #endregion

                        #region Deduct BTC Wallet Balance

                        #region Create Transaction Object
                        Transaction = new Classes.Instbtc.Models.TransactionModel
                        {
                            Psp_ID = 11,
                            Deposit_Currency = from_currency,
                            Deposit_Amount = from_amount * -1,
                            Exchange_Currency = from_currency,
                            Exchange_Amount = from_amount * -1,
                            Created_Date = DateTime.UtcNow,
                            Client_ID = Convert.ToDecimal(client_id),
                            Psp_Status = status,
                            Credited_Status = "Credited",
                            PaymentReference = "",
                            Notes = $"Deducted {from_amount} {from_currency} on BTC Wallet. [reff trans: {eur_trans_id}]",
                            type = Classes.Instbtc.Models.TransactionType.CONVERSION,
                            CardLast4 = "",
                            CardHolderName = "",
                            Transaction_Currency = "BTC"
                        };
                        #endregion

                        #region Create Transaction
                        var btc_trans_id = Classes.Instbtc.Create.Transactions.CreateTransaction(Transaction)?.ToString();
                        #endregion

                        #endregion

                        #region Update Conversion Request Transaction Id
                        query = $"UPDATE conversion_requests SET transaction_id = @transaction_id WHERE id=@id";
                        cmd = new NpgsqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@transaction_id", int.Parse(eur_trans_id));
                        cmd.Parameters.AddWithValue("@id", int.Parse(conversion_id));
                        UpdateConversionRequest(con, query, cmd);
                        #endregion

                        return "Success, conversion request is updated!";

                    }

                    if (transaction_currency == "EUR" && status == "Approved")
                    {
                        #region Check if Bitcoin Balance is still Enough
                        if (from_amount > Convert.ToDecimal(ClientEuroBalance))
                        {
                            return "Not Enough Euro Balance";
                        }
                        #endregion

                        #region Add Funds on BTC Wallet

                        #region Create Transaction Object
                        Transaction = new Classes.Instbtc.Models.TransactionModel
                        {
                            Psp_ID = 11,
                            Deposit_Currency = from_currency,
                            Deposit_Amount = from_amount,
                            Exchange_Currency = to_currency,
                            Exchange_Amount = to_amount,
                            Created_Date = DateTime.UtcNow,
                            Client_ID = Convert.ToDecimal(client_id),
                            Psp_Status = status,
                            Credited_Status = "Credited",
                            PaymentReference = "",
                            Notes = $"Converted {from_amount} {from_currency}. {to_currency} Wallet recieved {to_amount} {to_currency}.",
                            type = Classes.Instbtc.Models.TransactionType.CONVERSION,
                            CardLast4 = "",
                            CardHolderName = "",
                            Transaction_Currency = "BTC"
                        };
                        #endregion

                        #region Create Transaction
                        var btc_trans_id = Classes.Instbtc.Create.Transactions.CreateTransaction(Transaction)?.ToString();
                        #endregion

                        #endregion

                        #region Deduct Funds on EUR Wallet

                        #region Create Transaction Object
                        Transaction = new Classes.Instbtc.Models.TransactionModel
                        {
                            Psp_ID = 11,
                            Deposit_Currency = from_currency,
                            Deposit_Amount = from_amount * -1,
                            Exchange_Currency = from_currency,
                            Exchange_Amount = from_amount * -1,
                            Created_Date = DateTime.UtcNow,
                            Client_ID = Convert.ToDecimal(client_id),
                            Psp_Status = status,
                            Credited_Status = "Credited",
                            PaymentReference = "",
                            Notes = $"Deducted {from_amount} {from_currency} on {from_currency} Wallet. [reff trans: {btc_trans_id}]",
                            type = Classes.Instbtc.Models.TransactionType.CONVERSION,
                            CardLast4 = "",
                            CardHolderName = "",
                            Transaction_Currency = "EUR"
                        };
                        #endregion

                        #region Create Transaction
                        var eur_trans_id = Classes.Instbtc.Create.Transactions.CreateTransaction(Transaction)?.ToString();
                        #endregion

                        #endregion

                        #region Update Conversion Request Transaction Id
                        query = $"UPDATE conversion_requests SET transaction_id = @transaction_id WHERE id=@id";
                        cmd = new NpgsqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@transaction_id", int.Parse(btc_trans_id));
                        cmd.Parameters.AddWithValue("@id", int.Parse(conversion_id));
                        UpdateConversionRequest(con, query, cmd);
                        #endregion

                        return "Success, conversion request is updated!"; ;
                    }

                }
            }
            return "Success, conversion request is updated!";
        }
    }
}