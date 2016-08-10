using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace ATVCommon.Cached.UrlRewrite
{
    public class _301Redirection
    {
        #region Config file object
        private List<RedirectRule> RedirectRules;
        public struct RedirectRule
        {
            public RedirectRule(string url, string parameters, string method)
            {
                this.Url = url;
                this.Parameters = parameters;
                this.Method = method;
            }
            public string Url, Parameters, Method;
        }

        public _301Redirection()
        {
            string cacheName = "TTOL_Cache_OldUrlRedirectionRules";
            RedirectRules = HttpContext.Current.Cache[cacheName] as List<RedirectRule>;
            if (null == RedirectRules)
            {
                RedirectRules = new List<RedirectRule>();

                try
                {
                    string configFilePath = HttpContext.Current.Server.MapPath("/Config/OldRewriteRules.config");
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(configFilePath);

                    XmlNodeList nlstRules = xmlDoc.DocumentElement.SelectNodes("//rules/rule");

                    for (int i = 0; i < nlstRules.Count; i++)
                    {
                        RedirectRule rule = new RedirectRule();
                        rule.Url = nlstRules[i].SelectSingleNode("url").InnerText;
                        rule.Parameters = nlstRules[i].SelectSingleNode("params").InnerText;
                        rule.Method = nlstRules[i].SelectSingleNode("method").InnerText;

                        RedirectRules.Add(rule);
                    }

                    XmlNode nodeFileSettingCacheExpire = xmlDoc.DocumentElement.SelectSingleNode("//Configuration/RedirectRulesFile");
                    long fileSettingCacheExpire = Lib.Object2Long(nodeFileSettingCacheExpire.Attributes["cacheExpire"].Value);
                    if (fileSettingCacheExpire <= 0)
                    {
                        fileSettingCacheExpire = 3600;// default 1h
                    }

                    CacheDependency fileDependency = new CacheDependency(configFilePath);
                    HttpContext.Current.Cache.Insert(cacheName, RedirectRules, fileDependency, DateTime.Now.AddSeconds(fileSettingCacheExpire), TimeSpan.Zero, CacheItemPriority.Normal, null);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public bool Redirect(string currentUrl)
        {
            bool mustRedirect = false;

            Regex rex;

            for (int i = 0; i < RedirectRules.Count; i++)
            {
                RedirectRule rule = RedirectRules[i];
                rex = new Regex(rule.Url, RegexOptions.IgnoreCase);
                Match match = rex.Match(currentUrl);

                if (match.Success)
                {
                    string parameters = rex.Replace(currentUrl, rule.Parameters);
                    mustRedirect = (bool)this.GetType().InvokeMember(rule.Method, System.Reflection.BindingFlags.InvokeMethod, null, this, new object[] { parameters.Split(',') });
                    break;
                }

            }

            return mustRedirect;
        }
        #endregion

        #region Redirect methods
        public static bool Redirect_Channel(string[] parameters)
        {
            return RedirectTo(parameters[0].ToString());
        }

        public static bool Redirect_ToList(string[] parameters)
        {
            string url = "/vn/{0}/index.html";
            string newsUrl = "";
            try
            {
                string catName = parameters[0];
                newsUrl = String.Format(url, catName);
            }
            catch
            {

            }
            return RedirectTo(newsUrl);
        }

        #region NewsDetail (without title)
        /// <summary>
        /// NewsDetail
        /// <list type="OldUrl">http://kenh14.vn/home/{NewsID}_tm,{ParentCatID}cat{CatID}/{Title}.chn</list>
        /// <list type="NewUrl">http://kenh14.vn/{NewsID}_tm,{ParentCatID}cat{CatID}/{Title}.chn</list>
        /// </summary>
        /// <param name="parameters">List of parameters in old url</param>
        public static bool Redirect_NewsDetail(string[] parameters)
        {
            string newUrlFormat = "/{0}_tm,{1}cat{2}/{3}.chn";
            string newUrl = "";

            #region Get new url

            try
            {
                string parentCatID = parameters[1];
                string catID = parameters[2];
                string newsID = parameters[0];
                string newsTitle = parameters[3];
                newUrl = string.Format(newUrlFormat, newsID, parentCatID, catID, newsTitle);
            }
            catch (Exception ex)
            {
            }

            #endregion

            return RedirectTo(newUrl);
        }
        #endregion

        #region Private methods
        public static bool RedirectTo(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.BufferOutput = true;
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location",  (url.StartsWith("/") ? "" : "/") + url);
                HttpContext.Current.Response.End();

                return true;
            }

            return false;
        }
        #endregion
        #endregion
    }
}
