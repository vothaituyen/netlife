using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVEntity;
using BOATV;

namespace NetLife.web.Controls.Common
{
    public partial class SlideHeader : System.Web.UI.UserControl
    {
        private string lstNews = "<div class=\"slide\"><div class=\"col-md-4\"> {0} </div> <div class=\"col-md-8 ttsl\"><h4>{3}</h4><a href=\"{1}\">{2}</a></div></div>";

        private int TinNong = 4;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<NewsPublishEntity> lstNew = BOATV.NewsPublished.NP_Tin_Nong(0, TinNong, 12, 90);
            if (lstNew.Count > 0)
            {
                for (int i = 0; i < lstNew.Count; i++)
                {
                    string catname = "";
                    var a = BOCategory.GetCategory(lstNew[i].Cat_Id);
                    if (a != null)
                    {
                        catname = a.Cat_Name;
                    }
                    Literal1.Text += String.Format(lstNews, lstNew[i].URL_IMG, lstNew[i].URL, lstNew[i].NEWS_TITLE, catname);
                }
            }
        }
    }
}