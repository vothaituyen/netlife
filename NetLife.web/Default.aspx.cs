using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BOATV;
using DALATV;

namespace NetLife.web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["UpdateSapo"] != null)
            {
                var data = new DataTable();
                using (MainDB db = new MainDB())
                {
                    data = db.SelectQuery("Select Top 100 ContentID, Description from Contents Where DistributionID is null");
                }
                foreach (DataRow row in data.Rows)
                {
                    long newsId = Convert.ToInt64(row["ContentID"]);
                    string News_InitContent = row["Description"].ToString();
                    using (MainDB db = new MainDB())
                    {
                        
                            db.UpdateQuery(
                                "Update Contents set Description=@Description, DistributionID = 0 Where ContentID=@ContentID;",
                                new object[] { HttpUtility.HtmlDecode(Utils.RemoveHTMLTag(News_InitContent)).Trim(), newsId }, new[] { "Description", "ContentID" });
                    }
                }
                Response.Write("<script>location.href = location.href;</script>");
            }
        }
    }
}