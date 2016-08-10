using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BOATV;

namespace NetLife.web.Pages
{
    public partial class Move301 : System.Web.UI.Page
    {
        public static string DecodeForUrl(string text)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(text.Replace("-", "+").Replace("_", "/")));
            }
            catch (Exception)
            {
                return "";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["type"] ?? string.Empty;
            if (type.Equals("tags") && Request.QueryString["key"] != null)
            {
                Response.RedirectPermanent(String.Format("/tag/{0}.html", HttpUtility.UrlEncode(DecodeForUrl(Request.QueryString["key"].ToString()))));
            }
            if (Request.QueryString["newsId"] != null)
            {
                var newsId = Utils.GetObj<Int64>(Request.QueryString["newsId"]);
                var newsObject = NewsPublished.NP_TinChiTiet(newsId, false);
                Response.RedirectPermanent(newsObject != null ? newsObject.URL : "/");
            }
            Response.RedirectPermanent("/");
        }
    }
}