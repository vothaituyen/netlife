using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using ATVEntity;
using BOATV;

namespace NetLifeMobile.Controls.Details
{
    public partial class Detail : System.Web.UI.UserControl
    {
        private string txtCat = "<a href=\"{0}\">{1}</a>";
        private string txtCatPr = "<span><img src=\"/Images/next.jpg\" /></span><a href=\"{0}\">{1}</a>";
        private string itemRelate = "<li class=\"news_home\"><a href=\"{0}\"><span class=\"relateNews\"><img src=\"{2}\" alt=\"\" height=\"50\" width=\"60\"></span><span>{1}</span></a></li>";
        private List<NewsPublishEntity> dataEntity;
        public List<NewsPublishEntity> DataEntity { get { return dataEntity; } set { dataEntity = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {

            NewsPublishEntity ne = NewsPublished.NP_TinChiTiet(Lib.QueryString.NewsID, true);
            if (ne != null)
            {
                if (Request.QueryString["move301"] != null)
                {
                    Utils.Move301(ne.URL);
                    return;
                }

                string htmlMobile2 = BOAdv.GetAdvItemById(Lib.Object2Integer(31), Lib.QueryString.CategoryID);
                string htmlMobile6Adpruce = BOAdv.GetAdvItemById(Lib.Object2Integer(38), Lib.QueryString.CategoryID);//live id = 38, sua dong duoi nua
                string htmlVideoMobile = BOAdv.GetAdvItemById(Lib.Object2Integer(39), Lib.QueryString.CategoryID);//live id = 39

                string adsContent = "";
                if (!String.IsNullOrWhiteSpace(htmlMobile2))
                {
                    adsContent = "<script>" + String.Format("var zone{0}_{2} = new RunBanner({1}, \"zone{0}_{2}_Adv\"); zone{0}_{2}.Show();", Lib.QueryString.CategoryID, htmlMobile2.Replace("\\n", " ").Replace("\\t", " "), 31).Replace("INSERT_RANDOM_NUMBER_HERE", DateTime.Now.ToFileTime().ToString()) + "</script>";

                    adsContent += "<script>" + String.Format("var zone{0}_{2} = new RunBanner({1}, \"zone{0}_{2}_Adv\"); zone{0}_{2}.Show();", Lib.QueryString.CategoryID, htmlMobile6Adpruce.Replace("\\n", " ").Replace("\\t", " "), 38).Replace("INSERT_RANDOM_NUMBER_HERE", DateTime.Now.ToFileTime().ToString()) + "</script>";

                    adsContent += "<script>" + String.Format("var zone{0}_{2} = new RunBanner({1}, \"zone{0}_{2}_Adv\"); zone{0}_{2}.Show();", Lib.QueryString.CategoryID, htmlVideoMobile.Replace("\\n", " ").Replace("\\t", " "), 39).Replace("INSERT_RANDOM_NUMBER_HERE", DateTime.Now.ToFileTime().ToString()) + "</script>";
                }

                ltrTitle.Text = ne.NEWS_TITLE;

                var content = ne.NEWS_CONTENT;
                string pattern = @"(?<start><a[^>]*)(?<end>>)";
                string repl = @"${start} target=""_blank"" ${end}";
                string newString = Regex.Replace(content, pattern, repl);
                ltrContent.Text = newString.Replace("src=\"/Uploaded/", "src=\"http://static.netlife.vn/Uploaded/").Replace("<div id=\"vmcbackground\"></div>", string.Format("<div id=\"vmcbackground\"><center>{0}</center></div>", adsContent));
                ltrContent.Text = ltrContent.Text.Replace("595px", "auto");// chinh lai do rong cua the de fit voi content
                ltrDate.Text = ne.NEWS_PUBLISHDATE.ToString("dd/MM/yyyy");
                ltrTime.Text = ne.NEWS_PUBLISHDATE.ToString("hh:mm");
                ltrDes.Text = ne.NEWS_INITCONTENT;
                ltrAthor.Text = ne.NEWS_ATHOR;

                if (!String.IsNullOrEmpty(ne.Keywrods))
                    ltrKeyword.Text = ne.Keywrods;
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
                    //ltrCatName.Text = cat.Cat_Name;

                    ltrCattxt.Text = string.Format(txtCat,(String.Format("/{0}.html", cat.Cat_DisplayURL.ToLower().Trim())), cat.Cat_Name);

                }
                if (Lib.QueryString.ParentCategoryID > 0)
                {
                    catId = Lib.QueryString.CategoryID;
                    CategoryEntity cat1 = BOCategory.GetCategory(catId);
                    ltrCatParent.Text = string.Format(txtCatPr,
                     (String.Format("/{0}.html", cat1.Cat_DisplayURL.ToLower().Trim())), cat1.Cat_Name);
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

                ltrTitle.Text += string.Format(GOOGLE, ne.URL.StartsWith("http") ? ne.URL : "http://netlife.com.vn" + ne.URL, cat.Cat_Name, ne.NEWS_TITLE, !String.IsNullOrEmpty(ne.Imgage.StorageUrl) && ne.Imgage.StorageUrl.StartsWith("http") ? ne.Imgage.StorageUrl : Utils.ImagesThumbUrl + "/" + ne.Imgage.StorageUrl, ne.NEWS_PUBLISHDATE);

            }
            if (ne.NEWS_RELATION.Count > 0)
            {
                //Tin_lien_quan1.DataEntity = nep.NEWS_RELATION;
                var abc = ne.NEWS_RELATION ;//.Take(ne.NEWS_RELATION.Count < 5 ? ne.NEWS_RELATION.Count : 5).ToList();
                dataEntity = abc;
                NewsPublishEntity nep;
                int iCount = dataEntity != null ? dataEntity.Count : 0;
                for (int i = 0; i < (iCount > 6 ? 6 : iCount); i++)
                {
                    nep = dataEntity[i];
                    //ltrListRelate.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                    ltrListRelate.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.Imgage.ImageUrl);
                }
                //ltrListRelate.Text = ltrListRelate.Text.Replace("img", "img width=\"60\" height=\"50\"");
            }
            else
            {
                relate.Visible = false;
            }

            var otherNews = NewsPublished.NP_Select_Tin_Khac(Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, Lib.QueryString.NewsID, 10);
            if (otherNews != null)
            {
                NewsPublishEntity nep;
                int iCount = otherNews != null ? otherNews.Count : 0;
                for (int i = 0; i < iCount; i++)
                {
                    nep = otherNews[i];
                    //ltrOther.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.URL_IMG);
                    ltrOther.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.Imgage.ImageUrl);
                }
                //ltrOther.Text = ltrOther.Text.Replace("img", "img width=\"60\" height=\"50\"");
            }

        }
    }
}