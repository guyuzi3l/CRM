using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class WithdrawalModel
    {
        public class MWithdrawal
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string WalletId { get; set; }
            public decimal Amount { get; set; }
            public string ClientName { get; set; }
            public string DocumentId { get; set; }
            public string Status { get; set; }
            public DocumentStatus DocumentStatus { get; set; }
            public string DocuLink { get; set; }
            public DateTime CreatedDate { get; set; }
            public decimal? UsdConversion { get; set; }
            public decimal? ServiceFee { get; set; }
            public decimal? ServiceFeeUsd { get; set; }
            public virtual ClientModel ClientModel { get; set; }
            public int? TransactionId { get; set; }
            public decimal? CurrentBalance { get; set; }
            public decimal? EuroBalance { get; set; }
            public string refference_hash { get; set; }
            public string transaction_currency { get; set; }

            public string html { get; set; }
            public string signature { get; set; }
            public string is_sign { get; set; }

            public string referral { get; set; }
        }

        public class WithdrawalSearchModel
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public string WalletId { get; set; }
            public string Amount { get; set; }
            public string ClientName { get; set; }
            public string DocumentId { get; set; }
            public string Status { get; set; }
            public string DocumentStatus { get; set; }
            public string DocuLink { get; set; }
            public string CreatedDate { get; set; }
            public string UsdConversion { get; set; }
            public virtual ClientModelSearch ClientModel { get; set; }
            public string refference_hash { get; set; }
            public string transaction_currency { get; set; }
        }

        public enum DocumentStatus
        {
            OUT_FOR_SIGNATURE,
            COMPLETED,
            DECLINE
        }
    }
}