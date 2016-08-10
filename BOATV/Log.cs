using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALATV;

namespace BOATV
{
    public class Log
    {
        public void CaculateLogViewCategory(Queue<Int32> queue)
        {
            var dic = new Dictionary<int, int>();
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (dic.ContainsKey(item))
                {
                    dic[item]++;
                }
                else
                {
                    dic.Add(item, 1);
                }
            }
            var d = DateTime.Now.Date;

            using (var db = new MainDB())
            {
                foreach (var k in dic)
                {
                    db.CallStoredProcedure("SiteStats_UpdateCategory", new object[] { k.Value, k.Key, d }, new[] { "count", "categoryId", "date" }, false);
                }
            }
        }

        public void CaculateLogViewNews(Queue<Int64> queue)
        {
            var dic = new Dictionary<Int64, int>();
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (dic.ContainsKey(item))
                {
                    dic[item]++;
                }
                else
                {
                    dic.Add(item, 1);
                }
            }

            using (var db = new MainDB())
            {
                foreach (var k in dic)
                {
                    db.CallStoredProcedure("News_UpdateViewCount", new object[] { k.Value, k.Key}, new[] { "count", "newsId" }, false);
                }
            }
        }

        public void CaculateLogViewAds(Queue<Int32> queue)
        {
            var dic = new Dictionary<Int32, int>();
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (dic.ContainsKey(item))
                {
                    dic[item]++;
                }
                else
                {
                    dic.Add(item, 1);
                }
            }

            using (var db = new MainDB())
            {
                foreach (var k in dic)
                {
                    db.CallStoredProcedure("QuangCao_Item_UpdateView", new object[] { k.Value, k.Key }, new[] { "View", "Id" }, false);
                }
            }
        }

        public void CaculateLogClickAds(Queue<Int32> queue)
        {
            var dic = new Dictionary<Int32, int>();
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (dic.ContainsKey(item))
                {
                    dic[item]++;
                }
                else
                {
                    dic.Add(item, 1);
                }
            }

            using (var db = new MainDB())
            {
                foreach (var k in dic)
                {
                    db.CallStoredProcedure("QuangCao_Item_UpdateClick", new object[] { k.Value, k.Key }, new[] { "Click", "Id" }, false);
                }
            }
        }
    }
}