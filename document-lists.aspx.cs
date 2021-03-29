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
    public partial class document_lists : System.Web.UI.Page
    {
        public string TotalDocs = String.Empty;
        public StringBuilder ClientsInformations = new StringBuilder();
        public String OrderBy { get; set; } = "kl342lk234"; // garbage value
        public String OrderByMode { get; set; }

        public Dictionary<string, string> OrderByDictionary { get; set; } = new Dictionary<string, string>()
        {
            { "Document Created Date", "dc.created_date" },
            { "Client Name", "fullname" },
            { "Document Expiry Date", "dc.expiry_date" },
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            this.OrderByMode = Request.QueryString["mode"] ?? "desc";
            this.OrderBy = Request.QueryString["orderby"] ?? "";

            if (this.OrderByMode.ToLower() != "desc")
                this.OrderByMode = "asc";

            if (!this.OrderByDictionary.ContainsKey(this.OrderBy))
                this.OrderBy = this.OrderByDictionary.Keys.First();

            using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
            {
                string txtType = Request.QueryString["txtType"];
                string txtStatus = Request.QueryString["txtStatus"];
                string txtClientId = Request.QueryString["txtClientId"];
                string txtClientEmail = Request.QueryString["txtClientEmail"];
                string txtClientName = Request.QueryString["txtClientName"];
                string txtCreatedDate = Request.QueryString["txtCreatedDate"];

                List<Documents> docs = new List<Documents>();
                Dictionary<string, int> pagerDictionary = new Dictionary<string, int>();
                string paginationString = "page";

                DocListPager.initialize(paginationString, 10);
                TotalDocs = Utilities.Documents.DocumentsCount(conn);
                pagerDictionary = DocListPager.paginate(TotalDocs);
                docs = Utilities.Documents.GetDocuments(conn, pagerDictionary["offset"], pagerDictionary["limit"], orderby: $"{this.OrderByDictionary[this.OrderBy]} {this.OrderByMode.ToUpper()}");
                
                string query = this.getQuery(conn, txtType, txtStatus, txtClientId, txtClientEmail, txtClientName, txtCreatedDate);

                if (query.Length > 0)
                {
                    TotalDocs = Utilities.Documents.DocumentsCount(conn, query);
                    DocListPager.AppendExistedQuery();
                    pagerDictionary = DocListPager.paginate(TotalDocs);
                    docs = Utilities.Documents.GetDocuments(conn, query, pagerDictionary["offset"], pagerDictionary["limit"],orderby: $"{this.OrderByDictionary[this.OrderBy]} {this.OrderByMode.ToUpper()}");
                    
                }
                rowCount.InnerText = $"{TotalDocs} Records Found";
                ShowDocuments(conn, docs);
            }
        }

        private string getQuery(NpgsqlConnection conn, string txtType, string txtStatus, string txtClientId, string txtClientEmail, string txtClientName, string txtCreatedDate)
        {
            string query = string.Empty;

            if (txtType != String.Empty && txtType != null)
                query += String.Format(" AND type = '{0}'",txtType);
            
            if (txtStatus != String.Empty && txtStatus != null)
                query += String.Format(" AND status = '{0}'", txtStatus);

            if (txtClientId != String.Empty && txtClientId != null)
                query += String.Format(" AND client_id = '{0}'", txtClientId);

            if (txtCreatedDate != String.Empty && txtCreatedDate != null)
                query += String.Format(" AND dc.created_date::date = '{0}'", txtCreatedDate);
                
            if (txtClientEmail != String.Empty && txtClientEmail != null)
                query += String.Format(" AND client_id = '{0}'", getClientID(conn, txtClientEmail, "email"));
              
            if (txtClientName != String.Empty && txtClientName != null)
                query += String.Format(" AND client_id = '{0}'", getClientID(conn, txtClientName, "name")); 

            return query;
        }

        private string getClientID(NpgsqlConnection conn, string parameter, string parameterType)
        {
            string clientId = string.Empty;
            string query = string.Empty;
            if (parameterType == "email")
                query = "SELECT id FROM clients WHERE email=@Parameter";

            if (parameterType == "name")
                query = "SELECT id FROM clients WHERE CONCAT(first_name, ' ', last_name)=@Parameter";

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Parameter", parameter);

            var verifyId = Classes.DB.SelectScalar(conn, query, cmd);
            clientId = verifyId != null ? verifyId.ToString() : "";

            return clientId;
        }

        private void ShowDocuments(NpgsqlConnection conn, List<Documents> docs)
        {
            foreach (var doc in docs)
            {
                ClientsInformations.Append("<tr>")
                    .AppendFormat("<td>{0}</td>", doc.Id)
                    .AppendFormat("<td>{0}</td>", doc.ClientId)
                    .AppendFormat("<td>{0}</td>", doc.Fullname)
                    .AppendFormat("<td>{0}</td>", doc.Type)
                    .AppendFormat("<td>{0}</td>", doc.Status)
                    .AppendFormat("<td>{0}</td>", doc.CreatedDate)
                    .AppendFormat("<td>{0}</td>", doc.UpdateDate)
                    .AppendFormat("<td>{0}</td>", doc.ExpiryDate)
                    .AppendFormat("<td>{0}</td>", doc.SubType)
                    .AppendFormat("<td>{0}</td>", doc.Note)
                    .AppendFormat("<td><a href='{0}' target='_blank'>View File</a></td>", !string.IsNullOrEmpty(doc.Aws_File) ? doc.Aws_File : "#")
                    .AppendFormat("<td>{0}</td>", doc.CardLastFour)
                    .AppendFormat("<td><a href='/manage-documents.aspx?id={0}' class='btn btn-sm btn-info'>Manage</a></td>", doc.Id);
                ClientsInformations.Append("</tr>");
                documentsBody.InnerHtml = ClientsInformations.ToString();
            }
        }

    }
}