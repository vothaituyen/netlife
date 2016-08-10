using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using ATVCommon;
using ATVEntity;
using BOATV;

namespace NetLife.web.Controls.Details
{
    public partial class Detail : System.Web.UI.UserControl
    {
        private string txtCat = "<a href=\"/{0}.html\">{1}</a>";

        private string txtCatPr = "<span><img src=\"/Images/next.jpg\" /></span><a href=\"{0}\">{1}</a>";

        private string itemRelate = "<li>{2}<a title=\"{1}\" href=\"{0}\">{1}</a></h4></li>";
        private string itemRelateNews = "<li class=\"news\">{2}<h4><a title=\"{1}\" href=\"{0}\">{1}</a></h4></li>"; // HTTHAO ADD 20160509
        //private string itemOther = "<li><a title=\"{1}\" href=\"{0}\">{1}</a></li>";
        private string itemOtherNews = "<li class=\"news\"><a title=\"{1}\" href=\"{0}\">{1}</a></li>";
        private string itemOther = "<li class=\"news\"><div class=\"row\">{2}<a title=\"{1}\" href=\"{0}\">{1}</a></div></li>";
        //private string itemOtherNews = "<li class=\"news\">{2}<a title=\"{1}\" href=\"{0}\">{1}</a></li>";
        private string relatedNewsId = "";
        private List<NewsPublishEntity> dataEntity;
        public List<NewsPublishEntity> DataEntity { get { return dataEntity; } set { dataEntity = value; } }

        //private string strListOne = "<div class=\"row img-nb\">{0}</div><div class=\"row title-nb\"> <a href=\"{1}\">{2}</a></div>";

