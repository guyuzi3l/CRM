using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CRM.Models;

namespace CRM.Utilities
{
    public class Documents
    {
        public int Id { get; set; }
        public decimal ClientId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SubType { get; set; }
        public string Note { get; set; }
        public string Fullname { get; set; }
        public string CardLastFour { get;set; }

        public string Aws_File { get; set; }

        public static List<Documents> GetDocuments(NpgsqlConnection conn, int offset = 0, int limit = 0, string orderby = "")
        {
            List<Documents> docs = new List<Documents>();

            string _limit = "ALL";

            if (limit > 0)
                _limit = limit.ToString();

            if (!string.IsNullOrEmpty(orderby))
                orderby = $"ORDER BY {orderby}";

            //string query = String.Format("SELECT id,client_id,type,status,created_date,updated_date,expiry_date,subtype,note FROM documents OFFSET {0} LIMIT {1}",offset,limit);
            string query = String.Format("SELECT dc.id,dc.client_id,cl.first_name::text || ' ' || cl.last_name::text AS fullname,dc.type,dc.status,dc.created_date,dc.updated_date,dc.expiry_date,dc.subtype,dc.note,dc.aws_file,dc.card_last_four FROM documents dc INNER JOIN clients cl on dc.client_id = cl.id {2} OFFSET {0} LIMIT {1}", offset, _limit,orderby);
            using (DataTable dt = Classes.DB.Select(conn, query))
            {
                if (dt.Rows.Count > 0)
                {
                    docs = dt.AsEnumerable().Select(x => new Documents()
                    {
                        Id = x.Field<int>("id"),
                        ClientId = x.Field<decimal>("client_id"),
                        Fullname = x.Field<string>("fullname"),
                        Type = x.Field<string>("type"),
                        Status = x.Field<string>("status"),
                        CreatedDate = x.Field<DateTime>("created_date"),
                        UpdateDate = x.Field<DateTime>("updated_date"),
                        ExpiryDate = x.Field<DateTime>("expiry_date"),
                        SubType = x.Field<string>("subtype"),
                        Note = x.Field<string>("note"),
                        Aws_File = !string.IsNullOrEmpty(x.Field<string>("aws_file")) ? x.Field<string>("aws_file") : "",
                        CardLastFour = !string.IsNullOrEmpty(x.Field<string>("card_last_four")) ? x.Field<string>("card_last_four") : ""
                    }).ToList();
                }
            }
            return docs;
        }

