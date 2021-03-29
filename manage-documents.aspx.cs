using CRM.Utilities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Models;
using System.Web.Services;

namespace CRM
{
    public partial class manage_documents : System.Web.UI.Page
    {
        protected string docid;
        protected string aws_file { get; set; }
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string client_id = Request.QueryString["client_id"];
            if (id != null && id != "")
            {
                docid = Classes.encryption.Encryption(id);
                using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
                {
                    //Get Document Informations
                    DocumentModel document = new DocumentModel();
                    document = Documents.GetDocument(conn, id);
                    Decimal clientId = document.ClientId;
                    aws_file = !string.IsNullOrEmpty(document.aws_file) ? document.aws_file : "#";
                    string base64image = document.Base64Doc;

                    //Get Client Informations
                    ClientModel clients = new ClientModel();
                    clients = Clients.GetClientsByDocId(conn, id);

                    //Print Base 64 Image on Front
                    imageContainer.InnerHtml = "<img id='imageEncoded' style='display:block; width:auto; height:500px; margin-left: auto; margin-bottom: 10px; box-shadow: 0px 0px 11px 1px #888888; cursor: pointer; margin-right: auto;' src='data:image/jpeg;base64," + base64image + "' onclick='openNewTab();'/>";
                    clientbtn.InnerHtml = "<a id='btnDocs' type='button' class='btn btn-info margin-bottom-10 margin-top-20 btn-sm' href='/client-detail.aspx?id=" + clientId + "'>Client Information</a>";
                    if (Request.HttpMethod == "POST")
                    {
                        string status = Request.Form["selectedStatus"];
                        string expiryDate = Request.Form["expiryDate"];
                        string cardLast4 = Request.Form["cardLast4"];
                        updateDocument(conn, id, status, expiryDate, cardLast4);

                        //Send GetDocumentDeclinedTemplate on Email if Rejected
                        if (status == "Rejected")
                        {
                            var message = EmailTemplateUtilities.GetDocumentDeclinedTemplate(clients, status);
                            EmailTemplateUtilities.SendEmail(clients.Email, message, "Get-Bitcoin Documents Declined");
                        }

                        //Send Email on Client If the Required Documents are all approved
                        List<DocumentModel> ListOfDocuments = new List<DocumentModel>();
                        ListOfDocuments = Documents.VerifiedDocument(conn, document.ClientId.ToString());
                        bool verified = Documents.CheckVerifiedDocumentsNoCard(ListOfDocuments);
                        if (verified)
                        {
                            if (document.Type == "Credit Card")
                            {
                                bool cardVerified = Documents.CheckVerifiedCard(ListOfDocuments);
                                if (cardVerified)
                                {
                                    //Send Email To Client
                                    var message = EmailTemplateUtilities.GetApproveStatusTemplate(clients, status);
                                    EmailTemplateUtilities.SendEmail(clients.Email, message, "Get-Bitcoin Documents Approved");
                                }
                            }
                            else
                            {
                                //Send Email To Client
                                var message = EmailTemplateUtilities.GetApproveStatusTemplate(clients, status);
                                EmailTemplateUtilities.SendEmail(clients.Email, message, "Get-Bitcoin Documents Approved");
                            }
                        }
                        currStatus.InnerText = document.Status.ToUpper();
                        currType.InnerText = document.Type.ToUpper();
                        currSubtype.InnerText = document.SubType.ToUpper();
                    }
                    else
                    {
                        currStatus.InnerText = document.Status.ToUpper();
                        currType.InnerText = document.Type.ToUpper();
                        currSubtype.InnerText = document.SubType.ToUpper();
                    }
                }
            }
            else
            {
                Response.Redirect("/document-lists.aspx");
            }
        }

        private void updateDocument(NpgsqlConnection conn, string id, string status , string expiryDate, string cardLast4)
        {
            ClientModel clients = new ClientModel();
            clients = Clients.GetClientsByDocId(conn, id);
            Documents.UpdateDocStatus(conn, status, id, expiryDate, cardLast4);
        }

        [WebMethod]
        public static DocumentModel ChangeType(string Type, string SubType)
        {
            DocumentModel document = new DocumentModel();
            string decryptedId = string.Empty;
            string id = HttpContext.Current.Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    decryptedId = Classes.encryption.Decrypt(id);
                    document = new DocumentModel()
                    {
                        Id = Convert.ToInt32(decryptedId),
                        Type = Type,
                        SubType = SubType
                    };

                    using (NpgsqlConnection conn =Classes.DB.InstBTCDB("instbtc"))
                    {
                        Documents.UpdateTypeAndSubtype(conn, Type, SubType, decryptedId);
                    }
                }
                catch
                {

                }
            }
            return document;
        }
    }
}