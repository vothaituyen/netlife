using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVEntity;
using BOATV;

namespace NetLife.web.Controls.Home
{
    public partial class Fashion : System.Web.UI.UserControl
    {
        private string lstNews = "<li class=\"row\"> <div class=\"img-it imghot\">{0}</div> <p class=\"txthot\"> <a title=\"{2}\" href=\"{1}\">{2}</a></p></li>";

        //private int TinNong = 3;
        protected void Page_Load(object sender, EventArgs e)
        {
            //List<NewsPublishEntity> lstNew = BOATV.NewsPublished.NP_Select_Tin_Tieu_Diem(0, 0, 5, 75);
            List<NewsPublishEntity> lstNew = BOATV.NewsPublished.NP_Select_Tin_Tieu_Diem(0, 0, 5, 75);
            if (lstNew.Count > 0)
            {
                for (int i = 0; i < lstNew.Count; i++)
                {
                    lstNew[i].NEWS_TITLE = lstNew[i].NEWS_TITLE.ToString().Substring(0, (lstNew[i].NEWS_TITLE.ToString().Length < 60 ? lstNew[i].NEWS_TITLE.ToString().Length : 60)) + (lstNew[i].NEWS_TITLE.ToString().Length < 60 ? "" : "...");
                    Literal1.Text += String.Format(lstNews, lstNew[i].URL_IMG, lstNew[i].URL, lstNew[i].NEWS_TITLE);
                }
            }
        }
    }
}