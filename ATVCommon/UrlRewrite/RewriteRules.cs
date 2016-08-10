using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace ATVCommon.UrlRewrite
{
    public class RewriteRules : CollectionBase
    {
        public static RewriteRules GetCurrentRewriteRules()
        {
            string cacheName = "CommonConfiguration_RewriteRules_TTVH";
            if (null != HttpContext.Current.Cache[cacheName])
            {
                try
                {
                    return (RewriteRules)HttpContext.Current.Cache[cacheName];
                }
                catch
                {
                    return new RewriteRules();
                }
            }
            else
            {
                try
                {
                    string configFilePath = HttpContext.Current.Server.MapPath("/Config/RewriteRules.config"); //@"D:\Running projects\VC Corporation\Dantri\Dantri.Cached\CacheSettings.config";
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(configFilePath);

                    RewriteRules rules = new RewriteRules();

                    XmlNodeList nlstRules = xmlDoc.DocumentElement.SelectNodes("//rules/rule");

                    for (int i = 0; i < nlstRules.Count; i++)
                    {
                        RewriteRule rule = new RewriteRule();
                        rule.Url = nlstRules[i].SelectSingleNode("url").InnerText;
                        rule.Rewrite = nlstRules[i].SelectSingleNode("rewrite").InnerText;

                        rules.List.Add(rule);
                    }

                    XmlNode nodeFileSettingCacheExpire = xmlDoc.DocumentElement.SelectSingleNode("//Configuration/RewriteRulesFile");
                    long fileSettingCacheExpire = Lib.Object2Long(nodeFileSettingCacheExpire.Attributes["cacheExpire"].Value);
                    if (fileSettingCacheExpire <= 0)
                    {
                        fileSettingCacheExpire = 3600;// default 1h
                    }

                    CacheDependency fileDependency = new CacheDependency(configFilePath);
                    HttpContext.Current.Cache.Insert(cacheName, rules, fileDependency, DateTime.Now.AddSeconds(fileSettingCacheExpire), TimeSpan.Zero, CacheItemPriority.Normal, null);

                    return rules;
                }
                catch
                {
                    return new RewriteRules();
                }
            }
        }

        public string GetMatchingRewrite(string url)
        {
            Regex rex;

            for (int i = 0; i < List.Count;i++ )
            {
                RewriteRule rule = (RewriteRule)List[i];
                rex = new Regex(rule.Url, RegexOptions.IgnoreCase);
                Match match = rex.Match(url);

                if (match.Success)
                {
                    return rex.Replace(url, rule.Rewrite);
                }

            }

            return string.Empty;
        }

        public RewriteRule GetMatchingRule(string url)
        {
            Regex rex;

            for (int i = 0; i < List.Count; i++)
            {
                RewriteRule rule = (RewriteRule)List[i];
                rex = new Regex(rule.Url, RegexOptions.IgnoreCase);
                Match match = rex.Match(url);

                if (match.Success)
                {
                    return rule;
                }

            }

            return new RewriteRule();
        }
        /*
        <rule name="Danh sách tin (Trang đầu tiên)">
			<url>/([0-9]+)CT([0-9]+)/([^/]+).htm</url>
			<rewrite>/List.aspx?CatParentID=$2&amp;CatID=$1&amp;CatName=$3&amp;PageIndex=1&amp;PageType=2</rewrite>
		</rule>
         */
        public RewriteRule GetListPageRule(bool havePaging)
        {
            for (int i = 0; i < List.Count; i++)
            {
                RewriteRule rule = (RewriteRule)List[i];
                rule.Rewrite = HttpUtility.HtmlDecode(rule.Rewrite);
                if (rule.Rewrite.Contains("/List.aspx") && rule.Rewrite.Contains("/List.aspx?CatParentID=$2&CatID=$1&CatName=$3&PageIndex=1&PageType=2"))
                {
                    rule.Rewrite = rule.Rewrite.Replace("$1", "{0}").Replace("$2", "{1}").Replace("&CatName=$3", "");
                    return rule;
                }
            }

            return new RewriteRule();
        }
        //Pages/Thread.aspx?PageType=2&amp;threadid=$1&amp;pageindex=$3&amp;cat_parentid=1000&amp;cat_id=0

        public RewriteRule GetRuleEvent(string EventId)
        {
            for (int i = 0; i < List.Count; i++)
            {
                RewriteRule rule = (RewriteRule)List[i];
                rule.Rewrite = HttpUtility.HtmlDecode(rule.Rewrite);
                if (rule.Rewrite.Contains("/Pages/Thread.aspx?PageType=2&threadid=$1") && rule.Rewrite.Contains("cat_parentid=1000&cat_id=0"))
                {
                    rule.Rewrite = rule.Rewrite.Replace("$1", "{0}").Replace("&pageindex=$3", "");
                    return rule;
                }
            }

            return new RewriteRule();
        }
        //temp: ([0-9]+)N([0-9]+)T([0-9]+)/([^/]+).htm; 
        //url: Details.aspx?CatID=$1&amp;NewsID=$2&amp;CatParentID=$3&amp;Type=$3&amp;PageType=2&amp;NewsUrl=$4
        public RewriteRule GetDetailPageRule()
        {
            for (int i = 0; i < List.Count; i++)
            {
                RewriteRule rule = (RewriteRule)List[i];
                rule.Rewrite = HttpUtility.HtmlDecode(rule.Rewrite);
                if (rule.Rewrite.Contains("/Details.aspx?CatID=$1&NewsID=$2&CatParentID=$3&Type=$3&PageType=2&NewsUrl=$4"))
                {
                    rule.Rewrite =
                        rule.Rewrite.Replace("$1", "{0}").Replace("$2", "{1}").Replace("$3", "{2}").Replace(
                            "&NewsUrl=$4", "");
                    return rule;
                }
            }

            return new RewriteRule();
        }
        //dong sự kiện: /ListNewsByEvent.aspx?CatParentID=$3&amp;CatID=$1&amp;PageIndex=1&amp;PageType=2&amp;ThreadID=$2
        public RewriteRule GetThreadPageRule()
        {
            for (int i = 0; i < List.Count; i++)
            {
                RewriteRule rule = (RewriteRule)List[i];
                rule.Rewrite = HttpUtility.HtmlDecode(rule.Rewrite);
                if (rule.Rewrite.Contains("/ListNewsByEvent.aspx?CatParentID=$3&CatID=$1&PageIndex=1&PageType=2&ThreadID=$2"))
                {
                    rule.Rewrite = rule.Rewrite.Replace("$1", "{0}").Replace("$2", "{1}").Replace("$3", "{2}");
                    return rule;
                }
            }

            return new RewriteRule();
        }
        public RewriteRule this[int index]
        {
            get
            {
                return (RewriteRule)List[index];
            }
        }

        public struct RewriteRule
        {
            public RewriteRule(string url, string rewrite)
            {
                this.Url = url;
                this.Rewrite = rewrite;
            }
            public string Url, Rewrite;
        }
    }
}
