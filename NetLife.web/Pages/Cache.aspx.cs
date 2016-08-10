using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BOATV;

namespace NetLife.web.Pages
{
    public partial class Cache : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["cache"] != null)
            {
                Utils.Reset_MemCache();
            }
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            var r = Utils.Get_MemCache(txtKey.Text);
            ltrCache.Text = r != null ? r.ToString() : "null";
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            Utils.Add_MemCache(DateTime.Now.ToString(), txtKey.Text);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Utils.Remove_MemCache(txtKey.Text);
        }
 

        protected void btnViewCache_Click(object sender, EventArgs e)
        {
            var keys = new List<string>();
            // retrieve application Cache enumerator
            var cache = HttpRuntime.Cache;
            var enumerator = cache.GetEnumerator();
            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }
            // delete every key from cache
            keys.Sort();
            string html = keys.Aggregate("", (current, t) => current + (t + "<br/>"));
            Response.Write(html);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           txtKey.Text = Utils.Decrypt(txtKey.Text, false);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var keys = new List<string>();
            // retrieve application Cache enumerator
            var cache = HttpRuntime.Cache;
            var enumerator = cache.GetEnumerator();
            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                HttpRuntime.Cache.Remove(enumerator.Key.ToString());
            }
        }
    }
}