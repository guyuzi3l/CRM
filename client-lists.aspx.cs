using CRM.Models;
using CRM.Utilities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM
{
    public partial class client_lists : System.Web.UI.Page
    {
        public string TotalClients = String.Empty;
        public StringBuilder ClientsInformations = new StringBuilder();
        protected bool SearchHasResult { get; set; } = true;


        public String OrderBy { get; set; } = "kl342lk234"; // garbage value
        public String OrderByMode { get; set; }

        public Dictionary<string, string> OrderByDictionary { get; set; } = new Dictionary<string, string>()
        {
            { "Client Created Date", "created_date" },
            { "Client Name", "fullname" },
            { "Client Email", "email" },
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            this.OrderByMode = Request.QueryString["mode"] ?? "desc";
            this.OrderBy = Request.QueryString["orderby"] ?? "";

            if (this.OrderByMode.ToLower() != "desc")
                this.OrderByMode = "asc";

            if (!this.OrderByDictionary.ContainsKey(this.OrderBy))
                this.OrderBy = this.OrderByDictionary.Keys.First();

            ShowClients();
        }
        
        protected List<ClientModel> ApplyPagination(PaginationUtil pager, List<ClientModel> list, int pageSize, string qkey, string total) 
        {
            pager.ItemCountPerPage = pageSize;
            pager.QueryKey = qkey;
            pager.CurrentPage = pager.ParseQueryString(Request.QueryString[pager.QueryKey]);
            pager.TotalItems = int.Parse(total);
            int offset = pager.GetRecordOffset();
            return list.Skip(offset).Take(pager.ItemCountPerPage).ToList<ClientModel>();
        }

        public void ShowClients()
        {
            string clientId = Request.QueryString["clientId"];
            string phonenumber = Request.QueryString["phonenumber"];
            string txtClientEmail = Request.QueryString["txtClientEmail"];
            string firstname = Request.QueryString["firstname"];
            string lastname = Request.QueryString["lastname"];
            string txtCreatedDate = Request.QueryString["txtCreatedDate"];


            List<ClientModel> clients = new List<ClientModel>();
            using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
            {
                string query = getQuery(conn, clientId, phonenumber, firstname, lastname, txtClientEmail, txtCreatedDate);

                if (query.Length > 0)
                {
                    clients = Utilities.Clients.GetClients(conn, query , orderby: $"{this.OrderByDictionary[this.OrderBy]} {this.OrderByMode.ToUpper()}");
                }
                else 
                {
                    clients = Utilities.Clients.GetClients(conn, orderby: $"{this.OrderByDictionary[this.OrderBy]} {this.OrderByMode.ToUpper()}");
                }
                
                TotalClients = Utilities.Clients.ClientsCount(conn);
            }

            List<ClientModel> filteredClients = this.ApplyPagination(ClientListPager, clients, 10, "p", this.TotalClients);

            if (filteredClients.Count > 0)
            {
                foreach (var client in filteredClients)
                {
                    string created_date = client.Created_date == DateTime.MinValue ? "NONE" : client.Created_date.ToString();

                    ClientsInformations.Append("<tr>")
                        .AppendFormat("<td>{0}</td>", client.Id)
                        .AppendFormat("<td>{0}</td>", client.First_name + " " + client.Last_name)
                        .AppendFormat("<td>{0}</td>", client.Email)
                        .AppendFormat("<td>{0}</td>", client.Phone_prefix + client.Phone_number)
                        .AppendFormat("<td>{0}</td>", client.Phone_verification)
                        .AppendFormat("<td>{0}</td>", client.Referral)
                        .AppendFormat("<td>{0}</td>", client.client_ip)
                        .AppendFormat("<td>{0}</td>", client.Created_date)
                        .AppendFormat("<td class='text-center'> <a href='/client-detail.aspx?id={0}' class='btn btn-sm btn-info'>view</a></td>", client.Id);
                    ClientsInformations.Append("</tr>");
                    clientsBody.InnerHtml = ClientsInformations.ToString();
                }
            }
            else
            {
                clientsBody.InnerHtml = $"<tr><td colspan='5'><p class='text-center text-danger'>No Results Found.</p></td></tr>";
                this.SearchHasResult = false;
            }
        }
        private string getQuery(NpgsqlConnection conn, string clientId, string phonenumber, string firstname, string lastname, string txtClientEmail, string txtCreatedDate)
        {
            string query = string.Empty;

            if (clientId != String.Empty && clientId != null)
                query += String.Format(" AND id = '{0}'", clientId);

            if (phonenumber != String.Empty && phonenumber != null)
                query += String.Format(" AND phone_number LIKE '%{0}%'", phonenumber);

            if (txtClientEmail != String.Empty && txtClientEmail != null)
                query += String.Format(" AND email_hash = '{0}'", Classes.encryption.Hash(txtClientEmail));

            if (firstname != String.Empty && firstname != null)
                query += String.Format(" AND first_name LIKE '%{0}%'", firstname);

            if (txtCreatedDate != String.Empty && txtCreatedDate != null)
                query += String.Format(" AND to_char(created_date,'YYYY-MM-DD') LIKE '%{0}%'", txtCreatedDate);


            if (lastname != String.Empty && lastname != null)
                query += String.Format(" AND last_name LIKE '%{0}%'", lastname);

            return query;
        }
    }
}