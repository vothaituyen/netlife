using System;
using System.Collections.Generic;
using System.Text;

namespace ATVCommon
{
    public class Constants
    {
        /// <summary>
        /// PageCachedContent[Cat:{category}_News:{news}_Event:{event}_Page:{page}]
        /// </summary>
        public const string CACHE_NAME_FORMAT_HTML_PAGE_CONTENT = "PageCachedContent[Cat:{0}_News:{1}_Event:{2}_Page:{3}]";
        /// <summary>
        /// ControlCachedContent[{control_container_id}][Cat:{category}_News:{news}_Event:{event}_Page:{page}]
        /// </summary>
        public const string CACHE_NAME_FORMAT_HTML_CONTROL_CONTENT = "ControlCachedContent[{0}][Cat:{1}_News:{2}_Event:{3}_Page:{4}]";
        /// <summary>
        /// ControlCachedContent[Top10LastestNewsInCat:{category}]
        /// </summary>
        public const string CACHE_NAME_TOP_10_LASTEST_NEW_ID = "ControlCachedContent[Top10LastestNewsInCat:{0}]";
        /// <summary>
        /// ControlCachedContent[Top10LastestEventNewsInCat:{event}]
        /// </summary>
        public const string CACHE_NAME_TOP_10_LASTEST_EVENT_NEW_ID = "ControlCachedContent[Top10LastestEventNewsInCat:{0}]";
        /// <summary>
        /// {key}_LastUpdate
        /// </summary>
        public const string CACHE_NAME_LAST_UPDATE = "{0}_LastUpdate";
        /// <summary>
        /// {NewsID}_NewsDetail_Title
        /// </summary>
        public const string CACHE_NAME_NEWS_DETAIL_TITLE = "{0}_NewsDetail_Title";
        /// <summary>
        /// {NewsID}_NewsDetail_Sapo
        /// </summary>
        public const string CACHE_NAME_NEWS_DETAIL_SAPO = "{0}_NewsDetail_Sapo";
        /// <summary>
        /// Parent ZoneID sử dụng cho các control trong trang News Detail
        /// </summary>
        public const int PARENT_ZONE_ID_FOR_NEWS_DETAIL_CACHE = -1;
        /// <summary>
        /// Parent ZoneID sử dụng cho Data cache
        /// </summary>
        public const int PARENT_ZONE_ID_FOR_DATA_CACHE = -2;
        /// <summary>
        /// Parent ZoneID sử dụng cho dòng sự kiện
        /// </summary>
        public const int PARENT_ZONE_ID_FOR_EVENT_LIST_CACHE = -3;
        /// <summary>
        /// Parent ZoneID sử dụng cho Homepage
        /// </summary>
        public const int PARENT_ZONE_ID_FOR_HOME_PAGE_CACHE = 0;
        /// <summary>
        /// ZoneID của mục sự kiện trong ngày
        /// </summary>
        public const int ZONE_ID_SU_KIEN_TRONG_NGAY = 20;


        /// <summary>
        /// 
        /// </summary>
        public const string CACHE_NAME_NEWS_DETAIL = "{0}_NewsDetail";
        /// <summary>
        /// Parent ZoneID sử dụng cho các control trong trang News Detail
        /// </summary>
    }
}
