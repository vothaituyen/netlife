using System;
using System.Collections.Generic;
using System.Text;

namespace BOATV
{
    public static class RssHelper
    {
        public class RssChannel
        {
            private string m_title = string.Empty;
            private string m_link = string.Empty;
            private string m_description = string.Empty;
            private int m_ttl = 10;
            private string m_copyright = string.Empty;
            private DateTime m_pubDate = DateTime.MinValue;
            private string m_generator = string.Empty;
            private string m_docs = string.Empty;
            private string m_image_title = string.Empty;
            private string m_image_url = string.Empty;
            private string m_image_link = string.Empty;
            private int m_image_width = 0;
            private int m_image_height = 0;
            private RssItemCollection m_items = new RssItemCollection();

            public string title
            {
                get { return this.m_title; }
                set { this.m_title = value; }
            }
            public string link
            {
                get { return this.m_link; }
                set { this.m_link = value; }
            }
            public string description
            {
                get { return this.m_description; }
                set { this.m_description = value; }
            }
            public int ttl
            {
                get { return this.m_ttl; }
                set { this.m_ttl = value; }
            }
            public string copyright
            {
                get { return this.m_copyright; }
                set { this.m_copyright = value; }
            }
            public DateTime pubDate
            {
                get { return this.m_pubDate; }
                set { this.m_pubDate = value; }
            }
            public string generator
            {
                get { return this.m_generator; }
                set { this.m_generator = value; }
            }
            public string docs
            {
                get { return this.m_docs; }
                set { this.m_docs = value; }
            }
            public string image_title
            {
                get { return this.m_image_title; }
                set { this.m_image_title = value; }
            }
            public string image_url
            {
                get { return this.m_image_url; }
                set { this.m_image_url = value; }
            }
            public string image_link
            {
                get { return this.m_image_link; }
                set { this.m_image_link = value; }
            }
            public int image_width
            {
                get { return this.m_image_width; }
                set { this.m_image_width = value; }
            }
            public int image_height
            {
                get { return this.m_image_height; }
                set { this.m_image_height = value; }
            }
            public RssItemCollection items
            {
                get { return this.m_items; }
                set { this.m_items = value; }
            }

            public override string ToString()
            {
                StringBuilder str = new StringBuilder();
                str.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                str.Append("<rss version=\"2.0\">");
                str.Append("<channel>");

                str.Append("<title><![CDATA[ " + this.m_title + " ]]></title>");
                str.Append("<link><![CDATA[ " + this.m_link + " ]]></link>");
                str.Append("<description><![CDATA[ " + this.m_description + " ]]></description>");
                str.Append("<ttl>" + this.m_ttl.ToString() + "</ttl>");
                str.Append("<copyright>" + this.m_copyright + "</copyright>");
                str.Append("<pubDate>" + this.m_pubDate.ToString() + "</pubDate>");
                str.Append("<generator>" + this.m_generator + "</generator>");
                str.Append("<docs>" + this.m_docs + "</docs>");

                if (this.m_image_url != string.Empty)
                {
                    str.Append("<image>");
                    str.Append("<title>" + this.m_image_title + "</title>");
                    str.Append("<url>" + this.m_image_url + "</url>");
                    str.Append("<link>" + this.m_image_link + "</link>");
                    if (this.m_image_width > 0) str.Append("<width>" + this.m_image_width.ToString() + "</width>");
                    if (this.m_image_height > 0) str.Append("<height>" + this.m_image_height.ToString() + "</height>");
                    str.Append("</image>");
                }

                for (int i = 0; i < this.m_items.Count; i++)
                {
                    str.Append(this.m_items[i].ToString());
                }

                str.Append("</channel>");
                str.Append("</rss>");

                return str.ToString();
            }
        }

        public class RSSItem
        {
            private string m_title = string.Empty;
            private string m_link = string.Empty;
            private string m_guid = string.Empty;
            private string m_description = string.Empty;
            private DateTime m_pubDate = DateTime.Now;

            public string title
            {
                get { return this.m_title; }
                set { this.m_title = value; }
            }
            public string link
            {
                get { return this.m_link; }
                set { this.m_link = value; }
            }
            public string guid
            {
                get { return this.m_guid; }
                set { this.m_guid = value; }
            }
            public string description
            {
                get { return this.m_description; }
                set { this.m_description = value; }
            }
            public DateTime pubDate
            {
                get { return this.m_pubDate; }
                set { this.m_pubDate = value; }
            }

            public override string ToString()
            {
                StringBuilder str = new StringBuilder();
                str.Append("<item>");
                str.Append("<title><![CDATA[ " + this.m_title + " ]]></title>");
                str.Append("<link><![CDATA[ " + this.m_link + " ]]></link>");
                str.Append("<guid isPermaLink=\"false\"><![CDATA[ " + this.m_guid + " ]]></guid>");
                str.Append("<description><![CDATA[ " + this.m_description + " ]]></description>");
                str.Append("<pubDate>" + this.m_pubDate.ToString() + "</pubDate>");
                str.Append("</item>");

                return str.ToString();
            }
        }

        public class RssItemCollection : System.Collections.CollectionBase
        {
            public RSSItem this[int index]
            {
                get
                {
                    return (List[index] as RSSItem);
                }
            }
            public int Add(RSSItem item)
            {
                return (List.Add(item));
            }
            public void Remove(RSSItem item)
            {
                List.Remove(item);
            }
        }
    }
}
