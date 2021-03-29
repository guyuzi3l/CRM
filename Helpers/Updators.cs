using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using System.Threading;

namespace CRM.Helpers
{
    public class Updators
    {
        public static void UpdateAllWDNoRefHash()
        {
            string select_query = $"SELECT id,user_id,amount,service_fee FROM withdrawal WHERE refference_hash='N/A' AND transaction_currency='BTC' LIMIT 5";
            string update_query = string.Empty;

            DataTable Withdrawals = new DataTable();
            List<DataModel> dataModels = new List<DataModel>();
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                Withdrawals = Classes.DB.Select(conn, select_query);

                try
                {
                    if (Withdrawals.Rows.Count > 0)
                    {
                        dataModels = Withdrawals.AsEnumerable().Select(s => new DataModel
                        {
                            id = s.Field<int?>("id"),
                            user_id = s.Field<int?>("user_id"),
                            amount = s.Field<decimal>("amount"),
                            service_fee = s.Field<decimal>("service_fee"),
                        }).ToList();
                    }


                    for (int i = 0; dataModels.Count > i; i++)
                    {
                        double amount2send = 0;
                        decimal rawamount = dataModels[i].amount;
                        decimal rawservicefee = dataModels[i].service_fee;
                        int? withrawal_id = dataModels[i].id;
                        int? user_id = dataModels[i].user_id;

                        amount2send = Double.Parse((rawamount - rawservicefee).ToString());
                        string reffHash = string.Empty;


                        var res = Classes.btcWallet.Utilitties.createSelfTransaction(amount2send, "http://172.31.39.117:4000/");
                        if (res.error == null)
                            reffHash = res.txResponse.hash;

                        if (!string.IsNullOrEmpty(reffHash))
                        {
                            update_query = $"UPDATE withdrawal SET refference_hash='{reffHash}' WHERE id={withrawal_id} AND user_id={user_id}";
                            var updateResponse = Classes.DB.Update(conn, update_query);
                            HttpContext.Current.Response.Write($"Row:[{i}],UpdateResponse:[{updateResponse}],Query:[{update_query}],Amount2Send:[{amount2send}]<br/><br/>");
                        }
                        Thread.Sleep(2000);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private class DataModel
        {
            public int? id { get; set; }
            public int? user_id { get; set; }
            public decimal amount { get; set; }
            public decimal service_fee { get; set; }
        }
    }
}