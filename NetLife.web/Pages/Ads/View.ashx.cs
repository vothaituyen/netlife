using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Messaging;
using System.Web;
using NC.Ads.BO;

namespace VMCAds.Dout
{
    /// <summary>
    /// Summary description for View
    /// </summary>
    public class View : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        private bool _isClick = ConfigurationManager.AppSettings["click"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["click"]);
        public static Queue<int> ListItemId = new Queue<int>();
        public static Queue<QuangCao_Item_Preroll> prerollQueue = new Queue<QuangCao_Item_Preroll>();

        //Auto Click
        public static Queue<Ads_Item_Click> AClick = new Queue<Ads_Item_Click>();

        public static string LogMSMQ = ConfigurationManager.AppSettings["LogMSMQ"] ?? ".\\private$\\adv";
        public static bool messageQueue = ConfigurationManager.AppSettings["MessageQueue"] != null && ConfigurationManager.AppSettings["MessageQueue"].ToString().ToUpper().Equals("TRUE");

        private string ClientIP()
        {
            HttpRequest currentRequest = HttpContext.Current.Request;
            string ipAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (ipAddress == null || ipAddress.ToLower() == "unknown")
                ipAddress = currentRequest.ServerVariables["REMOTE_ADDR"];
            return ipAddress;
        }

        /// <summary>
        /// Neu 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {

            #region Tracking Impression cho Quảng cáo
            if (context.Request.QueryString["itemIds"] != null && !String.IsNullOrEmpty(context.Request.QueryString["itemIds"].ToString()))
            {
                if (messageQueue)
                {
                    if (!String.IsNullOrEmpty(context.Request.QueryString["itemIds"]))
                    {
                        var msg = new Ads_MMSQ
                        {
                            ListItemIds = context.Request.QueryString["itemIds"].ToString(CultureInfo.InvariantCulture),
                            ViewDate = DateTime.Now,
                            IPAddress = ClientIP(),
                            ReferUrl = context.Request.QueryString["location"] ?? string.Empty,
                            SessionID = context.Session.SessionID.ToString(CultureInfo.InvariantCulture),
                            TrueImpression = context.Request.QueryString["trueImpression"] != null && context.Request.QueryString["trueImpression"].ToString().Equals("1") ? "1" : "0"
                        };
                        if (!context.Request.IsLocal)
                        (new MessageQueue(LogMSMQ)).Send(msg);
                    }

                }
                else
                {
                    int key = 0;
                    var itemId = context.Request.QueryString["itemIds"].ToString().Split(',');
                    foreach (string id in itemId)
                    {
                        key = Utils.Object2Integer(id);
                        if (key > 0)
                            ListItemId.Enqueue(key);
                    }
                }
            }

            // truong hop Auto
            if (context.Request.QueryString["type"] != null && context.Request.QueryString["type"].ToString().Equals("1"))
                ProcessRequestClick(context);
            #endregion

            #region Tracking TrueImpression
            if (context.Request.QueryString["trueImpression"] != null && !String.IsNullOrEmpty(context.Request.QueryString["trueImpression"].ToString()))
            {

            }

            #endregion

            #region Tracking Event cho Video Preroll
            if (context.Request.QueryString["event"] != null && context.Request.QueryString["itemId"] != null)
            {
                string strEvent = context.Request.QueryString["event"] ?? string.Empty;

                if (context.Request.QueryString["itemId"] != null && !String.IsNullOrEmpty(context.Request.QueryString["itemId"].ToString()))
                {
                    var pre = new QuangCao_Item_Preroll { AdvID = Convert.ToInt32(context.Request.QueryString["itemId"]), Domain = context.Request.QueryString["domain"] };
                    switch (strEvent)
                    {
                        case "firstQuartile":
                            pre.FirstQuartile = 1;
                            break;
                        case "midpoint":
                            pre.Midpoint = 1;
                            break;
                        case "thirdQuartile":
                            pre.ThirdQuartile = 1;
                            break;
                        case "complete":
                            pre.Complete = 1;
                            break;
                        default:
                            break;
                    }

                    prerollQueue.Enqueue(pre);
                }
                return;
            }
            #endregion

