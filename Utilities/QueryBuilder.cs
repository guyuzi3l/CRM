using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Utilities
{
    public class QueryBuilder
    {
        public NpgsqlConnection Connection { get; set; }
        public string SelectedTable { get; set; }
        public string QueryString { get; set; }
        public Dictionary<string, string> QueryDictionary { get; set; }

        public QueryBuilder(){}

        public QueryBuilder(string database)
        {
            this.Connection = Classes.DB.InstBTCDB(database);
            this.QueryDictionary = new Dictionary<string, string>();
            this.QueryDictionary.Add("action","");
            this.QueryDictionary.Add("table", "");
            this.QueryDictionary.Add("field", "");
            this.QueryDictionary.Add("where", "");
            this.QueryDictionary.Add("limit", "");
            this.QueryDictionary.Add("offset", "");
        }

        public QueryBuilder Table(string table)
        {
            this.QueryDictionary["table"] = table;
            return this;
        }

        public QueryBuilder Select(string f)
        {
            this.QueryDictionary["field"] = f;
            return this;
        }

        public string GetQueryString()
        {
            this.Build();
            return this.QueryString;
        }

        protected void Build()
        {
            this.QueryString = String.Format("SELECT {0} FROM {1}", this.QueryDictionary["field"], this.QueryDictionary["table"]);
        }


    }
}