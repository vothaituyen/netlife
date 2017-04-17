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
        private string itemNews = "<li class=\"news\"><div class=\"row\">{2}<a title=\"{1}\" href=\"{0}\">{1}</a></div></li>";
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
                ltrContent.Text = ltrContent.Text.Replace("jpg\"", "jpg?maxwidth=480\"" + " alt=\"" + ne.NEWS_TITLE + "\"");



                string html = BOAdv.GetAdvEmbedScriptItemById(Lib.Object2Integer(37), Lib.Object2Integer(Lib.QueryString.CategoryID));
                //string adsContent = "";
                //if (!String.IsNullOrWhiteSpace(html) && html.Length > 2)
                //{
                //    if (html.Contains("235")) //pc video ebound
                //    {
                //        ltrContent.Text = newString.Replace("<video", "<div id=\"selectorElement\" style=\"width:500px;height:auto;\"><video").Replace("video>", "video><script data-cfasync=\"false\" id=\"EboundAd\" type=\"text/javascript\" src=\"//eboundservices.com/ads/ads.js\" width=\"500px\" height=\"auto\"></script></div>");
                //    }
                //    else if (html.Contains("233")) //pc video ambient
                //    {
                //        adsContent = "<script>" + String.Format("var zone{0}_{2} = new RunBanner({1}, \"zone{0}_{2}_Adv\"); zone{0}_{2}.Show();", Lib.QueryString.CategoryID, html.Replace("\\n", " ").Replace("\\t", " "), 37).Replace("INSERT_RANDOM_NUMBER_HERE", DateTime.Now.ToFileTime().ToString()) + "</script>";
                //        ltrContent.Text = ltrContent.Text + adsContent;
                //    }
                //}
                if (html.Length > 2) //add ad video 7
                {
                    if (html.Contains("ambient")) //ambient
                    {
                        ltrContent.Text += Environment.NewLine;
                        ltrContent.Text += html;
                    }
                    else if (html.Contains("ebound")) //ebound
                    {
                        String[] part = html.Split('@');
                        ltrContent.Text += Environment.NewLine;
                        ltrContent.Text = ltrContent.Text.Replace("<video", part[0] + "<video").Replace("video>", "video>" + part[1]);
                    }

                }
                if (!ltrContent.Text.Contains("people_write"))
                {
                    ltrContent.Text += "\n<table class=\"tplCaption\"><div class=\"people_write hide\" itemprop=\"author\" itemscope=\"\" itemtype=\"https://schema.org/Person\"> <b><span itemprop=\"name\" class=\"icon_author\">Tổng hợp</span>/ Theo google</b><br></div> </div> </div></table>";
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
                    //if (i < 3) //Gia tri cu la 4, thay doi vi ly do giam muc xuong con 6 bai, thay vi 8 bai nhu cu
                    //    ltrListRelate.Text += String.Format(itemRelateNews, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                    //else
                    //{
                    //    ltrListRelate2.Text += String.Format(itemRelateNews, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                    //    relatenews.Visible = true;
                    //}
                    // HTTHAO add 20160525: to get Related News Id for filter "Tin Cung Chuyen Muc"
                    if (relatedNewsId == null || relatedNewsId == "")
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

            var video = NewsPublished.NP_Danh_Sach_Tin(0, 134, 4, 1, 150);
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
            {
                var listNews = NewsPublished.NP_Tin_Moi_Trong_Ngay(10, 60);
                if (listNews != null)
                {
                    NewsPublishEntity nep;
                    int iCount = listNews != null ? listNews.Count : 0;
                    //for (int i = 0; i < iCount; i++)
                    //{
                    //    nep = otherNews[i];
                    //    nep.Imgage = new ImageEntity(40, nep.Imgage.ImageUrl);
                    //    ltrOther.Text += String.Format(itemOther, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                    //}
                    for (int i = 0; i < (iCount > 6 ? 6 : iCount); i++) //(int i = 0; i < iCount; i++)
                    {
                        nep = listNews[i];
                        nep.Imgage = new ImageEntity(150, nep.Imgage.ImageUrl);
                        /*HTTHAO EDIT: Chinh muc tin cung chuyen muc hien thi tuong tu Muc tin lien quan*/
                        if (i < 3) //Gia tri cu la 4, thay doi vi ly do giam muc xuong con 6 bai, thay vi 8 bai nhu cu
                            LiteralNews1.Text += String.Format(itemNews, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                        else
                        {
                            LiteralNews2.Text += String.Format(itemNews, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                            listnews.Visible = true;
                        }


                    }
                }
            }
        }
    }
}