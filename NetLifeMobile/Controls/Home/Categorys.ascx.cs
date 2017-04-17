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
    public partial class Categorys : System.Web.UI.UserControl
    {
        private int _cat_id = 0;
        public int Cat_ID { set { _cat_id = value; } get { return _cat_id; } }
        private int _cat_parent_id = 0;
        string catName = "<a href=\"{1}\"><div class=\"row tit-cat\">{0}</div> </a>";
        string baiNoiBat = "<div class=\"col-xs-12 item-cat-img\">{0} </div><a href=\"{1}\"> <div class=\"col-xs-12 tit-content-cat\">{2}</div>  </a>";
        //string listNews = "<a href=\"{1}\"><li>{2}</li> </a>"; // htthao edit 20160527
        //string listNews = "<li class=\"news_home\"><div class=\"row\"><span class=\"col-xs-2 col-sm-1 col-md-1\"><a href=\"{1}\"><img src=\"{3}\" alt=\"\" height=\"50\" width=\"60\"></span> <span class=\"col-xs-10 title\" style=\"padding-left:20px;\">{2}</a></span></div></li>"; // htthao add 20160527
        //string listNews = "<li class=\"news_home\"><div class=\"row\"><span class=\"col-xs-2 col-sm-1 col-md-1\"><a href=\"{1}\"><img src=\"{3}?width=100&height=83&mode=crop\" alt=\"\" ></span> <span class=\"col-xs-10 title\" style=\"padding-left:40px;\">{2}</a></span></div></li>";
        //string listNews = "<li class=\"news_home\"> <div class=\"row\"> <a href=\"{1}\"> <span class=\"col-xs-5 col-sm-5 col-md-5\" > <img src=\"{3}?width=213&crop=auto&scale=both\" alt=\"\"> </span> <span class=\"col-xs-7 col-sm-7 col-md-7 title\">{2} </span> </a> </div> </li>";
        //string listNews = "<li class=\"news_home\"> <div class=\"row\"> <a href=\"{1}\"> <span class=\"col-xs-4 col-sm-3\"> <center><img src=\"{3}?width=213&crop=auto&scale=both\" alt=\"\" ></center> </span> <span class=\"col-xs-8 col-sm-9 title\" style=\"padding-left:5px;\">{2} </span> </a> </div> </li>";
        string listNews = "<div class=\"row item-list\"> 	<div class=\"col-xs-12 pd\">       <div class=\"col-xs-5 img-list-item\">          <a href=\"{1}\" title=\"{2}\">             <img src=\"{3}?width=213&crop=auto&scale=both\" title=\"{2}\" alt=\"{2}\">          </a>       </div>       <div class=\"col-xs-7 info-list-item\"><a href=\"{1}\">{2} </a></div> 	</div> </li>";

        private long newsId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //var domain = 
            CategoryEntity cat = BOCategory.GetCategory(_cat_id);
            ltrCatName.Text = String.Format(catName, cat.Cat_Name, (String.Format("/{0}.html", cat.Cat_DisplayURL.ToLower())));

            List<NewsPublishEntity> lst = BOATV.NewsPublished.GetListNewsByNewsMode3(_cat_id, 1, 5, 6, 1, 460);
            if (lst != null && lst.Count > 0)
            {
                ltrNotBat.Text = String.Format(baiNoiBat, lst[0].URL_IMG, lst[0].URL, lst[0].NEWS_TITLE, Utils.CatSapo(lst[0].NEWS_INITCONTENT, 25));
                newsId = lst[0].NEWS_ID;
            }
            List<NewsPublishEntity> lstNew = BOATV.NewsPublished.GetListNewsByCatAndDate(_cat_id, newsId, 1, 6, 0);
            if (lstNew.Count > 0)
            {
                for (int i = 0; i < lstNew.Count; i++)
                {
                    lrtListNew.Text += String.Format(listNews, lstNew[i].URL_IMG, lstNew[i].URL, lstNew[i].NEWS_TITLE, lstNew[i].Imgage.ImageUrl);
                }
            }
        }
    }
}