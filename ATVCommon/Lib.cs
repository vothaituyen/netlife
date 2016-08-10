using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Web;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;


namespace ATVCommon
{
    public static class Lib
    {
        /// <summary>
        /// Chuyển đổi 1 giá trị sang kiểu Integer
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi</param>
        /// <returns>Số kiểu Integer, nếu lỗi return int.MinValue</returns>
        public static int Object2Integer(object value)
        {
            if (null == value) return 0;
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Chuyển đổi 1 giá trị sang kiểu Long
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi</param>
        /// <returns>Số kiểu Long, nếu lỗi return long.MinValue</returns>
        public static long Object2Long(object value)
        {
            if (null == value) return 0;
            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Chuyển đổi 1 giá trị sang kiểu Double
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi</param>
        /// <returns>Số kiểu Double, nếu lỗi return double.NaN</returns>
        public static double Object2Double(object value)
        {
            if (null == value) return 0;
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Chuyển đổi 1 giá trị sang kiểu float
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi</param>
        /// <returns>Số kiểu float, nếu lỗi return float.NaN</returns>
        public static float Object2Float(object value)
        {
            if (null == value) return 0;
            try
            {
                return float.Parse(value.ToString());
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Chuyển đổi 1 giá trị sang kiểu boolean
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi</param>
        /// <returns>giá trị kiểu boolean, nếu lỗi return false</returns>
        public static bool Object2Boolean(object value)
        {
            if (null == value) return false;
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Chuyển đổi 1 giá trị sang kiểu DateTime
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi</param>
        /// <returns>Số kiểu DateTime, nếu lỗi return DateTime.MinValue</returns>
        public static DateTime Object2DateTime(object value)
        {
            if (null == value) return DateTime.MinValue;
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// Chuyển đổi 1 xâu ngày tháng dạng dd/MM/yyyy sang ngày tháng
        /// </summary>
        /// <param name="value">Xâu nhập</param>
        /// <returns>Trả về kiểu DateTime cua ngày cần chuyển đổi (Nếu lỗi thì trả về DateTime.MinValue)</returns>
        public static DateTime String2Date(string value)
        {
            string temp = value;

            string date = temp.Substring(0, temp.IndexOf("/"));
            temp = temp.Substring(temp.IndexOf("/") + 1);
            string month = temp.Substring(0, temp.IndexOf("/"));
            string year = temp.Substring(temp.IndexOf("/") + 1);

            string[] months = new string[] { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            try
            {
                return Convert.ToDateTime(date + " " + months[Convert.ToInt32(month) - 1] + " " + year);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// Lấy 1 xâu ngẫu nhiên
        /// </summary>
        /// <param name="length">Số lượng ký tự</param>
        /// <returns>Xâu ngẫu nhiên</returns>
        public static string GetRamdomString(int length)
        {
            string temp = "";
            string[] myAlphabet = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            Random Rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                temp += myAlphabet[Rnd.Next(0, myAlphabet.Length - 1)];
            }
            return temp;
        }

        #region Chuyen doi xau dang unicode co dau sang dang khong dau
        private const string KoDauChars =
            "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";

        private const string uniChars =
            "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";

        /// <summary>
        /// Chuyển đổi 1 xâu từ dạng unicode có dấu sang dạng unicode không dấu
        /// </summary>
        /// <param name="s">xâu unicode có dấu</param>
        /// <returns>xâu unicode không dấu đã convert</returns>
        public static string UnicodeToKoDau(string s)
        {
            string retVal = String.Empty;
            s = s.Trim();
            int pos;
            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += KoDauChars[pos];
                else
                    retVal += s[i];
            }
            return retVal;
        }

        /// <summary>
        /// Chuyển đổi 1 xâu từ dạng unicode có dấu sang dạng unicode không dấu và có gạch ngăn cách giữa mỗi từ
        /// </summary>
        /// <param name="s">xâu unicode có dấu</param>
        /// <returns>xâu unicode không dấu và có gạch ngăn cách giữa mỗi từ</returns>
        public static string UnicodeToKoDauAndGach(string s)
        {
            string strChar = "abcdefghiklmnopqrstxyzuvxw0123456789 ";
            //string retVal = UnicodeToKoDau(s);
            //s = s.Replace("-", " ");
            s = s.Replace("–", "");
            s = s.Replace("  ", " ");
            s = UnicodeToKoDau(s.ToLower().Trim());
            string sReturn = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (strChar.IndexOf(s[i]) > -1)
                {
                    if (s[i] != ' ')
                        sReturn += s[i];
                    else if (i > 0 && s[i - 1] != ' ' && s[i - 1] != '-')
                        sReturn += "-";
                }
            }

            return sReturn;
        }
        #endregion

        #region Build links
        /// <summary>
        /// Build link cho trang danh sách tin
        /// </summary>
        /// <param name="catName">CategoryName</param>
        /// <param name="pageIndex">PageIndex</param>
        /// <returns>link cho trang danh sách tin</returns>
        public static string BuildLink_ListNewsDetails(int parentCatId, int catId, string catName, int pageIndex)
        {
            string urlFormat_Paged = "/c{0}s{1}/{2}/trang-{3}.html";
            string urlFormat_WithoutPaged = "/c{0}s{1}/{2}.html";

            if (parentCatId <= 0) parentCatId = catId;
            if (catId <= 0) catId = parentCatId;

            if (pageIndex > 0)
            {
                return string.Format(urlFormat_Paged, parentCatId, catId, UnicodeToKoDauAndGach(catName), pageIndex);
            }
            else
            {
                return string.Format(urlFormat_WithoutPaged, parentCatId, catId, UnicodeToKoDauAndGach(catName));
            }
        }
        /// <summary>
        /// Build link cho trang tin chi tiết
        /// </summary>
        /// <param name="catName">CategoryName</param>
        /// <param name="newsId">NewsID</param>
        /// <param name="newsTitle">NewsTitle</param>
        /// <returns>link cho trang tin chi tiết</returns>
        public static string BuildLink_NewsDetails(int parentCatId, int catId, long newsId, string newsTitle)
        {
            string urlFormat = "/c{0}/s{1}-{2}/{3}.html";

            if (parentCatId <= 0) parentCatId = catId;
            if (catId <= 0) catId = parentCatId;

            return string.Format(urlFormat, parentCatId, catId, newsId, UnicodeToKoDauAndGach(newsTitle));
        }

        /// <summary>
        /// Build Link cho trang danh sách tin theo chuyên đề
        /// </summary>
        /// <param name="EventID">Chuyên đề ID</param>
        /// <param name="EventName">Tên chuyên đề</param>
        /// <returns></returns>
        public static string BuildLinkEvents(int EventID, string EventName)
        {
            string urlFormat = "/event-{0}/{1}.html";
            return string.Format(urlFormat, EventID, UnicodeToKoDauAndGach(EventName));
        }

        #endregion

        #region QueryStrings
        public static class QueryString
        {
            public static int CategoryID
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Cat_ID"])) return 0;

                    return Lib.Object2Integer(HttpContext.Current.Request.QueryString["Cat_ID"]);
                }
            }

            public static string Key
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["key"])) return "";

                    return HttpContext.Current.Request.QueryString["key"].ToString();
                }
            }

            public static bool TS
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Ts"])) return false;

                    return true;
                }
            }
            public static int ParentCategoryID
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Cat_ParentID"])) return 0;

