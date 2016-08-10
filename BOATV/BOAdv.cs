using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using ATVEntity;
using DALATV;
using System.Web.UI;
using System.Web;
using System.Configuration;

namespace BOATV
{
    [Serializable]
    public class AdvEntity
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Html { get; set; }
    }

    public class BOAdv
    {
        public static string MasterConnectionString = ConfigurationSettings.AppSettings["MasterConnectionString"] != null
                                                   ? ConfigurationSettings.AppSettings["MasterConnectionString"].
                                                         ToString()
                                                   : string.Empty;
        // Fields
        private const string HTML_Adv_Rotate = "<div id='Zone_{0}' class='{3}'></div>{2}<script type=\"text/javascript\" language=\"javascript\">{2}var {0} = new RunBanner({1}, 'Zone_{0}', '{0}');{2} {0}.Show();</script>";

        // Methods
        public static List<AdvItemEntity> GetAdvByZoneId(int zoneId)
        {
            string cacheName = string.Format("GetAdvByZoneId_{0}", zoneId);
            List<AdvItemEntity> fromCache = Utils.Get_MemCache<List<AdvItemEntity>>(cacheName) ?? Utils.GetFromCache<List<AdvItemEntity>>(cacheName);
            if (fromCache == null)
            {
                DataTable advByZoneId;
                using (var ndb = new MainDB())
                {
                    advByZoneId = ndb.StoredProcedures.GetAdvByZoneId(zoneId);
                }
                int num = (advByZoneId != null) ? advByZoneId.Rows.Count : 0;
                if (num > 0)
                {
                    for (int i = 0; i < num; i++)
                    {
                        DataRow row = advByZoneId.Rows[0];
                        var item = new AdvItemEntity();
                        item.ID = Utils.GetObj<int>(row["ID"]);
                        item.isActive = (row["isActive"] != null) && Convert.ToBoolean(row["isActive"]);
                        item.Name = Utils.GetObj<string>(row["Name"]);
                        item.SourceFile = Utils.GetObj<string>(row["SourceFile"]);
                        item.STT = Utils.GetObj<int>(row["STT"]);
                        item.TargetUrl = Utils.GetObj<string>(row["TargetUrl"]);
                        item.Type = Utils.GetObj<int>(row["Type"]);
                        fromCache.Add(item);

                    }
                }

                if (!Utils.Add_MemCache(cacheName, fromCache))
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.QUANGCAO_ITEM, cacheName, fromCache);
            }

            return fromCache ?? (fromCache = new List<AdvItemEntity>());
        }

        public static string DataTableToJSON(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }

                list.Add(dict);
            }
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(list);
        }



        public static string ListDataTableToJSON(List<DataTable> lsttable, string zoneId)
        {

            string html = String.Empty;
            foreach (DataTable table in lsttable)
            {
                var list = new List<Dictionary<string, object>>();
                foreach (DataRow row in table.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn col in table.Columns)
                    {
                        dict[col.ColumnName] = row[col];
                    }
                    list.Add(dict);
                }
                var serializer = new JavaScriptSerializer();
                html += serializer.Serialize(list) + ",";
            }

            if (html.EndsWith(",")) html =
                html.Substring(0, html.Length - 1);
            return html;
        }

        public static string GetAdvItemById(int zoneId, int catId)
        {
            string cacheName = string.Format("GetAdvItemById__{0}__{1}r", zoneId, catId);
            string fromCache = Utils.GetFromCache<string>(cacheName);
            var advEntity = new List<AdvEntity>();
            if (fromCache == null)
            {
                DataTable advItemById;
                using (var ndb = new MainDB())
                {
                    advItemById = ndb.StoredProcedures.GetAdvItemById(zoneId, catId);
                }
                fromCache = DataTableToJSON(advItemById);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, new string[] { TableName.QUANGCAO_ITEM, TableName.ZONE_ITEM }, cacheName, fromCache);
            }
            return fromCache;
        }


        /// <summary>
        /// Dành cho trường hợp nhúng Embed vào trang của Ambient hoặc Infinity.
        /// </summary>
        /// <param name="zoneId"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static string GetAdvEmbedScriptItemById(int zoneId, int catId)
        {

            DataTable advItemById;
            string cacheName = string.Format("GetAdvEmbedScriptItemById_{0}__{1}", zoneId, catId);
            var fromCache = Utils.GetFromCache<string>(cacheName);
            if (fromCache == null)
            {
                using (MainDB ndb = new MainDB())
                {
                    advItemById = ndb.StoredProcedures.GetAdvItemById(zoneId, catId);
                }
                fromCache = string.Empty;
                string str3 = string.Empty;
                var entity = new AdvItemEntity();
                int num = (advItemById != null) ? advItemById.Rows.Count : 0;
                for (int i = 0; i < num; i++)
                {
                    DataRow row = advItemById.Rows[i];

                    entity.Type = (row["Type"] != null) ? Utils.GetObj<int>(row["Type"]) : 0;

                    if (entity.Type == 5)
                    {
                        str3 += (row["SourceFile"] != null) ? Utils.GetObj<string>(row["SourceFile"]) : string.Empty;
                    }

                }
                str3 = str3.Trim();


                fromCache = str3 + fromCache;
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, new string[] { TableName.QUANGCAO_ITEM, TableName.ZONE_ITEM }, cacheName, fromCache);
            }
            return fromCache;
        }

        public static List<AdvZoneEntity> GetZoneByCatId(int catId)
        {
            var zoneByCatId = new DataTable();
            string cacheName = string.Format("GetZoneByCatId_{0}", catId);
            var fromCache = Utils.GetFromCache<List<AdvZoneEntity>>(cacheName);
            if ((fromCache == null) || (fromCache == null))
            {
                using (MainDB ndb = new MainDB())
                {
                    zoneByCatId = ndb.StoredProcedures.GetZoneByCatId(catId);
                }
                int num = (zoneByCatId != null) ? zoneByCatId.Rows.Count : 0;
                if (num > 0)
                {
                    for (int i = 0; i < num; i++)
                    {
                        var row = zoneByCatId.Rows[0];
                        var item = new AdvZoneEntity();
                        item.ID = Utils.GetObj<int>(row["ID"]);
                        item.isActive = (row["isActive"] != null) ? Convert.ToBoolean(row["isActive"]) : false;
                        item.Name = Utils.GetObj<string>(row["Name"]);
                        item.WidthDefault = Utils.GetObj<int>(row["WidthDefault"]);
                        item.Cat_ID = Utils.GetObj<int>(row["Cat_ID"]);
                        fromCache.Add(item);
                    }
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.QUANGCAO_ZONE, cacheName, fromCache);
            }
            if (fromCache == null)
            {
                fromCache = new List<AdvZoneEntity>();
            }
            return fromCache;
        }

        public static void UpdateView(int advId)
        {
            using (var ndb = new MainDB(MasterConnectionString))
            {
                ndb.StoredProcedures.AdvUpdateView(advId);
            }
        }
    }


}
