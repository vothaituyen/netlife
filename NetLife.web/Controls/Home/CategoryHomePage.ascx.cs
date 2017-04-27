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
    public partial class CategoryHomePage : System.Web.UI.UserControl
    {
        private int _cat_id = 0;
        public int Cat_ID { set { _cat_id = value; } get { return _cat_id; } }
        //private int _cat_parent_id = 0;
        string catName = "<h3><a href=\"{1}\" title=\"{0}\">{0}</a></h3>";
        string baiNoiBat = "<div class=\"row\">{0}</div><h2 class=\"row\"><a href=\"{1}\">{2}</a></h2><p>{3}</p>";
        string listNews = "<li class=\"col-md-12 liitem\"><a href=\"{1}\">{2}</a></li>";
        private int top = 2;
        public int Top { set { top = value; } }
        private long newsId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //var domain = 
            CategoryEntity cat = BOCategory.GetCategory(_cat_id);
            ltrCatName.Text = String.Format(catName, cat.Cat_Name, cat.HREF);

            List<NewsPublishEntity> lst = BOATV.NewsPublished.GetListNewsByNewsMode3(_cat_id, 1, 5, 6,1, 310);
            if (lst != null && lst.Count > 0)
            {
                ltrNotBat.Text = String.Format(baiNoiBat, lst[0].URL_IMG, lst[0].URL, lst[0].NEWS_TITLE, Utils.CatSapo(Utils.RemoveHTMLTag(lst[0].NEWS_INITCONTENT), 25));
                newsId = lst[0].NEWS_ID;
            }
            List<NewsPublishEntity> lstNew = BOATV.NewsPublished.GetListNewsByCatAndDate(_cat_id, newsId, 1, top, 150);
            if (lstNew.Count > 0)
            {
                for (int i = 0; i < lstNew.Count; i++)
                {
                    lstNew[i].NEWS_TITLE = lstNew[i].NEWS_TITLE.ToString().Substring(0, (lstNew[i].NEWS_TITLE.ToString().Length < 60 ? lstNew[i].NEWS_TITLE.ToString().Length : 57)) + (lstNew[i].NEWS_TITLE.ToString().Length < 60 ? "" : "...");
                    lrtListNew.Text += String.Format(listNews, lstNew[i].URL_IMG, lstNew[i].URL, lstNew[i].NEWS_TITLE);
                }
            }
        }
    }
}