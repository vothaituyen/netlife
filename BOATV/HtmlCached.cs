using System;
using System.Collections.Generic;
using System.Text;
using ATVEntity;


namespace BOATV
{
    public class HtmlCached
    {
        #region Trang chu
        #region BoxH
        static string GUI_BOXH_KEY =  "GUI_BoxH-{0}-{1}-{2}-{3}-{4}";
        static string GUI_BOXH_LI_ITEM = "<li><a href=\"{1}\" title=\"{2}\" class=\"title_home\">{2}</a><p>{3}</p></li>";

        public static string GUI_BoxH(int cat_parentid, int cat_id, int top, int ImgWidth, News_Mode news_mode)
        {
            string key = String.Format(GUI_BOXH_KEY, cat_id, cat_parentid, top, ImgWidth, news_mode.ToString());
            string strHTML = Utils.GetFromCache<string>(key);
            if (strHTML != null && strHTML.Trim().Length > 0) return strHTML;
            List<NewsPublishEntity> lst = BOATV.NewsPublished.NP_Select_Top_Home(cat_parentid, cat_id, top, ImgWidth);
            NewsPublishEntity nep;
            int iCount = lst != null ? lst.Count : 0;
            for (int i = 0; i < iCount; i++)
            {
                nep = lst[i];
                strHTML += String.Format(GUI_BOXH_LI_ITEM, nep.URL_IMG, nep.URL, nep.NEWS_TITLE, Utils.CatSapo(nep.NEWS_INITCONTENT, 25));
            }
            Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, strHTML);
            return strHTML; 
        }
        #endregion
        #endregion

        #region Sao
        #endregion

        #region Am nhac
        #region Hot Album
        static string GUI_HOTALBUM_ITEM = "<div class=\"hotalbum_item\">{0}<h1><a href=\"{1}\">{2}</a></h1></div>";
        static string GUI_HOTALBUM_KEY = "GUI_NP_SelectListTopHotByCat-{0}-{1}-{2}-{3}-{4}";
            
        public static string GUI_HotAlbum(int cat_parentid, int cat_id, int top, int ImgWidth, News_Mode news_mode)
        {
            string key = String.Format(GUI_HOTALBUM_KEY, cat_id, cat_parentid, top, ImgWidth, news_mode.ToString());
            string strHTML = Utils.GetFromCache<string>(key);
            if (strHTML != null && strHTML.ToString().Length > 0) return strHTML;
            List<NewsPublishEntity> lst = BOATV.NewsPublished.NP_SelectListTopHotByCat(cat_parentid, cat_id, top, ImgWidth, news_mode);
            NewsPublishEntity nep;
            strHTML = String.Empty;
            int iCount = lst != null ? lst.Count : 0;
            for (int i = 0; i < iCount; i++)
            {
                nep = lst[i];
                strHTML += String.Format(GUI_HOTALBUM_ITEM, nep.URL_IMG, nep.URL, nep.NEWS_TITLE);
            }
            
            Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, strHTML);
            nep = null;
            return strHTML;
        }
        #endregion

        #region

        #endregion
        #endregion

        #region Phim
        #endregion

        #region Thu vien
        #endregion

        #region Thoi trang
        #endregion

        #region Hoi dap
        #endregion
    }
}
