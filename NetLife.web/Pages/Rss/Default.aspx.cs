using System;
using System.Collections.Generic;

using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using BOATV;

namespace NetLife.Web.Rss
{
    public class Default : Page
    {
        // Fields
        protected Repeater rptCategory;

        // Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                DataTable categoryByParentId = new DataTable();
                categoryByParentId = BOCategory.GetCategoryByParent(0, false);
                this.rptCategory.DataSource = categoryByParentId;
                this.rptCategory.DataBind();
            }
        }
    }


}