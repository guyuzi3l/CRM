using CRM.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM.Utilities
{
    public class PspUtilities
    {
        public static List<PspModel> GetPsp(NpgsqlConnection conn)
        {
            List<PspModel> psp = new List<PspModel>();
            string query = "SELECT  * FROM psp WHERE is_active=true OR name IN ('Wire Transfer','USD Wire Transfer','EUR Wire Transfer','AUD Wire Transfer')";
            DataTable dt = Classes.DB.Select(conn, query);
            if (dt.Rows.Count > 0)
            {
                psp = dt.AsEnumerable().Select(x => new PspModel() {
                    Id = x.Field<int>("id"),
                    Name = x.Field<string>("name"),
                    IsActive = x.Field<bool>("is_active"),
                    ImageLink = x.Field<string>("image_link"),
                    AllowedCountries = x.Field<string>("allowed_countries")
                }).ToList();
            }
            return psp;
        }

        public List<KeyValuePair<int, string>> GetPspOptions()
        {
            List<PspModel> psp = new List<PspModel>();
            using (var conn = Classes.DB.InstBTCDB("instbtc"))
            {
                psp = GetPsp(conn);
            };

            List<KeyValuePair<int, string>> pspOptions = new List<KeyValuePair<int, string>>();
            if (psp.Count > 0)
            {
                pspOptions = psp.Select(x => new KeyValuePair<int, string>(x.Id, x.Name)).ToList();
            }

            return pspOptions;
        }

        public static List<PspModel> GetPspById( string id)
        {
            List<PspModel> psp = new List<PspModel>();
            using (NpgsqlConnection conn = Classes.DB.InstBTCDB("instbtc"))
            {
                string query = $"SELECT * FROM psp WHERE id={id}";
                DataTable dt = Classes.DB.Select(conn, query);
                if (dt.Rows.Count > 0)
                {
                    psp = dt.AsEnumerable().Select(x => new PspModel()
                    {
                        Id = x.Field<int>("id"),
                        Name = x.Field<string>("name"),
                        IsActive = x.Field<bool>("is_active"),
                        ImageLink = x.Field<string>("image_link"),
                        AllowedCountries = x.Field<string>("allowed_countries")
                    }).ToList();
                }
            }
            return psp;
        }
    }
}