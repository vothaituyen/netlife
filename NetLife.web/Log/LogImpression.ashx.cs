using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLife.web.Log
{
    /// <summary>
    /// Summary description for LogImpression
    /// </summary>
    public class LogImpression : IHttpHandler
    {

        public static Queue<Int32> QImpressionQueue = new Queue<int>(); 
        public void ProcessRequest(HttpContext context)
        {
            Int32 itemId = 0;
            if (context.Request.QueryString["id"] != null)
            {
                int.TryParse(context.Request.QueryString["id"], out itemId); 
                QImpressionQueue.Enqueue(itemId);
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