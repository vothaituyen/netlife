using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLife.web.Log
{
    /// <summary>
    /// Summary description for LogView
    /// </summary>
    public class LogView : IHttpHandler
    {

        public static Queue<Int32> ViewQueue = new Queue<int>();
        public static Queue<Int32> VisitorQueue = new Queue<int>();
        public static Queue<Int64> NewsQueue = new Queue<Int64>();
        public void ProcessRequest(HttpContext context)
        {

            if (context.Request.QueryString["categoryId"] != null)
            {
                var itemId = 0;
                int.TryParse(context.Request.QueryString["categoryId"], out itemId);
                ViewQueue.Enqueue(itemId);
            }

            if (context.Request.QueryString["newsId"] != null)
            {
                Int64 newsId = 0;
                Int64.TryParse(context.Request.QueryString["newsId"], out newsId);
                if (newsId > 0)
                    NewsQueue.Enqueue(newsId);
            }

            context.Response.Expires = -1;
            context.Response.ContentType = "text/html";
            context.Response.Write("");
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