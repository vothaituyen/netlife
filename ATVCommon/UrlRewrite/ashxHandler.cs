using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

namespace ATVCommon.UrlRewrite
{
    public class ashxHandler : IHttpHandlerFactory, IRequiresSessionState
    {

        public System.Web.IHttpHandler GetHandler(HttpContext context, string requestType, string url1, string pathTranslated)
        {
            string url = context.Request.RawUrl.ToString();
            if (url.IndexOf("?") > 0) url = url.Substring(0, url.IndexOf("?"));
            string fileName = url.Substring(0, url.IndexOf("."));
            string newFilePath = String.Format("{0}.ashx", fileName);
            return PageParser.GetCompiledPageInstance(newFilePath, context.Server.MapPath(newFilePath), context);
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
