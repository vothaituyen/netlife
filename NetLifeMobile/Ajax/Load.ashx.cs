using ATVCommon;
using BOATV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace NetLifeMobile.Ajax
{
    /// <summary>
    /// Summary description for Load
    /// </summary>
    public class Load : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        [WebMethod]
        public static UserDetails[] BindDatatable(int count)
        {
            List<UserDetails> details = new List<UserDetails>();
            var dt = NewsPublished.Danh_Sach_Tin_Theo_Cat(Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, count, Lib.QueryString.PageIndex, 213, "");
            foreach (var a in dt)
            {
                UserDetails user = new UserDetails();
                user.Img = a.URL_IMG;
                user.Title = a.NEWS_TITLE;
                user.Url = a.URL;
                user.Date = a.NEWS_PUBLISHDATE;
                user.subtitle = a.NEWS_INITCONTENT;
                details.Add(user);
            }
            return details.ToArray();
        }

        public class UserDetails
        {
            public string Img { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public DateTime Date { get; set; }
            public string subtitle { get; set; }
        }

    }
}