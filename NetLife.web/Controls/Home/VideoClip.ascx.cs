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
    public partial class VideoClip : System.Web.UI.UserControl
    {
        private int _cat_id = 0;
        public int Cat_ID { set { _cat_id = value; } get { return _cat_id; } }
        private int _cat_parent_id = 0;
        string catName = "<h3><a href=\"{1}\" title=\"{0}\">{0}</a></h3>";
        string listNews = "<li class=\"col-md-12\"><div> {0}</div> <h2> <a href=\"{1}\" title=\"{2}\">{2}</a></h2> </li>";

        private long newsId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //var domain = 
            CategoryEntity cat = BOCategory.GetCategory(_cat_id);
            ltrCatName.Text = String.Format(catName, cat.Cat_Name, cat.HREF);

            List<NewsPublishEntity> lstNew = BOATV.NewsPublished.GetListNewsByCatAndDate(_cat_id, newsId, 1, 5, 280);
            if (lstNew.Count > 0)
            {
                for (int i = 0; i < lstNew.Count; i++)
                {
                    Literal1.Text += String.Format(listNews, lstNew[i].URL_IMG, lstNew[i].URL, lstNew[i].NEWS_TITLE);
                }
            }
        }
    }
}