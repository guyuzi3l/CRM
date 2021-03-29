using CRM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Models;
using Npgsql;

namespace CRM
{
    public partial class upload_docu : Classes.Exceptions
    {
        protected ToastrUtilities toastrUtilities = new ToastrUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Classes.Cookie.GetCookie("ggZurkVKwLIM+SQ2NMcfsra8/nnrhm9u5sl4TMYTE2Y", false);
            var roles = Utilities.AccountUtilities.getUserRoles(username);
            RoleUtilities roleUtilities = new RoleUtilities();
            List<RolesModel> rolesModels = new List<RolesModel>();
            List<string> roleLists = roles.Split(',').ToList();

            if (roleLists.Find(f => f.ToLower() == "16") == null)
            {
                Response.Redirect("/dashboard.aspx");
            }

            if (Request.HttpMethod == "POST") 
            {
                if (Request.Files["documentUpload"].ContentLength > 0)
                {
                    if (Request.Files["documentUpload"].ContentLength > 10485760)
                    {
                        toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Please Upload a File Below 5MB"));
                    }
                    else 
                    {
                        var file = Request.Files[0];

                        using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc")) 
                        {

                            #region Validate if Client Id is Valid
                            var query = @"SELECT id FROM clients WHERE id=@id";
                            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@id", int.Parse(Request.Form["clientId"]));
                            var dt = Classes.DB.Select(conn, query, cmd);
                            if (dt.Rows.Count < 0 || dt == null) 
                            {
                                toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Client is not existing, please check the correctness of client id."));
                                return;
                            }
                            #endregion

                            #region Check if file can convert to base64 docs
                            var base64doc = string.Empty;
                            try
                            {
                                base64doc = GeneralUtilities.ConvertToBase64(Request.Files["documentUpload"]);
                            }
                            catch
                            {
                                base64doc = string.Empty;
                            }
                            #endregion

                            #region Upload the file to Aws Bucket
                            string awsBaseUrl = $"https://instbtcdocs.s3.eu-central-1.amazonaws.com/";
                            var DocFileName = Classes.BrandsMaster.Utilities.AmazonS3Utility.S3UploadFile("instbtcdocs", file, "AKIAXCLOKQTQ4SHCAS64", "Sdx7hnFx2U1D7WUv+mDSoZjvaj3+/MDD8K98fJOe");
                            #endregion

                            #region Build the Document Model
                            DocumentModel document = new DocumentModel
                            {
                                ClientId = int.Parse(Request.Form["clientId"]),
                                Type = Request.Form["documentType"],
                                Base64Doc = base64doc,
                                Status = "Initial",
                                CreatedDate = DateTime.UtcNow,
                                SubType = GeneralUtilities.FixInvalidChars(Request.Form["documentSubtype"]),
                                aws_file = $"{awsBaseUrl}{DocFileName}"
                            };
                            #endregion

                            var res = InsertDocument(conn, document);

                            if (res == "Success")
                            {
                                toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("success", $"Document Upload Successfully."));
                            }
                            else 
                            {
                                toastrUtilities.SessionPush("toast", new KeyValuePair<string, string>("error", $"Problem with uploading document"));
                                return;
                            }
                        }

                    }
                }
            }
        }

        public static string InsertDocument(NpgsqlConnection conn, DocumentModel document)
        {
            string query = "INSERT INTO documents (id, client_id, type, status, base64doc, created_date, updated_date, expiry_date, subtype,aws_file) VALUES "
                         + "(default, @ClientId, @Type, @Status, @Base64Doc, @CreatedDate, @MinValue, @MinValue, @Subtype,@aws_file)";
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClientId", document.ClientId);
                cmd.Parameters.AddWithValue("@Type", document.Type);
                cmd.Parameters.AddWithValue("@Status", document.Status);
                cmd.Parameters.AddWithValue("@Base64Doc", document.Base64Doc);
                cmd.Parameters.AddWithValue("@CreatedDate", document.CreatedDate);
                cmd.Parameters.AddWithValue("@MinValue", DateTime.MinValue);
                cmd.Parameters.AddWithValue("@Subtype", document.SubType);
                cmd.Parameters.AddWithValue("@aws_file", document.aws_file);
                Classes.DB.Insert(conn, query, cmd);

                return "Success";
            }
            catch (Exception ex)
            {
                return $"Error - {ex.Message} - {ex.StackTrace}";
            }
        }
    }
}