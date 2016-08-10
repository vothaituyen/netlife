using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using BOATV;

namespace NetLife.web.Controls.Common
{
    public partial class Menu : System.Web.UI.UserControl
    {
        private int _cat_id = 0;
        public int Cat_ID { set { _cat_id = value; } get { return _cat_id; } }
        public static string childCat = "<li id=\"li{2}\"><a href=\"{3}/{0}.html\" title=\"{1}\">{1}</a></li>";
        protected void Page_Load(object sender, EventArgs e)
        {
            var tbl = BOCategory.GetCategoryByParent(0,false);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in tbl.Rows)
                {
                    //Literal1.Text += String.Format(childCat, row["Cat_DisplayUrl"].ToString().Trim().ToLower(), row["Cat_Name"].ToString(), row["Cat_ID"].ToString(), row["Cat_ID"].ToString().Equals("78") ? "http://clip.netlife.com.vn" : "http://netlife.com.vn");
                    Literal1.Text += String.Format(childCat, row["Cat_DisplayUrl"].ToString().Trim().ToLower(), row["Cat_Name"].ToString(), row["Cat_ID"].ToString(), "");
                }
            }

        }
    }
}