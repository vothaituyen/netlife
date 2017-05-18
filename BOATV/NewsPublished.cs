using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Data;
using DALATV;
using ATVEntity;
using System.Web;
using ATVCommon;
using System.Configuration;
using HtmlAgilityPack;
namespace BOATV
{
    public class NewsPublished
    {
        private static string STRING_CAT_NAME_URL = "/{0}/{1}-c{3}-{2}.html";
        public static void NewsSAO_UpdatePageView(Dictionary<long, int> dic)
        {
            using (var objDB = new MainDB())
            {
                int count = dic.Count;
                string sqlUpdate = String.Empty;
                int i = 0;
                foreach (var pair in dic)
                {
                    i++;
                    sqlUpdate += String.Format("Update News Set ViewCount = ViewCount + {0} Where News_ID = {1} ;",
                                               pair.Value, pair.Key);
                    if (i % 50 == 0 || i == count - 1)
                    {
                        objDB.SelectQuery(sqlUpdate);
                        sqlUpdate = string.Empty;
                    }
                }
            }
        }

        public static DataTable Sao_SelectVoteItemByVoteId(int voteId, bool isOther)
        {
            string key = String.Format("Sao_SelectVoteItemByVoteId-{0}-{1}", voteId, isOther);
            DataTable tbl = (DataTable)HttpContext.Current.Cache[key];
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Sao_SelectVoteItemByVoteId(voteId);
                }

