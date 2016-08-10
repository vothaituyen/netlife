using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using DALATV;
using ATVEntity;
using System.Web;
using ATVCommon;
using System.Configuration;
using HtmlAgilityPack;

namespace BOATV
{
    public class BOThread
    {

        /// <summary>
        /// get thread theo top
        /// </summary>
        /// <param name="top"></param>
        /// <param name="imgWidth"></param>
        /// <returns></returns>
        public static List<ThreadEntity> GetThreadHome(int top, int imgWidth)
        {
            string key = String.Format("GetThreadHome-{0}-{1}", top, imgWidth);
            var obj = (List<ThreadEntity>)HttpContext.Current.Cache[key];
            if (obj == null)
            {
                obj = new List<ThreadEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.GetListThreadHome(top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                ThreadEntity thr;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    thr = new ThreadEntity();
                    thr.Title = Utils.GetObj<String>(row["Title"]);
                    thr.ThreadId = Utils.GetObj<Int32>(row["Thread_Id"]);
                    thr.Url = String.Format("/event/{0}-e{1}.html", Utils.UnicodeToKoDauAndGach(Utils.GetObj<String>(row["Title"])), thr.ThreadId);
                    thr.Image = Utils.GetThumbNail(thr.Title, thr.Url, Utils.GetObj<String>(row["Thread_logo"]), imgWidth);
                    obj.Add(thr);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, new[] { TableName.THREAD}, key, obj);

            }

            return obj;
        }

        /// <summary>
        /// Get thread theo Mới nhất
        /// </summary>
        /// <param name="top"></param>
        /// <param name="imgWidth"></param>
        /// <returns></returns>
        public static List<ThreadEntity> GetAllThreadNews(int pageIndex, int pageSize, int imgWidth)
        {
            string key = String.Format("GetAllThreadNews-{0}-{1}-{2}", pageIndex, pageSize, imgWidth);
            List<ThreadEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<ThreadEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.GetAllThreadNews(pageIndex, pageSize);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                ThreadEntity thr;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    thr = new ThreadEntity();
                    thr.Title = Utils.GetObj<String>(row["Title"]);
                    thr.ThreadId = Utils.GetObj<Int32>(row["Thread_Id"]);
                    thr.Url = String.Format("/event/{0}-e{1}.html", Utils.UnicodeToKoDauAndGach(Utils.GetObj<String>(row["Title"])), thr.ThreadId);
                    thr.Image = Utils.GetThumbNail(thr.Title, thr.Url, Utils.GetObj<String>(row["Thread_logo"]), imgWidth);
                    lst.Add(thr);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.THREAD, key, lst);
                return lst;
            }
            return (List<ThreadEntity>)obj;
        }

        public static List<ThreadEntity> GetAllThreadNewsOtherId(int pageIndex, int pageSize, int threadId, int imgWidth)
        {
            string key = String.Format("GetAllThreadNewsOtherId-{0}-{1}-{2}-{3}", pageIndex, pageSize, threadId, imgWidth);
            var obj = (List<ThreadEntity>)HttpContext.Current.Cache[key];
            if (obj == null)
            {
                obj = new List<ThreadEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.GetAllThreadNewsOtherId(pageIndex, pageSize, threadId);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                ThreadEntity thr;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    thr = new ThreadEntity();
                    thr.Title = Utils.GetObj<String>(row["Title"]);
                    thr.ThreadId = Utils.GetObj<Int32>(row["Thread_Id"]);
                    thr.Url = String.Format("/event/{0}-e{1}.html", Utils.UnicodeToKoDauAndGach(Utils.GetObj<String>(row["Title"])), thr.ThreadId);
                    thr.Image = Utils.GetThumbNail(thr.Title, thr.Url, Utils.GetObj<String>(row["Thread_logo"]), imgWidth);
                    obj.Add(thr);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.THREAD, key, obj);
                return obj;
            }
            return obj;
        }

        /// <summary>
        /// Get thread
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static List<ThreadEntity> GetThreadByThreadId(int threadId)
        {
            string key = String.Format("GetThreadByThreadId-{0}", threadId);
            var obj = (List<ThreadEntity>)HttpContext.Current.Cache[key];
            if (obj == null)
            {
                obj = new List<ThreadEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.GetThreadByThreadId(threadId);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                ThreadEntity thr;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    thr = new ThreadEntity();
                    thr.Title = Utils.GetObj<String>(row["Title"]);
                    thr.ThreadId = Utils.GetObj<Int32>(row["Thread_Id"]);
                    thr.Url = String.Format("/event/{0}-e{1}.html", Utils.UnicodeToKoDauAndGach(Utils.GetObj<String>(row["Title"])), thr.ThreadId);
                    obj.Add(thr);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.THREAD, key, obj);
                return obj;
            }
            return obj;
        }
    }
}
