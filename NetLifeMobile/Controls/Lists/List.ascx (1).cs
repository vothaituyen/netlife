using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using ATVEntity;
using BOATV;
using System.Web.Services;
using System.Data;

namespace NetLifeMobile.Controls.Lists
{
    public partial class List : System.Web.UI.UserControl
    {
        private int pageSize = 20;
        public int PageSize { set { pageSize = value; } }
        private string item = "<div class=\"row item-list\"><div class=\"col-xs-12 pd\"><div class=\"col-xs-5 img-list-item\"> {0} </div><div class=\"col-xs-7 info-list-item\"><p> <a href=\"{1}\">{2}</a></p></div> </div></div>";
        private string baiNoiBat ="<div class=\"col-xs-12\" style=\"text-align: center\">{0}</div><a href=\"{1}\"> <div class=\"col-xs-12 title-nb\">{2} </div></a>";
        string newsIds = string.Empty;
        long newsId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Lib.QueryString.PageIndex <= 1)
            {
                List<NewsPublishEntity> lst = BOATV.NewsPublished.GetListNewsByNewsMode3(Lib.QueryString.CategoryID, 1, 5, 6, 1, 453);
                if (lst != null && lst.Count > 0)
                {
                    Literal1.Text = String.Format(baiNoiBat, lst[0].URL_IMG, lst[0].URL, lst[0].NEWS_TITLE, Utils.CatSapo(lst[0].NEWS_INITCONTENT, 25));
                    newsId = lst[0].NEWS_ID;
                    newsIds = newsId.ToString() + ",";
                }
                else
                {
                    Literal1.Visible = false;
                }
            }
           
            List<NewsPublishEntity> lstNew = NewsPublished.Danh_Sach_Tin_Theo_Cat(Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, pageSize, Lib.QueryString.PageIndex, 213, newsIds);
            if (lstNew.Count > 0)
            {
                for (int i = 0; i < lstNew.Count; i++)
                {
                    LtrItem.Text += String.Format(item, lstNew[i].URL_IMG, lstNew[i].URL, lstNew[i].NEWS_TITLE, lstNew[i].NEWS_PUBLISHDATE.ToString("dd-MM-yyyy HH:mm"), lstNew[i].NEWS_INITCONTENT);
                }
            }

            var c = BOCategory.GetCategory(Lib.QueryString.CategoryID);
            if (c != null)
            {
                hplNext.Text = c.Cat_Name;
                hplNext.NavigateUrl = c.HREF;
                Utils.SetPageHeader(this.Page, c.Cat_Name, c.Cat_Description, "");

                if (c.Cat_ParentID != null && c.Cat_ParentID > 0) {
                    var catParentMenu = BOCategory.GetCategory(c.Cat_ParentID);
                    hplNextMenu.Text = catParentMenu.Cat_Name;
                    hplNextMenu.NavigateUrl = catParentMenu.HREF;
                }

            }
            
                

            Paging1.TotalPage = NewsPublished.NP_Sao_Danh_Sach_Tin_Count(Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, pageSize);
            Paging1.DoPagging(Lib.QueryString.PageIndex);
            Paging1.HidePagging(false);
        }

        
    }
}