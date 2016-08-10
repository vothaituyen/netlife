using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using ATVEntity;

namespace NetLife.web.Controls.Lists
{
    public partial class Hot : System.Web.UI.UserControl
    {
        private string lstNews = "<li class=\"row\"> <div class=\"img-it imghot\">{0}</div> <p class=\"txthot\"> <a title=\"{2}\" href=\"{1}\">{2}</a></p></li>";

        private int TinNong = 3;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<NewsPublishEntity> lstNew = BOATV.NewsPublished.NP_Select_Tin_Tieu_Diem(Lib.QueryString.CategoryID, Lib.QueryString.CategoryID, 5, 75);
            if (lstNew != null && lstNew.Count > 0)
            {
                for (int i = 0; i < lstNew.Count; i++)
                {
                    Literal1.Text += String.Format(lstNews, lstNew[i].URL_IMG, lstNew[i].URL, lstNew[i].NEWS_TITLE);
                }
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}