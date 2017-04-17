using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using ATVEntity;
using BOATV;

namespace NetLife.web.Controls.Lists
{
    public partial class HighlightsList : System.Web.UI.UserControl
    {
        //private string hot = "<li><div class=\"row\"><span class=\"col-md-4\" style=\"padding-top:10px;\"><a title=\"{2}\" href=\"{1}\"><img src=\"{0}\" alt=\"\" height=\"50\" width=\"60\"></a></span><span class=\"col-md-8\"><a title=\"{2}\" href=\"{1}\">{2}</a></span></div></li>";
        private string hot = "<li><div class=\"row\"><span class=\"col-md-4\" style=\"padding-top:10px;\"><a title=\"{2}\" href=\"{1}\"><img src=\"{0}?width=60&height=50&mode=crop\" alt=\"{2}\" ></a></span><span class=\"col-md-8\"><a title=\"{2}\" href=\"{1}\">{2}</a></span></div></li>";
        private string baiNoiBat = "<div class=\"row img-nb\">{0}</div><div class=\"row title-nb\"> <a title=\"{2}\" href=\"{1}\">{2}</a> </div>";

        private long newsId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //var domain = 
            //CategoryEntity cat = BOCategory.GetCategory(_cat_id);
            //ltrCatName.Text = String.Format(catName, cat.Cat_Name, (String.Format("/{2}/p{0}c{1}.html", _cat_parent_id, _cat_id, Utils.UnicodeToKoDauAndGach(cat.Cat_Name).ToLower())));

            List<NewsPublishEntity> lst = BOATV.NewsPublished.GetListNewsByNewsMode3(Lib.QueryString.CategoryID, 1, 5, 6, 1, 440);
            if (lst != null && lst.Count > 0)
            {
                Literal1.Text = String.Format(baiNoiBat, lst[0].URL_IMG, lst[0].URL, lst[0].NEWS_TITLE, Utils.CatSapo(lst[0].NEWS_INITCONTENT, 25));
                newsId = lst[0].NEWS_ID;
            }

            List<NewsPublishEntity> lstHot = BOATV.NewsPublished.NP_Xem_Nhieu_Nhat(6, 75, Lib.QueryString.CategoryID);
            if (lstHot != null && lstHot.Count > 0)
            {
                for (int i = 0; i < (lstHot.Count> 5 ? 5 : lstHot.Count); i++)
                {
                    //Literal2.Text += String.Format(hot, lstHot[i].URL_IMG, lstHot[i].URL, lstHot[i].NEWS_TITLE);70
                    lstHot[i].NEWS_TITLE = lstHot[i].NEWS_TITLE.ToString().Substring(0, (lstHot[i].NEWS_TITLE.ToString().Length < 65 ? lstHot[i].NEWS_TITLE.ToString().Length : 62)) + (lstHot[i].NEWS_TITLE.ToString().Length < 65 ? "" : "...");
                    Literal2.Text += String.Format(hot, lstHot[i].Imgage.ImageUrl, lstHot[i].URL, lstHot[i].NEWS_TITLE);
                }
            }
        }
    }
}