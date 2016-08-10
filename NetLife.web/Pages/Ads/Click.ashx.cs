using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NC.Ads.BO;

namespace VMCAds.Dout
{
    /// <summary>
    /// Summary description for Click
    /// </summary>
    public class Click : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public static Queue<int> ListItemId = new Queue<int>();
        public static Queue<Ads_MMSQ> MsmQueueAds = new Queue<Ads_MMSQ>(); 
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["itemId"] != null)
            {
                var itemId = Convert.ToInt32(context.Request.QueryString["itemId"]);
                if (itemId > 0)
                    ListItemId.Enqueue(itemId);

                var msg = new Ads_MMSQ
                {
                    ListItemIds = context.Request.QueryString["itemId"].ToString(CultureInfo.InvariantCulture),
                    ViewDate = DateTime.Now,
                    IPAddress = context.Request.UserHostAddress,
                    ReferUrl = context.Request.QueryString["location"] ?? string.Empty,
                    SessionID = context.Session.SessionID.ToString(CultureInfo.InvariantCulture),
                    TrueImpression = string.Empty
                };
                     
                MsmQueueAds.Enqueue(msg);
            }
            string url = context.Request.QueryString["nextUrl"] ?? context.Request.QueryString["location"];
            if (!String.IsNullOrEmpty(url))
                context.Response.Redirect(url, true);
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