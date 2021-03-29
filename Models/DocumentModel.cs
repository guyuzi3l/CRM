using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class DocumentModel
    {
        public int Id { get; set; }
        public decimal ClientId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Base64Doc { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SubType { get; set; }
        public string Note { get; set; }
        public string aws_file { get; set; }
    }
}