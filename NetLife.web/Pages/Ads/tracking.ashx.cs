using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using NC.Ads.BO;

namespace VMCAds.Dout
{
    /// <summary>
    /// Summary description for tracking
    /// </summary>
    public class tracking : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/x-javascript";
            context.Response.Expires = 10;

            var result = Lib.getExt(context);

            var item = new QuangCaoItem();
            string logDomain = QuangCaoItem.GetScriptVersion("DOMAIN");
            if (!String.IsNullOrEmpty(logDomain))
            {
                context.Response.Write(item.QuangCaoItemByExtCache(result[0], result[1]).Replace("tracking.vietnamnetad.vn", logDomain));
            }
            else
            {
                context.Response.Write(item.QuangCaoItemByExtCache(result[0], result[1]));
            }
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