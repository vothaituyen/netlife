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
        private string txtCat = "<a href=\"{0}\" title=\"{1}\">{1}</a>";
        private string txtCatPr = "<span><img src=\"/Images/next.jpg\" /></span><a href=\"{0}\" title=\"{1}\">{1}</a>";
        private string itemRelate = "<div class=\"row item-list\"> 	<div class=\"col-xs-12 pd\">       <div class=\"col-xs-5 img-list-item\">          <a href=\"{0}\" title=\"{1}\">             <img src=\"{2}?width=213&crop=auto&scale=both\" title=\"{1}\" alt=\"{1}\">          </a>       </div>       <div class=\"col-xs-7 info-list-item\"><a href=\"{0}\">{1} </a><br/><span>{3}</span></div> 	</div> </li>";
        private string itemHot = "<li class=\"news_home\"><a href=\"{0}\" title=\"{1}\"><span>{1}</span></a></li>";
        private List<NewsPublishEntity> dataEntity;
        public List<NewsPublishEntity> DataEntity { get { return dataEntity; } set { dataEntity = value; } }

        private string countTime(DateTime dateTime)
        {
            if ((DateTime.Now - dateTime).TotalMinutes < 60)
            {
                return (DateTime.Now - dateTime).Minutes.ToString() + " phút trước";
            }
            else if ((DateTime.Now - dateTime).TotalHours < 24)
            {
                return (DateTime.Now - dateTime).Hours.ToString() + " tiếng trước";
            }
            return dateTime.ToString("dd-MM-yyyy HH:mm");
        }

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
                string htmlMobile2Extend = BOAdv.GetAdvItemById(Lib.Object2Integer(47), Lib.QueryString.CategoryID);
                string htmlMobile2Perfect = BOAdv.GetAdvItemById(Lib.Object2Integer(55), Lib.QueryString.CategoryID);
                string htmlMobile6Adpruce = BOAdv.GetAdvEmbedScriptItemById(Lib.Object2Integer(38), Lib.QueryString.CategoryID);//live id = 38, sua dong duoi nua
                string htmlVideoMobile = BOAdv.GetAdvEmbedScriptItemById(Lib.Object2Integer(39), Lib.QueryString.CategoryID);//live id = 39
                string mobile8 = BOAdv.GetAdvEmbedScriptItemById(Lib.Object2Integer(40), Lib.QueryString.CategoryID);//live id = 39

                string adsContent = "";
                string adsContentExtend = "";
                string adsContentPerfect = "";

                if (!String.IsNullOrWhiteSpace(htmlMobile2))
                {
                    adsContent = Environment.NewLine + "<script>" + String.Format("var zone{0}_{2} = new RunBanner({1}, \"zone{0}_{2}_Adv\"); zone{0}_{2}.Show();", Lib.QueryString.CategoryID, htmlMobile2.Replace("\\n", " ").Replace("\\t", " "), 31).Replace("INSERT_RANDOM_NUMBER_HERE", DateTime.Now.ToFileTime().ToString()) + "</script>" + Environment.NewLine;
                    //adsContent = Environment.NewLine + "<div class=\"stickyads\"><a  href=\"https://www.toshiba.com.vn/san-pham/tivi/pro-theatre-series/pro-theatre-l36-series\" target=\"_blank\"><img alt=\"\" src=\"http://static.netlife.vn/2016/06/04/07/41/Left-x2300x600.png\" style=\"width: 300px; height: 600px;\"></a></div>" + Environment.NewLine;
                }
                if (!String.IsNullOrWhiteSpace(htmlMobile2Extend))
                {
                    adsContentExtend = Environment.NewLine + "<script>" + String.Format("var zone{0}_{2} = new RunBanner({1}, \"zone{0}_{2}_Adv\"); zone{0}_{2}.Show();", Lib.QueryString.CategoryID, htmlMobile2Extend.Replace("\\n", " ").Replace("\\t", " "), 47).Replace("INSERT_RANDOM_NUMBER_HERE", DateTime.Now.ToFileTime().ToString()) + "</script>" + Environment.NewLine;
                }
                if (!String.IsNullOrWhiteSpace(htmlMobile2Perfect))
                {
                    adsContentPerfect = Environment.NewLine + "<script>" + String.Format("var zone{0}_{2} = new RunBanner({1}, \"zone{0}_{2}_Adv\"); zone{0}_{2}.Show();", Lib.QueryString.CategoryID, htmlMobile2Perfect.Replace("\\n", " ").Replace("\\t", " "), 47).Replace("INSERT_RANDOM_NUMBER_HERE", DateTime.Now.ToFileTime().ToString()) + "</script>" + Environment.NewLine;
                }



                ltrTitle.Text = ne.NEWS_TITLE;

                var content = ne.NEWS_CONTENT;
                string pattern = @"(?<start><a[^>]*)(?<end>>)";
                string repl = @"${start} target=""_blank"" ${end}";
                string newString = Regex.Replace(content, pattern, repl);
                //ltrContent.Text = newString.Replace("src=\"/Uploaded/", "src=\"http://static.netlife.vn/Uploaded/").Replace("<div id=\"vmcbackground\"></div>", string.Format("<div id=\"vmcbackground\"><center>{0}</center></div>", adsContent)).Replace("<div id=\"vmcbackgroundExtend\"></div>", string.Format("<div id=\"vmcbackground\"><center>{0}</center></div>", adsContentExtend));
                ltrContent.Text = newString.Replace("src=\"/Uploaded/", "src=\"http://static.netlife.vn/Uploaded/").Replace("<div id=\"vmcbackground\"></div>", string.Format("<div id=\"vmcbackground\"><center>{0}</center></div>", adsContent)).Replace("<div id=\"vmcbackgroundExtend\"></div>", string.Format("<div id=\"vmcbackground\"><center>{0}</center></div>", adsContentExtend)).Replace("<div id=\"vmcbackgroundPerfect\"></div>", string.Format("<div id=\"vmcbackground\"><center>{0}</center></div>", adsContentPerfect));
                //ltrContent.Text = newString.Replace("src=\"/Uploaded/", "src=\"http://static.netlife.vn/Uploaded/").Replace("<div id=\"vmcbackground\"></div>", string.Format("{0}", adsContent)).Replace("<div id=\"vmcbackgroundExtend\"></div>", string.Format("<div id=\"vmcbackground\"><center>{0}</center></div>", adsContentExtend));

                //ltrContent.Text = newString.Replace("<div id=\"vmcbackgroundExtend\"></div>", string.Format("<div id=\"vmcbackground1\"><center>{0}</center></div>", adsContentExtend));
                //ltrContent.Text = ltrContent.Text.Replace("jpg\"", "jpg?maxwidth=480\"");
                ltrContent.Text = ltrContent.Text.Replace("jpg\"", "jpg?maxwidth=480\"" + " alt=\"" + ne.NEWS_TITLE + "\"");
                //ltrContent.Text = ltrContent.Text.Replace("<div id=\"abde\">", "<div id=\"abdad\"> <script type=\"text/javascript\"> var _ase  = _ase || []; /* load placement for account: Netlife, site: http://m.netlife.vn, zone size : 640x1280 */ _ase.push(['1464661938','1491064202']); </script> <script src=\"http://static.gammaplatform.com/js/ad-exchange.js\" type=\"text/javascript\"></script> </div><div id=\"abde\">");
                if (!String.IsNullOrWhiteSpace(htmlMobile6Adpruce) && htmlMobile6Adpruce.Length > 2)
                {
                    ltrContent.Text += Environment.NewLine;
                    ltrContent.Text += htmlMobile6Adpruce;
                }
                //if (htmlVideoMobile.Length > 2) //add ad video 7
                //{
                //    if (htmlVideoMobile.Contains("ambient")) //ambient
                //    {
                //        ltrContent.Text += Environment.NewLine;
                //        ltrContent.Text += htmlVideoMobile;
                //    }
                //    else if (htmlVideoMobile.Contains("Ebound")) //ebound
                //    {
                //        String[] part = htmlVideoMobile.Split('@');
                //        ltrContent.Text += Environment.NewLine;
                //        ltrContent.Text = ltrContent.Text.Replace("<video", part[0] + "<video").Replace("video>", "video>" + part[1]);
                //    }
                if (!String.IsNullOrWhiteSpace(htmlVideoMobile))
                {
                    ltrContent.Text += Environment.NewLine;
                    ltrContent.Text += htmlVideoMobile;
                }


                //}
                // mobile 8
                //if (!String.IsNullOrWhiteSpace(mobile8))
                //{
                //    ltrContent.Text += "<a href=\"http://idp.vn/lif/vi\" target=\"_blank\"><div class=\"video_Sticky\"><video id=\"myVideo\"   autoplay height=\"80\" ><source src=\"" + mobile8 + "\"></source></video></div></a>";
                //}

                ltrContent.Text = ltrContent.Text.Replace("595px", "device-width");// chinh lai do rong cua the de fit voi content
                if (!ltrContent.Text.Contains("people_write"))
                {
                    ltrContent.Text += "<div class=\"people_write hide\" itemprop=\"author\" itemscope=\"\" itemtype=\"https://schema.org/Person\"> <b><span itemprop=\"name\" class=\"icon_author\">Tổng hợp</span>/ Theo google</b><br></div> </div> </div>";
                }
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
                    CategoryEntity catParent = BOCategory.GetCategory(cat.Cat_ParentID);

                    //ltrCatParentMenu.Text=
                    if (cat.Cat_ParentID != null && cat.Cat_ParentID != 0) {
                        ltrCatParentMenu.Text = string.Format(txtCat, (String.Format("/{0}.html", catParent.Cat_DisplayURL.ToLower().Trim())), catParent.Cat_Name);
                    }


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

                    ltrListRelate.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.Imgage.ImageUrl, countTime(nep.NEWS_PUBLISHDATE));
                }
                //ltrListRelate.Text = ltrListRelate.Text.Replace("img", "img width=\"60\" height=\"50\"");
            }
            else
            {
                relate.Visible = false;
            }

            //var otherNews = NewsPublished.NP_Select_Tin_Khac(Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, Lib.QueryString.NewsID, 10);
            var otherNews = NewsPublished.NP_Xem_Nhieu_Nhat(10, 213, Lib.QueryString.CategoryID);
            
            if (otherNews != null)
            {
                NewsPublishEntity nep;
                int iCount = otherNews != null ? otherNews.Count : 0;
                for (int i = 0; i < iCount; i++)
                {
                    nep = otherNews[i];
                    ltrOther.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.Imgage.ImageUrl, countTime(nep.NEWS_PUBLISHDATE));   
                }
            }
            var newsHot = NewsPublished.NoiBatTrangChu(5, 50);
            if (newsHot != null)
            {
                NewsPublishEntity nep;
                int iCount = newsHot != null ? newsHot.Count : 0;
                for (int i = 0; i < iCount; i++)
                {
                    nep = newsHot[i];
                    ltrHot.Text += String.Format(itemHot, nep.URL, nep.NEWS_TITLE);
                }

            }
            var listNew = NewsPublished.NP_Tin_Moi_Trong_Ngay(10, 60);
            if (listNew != null)
            {
                NewsPublishEntity nep;
                int iCount = listNew != null ? listNew.Count : 0;
                for (int i = 0; i < iCount; i++)
                {
                    nep = listNew[i];
                    ltrNew.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.Imgage.ImageUrl, countTime(nep.NEWS_PUBLISHDATE));
                }
            }


            var sameCategorys = NewsPublished.NP_Select_Tin_Khac(Lib.QueryString.ParentCategoryID, Lib.QueryString.CategoryID, Lib.QueryString.NewsID, 10);
            if (sameCategorys != null)
            {
                NewsPublishEntity nep;
                int iCount = sameCategorys != null ? sameCategorys.Count : 0;
                for (int i = 0; i < iCount; i++)
                {
                    nep = sameCategorys[i];
                    ltsameCategorys.Text += String.Format(itemRelate, nep.URL, nep.NEWS_TITLE, nep.Imgage.ImageUrl, countTime(nep.NEWS_PUBLISHDATE));
                }
            }


        }
    }
}