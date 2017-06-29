using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using BOATV;

namespace NetLifeMobile.Controls.Advs
{
    public partial class Ads : System.Web.UI.UserControl
    {
        int postId = 0;
        string css = "row padbt10 fl";
        int catid = Lib.QueryString.ParentCategoryID == 0 ? Lib.QueryString.CategoryID : Lib.QueryString.ParentCategoryID;
        public int CatId { set { catid = value; } }
        public int PositionId { set { postId = value; } }
        public string ClassName { set { css = value; } }

        public bool IsRotateOnly
        {
            get { return isRotateOnly; }
            set { isRotateOnly = value; }
        }


        private bool isRotateOnly = true;

        private bool lazyLoad = ConfigurationManager.AppSettings["lazyload"] != null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!lazyLoad)
            //{
            //    string html = BOAdv.GetAdvItemById(Lib.Object2Integer(postId), Lib.Object2Integer(catid));
            //    if (!String.IsNullOrWhiteSpace(html))
            //    {
            //        ltrContent.Text = String.Format("<div class=\"{0}\"><script>", css) + String.Format("var zone{0}_{2} = new RunBanner({1}, \"zone{0}_{2}_Adv\"); zone{0}_{2}.Show();", catid, html.Replace("\\n", " ").Replace("\\t", " "), postId).Replace("INSERT_RANDOM_NUMBER_HERE", DateTime.Now.ToFileTime().ToString()) + "</script></div>";
            //    }
            //}
            //else
            //    ltrContent.Text = String.Format("<div class=\"{2}\" id=\"zone_{0}_{1}\"><script type=\"text/javascript\">$(document).ready(function () {{vmcLoadScript(\"#zone_{0}_{1}\", \"/Pages/Ads/Show.ashx?catId={0}&amp;zoneId={1}\");}})</script></div>", catid, postId, css);
        }

    }
}