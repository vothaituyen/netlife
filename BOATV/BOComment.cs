using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DALATV;
using ATVEntity;
using System.Web;
using System.Collections;
namespace BOATV
{
    public class BOComment
    {
        /// <summary>
        /// 
        /// </summary>
        public static string KEY_COMMENT = "Comment_GetByNewsID_{0}";
        public static List<CommentEntity> Comment_GetByNewsID(long news_id)
        {
            string key = String.Format(KEY_COMMENT, news_id);
            var lst = Utils.Get_MemCache<List<CommentEntity>>(key) ?? Utils.GetFromCache<List<CommentEntity>>(key);
            if (lst == null || lst.Count == 0)
            {
                DataTable tbl;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.Comment_GetByNewsID(news_id);
                }
                lst = new List<CommentEntity>();
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                CommentEntity ce;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    ce = new CommentEntity();
                    row = tbl.Rows[i];
                    ce.Avatar = Utils.GetObj<string>(row["Avatar"]);
                    ce.Comment_Content = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["Comment_Content"])).Replace("\n", " <br />");
                    ce.Comment_Date = Utils.GetObj<DateTime>(row["Comment_Date"]);
                    ce.Comment_Email = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["Comment_Email"]));
                    ce.Comment_ID = Utils.GetObj<Int64>(row["Comment_ID"]);
                    ce.CommentLike = Utils.GetObj<int>(row["Film_ID"]);
                    ce.Rate = Utils.GetObj<Int64>(row["Comment_Rate"]);
                    ce.Comment_User = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["Comment_User"]));
                    //ce.CommentParent = Utils.GetObj<Int32>(row["SP_ID"]);
                    ce.CommentParent = Utils.GetObj<Int64>(row["ParentID"]);
                    lst.Add(ce);
                }
                
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.COMMENT, key, lst);
                Utils.Add_MemCache(key, lst);
            }
            return lst;
        }

        public static List<CommentEntity> GetCommentByNewsIdAndPage(long news_id, int pageIndex, int pageSize)
        {
            string key = String.Format(KEY_COMMENT, news_id);
            var lst = Utils.Get_MemCache<List<CommentEntity>>(key);
            if (lst == null)
            {
                DataTable tbl;
                using (MainDB db = new MainDB())
                {
                    tbl = db.StoredProcedures.GetCommentByNewsIdAndPage(news_id, pageIndex, pageSize);
                }
                lst = new List<CommentEntity>();
                int iCount = tbl != null ? tbl.Rows.Count : 0;
                CommentEntity ce;
                DataRow row;
                for (int i = 0; i < iCount; i++)
                {
                    ce = new CommentEntity();
                    row = tbl.Rows[i];
                    ce.Avatar = Utils.GetObj<string>(row["Avatar"]);
                    ce.Comment_Content = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["Comment_Content"])).Replace("\n", " <br />");
                    ce.Comment_Date = Utils.GetObj<DateTime>(row["Comment_Date"]);
                    ce.Comment_Email = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["Comment_Email"]));
                    ce.Comment_ID = Utils.GetObj<Int64>(row["Comment_ID"]);
                    ce.CommentLike = Utils.GetObj<int>(row["Film_ID"]);
                    ce.Rate = Utils.GetObj<Int64>(row["Comment_Rate"]);
                    ce.Comment_User = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["Comment_User"]));
                    ce.CommentParent = Utils.GetObj<Int32>(row["SP_ID"]);
                    lst.Add(ce);
                }
                //Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.COMMENT, key, lst);
                Utils.Add_MemCache(key, lst);
            }
            return lst;
        }


        public static void Comment_Insert(CommentEntity ce)
        {
            using (MainDB db = new MainDB(BOAdv.MasterConnectionString))
            {
                db.StoredProcedures.Comment_Insert(ce);
            }
        }

        public static string[] GetTopNewsestCommentByCatID(int Top, int CatID, int cat_parentid)
        {
            string key = String.Format("GetTopNewsestCommentByCatID_{0}_{1}", Top, CatID);
            string[] temp = Utils.GetFromCache<string[]>(key);
            if (temp != null) return temp;
            temp = new string[2] { "", "" };
            string Item1 = "<div class=\"gridItem\">{0}<p style='padding-top:0px;'><span><a>{1} : </a></span>{2}</p></div>";
            string Item = "<div class=\"gridItem\">{0}<h3><a href=\"{1}\">{4}</a></h3><h5>{5}</h5><p><span><a>{2} : </a></span>{3}</p></div>";
            string DIV = "<div class=\"chude_khac_col\">{0}</div>";
            string DIV_Khac = "<li><a href=\"{0}\">{1}</a></li>";
            NewsPublishEntity nep = new NewsPublishEntity();
            DataTable tbl;
            using (MainDB db = new MainDB())
            {
                tbl = db.StoredProcedures.GetTopNewestCommentByCatID(Top, CatID);
            }

            if (tbl != null && tbl.Rows.Count > 0)
            {
                if (!tbl.Columns.Contains("Image"))
                    tbl.Columns.Add("Image");
                if (!tbl.Columns.Contains("CommentAvatar"))
                    tbl.Columns.Add("CommentAvatar");
                if (!tbl.Columns.Contains("NewsIcon"))
                    tbl.Columns.Add("NewsIcon");

                ImageEntity _newsImage = null;
                ImageEntity _commentAvatar = null;
                ImageEntity _icon = null;
                Hashtable htb = new Hashtable();

                DataRow _row = null;

                string _newsUrl = "";
                string temp1 = "";
                int realCount = 0;
                for (int i = 0; i < tbl.Rows.Count; i++)
                {

                    temp1 = "";
                    _row = tbl.Rows[i];

                    if (htb.Contains(_row["News_ID"].ToString())) { continue; }
                    htb.Add(_row["News_ID"].ToString(), _row["News_ID"].ToString());
                    if (realCount <= 1)
                    {
                        if (_row["News_Image"] != null && _row["News_Image"].ToString() != "")
                        {
                            _newsImage = new ImageEntity(78, _row["News_Image"].ToString());
                            _row["Image"] = "<img src=\"" + _newsImage.ImageUrl + "\" width=\"78px\" onerror=\"LoadImage(this,'" + _newsImage.OnError + "');\">";
                        }
                        else
                            _row["Image"] = DBNull.Value;

                        if (_row["Avatar"] != null && _row["Avatar"].ToString() != "")
                        {
                            _commentAvatar = new ImageEntity(78, _row["Avatar"].ToString());
                            _row["CommentAvatar"] = "<img src=\"" + _commentAvatar.ImageUrl + "\" width=\"78px\" onerror=\"LoadImage(this,'" + _commentAvatar.OnError + "');\">";
                        }
                        else
                            _row["CommentAvatar"] = DBNull.Value;

                        if (_row["Icon"] != null && _row["Icon"].ToString() != "")
                        {
                            _icon = new ImageEntity(78, _row["Icon"].ToString());
                            _row["NewsIcon"] = "<img src=\"" + _icon.ImageUrl + "\" width=\"78px\" onerror=\"LoadImage(this,'" + _icon.OnError + "');\">";
                        }
                        else
                            _row["NewsIcon"] = DBNull.Value;


                        nep = new NewsPublishEntity();
                        nep.NEWS_ID = Convert.ToInt64(_row["News_ID"]);
                        nep.NEWS_TITLE = _row["News_Title"].ToString();

                        cat_parentid = Convert.ToInt32(_row["Cat_ParentID"]);
                        CatID = Convert.ToInt32(_row["Cat_ID"]);

                        _newsUrl = NewsPublished.BuildLink(cat_parentid, CatID, nep);
                        temp1 += String.Format(Item, "<a href=\"" + _newsUrl + "\">" + _row["Image"].ToString() + "</a>", _newsUrl, _row["News_SubTitle"].ToString(), Utils.CatSapo(_row["News_InitContent"].ToString(), 30), _row["News_Title"].ToString(), Convert.ToDateTime(_row["News_PublishDate"]).ToString("dd/MM/yyyy HH:mm"));
                        temp1 += String.Format(Item1, _row["CommentAvatar"] != DBNull.Value ? "<a href=\"" + _newsUrl + "\">" + _row["CommentAvatar"].ToString() + "</a>" : "", _row["Comment_User"].ToString(), Utils.CatSapo(_row["Comment_Content"].ToString(), 30));
                        temp[0] += String.Format(DIV, temp1);
                    }
                    else
                    {
                        nep = new NewsPublishEntity();
                        nep.NEWS_ID = Convert.ToInt64(_row["News_ID"]);
                        nep.NEWS_TITLE = _row["News_Title"].ToString();

                        cat_parentid = Convert.ToInt32(_row["Cat_ParentID"]);
                        CatID = Convert.ToInt32(_row["Cat_ID"]);

                        _newsUrl = NewsPublished.BuildLink(cat_parentid, CatID, nep);

                        temp[1] += String.Format(DIV_Khac, _newsUrl, _row["News_Title"].ToString());
                    }
                    realCount++;
                }
                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, new string[] { TableName.COMMENT, TableName.NEWSPUBLISHED }, key, temp);
            }


            return temp;
        }
        public static void CommentLike(Int64 commentId)
        {
            using (MainDB db = new MainDB(BOAdv.MasterConnectionString))
            {
                db.StoredProcedures.CommentLike(commentId);
            }
        }
    }
}
