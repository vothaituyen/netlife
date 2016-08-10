using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using BOATV;

namespace NetLifeMobile.Controls.Common
{
    public partial class Header : System.Web.UI.UserControl
    {
        private int _cat_id = 0;
        public int Cat_ID { set { _cat_id = value; } get { return _cat_id; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable tbl = BOCategory.GetCategoryByParent(0, false);
            if (tbl.Rows.Count > 0)
            {
                rptCategory.DataSource = tbl;
                rptCategory.DataBind();
            }

        }
        protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var row = ((DataRowView)e.Item.DataItem);
                int catId = Convert.ToInt32(row["Cat_ID"]);
                DataTable categoryByParentId = BOCategory.GetCategoryByParent(catId, false);
                var rpt = (Repeater)e.Item.FindControl("rptSubCategory");
                if (rpt != null && categoryByParentId != null)
                {
                    rpt.DataSource = categoryByParentId;
                    rpt.DataBind();
                }
            }
        }
    }
}