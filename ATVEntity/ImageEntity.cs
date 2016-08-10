using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ATVEntity
{
    [Serializable()]
    public class ImageEntity
    {
        string ImagesThumbUrl = System.Configuration.ConfigurationSettings.AppSettings["ImageUrl"].ToString().TrimEnd('/');
        string ImagesStorageUrl = System.Configuration.ConfigurationSettings.AppSettings["ImagesStorageUrl"].ToString().TrimEnd('/');        
        public ImageEntity(int width, string url)
        {
            this.width = width;
            if (url != null)
                this.url = url;
            else
                this.url = "";
        }
        private string url = "";
        private int width;
        public int Width { set { width = value; } get { return width; } }
        public string OnError
        {
            get
            {
                return String.Format("{0}/GetThumbNail.ashx?ImgFilePath={1}&width={2}", 
                                                            ImagesThumbUrl,
                                                            HttpUtility.UrlEncode(String.Format("{0}/{1}", ImagesStorageUrl, url)), width
                                                            ); } }
        public string ImageUrl { get { return url != null ? url : String.Empty; } }
        public string StorageUrl
        {
            get
            {
                if (url.StartsWith("http")) 
                    return url;
                else
                    return String.Format("{0}/{1}", ImagesStorageUrl, url);
            }
        }

    }
}
