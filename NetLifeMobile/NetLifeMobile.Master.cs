using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BOATV;

namespace NetLifeMobile
{
    public partial class NetLifeMobile : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.form1.Action = Request.RawUrl;
            if (Request.Url.DnsSafeHost.ToLower().IndexOf("netlife.com.vn", System.StringComparison.Ordinal) != -1)
            {
                Utils.Move301("http://m.netlife.vn" + Request.RawUrl);
                return;
            }
            Utils.SetCanonicalLink(this.Page, "http://m.netlife.vn" + Request.RawUrl);
        }
    }
}