        protected void Page_Load(object sender, EventArgs e)
        {

            NewsPublishEntity ne = NewsPublished.NP_TinChiTiet(Lib.QueryString.NewsID, false);
            if (ne != null)
            {
                if (Request.QueryString["move301"] != null)
                {
                    Utils.Move301(ne.URL);
                    return;
                }

                ltrTitle.Text = ne.NEWS_TITLE;

                var content = ne.NEWS_CONTENT;
                string pattern = @"(?<start><a[^>]*)(?<end>>)";
                string repl = @"${start} target=""_blank"" ${end}";
                string newString = Regex.Replace(content, pattern, repl);
                ltrContent.Text = newString.Replace("src=\"/Uploaded/", "src=\"http://static.netlife.vn/Uploaded/");
                //neu clip thi add script quang cao
                if (
                    Lib.QueryString.CategoryID == 78 || 
                    Lib.QueryString.CategoryID == 90 ||
                    Lib.QueryString.CategoryID == 91 ||
                    Lib.QueryString.CategoryID == 92 ||
                    Lib.QueryString.CategoryID == 93 ||
                    Lib.QueryString.CategoryID == 94 ||
                    Lib.QueryString.CategoryID == 95 
                    )
                {
                    ltrContent.Text = ltrContent.Text + "<script type=\"text/javascript\" src=\"http://media.adnetwork.vn/js/jwplayer.js\"></script> <script type=\"text/javascript\"> var _abd = _abd || []; _abd.push([\"1427708093\",\"Video\",\"1427711687\",\"abdplayer\",\"500\",\"282\"]); </script> <script src=\"http://media.adnetwork.vn/js/adnetwork.js\" type=\"text/javascript\"></script> <noscript><a href=\"http://track.adnetwork.vn/247/adServerNs/zid_1427711687/wid_1427708093/\" target=\"_blank\"><img src=\"http://delivery.adnetwork.vn/247/noscript/zid_1427711687/wid_1427708093/\" /></a></noscript>";
                }
                ltrDate.Text = ne.NEWS_PUBLISHDATE.ToString("dd/MM/yyyy");
                ltrTime.Text = ne.NEWS_PUBLISHDATE.ToString("hh:mm");
                ltrDes.Text = ne.NEWS_INITCONTENT;
                ltrAthor.Text = ne.NEWS_ATHOR;

                if (!String.IsNullOrEmpty(ne.Keywrods))
                    ltrKeyword.Text = ne.Keywrods;
                else
                {
                    tags.Visible = false;
                }
                var catId = 0;

                if (Lib.QueryString.ParentCategoryID == 0)
                {
                    catId = Lib.QueryString.CategoryID;
                }
                else
                {
                    catId = Lib.QueryString.ParentCategoryID;
                }
                CategoryEntity cat = BOCategory.GetCategory(catId);
                if (cat != null)
                {
                    ltrCatName.Text = cat.Cat_Name;

                    ltrCattxt.Text = string.Format(txtCat, cat.Cat_DisplayURL.ToLower(), cat.Cat_Name);

                }
                if (Lib.QueryString.ParentCategoryID > 0)
                {
                    catId = Lib.QueryString.CategoryID;
                    CategoryEntity cat1 = BOCategory.GetCategory(catId);
                    ltrCatParent.Text = string.Format(txtCatPr, cat1.Cat_Description, cat1.Cat_Name);
                }




                Utils.SetPageHeader(this.Page, ne.NEWS_TITLE, ne.NEWS_INITCONTENT, Utils.RemoveHTMLTag(ne.Keywrods));
                Utils.SetFaceBookSEO(this.Page, ne.NEWS_TITLE, ne.NEWS_INITCONTENT, ne.Imgage.StorageUrl, Request.RawUrl);

                string GOOGLE =
                                    @"  <meta itemprop=""datePublished"" content=""{4}"" /> 
                                        <meta itemprop=""sourceOrganization"" content=""NetLife"" />
                                        <meta itemprop=""url"" property=""og:url"" content=""{0}"" />
                                        <meta itemprop=""articleSection"" content=""{1}"" />
                                        <meta itemprop=""image"" content=""{3}"" />
                                        <div style=""display: none !important"" itemscope itemtype=""http://schema.org/Recipe"">
                                            <span itemprop=""name"">{2}</span>
                                            <img itemprop=""image"" src=""{3}"" />                                             
                                        </div>";

                ltrTitle.Text += string.Format(GOOGLE, ne.URL.StartsWith("http") ? ne.URL : "http://netlife.vn" + ne.URL, cat.Cat_Name, ne.NEWS_TITLE, !String.IsNullOrEmpty(ne.Imgage.StorageUrl) && ne.Imgage.StorageUrl.StartsWith("http") ? ne.Imgage.StorageUrl : Utils.ImagesThumbUrl + "/" + ne.Imgage.StorageUrl, ne.NEWS_PUBLISHDATE);

            }
            if (ne != null && ne.NEWS_RELATION.Count > 0)
            {

                var abc = ne.NEWS_RELATION.Count > 2 ? ne.NEWS_RELATION.Skip(2).Take(6).ToList() : ne.NEWS_RELATION; // Edit Take(8) -> Take(6) HTTHAO edit 20160525
                dataEntity = abc;
                NewsPublishEntity nep;
                int iCount = dataEntity != null ? dataEntity.Count : 0;
                for (int i = 0; i < (iCount > 6 ? 6 : iCount); i++) //(int i = 0; i < iCount; i++)
                {
                    nep = dataEntity[i];
                    nep.Imgage = new ImageEntity(150, nep.Imgage.ImageUrl);
                    /* HTTHAO EDIT 20160508 Giam muc tin lien quan con 6 bai
                    if (i < 4)
                        ltrListRelate.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                    else
                    {
                        ltrListRelate2.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                        relatenews.Visible = true;
                    }
                    */
                    if (i < 3) //Gia tri cu la 4, thay doi vi ly do giam muc xuong con 6 bai, thay vi 8 bai nhu cu
                        ltrListRelate.Text += String.Format(itemRelateNews, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                    else
                    {
                        ltrListRelate2.Text += String.Format(itemRelateNews, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                        relatenews.Visible = true;
                    }
                    // HTTHAO add 20160525: to get Related News Id for filter "Tin Cung Chuyen Muc"
                    if(relatedNewsId==null || relatedNewsId == "")
                    {
                        relatedNewsId = nep.NEWS_ID.ToString();
                    }
                    else
                    {
                        relatedNewsId = relatedNewsId + "," + nep.NEWS_ID.ToString();
                    }                    
                }
               
                for (int i = 0; i < (iCount > 3 ? 3 : iCount); i++) //Gia tri cu la 4, thay doi vi ly do giam muc xuong con 6 bai, thay vi 8 bai nhu cu
                {
                    nep = dataEntity[i];
                    ltrRelate.Text += String.Format(itemOtherNews, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                }
            }
            else
            {
                lienquan.Visible = false;

            }

            //ltrVideo

            var video = NewsPublished.NP_Danh_Sach_Tin(0, 78, 4, 1, 150);
            if (video != null)
            {
                NewsPublishEntity nep;
                int iCount = video != null ? video.Count : 0;
                for (int i = 0; i < iCount; i++)
                {
                    nep = video[i];

                    ltrVideo.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                }
            }

            {
                var otherNews = NewsPublished.NP_Select_Tin_Cung_Chuyen_Muc(Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, Lib.QueryString.NewsID, 6, relatedNewsId);
                if (otherNews != null)
                {
                    NewsPublishEntity nep;
                    int iCount = otherNews != null ? otherNews.Count : 0;
                    //for (int i = 0; i < iCount; i++)
                    //{
                    //    nep = otherNews[i];
                    //    nep.Imgage = new ImageEntity(40, nep.Imgage.ImageUrl);
                    //    ltrOther.Text += String.Format(itemOther, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                    //}
                    for (int i = 0; i < (iCount > 6 ? 6 : iCount); i++) //(int i = 0; i < iCount; i++)
                    {
                        nep = otherNews[i];
                        nep.Imgage = new ImageEntity(150, nep.Imgage.ImageUrl);
                        /*HTTHAO EDIT: Chinh muc tin cung chuyen muc hien thi tuong tu Muc tin lien quan*/
                        if (i < 3) //Gia tri cu la 4, thay doi vi ly do giam muc xuong con 6 bai, thay vi 8 bai nhu cu
                            LiteralOther1.Text += String.Format(itemOther, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                        else
                        {
                            LiteralOther2.Text += String.Format(itemOther, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                            othernews.Visible = true;
                        }

                    }
                }
                else
                {
                    cungchuyenmuc.Visible = false;

                }

            }
        }
    }
}