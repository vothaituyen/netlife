using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DALATV;

namespace BOATV
{
    public class CacheMonitorManager
    {

        private static void ErrorLog(string sErrMsg)
        {
            if (!string.IsNullOrEmpty(logFolder))
            {
                string filename = logFolder + "\\Error_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                if (!Directory.Exists(logFolder)) Directory.CreateDirectory(logFolder);
                var sw = new StreamWriter(filename, true);
                sw.WriteLine(sErrMsg);
                sw.Flush();
                sw.Close();
            }

        }

        private static string logFolder = ConfigurationSettings.AppSettings["logFolder"] ?? "";


        private int interval = 10000;
        private static bool isFirstRun = true;
        public void UpdateHtmlCache()
        {
            try
            {
                #region Load Category
                if (isFirstRun)
                {
                    GetAllCategory();
                    isFirstRun = false;
                }
                #endregion

                int toprow = 0;
                using (var db = new MainDB())
                {
                    //Type = 0 la web
                    //var data = db.SelectQuery("select [TableName] News_ID,[ChangeID] Cat_ID, [ChangeTime] from HtmlCached Where Type=0 where ChangeTime <= getdate()");
                    var data = db.SelectQuery("select [TableName] News_ID,[ChangeID] Cat_ID, [ChangeTime] from HtmlCached Where ChangeTime <= getdate()"); // htthao edit 20160623
                    toprow = data != null ? data.Rows.Count : 0;
                }

               if (toprow > 0)
                    UpdateMonitor();
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message + Environment.NewLine + ex.StackTrace + ":" + DateTime.Now + Environment.NewLine);
            }

        }


        static Dictionary<Int32, Int32> categoryDiction = new Dictionary<Int32, Int32>();
        private static void GetAllCategory()
        {
            var tbl = new DataTable();
            categoryDiction.Clear();
            using (var db = new MainDB())
            {
                tbl = db.SelectQuery("Select * from Category");
            }

            foreach (DataRow row in tbl.Rows)
            {
                if (!categoryDiction.ContainsKey(Convert.ToInt32(row["Cat_ID"].ToString())))
                    categoryDiction.Add(Convert.ToInt32(row["Cat_ID"].ToString()), Convert.ToInt32(row["Cat_ParentID"]));
            }

        }

        private static bool firstRun = false;
        /// <summary>
        ///
        /// </summary>
        private void UpdateMonitor()
        {
            var tbl = new DataTable();
            using (MainDB db = new MainDB())
            {
                tbl = db.CallStoredProcedure("Insert_HtmlCached", true);
            }

            if (tbl != null)
            {
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    DataRow row = tbl.Rows[i];
                    Int64 newsId = 0;
                    Int64.TryParse(row["News_ID"].ToString(), out newsId);
                    int catId = 0;
                    Int32.TryParse(row["Cat_ID"].ToString(), out catId);
                    if (newsId > 0)
                    {
                        AddRemoveCache(catId, newsId);
                    }
                }
            }
        }

        /// <summary>
        /// Luôn phải chạy sau hàm Update cache
        /// </summary>
        /// <param name="qnaId"></param>
       

        private static void AddRemoveCache(int catId, Int64 news_id)
        {

            string key = string.Empty;
            //GetListNewsByNewsMode2-54-1-1-10-310

            Utils.Remove_MemCache(String.Format("NP_Tin_Nong-0-4-12-90"));
            Utils.Remove_MemCache(String.Format("GetListNewsByNewsMode2-{0}-1-1-10-310", catId));
            Utils.Remove_MemCache(String.Format("Danh_Sach_Tin_Theo_Cat-{0}-20-1-213", catId));
            Utils.Remove_MemCache(String.Format("GetListNewsByCatAndDate-{0}-1-2-150", catId));
            Utils.Remove_MemCache(String.Format("GetListNewsByCatAndDate-{0}-1-9-140", catId));
            Utils.Remove_MemCache(String.Format("GetListNewsByCatAndDate-{0}-1-3-280", catId));
            Utils.Remove_MemCache(String.Format("GetListNewsByCatAndDate-{0}-1-3-150", catId));
            Utils.Remove_MemCache(String.Format("GetListNewsByNewsMode3-{0}-1-5-6-1-310", catId));
            Utils.Remove_MemCache(String.Format("GetListNewsByNewsMode3-{0}-1-5-6-1-440", catId));
            Utils.Remove_MemCache(String.Format("GetListNewsByNewsMode3-{0}-1-5-6-1-453", catId));
            Utils.Remove_MemCache(String.Format("NewsPublishEntity_Sao_Danh_Sach_Tin-{0}-0-4-1-150", catId));
            Utils.Remove_MemCache(String.Format("NP_Sao_Danh_Sach_Tin_Count-{0}-20", catId));
            Utils.Remove_MemCache(String.Format("NP_Select_Tin_Tieu_Diem-{0}-47-5-75", catId));
            Utils.Remove_MemCache(String.Format("TTOL-GetListBonBaiNoibat-6-440", catId)); //
            Utils.Remove_MemCache(String.Format("NP_Tin_Moi_Trong_Ngay-5-75", catId));
            Utils.Remove_MemCache(String.Format("NP_Tin_Moi_Trong_Ngay-7-0", catId));
            Utils.Remove_MemCache(String.Format("NP_Tin_Nong-0-4-12-90", catId));
            Utils.Remove_MemCache(String.Format("NP_Tin_Nong-0-3-6-0"));
            Utils.Remove_MemCache(String.Format("NP_Xem_Nhieu_Nhat-6-75-{0}", catId));

            //Remove cho tin chi tiêt
            Utils.Remove_MemCache(String.Format("Select_Tin_Khac-{0}", news_id));
            Utils.Remove_MemCache(String.Format("{0}_NewsDetail", news_id));

            if (categoryDiction.ContainsKey(catId) && categoryDiction[catId] > 0)
            {
                AddRemoveCache(categoryDiction[catId], news_id);
            }
  
        }
    }
}
