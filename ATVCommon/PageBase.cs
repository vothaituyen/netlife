using System;
using System.Text;
using System.Web.Caching;
using System.Web.UI;
using System.IO;
using System.Web;
using System.Configuration;
using System.Reflection;

namespace ATVCommon
{
    
    public class PageBase : Page
    {         
        bool isUpdate = false;
        DateTime startTime;
        DateTime endTime;
        public bool IsUpdate { set { isUpdate = value; } }
        private static readonly Assembly _currentAssembly = Assembly.GetExecutingAssembly();

        public string PageCacheName
        {
            get
            {
                return string.Format(Constants.CACHE_NAME_FORMAT_HTML_PAGE_CONTENT, Lib.QueryString.CategoryID, Lib.QueryString.NewsID, Lib.QueryString.EventID, Lib.QueryString.PageIndex);
            }
        }

        /// <summary>
        /// ControlCache[{container_id}][{raw_url}]
        /// </summary>
        /// <param name="containerId">Control Container Id</param>
        /// <returns>Control cache name for current url and container id</returns>
        public string ControlCacheName(string containerId)
        {
            if (Lib.QueryString.NewsID == 0 || Lib.QueryString.EventID == 0)
            {
                return string.Format(Constants.CACHE_NAME_FORMAT_HTML_CONTROL_CONTENT, containerId, Lib.QueryString.CategoryID, Lib.QueryString.NewsID, Lib.QueryString.EventID, 0);
            }
            else
            {
                return string.Format(Constants.CACHE_NAME_FORMAT_HTML_CONTROL_CONTENT, containerId, Lib.QueryString.CategoryID, Lib.QueryString.NewsID, Lib.QueryString.EventID, Lib.QueryString.PageIndex);
            }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            startTime = DateTime.Now;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder strBuilder = new StringBuilder();
            using (StringWriter strWriter = new StringWriter(strBuilder))
            {
                string sVirURL = (null != HttpContext.Current.Items["VirtualUrl"] ? HttpContext.Current.Items["VirtualUrl"].ToString() : "");

                using (RewriteFormHtmlTextWriter htmlWriter = new RewriteFormHtmlTextWriter(strWriter, sVirURL))
                {
                    base.Render(htmlWriter);
                    string html = strBuilder.ToString();
                    html = html.Replace("\t", " ");
                    html = html.Replace("    ", " ");
                    html = html.Replace("  ", " ");
                    //html = html.Replace(Environment.NewLine, " ");
                    endTime = DateTime.Now;
                    TimeSpan ts = endTime.Subtract(startTime);
                    //string realHtml = html;
                    html = html.Replace("#LoadTime#","-No-Cached-" + ts.TotalMilliseconds.ToString());
                    writer.Write(html);
                    
                    if (!isUpdate && ConfigurationManager.AppSettings["AllowDistCache"] == "1")
                    {
                        SaveToCacheDependency(Request.RawUrl, html);
                    }
                }
            }
        }

        public static void SaveToCacheDependency(string cacheName, object data)
        {
            string database = System.Configuration.ConfigurationSettings.AppSettings["CoreDb"];
            SqlCacheDependency sqlDep = new SqlCacheDependency(database, "HtmlCached");
            if (data != null)
                HttpContext.Current.Cache.Insert(cacheName, data, sqlDep);
        }

        private class RewriteFormHtmlTextWriter : HtmlTextWriter
        {
            private string _formAction;

            public RewriteFormHtmlTextWriter(System.IO.TextWriter writer)
                : base(writer)
            {

            }
            public RewriteFormHtmlTextWriter(System.IO.TextWriter writer, string action)
                : base(writer)
            {
                if (!string.IsNullOrEmpty(action))
                {
                    this._formAction = action;
                }
            }

            public override void RenderBeginTag(string tagName)
            {
                if (tagName.ToString().IndexOf("form") >= 0)
                {
                    base.RenderBeginTag(tagName);
                }
            }

            public override void WriteAttribute(string name, string value, bool fEncode)
            {
                if ("action" == name && !string.IsNullOrEmpty(this._formAction))
                {
                    value = this._formAction;
                }
                base.WriteAttribute(name, value, fEncode);
            }
        }
    }
}
