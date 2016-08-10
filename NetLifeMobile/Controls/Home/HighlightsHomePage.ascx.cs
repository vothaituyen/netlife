using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVEntity;
using BOATV;

namespace NetLifeMobile.Controls.Home
{
    public partial class HighlightsHomePage : System.Web.UI.UserControl
    {
        string listitem = "<div class=\"col-xs-6 item-hot\"><div class=\"col-xs-12\">{0}</div><a href=\"{1}\"><div class=\"col-xs-12 tit-hot\">{2} </div> </a> </div>";
        private string strListOne = "<div class=\"col-xs-12\" style=\"text-align: center\">{0}</div><a href=\"{1}\"><div class=\"col-xs-12 title-nb\">{2}</div> </a>";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                var lst = BOATV.NewsPublished.GetListBonBaiNoibat(6, 460);
                if (lst != null && lst.Count > 0)
                {
                    ltrnb.Text = String.Format(strListOne, lst[0].URL_IMG, lst[0].URL, lst[0].NEWS_TITLE,
                        Utils.CatSapo(lst[0].NEWS_INITCONTENT, 40));
                }
                if (lst != null && lst.Count > 1)
                {
                    for (int i = 1; i < 5; i++)
                    {
                        lst[i].Imgage = new ImageEntity(460, lst[i].Imgage.ImageUrl);
                        if (lst[i].NEWS_TITLE.Length > 100)
                        {
                            lst[i].NEWS_TITLE = lst[i].NEWS_TITLE.Substring(0, 100) + "...";
                        }
                        if (i <= 2)
                            ltrItem.Text += String.Format(listitem, lst[i].URL_IMG, lst[i].URL, lst[i].NEWS_TITLE);
                        else
                        {
                            ltrItem1.Text += String.Format(listitem, lst[i].URL_IMG, lst[i].URL, lst[i].NEWS_TITLE);
                        }
                    }

                }
            }
            catch
            {
                
            }
        }
    }
}