        public static List<Documents> GetDocuments(NpgsqlConnection conn, string CustomParameters, int offset = 0, int limit = 0, string orderby = "")
        {
            List<Documents> docs = new List<Documents>();
            string _limit = "ALL";

            if (limit > 0)
                _limit = limit.ToString();

            if (!string.IsNullOrEmpty(orderby))
                orderby = $"ORDER BY {orderby}";

            string query = String.Format($"SELECT dc.id,dc.client_id,cl.first_name::text || ' ' || cl.last_name::text AS fullname,dc.type,dc.status,dc.created_date,dc.updated_date,dc.expiry_date,dc.subtype,dc.note,dc.aws_file,dc.card_last_four FROM documents dc INNER JOIN clients cl on dc.client_id = cl.id WHERE dc.id IS NOT NULL " + CustomParameters + " {2} OFFSET {0} LIMIT {1}", offset, _limit,orderby);

            using (DataTable dt = Classes.DB.Select(conn, query))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    docs = dt.AsEnumerable().Select(x => new Documents()
                    {
                        Id = x.Field<int>("id"),
                        ClientId = x.Field<decimal>("client_id"),
                        Fullname = x.Field<string>("fullname"),
                        Type = x.Field<string>("type"),
                        Status = x.Field<string>("status"),
                        CreatedDate = x.Field<DateTime>("created_date"),
                        UpdateDate = x.Field<DateTime>("updated_date"),
                        ExpiryDate = x.Field<DateTime>("expiry_date"),
                        SubType = x.Field<string>("subtype"),
                        Note = x.Field<string>("note"),
                        Aws_File = !string.IsNullOrEmpty(x.Field<string>("aws_file")) ? x.Field<string>("aws_file") : "",
                        CardLastFour = !string.IsNullOrEmpty(x.Field<string>("card_last_four")) ? x.Field<string>("card_last_four") : ""
                    }).ToList();
                }
            }
            return docs;
        }

        public static string DocumentsCount(NpgsqlConnection conn)
        {
            string query = "SELECT COUNT (*) FROM documents";
            var getDocs = Classes.DB.SelectScalar(conn, query);
            string TotalgetDocs = getDocs != null ? getDocs.ToString() : "0";
            return TotalgetDocs;
        }

        public static string GetDocumentStatus(NpgsqlConnection conn, string id)
        {
            string query = $"SELECT status FROM documents WHERE id=@Id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            var getDocs = Classes.DB.SelectScalar(conn, query, cmd);
            string docStatus = getDocs != null ? getDocs.ToString() : "N/A";
            return docStatus;
        }

        public static string DocumentsCount(NpgsqlConnection conn, string CustomParameters)
        {
            string query = "SELECT COUNT (*) FROM documents dc WHERE id IS NOT NULL" + CustomParameters;
            var getDocs = Classes.DB.SelectScalar(conn, query);
            string TotalgetDocs = getDocs != null ? getDocs.ToString() : "0";
            return TotalgetDocs;
        }

        public static void UpdateDocStatus(NpgsqlConnection conn, string status, string id, string expiryDate,string cardLast4)
        {
            string query = $"UPDATE documents SET status = @Status, expiry_date=COALESCE(@expiry_date,expiry_date),card_last_Four=COALESCE(@card_last_Four,card_last_Four), updated_date = @updated_date WHERE id=@Id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            cmd.Parameters.AddWithValue("@card_last_Four", !string.IsNullOrEmpty(cardLast4) ? cardLast4 : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@expiry_date", !string.IsNullOrWhiteSpace(expiryDate) ? DateTime.Parse(expiryDate) : (object)DBNull.Value);
            Classes.DB.Update(conn, query, cmd);
        }

        public static void UpdateTypeAndSubtype(NpgsqlConnection conn, string type, string subtype, string id)
        {
            string query = $"UPDATE documents SET type = @Type, subtype = @SubType WHERE id=@Id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@SubType", subtype);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            Classes.DB.Update(conn, query, cmd);
        }

        public static List<DocumentModel> VerifiedDocument(NpgsqlConnection conn, string ClientId)
        {
            List<DocumentModel> docs = new List<DocumentModel>();
            string query = $"SELECT DISTINCT type as distincttype, * FROM documents WHERE ((type='Proof of ID' AND status = 'Approved') OR ( status ='Approved' AND type='Proof of Residence')) AND client_id=@ClientId UNION SELECT DISTINCT subtype, * FROM documents WHERE ((status = 'Approved' AND type ='Credit Card' AND subtype='Front') OR (status = 'Approved' AND type='Credit Card' AND subtype='Back')) AND client_id=@ClientId";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClientId", Convert.ToInt32(ClientId));
            using (DataTable dt = Classes.DB.Select(conn, query, cmd))
            {
                if (dt.Rows.Count > 0)
                {
                    docs = dt.AsEnumerable().Select(x => new DocumentModel()
                    {
                        Id = x.Field<int>("id"),
                        ClientId = x.Field<decimal>("client_id"),
                        Type = x.Field<string>("type"),
                        Status = x.Field<string>("status"),
                        CreatedDate = x.Field<DateTime>("created_date"),
                        UpdatedDate = x.Field<DateTime>("updated_date"),
                        ExpiryDate = x.Field<DateTime>("expiry_date"),
                        SubType = x.Field<string>("subtype"),
                        Note = x.Field<string>("note"),
                    }).ToList();
                }
            }
            return docs;
        }

        public static DocumentModel GetDocument(NpgsqlConnection conn, string Id)
        {
            DocumentModel docs = new DocumentModel();
            string query = "SELECT * FROM documents WHERE id=@Id";

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(Id));
            using (var reader = Classes.DB.ExecuteReader(conn, query, cmd))
            {
                if (reader.Read())
                {
                    docs.Id = !string.IsNullOrEmpty(reader["id"].ToString()) ? Convert.ToInt32(reader["id"]) : 0;
                    docs.Base64Doc = reader["base64doc"].ToString();
                    docs.ClientId = !string.IsNullOrEmpty(reader["client_id"].ToString()) ? Convert.ToDecimal(reader["client_id"]) : 0;
                    docs.Type = reader["type"].ToString();
                    docs.Status = reader["status"].ToString();
                    docs.CreatedDate = !string.IsNullOrEmpty(reader["created_date"].ToString()) ? Convert.ToDateTime(reader["created_date"]) : DateTime.MinValue;
                    docs.UpdatedDate = !string.IsNullOrEmpty(reader["updated_date"].ToString()) ? Convert.ToDateTime(reader["updated_date"]) : DateTime.MinValue;
                    docs.ExpiryDate = !string.IsNullOrEmpty(reader["expiry_date"].ToString()) ? Convert.ToDateTime(reader["expiry_date"]) : DateTime.MinValue;
                    docs.SubType = reader["subtype"].ToString();
                    docs.Note = reader["note"].ToString();
                    docs.aws_file = !string.IsNullOrEmpty(reader["aws_file"].ToString()) ? reader["aws_file"].ToString() : "";
                }
            }
            return docs;
        }

        public static bool CheckVerifiedDocuments(List<DocumentModel> ListOfDocuments)
        {
            List<DocumentModel> ListOfDocumentsResult = new List<DocumentModel>();
            int countDocs = 0;
            //Check if there's an Approved Proof of Residence
            ListOfDocumentsResult = ListOfDocuments.Where(x => x.Status == "Approved" && x.Type == "Proof of Residence").ToList();
            if (ListOfDocumentsResult.Count > 0)
            {
                countDocs++;
            }

            //Check if there's an Approved Proof of Id
            ListOfDocumentsResult = ListOfDocuments.Where(x => x.Status == "Approved" && x.Type == "Proof of ID").ToList();
            if (ListOfDocumentsResult.Count > 0)
            {
                countDocs++;
            }

            //Check if there's an Approved Credit Card Front
            ListOfDocumentsResult = ListOfDocuments.Where(x => x.Status == "Approved" && x.Type == "Credit Card" && x.SubType == "Front").ToList();
            if (ListOfDocumentsResult.Count > 0)
            {
                countDocs++;
            }

            //Check if there's an Approved Credit Card Back
            ListOfDocumentsResult = ListOfDocuments.Where(x => x.Status == "Approved" && x.Type == "Credit Card" && x.SubType == "Back").ToList();
            if (ListOfDocumentsResult.Count > 0)
            {
                countDocs++;
            }

            if (countDocs == 4)
            {
                return true;
            }
            return false;
        }

        public static bool CheckVerifiedCard(List<DocumentModel> ListOfDocuments)
        {
            List<DocumentModel> ListOfDocumentsResult = new List<DocumentModel>();
            int countDocs = 0;

            //Check if there's an Approved Credit Card Front
            ListOfDocumentsResult = ListOfDocuments.Where(x => x.Status == "Approved" && x.Type == "Credit Card" && x.SubType == "Front").ToList();
            if (ListOfDocumentsResult.Count > 0)
            {
                countDocs++;
            }

            //Check if there's an Approved Credit Card Back
            ListOfDocumentsResult = ListOfDocuments.Where(x => x.Status == "Approved" && x.Type == "Credit Card" && x.SubType == "Back").ToList();
            if (ListOfDocumentsResult.Count > 0)
            {
                countDocs++;
            }

            if (countDocs == 2)
            {
                return true;
            }
            return false;
        }

        //Check verified documents without credit card
        public static bool CheckVerifiedDocumentsNoCard(List<DocumentModel> ListOfDocuments)
        {
            List<DocumentModel> ListOfDocumentsResult = new List<DocumentModel>();
            int countDocs = 0;
            //Check if there's an Approved Proof of Residence
            ListOfDocumentsResult = ListOfDocuments.Where(x => x.Status == "Approved" && x.Type == "Proof of Residence").ToList();
            if (ListOfDocumentsResult.Count > 0)
            {
                countDocs++;
            }

            //Check if there's an Approved Proof of Id
            ListOfDocumentsResult = ListOfDocuments.Where(x => x.Status == "Approved" && x.Type == "Proof of ID").ToList();
            if (ListOfDocumentsResult.Count > 0)
            {
                countDocs++;
            }

            if (countDocs == 2)
            {
                return true;
            }
            return false;
        }

        public string GetImageById(NpgsqlConnection conn, string id)
        {
            string query = "SELECT base64doc FROM documents WHERE id=@Id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            var verifyImage = Classes.DB.SelectScalar(conn, query, cmd);
            string base64image = verifyImage != null ? verifyImage.ToString() : "";
            return base64image;
        }
    }
}