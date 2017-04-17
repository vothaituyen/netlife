using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATVCommon;
using BOATV;

namespace NetLife.web.Controls.Lists
{
    public partial class Paging : System.Web.UI.UserControl
    {
        #region ATTRIBUTES


        private string currentPage = "1";
        public int PageSize;
        public int Size;
        public int PageIndex = 1;
        string buildLink = "";
        bool isSearch;
        string normalcss = "";
        string activecss = "";
        int totalPage;
        public string NormalCss { set { normalcss = value; } }
        public string ActiveCss { set { activecss = value; } }
        public int TotalPage { set { totalPage = value; } }
        private bool isShowXem = false;
        public bool IsShowXemNgayThang { set { isShowXem = value; } }
        public bool IsSearch { set { isSearch = value; } }
        #endregion



        public void DoPagging(int pageIndex)
        {
            //divXem.Visible = isShowXem;

            string css = "";

            currentPage = pageIndex.ToString();
            string __link = "";
            int iCurrentPage = 0;

            if (Utils.IsNumber(currentPage))
                int.TryParse(currentPage, out iCurrentPage);
            else
                iCurrentPage = 1;

            if (iCurrentPage == 0) iCurrentPage = 1;

            ltrPagging.Text = "";

            if (totalPage <= 5)
            {
                if (totalPage != 1)
                {
                    for (int i = 1; i <= totalPage; i++)
                    {
                        css = currentPage == i.ToString() ? activecss : normalcss;
                        __link += BuildLink(i, i.ToString(), css);
                    }
                }
            }
            else
            {
                if (iCurrentPage > 1)
                {
                    __link += BuildLink(1, "&lt;&lt;", normalcss);
                }
                else
                {
                    for (int i = 1; i <= 5; i++)
                    {

                        css = currentPage == i.ToString() ? activecss : normalcss;
                        __link += BuildLink(i, i.ToString(), css);
                    }


                    __link += BuildLink(6, "...", normalcss);
                }
                if (iCurrentPage > 1 && iCurrentPage < totalPage)
                {
                    if (iCurrentPage > 2)
                    {

                        __link += BuildLink(iCurrentPage - 2, "...", normalcss);
                    }
                    for (int i = (iCurrentPage - 1); i <= (iCurrentPage + 1); i++)
                    {
                        css = currentPage == i.ToString() ? activecss : normalcss;

                        __link += BuildLink(i, i.ToString(), css);
                    }
                    if (iCurrentPage <= totalPage - 2)
                    {

                        __link += BuildLink(iCurrentPage + 2, "...", normalcss);
                    }
                }
                int intCurrentPage = 0;
                int.TryParse(currentPage, out intCurrentPage);
                if (intCurrentPage == 0) intCurrentPage = 1;
                if (intCurrentPage < totalPage)
                {

                    __link += BuildLink(totalPage, "&gt;&gt;", normalcss);
                }
                else
                {

                    __link += BuildLink(totalPage - 4, "...", normalcss);
                    int j = 5;
                    for (int i = totalPage; i >= totalPage - 5; i--)
                    {

                        css = currentPage == (totalPage - j).ToString() ? activecss : normalcss;
                        __link += BuildLink(totalPage - j, (totalPage - j).ToString(), css);
                        j--;
                    }
                }
            }
            ltrPagging.Text = __link;
        }

        public void HidePagging(bool ishidden)
        {
            ltrPagging.Visible = !ishidden;
        }
        /// <summary>
        /// Hàm thực hiện build link phân trang - Tuấn VA
        /// </summary>
        /// <param name="CurrentPageURL">link cho trang hiện tại - ex: trang-1, trang-2</param>
        /// <param name="BeginSymbol">Ký tự bắt đầu</param>
        /// <param name="CurrentPage">Text hiển thị cho trang hiện tại</param>
        /// <param name="EndSymbol">Ký tự kết thúc</param>
        /// <returns></returns>
        private string BuildLink(int page, string TextDisplay, string css)
        {
            //if (Request.RawUrl.ToLower().IndexOf("/video/") == 0)
            //{
            //    buildLink = "<a class=\"{0}\" href='/video/{0}-p0c1085n{1}/trang-{3}.html>{4}</a> &nbsp;";
            //    return String.Format(buildLink, css, Utils.UnicodeToKoDauAndGach(Lib.QueryString.Title), Lib.QueryString.NewsID, page, TextDisplay);
            //}
            if (isSearch)
            {
                buildLink = "<li><a class=\"{2}\" href='/tag/{0}/trang-{1}.html'><span>{3}</span></a></li>";
                return String.Format(buildLink, Request.QueryString["key"] != null ? HttpUtility.HtmlEncode(Request.QueryString["key"]).Replace(" ", "+").Replace("/", "_") : "netlife", page, css, TextDisplay);
            }
             
            buildLink = "<li><a class=\"{2}\" href='/{0}/trang-{1}.html'>{3}</a></li>";
            return String.Format(buildLink, Utils.UnicodeToKoDauAndGach(Lib.QueryString.CategoryName), page, css, TextDisplay);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pager.Visible = totalPage > 1;
            this.EnableViewState = true;
        }
    }
}