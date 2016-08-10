using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLife.web.Log
{
    /// <summary>
    /// Summary description for LogAds
    /// </summary>
    public class LogAds : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}