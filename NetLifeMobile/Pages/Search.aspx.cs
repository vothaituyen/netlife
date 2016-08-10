using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using ATVEntity;
using BOATV;
using SortOrder = System.Data.SqlClient.SortOrder;

namespace NetLifeMobile.Pages
{
    public partial class Search : System.Web.UI.Page
    {
        public string query = string.Empty;
        protected bool isSearchTags = false;
        private int pageCount;
        private int pageSize = 20;
        protected void Page_Load(object sender, EventArgs e)
        {
            isSearchTags = Request.QueryString["isSearchTags"] != null;
            string key = Request.QueryString["key"];
            key = key.Replace(" ", "+").Replace("@", "+").Replace("/", "+").Replace("\\", "+").Replace("!", "+");
            if (!IsPostBack)
            {
                if (!String.IsNullOrWhiteSpace(key))
                {
                    if (key.Length > 0)
                    {
                        DataTable result = BOATV.NewsPublished.SearchFulltext(key, pageSize, Lib.QueryString.PageIndex, 213, 0);

                        int newsCount = 0;
                        newsCount = NewsPublished.SearchResulCount(key, 15, 0);
                        if (result != null && result.Rows.Count > 0)
                        {
                            rptData.DataSource = result;
                            rptData.DataBind();

                            pageCount = ((newsCount) - 1) / pageSize + 1;
                            Paging1.TotalPage = pageCount;
                            Paging1.IsSearch = true;
                            Paging1.DoPagging(Lib.QueryString.PageIndex);
                            Paging1.HidePagging(false);
                        }
                        ltrAlert.Text = "Có <span style=\"color:red\">" + newsCount + "</span> kết quả phù hợp với từ khóa \"" + key.Replace("+", " ") + "\"";
                    }
                }
            }
            Utils.SetPageHeader(this.Page, key.Replace("+", " ") + " | Kết quả trang " + Lib.QueryString.PageIndex, key, key);
        }
    }
}