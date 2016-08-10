using System;
using System.Collections.Generic;

using System.Web;
using System.Data;
using System.Web.UI;
using ATVCommon;
using ATVEntity;
using BOATV;

namespace NetLife.Web.Rss
{
    public class Rss_Content : Page
    {
        // Fields
        private int CatID = 0;
        private int CatParentID = 0;
        private int ImageWidth = 130;
        private string siteUrl = "http://netlife.vn";
        private const int TOP = 20;

        // Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url.DnsSafeHost.IndexOf("netlife.vn", System.StringComparison.Ordinal) != -1)
            {
                Utils.Move301("http://netlife.vn" + Request.RawUrl);
                return;
            }

            if (!base.IsPostBack)
            {
                DataTable table;
                DataRow row;
                int num2;
                RssHelper.RSSItem item;
                RssHelper.RssChannel channel = new RssHelper.RssChannel();
                channel.ttl = 10;
                channel.copyright = "NetLife.vn";
                channel.pubDate = DateTime.Now;
                channel.generator = "NetLife.vn";
                channel.docs = this.siteUrl;
                this.CatID = Lib.QueryString.CategoryID;
                this.CatParentID = Convert.ToInt32(Lib.QueryString.ParentCategoryID);

                {
                    CategoryEntity categoryById = BOCategory.GetCategory(this.CatID);
                    if (categoryById != null)
                    {
                        channel.description = categoryById.Cat_Description;
                        channel.title = categoryById.Cat_Name + " | RSS Feed | NetLife.vn";
                        channel.link = this.siteUrl.TrimEnd(new char[] { '/' }) + categoryById.HREF.Trim();

                        var list = NewsPublished.NP_Danh_Sach_Tin(categoryById.Cat_ParentID, categoryById.Cat_ID, 10, 1, this.ImageWidth);
                        if ((list != null) && (list.Count > 0))
                        {
                            for (num2 = 0; num2 < list.Count; num2++)
                            {
                                var row1 = list[num2];
                                item = new RssHelper.RSSItem();
                                item.title = HttpUtility.HtmlDecode(row1.NEWS_TITLE);
                                item.link = this.siteUrl.TrimEnd(new char[] { '/' }) + row1.URL;
                                item.guid = item.link;
                                item.description = row1.NEWS_INITCONTENT;
                                item.pubDate = row1.NEWS_PUBLISHDATE;
                                channel.items.Add(item);
                            }
                        }
                    }
                }
                base.Response.ContentType = "text/xml";
                base.Response.Write(channel.ToString());
            }
        }
    }


}