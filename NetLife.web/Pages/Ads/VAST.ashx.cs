using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using NC.Ads.BO;

namespace VMCAds.Dout
{
    /// <summary>
    /// Summary description for VAST
    /// </summary>
    public class VAST : IHttpHandler
    {

        public static Dictionary<int, Dictionary<int, int>> AllPrerollItem = new Dictionary<int, Dictionary<int, int>>(); 
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            context.Response.Expires = -1;

            string zoneId = context.Request.QueryString["zoneId"] ?? String.Empty;
            if (!String.IsNullOrEmpty(zoneId))
            {
                var result = Lib.getExt(context);
                if (3 == result.Length)
                {
                    var item = new QuangCaoItem();
                    if (result[1].Length < 4)
                    {
                        result[1] = String.Empty;
                    }
                    else
                    {
                        if (context.Request.QueryString["domain"] != null)
                        {
                            result[1] = context.Request.QueryString["domain"].ToString().Trim();
                            if (result[1].Length > 0)
                                result[1] = result[1].TrimEnd("/".ToCharArray());

                            if (result[1].Equals("http://vietnamnet.vn/home") || result[1].Equals("http://vietnamnet.vn/"))
                            {
                                result[1] = "/";
                            }
                            else if (result[1].StartsWith("http://vietnamnet.vn"))
                            {

                                result[1] = result[1].Replace("http://vietnamnet.", "") + "/";
                            }
                        }
                        else
                        {
                            var mS = result[1].Split('/');
                            if (mS.Length > 2)
                            {
                                result[1] = String.Join("/", mS, 0, 2);

                                if (!result[1].StartsWith("/")) result[1] = "/" + result[1];
                            }
                            else
                            {
                                result[1] = "/";
                            }
                        }

                    }

                    //Trả lại dữ liệu dạng VAST.

                    var db = item.VASTByZoneId(result[0], result[1]);

                    string cacheCount = String.Format("VASTByZoneId-Count-{0}-{1}", result[0], result[1]);

                    int resultCount = (HttpContext.Current.Cache[cacheCount] != null) ? Convert.ToInt32(HttpContext.Current.Cache[cacheCount]) : 0;

                    if (db != null && db.Count > 0)
                    {
                        // context.Response.Redirect(db[resultCount] + "&d=" + System.Guid.NewGuid().ToString());
                        // context.Response.Write(db[resultCount]);


                        var vlCookie = context.Request.Cookies["cmp_l"] == null ? string.Empty : context.Request.Cookies["cmp_l"].Value;
                        var viewList = new List<int>(); // AdvItemId
                        if (!string.IsNullOrEmpty(vlCookie) && vlCookie.EndsWith("."))
                        {
                            var tempArray = vlCookie.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < tempArray.Length; i++)
                            {
                                viewList.Add(Utils.Object2Integer(tempArray[i]));
                            }
                        }
                        else
                            vlCookie = string.Empty;

                        var vdCookie = context.Request.Cookies["cmp_d"] == null ? string.Empty : context.Request.Cookies["cmp_d"].Value;
                        var viewDic = new Dictionary<int, int>();
                        if (!string.IsNullOrEmpty(vdCookie) && vdCookie.EndsWith("."))
                        {
                            var tempArray = vdCookie.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < tempArray.Length; i++)
                            {
                                var dicItem = tempArray[i].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                viewDic.Add(Utils.Object2Integer(dicItem[0]), Utils.Object2Integer(dicItem[1]));
                            }
                        }

                        var lstAdv = new List<Ads_Items>();
                        var lstInts = new Dictionary<int, int>();
                        foreach (var matchedAdv in db)
                        {
                            if (matchedAdv.Value != null && (!viewDic.ContainsKey(matchedAdv.Key) || matchedAdv.Value.Frequency == 0 || (viewDic.ContainsKey(matchedAdv.Key) && viewDic[matchedAdv.Key] < matchedAdv.Value.Frequency))) 
                            {
                                if (matchedAdv.Value.CPM == 0 || AllPrerollItem.ContainsKey(matchedAdv.Key) &&
                                    matchedAdv.Value.CpmByHour.ContainsKey(DateTime.Now.Hour) &&
                                    matchedAdv.Value.CpmByHour[DateTime.Now.Hour] >= AllPrerollItem[matchedAdv.Key][DateTime.Now.Hour])
                                {
                                    lstAdv.Add(matchedAdv.Value);
                                    lstInts[lstInts.Count] = matchedAdv.Key;    
                                }
                                
                            }

                        }

                        resultCount++;

                        if (resultCount >= lstAdv.Count)
                            resultCount = 0;

                        if (lstAdv.Count > 0)
                        {
                            var url = lstAdv[resultCount].Url;

                            HttpContext.Current.Cache.Insert(cacheCount, resultCount, null, DateTime.Now.AddMinutes(15), TimeSpan.Zero);
                            if (!String.IsNullOrEmpty(url))
                            {

                                context.Response.Write(lstAdv[resultCount].Url);

                                // cookie list 
                                int advId = lstInts[resultCount];
                                vlCookie = vlCookie.Replace(advId + ".", "");
                                vlCookie = advId + "." + vlCookie;
                                // cookie dictionary
                                if (viewDic.ContainsKey(advId))
                                    viewDic[advId] += 1;
                                else
                                    viewDic.Add(advId, 1);

                                var cookie = new HttpCookie("cmp_l")
                                {
                                    Value = vlCookie,
                                    Domain = ".vietnamnetad.vn",
                                    Expires = DateTime.Now.AddYears(1)
                                };

                                context.Response.Cookies.Set(cookie);

                                vdCookie = viewDic.Aggregate(string.Empty, (current, a) => current + (a.Key + "," + a.Value + "."));

                                var cookie2 = new HttpCookie("cmp_d")
                                {
                                    Value = vdCookie,
                                    Domain = ".vietnamnetad.vn",
                                    Expires = DateTime.Now.AddYears(1)
                                };

                                context.Response.Cookies.Set(cookie2);
                            }

                            else
                            {
                                context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><VAST version=\"2.0\"/>");
                            }
                        }
                        else
                        {
                            context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><VAST version=\"2.0\"/>");
                        }
                    }
                    else
                    {
                        context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><VAST version=\"2.0\"/>");
                    }

                }
            }
            else
            {
                context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><VAST version=\"2.0\"/>");
            }
            return;
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}