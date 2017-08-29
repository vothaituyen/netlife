using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.Routing;
using System.Web.Security;
using NetLifeMobile;
using System.IO.Compression;
using BOATV;
//using NetLife.web.Log;
//using NetLife.web.Pages.Ads;
using NetLife.web.Pages;
using NetLife.web;

namespace NetLifeMobile
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //var log = new Thread(UpdateLog);
            //log.Start();

            //var monitor = new Thread(CacheMonitorManagerUpdate);
            //monitor.Start();
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            //HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //HttpContext.Current.Response.Cache.SetNoStore();
            HttpApplication app = (HttpApplication)sender;
            string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            Stream prevUncompressedStream = app.Response.Filter;

            if (acceptEncoding == null || acceptEncoding.Length == 0)
                return;

            acceptEncoding = acceptEncoding.ToLower();

            if (acceptEncoding.Contains("gzip"))
            {
                // gzip
                app.Response.Filter = new GZipStream(prevUncompressedStream,
                    CompressionMode.Compress);
                app.Response.AppendHeader("Content-Encoding",
                    "gzip");
            }
            else if (acceptEncoding.Contains("deflate"))
            {
                // defalte
                app.Response.Filter = new DeflateStream(prevUncompressedStream,
                    CompressionMode.Compress);
                app.Response.AppendHeader("Content-Encoding",
                    "deflate");
            }
        }
        private void UpdateLog()
        {
            //var log = new BOATV.Log();
            //while (true)
            //{
            //    //Cap nhat PageView theo chuyen muc
            //    log.CaculateLogViewCategory(LogView.ViewQueue);

            //    //Cap nhat PageView theo bai viet
            //    log.CaculateLogViewNews(LogView.NewsQueue);
                

            //    //log.CaculateLogClickAds(Pages.Ads.log.ClickQueue);

            //    //log.CaculateLogViewAds(Pages.Ads.log.ImpressionQueue);
            //    log.CaculateLogClickAds(NetLife.web.Pages.Ads.log.ClickQueue);
                
            //    log.CaculateLogViewAds(NetLife.web.Pages.Ads.log.ImpressionQueue);
                

            //    Thread.Sleep(5 * 60 * 1000);
            //}
        }
        private void CacheMonitorManagerUpdate()
        {
            while (true)
            {
                var c = new CacheMonitorManager();
                c.UpdateHtmlCache();
                Thread.Sleep(10 * 1000);
            }
        }
        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        private string sLogFormat;
        private string sErrorTime;
        string writeErrorToFile = System.Configuration.ConfigurationSettings.AppSettings["WriteErrorToFile"] != null ? System.Configuration.ConfigurationSettings.AppSettings["WriteErrorToFile"].ToString() : "";
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();

            if (writeErrorToFile.ToUpper() == "TRUE")
            {
                HttpApplication application = (HttpApplication)sender;
                string localPath = application.Request.Url.LocalPath;
                if (localPath.Contains(".aspx") || localPath.Contains(".html"))
                {
                    ErrorLog(Server.MapPath("/ErrorLog"), Request.Url.ToString() + Environment.NewLine + ex.Message + Environment.NewLine +
                             ex.StackTrace);
                }
            }

            HttpApplication app = sender as HttpApplication;
            app.Response.Filter = null;
        }

        private void ErrorLog(string sPathName, string sErrMsg)
        {
            string filename = sPathName + "\\Error_" + sErrorTime + ".txt";
            if (!Directory.Exists(sPathName)) Directory.CreateDirectory(sPathName);
            var sw = new StreamWriter(filename, true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();


        }
    }
}