                    return Lib.Object2Integer(HttpContext.Current.Request.QueryString["Cat_ParentID"]);
                }
            }
            public static string CategoryName
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Cat_Name"])) return "";

                    return HttpContext.Current.Request.QueryString["Cat_Name"];
                }
            }
            public static string Title
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["News_Title"])) return "";

                    return HttpContext.Current.Request.QueryString["News_Title"];
                }
            }
            public static int PageIndex
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["PageIndex"])) return 1;

                    return Lib.Object2Integer(HttpContext.Current.Request.QueryString["PageIndex"]);
                }
            }
            public static long NewsID
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["News_ID"])) return 0;

                    return Lib.Object2Long(HttpContext.Current.Request.QueryString["News_ID"]);
                }
            }
            public static Int32 EventID
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Event_ID"])) return 0;

                    return Lib.Object2Integer(HttpContext.Current.Request.QueryString["Event_ID"]);
                }
            }
            public static string Event_Name
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Event_Name"])) return String.Empty;

                    return HttpContext.Current.Request.QueryString["Event_Name"].ToString();
                }
            }

            public static DateTime Date
            {
                get
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Date"])) return DateTime.MinValue;
                    try
                    {
                        return Convert.ToDateTime(HttpContext.Current.Request.QueryString["Date"], new CultureInfo(1066));
                    }
                    catch { return DateTime.MinValue; }
                }
            }
            
        }
        #endregion
       

        #region SqlHelper
        public static int ExecuteNoneQuery(string connectionString, string commandText, CommandType commandType, params SqlParameter[] sqlParams)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, cnn))
                {
                    cmd.CommandType = commandType;

                    foreach (SqlParameter param in sqlParams)
                    {
                        cmd.Parameters.Add(param);
                    }

                    int returnValue = 0;

                    cnn.Open();
                    returnValue = cmd.ExecuteNonQuery();
                    cnn.Close();

                    return returnValue;
                }
            }
        }

        public static DataTable ExecuteDataTable(string connectionString, string commandText, CommandType commandType, params SqlParameter[] sqlParams)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, cnn))
                {
                    cmd.CommandType = commandType;

                    foreach (SqlParameter param in sqlParams)
                    {
                        cmd.Parameters.Add(param);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dtReturnValue = new DataTable();
                        adapter.Fill(dtReturnValue);
                        return dtReturnValue;
                    }
                }
            }
        }

        public static DataTable ExecuteDataTable(string connectionString, string commandText, CommandType commandType, int startRecord, int maxRecord, params SqlParameter[] sqlParams)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, cnn))
                {
                    cmd.CommandType = commandType;

                    foreach (SqlParameter param in sqlParams)
                    {
                        cmd.Parameters.Add(param);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dtReturnValue = new DataTable();
                        adapter.Fill(startRecord, maxRecord, dtReturnValue);
                        return dtReturnValue;
                    }
                }
            }
        }
        #endregion

        #region Log methods
        public static void WriteLogToFile(string message)
        {
            string filePath = ConfigurationManager.AppSettings["LogFile"];

            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    StreamWriter writer;
                    if (!File.Exists(filePath))
                    {
                        writer = File.CreateText(filePath);
                    }
                    else
                    {
                        writer = File.AppendText(filePath);
                    }

                    writer.WriteLine("\n" + message);
                    writer.Close();
                    writer.Dispose();
                }
                catch
                {
                }
            }
        }
        public static void WriteLogToEventLog(string group, Exception ex, EventLogEntryType logType)
        {
            if (ex.InnerException != null)
            {
                EventLog.WriteEntry(group, ex.InnerException.Message + ".\n" + ex.InnerException.StackTrace, logType);
            }
            else
            {
                EventLog.WriteEntry(group, ex.Message + ".\n" + ex.StackTrace, logType);
            }
        }
        public static void WriteLogToEventLog(string group, string message, EventLogEntryType logType)
        {
            EventLog.WriteEntry(group, message, logType);
        }
        public static void WriteLog(string group, Exception ex, EventLogEntryType logType)
        {
            if (Lib.Object2Boolean(ConfigurationManager.AppSettings["LogEvents"]))
            {
                if (ex.InnerException != null)
                {
                    EventLog.WriteEntry(group, ex.InnerException.Message + ".\n" + ex.InnerException.StackTrace, logType);
                }
                else
                {
                    EventLog.WriteEntry(group, ex.Message + ".\n" + ex.StackTrace, logType);
                }
            }
        }
        public static void WriteLog(string group, string message, EventLogEntryType logType)
        {
            if (Lib.Object2Boolean(ConfigurationManager.AppSettings["LogEvents"]))
            {
                EventLog.WriteEntry(group, message, logType);
            }
        }
        #endregion

        public static void SetCanonicalLink(Page p, string url)
        {
            HtmlLink link = new HtmlLink();
            link.Href = url.StartsWith("http") ? url : "http://netlife.com.vn" + url;
            link.Attributes.Add("rel", "canonical");
            p.Header.Controls.Add(link);
        }
       
    }
}