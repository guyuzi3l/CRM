using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CRM.Models;
using System.Web.Services;

namespace CRM
{
    public partial class conversion_request : System.Web.UI.Page
    {
        protected string AddedQuery { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ConversionReqModel> ConversionRequest = new List<ConversionReqModel>();
            using (var con =Classes.DB.InstBTCDB("instbtc"))
            {

                if (Request.HttpMethod == "POST")
                {
                    #region Build Additional Query for Search
                    string client_id_query = !string.IsNullOrEmpty(txtClientId.Value) ? $"AND cv.client_id={txtClientId.Value} " : string.Empty;
                    string email_query = !string.IsNullOrEmpty(txtClientEmail.Value) ? $"AND cl.email='{txtClientEmail.Value}' " : string.Empty;
                    string created_date_query = !string.IsNullOrEmpty(txtCreatedDate.Value) ? $"AND cv.created_date::date='{DateTime.Parse(txtCreatedDate.Value)}' " : string.Empty;
                    string credited_status_query = !string.IsNullOrEmpty(txtStatus.Value) ? $"AND cv.credited_status='{txtStatus.Value}' " : string.Empty;
                    string client_name_query = !string.IsNullOrEmpty(txtClientName.Value) ? $"AND cv.client_name LIKE '%{txtClientName.Value}%' " : string.Empty;

                    AddedQuery = string.Concat(client_id_query, email_query, created_date_query, credited_status_query, client_name_query);
                    #endregion
                }

                #region Data Filling and Pagination Handling
                Dictionary<string, int> pagerDictionary = new Dictionary<string, int>();
                string paginationString = "page";
                DocListPager.initialize(paginationString, 10);
                pagerDictionary = DocListPager.paginate(Utilities.ConversionReqUtilities.GetConversionRequest(con, AddedQuery).Count().ToString());
                ConversionRequest = Utilities.ConversionReqUtilities.GetConversionRequest(con, AddedQuery, pagerDictionary["offset"], pagerDictionary["limit"]);
                #endregion
            }
           
            rowCount.InnerHtml = $"{ConversionRequest.Count().ToString()} Records Found. .";
            resultBody.InnerHtml = BuildHtmlTable(ConversionRequest);
        }

        private string BuildHtmlTable(List<ConversionReqModel> Model)
        {
            StringBuilder Body = new StringBuilder();
            foreach (var i in Model)
            {
                Body.Append("<tr>")
                    .Append("<td>")
                    .Append("<div class='dropdown'>")
                    .Append("<button class='btn btn-sm btn-success dropdown-toggle' type='button' id='dropdownMenu1' data-toggle='dropdown' aria-haspopup='true' aria-expanded='true'>")
                    .Append("<i class='fa fa-pencil'></i></button>")
                    .Append("<ul class='dropdown-menu' aria-labelledby='dropdownMenu1'>");

                if (i.credited_status.ToLower() != "approved")
                {
                    Body.AppendFormat("<li><a href='#!' class='class-change-status' data-document-id='{0}'>Change Status</a></li>", i.id);
                }
                else
                {
                    Body.Append("<li><a href='#'>Already Approved.</a></li>");
                }
                
                Body.Append("</ul>")
                    .Append("</div>")
                    .Append("</td>")
                    .AppendFormat("<td>{0}</td>",i.id)
                    .AppendFormat("<td>{0}</td>",i.client_id)
                    .AppendFormat("<td>{0}</td>",i.client_name)
                    .AppendFormat("<td>{0}</td>",i.amount)
                    .AppendFormat("<td>{0}</td>", i.currency)
                    .AppendFormat("<td>{0}</td>", i.credited_status)
                    .AppendFormat("<td>{0}</td>", i.conversion_amount)
                    .AppendFormat("<td>{0}</td>", i.conversion_currency)
                    .AppendFormat("<td>{0}</td>", i.created_date)
                    .AppendFormat("<td>{0}</td>", i.btc_balance)
                    .AppendFormat("<td>{0}</td>", i.eur_balance)
                    .Append("</tr>");
            }
            return Body.ToString();
        }

        [WebMethod]
        public static string UpdateConversionRequest(string id,string selectedStatus)
        {
            string result = string.Empty;
            try
            {
                result =  Utilities.ConversionReqUtilities.ProcessConversionRequest(id, selectedStatus);
            }
            catch(Exception ex)
            {
                result = ex.Message.ToString();
            }
            return result;
        }

    }
}