using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ATVEntity
{
    [Serializable()]
    public class NewsPublishEntity
    {
        public NewsPublishEntity()
        {
            url = "";
            news_title = "";
            news_initcontent = "";
            news_publishdate = new DateTime();
            news_subtitle = "";
            news_relation = new List<NewsPublishEntity>();
            news_content = "";
            img = new ImageEntity(0, "");
            news_imagesnote = "";
            news_otherCat = new List<int>();
            keyword = string.Empty;
            news_Rate = 0;

        }

        private int news_Rate;
        public Int32 News_Rate
        {
            get { return news_Rate; }
            set { news_Rate = value; }
        }
        private string news_imagesnote;
        private List<Int32> news_otherCat;
        public List<Int32> NEWS_OTHERCAT { get { return news_otherCat; } set { news_otherCat = value; } }
        private string url;
        private string news_content;
        List<NewsPublishEntity> news_relation;
        private Int64 newsid;
        private int cat_id;
        private int cat_parentid;
        private string news_Athor;
        public int Cat_Id { set { cat_id = value; } get { return cat_id; } }
        public int Cat_ParentId { set { cat_parentid = value; } get { return cat_parentid; } }

        private string news_title;
        private string news_initcontent;
        private DateTime news_publishdate;
        private string news_subtitle;
        private string icon;
        private ImageEntity img;
        public ImageEntity Imgage { get { return img; } set { img = value; } }
        public Int64 NEWS_ID { get { return newsid; } set { newsid = value; } }
        public string NEWS_CONTENT { get { return news_content; } set { news_content = value; } }
        public string NEWS_ATHOR { get { return news_Athor; } set { news_Athor = value; } }
        public string URL { get { return url; } set { url = value; } }
        public List<NewsPublishEntity> NEWS_RELATION { get { return news_relation; } set { news_relation = value; } }
        public string NEWS_TITLE { get { return news_title; } set { news_title = value; } }
        public string NEWS_INITCONTENT { get { return news_initcontent; } set { news_initcontent = value; } }
        public string ICON { set { icon = value; } get { return icon; } }
        public DateTime NEWS_PUBLISHDATE { get { return news_publishdate; } set { news_publishdate = value; } }
        public string NEWS_SUBTITLE { get { return news_subtitle; } set { news_subtitle = value; } }
        public string NEWS_IMAGESNOTE { get { return news_imagesnote; } set { news_imagesnote = value; } }
        private string keyword;
        public string Keywrods { get { return keyword; } set { keyword = value; } }
        public string URL_IMG
        {
            get
            {
                if (img.ImageUrl == null || img.ImageUrl.Trim().Length == 0) return String.Empty;
                //Dam bao rang title da dc decode chuan
                return GetThumbNail(news_title, url, img.ImageUrl, img.Width);
            }
        }
         

        string ImagesThumbUrl = System.Configuration.ConfigurationSettings.AppSettings["ImageUrl"].ToString().TrimEnd('/');
        string ImagesStorageUrl = System.Configuration.ConfigurationSettings.AppSettings["ImagesStorageUrl"].ToString().TrimEnd('/');

        private string GetThumbNail(string title, string url, string img, int width)
        {
            if (img == null || String.IsNullOrEmpty(img)) return String.Empty;
            //return String.Format("<a title=\"{2}\" href=\"{0}\"><img src=\"{1}?width={3}&crop=auto&scale=both\" title=\"{2}\" alt=\"{2}\" border=\"0\"/></a>", url, (img.StartsWith(ImagesStorageUrl) ? img : ImagesStorageUrl + "/" + img), HttpUtility.HtmlEncode(title), width); ;
            return String.Format("<a title=\"{2}\" href=\"{0}\"><img src=\"{1}\" title=\"{2}\" alt=\"{2}\" border=\"0\"/></a>", url, (img.StartsWith(ImagesStorageUrl) ? img : ImagesStorageUrl + "/" + img), HttpUtility.HtmlEncode(title), width); ;
        }

      

        
    }
}
