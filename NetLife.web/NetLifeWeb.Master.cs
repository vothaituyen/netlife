using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BOATV;

namespace NetLife.web
{
    public partial class NetLifeWeb : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["desktopview"] != null && Request.QueryString["desktopview"].ToString().ToLower().Equals("true"))
            {
                Session["desktopView"] = 1;
            }
            else if (Request.QueryString["desktopview"] != null &&
                     Request.QueryString["desktopview"].ToString().ToLower().Equals("false"))
            {
                Session.Remove("desktopView");
                HttpContext.Current.Response.Redirect("http://m.netlife.vn");
            }

            string urlMobile = HttpContext.Current.Request.RawUrl.ToString().ToLower();
            if (Session["desktopView"] == null && Utils.isMobileBrowser(HttpContext.Current))
            {
                if (urlMobile.Contains("/default.aspx"))
                    HttpContext.Current.Response.Redirect("http://m.netlife.vn");
                else
                    HttpContext.Current.Response.Redirect("http://m.netlife.vn" + urlMobile);
            }



            this.form1.Action = Request.RawUrl;
            if (Request.Url.DnsSafeHost.ToLower().IndexOf("www.", System.StringComparison.Ordinal) != -1
                 || Request.Url.DnsSafeHost.ToLower().IndexOf("netlife.com.vn", System.StringComparison.Ordinal) != -1)
            {
                Utils.Move301("http://netlife.vn" + Request.RawUrl);
                return;
            }
            Utils.SetCanonicalLink(this.Page, "http://netlife.vn" + Request.RawUrl);
        }


    }
}