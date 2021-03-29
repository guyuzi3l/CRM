using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string Phone_prefix { get; set; }
        public string Phone_number { get; set; }
        public string Utm_source { get; set; }
        public DateTime Created_date { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Max_deposit { get; set; }
        public string Phone_verification { get; set; }
        public string client_ip { get; set; }
        public string Referral { get; set; }
        public string walletAddress { get; set; }

        public string dob { get; set; }
        public string actual_client_ip { get; set; }
    }

    public class ClientModelSearch
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string Phone_prefix { get; set; }
        public string Phone_number { get; set; }
        public string Utm_source { get; set; }
        public string Created_date { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Max_deposit { get; set; }
        public string Phone_verification { get; set; }
        public string Referral { get; set; }
        public string walletAddress { get; set; }
    }
}