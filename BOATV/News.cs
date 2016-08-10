using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using ATVEntity;
using DALATV;

namespace BOATV
{
    public class News
    {
        public DataSet GetTopHomeByCategory(int cat_id, int top, int ImgWidthTop1, int ImgWidthTop2)
        {
            string key = String.Format("NP_Select_Top_Home-{0}-{1}-{2}-{3}", cat_id, top, ImgWidthTop1, ImgWidthTop2);
            var data = (DataSet) HttpContext.Current.Cache[key];
            if (data == null)
            {
                data = new DataSet();
                DataTable da;
                using (MainDB db = new MainDB())
                {
                    da = db.StoredProcedures.NP_Select_Top_Home(cat_id, top);
                }
               
                if (da != null)
                {
                    
                    int iCount = da.Rows.Count;
                    DataRow row;
                    string news_id = "";
                    if (!da.Columns.Contains("AnchorImage")) da.Columns.Add("AnchorImage");
                    if (!da.Columns.Contains("News_Url")) da.Columns.Add("News_Url");
                    DataTable tmpUp = da.Clone(), tmpDown = da.Clone();
                    tmpUp.Clear();
                    tmpDown.Clear();
                    for (int i = 0; i < iCount; i++)
                    {
                        row = da.Rows[i];
                        row["NEWS_INITCONTENT"] = Utils.CatSapo(Utils.GetObj<string>(row["NEWS_INITCONTENT"]), 20);
                        row["News_Url"] = Utils.BuildLinkDetail(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), Convert.ToInt64(row["NEWS_ID"]), row["NEWS_Title"].ToString()); ;
                        var img = Utils.GetObj<string>(row["NEWS_Image"]);
                        row["AnchorImage"] = Utils.GetThumbNail(Utils.GetObj<string>(row["NEWS_TITLE"]), row["News_Url"].ToString(), img, i == 0 ? ImgWidthTop1 : (i == 1 ? ImgWidthTop2 : 0));
                        row["NEWS_TITLE"] = HttpUtility.HtmlEncode(Utils.GetObj<string>(row["NEWS_TITLE"]));

                        if (i < top)
                        {
                            tmpUp.Rows.Add(row.ItemArray);
                        }
                        else
                        {
                            tmpDown.Rows.Add(row.ItemArray);
                        }
                    }
                    data.Tables.AddRange(new DataTable[] { tmpUp, tmpDown });
                }

                Utils.SaveToCacheDependency(TableName.DATABASE_NAME, TableName.NEWSPUBLISHED, key, data);
            }
            return  data;
        }

        public static string LBDomain = ConfigurationSettings.AppSettings["LBDomain"] ?? "img.2sao.vn|183.91.14.17";
        public NewsPublishEntity GetNewsById(long News_ID)
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
                    npe.NEWS_IMAGESNOTE = Utils.GetObj<string>(row["News_ImageNote"]);
                    npe.NEWS_TITLE = Utils.GetObj<string>(row["NEWS_TITLE"]);
                    npe.NEWS_ID = Convert.ToInt64(row["NEWS_ID"]);
                    npe.ICON = Utils.GetObj<string>(row["ICON"]);
                    npe.News_Rate = Utils.GetObj<Int32>(row["NEWS_Rate"]);
                    npe.Cat_Id = cat_id = Utils.GetObj<Int32>(row["Cat_Id"]);
                    npe.Cat_ParentId = cat_parentid = Utils.GetObj<Int32>(row["Cat_ParentId"]);
                    npe.URL = Utils.BuildLinkDetail(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe.NEWS_ID, npe.NEWS_TITLE);
                    npe.Imgage = new ImageEntity(100, Utils.GetObj<string>(row["NEWS_IMAGE"]));
                    string content = Utils.GetObj<string>(row["News_Content"]).Replace("//<![CDATA[", "").Replace("//]]>", "");

                    var lstDomain = LBDomain.Split('|');

                    if (npe.NEWS_PUBLISHDATE < new DateTime(2010, 7, 8, 23, 59, 00) && lstDomain.Length > 0)
                    {
                        content = content.Replace("img.2sao.vietnamnet.vn", lstDomain[0]);
                    }
                    else if (npe.NEWS_PUBLISHDATE < DateTime.Now.AddDays(-3) && lstDomain.Length > 1)
                    {
                        content = content.Replace("img.2sao.vietnamnet.vn", lstDomain[1]);
                    }

                    npe.NEWS_CONTENT = content;
                    npe.Keywrods = Utils.GetObj<string>(row["Extension3"]);
                    npe.NEWS_RELATION = GetRelation(Utils.GetObj<string>(row["NEWS_RELATION"]), 200);

                    var tmpKeyword = new List<string>();
                    try
                    {
                        tmpKeyword = NGramKeyword.GetNGramKeyword(npe.NEWS_TITLE, npe.NEWS_INITCONTENT, npe.NEWS_CONTENT, 10);
                    }
                    catch
                    {
                        tmpKeyword = new List<string>();
                    }


                    npe.Keywrods = string.Concat(npe.Keywrods, " ", String.Join(", ", tmpKeyword.ToArray()));


                    if (npe.Keywrods.Length > 0)
                    {
                        string[] arStrings = npe.Keywrods.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < arStrings.Length; i++)
                        {
                            arStrings[i] = String.Format("<a title=\"{2}\" href=\"http://sao.tintuconline.com.vn/{0}.search\">{1}</a>", arStrings[i].Trim().Replace(" ", "-"), HttpUtility.HtmlEncode(arStrings[i].Trim()), HttpUtility.HtmlEncode(arStrings[i].Trim()));
                        }

                        npe.Keywrods = String.Join(", ", arStrings);
                    }

                    string news_otherCat = Utils.GetObj<String>(row["News_OtherCat"]);
                    if (news_otherCat.Length > 0)
                    {
                        string[] otherCat = news_otherCat.Split(',');
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

        public List<NewsPublishEntity> GetRelation(string News_ID, int ImgWidth)
        {
            if (!string.IsNullOrEmpty(News_ID))
            {
                string key = String.Format("NP_Tin_Lien_Quan-{0}",  News_ID);
                var lst = Utils.Get_MemCache<List<NewsPublishEntity>>(key) ?? Utils.GetFromCache<List<NewsPublishEntity>>(key);
                if (lst == null || lst.Count == 0)
                {
                    lst = new List<NewsPublishEntity>();
                    DataTable da;
                    using(var db = new MainDB())
                    {
                        da = db.StoredProcedures.NP_Tin_Lien_Quan(News_ID);
                    }
                        
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
                            npe.URL = Utils.BuildLinkDetail(Convert.ToInt32(row["Cat_ParentID"]), Convert.ToInt32(row["Cat_ID"]), npe.NEWS_ID, npe.NEWS_TITLE);

                            string img = Utils.GetObj<string>(row["NEWS_Image"]);
                            if (img.Trim().Length > 0) npe.Imgage = new ImageEntity(ImgWidth, img);

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
    }
}
