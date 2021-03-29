using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM
{
    public partial class PaginationUtil : System.Web.UI.UserControl
    {

        protected struct PaginationPage
        {
            public int Value {get; set;}
            public string CssClass { get; set; }
            public string Url { get; set; }
        }

        public int ItemCountPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        protected List<PaginationPage> Pages { get; set; }
        public string ActiveCssClass { get; set; }
        public string QueryKey { get; set; }
        public int ItemsRight { get; set; }
        public int ItemsLeft { get; set; }
        public string FirstPageUrl { get; set; }
        public string LastPageUrl { get; set; }

        public string ExistedQuery { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.TotalPages = (int)Math.Ceiling((double)this.TotalItems / this.ItemCountPerPage);
            this.Pages = new List<PaginationPage>();
            this.ItemsRight = 3;
            this.ItemsLeft = 3;

            bool showFirstLink = this.TotalPages > 0 ? true : false;
            bool showLastLink = this.TotalPages > 0 ? true : false; 

            if (this.CurrentPage >  this.TotalPages)
            {
                showFirstLink = false;
                showLastLink = false;
            }

            this.AppendExistedQuery();

            if (this.CurrentPage == 0)
                this.CurrentPage = 1;

            if((int)this.CurrentPage <= this.ItemsLeft)
                this.ItemsRight += this.ItemsLeft - (int)this.CurrentPage;

            for (int i = 1; i <= this.TotalPages; i++)
            {
                PaginationPage p = new PaginationPage() { Value = i, CssClass = "", Url = Request.Url.AbsolutePath + "?"+ this.ExistedQuery + "&" + this.QueryKey+"="+ i };
                if (this.CurrentPage == p.Value)
                    p.CssClass += this.ActiveCssClass + " ";

                if (p.Value == (int)this.CurrentPage || (p.Value <= ((int)this.CurrentPage + this.ItemsRight) && p.Value > this.CurrentPage) || (p.Value >= ((int)this.CurrentPage - this.ItemsLeft) && p.Value < this.CurrentPage))
                {
                    this.Pages.Add(p);

                    if (p.Value == 1)
                        showFirstLink = false;

                    if (p.Value == this.TotalPages)
                        showLastLink = false;   
                }
            }

            if (showFirstLink == true)
                this.FirstPageUrl = this.GeneratePageUrl("1");

            if (showLastLink == true)
                this.LastPageUrl = this.GeneratePageUrl(this.TotalPages.ToString());

            PaginationRepeater.DataSource = this.Pages;
            PaginationRepeater.DataBind();

         
        }

        protected string GeneratePageUrl(string pageNumber)
        {
            return Request.Url.AbsolutePath + "?" + this.QueryKey + "=" + pageNumber;
        }

        public int GetRecordOffset()
        {
            return this.ItemCountPerPage * ((int)this.CurrentPage - 1);
        }

        public int ParseQueryString(string q)
        {
            if (!String.IsNullOrWhiteSpace(q))
            {
                int queryAsInt;
                bool isNumeric = int.TryParse(q, out queryAsInt);
                if (isNumeric == true)
                    return queryAsInt;
            }

            return 1;
        }

        public void initialize(string qkey, int pageSize)
        {
            this.ItemCountPerPage = pageSize;
            this.QueryKey = qkey;
        }


        public Dictionary<string,int> paginate(string total)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            this.CurrentPage = this.ParseQueryString(Request.QueryString[this.QueryKey]);
            this.TotalItems = int.Parse(total);
            int offset = this.GetRecordOffset();
            d.Add("offset", this.GetRecordOffset());
            d.Add("limit", this.ItemCountPerPage);
            return d;
        }


        public void AppendExistedQuery()
        {
            var nvc = HttpUtility.ParseQueryString(Request.Url.Query);
            nvc.Remove(this.QueryKey);
            this.ExistedQuery = nvc.ToString();
        }
    }
}