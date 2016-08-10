using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BOATV;
using NetLifeMobile.Controls.Home;

namespace NetLifeMobile
{
    public partial class Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //
           

            DataTable tbl = BOCategory.GetCategoryByParent(0, false);
            if (tbl.Rows.Count > 0)
            {
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var ctr = (Categorys) LoadControl("~/Controls/Home/Categorys.ascx");
                    if (ctr != null)
                    {
                        ctr.Cat_ID = Convert.ToInt32(tbl.Rows[i]["Cat_ID"]);
                        this.pnControl.Controls.Add(ctr);   
                    }
                    
                }
            }
        }
    }
}