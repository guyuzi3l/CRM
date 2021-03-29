using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class ConversionReqModel
    {
        public int id { get; set; }
        public string client_name { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public DateTime created_date { get; set; }
        public int? transaction_id { get; set; }
        public string transaction_currency { get; set; }
        public string credited_status { get; set; }
        public int client_id { get; set; }
        public decimal conversion_amount { get; set; }
        public string conversion_currency { get; set; }
        public string email { get; set; }
        public decimal eur_balance { get; set; }
        public decimal btc_balance { get; set; }
    }
}