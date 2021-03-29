using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Utilities
{
    public class TransferUtility
    {
        public static bool CreateTransfer(int transactionId, Classes.Instbtc.Models.TransactionModel transaction)
        {
            string result = "";
            decimal btc_amount = decimal.Parse(Classes.Instbtc.Utilities.Conversion.GetBTCAmountWithInterest(transaction.Exchange_Amount.ToString(), transaction.Exchange_Currency,out decimal deduction));
            
            using (var con = Classes.DB.InstBTCDB("instbtc"))
            {
                string insertTransferQuery = " INSERT INTO transfer(id,btc_amount,service_fee_btc_amount,created_date) ";
                insertTransferQuery += $"VALUES(default,{btc_amount},{deduction}, '{DateTime.UtcNow.ToString()}') returning id;";

                try
                {
                    result = Classes.DB.Insert(con, insertTransferQuery, out int? transferId);
                    if (result == "Success")
                    {
                        string insertTransferTransactionQuery = " INSERT INTO transfer_transaction(id,transfer_id,transaction_id) ";
                        insertTransferTransactionQuery += $"VALUES(default,{transferId},{transactionId}) returning id;";
                        result = Classes.DB.Insert(con, insertTransferTransactionQuery, out int? ttid);
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message.ToString();
                }
            }

            return result == "Success";
        }
    }
}