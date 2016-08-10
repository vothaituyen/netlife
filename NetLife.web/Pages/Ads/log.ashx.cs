using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATVCommon;

namespace NetLife.web.Pages.Ads
{
    /// <summary>
    /// Summary description for log
    /// </summary>
    public class log : IHttpHandler
    {
        public static Queue<Int32> ClickQueue = new Queue<int>();
        public static Queue<Int32> ImpressionQueue = new Queue<int>(); 
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request.QueryString["type"] ?? string.Empty;
            int itemId = Lib.Object2Integer(context.Request.QueryString["itemId"]);
            string clickLink = context.Request.QueryString["nextUrl"] ?? string.Empty;
            switch (type)
            {
                case "impression":
                    var itemIds = context.Request.QueryString["itemIds"] ?? string.Empty;
                    if (itemIds.Length > 0)
                    {
                        foreach (string s in itemIds.Split(','))
                        {
                            ImpressionQueue.Enqueue(Lib.Object2Integer(s));
                        }
                    }
                    
                    break;
                case "click":
                    ClickQueue.Enqueue(itemId);
                    if (!string.IsNullOrEmpty(clickLink))
                        context.Response.Redirect(clickLink);
                    break;
                default:
                    break;
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