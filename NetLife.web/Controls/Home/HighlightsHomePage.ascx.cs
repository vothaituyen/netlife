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
    
    public partial class HighlightsHomePage : System.Web.UI.UserControl
    {
        private int top = 7;
        public int Top {  set { top = value; } }
        private string news = "<li><div class=\"row\"><span class=\"col-md-4\" style=\"padding-right:0px;\"><a href=\"{1}\"><img src=\"{0}\" alt=\"\" height=\"50\" width=\"60\"></a></span><span class=\"col-md-8\"><a title=\"{2}\" href=\"{1}\">{2}</a></span></div></li>";
        string title = "<span><a href=\"{0}\">{1}</a></span>";
        string listitem = "<div class=\"col-md-3 item-nb\">{0}<h3><a href=\"{1}\" title=\"{2}\">{2}</a></h3></div>";
        private string strListOne = "<div class=\"row img-nb\">{0}</div><div class=\"row title-nb\"> <a href=\"{1}\">{2}</a></div>";
        protected void Page_Load(object sender, EventArgs e)
        {
            var lst = BOATV.NewsPublished.GetListBonBaiNoibat(6, 440);
            if (lst != null && lst.Count > 0)
            {
                /*ltrnb.Text = String.Format(strListOne, lst[0].URL_IMG, lst[0].URL, lst[0].NEWS_TITLE,
                                            Utils.CatSapo(lst[0].NEWS_INITCONTENT, 40));*/

                ltrnb.Text = String.Format(strListOne, lst[0].URL_IMG, lst[0].URL, lst[0].NEWS_TITLE,
                                            Utils.CatSapo(lst[0].NEWS_INITCONTENT, 40));

            }

            if (lst != null && lst.Count > 1)
            {
                for (int i = 1; i < 5; i++)
                {
                    lst[i].Imgage = new ImageEntity(160, lst[i].Imgage.ImageUrl);
                    lst[i].NEWS_TITLE = lst[i].NEWS_TITLE.ToString().Substring(0, (lst[i].NEWS_TITLE.ToString().Length < 70 ? lst[i].NEWS_TITLE.ToString().Length : 67)) + (lst[i].NEWS_TITLE.ToString().Length < 70 ? "" : "...");
                    ltrItem.Text += String.Format(listitem, lst[i].URL_IMG, lst[i].URL, lst[i].NEWS_TITLE);
                }
             
            }
            var tinmoi = BOATV.NewsPublished.NP_Tin_Nong(0, 3, top, 0);
            if (tinmoi != null && tinmoi.Count > 0)
            {
                for (int i = 0; i < (tinmoi.Count>5? 5:tinmoi.Count); i++)
                {
                    tinmoi[i].NEWS_TITLE = tinmoi[i].NEWS_TITLE.ToString().Substring(0, (tinmoi[i].NEWS_TITLE.ToString().Length<55? tinmoi[i].NEWS_TITLE.ToString().Length:50)) + (tinmoi[i].NEWS_TITLE.ToString().Length < 55 ? "" : "...");
                    ltrNews.Text += String.Format(news, tinmoi[i].Imgage.ImageUrl, tinmoi[i].URL, tinmoi[i].NEWS_TITLE);
                }
            }
        }
    }
}