using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using BOATV;

namespace NetLife.web.Pages
{
    public partial class List : System.Web.UI.Page
    {
        //int CatID = Lib.QueryString.ParentCategoryID == 0
        //                ? Lib.QueryString.CategoryID
        //                : Lib.QueryString.ParentCategoryID;
        int CatID = Lib.QueryString.CategoryID;
        public static string childCat = "<li id=\"li{2}\"><a href=\"{0}\" title=\"{1}\">{1}</a></li>";
        protected void Page_Load(object sender, EventArgs e)
        {
            var tbl = BOCategory.GetCategoryByParent(CatID);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in tbl.Rows)
                {
                    Literal1.Text += String.Format(childCat, (String.Format("/{0}.html", row["Cat_DisplayUrl"].ToString())), row["Cat_Name"].ToString(),row["Cat_ID"]);
                }
            }

        }
    }
}