            #region Tracking View của bài viết gắn trên trang
            //Nếu tất cả các bài viết cần đo view thì sẽ sử dụng cái này để đo
            //http://tracking.vietnamnetad.vn/Dout/View.ashx?itemIds=123&detail=1
            if (context.Request.QueryString["itemId"] != null && context.Request.QueryString["detail"] != null && context.Request.QueryString["detail"].ToString().Equals("1"))
            {
                var ads = new Ads_Items { AdvID = Convert.ToInt32(context.Request.QueryString["itemId"]) };
                string key = String.Format("Bai-chi-tiet-{0}", ads.AdvID);
                var cacheItem = HttpContext.Current.Cache[key] as Ads_Items;
                if (cacheItem == null)
                {
                    cacheItem = ads.SelectOne();
                    Utils.SaveToCacheDependency(Constants.DATABASE_NAME, Constants.QUANGCAO_ITEM, key, cacheItem);
                }
                context.Response.Redirect(cacheItem.Url);
                return;

            }
            #endregion

        }


        public static Dictionary<string, Queue<DataRow>> _dictionary = null;
        private static DateTime _lastTimeClearQueue = DateTime.Now.AddHours(1);

        public static Dictionary<string, string> categoryDic = new Dictionary<string, string>();
        public void ProcessRequestClick(HttpContext context)
        {

            string category = context.Request.QueryString["domain"] ?? string.Empty;
            if (String.IsNullOrEmpty(category) || context.Request.UrlReferrer == null) return;


            category = context.Request.QueryString["domain"].ToString().Trim();

            if (!categoryDic.ContainsKey(category) && category.IndexOf("http://vietnamnet.vn/") != -1) categoryDic.Add(category, category);

            if (category.Equals("http://vietnamnet.vn/home"))
            {
                category = "/";
            }
            else if (category.StartsWith("http://vietnamnet.vn"))
            {
                category = category.Replace("http://vietnamnet.", "") + "/";
            }

            string time = string.Empty;

            if (context.Request.UrlReferrer != null)
            {
                //Mỗi tiếng reset lại Queue 1 lần để chạy cho nó mượt
                if (_lastTimeClearQueue < DateTime.Now)
                {
                    _dictionary = null;
                    _lastTimeClearQueue = DateTime.Now.AddHours(1);
                }

                if (_dictionary == null)
                {
                    _dictionary = new Dictionary<string, Queue<DataRow>>();
                    var activeAds = (new QuangCaoItem()).GetActiveBanner();
                    if (activeAds != null && activeAds.Rows.Count > 0)
                    {

                        foreach (DataRow t in activeAds.Rows)
                        {
                            var fTime = ClickTimeline.DoClickTimeLineMinute(Convert.ToDateTime(t["StartDate"]), Convert.ToDateTime(t["EndDate"]).AddDays(1), Convert.ToInt32(t["MaxClickPerday"]), true);
                            if (t["Cat_DisplayUrl"] != null && !String.IsNullOrWhiteSpace(t["Cat_DisplayUrl"].ToString()))
                                category = t["Cat_DisplayUrl"].ToString().Trim();

                            foreach (var k in fTime)
                            {
                                if (k.Key >= DateTime.Now && k.Value > 0 && k.Key <= DateTime.Now.AddHours(2))
                                {
                                    time = k.Key.ToString("ddMMHHmm") + Utils.GetRandomCategory(new List<string>() { category });
                                    if (_dictionary.ContainsKey(time))
                                        _dictionary[time].Enqueue(t);
                                    else
                                    {
                                        var que = new Queue<DataRow>();
                                        que.Enqueue(t);
                                        _dictionary.Add(time, que);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    time = DateTime.Now.ToString("ddMMHHmm") + category;

                    if (_dictionary.ContainsKey(time))
                    {
                        var lastItem = _dictionary[time];
                        if (lastItem != null && lastItem.Count > 0)
                        {
                            var qItem = lastItem.Dequeue();

                            // Bổ xung click thật vào danh sách
                            int id = Utils.Object2Integer(qItem["ID"]);
                            Click.ListItemId.Enqueue(id);

                            // Công vao Auto
                            var click = new Ads_Item_Click { AdvID = id, ClickDate = DateTime.Now, ReferUrl = context.Request.UrlReferrer.ToString() ?? String.Empty, ClickIP = context.Request.UserHostAddress };
                            AClick.Enqueue(click);

                            if (qItem["TargetUrl"] != null)
                                context.Response.Write(String.Format(ScriptClient, qItem["TargetUrl"].ToString()));
                        }
                        else
                        {
                            _dictionary.Remove(time);
                            context.Response.Write("document.write('')");

                        }
                    }
                    else
                    {
                        context.Response.Write("document.write('');");
                    }

                }
            }

            //context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(30));
            context.Response.ContentType = "text/javascript";
            //context.Response.Cache.SetCacheability(HttpCacheability.Public);

        }

        /// <summary>
        /// 
        /// </summary>
        private const string ScriptClient = "if (document.createElement) {{var iframeAds = document.createElement('iframe');  iframeAds.height = \"0\" ;  iframeAds.width = \"0\" ;  iframeAds.style.display = \"none\" ;  iframeAds.src = \"{0}\";  if (document.body.appendChild) {{document.body.appendChild(iframeAds);}}}} ";





        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}