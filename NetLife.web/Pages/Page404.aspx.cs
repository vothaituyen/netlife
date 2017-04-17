using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NetLife.web.Pages
{
    public partial class Page404 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 404;
            Response.Status = "404 Not Found";
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}