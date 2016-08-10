using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DALATV;
using ATVEntity;
using System.Web;

namespace BOATV
{
    public class BOCategory
    {
        public static CategoryEntity GetCategory(int catId)
        {
            DataTable tbl = new DataTable();
            string key = String.Format("GetCategory_{0}", catId);
            var ce = Utils.GetFromCache<CategoryEntity>(key);
            if (ce == null)
            {
                ce = new CategoryEntity();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.GetCategoryByID(catId);
                }
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    DataRow row = tbl.Rows[0];
                    ce.Cat_ID = Utils.GetObj<Int32>(row["Cat_ID"]);
                    ce.Cat_ParentID = Utils.GetObj<Int32>(row["Cat_ParentID"]);
                    ce.Cat_Name = Utils.GetObj<string>(row["Cat_Name"]);
                    ce.Cat_DisplayURL = Utils.GetObj<string>(row["Cat_DisplayURL"]).Trim().ToLower();
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.CATEGORY, key, ce);
            }

            return ce;
        }


        public static DataTable GetCategoryTree()
        {
            string key = String.Format("GetCategoryTree");
            DataTable tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.CategoryBuildTree();
                }
                int iCout = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                DataRow[] tblTemp;
                DataTable tblApp = tbl.Clone();
                for (int i = 0; i < iCout; i++)
                {
                    row = tbl.Rows[i];
                    if (Convert.ToInt32(row["Cat_ParentId"]) == 0)
                    {
                        tblApp.ImportRow(row);
                        tblTemp = tbl.Select("Cat_ParentId = " + row["Cat_Id"].ToString());
                        if (tblTemp != null)
                        {
                            for (int j = 0, jCout = tblTemp.Length; j < jCout; j++)
                            {
                                tblTemp[j]["Cat_Name"] = " -- " + tblTemp[j]["Cat_Name"].ToString();
                                tblApp.ImportRow(tblTemp[j]);
                            }
                        }
                    }
                }
                tblApp.AcceptChanges();
                tbl = tblApp;
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.CATEGORY, key, tbl);
            }
            return tbl;
        }


        public static DataTable GetCategoryByParent(int parentID)
        {
            DataTable tbl = new DataTable();
            string key = String.Format("GetCategoryByParent_{0}", parentID);
            tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.GetCategoryByParent(parentID);
                }
                if (!tbl.Columns.Contains("Href")) tbl.Columns.Add("Href");
                int count = tbl.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    tbl.Rows[i]["Href"] = String.Format("/{0}.html", Utils.UnicodeToKoDauAndGach(tbl.Rows[i]["Cat_DisplayUrl"].ToString()).ToLower());
                }
                tbl.AcceptChanges();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.CATEGORY, key, tbl);
            }
            return tbl;
        }
        public static DataTable GetCategoryByParentAndTop(int parentID, int Top)
        {
            DataTable tbl = new DataTable();
            string key = String.Format("GetCategoryByParentAndTop-{0}-{1}", parentID, Top);
            tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.GetCategoryByParentAndTop(parentID, Top);
                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.CATEGORY, key, tbl);
            }
            return tbl;
        }

        public static DataTable GetCategoryByCatName(string catName)
        {
            DataTable tbl = new DataTable();
            string key = String.Format("GetCategoryByCatName-{0}", catName);
            tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.GetCatIDByCatDisplayUrl(catName);
                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.CATEGORY, key, tbl);
            }
            return tbl;
        }

        public static DataTable TTOL_GetCatIDByCatDisplayUrl(string catName)
        {
            DataTable tbl = new DataTable();
            string key = String.Format("TTOL_GetCatIDByCatDisplayUrl-{0}", catName);
            tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.TTOL_GetCatIDByCatDisplayUrl(catName);
                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.CATEGORY, key, tbl);
            }
            return tbl;
        }


        public static DataTable GetCategoryByParent(int parentID, bool ishidden)
        {
            DataTable tbl = new DataTable();
            string key = String.Format("GetCategoryByParent_{0}_{1}", parentID, ishidden);
            tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    if (ishidden)
                        tbl = db.StoredProcedures.GetCategoryByParent(parentID);
                    else
                        tbl = db.StoredProcedures.GetCategoryByParentRemoveHidden(parentID);
                }

                if (!tbl.Columns.Contains("Href")) tbl.Columns.Add("Href");
                int count = tbl.Rows.Count;
                for (int i = 0; i < count; i++)
                {

                    if (Convert.ToBoolean(tbl.Rows[i]["Cat_isHidden"]) != ishidden)
                    {
                        tbl.Rows.Remove(tbl.Rows[i]);

                        count--;
                        continue;
                    }

                    tbl.Rows[i]["Href"] = String.Format("/{0}.html", Utils.UnicodeToKoDauAndGach(tbl.Rows[i]["Cat_DisplayUrl"].ToString()).ToLower());
                }
                tbl.AcceptChanges();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.CATEGORY, key, tbl);
            }
            return tbl;
        }


    }
}
