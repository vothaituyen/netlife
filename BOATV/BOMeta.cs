using System;
using System.Collections.Generic;
using System.Text;
using ATVEntity;
using System.Data;
using DALATV;

namespace BOATV
{
    public class BOMeta
    {
        public static MetaEntity GetMeta(int id)
        {
            DataTable tbl = new DataTable();
            string key = String.Format("GetMeta__{0}", id);
            MetaEntity ce = Utils.GetFromCache<MetaEntity>(key);
            if (ce == default(MetaEntity) || ce == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.GetMetaByID(id);
                }
                ce = new MetaEntity();
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    DataRow row = tbl.Rows[0];
                    ce.Description = Utils.GetObj<string>(row["Description"]);
                    ce.Keyword = Utils.GetObj<string>(row["Keywords"]);                    
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.META, key, ce);
            }
            if (ce == null) ce = new MetaEntity();
            return ce;
        }
    }
}
