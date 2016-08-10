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
    public partial class List : System.Web.UI.UserControl
    {
        private int pageSize = 20;
        public int PageSize { set { pageSize = value; } }
        private string item = " <div class=\"row item-list\"><div class=\"img-list\">{0} </div> <div class=\"info-list\"><h5>{3}</h5><h3><a title=\"2\" href=\"{1}\">{2}</a></h3><p>{4}</p></div>  </div>";
        string newsIds = string.Empty;
        long newsId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Lib.QueryString.ParentCategoryID > 0)
            {
                var cat = BOCategory.GetCategory(Lib.QueryString.ParentCategoryID);
                if (cat != null)
                    Utils.Move301(cat.HREF);
                return;
            }

            List<NewsPublishEntity> lst = BOATV.NewsPublished.GetListNewsByNewsMode3(Lib.QueryString.CategoryID, 1, 5, 6, 1, 453);
            if (lst != null && lst.Count > 0)
            {
                newsId = lst[0].NEWS_ID;
                newsIds = newsId.ToString() + ",";
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
                Utils.SetPageHeader(this.Page, c.Cat_Name + (Lib.QueryString.PageIndex > 1 ? " | trang " + Lib.QueryString.PageIndex : ""), c.Cat_Description + " - trang " + Lib.QueryString.PageIndex, "");

            Paging1.TotalPage = NewsPublished.NP_Sao_Danh_Sach_Tin_Count(Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, pageSize);
            Paging1.DoPagging(Lib.QueryString.PageIndex);
            Paging1.HidePagging(false);
        }
    }
}