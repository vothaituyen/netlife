using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using BOATV;
using NetLife.web;
using NetLife.web.Log;

namespace NetLife.web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            var log = new Thread(UpdateLog);
            log.Start();

            var monitor = new Thread(CacheMonitorManagerUpdate);
            monitor.Start();
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
        private void UpdateLog()
        {
            var log = new BOATV.Log();
            while (true)
            {
                //Cap nhat PageView theo chuyen muc
                log.CaculateLogViewCategory(LogView.ViewQueue);

                //Cap nhat PageView theo bai viet
                log.CaculateLogViewNews(LogView.NewsQueue);

                log.CaculateLogClickAds(Pages.Ads.log.ClickQueue);

                log.CaculateLogViewAds(Pages.Ads.log.ImpressionQueue);

                Thread.Sleep(5 * 60 * 1000);
            }
        }
        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            string urlMobile = HttpContext.Current.Request.RawUrl.ToString().ToLower();
            if (Utils.isMobileBrowser(HttpContext.Current))
            {
                if (urlMobile.Contains("/default.aspx"))
                    HttpContext.Current.Response.Redirect("http://m.netlife.vn");
                else
                    HttpContext.Current.Response.Redirect("http://m.netlife.vn" + urlMobile);
            }
        }

        private string sLogFormat;
        private string sErrorTime;
        string writeErrorToFile = System.Configuration.ConfigurationSettings.AppSettings["WriteErrorToFile"] != null ? System.Configuration.ConfigurationSettings.AppSettings["WriteErrorToFile"].ToString() : "";
       /* protected void Application_Error(Object sender, EventArgs e)
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
        */
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
