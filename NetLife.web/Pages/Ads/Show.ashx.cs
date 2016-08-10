using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATVCommon;
using BOATV;

namespace NetLife.web.Dout
{
    /// <summary>
    /// Summary description for Show
    /// </summary>
    public class Show : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/x-javascript";
            context.Response.Expires = -1;

            string postId = context.Request.QueryString["zoneId"] ?? String.Empty;
            string catId = context.Request.QueryString["catId"] ?? String.Empty;
            if (!String.IsNullOrEmpty(postId) && !String.IsNullOrEmpty(catId))
            {
                string html = BOAdv.GetAdvItemById(Lib.Object2Integer(postId), Lib.Object2Integer(catId));
                if (!String.IsNullOrWhiteSpace(html))
                {
                    context.Response.Write(String.Format("var zone{0}_{2} = new RunBanner({1}, \"zone{0}_{2}_Adv\"); zone{0}_{2}.Show();", catId, html.Replace("\\n", " ").Replace("\\t", " "), postId).Replace("INSERT_RANDOM_NUMBER_HERE", DateTime.Now.ToFileTime().ToString()));
                }
            }

            return;

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