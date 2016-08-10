using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NC.Ads.BO;

namespace VMCAds.Dout
{
    /// <summary>
    /// Summary description for CPM
    /// </summary>
    public class CPM : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int Id = 0;
            string zoneId = context.Request.QueryString["Zone_ID"] ?? "ZoneId";
            int.TryParse(context.Request.QueryString["Id"], out Id);
             
            var banner = new QuangCaoItem();
            var html = banner.QuangCaoItemById(Id);
            string autoId = System.Guid.NewGuid().ToString("N");

            context.Response.Write(String.Format("(new RunBanner({0}, '{1}', 0 , true)).Show();", html, zoneId));
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