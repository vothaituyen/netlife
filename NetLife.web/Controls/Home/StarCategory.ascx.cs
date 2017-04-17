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
    public partial class StarCategory : System.Web.UI.UserControl
    {
        private int _cat_id = 0;
        public int Cat_ID { set { _cat_id = value; } get { return _cat_id; } }
//        private int _cat_parent_id = 0;
        string catName = "<h3><a href=\"{1}\" title=\"{0}\">{0}</a></h3>";
        string baiNoiBat = "<div class=\"row\">{0}</div><h2 class=\"row\"><a href=\"{1}\">{2}</a></h2><p style=\"height:55px\">{3}</p>";
        string listNews = "<li class=\"col-md-12\"> <div class=\"img-it\">{0}</div><p> <a href=\"{1}\">{2}</a></p> </li>";

        private int top = 11;
        public int Top { set { top = value; } }

        private long newsId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //var domain = 
            CategoryEntity cat = BOCategory.GetCategory(_cat_id);
            //ltrCatName.Text = String.Format(catName, cat.Cat_Name, cat.HREF); // old source
            
             
                ltrCatName_other.Text = String.Format(catName, cat.Cat_Name, cat.HREF);


            List<NewsPublishEntity> lst = BOATV.NewsPublished.GetListNewsByNewsMode2(_cat_id, 1, 1, 16, 310); // thay 10 bang 11
            //List<NewsPublishEntity> lst;
            //if (cat.Cat_Name == "Sao")
            //{
            //    lst = BOATV.NewsPublished.GetListNewsByNewsMode2(_cat_id, 1, 1, 6, 310);
            //}
            //else
            //{
            //    lst = BOATV.NewsPublished.GetListNewsByNewsMode2(_cat_id, 1, 1, 10, 310);
            //}
            if (lst != null && lst.Count > 0)
            {
     
                lst[0].NEWS_INITCONTENT = lst[0].NEWS_INITCONTENT.ToString().Substring(0, (lst[0].NEWS_INITCONTENT.ToString().Length < 100 ? lst[0].NEWS_INITCONTENT.ToString().Length : 97)) + (lst[0].NEWS_INITCONTENT.ToString().Length < 100 ? "" : "...");
                lst[0].NEWS_TITLE = lst[0].NEWS_TITLE.ToString().Substring(0, (lst[0].NEWS_TITLE.ToString().Length < 70 ? lst[0].NEWS_TITLE.ToString().Length : 70)) + (lst[0].NEWS_TITLE.ToString().Length < 70 ? "" : "...");

                    ltrNotBat_other.Text = String.Format(baiNoiBat, lst[0].URL_IMG, lst[0].URL, lst[0].NEWS_TITLE, Utils.CatSapo(lst[0].NEWS_INITCONTENT, 25));
                    newsId = lst[0].NEWS_ID;
                
               
                for (int i = 1; i <= (_cat_id == 54? 5:5); i++)
                {
                    lst[i].NEWS_TITLE = lst[i].NEWS_TITLE.ToString().Substring(0, (lst[i].NEWS_TITLE.ToString().Length < 70 ? lst[i].NEWS_TITLE.ToString().Length : 70)) + (lst[i].NEWS_TITLE.ToString().Length < 70 ? "" : "...");
                   
                        lst[i].Imgage = new ImageEntity(140, lst[i].Imgage.ImageUrl);
                        lrtListNew_other.Text += String.Format(listNews, lst[i].URL_IMG, lst[i].URL, lst[i].NEWS_TITLE);
                     
                    
                }
            }
        }
    }
}