using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NetLifeMobile.Controls.Home
{
    public partial class News : System.Web.UI.UserControl
    {
        private string news = "<li class=\"news_home\"><div class=\"row\"><span class=\"col-xs-2 col-sm-1 col-md-1\" ><a href=\"{1}\"><img src=\"{0}\" alt=\"\" height=\"50\" width=\"60\"></span><span class=\"col-xs-10 title\" style=\"padding-left:20px;\">{2}</a></span></div></li>";
        private int top = 7;
        protected void Page_Load(object sender, EventArgs e)
        {
            //var tinmoi = BOATV.NewsPublished.NP_Tin_Moi_Trong_Ngay(5, 0);
            var tinmoi = BOATV.NewsPublished.NP_Tin_Nong(0, 3, top, 0);
            if (tinmoi.Count > 0)
            {
                for (int i = 0; i < tinmoi.Count; i++)
                {
                    ltrNews.Text += String.Format(news, tinmoi[i].Imgage.ImageUrl, tinmoi[i].URL, tinmoi[i].NEWS_TITLE);
                }
            }
        }
    }
}