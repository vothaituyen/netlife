using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using NetLifeMobile;

namespace NetLifeMobile
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
           
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
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