                if (tbl == null) tbl = new DataTable();
                if (!isOther)
                {
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(tbl.Rows[i]["VoteItem_ID"]) == 100000000)
                        {
                            tbl.Rows[i].Delete();
                            tbl.AcceptChanges();
                        }
                    }
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.VoteItem, key, tbl);
            }
            return tbl;
        }


        public static DataTable Sao_SelectVoteById(int voteId, int catId)
        {
            string key = String.Format("Sao_SelectVoteById-{0}-{1}", voteId, catId);
            DataTable tbl = (DataTable)HttpContext.Current.Cache[key];
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Sao_SelectVoteById(voteId, catId);
                }
                if (tbl == null) tbl = new DataTable();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.Vote, key, tbl);
            }
            return tbl;
        }




        public static DataTable NP_SelectTopHotByCat(int cat_id, int top, News_Mode news_mode)
        {
            string key = String.Format("NP_Select-TopHotByCat_{0}-{1}-{2}", cat_id, top, news_mode.ToString());
            DataTable tbl = (DataTable)HttpContext.Current.Cache[key];
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_SelectTopHotByCat(cat_id, top, Convert.ToInt16(news_mode));
                }
                if (tbl == null) tbl = new DataTable();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, tbl);

            }
            return tbl;
        }


        public static DataTable NP_Select_Top_Home(int cat_id, int top)
        {
            string key = String.Format("NP_Select_Top_Home-{0}", cat_id);
            DataTable tbl = Utils.Get_MemCache<DataTable>(key) ?? (DataTable)HttpContext.Current.Cache[key];
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_Select_Top_Home(cat_id, top);
                }
                if (tbl == null) tbl = new DataTable();

                if (!Utils.Add_MemCache(key, tbl))
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, tbl);

            }
            return tbl;
        }

        public static DataTable NP_Select_Top_Cat(int cat_id, int top)
        {
            string key = String.Format("NP_Select_Top_Cat-{0}-{1}", cat_id, top);
            DataTable tbl = (DataTable)HttpContext.Current.Cache[key];
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_Select_Top_Cat(cat_id, top);
                }
                if (tbl == null) tbl = new DataTable();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, tbl);

            }
            return tbl;
        }

        /// <summary>
        /// Lấy ra 4 bài nổi bật của 1 mục
        /// </summary>
        /// <param name="cat_id"></param>
        /// <param name="top"></param>
        /// <param name="imgWidth"></param>
        /// <param name="imgWidthSmall"></param>
        /// <returns></returns>

        const string TOP = @"{0}<div class=""subnews""><h3><a title=""{2}"" href=""{1}"">{2}</a></h3></div>";
        const string DOWN = @"  <li>{0}<div class=""divnav1""> <a href=""{1}"">{1} </a><p>{2}</p></div></li>";

        public static List<string> NP_Select_Top_CatV2(int cat_parentId, int cat_id, int top, int imgWidth, int imgWidthSmall)
        {
            string key = String.Format("NP_Select_Top_Cat-{0}-{1}-{2}-{3}-{4}", cat_parentId, cat_id, top, imgWidth, imgWidthSmall);
            string keyNewwId = String.Format(keyNewID, cat_parentId, cat_id);

            var lst = (List<string>)HttpContext.Current.Cache[key];
            if (lst == null)
            {
                lst = new List<string>();
                string UpHTML = String.Empty;
                string DownHTML = String.Empty;
                string title = string.Empty;
                string initcontent = string.Empty;
                string date = string.Empty;
                string url = string.Empty;
                string image = string.Empty;
                string[] newsId = new string[] { "0", "0", "0", "0" };
                DataTable tbl = new DataTable();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_Select_Top_Cat(cat_id, top);
                }
                if (tbl == null) tbl = new DataTable();
                DataRow row;
                for (int i = 0, icount = tbl.Rows.Count; i < icount; i++)
                {
                    row = tbl.Rows[i];
                    title = row["News_Title"] != null ? HttpUtility.HtmlEncode(row["News_Title"].ToString()) : String.Empty;
                    initcontent = row["News_InitContent"] != null ? row["News_InitContent"].ToString() : String.Empty;
                    date = row["News_PUBLISHDATE"] != null ? row["News_PUBLISHDATE"].ToString() : String.Empty;

                    url = BuildLink(Convert.ToInt32(row["Cat_ParentId"]), Convert.ToInt32(row["Cat_Id"]), Convert.ToInt64(row["News_ID"]), title);
                    newsId[i] = row["News_ID"].ToString();
                    if (i == 0)
                    {
                        image = Utils.GetThumbNail(title, url, row["News_Image"] != null ? row["News_Image"].ToString() : String.Empty, imgWidth);
                        UpHTML = String.Format(TOP, image, url, title, date);
                    }
                    else
                    {
                        image = Utils.GetThumbNail(title, url, row["News_Image"] != null ? row["News_Image"].ToString() : String.Empty, imgWidthSmall);
                        DownHTML += string.Format(DOWN, image, url, title, date, i);
                    }
                }
                lst.Add(UpHTML);
                lst.Add(DownHTML);

                //bool result = Utils.Add_MemCache(key, lst);

                //if (!result)
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);

                //result = Utils.Add_MemCache(keyNewwId, string.Join(",", newsId));

                //if (!result)
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNewwId, string.Join(",", newsId));

            }
            return lst;
        }



        public static List<string> NP_NhatKyNoiBat(int cat_parentId, int cat_id, int top, int imgWidth)
        {
            string key = String.Format("NP_NhatKyNoiBat-{0}-{1}-{2}-{3}", cat_parentId, cat_id, top, imgWidth);
            List<string> lst = (List<string>)HttpContext.Current.Cache[key];
            if (lst == null)
            {
                lst = new List<string>();
                string url = string.Empty;
                string title = "", initcontent = "";
                string image = string.Empty;
                string UpHTML = string.Empty;
                DataTable tbl = new DataTable();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_NhatKyNoiBat(cat_id, top);
                }
                if (tbl == null) tbl = new DataTable();
                DataRow row;
                for (int i = 0, icount = tbl.Rows.Count; i < icount; i++)
                {
                    row = tbl.Rows[i];
                    title = row["News_Title"] != null ? HttpUtility.HtmlEncode(row["News_Title"].ToString()) : String.Empty;
                    initcontent = row["News_InitContent"] != null ? row["News_InitContent"].ToString() : String.Empty;
                    url = BuildLink(Convert.ToInt32(cat_parentId), Convert.ToInt32(row["Cat_Id"]), Convert.ToInt64(row["News_ID"]), title);


                    image = Utils.GetThumbNail(title, url, row["News_Image"] != null ? row["News_Image"].ToString() : String.Empty, imgWidth);
                    UpHTML = String.Format(TOP, image, url, title, initcontent);
                    lst.Add(UpHTML);

                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);

            }
            return lst;
        }

        const string TOP_HOA_HAU = @"{0}<div><h3 title=""{2}""><a title=""{2}"" href=""{1}"">{2}</a></h3><p>{3}</p></div>";
        const string DOWN_HOA_HAU = @"<li><h3 title=""{1}""><a title=""{1}"" href=""{0}"">{1}</a></h3><p>{2}</p></li>";

        public static List<string> NP_Select_Top_CatV2_HoaHau(int cat_parentId, int cat_id, int top, int imgWidth)
        {
            string key = String.Format("NP_Select_Top_CatV2_HoaHau-{0}-{1}-{2}-{3}", cat_parentId, cat_id, top, imgWidth);
            string keyNewwId = String.Format(keyNewID, cat_parentId, cat_id);
            List<string> lst = (List<string>)HttpContext.Current.Cache[key];
            if (lst == null)
            {
                lst = new List<string>();
                string UpHTML = String.Empty;
                string DownHTML = String.Empty;
                string title = string.Empty;
                string initcontent = string.Empty;
                string url = string.Empty;
                string image = string.Empty;
                string icon = String.Empty;
                string[] newsId = new string[] { "0", "0", "0", "0" };
                DataTable tbl = new DataTable();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_Select_Top_Cat(cat_id, top);
                }
                if (tbl == null) tbl = new DataTable();
                DataRow row;
                for (int i = 0, icount = tbl.Rows.Count; i < icount; i++)
                {
                    row = tbl.Rows[i];
                    icon = row["Icon"] != null ? row["Icon"].ToString() : String.Empty;
                    title = row["News_Title"] != null ? HttpUtility.HtmlEncode(row["News_Title"].ToString()) : String.Empty;
                    initcontent = row["News_InitContent"] != null ? row["News_InitContent"].ToString() : String.Empty;
                    initcontent = Utils.CatSapo(initcontent, 30);
                    initcontent = HttpUtility.HtmlEncode(initcontent);
                    url = BuildLink(Convert.ToInt32(row["Cat_ParentId"]), Convert.ToInt32(row["Cat_Id"]), Convert.ToInt64(row["News_ID"]), title);
                    newsId[i] = row["News_ID"].ToString();
                    if (i == 0)
                    {
                        image = icon.Length > 0 ? icon : (row["News_Image"] != null ? row["News_Image"].ToString() : String.Empty);

                        image = String.Format("<a href=\"{0}\" title=\"{1}\"><img title=\"{1}\" src=\"{2}\"/></a>", url, title, image);

                        UpHTML = String.Format(TOP_HOA_HAU, image, url, title, initcontent);
                    }
                    else
                    {
                        DownHTML += string.Format(DOWN_HOA_HAU, url, title, initcontent);
                    }
                }
                lst.Add(UpHTML);
                lst.Add(DownHTML);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNewwId, string.Join(",", newsId));

            }
            return lst;
        }

        public static List<string> NP_Select_Top_CatV2_Diana_Home(int cat_parentId, int cat_id, int imgWidth)
        {
            string key = String.Format("NP_Select_Top_CatV2_Diana_Home-{0}-{1}-{2}", cat_parentId, cat_id, imgWidth);
            string keyNewwId = String.Format(keyNewID, cat_parentId, cat_id);
            List<string> lst = (List<string>)HttpContext.Current.Cache[key];
            if (lst == null)
            {
                lst = new List<string>();
                string UpHTML = String.Empty;
                string DownHTML = String.Empty;
                string title = string.Empty;
                string initcontent = string.Empty;
                string url = string.Empty;
                string image = string.Empty;
                string icon = String.Empty;
                string[] newsId = new string[] { "0", "0", "0" };
                DataTable tbl = new DataTable();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_Select_Top_Cat(cat_id, 3);
                }
                if (tbl == null) tbl = new DataTable();
                DataRow row;
                for (int i = 0, icount = tbl.Rows.Count; i < icount; i++)
                {
                    row = tbl.Rows[i];
                    icon = row["Icon"] != null ? row["Icon"].ToString() : String.Empty;
                    title = row["News_Title"] != null ? HttpUtility.HtmlEncode(row["News_Title"].ToString()) : String.Empty;
                    initcontent = row["News_InitContent"] != null ? row["News_InitContent"].ToString() : String.Empty;
                    initcontent = Utils.CatSapo(initcontent, 30);
                    initcontent = HttpUtility.HtmlEncode(initcontent);
                    //url = BuildLink(Convert.ToInt32(row["Cat_ParentId"]), Convert.ToInt32(row["Cat_Id"]), Convert.ToInt64(row["News_ID"]), title);
                    url = row["News_SubTitle"] != null && !string.IsNullOrEmpty(row["News_SubTitle"].ToString()) ? row["News_SubTitle"].ToString() : "#";
                    newsId[i] = row["News_ID"].ToString();
                    if (i == 0)
                    {
                        image = row["News_Image"] != null ? row["News_Image"].ToString() : String.Empty;

                        image = Utils.GetThumbNail(title, url, image, 100);


                        UpHTML = String.Format(@"<h2 title=""{2}""><a title=""{2}"" target=""_blank"" href=""{1}"">{2}</a></h2>{0}<p>{3}</p>", image, url, title, initcontent);
                    }
                    else
                    {
                        DownHTML += string.Format(@"<h3 title=""{1}""><a title=""{1}"" target=""_blank""  href=""{0}"">{1}</a></h3>", url, title);
                    }
                }
                lst.Add(UpHTML);
                lst.Add(DownHTML);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNewwId, string.Join(",", newsId));

            }
            return lst;
        }

        public static List<string> NP_Select_Top_CatV2_HoaHau_Home(int cat_parentId, int cat_id, int imgWidth)
        {
            string key = String.Format("NP_Select_Top_CatV2_HoaHau_Home-{0}-{1}-{2}", cat_parentId, cat_id, imgWidth);
            string keyNewwId = String.Format(keyNewID, cat_parentId, cat_id);
            List<string> lst = (List<string>)HttpContext.Current.Cache[key];
            if (lst == null)
            {
                lst = new List<string>();
                string UpHTML = String.Empty;
                string DownHTML = String.Empty;
                string title = string.Empty;
                string initcontent = string.Empty;
                string url = string.Empty;
                string image = string.Empty;
                string icon = String.Empty;
                string[] newsId = new string[] { "0", "0", "0" };
                DataTable tbl = new DataTable();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_Select_Top_Cat(cat_id, 3);
                }
                if (tbl == null) tbl = new DataTable();
                DataRow row;
                for (int i = 0, icount = tbl.Rows.Count; i < icount; i++)
                {
                    row = tbl.Rows[i];
                    icon = row["Icon"] != null ? row["Icon"].ToString() : String.Empty;
                    title = row["News_Title"] != null ? HttpUtility.HtmlEncode(row["News_Title"].ToString()) : String.Empty;
                    initcontent = row["News_InitContent"] != null ? row["News_InitContent"].ToString() : String.Empty;
                    initcontent = Utils.CatSapo(initcontent, 30);
                    initcontent = HttpUtility.HtmlEncode(initcontent);
                    url = BuildLink(Convert.ToInt32(row["Cat_ParentId"]), Convert.ToInt32(row["Cat_Id"]), Convert.ToInt64(row["News_ID"]), title);
                    newsId[i] = row["News_ID"].ToString();
                    if (i == 0)
                    {
                        image = row["News_Image"] != null ? row["News_Image"].ToString() : String.Empty;

                        image = Utils.GetThumbNail(title, url, image, 100);


                        UpHTML = String.Format(@"<h2 title=""{2}""><a title=""{2}"" href=""{1}"">{2}</a></h2>{0}<p>{3}</p>", image, url, title, initcontent);
                    }
                    else
                    {
                        DownHTML += string.Format(@"<h3 title=""{1}""><a title=""{1}"" href=""{0}"">{1}</a></h3>", url, title);
                    }
                }
                lst.Add(UpHTML);
                lst.Add(DownHTML);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNewwId, string.Join(",", newsId));

            }
            return lst;
        }

        public static int NP_Sao_Danh_Sach_Tin_Hidden_Count(int cat_parentid, int cat_id, int pageSize)
        {

            string key = String.Format("NP_Sao_Danh_Sach_Tin_Hidden_Count-{0}-{1}", cat_id, pageSize);
            object result = HttpContext.Current.Cache[key];
            if (result == null)
            {
                DataTable tbl;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id);
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin_Hidden_Count(cat_id, news_ids);
                }
                int Count = tbl != null ? Convert.ToInt32(tbl.Rows[0][0]) : 1;
                Count = Convert.ToInt32((Count - 1) / pageSize) + 1;
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, Count);
                return Count;
            }
            return Convert.ToInt32(result);
        }

        public static List<String> NP_Sao_Danh_Sach_Tin(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string ITEM = "<div class=\"listnews\">{0}<h1><a href=\"{1}\">{2}</a></h1><h4>{4}</h4><p>{3}</p></div>";
            string ITEM_KHAC = "<li><h1><a href=\"{0}\">{1} ({2})</a></h1></li>";
            string key = String.Format("NP_Sao_Danh_Sach_Tin-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            List<String> lst;
            object obj = HttpContext.Current.Cache[key];
            string TopUp = "";
            string TopDown = "";
            NewsPublishEntity npe;
            if (obj == null)
            {
                lst = new List<String>();
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id);
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin(cat_id, pageSize, pageIndex, news_ids);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    if (i < pageSize * 2 / 3)
                    {
                        if (i > 0 && i % 2 == 0 && i < (pageSize * 2 / 3) - 1)
                            TopUp += "</div><div class=\"listnews_parent\">";
                        TopUp += String.Format(ITEM, npe.URL_IMG, npe.URL, npe.NEWS_TITLE, Utils.CatSapo(npe.NEWS_INITCONTENT, 25), npe.NEWS_PUBLISHDATE.ToString("dd/MM/yyyy HH:mm"));
                    }
                    else
                    {
                        TopDown += String.Format(ITEM_KHAC, npe.URL, npe.NEWS_TITLE, npe.NEWS_PUBLISHDATE.ToString("dd/MM"));
                    }
                }
                lst.Add(TopUp);
                lst.Add(TopDown);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<String>)obj;
        }




        public static List<NewsPublishEntity> NP_SearchByDate(int cat_parentid, int cat_id, DateTime dt, int ImgWidth)
        {
            string key = String.Format("NP_SearchByDate-{0}-{1}-{2}-{3}", cat_parentid, cat_id, dt, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            NewsPublishEntity npe;
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable tbl = null;

                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_SearchByDate(cat_id, dt);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;

                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 25);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }


        public static DataTable NP_Tin_Lien_Quan(string News_ID)
        {
            using (MainDB db = new MainDB())
            {
                return db.StoredProcedures.NP_Tin_Lien_Quan(News_ID);
            }
        }


        public static List<NewsPublishEntity> NP_Select_Tin_Khac(int cat_parentid, int cat_id, long News_ID, int top)
        {
            string key = String.Format("Select_Tin_Khac-{0}", News_ID);
            var lst = Utils.Get_MemCache<List<NewsPublishEntity>>(key) ?? (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
            if (lst == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da = NP_Select_Tin_Khac(cat_id, News_ID, top);
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(40, img);
                    lst.Add(npe);
                }
                var result = Utils.Add_MemCache(key, lst);
                if (!result)
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)lst;
        }

        private static DataTable NP_Select_Tin_Khac(int cat_id, long News_ID, int top)
        {
            DataTable tbl = new DataTable();
            using (MainDB db = new MainDB())
            {
                tbl = db.StoredProcedures.NP_Select_Tin_Khac(cat_id, News_ID, top);
            }
            return tbl;

        }

        // ADD new store Procedure to get "Tin Cung Chuyen Muc" 2016052015
        public static List<NewsPublishEntity> NP_Select_Tin_Cung_Chuyen_Muc(int cat_parentid, int cat_id, long News_ID, int top, string relatedNewsId)// bổ sung Id của tin liên quan để lọc tin trùng
        {
            string key = String.Format("Select_Tin_Cung_Chuyen_Muc-{0}", News_ID);
            var lst = Utils.Get_MemCache<List<NewsPublishEntity>>(key) ?? (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
            if (lst == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da = NP_Select_Tin_Cung_Chuyen_Muc(cat_id, News_ID, top, relatedNewsId);
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(40, img);
                    lst.Add(npe);
                }
                var result = Utils.Add_MemCache(key, lst);
                if (!result)
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)lst;
        }

        private static DataTable NP_Select_Tin_Cung_Chuyen_Muc(int cat_id, long News_ID, int top, string relatedNewsId)
        {
            DataTable tbl = new DataTable();
            using (MainDB db = new MainDB())
            {
                tbl = db.StoredProcedures.NP_Select_Tin_Cung_Chuyen_Muc(cat_id, News_ID, top, relatedNewsId);
            }
            return tbl;

        }
        //  END add new store procedure 2016052015

        public static string keyNewID = "News_ID_{0}-{1}";
        public static string keyNewIDHome = "keyNewIDHome_{0}-{1}";
        public static string keyNewIDHomeCat = "keyNewIDHome_Cat";
        public static string NP_NewsID_In_TopHotByCat(int cat_parentid, int cat_id)
        {
            try
            {
                string keyNews_ID = String.Format(keyNewID, cat_parentid, cat_id);
                string result = Utils.Get_MemCache(keyNews_ID) ?? Utils.GetFromCache(keyNews_ID);
                return result != null ? result : String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        //public static string keyNewIDMB = "News_ID_{0}-{1}";
        //public static string keyNewIDHomeMB = "keyNewIDHome_{0}-{1}";
        //public static string keyNewIDHomeCatMB = "keyNewIDHome_Cat";
        //public static string NP_NewsID_In_TopHotByCatMB(int cat_parentid, int cat_id)
        //{
        //    try
        //    {
        //        string keyNews_ID = String.Format(keyNewIDMB, cat_parentid, cat_id);
        //        string result = Utils.Get_MemCache(keyNews_ID) ?? Utils.GetFromCache(keyNews_ID);
        //        return result != null ? result : String.Empty;
        //    }
        //    catch
        //    {
        //        return String.Empty;
        //    }
        //}

        //public static string NP_NewsID_In_TopHotByCat(int cat_parentid, int cat_id)
        //{
        //    try
        //    {
        //        string keyNews_ID = String.Format(keyNewID, cat_parentid, cat_id);
        //        string result = Utils.Get_MemCache(keyNews_ID) ?? Utils.GetFromCache(keyNews_ID);
        //        return result != null ? result : String.Empty;
        //    }
        //    catch
        //    {
        //        return String.Empty;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat_parentid"></param>
        /// <param name="cat_id"></param>
        /// <param name="top"></param>
        /// <param name="ImgWidth"></param>
        /// <param name="news_mode"></param>
        /// <returns></returns>NP_SelectTopHotByCat
        public static List<NewsPublishEntity> NP_SelectListTopHotByCat(int cat_parentid, int cat_id, int top, int ImgWidth, News_Mode news_mode)
        {
            string key = String.Format("NP_SelectListTopHotByCat-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, top, ImgWidth, news_mode.ToString());
            string keyNews_ID = String.Format(keyNewID, cat_parentid, cat_id);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da = NP_SelectTopHotByCat(cat_id, top, news_mode);
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                string news_id = "";
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    news_id += npe.NEWS_ID + ",";
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNews_ID, news_id.TrimEnd(','));
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }


        public static List<NewsPublishEntity> NP_SelectHoaHauTopHotByCat(int cat_parentid, int cat_id, int top, int ImgWidth, News_Mode news_mode)
        {
            string key = String.Format("NP_SelectHoaHauTopHotByCat-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, top, ImgWidth, news_mode.ToString());
            string keyNews_ID = String.Format(keyNewID, cat_parentid, cat_id);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da = new DataTable();
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_SelectHoaHauTopHotByCat(cat_id, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                string news_id = "";
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    news_id += npe.NEWS_ID + ",";
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNews_ID, news_id.TrimEnd(','));
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        /// <summary>
        /// Danh sach tin moi nhat theo cateogry
        /// </summary>
        /// <param name="cat_parentid"></param>
        /// <param name="cat_id"></param>
        /// <param name="top"></param>
        /// <param name="ImgWidth"></param>
        /// <param name="keyNoibat"></param>
        /// <returns></returns>
        public static List<NewsPublishEntity> NP_SelectTin_Moi_Nhat_ByCat(int cat_parentid, int cat_id, int top, int ImgWidth, string keyNoibat)
        {
            string key = String.Format("NP_SelectTin_Moi_Nhat_ByCat-{0}", cat_id);
            string keyNews_ID = String.Format(keyNewIDHome, cat_id, cat_parentid);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                Object news_id = HttpContext.Current.Cache[keyNews_ID];
                using (MainDB db = new MainDB())
                {
                    if (news_id == null) news_id = "";
                    da = db.StoredProcedures.NP_SelectTin_Moi_Nhat_ByCat(cat_id, top, news_id.ToString());
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                news_id = "";
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.News_Rate = Utils.GetObj<Int32>(row["NEWS_Rate"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    news_id += npe.NEWS_ID + ",";
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNews_ID, news_id);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);

                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }
        //thuynt
        public static List<NewsPublishEntity> NP_SelectTin_Moi_Nhat_ByCat_home(int cat_parentid, int cat_id, int top)
        {

            string key = String.Format("NP_SelectTin_Moi_Nhat_ByCat_home-{0}-{1}-{2}", cat_id, cat_parentid, top);
            string keyNews_ID = String.Format(keyNewIDHome, cat_id, cat_parentid);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                Object news_id = HttpContext.Current.Cache[keyNews_ID];
                using (MainDB db = new MainDB())
                {
                    if (news_id == null) news_id = "";
                    da = db.StoredProcedures.NP_SelectTin_Moi_Nhat_ByCat(cat_id, top, news_id.ToString());
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                news_id = "";
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.News_Rate = Utils.GetObj<Int32>(row["NEWS_Rate"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    news_id += npe.NEWS_ID + ",";

                    lst.Add(npe);
                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNews_ID, news_id);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        public static List<NewsPublishEntity> NP_SelectTin_Khac_Fashion(int cat_parentid, int cat_id, int top, int ImgWidth)
        {
            string key = String.Format("NP_SelectTin_Khac_Fashion-{0}-{1}-{2}-{3}", cat_id, cat_parentid, top, ImgWidth);

            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                string keyNews_ID = "";
                DataTable tbl = BOCategory.GetCategoryByParent(cat_id);
                string news_id = "";
                DataRow row;
                if (tbl != null)
                {
                    object obj1;
                    keyNews_ID = String.Format(keyNewID, cat_parentid, cat_id);
                    obj1 = HttpContext.Current.Cache[keyNews_ID];
                    if (obj1 != null) news_id += obj1.ToString();
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        row = tbl.Rows[i];
                        keyNews_ID = String.Format(keyNewID, cat_id, row["Cat_ID"].ToString());
                        obj1 = HttpContext.Current.Cache[keyNews_ID];
                        if (obj1 != null)
                            news_id += "," + obj1.ToString();

                    }
                }

                lst = new List<NewsPublishEntity>();
                DataTable da;

                using (MainDB db = new MainDB())
                {
                    if (news_id == null) news_id = "";
                    da = db.StoredProcedures.NP_SelectTin_Moi_Nhat_ByCat(cat_id, top, news_id.ToString());
                }
                int iCount = da != null ? da.Rows.Count : 0;

                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }


        public static List<NewsPublishEntity> NP_Select_Top_Home(int cat_parentid, int cat_id, int top, int ImgWidth)
        {
            string key = String.Format("NP_Select_Top_Home-{0}-{1}-{2}-{3}", cat_id, cat_parentid, top, ImgWidth);
            string keyNews_ID = String.Format(keyNewIDHome, cat_id, cat_parentid);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Select_Top_Home(cat_id, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                string news_id = "";
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_CONTENT = Utils.CatSapo(Utils.RemoveHTMLTag(Utils.GetObj<string>(row["NEWS_CONTENT"])), 20);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.News_Rate = Utils.GetObj<Int32>(row["NEWS_Rate"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["NEWS_TITLE"]));
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    news_id += npe.NEWS_ID + ",";
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNews_ID, news_id.TrimEnd(','));
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }
        //thuynt add new
        public static List<NewsPublishEntity> NP_Select_Top_HomeCatNotHome(int top, int ImgWidth)
        {
            string key = String.Format("NP_Select_Top_Home-{0}-{1}", top, ImgWidth);
            string keyNews_ID = String.Format(keyNewIDHomeCat);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Select_Top_HomeCatNotHome(top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                string news_id = "";
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_CONTENT = Utils.CatSapo(Utils.RemoveHTMLTag(Utils.GetObj<string>(row["NEWS_CONTENT"])), 20);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.News_Rate = Utils.GetObj<Int32>(row["NEWS_Rate"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["NEWS_TITLE"]));
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    news_id += npe.NEWS_ID + ",";
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNews_ID, news_id.TrimEnd(','));
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }


        public static List<NewsPublishEntity> NoiBatTrangChu(int top, int ImgWidth)
        {
            string key = String.Format("NoiBatTrangChu-{0}-{1}", top, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NoiBatTrangChu(top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                string news_id = "";
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_CONTENT = "";
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["NEWS_TITLE"]));
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    news_id += npe.NEWS_ID + ",";
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, new string[] { TableName.NEWSPUBLISHED, TableName.BONBAINOIBAT }, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        public static List<NewsPublishEntity> GetListBonBaiNoibat(int top, int ImgWidth)
        {
            string key = String.Format("TTOL-GetListBonBaiNoibat-{0}-{1}", top, ImgWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB db = new MainDB())
                    {
                        da = db.StoredProcedures.GetListBonBaiNoibat(top);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    string news_id = "";
                    NewsPublishEntity npe;
                    for (int i = 0; i < iCount; i++)
                    {
                        row = da.Rows[i];
                        npe = new NewsPublishEntity();
                        npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                        npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                        npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                        npe.NEWS_TITLE = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["NEWS_TITLE"]));
                        npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                        news_id += npe.NEWS_ID + ",";
                        npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                        string img = Utils.GetObj<string>(row["NEWS_Image"]);
                        if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                        obj.Add(npe);
                    }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, new string[] { TableName.NEWSPUBLISHED, TableName.BONBAINOIBAT }, key, obj);
                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }
            return obj;
        }

        //public static List<NewsPublishEntity> GetListBonBaiNoibat(int top, int ImgWidth)
        //{
        //    string key = String.Format("GetListBonBaiNoibat-{0}-{1}", top, ImgWidth);
        //    List<NewsPublishEntity> lst;
        //    object obj = HttpContext.Current.Cache[key];
        //    if (obj == null)
        //    {
        //        lst = new List<NewsPublishEntity>();
        //        DataTable da;
        //        using (MainDB db = new MainDB())
        //        {
        //            da = db.StoredProcedures.GetListBonBaiNoibat(top);
        //        }
        //        int iCount = da != null ? da.Rows.Count : 0;
        //        DataRow row;
        //        string news_id = "";
        //        NewsPublishEntity npe;
        //        for (int i = 0; i < iCount; i++)
        //        {
        //            row = da.Rows[i];
        //            npe = new NewsPublishEntity();
        //            npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
        //            npe.NEWS_CONTENT = "";
        //            npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
        //            npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
        //            npe.NEWS_TITLE = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["NEWS_TITLE"]));
        //            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
        //            news_id += npe.NEWS_ID + ",";
        //            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
        //            string img = Utils.GetObj<string>(row["NEWS_Image"]);
        //            if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
        //            lst.Add(npe);
        //        }
        //        Utils.SaveToCacheDependency(TableName.DATABASE_NAME, new string[] { TableName.NEWSPUBLISHED, TableName.BONBAINOIBAT }, key, lst);
        //        return lst;
        //    }
        //    return (List<NewsPublishEntity>)obj;
        //}

        //ducdm add new
        // hiện thị tin theo newmode
        public static List<NewsPublishEntity> NP_Tin_Nong(int cat, int newsMode, int top, int ImgWidth)
        {
            string key = String.Format("NP_Tin_Nong-{0}-{1}-{2}-{3}", cat, newsMode, top, ImgWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB db = new MainDB())
                    {
                        da = db.StoredProcedures.NP_Tin_Nong(cat, newsMode, top);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    for (int i = 0; i < iCount; i++)
                    {
                        row = da.Rows[i];
                        npe = new NewsPublishEntity();
                        npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                        npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                        npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["News_InitContent"]);
                        npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                        npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                        npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                        npe.Cat_Id = Convert.ToInt32(row["Cat_ID"]);
                        npe.ICON = Utils.GetObj<string>(row["ICON"]);
                        npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["NEWS_Image"]);
                        string img = Utils.GetObj<string>(row["NEWS_Image"]);
                        if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                        obj.Add(npe);
                    }

                    Utils.Add_MemCache(key, obj);
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }

            }

            return obj;
        }

        public static List<NewsPublishEntity> GetListNewsByNewsMode2(int cat, int newsMode, int newsMode2, int top, int ImgWidth)
        {
            string key = String.Format("GetListNewsByNewsMode2-{0}-{1}-{2}-{3}-{4}", cat, newsMode, newsMode2, top, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = Utils.GetFromCache<object>(key);
            //if (obj == null)
            //{
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.GetListNewbyNewsMode2(cat, newsMode, newsMode2, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["News_InitContent"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["NEWS_Image"]);
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }

                Utils.Add_MemCache(key, lst);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            //}
            return (List<NewsPublishEntity>)obj;
        }

        public static List<NewsPublishEntity> GetListNewsByNewsMode3(int cat, int newsMode, int newsMode2, int newsMode3, int top, int ImgWidth)
        {
            string key = String.Format("GetListNewsByNewsMode3-{0}-{1}-{2}-{3}-{4}-{5}", cat, newsMode, newsMode2, newsMode3, top, ImgWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB db = new MainDB())
                    {
                        da = db.StoredProcedures.GetListNewbyNewsMode3(cat, newsMode, newsMode2, newsMode3, top);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    for (int i = 0; i < iCount; i++)
                    {
                        row = da.Rows[i];
                        npe = new NewsPublishEntity();
                        npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                        npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                        npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["News_InitContent"]);
                        npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                        npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                        npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                        npe.ICON = Utils.GetObj<string>(row["ICON"]);
                        npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["NEWS_Image"]);
                        string img = Utils.GetObj<string>(row["NEWS_Image"]);
                        if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                        obj.Add(npe);
                    }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }

            return obj;
        }

        //Hiện bài nổi bật mục
        //ducdm
        public static List<NewsPublishEntity> NP_Select_Top_Cat(int cat_parentid, int cat_id, int top, int ImgWidth)
        {
            string key = String.Format("NP_Select_Top_Cat-{0}-{1}-{2}-{3}", cat_id, cat_parentid, top, ImgWidth);
            string keyNews_ID = String.Format(keyNewID, cat_parentid, cat_id);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Select_Top_Cat(cat_id, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                string news_id = "";
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    news_id += npe.NEWS_ID + ",";
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNews_ID, news_id.TrimEnd(','));
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }


        public static List<NewsPublishEntity> NP_Select_Top_Cat_None_Child(int cat_parentid, int cat_id, int top, int ImgWidth)
        {
            string key = String.Format("NP_Select_Top_Cat_None_Child-{0}-{1}-{2}-{3}_", cat_id, cat_parentid, top, ImgWidth);
            List<NewsPublishEntity> lst;
            string keyNews_ID = String.Format(keyNewID, cat_parentid, cat_id);
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Select_Top_Cat_None_Child(cat_id, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                string news_id = "";
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    news_id += npe.NEWS_ID + ",";
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNews_ID, news_id.TrimEnd(','));
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat_parentid"></param>
        /// <param name="cat_id"></param>
        /// <param name="top"></param>
        /// <param name="ImgWidth"></param>
        /// <param name="news_mode"></param>
        /// <returns></returns>NP_SelectTopHotByCat
        public static List<NewsPublishEntity> NP_SelectTopByCat(int cat_parentid, int cat_id, int top, int ImgWidth)
        {
            string key = String.Format("NP_SelectTopByCat-{0}-{1}-{2}-{3}", cat_parentid, cat_id, top, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_SelectTopByCat(cat_id, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                string news_id = "";
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    news_id += npe.NEWS_ID + ",";
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }



        public static List<NewsPublishEntity> NP_SelectTopQnA(int cat_parentid, int cat_id, int top, int ImgWidth, News_Mode news_mode)
        {
            string key = String.Format("NP_Select_Top_QnA-{0}-{1}-{2}-{3}-{4}", cat_parentid, cat_id, top, ImgWidth, news_mode.ToString());
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.Get_Danh_Sach_Tin_QA(cat_id, top, 1);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["News_ImageNote"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_CONTENT = Utils.CatSapo(Utils.RemoveHTMLTag(Utils.GetObj<string>(row["NEWS_CONTENT"])), 23);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        public static int NP_Sao_Danh_Sach_Tin_Count(int cat_parentid, int cat_id, int pageSize)
        {

            string key = String.Format("NP_Sao_Danh_Sach_Tin_Count-{0}-{1}", cat_id, pageSize);
            object result = Utils.Get_MemCache<object>(key) ?? HttpContext.Current.Cache[key];
            if (result == null)
            {
                DataTable tbl;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id);
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin_Count(cat_id, news_ids);
                }
                int Count = tbl != null ? Convert.ToInt32(tbl.Rows[0][0]) : 1;
                Count = Convert.ToInt32((Count - 1) / pageSize) + 1;

                bool resultCache = Utils.Add_MemCache(key, Count, 60 * 24);

                if (!resultCache)
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, Count);
                return Count;
            }
            return Convert.ToInt32(result);
        }

        public static List<String> NP_Sao_Danh_Sach_Tin(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth, string news_id)
        {
            string ITEM = "<div class=\"listnews\">{0}<h1><a href=\"{1}\">{2}</a></h1><h4>{4}</h4><p>{3}</p></div>";
            string ITEM_KHAC = "<li><h1><a href=\"{0}\">{1} ({2})</a></h1></li>";
            string key = String.Format("NP_Sao_Danh_Sach_Tin-{0}-{1}-{2}-{3}-{4}-{5}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth, news_id);
            List<String> lst;
            object obj = HttpContext.Current.Cache[key];
            string TopUp = "";
            string TopDown = "";
            NewsPublishEntity npe;
            if (obj == null)
            {
                lst = new List<String>();
                DataTable tbl = null;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin(cat_id, pageSize, pageIndex, news_id);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    if (i < pageSize / 2)
                    {
                        if (i > 0 && i % 2 == 0 && i < (pageSize / 2) - 1)
                            TopUp += "</div><div class=\"listnews_parent\">";
                        TopUp += String.Format(ITEM, npe.URL_IMG, npe.URL, npe.NEWS_TITLE, Utils.CatSapo(npe.NEWS_INITCONTENT, 25), npe.NEWS_PUBLISHDATE.ToString("dd/MM/yyyy HH:mm"));
                    }
                    else
                    {
                        TopDown += String.Format(ITEM_KHAC, npe.URL, npe.NEWS_TITLE, npe.NEWS_PUBLISHDATE.ToString("dd/MM"));
                    }
                }
                lst.Add(TopUp);
                lst.Add(TopDown);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<String>)obj;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat_parentid"></param>
        /// <param name="cat_id"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="ImgWidth"></param>
        /// <returns></returns>
        public static List<NewsPublishEntity> NP_Danh_Sach_Tin(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("NewsPublishEntity_Sao_Danh_Sach_Tin-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            NewsPublishEntity npe;
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin(cat_id, pageSize, pageIndex, news_ids);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        public static List<NewsPublishEntity> NP_Danh_Sach_Tin_Hidden(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("NP_Danh_Sach_Tin_Hidden-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            NewsPublishEntity npe;
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin_Hidden(cat_id, pageSize, pageIndex, news_ids);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }



        /// <summary>
        /// Lay ra top 5 meo vat theo cat meo vat
        /// </summary>
        /// <param name="cat_parentid"></param>
        /// <param name="cat_id"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="ImgWidth"></param>
        /// <returns></returns>
        public static List<string> NP_Top_Meo_Vat(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("NP_Top_Meo_Vat-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            List<string> obj = HttpContext.Current.Cache[key] as List<string>;
            if (obj == null)
            {
                DataTable tbl = null;
                obj = new List<string>();
                string t1 = string.Empty, t2 = string.Empty;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin_Hidden(cat_id, pageSize, pageIndex, "");
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    Utils.GetObj<string>(row["NEWS_Image"]);
                    t1 += Utils.GetObj<string>(row["NEWS_Image"]) != null ? "<img src=\"" + Utils.GetObj<string>(row["NEWS_Image"]) + "\"/>" : string.Empty;
                    t2 += "<span style='display:none'>" + Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30).Replace("\n", "<br/>") + "</span>";
                    //t2 += "<span style='display:none'><h3>" + Utils.GetObj<string>(row["NEWS_Title"]) + "</h3>" + Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30).Replace("\n", "<br/>") + "</span>";
                }
                obj.Add(t1);
                obj.Add(t2);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                return obj;
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat_parentid"></param>
        /// <param name="cat_id"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="ImgWidth"></param>
        /// <returns></returns>
        public static List<NewsPublishEntity> NP_Danh_Sach_Tin_Hoi_Dap(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("NP_Danh_Sach_Tin_Hoi_Dap-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            NewsPublishEntity npe;
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable tbl = null;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin_QA(cat_id, pageSize, pageIndex);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.NEWS_CONTENT = Utils.CatSapo(Utils.RemoveHTMLTag(Utils.GetObj<string>(row["NEWS_CONTENT"])), 100);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img != null && img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        /// <summary>
        /// Danh sach tin lien quan
        /// </summary>
        /// <param name="cat_parentid">Chuyen mục cha</param>
        /// <param name="cat_id">Chuyen mục chinh</param>
        /// <param name="News_ID">NewsID</param>
        /// <returns></returns>
        public static List<NewsPublishEntity> NP_Tin_Lien_Quan(int cat_parentid, int cat_id, string NEWS_RELATION, int ImgWidth, long newsId)
        {
            if (!string.IsNullOrEmpty(NEWS_RELATION))
            {
                string key = String.Format("TTOL_NP_Tin_Lien_Quan-{0}", newsId);
                var lst = Utils.Get_MemCache<List<NewsPublishEntity>>(key) ?? Utils.GetFromCache<List<NewsPublishEntity>>(key);
                if (lst == null || lst.Count == 0)
                {
                    lst = new List<NewsPublishEntity>();
                    NEWS_RELATION = NEWS_RELATION.TrimEnd(',');
                    DataTable da = NP_Tin_Lien_Quan(NEWS_RELATION);
                    int iCount = da != null ? da.Rows.Count : 0;
                    if (da != null)
                    {
                        for (int i = 0; i < iCount; i++)
                        {
                            var row = da.Rows[i];
                            var npe = new NewsPublishEntity();
                            npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                            npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);

                            string img = Utils.GetObj<string>(row["NEWS_Image"]);
                            if (img != null)
                            {
                                if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                            }

                            lst.Add(npe);
                        }
                    }
                    if (!Utils.Add_MemCache(key, lst))
                        Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                }
                return lst;
            }
            else
            {
                return new List<NewsPublishEntity>();
            }
        }

        public static List<NewsPublishEntity> Sao_Hit_Cau_Hoi(int cat_parentid, int cat_id, int top, int ImgWidth)
        {
            string key = String.Format("Sao_Hit_Cau_Hoi-{0}-{1}-{2}-{3}", cat_id, cat_parentid, top, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.Sao_Hit_Cau_Hoi(cat_id, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }


        public static List<NewsPublishEntity> NP_Select_Tin_Tieu_Diem(int cat_parentid, int cat_id, int top, int ImgWidth)
        {
            string key = String.Format("NP_Select_Tin_Tieu_Diem-{0}-{1}-{2}-{3}", cat_id, cat_parentid, top, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Select_Tin_Tieu_Diem(cat_id, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        public static string TIN_TIEU_DIEM = "NP_Select_Tin_Tieu_Diem_String-{0}";
        public static List<NewsPublishEntity> NP_Select_Tin_Tieu_Diem_String(int cat_parentid, int cat_id, int top, int ImgWidth, string bainoibat)
        {
            //string key = String.Format(TIN_TIEU_DIEM, cat_id);

            //string html = Utils.Get_MemCache<String>(key) ?? Utils.GetFromCache(key);
            //if (html == null)
            //{

            //    string DONG_SU_KIEN_ITEM = "<li>{0}<h1><a href=\"{1}\">{2}</a></h1></li>";

            //    DataTable da;
            //    using (MainDB db = new MainDB())
            //    {
            //        da = db.StoredProcedures.NP_Select_Tin_Tieu_Diem(cat_id, top);
            //    }
            //    int iCount = da != null ? da.Rows.Count : 0;
            //    DataRow row;
            //    NewsPublishEntity npe;
            //    for (int i = 0; i < iCount; i++)
            //    {
            //        row = da.Rows[i];
            //        npe = new NewsPublishEntity();
            //        npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
            //        npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
            //        npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
            //        npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
            //        npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
            //        string img = Utils.GetObj<string>(row["NEWS_Image"]);
            //        if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);

            //        html += String.Format(DONG_SU_KIEN_ITEM, npe.URL_IMG, npe.URL, npe.NEWS_TITLE);
            //    }
            //    //
            //    // 
            //    bool result = Utils.Add_MemCache(key, html);

            //    if (!result)
            //        Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);

            //    return html;
            //}
            //return html;


            string key = String.Format("NP_Select_Tin_Tieu_Diem-{0}", cat_id);
            string keyNews_ID = String.Format(keyNewID, cat_parentid, cat_id);
            List<NewsPublishEntity> lst;
            object obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key) ?? HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                Object news_id = HttpContext.Current.Cache[bainoibat];
                using (MainDB db = new MainDB())
                {
                    if (news_id == null) news_id = ""; da = db.StoredProcedures.NP_Select_Tin_Tieu_Diem(cat_id, top);
                    da = db.StoredProcedures.NP_Select_Tin_Tieu_Diem(cat_id, top);

                    // da = db.StoredProcedures.NP_SelectTin_Moi_Nhat_ByCat(cat_id, top, news_id.ToString());
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                news_id = "";
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }

                if (!Utils.Add_MemCache(key, lst))
                {
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNews_ID, news_id);
                }
                return lst;
            }
            return (List<NewsPublishEntity>)obj;

        }


        public static string ClipCategory(int cat_parentid, int cat_id, int top, int ImgWidth)
        {
            string key = String.Format("ClipCategory" + TIN_TIEU_DIEM, cat_id + "-" + top);

            string html = Utils.Get_MemCache<String>(key) ?? Utils.GetFromCache(key);
            if (html == null)
            {

                string DONG_SU_KIEN_ITEM = "<li style=\"{3}\">{0}<h3><a href=\"{1}\">{2}</a></h3></li>";

                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Select_Top_Cat(cat_id, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);

                    html += String.Format(DONG_SU_KIEN_ITEM, npe.URL_IMG, npe.URL, npe.NEWS_TITLE, i == 1 ? "padding:0 15px" : "");
                }
                //
                // 
                bool result = Utils.Add_MemCache(key, html);

                if (!result)
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);

                return html;
            }
            return html;
        }

        //public static string LBDomain = ConfigurationSettings.AppSettings["LBDomain"] ?? "img.2sao.vn|183.91.14.17";
        //public static NewsPublishEntity NP_TinChiTiet_link(int top)
        //{
        //    int cat_parentid;
        //    int cat_id;
        //    string key = String.Format(ATVCommon.Constants.CACHE_NAME_NEWS_DETAIL, top);
        //    NewsPublishEntity npe = Utils.Get_MemCache<NewsPublishEntity>(key) ?? Utils.GetFromCache<NewsPublishEntity>(key);
        //    if (npe == null)
        //    {
        //        DataTable tbl = new DataTable();

        //        using (MainDB db = new MainDB())
        //        {
        //            tbl = db.StoredProcedures.NP_TinChiTiet(News_ID);
        //            tbl = db.StoredProcedures.NP_Xem_Nhieu_Nhat(5);
        //        }

        //        int iCount = tbl != null ? tbl.Rows.Count : 0;
        //        npe = new NewsPublishEntity();
        //        if (iCount > 0)
        //        {
        //            DataRow row;
        //            row = tbl.Rows[0];
        //            npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
        //            npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
        //            npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
        //            npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["News_ImageNote"]);
        //            npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
        //            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
        //            npe.ICON = Utils.GetObj<string>(row["ICON"]);
        //          //  npe.News_Rate = Utils.GetObj<Int32>(row["NEWS_Rate"]);
        //            npe.Cat_Id = cat_id = Utils.GetObj<Int32>(row["Cat_Id"]);
        //            npe.Cat_ParentId = cat_parentid = Utils.GetObj<Int32>(row["Cat_ParentId"]);
        //            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
        //            npe.Imgage = new ImageEntity(100, Utils.GetObj<string>(row["NEWS_IMAGE"]));
        //            string content = Utils.GetObj<string>(row["News_Content"]).Replace("//<![CDATA[", "").Replace("//]]>", "");

        //            var lstDomain = LBDomain.Split('|');

        //            if (npe.NEWS_PUBLISHDATE < new DateTime(2010, 7, 8, 23, 59, 00) && lstDomain.Length > 0)
        //            {
        //                content = content.Replace("img.2sao.vietnamnet.vn", lstDomain[0]);
        //            }
        //            else if (npe.NEWS_PUBLISHDATE < DateTime.Now.AddDays(-3) && lstDomain.Length > 1)
        //            {
        //                content = content.Replace("img.2sao.vietnamnet.vn", lstDomain[1]);
        //            }

        //            npe.NEWS_CONTENT = content;
        //            npe.Keywrods = Utils.GetObj<string>(row["Extension3"]);
        //            npe.NEWS_RELATION = NP_Tin_Lien_Quan(cat_parentid, cat_id, Utils.GetObj<string>(row["NEWS_RELATION"]), 200);

        //            //var tmpKeyword = new List<string>();
        //            //try
        //            //{
        //            //    tmpKeyword = NGramKeyword.GetNGramKeyword(npe.NEWS_TITLE, npe.NEWS_INITCONTENT, npe.NEWS_CONTENT, 10);
        //            //}
        //            //catch
        //            //{
        //            //    tmpKeyword = new List<string>();
        //            //}


        //            //npe.Keywrods = string.Concat(npe.Keywrods, " ", String.Join(", ", tmpKeyword.ToArray()));


        //            if (npe.Keywrods.Length > 0)
        //            {
        //                string[] arStrings = npe.Keywrods.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //                for (int i = 0; i < arStrings.Length; i++)
        //                {
        //                    if (!String.IsNullOrEmpty(arStrings[i]))
        //                        arStrings[i] = String.Format("<a title=\"{2}\" href=\"http://sao.tintuconline.com.vn/{0}.search\">{1}</a>", arStrings[i].Trim().Replace(" ", "-"), HttpUtility.HtmlEncode(arStrings[i].Trim()), HttpUtility.HtmlEncode(arStrings[i].Trim()));
        //                }

        //                npe.Keywrods = String.Join(", ", arStrings);
        //            }

        //            string news_otherCat = Utils.GetObj<String>(row["News_OtherCat"]);
        //            if (news_otherCat.Length > 0)
        //            {
        //                string[] otherCat = news_otherCat.Split(',');
        //                foreach (var s in otherCat)
        //                {
        //                    if (Utils.IsNumber(s))
        //                    {
        //                        npe.NEWS_OTHERCAT.Add(Convert.ToInt32(s));
        //                    }
        //                }
        //            }
        //        }

        //        bool isSuccess = Utils.Add_MemCache(key, npe);

        //        if (!isSuccess)
        //            Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, npe);

        //        return npe;
        //    }
        //    return npe;
        //}
        private static string AdsContent(string input)
        {
            string origin = input;
            try
            {
                const string appendDiv = "<div id=\"vmcbackground\"></div>";
                const string appendDivExtend = "<div id=\"vmcbackgroundExtend\"></div>";
                string output = string.Empty;
                var doc = new HtmlDocument();
                doc.LoadHtml(input);
                int tryCount = 0;
                while (tryCount < 5 && doc.DocumentNode.ChildNodes.Count == 1)
                {
                    doc.DocumentNode.InnerHtml = doc.DocumentNode.ChildNodes[0].InnerHtml;
                    tryCount++;
                }

                if (doc.DocumentNode.ChildNodes.Count >= 1)
                {
                    if (doc.DocumentNode.ChildNodes.Count > 5)
                    {
                        for (int i = 0; i < doc.DocumentNode.ChildNodes.Count; i++)
                        {
                            if (i == 0) output += "<div id=\"abdf\"> <p class=\"pcontent\">";
                            output += doc.DocumentNode.ChildNodes[i].OuterHtml;
                            if (doc.DocumentNode.ChildNodes.Count > 9)
                            {
                                if (i == 9)
                                {
                                    output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                }
                                if (doc.DocumentNode.ChildNodes.Count > 14)
                                {
                                    if (i == 14)
                                    {
                                        output += "</p></div>" + appendDivExtend + "<div id=\"abdi\">";
                                    }
                                }
                            }
                            else
                            {
                                if (i == 5)
                                {
                                    output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                }
                            }

                        }
                    }
                    else if (doc.DocumentNode.ChildNodes[0].ChildNodes.Count > 3 || doc.DocumentNode.ChildNodes[1].ChildNodes.Count > 3)
                    {
                        for (int i = 0; i < doc.DocumentNode.ChildNodes.Count; i++)
                        {
                            if (i == 0) output += "<div id=\"abdf\"> <p class=\"pcontent\">";
                            else if (i == 1 && doc.DocumentNode.ChildNodes[0].ChildNodes.Count > 3)
                            {
                                for (int j = 0; j < doc.DocumentNode.ChildNodes[0].ChildNodes.Count; j++)
                                {
                                    output += doc.DocumentNode.ChildNodes[0].ChildNodes[j].OuterHtml;
                                    if (doc.DocumentNode.ChildNodes[0].ChildNodes.Count < 9)
                                    {
                                        if (j == 5)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                    }
                                    else
                                    {
                                        if (j == 9)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                        if (doc.DocumentNode.ChildNodes[0].ChildNodes.Count > 14)
                                        {
                                            if (j == 14)
                                            {
                                                output += "</p></div>" + appendDivExtend + "<div id=\"abdi\">";
                                            }
                                        }
                                    }
                                }
                            }
                            else if (i == 1 && doc.DocumentNode.ChildNodes[1].ChildNodes.Count > 3)
                            {
                                for (int j = 0; j < doc.DocumentNode.ChildNodes[1].ChildNodes.Count; j++)
                                {
                                    output += doc.DocumentNode.ChildNodes[1].ChildNodes[j].OuterHtml;
                                    if (doc.DocumentNode.ChildNodes[1].ChildNodes.Count < 9)
                                    {
                                        if (j == 5)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                    }
                                    else
                                    {
                                        if (j == 9)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                        if (doc.DocumentNode.ChildNodes[1].ChildNodes.Count > 14)
                                        {
                                            if (j == 14)
                                            {
                                                output += "</p></div>" + appendDivExtend + "<div id=\"abdi\">";
                                            }
                                        }
                                    }
                                }
                            }
                            else if (i == 2 && doc.DocumentNode.ChildNodes[1].ChildNodes.Count > 3)
                            {
                                for (int j = 0; j < doc.DocumentNode.ChildNodes[1].ChildNodes.Count; j++)
                                {
                                    output += doc.DocumentNode.ChildNodes[1].ChildNodes[j].OuterHtml;
                                    if (doc.DocumentNode.ChildNodes[1].ChildNodes.Count < 9)
                                    {
                                        if (j == 5)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                    }
                                    else
                                    {
                                        if (j == 9)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                        if (doc.DocumentNode.ChildNodes[1].ChildNodes.Count > 14)
                                        {
                                            if (j == 14)
                                            {
                                                output += "</p></div>" + appendDivExtend + "<div id=\"abdi\">";
                                            }
                                        }
                                    }
                                }
                            }
                            else if (i == 2 && doc.DocumentNode.ChildNodes[2].ChildNodes.Count > 3)
                            {
                                for (int j = 0; j < doc.DocumentNode.ChildNodes[2].ChildNodes.Count; j++)
                                {
                                    output += doc.DocumentNode.ChildNodes[2].ChildNodes[j].OuterHtml;
                                    if (doc.DocumentNode.ChildNodes[2].ChildNodes.Count < 9)
                                    {
                                        if (j == 5)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                    }
                                    else
                                    {
                                        if (j == 9)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                        if (doc.DocumentNode.ChildNodes[2].ChildNodes.Count > 14)
                                        {
                                            if (j == 14)
                                            {
                                                output += "</p></div>" + appendDivExtend + "<div id=\"abdi\">";
                                            }
                                        }
                                    }
                                }
                            }
                            else if (i == 3 && doc.DocumentNode.ChildNodes[2].ChildNodes.Count > 3)
                            {
                                for (int j = 0; j < doc.DocumentNode.ChildNodes[2].ChildNodes.Count; j++)
                                {
                                    output += doc.DocumentNode.ChildNodes[2].ChildNodes[j].OuterHtml;
                                    if (doc.DocumentNode.ChildNodes[2].ChildNodes.Count < 9)
                                    {
                                        if (j == 5)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                    }
                                    else
                                    {
                                        if (j == 9)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                        if (doc.DocumentNode.ChildNodes[2].ChildNodes.Count > 14)
                                        {
                                            if (j == 14)
                                            {
                                                output += "</p></div>" + appendDivExtend + "<div id=\"abdi\">";
                                            }
                                        }
                                    }
                                }
                            }
                            else if (i == 3 && doc.DocumentNode.ChildNodes[3].ChildNodes.Count > 3)
                            {
                                for (int j = 0; j < doc.DocumentNode.ChildNodes[3].ChildNodes.Count; j++)
                                {
                                    output += doc.DocumentNode.ChildNodes[3].ChildNodes[j].OuterHtml;
                                    if (doc.DocumentNode.ChildNodes[3].ChildNodes.Count < 9)
                                    {
                                        if (j == 5)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                    }
                                    else
                                    {
                                        if (j == 9)
                                        {
                                            output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                        }
                                        if (doc.DocumentNode.ChildNodes[3].ChildNodes.Count > 14)
                                        {
                                            if (j == 14)
                                            {
                                                output += "</p></div>" + appendDivExtend + "<div id=\"abdi\">";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                output += doc.DocumentNode.ChildNodes[i].OuterHtml;
                            }
                        }
                    }
                    else if (doc.DocumentNode.ChildNodes[1].ChildNodes[0].ChildNodes.Count > 3)
                    {
                        for (int i = 0; i < doc.DocumentNode.ChildNodes[1].ChildNodes[0].ChildNodes.Count; i++)
                        {
                            if (i == 0) output += "<div id=\"abdf\"> <p class=\"pcontent\">";
                            output += doc.DocumentNode.ChildNodes[1].ChildNodes[0].ChildNodes[i].OuterHtml;
                            if (doc.DocumentNode.ChildNodes[1].ChildNodes[0].ChildNodes.Count > 9)
                            {
                                if (i == 9)
                                {
                                    output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                }
                                if (doc.DocumentNode.ChildNodes[1].ChildNodes[0].ChildNodes.Count > 14)
                                {
                                    if (i == 14)
                                    {
                                        output += "</p></div>" + appendDivExtend + "<div id=\"abdi\">";
                                    }
                                }
                            }
                            else
                            {
                                if (i == 5)
                                {
                                    output += "</p></div>" + appendDiv + "<div id=\"abde\">";
                                }

                            }

                        }
                    }



                    output += "</div>";
                }
                else return origin;
                return output;
            }
            catch (Exception ex)
            {
                return origin;
            }

        }

        public static string LBDomain = ConfigurationSettings.AppSettings["LBDomain"] ?? "";
        public static NewsPublishEntity NP_TinChiTiet(long News_ID, bool isMobile)
        {
            int cat_parentid;
            int cat_id;
            string key = String.Format(ATVCommon.Constants.CACHE_NAME_NEWS_DETAIL, News_ID);
            NewsPublishEntity npe = Utils.Get_MemCache<NewsPublishEntity>(key) ?? Utils.GetFromCache<NewsPublishEntity>(key);
            if (npe == null)
            {
                DataTable tbl = new DataTable();

                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_TinChiTiet(News_ID);
                }

                int iCount = tbl != null ? tbl.Rows.Count : 0;
                npe = new NewsPublishEntity();
                if (iCount > 0)
                {
                    DataRow row;
                    row = tbl.Rows[0];
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    //npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["News_ImageNote"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["NEWS_IMAGE"]);
                    npe.News_Rate = Utils.GetObj<Int32>(row["NEWS_Rate"]);
                    npe.Cat_Id = cat_id = Utils.GetObj<Int32>(row["Cat_Id"]);
                    npe.Cat_ParentId = cat_parentid = Utils.GetObj<Int32>(row["Cat_ParentId"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    npe.Imgage = new ImageEntity(100, Utils.GetObj<string>(row["NEWS_IMAGE"]));
                    npe.NEWS_ATHOR = Utils.GetObj<string>(row["News_Athor"]);
                    string content = Utils.GetObj<string>(row["News_Content"]).Replace("//<![CDATA[", "").Replace("//]]>", "");
                    npe.NEWS_CONTENT = AdsContent(content);
                    npe.Keywrods = Utils.GetObj<string>(row["Extension2"]);
                    npe.NEWS_RELATION = NP_Tin_Lien_Quan(cat_parentid, cat_id, Utils.GetObj<string>(row["NEWS_RELATION"]), 200, npe.NEWS_ID);

                    if (!string.IsNullOrEmpty(npe.Keywrods))
                    {
                        string[] arStrings = npe.Keywrods.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < arStrings.Length; i++)
                        {
                            if (!String.IsNullOrEmpty(arStrings[i]))
                            {
                                arStrings[i] = String.Format("<a title=\"{2}\" href=\"/tag/{0}.html\">{1}</a>", HttpUtility.UrlEncode(arStrings[i].Trim()), arStrings[i].Trim(), HttpUtility.HtmlEncode(arStrings[i].Trim()));
                            }

                        }

                        npe.Keywrods = String.Join("", arStrings);
                    }

                    string newsOtherCat = Utils.GetObj<String>(row["News_OtherCat"]);
                    if (!string.IsNullOrEmpty(newsOtherCat))
                    {
                        string[] otherCat = newsOtherCat.Split(',');
                        foreach (var s in otherCat)
                        {
                            if (Utils.IsNumber(s))
                            {
                                npe.NEWS_OTHERCAT.Add(Convert.ToInt32(s));
                            }
                        }
                    }
                }

                bool isSuccess = Utils.Add_MemCache(key, npe);

                if (!isSuccess)
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, npe);

                return npe;
            }
            return npe;
        }

        public static NewsPublishEntity NP_TinChiTiet301(long News_ID)
        {
            string key = String.Format("_NP_TinChiTiet-{0}", News_ID);
            NewsPublishEntity npe;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_TinChiTiet(News_ID);
                }

                int iCount = tbl != null ? tbl.Rows.Count : 0;
                npe = new NewsPublishEntity();
                if (iCount > 0)
                {
                    DataRow row;
                    row = tbl.Rows[0];
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["News_ImageNote"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.Cat_Id = Utils.GetObj<Int32>(row["Cat_Id"]);
                    npe.Cat_ParentId = Utils.GetObj<Int32>(row["Cat_ParentId"]);
                    npe.Imgage = new ImageEntity(100, Utils.GetObj<string>(row["NEWS_IMAGE"]));
                    npe.NEWS_CONTENT = Utils.GetObj<string>(row["News_Content"]);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, npe);
                return npe;
            }
            return (NewsPublishEntity)obj;
        }

        public static string BuildLink(int Cat_ParentID, int Cat_ID, NewsPublishEntity nep)
        {
            return BuildLink(Cat_ParentID, Cat_ID, nep.NEWS_ID, HttpUtility.HtmlDecode(nep.NEWS_TITLE));
        }


        /// <summary>
        /// Sửa lại code build link này là ok hêt
        /// </summary>
        /// <param name="Cat_ParentID"></param>
        /// <param name="Cat_ID"></param>
        /// <param name="news_id"></param>
        /// <param name="news_title"></param>
        /// <returns></returns>
        public static string BuildLink(int Cat_ParentID, int Cat_ID, long news_id, string news_title)
        {

            var e = BOCategory.GetCategory(Cat_ID);

            if (e != null && !String.IsNullOrEmpty(e.Cat_DisplayURL))
                return BuildLink(Utils.UnicodeToKoDauAndGach(e.Cat_Name.ToLower()), news_title, news_id, Cat_ParentID, Cat_ID);

            return "/";
        }

        static Dictionary<int, string> CategoryUrl = new Dictionary<int, string>();

        public static string BuildLink(string cat_Name, string news_title, long news_id, int parent, int cat)
        {
            if (cat == 78)
            {
                if (HttpContext.Current.Request.Url.DnsSafeHost.ToLower() == "m.netlife.com.vn")
                {
                    return String.Format(STRING_CAT_NAME_URL, cat_Name, Utils.UnicodeToKoDauAndGach(news_title), news_id, cat);
                }
                else
                {
                    return String.Format("http://netlife.com.vn" + STRING_CAT_NAME_URL, cat_Name, Utils.UnicodeToKoDauAndGach(news_title), news_id, cat);
                }
            }
            else
            {
                if (HttpContext.Current.Request.Url.DnsSafeHost.ToLower() == "m.netlife.com.vn")
                {
                    return String.Format("http://m.netlife.com.vn" + STRING_CAT_NAME_URL, cat_Name, Utils.UnicodeToKoDauAndGach(news_title), news_id, cat);
                }
            }
            return String.Format(STRING_CAT_NAME_URL, cat_Name, Utils.UnicodeToKoDauAndGach(news_title), news_id, cat);
        }


        #region  Dong su kien
        public static List<NewsPublishEntity> NP_Dong_Su_Kien(int pageZise, int pageIndex, int threadId, int ImgWidth)
        {
            string key = String.Format("NP_Dong_Su_Kien-{0}-{1}-{2}-{3}", pageZise, pageIndex, threadId, ImgWidth);
            var obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
            if (obj == null)
            {
                obj = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Dong_Su_Kien(threadId, pageZise, pageIndex);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    obj.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.Vote, key, obj);
                return obj;
            }
            return obj;
        }

        public static int NP_Dong_Su_Kien_Count(int threadId, int pageZise)
        {
            string key = String.Format("NP_Dong_Su_Kien_Count-{0}-{1}", threadId, pageZise);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Dong_Su_Kien_Count(threadId);
                }
                int iCount = (da != null && da.Rows.Count > 0) ? ((Convert.ToInt32(da.Rows[0][0]) - 1) / pageZise + 1) : 0;

                //int Count = tbl != null ? Convert.ToInt32(tbl.Rows[0][0]) : 1;
                //Count = Convert.ToInt32((Count - 1) / pageSize) + 1;

                //bool resultCache = Utils.Add_MemCache(key, Count, 60 * 24);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.Vote, key, iCount);
                return Convert.ToInt32(iCount);
            }
            return Convert.ToInt32(obj);
        }


        public static int TTOL_Dong_Su_Kien_All_Count(int pageZise)
        {
            string key = String.Format("TTOL_Dong_Su_Kien_All_Count-{0}", pageZise);
            List<NewsPublishEntity> lst;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.TTOL_Dong_Su_Kien_All_Count();
                }
                int iCount = (da != null && da.Rows.Count > 0) ? ((Convert.ToInt32(da.Rows[0][0]) - 1) / pageZise + 1) : 0;

                //int Count = tbl != null ? Convert.ToInt32(tbl.Rows[0][0]) : 1;
                //Count = Convert.ToInt32((Count - 1) / pageSize) + 1;

                //bool resultCache = Utils.Add_MemCache(key, Count, 60 * 24);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, iCount);
                return Convert.ToInt32(iCount);
            }
            return Convert.ToInt32(obj);
        }

        #endregion


        public static List<NewsPublishEntity> NP_SelectTopTinDon(int _cat_parentid, int _cat_id, int top, bool ishome)
        {
            string key = String.Format("NP_SelectTopTinDon-{0}-{1}", _cat_id, top);
            List<NewsPublishEntity> lst = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
            if (lst == null || lst == default(List<NewsPublishEntity>))
            {
                DataTable tbl;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_SelectTopTinDon(_cat_id, top, ishome);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                lst = new List<NewsPublishEntity>();
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);

            }
            return lst;

        }





        public static void Sao_UpdateVoteById(int VoteItemId, string other, int VoteId)
        {
            DataTable tbl;
            using (MainDB db = new MainDB())
            {
                tbl = db.StoredProcedures.Sao_UpdateVoteById(VoteItemId, other, VoteId);
            }
            if (tbl == null) tbl = new DataTable();
            Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.VoteItem, String.Format("Sao_SelectVoteItemByVoteId-{0}-true", VoteId), tbl);
        }

        public static void QuaTang(string hoten, string ngaysinh, string email, string diachi, string cmt, string dienthoai)
        {
            using (MainDB db = new MainDB())
            {
                db.StoredProcedures.QuaTang(hoten, ngaysinh, email, diachi, cmt, dienthoai);
            }
        }

        public static List<NewsPublishEntity> NP_Select_Tin_Moi(int cat_parentid, int cat_id, long News_ID, int top, int imgWidth)
        {
            string key = String.Format("NP_Select_Tin_Moi-{0}-{1}-{2}-{3}-{4}", cat_parentid, cat_id, News_ID, top, imgWidth);
            object obj = Utils.Get_MemCache<object>(key) ?? Utils.GetFromCache<object>(key);
            if (obj == null)
            {
                var lst = new List<NewsPublishEntity>();
                DataTable da = null;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Select_Tin_Moi(cat_id, News_ID, top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                for (int i = 0; i < iCount; i++)
                {
                    DataRow row = da.Rows[i];
                    var npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                    npe.Imgage = new ImageEntity(imgWidth, Utils.GetObj<string>(row["NEWS_Image"]));
                    lst.Add(npe);
                }
                var result = Utils.Add_MemCache(key, lst);
                if (!result)
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;

        }

        public static DataTable NP_Search(string key, int page, int catid)
        {

            QASearchEntity qae = new QASearchEntity();
            qae.FDate = new DateTime(2000, 01, 01);
            qae.EDate = DateTime.Now;
            qae.KeySearch = HttpUtility.UrlDecode(key);
            qae.PageIndex = page < 1 ? 1 : page;
            qae.PageSize = 20;
            qae.Cat = catid;
            DataTable tbl = new DataTable();
            using (MainDB db = new MainDB())
            {
                tbl = db.StoredProcedures.News_DoAdvanceSearch(qae);
            }
            int icount = tbl != null ? tbl.Rows.Count : 0;
            if (icount == 0) return new DataTable();
            if (!tbl.Columns.Contains("url")) tbl.Columns.Add("url");

            DataRow row;
            for (int i = 0; i < icount; i++)
            {
                row = tbl.Rows[i];
                row["url"] = String.Format("/p{0}c{1}n{2}/{3}.html", row["cat_parentid"].ToString(), row["cat_id"].ToString(), row["news_id"].ToString(), Utils.UnicodeToKoDauAndGach(row["news_title"].ToString()));
                row["news_image"] = (row["news_image"] != null && row["news_image"].ToString().Length > 0) ? Utils.GetThumbNail(row["News_title"].ToString(), row["url"].ToString(), row["news_image"].ToString(), 120) : string.Empty;
                tbl.AcceptChanges();
            }
            return tbl;

        }

        public static int NP_SearchCount(string key, int pageSize, int catid)
        {
            QASearchEntity qae = new QASearchEntity();
            qae.FDate = new DateTime(2000, 01, 01);
            qae.EDate = DateTime.Now;
            qae.KeySearch = HttpUtility.UrlDecode(key);
            qae.PageSize = 20;
            qae.Cat = catid;
            DataTable tbl = new DataTable();
            using (MainDB db = new MainDB())
            {
                tbl = db.StoredProcedures.News_DoAdvanceSearchCount(qae);
            }
            int icount = (tbl != null && tbl.Rows.Count > 0) ? Convert.ToInt32(tbl.Rows[0][0]) : 0;
            return Convert.ToInt32((icount - 1) / pageSize + 1);
        }

        #region
        public static string MusicCacheBindData(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("MusicCacheBindData-{0}-{1}-{2}-{3}-{4}", cat_parentid, cat_id, pageSize, pageIndex, ImgWidth);
            string html = Utils.GetFromCache<String>(key);
            if (html == null || html.Length == 0)
            {
                List<NewsPublishEntity> lst = BOATV.NewsPublished.NP_Danh_Sach_Tin(cat_parentid, cat_id, pageSize, pageIndex, ImgWidth);

                NewsPublishEntity nep;
                string ITEM = "<div class=\"{5}\">{0}<h1><a href=\"{1}\">{2}</a></h1><h4>{3}</h4><p>{4}</p></div>";
                string css = "";
                int iCount = lst != null ? lst.Count : 0;
                for (int i = 0; i < iCount; i++)
                {
                    nep = lst[i];
                    css = "listnews";
                    html += String.Format(ITEM, nep.URL_IMG, nep.URL, nep.NEWS_TITLE, nep.NEWS_PUBLISHDATE.ToString("dd/MM/yyyy HH:ss"), nep.NEWS_INITCONTENT, css);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
            }
            return html;
        }
        #endregion

        //public const string DST = @"<div class=""{5}"">{0}<h3><a href=""{1}"" title=""{2}"">{2}</a></h3><span>{3}</span><p>{4}</p></div>";
        //const string DST = @"<div title=""{0}"">{1}<h2 title=""{0}""><a title=""{0}"" href=""{2}"">{0}</a></h2><h4 data=""{5}"" title=""{0}"">{3}</h4><p title=""{0}"">{4}</p></div>";
        const string DST = @"<li class=""lilist"">{0}<div class=""divnav2""> <a href=""{1}"" title=""{2}"">{2}</a><p>{3}</p><p class=""psapo"">{4}</p> </div> </li>";
        //mobile
        private const string mobileline = @"<div class=""col-md-3 floatleft borderline morepadding""> <div class=""portfolio app mix_all""> {0} </div> <div class=""portfolio icon mix_all""><h4><a href=""{1}"" title=""{2}"">{2}</a> </h4> </div></div>";

        public static string String_Danh_Sach_Tin_Mobile(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("String_Danh_Sach_Tin_Mobile-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            string keyNewwId = String.Format(keyNewID, cat_parentid, cat_id);
            string html = String.Empty;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin(cat_id, pageSize, pageIndex, news_ids);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                if (iCount > 0 && iCount % 2 != 0) iCount--;
                DataRow row;
                string temp = String.Empty;
                string title = String.Empty;
                string url = String.Empty;
                string img = String.Empty;
                for (int i = 1; i <= iCount; i++)
                {
                    row = tbl.Rows[i - 1];
                    title = row["NEWS_TITLE"] != null ? HttpUtility.HtmlEncode(row["NEWS_TITLE"].ToString()) : String.Empty;
                    url = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), title);
                    img = Utils.GetThumbNail(title, url, row["NEWS_Image"] != null ? row["NEWS_Image"].ToString() : String.Empty, ImgWidth);
                    if (String.IsNullOrEmpty(title)) temp += "<div class=\"portfolio app mix_all\"></div> <div class=\"portfolio icon mix_all\"><h4><a href=\"\" title=\"\"></a> </h4> </div>";
                    else
                    {
                        var pd = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                        temp += String.Format(mobileline, img, url, title, Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]).ToString("dd/MM/yyyy"), Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30), (String.Format("/d{0}-{1}-{2}/p{3}c{4}/{5}.html", pd.Day, pd.Month, pd.Year, Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, Lib.QueryString.CategoryName)));
                    }
                    if (i > 0 && i % 2 == 0)
                    {
                        html += String.Format("{0}", temp);
                        temp = String.Empty;
                    }
                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return obj.ToString();
        }


        public static string String_Danh_Sach_Tin(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("String_Danh_Sach_Tin-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            string keyNewwId = String.Format(keyNewID, cat_parentid, cat_id);
            string html = String.Empty;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin(cat_id, pageSize, pageIndex, news_ids);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                if (iCount > 0 && iCount % 2 != 0) iCount--;
                DataRow row;
                string temp = String.Empty;
                string title = String.Empty;
                string url = String.Empty;
                string img = String.Empty;
                for (int i = 1; i <= iCount; i++)
                {
                    row = tbl.Rows[i - 1];
                    title = row["NEWS_TITLE"] != null ? HttpUtility.HtmlEncode(row["NEWS_TITLE"].ToString()) : String.Empty;
                    url = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), title);
                    img = Utils.GetThumbNail(title, url, row["NEWS_Image"] != null ? row["NEWS_Image"].ToString() : String.Empty, ImgWidth);
                    if (String.IsNullOrEmpty(title)) temp += "<li class=\"lilist\"></li>";
                    else
                    {
                        var pd = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                        temp += String.Format(DST, img, url, title, Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]).ToString("dd/MM/yyyy"), Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30), (String.Format("/d{0}-{1}-{2}/p{3}c{4}/{5}.html", pd.Day, pd.Month, pd.Year, Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, Lib.QueryString.CategoryName)));
                        // const string DST = @"<li class=""lilist"">{0}<div class=""divnav2""> <a href=""{1}"" title=""{2}"">{2}</a><p>{3}</p><p class=""psapo"">{4}</p> </div> </li>";
                    }
                    if (i > 0 && i % 2 == 0)
                    {
                        html += String.Format("{0}", temp);
                        temp = String.Empty;
                    }
                }

                //Chỉ nạp memcache cho những trang đầu tiên PageIndex <=10

                //bool result = Utils.Add_MemCache(key, html) && (pageIndex <= 10);
                //if (!result)
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return obj.ToString();
        }




        public static string String_Danh_Sach_Tin_Lien_Quan(List<NewsPublishEntity> entities, int ImgWidth)
        {
            DataTable tbl = null;
            int iCount = entities.Count;
            string temp = "", html = "";
            if (iCount > 0 && iCount % 2 != 0) iCount--;
            for (int i = 1; i <= iCount; i++)
            {
                var row = entities[i - 1];
                if (String.IsNullOrEmpty(row.NEWS_TITLE)) temp += "<div></div>";
                else
                {
                    var pd = row.NEWS_PUBLISHDATE;
                    row.Imgage.Width = ImgWidth;
                    temp += String.Format(DST, row.NEWS_TITLE, row.URL_IMG, row.URL, pd.ToString("dd/MM/yyyy"), Utils.CatSapo(row.NEWS_INITCONTENT, 30), (String.Format("/d{0}-{1}-{2}/p{3}c{4}/{5}.html", pd.Day, pd.Month, pd.Year, Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, Lib.QueryString.CategoryName)));
                }
                if (i > 0 && i % 2 == 0)
                {
                    html += String.Format("<div>{0}</div>", temp);
                    temp = String.Empty;
                }
            }

            return html;
        }
        /// <summary>
        /// 
        /// </summary>
        const string DST_NK = @"<div class=""{5}"" title=""{0}"">{1}<h2 title=""{0}""><a title=""{0}"" href=""{2}"">{0}</a></h2><h4 title=""{0}"">{3}</h4><p title=""{0}"">{4}</p></div>";
        public static string String_Danh_Sach_Tin_NhatKy(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("String_Danh_Sach_Tin_NhatKy-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            string keyNewwId = String.Format("keyNewwId-{0}--", cat_id);
            string html = String.Empty;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin(cat_id, pageSize, pageIndex, news_ids);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                if (iCount > 0 && iCount % 2 != 0) iCount--;
                DataRow row;
                string temp = String.Empty;
                string title = String.Empty;
                string url = String.Empty;
                string img = String.Empty;
                for (int i = 1; i <= iCount; i++)
                {
                    row = tbl.Rows[i - 1];
                    title = row["NEWS_TITLE"] != null ? HttpUtility.HtmlEncode(row["NEWS_TITLE"].ToString()) : String.Empty;
                    url = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), title);
                    img = Utils.GetThumbNail(title, url, row["NEWS_Image"] != null ? row["NEWS_Image"].ToString() : String.Empty, ImgWidth);
                    html += String.Format(DST_NK, title, img, url, Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]).ToString("dd/MM/yyyy"), Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30), i % 2 == 0 ? "alter" : "");

                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return obj.ToString();
        }

        const string DS_MEOVAT = @"<div class=""lsmeo"">{1}<h2><a title=""{0}"" ohref=""{2}"">{0}</a></h2><p class='mv'>{4}</p></div>";

        public static string String_Danh_Sach_Meo_Vat(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("String_Danh_Sach_Meo_Vat-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            string keyNewwId = String.Format("keyNewwId-{0}--", cat_id);
            string html = String.Empty;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin_Hidden(cat_id, pageSize, pageIndex, news_ids);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                if (iCount > 0 && iCount % 2 != 0) iCount--;
                DataRow row;
                string temp = String.Empty;
                string title = String.Empty;
                string url = String.Empty;
                string img = String.Empty;
                for (int i = 1; i <= iCount; i++)
                {
                    row = tbl.Rows[i - 1];
                    title = row["NEWS_TITLE"] != null ? HttpUtility.HtmlEncode(row["NEWS_TITLE"].ToString()) : String.Empty;
                    url = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), title);
                    img = Utils.GetThumbNail(title, url, row["NEWS_Image"] != null ? row["NEWS_Image"].ToString() : String.Empty, ImgWidth).Replace("href", "oldhref");
                    html += String.Format(DS_MEOVAT, title, img, url, Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]).ToString("dd/MM/yyyy"), Utils.GetObj<string>(row["NEWS_INITCONTENT"]).Replace("\n", "<br/>"));



                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return obj.ToString();
        }


        const string DST_QNA = @"<div title=""{0}""><h3>{5}</h3>{1}<h2 title=""{0}""><a title=""{0}"" href=""{2}"">{0}</a></h2><h4 title=""{0}"">{3}</h4><p title=""{0}"">{4}</p></div>";
        public static string String_Danh_Sach_Tin_QNA(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth)
        {
            string key = String.Format("String_Danh_Sach_Tin_QNA-{0}-{1}-{2}-{3}-{4}", cat_id, cat_parentid, pageSize, pageIndex, ImgWidth);
            string keyNewwId = String.Format("keyNewwId-{0}--", cat_id);
            string html = String.Empty;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin(cat_id, pageSize, pageIndex, news_ids);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                if (iCount > 0 && iCount % 2 != 0) iCount--;
                DataRow row;
                string temp = String.Empty;
                string title = String.Empty;
                string url = String.Empty;
                string img = String.Empty;
                CategoryEntity parentCE = BOCategory.GetCategory(1005);
                CategoryEntity ce;
                for (int i = 1; i <= iCount; i++)
                {
                    row = tbl.Rows[i - 1];
                    ce = BOCategory.GetCategory(Convert.ToInt32(row["Cat_ID"]));
                    title = row["NEWS_TITLE"] != null ? HttpUtility.HtmlEncode(row["NEWS_TITLE"].ToString()) : String.Empty;
                    url = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), title);
                    img = Utils.GetThumbNail(title, url, row["NEWS_Image"] != null ? row["NEWS_Image"].ToString() : String.Empty, ImgWidth);
                    temp += String.Format(DST_QNA, title, img, url, Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]).ToString("dd/MM/yyyy"), Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30), String.Format("<a href=\"{0}\">{1}</a>", parentCE.HREF, ce.Cat_Name));
                    if (i > 0 && i % 2 == 0)
                    {
                        html += String.Format("<div>{0}</div>", temp);
                        temp = String.Empty;
                    }
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return obj.ToString();
        }

        public static string String_Danh_Sach_Tin_Ngay_Mobile(int cat_parentid, int cat_id, DateTime date, int ImgWidth)
        {
            string key = String.Format("String_Danh_Sach_Tin_Ngay_Mobile-{0}-{1}-{2}-{3}", cat_id, cat_parentid, date, ImgWidth);
            string html = String.Empty;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_SearchByDate(cat_id, date);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                if (iCount > 0 && iCount % 2 != 0) { tbl.Rows.Add(tbl.NewRow()); iCount++; }
                DataRow row;
                string temp = String.Empty;
                string title = String.Empty;
                string url = String.Empty;
                string img = String.Empty;
                for (int i = 1; i <= iCount; i++)
                {
                    row = tbl.Rows[i - 1];
                    title = row["NEWS_TITLE"] != null ? HttpUtility.HtmlEncode(row["NEWS_TITLE"].ToString()) : String.Empty;
                    if (!string.IsNullOrEmpty(title))
                    {
                        url = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), title);
                        img = Utils.GetThumbNail(title, url, row["NEWS_Image"] != null ? row["NEWS_Image"].ToString() : String.Empty, ImgWidth);
                        var pd = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                        temp += String.Format(DST, title, img, url, Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]).ToString("dd/MM/yyyy"), Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30), (String.Format("/d{0}-{1}-{2}/p{3}c{4}/{5}.html", pd.Day, pd.Month, pd.Year, Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, Lib.QueryString.CategoryName)));
                    }
                    else
                        temp += "<div></div>";

                    if (i > 0 && i % 2 == 0)
                    {
                        html += String.Format("<div class=\"divsub\">{0}</div>", temp);
                        temp = String.Empty;
                    }
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return obj.ToString();
        }

        public static string String_Danh_Sach_Tin_Ngay(int cat_parentid, int cat_id, DateTime date, int ImgWidth)
        {
            string key = String.Format("String_Danh_Sach_Tin_Ngay-{0}-{1}-{2}-{3}", cat_id, cat_parentid, date, ImgWidth);
            string html = String.Empty;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_SearchByDate(cat_id, date);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                if (iCount > 0 && iCount % 2 != 0) { tbl.Rows.Add(tbl.NewRow()); iCount++; }
                DataRow row;
                string temp = String.Empty;
                string title = String.Empty;
                string url = String.Empty;
                string img = String.Empty;
                for (int i = 1; i <= iCount; i++)
                {
                    row = tbl.Rows[i - 1];
                    title = row["NEWS_TITLE"] != null ? HttpUtility.HtmlEncode(row["NEWS_TITLE"].ToString()) : String.Empty;
                    if (!string.IsNullOrEmpty(title))
                    {
                        url = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), title);
                        img = Utils.GetThumbNail(title, url, row["NEWS_Image"] != null ? row["NEWS_Image"].ToString() : String.Empty, ImgWidth);
                        var pd = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                        temp += String.Format(DST, title, img, url, Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]).ToString("dd/MM/yyyy"), Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30), (String.Format("/d{0}-{1}-{2}/p{3}c{4}/{5}.html", pd.Day, pd.Month, pd.Year, Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, Lib.QueryString.CategoryName)));
                    }
                    else
                        temp += "<div></div>";

                    if (i > 0 && i % 2 == 0)
                    {
                        html += String.Format("<div class=\"divsub\">{0}</div>", temp);
                        temp = String.Empty;
                    }
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return obj.ToString();
        }

        public static string String_Danh_Sach_Tin_Ngay_NhatKy(int cat_parentid, int cat_id, DateTime date, int ImgWidth)
        {
            string key = String.Format("String_Danh_Sach_Tin_Ngay-{0}-{1}-{2}-{3}", cat_id, cat_parentid, date, ImgWidth);
            string html = String.Empty;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl = null;
                string news_ids = NP_NewsID_In_TopHotByCat(cat_parentid, cat_id); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_SearchByDate(cat_id, date);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                if (iCount > 0 && iCount % 2 != 0) { tbl.Rows.Add(tbl.NewRow()); iCount++; }
                DataRow row;
                string temp = String.Empty;
                string title = String.Empty;
                string url = String.Empty;
                string img = String.Empty;
                for (int i = 1; i <= iCount; i++)
                {
                    row = tbl.Rows[i - 1];
                    title = row["NEWS_TITLE"] != null ? HttpUtility.HtmlEncode(row["NEWS_TITLE"].ToString()) : String.Empty;

                    url = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), title);
                    img = Utils.GetThumbNail(title, url, row["NEWS_Image"] != null ? row["NEWS_Image"].ToString() : String.Empty, ImgWidth);
                    html += String.Format(DST_NK, title, img, url, Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]).ToString("dd/MM/yyyy"), Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30), i % 2 == 0 ? "alter" : "");



                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return obj.ToString();
        }

        private static string ITEM = "<div class=\"w_260_f_l p_b_10\"><div class=\"tieudiem\">{0}<h1><a href=\"{1}\">{2}</a></h1></div></div>";
        /// <summary>
        /// Box nổi bật mục con ở danh sách cha
        /// </summary>
        /// <returns></returns>
        public static string BoxNBM_V2_String(int _parent_cat_id, int _cat_id, int top, int imgWidth, News_Mode mode)
        {
            string result = String.Empty;
            string key = String.Format("BoxNBM_V2_String-{0}_cat_id", _cat_id);
            string keyNewwId = String.Format(keyNewID, _parent_cat_id, _cat_id);
            result = Utils.Get_MemCache(key) ?? Utils.GetFromCache(key);
            if (result == null)
            {
                List<NewsPublishEntity> lst = BOATV.NewsPublished.NP_SelectListTopHotByCat(_parent_cat_id, _cat_id, top, imgWidth, mode);
                NewsPublishEntity nep;
                int iCount = lst != null ? lst.Count : 0;
                string temp = String.Empty;
                string[] newsId = new string[] { "0", "0", "0", "0" };
                for (int i = 0; i < iCount; i++)
                {
                    nep = lst[i];
                    newsId[i] = nep.NEWS_ID.ToString();
                    if (i == 0)
                        result += String.Format(ITEM, nep.URL_IMG, nep.URL, nep.NEWS_TITLE);
                    else
                        temp += String.Format("<li><a href=\"{0}\">{1}</a></li>", nep.URL, nep.NEWS_TITLE);
                }
                if (temp.Length > 0)
                {
                    temp = String.Format("<ul>{0}</ul>", temp);
                    result += temp;
                }

                if (!Utils.Add_MemCache(key, result))
                {
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, result);
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyNewwId, string.Join(",", newsId));
                }
            }
            return result;
        }

        public static DataTable Sao_DamLuanChild(long newsId)
        {
            DataTable tbl;
            string key = String.Format("Sao_DamLuanChild-{0}", newsId);
            tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Sao_DamLuanChild(newsId);
                }

                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                if (!tbl.Columns.Contains("News_Url")) tbl.Columns.Add("News_Url");
                if (!tbl.Columns.Contains("Child_News_Url")) tbl.Columns.Add("Child_News_Url");
                if (!tbl.Columns.Contains("VietNamDate")) tbl.Columns.Add("VietNamDate");
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    row["News_Url"] = BuildLink(Convert.ToInt32(row["cat_parentid"]), Convert.ToInt32(row["cat_Id"]), Convert.ToInt64(row["news_id"]), row["news_title"].ToString());
                    row["Child_News_Url"] = String.Format("/cn-{0}/{1}.html", Convert.ToInt64(row["OtherId"]), Utils.UnicodeToKoDauAndGach(row["news_title"].ToString()));
                    row["VietNamDate"] = Convert.ToDateTime(row["Child_PublishDate"]).ToString("dd/MM/yyyy");
                }
                tbl.AcceptChanges();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, tbl);
            }
            return tbl;
        }

        public static DataTable Sao_GetChannel()
        {
            string key = "Sao_GetChannel";
            DataTable tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Sao_GetChannel();
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.KENHTRUYENHINH, key, tbl);
            }
            return tbl;
        }

        public static DataTable Sao_GetLinkTheoNgay(int channelID, DateTime date)
        {
            string key = String.Format("Sao_GetLinkTheoNgay-{0}-{1}", channelID, date);
            DataTable tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Sao_GetLinkTheoNgay(channelID, date);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    row["ProgrameDetail"] = row["ProgrameDetail"] != null ? HttpUtility.HtmlEncode(row["ProgrameDetail"].ToString()).Replace("\n", "<br/>") : String.Empty;
                }
                tbl.AcceptChanges();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.LICHTRUYENHINH, key, tbl);
            }
            return tbl;
        }

        public static NewsPublishEntity NP_TinChiTietBaiCon(long News_ID)
        {
            string key = String.Format("NP_TinChiTietBaiCon-{0}", News_ID);
            NewsPublishEntity npe;
            object obj = HttpContext.Current.Cache[key];
            if (obj == null)
            {
                DataTable tbl;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_TinChiTietBaiCon(News_ID);
                }

                int iCount = tbl != null ? tbl.Rows.Count : 0;
                npe = new NewsPublishEntity();
                if (iCount > 0)
                {
                    DataRow row;
                    row = tbl.Rows[0];
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["Child_Sapo"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["Child_Title"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["Child_PublishDate"]);
                    npe.Imgage = new ImageEntity(100, Utils.GetObj<string>(row["Avatar"]));
                    npe.NEWS_CONTENT = Utils.GetObj<string>(row["Child_Content"]);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, npe);
                return npe;
            }
            return (NewsPublishEntity)obj;
        }

        public static string NP_GetMeidaObject_ByNews(long News_Id)
        {
            string key = String.Format("NP_GetMeidaObject_ByNews-{0}", News_Id);
            string html = Utils.GetFromCache(key);
            if (html == null)
            {
                DataTable tbl = new DataTable();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Sao_GetMeidaObject_ByNews(News_Id);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    html += row["Object_Url"] != null ? String.Format("<img src=\"{0}\"/>", Utils.GetObj<string>(row["Object_Url"])) : String.Empty;
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
            }
            return html;

        }

        const string HH_TOP = "{0}<h3>{1}</h3>";
        const string HH_ITEM = "<li>{0}</li>";
        public static List<String> NP_HoaHauVote(int voteId, int top)
        {
            string key = String.Format("NP_HoaHauVote-{0}", voteId);
            List<string> html = Utils.GetFromCache<List<string>>(key);
            if (html == null)
            {
                DataTable tbl = new DataTable();
                html = new List<string>();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.NP_HoaHauVote(voteId, top);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                string temp = String.Empty;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    if (i == 0)
                    {
                        html.Add(string.Format(HH_TOP, String.Format("<img title=\"{0}\" src=\"{1}\"/>", Utils.GetObj<string>(row["VoteItem_Content"]), Utils.GetObj<string>(row["Avatar"])), Utils.GetObj<String>(row["VoteItem_Content"])));
                    }
                    else
                    {
                        temp += string.Format(HH_ITEM, Utils.GetThumbNail(Utils.GetObj<string>(row["VoteItem_Content"]), "javascript:void(0);", row["Avatar"].ToString(), 80));
                    }

                }
                html.Add(temp);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.VoteItem, key, html);
            }
            return html;
        }
        /// <summary>
        /// Danh sach hoa hau
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public static String NP_HoaHauVoteList(int voteId)
        {
            string key = String.Format("NP_HoaHauVoteList-{0}", voteId);
            string html = Utils.GetFromCache(key);
            if (html == null)
            {
                DataTable tbl = new DataTable();
                using (MainDB db = new MainDB())
                {
                    //0 : lay tat ca vote item
                    tbl = db.StoredProcedures.NP_HoaHauVote(voteId, 0);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                string temp = String.Empty;
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    html += string.Format("<li class='hhitem'>{0}<h3><input type=\"radio\" value=\"{1}\" id=\"answer_{1}\" name=\"answer_{2}\"> {3}</h3></li>", "<img src=\"" + row["Avatar"].ToString() + "\"/>", Utils.GetObj<int>(row["VoteItem_ID"]), Utils.GetObj<int>(row["Vote_ID"]), Utils.GetObj<string>(row["VoteItem_Content"]));


                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.VoteItem, key, html);
            }
            return html;
        }

        public static DataTable VoteWithAvatar(int voteId)
        {
            string key = String.Format("VoteWithAvatar-{0}", voteId);
            DataTable tbl = Utils.GetFromCache<DataTable>(key);
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    //0 : lay tat ca vote item
                    tbl = db.StoredProcedures.NP_HoaHauVote(voteId, 0);
                }
                if (tbl == null)
                    tbl = new DataTable();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.VoteItem, key, tbl);
            }
            return tbl;
        }

        public static List<NewsPublishEntity> NP_Tin_Moi_Trong_Ngay(int top, int ImgWidth)
        {
            string key = String.Format("NP_Tin_Moi_Trong_Ngay-{0}-{1}", top, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = Utils.Get_MemCache<object>(key) ?? Utils.GetFromCache<object>(key);
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Tin_Moi_Trong_Ngay(top);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }

                Utils.Add_MemCache(key, lst);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        public static List<NewsPublishEntity> NP_Tin_HOT(int top, int cat, int ImgWidth)
        {
            string key = String.Format("NP_Tin_HOT-{0}-{1}-{2}", top, cat, ImgWidth);
            List<NewsPublishEntity> lst;
            object obj = Utils.GetFromCache<object>(key);
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Tin_Hot(top, cat);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }

                // Utils.Add_MemCache(key, lst);
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return (List<NewsPublishEntity>)obj;
        }

        public static List<NewsPublishEntity> NP_Xem_Nhieu_Nhat(int top, int ImgWidth, int catId)
        {
            string key = String.Format("NP_Xem_Nhieu_Nhat-{0}-{1}-{2}", top, ImgWidth, catId);
            var lst = Utils.GetFromCache<List<NewsPublishEntity>>(key);
            if (lst == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB db = new MainDB(BOAdv.MasterConnectionString))
                {
                    da = db.StoredProcedures.NP_Xem_Nhieu_Nhat(top, catId);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0)
                        npe.Imgage = new ImageEntity(ImgWidth, img);
                    lst.Add(npe);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            return lst;
        }



        //        public static string NP_Xem_Nhieu_Nhat(int top, int ImgWidth)
        //        {
        //            string key = String.Format("NP_Xem_Nhieu_Nhat-{0}-{1}", top, ImgWidth);
        //            string html = Utils.GetFromCache<string>(key);
        //            if (html == null)
        //            {
        //                DataTable da;
        //                using (MainDB db = new MainDB(BOAdv.MasterConnectionString))
        //                {
        //                    da = db.StoredProcedures.NP_Xem_Nhieu_Nhat(top);
        //                }
        //                int iCount = da != null ? da.Rows.Count : 0;
        //                DataRow row;
        //                NewsPublishEntity npe;
        //                for (int i = 0; i < iCount; i++)
        //                {
        //                    row = da.Rows[i];
        //                    npe = new NewsPublishEntity();
        //                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
        //                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
        //                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
        //                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
        //                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
        //                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
        //                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
        //                    html += String.Format(@"<div class=""tit-docnhieu"">
        //                                            <div class=""so"">
        //                                                {0}</div>
        //                                            <div class=""tin1"">
        //                                                <a href=""{1}"">
        //                                                    {2}</a>
        //                                                <p>
        //                                                    {3}</p>
        //                                            </div>
        //                                        </div>", (i + 1), npe.URL, npe.NEWS_TITLE, npe.NEWS_PUBLISHDATE.ToString("dd-MM-yyyy HH:mm"));
        //                }
        //                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
        //                return html;
        //            }
        //            return html;
        //        }

        public static string NP_Xem_Nhieu_Nhat170(int top, int ImgWidth)
        {
            string key = String.Format("NP_Xem_Nhieu_Nhat1701-{0}-{1}", top, ImgWidth);
            string html = Utils.GetFromCache<string>(key);
            string DONG_SU_KIEN_ITEM = "<li style=\"display:inline\" >{0}<h3><a style=\"color:#000\" href=\"{1}\">{2}</a></h3></li>";
            if (html == null)
            {
                html = string.Empty;
                DataTable da;
                using (MainDB db = new MainDB(BOAdv.MasterConnectionString))
                {
                    da = db.StoredProcedures.NP_Xem_Nhieu_Nhat(top, 0);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                    npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    html += String.Format(DONG_SU_KIEN_ITEM, npe.URL_IMG, npe.URL, npe.NEWS_TITLE);
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return html;
        }

        public static string NP_Nhat_Ky_Nhieu_Comment(int catParentId, int catId, int top, int ImgWidth)
        {
            string key = String.Format("NP_Nhat_Ky_Nhieu_Comment-{0}-{1}", top, ImgWidth);
            string html = HttpContext.Current.Cache[key] != null ? HttpContext.Current.Cache[key].ToString() : string.Empty;
            if (html.Length == 0)
            {
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Nhat_Ky_Nhieu_Comment(top, catId);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    npe = new NewsPublishEntity();
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.URL = BuildLink(catParentId, Convert.ToInt32(row["Cat_ID"]), npe); ;
                    string img = Utils.GetObj<string>(row["NEWS_Image"]);
                    if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                    html += String.Format("<li class='{4}'>{0}<h3><a title=\"{3}\" class=\"title_home\" href=\"{1}\">{2}</a></h3></li>", npe.URL_IMG, npe.URL, npe.NEWS_TITLE, HttpUtility.HtmlEncode(npe.NEWS_TITLE), i % 2 == 0 ? "tieu_diem_alter" : "");
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, html);
                return html;
            }
            return html;
        }


        public static string UnicodeToKoDauAndGach(string s)
        {
            if (s == null)
            {
                return string.Empty;
            }
            s = HttpUtility.HtmlDecode(s);
            string str = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                int index = "\x00e0\x00e1ả\x00e3ạ\x00e2ầấẩẫậăằắẳẵặ\x00e8\x00e9ẻẽẹ\x00eaềếểễệđ\x00ec\x00edỉĩị\x00f2\x00f3ỏ\x00f5ọ\x00f4ồốổỗộơờớởỡợ\x00f9\x00faủũụưừứửữựỳ\x00fdỷỹỵ\x00c0\x00c1Ả\x00c3Ạ\x00c2ẦẤẨẪẬĂẰẮẲẴẶ\x00c8\x00c9ẺẼẸ\x00caỀẾỂỄỆĐ\x00cc\x00cdỈĨỊ\x00d2\x00d3Ỏ\x00d5Ọ\x00d4ỒỐỔỖỘƠỜỚỞỠỢ\x00d9\x00daỦŨỤƯỪỨỬỮỰỲ\x00ddỶỸỴ\x00c2ĂĐ\x00d4ƠƯ".IndexOf(ch.ToString());
                if (index >= 0)
                {
                    str = str + "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU"[index];
                }
                else
                {
                    str = str + s[i];
                }
            }
            return str.Replace("–", "").Replace("-", "").Replace("  ", "").Replace(" ", "-").Replace("--", "-").Replace(":", "").Replace(";", "").Replace("+", "").Replace("@", "").Replace(">", "").Replace("<", "").Replace("*", "").Replace("{", "").Replace("}", "").Replace("|", "").Replace("^", "").Replace("~", "").Replace("]", "").Replace("[", "").Replace("`", "").Replace(".", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("”", "").Replace("“", "").Replace("?", "").Replace("\"", "").Replace("&", "").Replace("$", "").Replace("#", "").Replace("_", "").Replace("=", "").Replace("%", "").Replace("…", "").Replace("/", "").Replace(@"\", "").Replace(" ", "-").Replace("--", "-").Replace("---", "-").Replace("----", "-").Replace("-----", "-").ToLower().TrimEnd(new char[] { '-' }).TrimStart(new char[] { '-' });
        }







        public static DataTable displayGetTinMoi(long News_ID)
        {
            string key = String.Format("displayGetTinMoi-{0}-{1}-{2}", Lib.QueryString.CategoryID, Lib.QueryString.ParentCategoryID, News_ID);

            DataTable obj = (DataTable)HttpContext.Current.Cache[key];
            if (obj == null)
            {
                //obj = new DataTable();
                //DataTable da = NP_Select_Tin_Moi(Lib.QueryString.CategoryID, News_ID, 10);
                //int iCount = da != null ? da.Rows.Count : 0;
                //DataRow row;
                //NewsPublishEntity npe;
                //for (int i = 0; i < iCount; i++)
                //{
                //    row = da.Rows[i];
                //    row["NEWS_PUBLISHDATE"] = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                //    row["NEWS_Url"] = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt32(row["News_ID"]), row["News_Title"].ToString());
                //}

                //Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, da);
                //return da;
            }
            return (DataTable)obj;
        }

        public static DataTable displayGetDanhSachTin(int pageIndex, int pageSize, int ImgWidth)
        {
            string key = String.Format("displayGetDanhSachTin-{0}-{1}-{2}-{3}-{4}", Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, pageSize, pageIndex, ImgWidth);

            DataTable tbl = (DataTable)HttpContext.Current.Cache[key];
            if (tbl == null)
            {

                tbl = new DataTable();
                string news_ids = NP_NewsID_In_TopHotByCat(Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID); ;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Get_Danh_Sach_Tin(Lib.QueryString.CategoryID, pageSize, pageIndex, news_ids);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                if (!tbl.Columns.Contains("News_URL")) tbl.Columns.Add("News_URL");
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    row["NEWS_INITCONTENT"] = Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 30);
                    row["NEWS_PUBLISHDATE"] = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    row["NEWS_SUBTITLE"] = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                    row["NEWS_TITLE"] = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    row["News_URL"] = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), row["NEWS_TITLE"].ToString());
                    row["News_Image"] = Utils.GetThumbNail(Utils.GetObj<string>(row["NEWS_TITLE"]), Utils.GetObj<string>(row["News_URL"]), Utils.GetObj<string>(row["NEWS_Image"]), ImgWidth);
                }
                tbl.AcceptChanges();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, tbl);

            }
            return tbl;
        }

        public static DataTable displayGetTinKhac(long News_ID)
        {
            string key = String.Format("displayGetTinKhac-{0}", News_ID);

            DataTable da = Utils.Get_MemCache<DataTable>(key) ?? Utils.GetFromCache<DataTable>(key);
            if (da == null)
            {
                da = NP_Select_Tin_Khac(Lib.QueryString.CategoryID, News_ID, 10);
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                if (!da.Columns.Contains("NEWS_Url")) da.Columns.Add("NEWS_Url");
                for (int i = 0; i < iCount; i++)
                {
                    row = da.Rows[i];
                    row["NEWS_PUBLISHDATE"] = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                    row["NEWS_Url"] = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["News_ID"]), row["News_Title"].ToString());
                }
                da.AcceptChanges();
                if (!Utils.Add_MemCache(key, da))
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, da);

            }
            return da;
        }

        public static DataTable SearchFulltext(string key, int pageSize, int pageIndex, int imgWidth, int catId)
        {
            string keyCache = "SearchFulltext" + key;

            DataTable tbl = Utils.GetFromCache<DataTable>(keyCache);
            //if (tbl != null) return tbl;

            using (var db = new MainDB())
            {
                tbl = db.StoredProcedures.SearchFulltext(key, pageIndex, pageSize, catId);
            }
            if (!tbl.Columns.Contains("NEWS_Url")) tbl.Columns.Add("NEWS_Url");
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DataRow row = tbl.Rows[i];
                row["NEWS_PUBLISHDATE"] = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                row["NEWS_Url"] = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["News_ID"]), row["News_Title"].ToString());
                row["News_Image"] = Utils.GetThumbNail(Utils.GetObj<string>(row["NEWS_TITLE"]), row["NEWS_Url"].ToString(), Utils.GetObj<string>(row["NEWS_Image"]), imgWidth);
                row["NEWS_INITCONTENT"] = Utils.GetObj<string>(row["News_InitContent"]);
                row["NEWS_TITLE"] = Utils.GetObj<string>(row["NEWS_TITLE"]);
            }
            tbl.AcceptChanges();
            Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, keyCache, tbl);
            return tbl;
        }

        public static int SearchResulCount(string key, int pageSize, int catId)
        {
            DataTable tbl;
            using (MainDB db = new MainDB())
            {
                tbl = db.StoredProcedures.SearchFulltextCount(key, pageSize, catId);
            }
            return (tbl != null && tbl.Rows.Count > 0) ? Convert.ToInt32(tbl.Rows[0][0]) : 0;
        }



        public static DataTable GetTinForGoogleNews(int top)
        {
            string key = String.Format("GetTinForGoogleNews-{0}", top);

            DataTable tbl = (DataTable)HttpContext.Current.Cache[key];
            if (tbl == null)
            {

                tbl = new DataTable();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.GetTinForGoogleNews(top);
                }
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                DataRow row;
                if (!tbl.Columns.Contains("News_URL")) tbl.Columns.Add("News_URL");
                for (int i = 0; i < iCount; i++)
                {
                    row = tbl.Rows[i];
                    row["NEWS_TITLE"] = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    row["News_URL"] = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), row["NEWS_TITLE"].ToString());
                    row["News_Image"] = Utils.GetThumbNail(Utils.GetObj<string>(row["NEWS_TITLE"]), Utils.GetObj<string>(row["News_URL"]), Utils.GetObj<string>(row["NEWS_Image"]), 80);
                }
                tbl.AcceptChanges();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, tbl);

            }
            return tbl;
        }

        public static DataTable displayMediaLienquan(long newsid)
        {
            string key = String.Format("displayMediaLienquan-{0}", newsid);

            var tbl = Utils.Get_MemCache<DataTable>(key);

            if (tbl == null)
            {
                tbl = (DataTable)HttpContext.Current.Cache[key];
                if (tbl == null)
                {
                    tbl = new DataTable();
                    using (MainDB db = new MainDB())
                    {
                        tbl = db.StoredProcedures.GetMediaLienquan(newsid);
                    }

                    if (tbl == null) tbl = new DataTable();
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.VIDEO_INFO, key, tbl);
                }

                if (tbl != null && tbl.Rows.Count > 0)
                    Utils.Add_MemCache(key, tbl);

            }
            return tbl;
        }

        public static DataTable VideoInfoCategory(int CategoryId)
        {
            string key = String.Format("VideoInfoCategory-{0}", CategoryId);

            DataTable tbl = Utils.Get_MemCache<DataTable>(key) ?? (DataTable)HttpContext.Current.Cache[key];

            if (tbl == null)
            {
                tbl = new DataTable();
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.VideoInfoCategory(CategoryId);
                }

                if (tbl == null) tbl = new DataTable();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.VIDEO_INFO, key, tbl);
                Utils.Add_MemCache(key, tbl);
            }

            return tbl;
        }



        /// <summary>
        /// Danh sach tin moi nhat theo cateogry
        /// </summary>
        /// <param name="cat_id"></param>
        /// <param name="newsId"></param>
        /// <param name="ImgWidth"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static List<NewsPublishEntity> GetListNewsByCatAndDate(int cat_id, long newsId, int PageIndex, int PageSize, int ImgWidth)
        {
            string key = String.Format("GetListNewsByCatAndDate-{0}-{1}-{2}-{3}", cat_id, PageIndex, PageSize, ImgWidth);
            // string keyNews_ID = String.Format(keyNewIDHome, cat_id, newsId);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB db = new MainDB())
                    {
                        da = db.StoredProcedures.GetListNewsByCatAndDate(cat_id, newsId, PageIndex, PageSize);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    for (int i = 0; i < iCount; i++)
                    {
                        row = da.Rows[i];
                        npe = new NewsPublishEntity();
                        npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                        npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                        npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                        npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                        npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                        npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                        //news_id += npe.NEWS_ID + ",";
                        string img = Utils.GetObj<string>(row["NEWS_Image"]);
                        if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);
                        obj.Add(npe);
                    }

                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }

            return obj;
        }

        public static List<NewsPublishEntity> Danh_Sach_Tin_Theo_Cat(int cat_parentid, int cat_id, int pageSize, int pageIndex, int ImgWidth, string news_id)
        {
            string key = String.Format("Danh_Sach_Tin_Theo_Cat-{0}-{1}-{2}-{3}", cat_id, pageSize, pageIndex, ImgWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];

                if (obj == null)
                {
                    NewsPublishEntity npe;
                    obj = new List<NewsPublishEntity>();
                    DataTable tbl = null;
                    using (MainDB db = new MainDB())
                    {
                        tbl = db.StoredProcedures.Get_Danh_Sach_Tin(cat_id, pageSize, pageIndex, news_id);
                    }
                    int iCount = tbl != null ? tbl.Rows.Count : 0;
                    DataRow row;
                    for (int i = 0; i < iCount; i++)
                    {
                        row = tbl.Rows[i];
                        npe = new NewsPublishEntity();
                        npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["NEWS_INITCONTENT"]);
                        npe.NEWS_PUBLISHDATE = Utils.GetObj<DateTime>(row["NEWS_PUBLISHDATE"]);
                        npe.NEWS_SUBTITLE = Utils.GetObj<string>(row["NEWS_SUBTITLE"]);
                        npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                        npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                        npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                        string img = Utils.GetObj<string>(row["NEWS_Image"]);
                        if (img != null) { if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img); }
                        obj.Add(npe);
                    }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }
            return obj;
        }


        public static List<NewsPublishEntity> GetVideoByCatAndTop(int top, int catId, int imageWidth)
        {
            string key = String.Format("GetVideoByCatAndTop-{0}-{1}-{2}", top, catId, imageWidth);
            List<NewsPublishEntity> lst;
            object obj = Utils.GetFromCache<object>(key);
            if (obj == null)
            {
                lst = new List<NewsPublishEntity>();
                DataTable da;
                using (MainDB __db = new MainDB())
                {
                    da = __db.StoredProcedures.GetVideoByCatAndTop(top, catId);
                }
                int iCount = da != null ? da.Rows.Count : 0;
                DataRow row;
                NewsPublishEntity npe;
                if (iCount > 0)

                    for (int i = 0; i < iCount; i++)
                    {
                        row = da.Rows[i];
                        npe = new NewsPublishEntity();
                        npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                        npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                        npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                        npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                        npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                        npe.ICON = Utils.GetObj<string>(row["ICON"]);
                        npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["NEWS_Image"]);
                        string img = Utils.GetObj<string>(row["NEWS_Image"]);
                        if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(imageWidth, img);
                        lst.Add(npe);
                    }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, lst);
                return lst;
            }
            //DataTable __dt;

            return (List<NewsPublishEntity>)obj;
        }

        public static List<NewsPublishEntity> GetNewsVideoByCatAndTop(int top, int catId, int imageWidth)
        {
            string key = String.Format("GetNewsVideoByCatAndTop-{0}-{1}-{2}", top, catId, imageWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB __db = new MainDB())
                    {
                        da = __db.StoredProcedures.GetNewsVideoByCatAndTop(top, catId);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    if (iCount > 0)

                        for (int i = 0; i < iCount; i++)
                        {
                            row = da.Rows[i];
                            npe = new NewsPublishEntity();
                            npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                            npe.ICON = Utils.GetObj<string>(row["ICON"]);
                            npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["NEWS_Image"]);
                            string img = Utils.GetObj<string>(row["NEWS_Image"]);
                            if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(imageWidth, img);
                            obj.Add(npe);
                        }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }
            return obj;
        }

        public static List<NewsPublishEntity> GetVideoAllAndTop(int pageIndex, int pageSize, int catId, long newsId, int imageWidth)
        {
            string key = String.Format("GetVideoAllAndTop-{0}-{1}-{2}-{3}", pageIndex, pageSize, catId, imageWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB __db = new MainDB())
                    {
                        da = __db.StoredProcedures.GetListVideoAndTop(pageIndex, pageSize, catId, newsId);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    if (iCount > 0)

                        for (int i = 0; i < iCount; i++)
                        {
                            row = da.Rows[i];
                            npe = new NewsPublishEntity();
                            npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                            npe.ICON = Utils.GetObj<string>(row["ICON"]);
                            string img = Utils.GetObj<string>(row["NEWS_Image"]);
                            if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(imageWidth, img);
                            obj.Add(npe);
                        }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }
            return obj;
        }

        public static List<NewsPublishEntity> GetTinXuatBanSau(long News_ID, int CatID, int CatParentID, int Top, int imageWidth)
        {
            string key = String.Format("TTOL-GetTinXuatBanSau-{0}-{1}-{2}-{3}", CatID, News_ID, Top, imageWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB __db = new MainDB())
                    {
                        da = __db.StoredProcedures.TinXuatBanSau(News_ID, CatID, Top);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    if (iCount > 0)

                        for (int i = 0; i < iCount; i++)
                        {
                            row = da.Rows[i];
                            npe = new NewsPublishEntity();
                            npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                            string img = Utils.GetObj<string>(row["NEWS_Image"]);
                            if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(imageWidth, img);
                            obj.Add(npe);
                        }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);

                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }
            return obj;
        }

        public static List<NewsPublishEntity> GetTinXuatBanTruoc(long News_ID, int CatID, int CatParentID, int Top, int imageWidth)
        {
            string key = String.Format("TTOL-GetTinXuatBanTruoc-{0}-{1}-{2}-{3}", CatID, News_ID, Top, imageWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB __db = new MainDB())
                    {
                        da = __db.StoredProcedures.TinXuatBanTruoc(News_ID, CatID, Top);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    if (iCount > 0)

                        for (int i = 0; i < iCount; i++)
                        {
                            row = da.Rows[i];
                            npe = new NewsPublishEntity();
                            npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                            string img = Utils.GetObj<string>(row["NEWS_Image"]);
                            if (img != null)
                            { if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(imageWidth, img); }
                            obj.Add(npe);
                        }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }
            return obj;
        }

        public static List<NewsPublishEntity> GetListTinDocNhieuNhat(int Top, int CatID, int imageWidth)
        {
            string key = String.Format("GetListTinDocNhieuNhat-{0}-{1}-{2}", Top, CatID, imageWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB __db = new MainDB())
                    {
                        da = __db.StoredProcedures.GetListTinDocNhieuNhat(Top, CatID);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    if (iCount > 0)

                        for (int i = 0; i < iCount; i++)
                        {
                            row = da.Rows[i];
                            npe = new NewsPublishEntity();
                            npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                            string img = Utils.GetObj<string>(row["NEWS_Image"]);
                            if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(imageWidth, img);
                            obj.Add(npe);
                        }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }
            return obj;
        }

        public static List<NewsPublishEntity> GetListTinPhanHoiNhieu(int Top, int CatID, int imageWidth)
        {
            string key = String.Format("GetListTinPhanHoiNhieu-{0}-{1}-{2}", Top, CatID, imageWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB __db = new MainDB())
                    {
                        da = __db.StoredProcedures.GetListTinPhanHoiNhieu(Top, CatID);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    if (iCount > 0)

                        for (int i = 0; i < iCount; i++)
                        {
                            row = da.Rows[i];
                            npe = new NewsPublishEntity();
                            npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe);
                            npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                            npe.URL = BuildLink(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe); ;
                            string img = Utils.GetObj<string>(row["NEWS_Image"]);
                            if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(imageWidth, img);
                            obj.Add(npe);
                        }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }

                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }


            return obj;
        }

        /// <summary>
        /// lấy thông tin Ảnh theo newsID
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="imageWidth"></param>
        /// <returns></returns>
        public static List<NewsPublishEntity> GetPhotobyNewsID(string newsId, int imageWidth)
        {
            string key = String.Format("GetPhotobyNewsID-{0}-{1}", newsId, imageWidth);
            var obj = Utils.Get_MemCache<List<NewsPublishEntity>>(key);
            if (obj == null)
            {
                obj = (List<NewsPublishEntity>)HttpContext.Current.Cache[key];
                if (obj == null)
                {
                    obj = new List<NewsPublishEntity>();
                    DataTable da;
                    using (MainDB __db = new MainDB())
                    {
                        da = __db.StoredProcedures.GetPhotobyNewsID(newsId);
                    }
                    int iCount = da != null ? da.Rows.Count : 0;
                    DataRow row;
                    NewsPublishEntity npe;
                    if (iCount > 0)

                        for (int i = 0; i < iCount; i++)
                        {
                            row = da.Rows[i];
                            npe = new NewsPublishEntity();
                            npe.NEWS_INITCONTENT = Utils.GetObj<string>(row["PhotoDes"]);
                            npe.NEWS_ID = Convert.ToInt64(row["PhotoID"]);
                            npe.ICON = Utils.Original_Link(Utils.GetObj<string>(row["Photo"]));
                            //if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(imageWidth, img);
                            obj.Add(npe);
                        }
                    Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, obj);
                }
                if (obj.Count > 0)
                {
                    Utils.Add_MemCache(key, obj);
                }
            }
            return obj;
        }


        /// <summary>
        /// Get vote theo catid
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static DataTable SelectVoteByCatId(int catId)
        {
            string key = String.Format("SelectVoteByCatId-{0}", catId);
            DataTable tbl = (DataTable)HttpContext.Current.Cache[key];
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.SelectVoteByCatId(catId);
                }
                if (tbl == null) tbl = new DataTable();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.Vote, key, tbl);
            }
            return tbl;
        }

        /// <summary>
        /// SelectVoteByVoteId
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public static DataTable SelectVoteByVoteId(int voteId)
        {
            string key = String.Format("SelectVoteByVoteId-{0}", voteId);
            var tbl = (DataTable)HttpContext.Current.Cache[key];
            if (tbl == null)
            {
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Sao_SelectVoteById(voteId);
                }
                if (tbl == null) tbl = new DataTable();
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.VoteItem, key, tbl);
                return tbl;
            }
            return tbl;
        }

        /// <summary>
        /// Update Vote
        /// </summary>
        /// <param name="voteItemId"></param>
        public static void cms_Vote_Vote(int voteItemId)
        {
            using (var __db = new MainDB())
            {
                __db.StoredProcedures.cms_Vote_Vote(voteItemId);
            }
        }
        #region Quang Cao
        public static int GetQuangCaoZone()
        {
            if (Lib.QueryString.NewsID > 0)
            {
                string key = "Category-News-" + Lib.QueryString.NewsID;
                int catId = Utils.GetFromCache<Int32>(key);
                if (catId > 0) return catId;
                var news = NP_TinChiTiet(Lib.QueryString.NewsID, false);
                if (news != null)
                {
                    catId = news.Cat_Id;
                    if (catId > 0)
                    {
                        var cat = BOCategory.GetCategory(catId);
                        if (cat != null)
                        {
                            var p = cat.Cat_ParentID;
                            if (p > 0) catId = p;
                        }
                    }

                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, catId);
                return catId;
            }
            return (Lib.QueryString.ParentCategoryID > 0 ? Lib.QueryString.ParentCategoryID : Lib.QueryString.CategoryID);
        }

        #endregion

    }
}
