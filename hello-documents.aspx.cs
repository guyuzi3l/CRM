using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Utilities;
using CRM.Models;
using HelloSign;
using Npgsql;
using static CRM.Models.WithdrawalModel;
using Classes.Instbtc.Models;

namespace CRM
{
    public partial class hello_documents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
            {
                //Get Client Information From Query String
                string DocumentId = Request.QueryString["doc_id"];
                string ClientName = Request.QueryString["name"];
                string dateTimeNow = DateTime.UtcNow.ToString("yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

                //Make the Initials Extractions
                string clientInitials = AccountUtilities.ExtracInitials(ClientName);

                //Fetch File From Hello Sign Api
                Client client = new Client("bbb7087eb89dd0c11f91cf037366f6e85eca26b38f84ea7a80a2566e66f52e8d");
                byte[] downloadDocument = client.DownloadSignatureRequestFiles(DocumentId, SignatureRequest.FileType.PDF);

                //Transmit File to client
                Response.Clear();
                Response.AppendHeader("Content-Disposition", $"attachment; filename={dateTimeNow}{clientInitials}{DocumentId}.pdf");
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(downloadDocument);
                Response.End();
            }
        }
    }
}