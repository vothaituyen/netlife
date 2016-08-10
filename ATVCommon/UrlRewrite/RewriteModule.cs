using System;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

namespace ATVCommon.UrlRewrite
{
    public class RewriteModule : IHttpHandlerFactory, IRequiresSessionState
    {
         

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url1, string pathTranslated)
        {
            string url = context.Request.Url.AbsolutePath;
            string keycach = context.Request.RawUrl.ToString();
            string html = context.Cache[keycach] != null ? context.Cache[keycach].ToString() : String.Empty;
            string newFilePath;
            if (!String.IsNullOrEmpty(html))
            {
                context.Response.Write(html);
                newFilePath = "/blank.aspx";
                return PageParser.GetCompiledPageInstance(newFilePath, context.Server.MapPath(newFilePath), context);
            }

            string rewrite = "";

            if (HttpContext.Current.Request.Url.Query != String.Empty)
            {
                if (context.Request.Url.Query.Length > 0)
                {
                    context.Items["VirtualUrl"] = context.Request.Path + context.Request.Url.Query;
                }
            }
            if (context.Items["VirtualUrl"] == null)
            {
                context.Items["VirtualUrl"] = context.Request.Path;
            }

            RewriteRules rewriteRules = RewriteRules.GetCurrentRewriteRules();
            rewrite = rewriteRules.GetMatchingRewrite(url);

            
            if (!string.IsNullOrEmpty(rewrite))
            {
                context.RewritePath("~" + rewrite);
            }
            else
            {
                {
                    rewrite = context.Request.Path + context.Request.Url.Query;
                }
            }

            newFilePath = rewrite != null && rewrite.IndexOf("?") > 0 ? rewrite.Substring(0, rewrite.IndexOf("?")) : rewrite;
            
            if (string.IsNullOrEmpty(newFilePath))
                newFilePath = "/blank.aspx";

            try
            {
                return PageParser.GetCompiledPageInstance(newFilePath, context.Server.MapPath(newFilePath), context);
            }
            catch (Exception ex)
            {
                return PageParser.GetCompiledPageInstance("/404.aspx", context.Server.MapPath("/blank.aspx"), context);
            }
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
         
    }
}
