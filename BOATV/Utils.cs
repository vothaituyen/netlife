using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Net;
using System.Configuration;
using System.Text.RegularExpressions;
using Channelvn.MemcachedProviders.Cache;

namespace BOATV
{
    public class TableName
    {
        public static string DATABASE_NAME = System.Configuration.ConfigurationSettings.AppSettings["CoreDb"];
        public static string LICHTRUYENHINH = "LichTruyenHinh";
        public static string KENHTRUYENHINH = "KenhTruyenHinh";
        public static string NEWSPUBLISHED = "NewsPublished";
        public static string VIDEO_INFO = "VideoInfo";
        public static string CATEGORY = "Category";
        public static string COMMENT = "Comment";
        public static string FLASHHOME = "HomeFlash";
        public static string BONBAINOIBAT = "BonBaiNoiBat";
        public static string THREAD = "NewsThread";
        public static string HTMLCACHED = "HtmlCached";
        public static string TINDON = "TinDon";
        public static string META = "Meta";
        public static string CINE = "Cine";
        public static string QNA = "QnA";
        public static string SinhNhatSao = "SinhNhatSao";
        public static string ALBUM = "Album";
        public static string PHOTODETAIL = "PhotoDetail";
        public static string HOMEFLASHPUBLISHED = "HomeFlashPublished";
        public static string QUANGCAO_ITEM = "QuangCao_Item";
        public static string QUANGCAO_ZONE = "QuangCao_Zone";
        public static string ZONE_ITEM = "Zone_Item";
        public static string VoteItem = "VoteItem";
        public static string Vote = "Vote";

    }
    public enum News_Mode { NOI_BAT_TRANG_CHU = 2, NOI_BAT_MUC = 1, TIN_THONG_THUONG = 0, TIN_HOT = 3 }

    public class NGramKeyword
    {
        public static List<string> GetNGramKeyword(string title, string sapo, string content, int topCount)
        {
            string[] sP = new string[] { ", ", ":", ";", "“", "”", "\"", "'", "‘", "’", "(", ")", "?", "\t", "-" };

            List<string> _nGram = new List<string>();
            Dictionary<string, int> countDict = new Dictionary<string, int>();

            List<string> toExtract = new List<string>();
            toExtract.Add(removeHTMLWithLineAndSpace(title).ToLower());
            toExtract.Add(removeHTMLWithLineAndSpace(sapo).ToLower());
            toExtract.Add(removeHTMLWithLineAndSpace(content).ToLower());

            int L = toExtract.Count;
            for (int i = 0; i < L; i++)
            {
                string[] tq = stringSeparate(toExtract[i]);
                for (int j = 0; j < tq.Length; j++)
                {
                    string[] ss = tq[j].Split(sP, StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < ss.Length; k++)
                    {
                        for (int l = 2; l < 5; l++)
                        {
                            var t_list = NGram(ss[k], l);
                            for (int m = 0; m < t_list.Count; m++)
                            {
                                if (!countDict.ContainsKey(t_list[m]))
                                    countDict.Add(t_list[m], 1);
                                else
                                    countDict[t_list[m]]++;
                            }
                        }
                    }
                }
            }

            Dictionary<string, double> pointDict = new Dictionary<string, double>();
            foreach (var m in countDict)
            {
                var _point = m.Value / 10.0 * m.Key.Split(' ').Length;
                pointDict.Add(m.Key, _point);
            }
            var items = from pair in pointDict
                        orderby pair.Value descending
                        select pair;
            var _items = items.ToList();

            int T = Math.Min(topCount, _items.Count);
            for (int i = 0; i < T; i++)
                _nGram.Add(_items[i].Key);

            return _nGram;
        }

        private static string NormalizeString(string _source)
        {
            if (_source.IsNormalized())
                return _source;
            else
                return _source.Normalize();
        }

        private static string removeHTMLWithLineAndSpace(string source)
        {
            if (source == null) return String.Empty;
            string pattern = @"(<[^>]+>)";
            string text = System.Text.RegularExpressions.Regex.Replace(source, pattern, " ");

            return NormalizeString(HttpUtility.HtmlDecode(text)).Trim();
        }

        private static string[] stringSeparate(string s)
        {
            string[] sentenceSpliter = new string[] { ". ", "\r\n", ":\"", ": ", "? ", "! " };

            string[] result = s.Split(sentenceSpliter, StringSplitOptions.RemoveEmptyEntries);
            List<string> _result = new List<string>();
            for (int i = 0; i < result.Length; i++)
            {
                string x = result[i].Trim(new char[] { ' ', '.', '\r', '\n', '\t', '-' });
                if (x != "")
                    _result.Add(x);
            }
            return _result.ToArray();
        }

        private static bool IsUpper(string s)
        {
            int L = s.Length;
            for (int i = 0; i < L; i++)
            {
                if (Char.IsLower(s[i]))
                    return false;
            }
            return true;
        }

        private static List<string> NGram(string S, int N)
        {
            List<string> _nGram = new List<string>();
            string[] SP = S.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int L = SP.Length - N + 1;

            if (L > 0)
            {
                for (int i = 0; i < L; i++)
                {
                    string temp = "";
                    for (int j = 0; j < N; j++)
                    {
                        temp += SP[i + j] + " ";
                    }
                    string name = temp.Trim();

                    if (name != "")
                        _nGram.Add(name);
                }
            }
            return _nGram;
        }
    }

    public class Utils
    {
        public static string CssVersion = System.Configuration.ConfigurationSettings.AppSettings["CssVersion"] != null ? System.Configuration.ConfigurationSettings.AppSettings["CssVersion"].ToString() : "";
        public static Queue<string> KeyCacheUsed = new Queue<string>();
        public static void SetPageHeader(Page p, string strtitle, string description, string keywords)
        {
            const string formatString = "{0} | NetLife";
            if (p.Master != null)
            {
                var title = (HtmlTitle)p.Master.FindControl("title");
                var metaDesc = (HtmlMeta)p.Master.FindControl("description");
                var keyword = (HtmlMeta)p.Master.FindControl("keywords");
                var newsKeywords = (HtmlMeta)p.Master.FindControl("news_keywords");
                if (strtitle != "" && title != null) title.Text = CatCharactor(String.Format(formatString, strtitle), 70);
                if (description != "" && (metaDesc != null)) metaDesc.Content = CatCharactor(String.Format(formatString, description), 160);
                if (keywords != "" && (keyword != null)) keyword.Content = CatCharactor(String.Format(formatString, keywords), 160);
                if (keywords != "" && (newsKeywords != null)) newsKeywords.Content = keywords;
            }
        }
        /// <summary>
        /// Thiết đặt SEO cho Facebook
        /// </summary>
        /// <param name="p">Page cần Set trạng thái</param>
        /// <param name="title">Tiêu đề bài viết</param>
        /// <param name="sapo">Tóm tắt bài viết</param>
        /// <param name="images">Dường dẫn ảnh nếu có</param>
        /// <param name="url">Đường dẫn đến bài viết mới nhất</param>
        public static void SetFaceBookSEO(Page p, string title, string sapo, string images, string url)
        {
            try
            {

                #region Facebook
                HtmlMeta m = new HtmlMeta();
                if (!string.IsNullOrEmpty(title))
                {
                    m.Attributes.Add("property", "og:title");
                    m.Content = HttpUtility.HtmlDecode(title);
                    p.Header.Controls.AddAt(1, m);
                    //p.Header.Controls.Add(new LiteralControl("br/"));
                }

                m = new HtmlMeta();
                m.Attributes.Add("property", "og:type");
                m.Attributes.Add("content", "article");
                p.Header.Controls.AddAt(1, m);
                //p.Header.Controls.Add(new LiteralControl("br/"));

                if (!string.IsNullOrEmpty(url))
                {
                    m = new HtmlMeta();
                    m.Attributes.Add("property", "og:url");
                    m.Attributes.Add("content", url.StartsWith("http") ? url : "http://" + HttpContext.Current.Request.Url.DnsSafeHost + url);
                    p.Header.Controls.AddAt(1, m);
                    //p.Header.Controls.Add(new LiteralControl("br/"));
                }
                if (!string.IsNullOrEmpty(images))
                {
                    m = new HtmlMeta();
                    m.Attributes.Add("property", "og:image");
                    m.Attributes.Add("content", images);
                    p.Header.Controls.AddAt(1, m);
                    //p.Header.Controls.Add(new LiteralControl("br/"));
                }
                m = new HtmlMeta();
                m.Attributes.Add("property", "og:site_name");
                m.Attributes.Add("content", HttpContext.Current.Request.Url.DnsSafeHost);
                p.Header.Controls.AddAt(1, m);
                //p.Header.Controls.Add(new LiteralControl("br/"));

                if (!string.IsNullOrEmpty(sapo))
                {
                    m = new HtmlMeta();
                    m.Attributes.Add("property", "og:description");
                    m.Content = HttpUtility.HtmlDecode(sapo);
                    p.Header.Controls.AddAt(1, m);
                    //p.Header.Controls.Add(new LiteralControl("<br/>"));
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// Size of the regular byte in bits
        /// </summary>
        private const int InByteSize = 8;

        /// <summary>
        /// Size of converted byte in bits
        /// </summary>
        private const int OutByteSize = 5;

        /// <summary>
        /// Alphabet
        /// </summary>
        private const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /// <summary>
        /// Convert byte array to Base32 format
        /// </summary>
        /// <param name="bytes">An array of bytes to convert to Base32 format</param>
        /// <returns>Returns a string representing byte array</returns>
        public static string ToBase32String(byte[] bytes)
        {
            // Check if byte array is null
            if (bytes == null)
            {
                return null;
            }
            // Check if empty
            else if (bytes.Length == 0)
            {
                return string.Empty;
            }

            // Prepare container for the final value
            StringBuilder builder = new StringBuilder(bytes.Length * InByteSize / OutByteSize);

            // Position in the input buffer
            int bytesPosition = 0;

            // Offset inside a single byte that <bytesPosition> points to (from left to right)
            // 0 - highest bit, 7 - lowest bit
            int bytesSubPosition = 0;

            // Byte to look up in the dictionary
            byte outputBase32Byte = 0;

            // The number of bits filled in the current output byte
            int outputBase32BytePosition = 0;

            // Iterate through input buffer until we reach past the end of it
            while (bytesPosition < bytes.Length)
            {
                // Calculate the number of bits we can extract out of current input byte to fill missing bits in the output byte
                int bitsAvailableInByte = Math.Min(InByteSize - bytesSubPosition, OutByteSize - outputBase32BytePosition);

                // Make space in the output byte
                outputBase32Byte <<= bitsAvailableInByte;

                // Extract the part of the input byte and move it to the output byte
                outputBase32Byte |= (byte)(bytes[bytesPosition] >> (InByteSize - (bytesSubPosition + bitsAvailableInByte)));

                // Update current sub-byte position
                bytesSubPosition += bitsAvailableInByte;

                // Check overflow
                if (bytesSubPosition >= InByteSize)
                {
                    // Move to the next byte
                    bytesPosition++;
                    bytesSubPosition = 0;
                }

                // Update current base32 byte completion
                outputBase32BytePosition += bitsAvailableInByte;

                // Check overflow or end of input array
                if (outputBase32BytePosition >= OutByteSize)
                {
                    // Drop the overflow bits
                    outputBase32Byte &= 0x1F;  // 0x1F = 00011111 in binary

                    // Add current Base32 byte and convert it to character
                    builder.Append(Base32Alphabet[outputBase32Byte]);

                    // Move to the next byte
                    outputBase32BytePosition = 0;
                }
            }

            // Check if we have a remainder
            if (outputBase32BytePosition > 0)
            {
                // Move to the right bits
                outputBase32Byte <<= (OutByteSize - outputBase32BytePosition);

                // Drop the overflow bits
                outputBase32Byte &= 0x1F;  // 0x1F = 00011111 in binary

                // Add current Base32 byte and convert it to character
                builder.Append(Base32Alphabet[outputBase32Byte]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Convert base32 string to array of bytes
        /// </summary>
        /// <param name="base32String">Base32 string to convert</param>
        /// <returns>Returns a byte array converted from the string</returns>
        public static byte[] FromBase32String(string base32String)
        {
            // Check if string is null
            if (base32String == null)
            {
                return null;
            }
            // Check if empty
            else if (base32String == string.Empty)
            {
                return new byte[0];
            }

            // Convert to upper-case
            string base32StringUpperCase = base32String.ToUpperInvariant();

            // Prepare output byte array
            byte[] outputBytes = new byte[base32StringUpperCase.Length * OutByteSize / InByteSize];

            // Check the size
            if (outputBytes.Length == 0)
            {
                throw new ArgumentException("");
            }

            // Position in the string
            int base32Position = 0;

            // Offset inside the character in the string
            int base32SubPosition = 0;

            // Position within outputBytes array
            int outputBytePosition = 0;

            // The number of bits filled in the current output byte
            int outputByteSubPosition = 0;

            // Normally we would iterate on the input array but in this case we actually iterate on the output array
            // We do it because output array doesn't have overflow bits, while input does and it will cause output array overflow if we don't stop in time
            while (outputBytePosition < outputBytes.Length)
            {
                // Look up current character in the dictionary to convert it to byte
                int currentBase32Byte = Base32Alphabet.IndexOf(base32StringUpperCase[base32Position]);

                // Check if found
                if (currentBase32Byte < 0)
                {
                    throw new ArgumentException(string.Format("Specified string is not valid Base32 format because character \"{0}\" does not exist in Base32 alphabet", base32String[base32Position]));
                }

                // Calculate the number of bits we can extract out of current input character to fill missing bits in the output byte
                int bitsAvailableInByte = Math.Min(OutByteSize - base32SubPosition, InByteSize - outputByteSubPosition);

                // Make space in the output byte
                outputBytes[outputBytePosition] <<= bitsAvailableInByte;

                // Extract the part of the input character and move it to the output byte
                outputBytes[outputBytePosition] |= (byte)(currentBase32Byte >> (OutByteSize - (base32SubPosition + bitsAvailableInByte)));

                // Update current sub-byte position
                outputByteSubPosition += bitsAvailableInByte;

                // Check overflow
                if (outputByteSubPosition >= InByteSize)
                {
                    // Move to the next byte
                    outputBytePosition++;
                    outputByteSubPosition = 0;
                }

                // Update current base32 byte completion
                base32SubPosition += bitsAvailableInByte;

                // Check overflow or end of input array
                if (base32SubPosition >= OutByteSize)
                {
                    // Move to the next character
                    base32Position++;
                    base32SubPosition = 0;
                }
            }

            return outputBytes;
        }

        static readonly System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
        //Get your key from config file to open the lock!
        static string key = (string)settingsReader.GetValue("SecurityKey",
                                                     typeof(String));
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);




            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);


            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static void SetCanonicalLink(Page p, string url)
        {
            HtmlLink link = new HtmlLink();
            link.Href = url;
            link.Attributes.Add("rel", "canonical");
            p.Header.Controls.Add(link);
        }

        public static void SetPageHeader(Page p, string strtitle, string description, string keywords, int pageIndex)
        {
            string FormatString = "{0} - {1}";
            HtmlTitle title = (HtmlTitle)p.Master.FindControl("title");
            HtmlMeta metaDesc = (HtmlMeta)p.Master.FindControl("description");
            HtmlMeta keyword = (HtmlMeta)p.Master.FindControl("keywords");

            if (strtitle != "" && title != null) title.Text = CatCharactor(String.Format(FormatString, strtitle, pageIndex < 2 ? "" : pageIndex.ToString()), 70);
            if (description != "" && (metaDesc != null))
                metaDesc.Content = CatCharactor(String.Format(FormatString, description, pageIndex), 160);
            if (keywords != "" && (keyword != null)) keyword.Content = CatCharactor(String.Format(FormatString, keywords, pageIndex), 160);
        }

        public static List<string> parseValue(string input, string pattern)
        {

            List<string> returnValue = new List<string>();
            MatchCollection MatchList = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in MatchList) returnValue.Add(match.Value);
            return returnValue;
        }

        static string staticDomain = ConfigurationSettings.AppSettings["staticdomain"] != null ? ConfigurationSettings.AppSettings["staticdomain"] : "http://static.2sao.vn";
        public static void SetStyleFile(Page page, string cssFile, string ie6Css)
        {
            HtmlHead hh = page.Master.FindControl("pageHeader") as HtmlHead;

            if (hh != null)
            {
                Literal lt = new Literal();
                if (cssFile != "")
                    lt.Text = String.Format("<link href=\"{3}{0}?v={1}\" rel=\"stylesheet\" type=\"text/css\" />{2}", cssFile, Utils.CssVersion, Environment.NewLine, staticDomain);
                //if (ie6Css != "")
                //    lt.Text += String.Format("<!--[if (gte IE 5.5)&(lt IE 7)]><link rel=\"stylesheet\" href=\"{3}{0}?v={1}\" type=\"text/css\" /><![endif]-->{2}", ie6Css, Utils.CssVersion, Environment.NewLine, staticDomain);
                hh.Controls.Add(lt);
            }
        }

        public static void SetStyleString(Page page, string stringStyle)
        {
            HtmlGenericControl stylehome = page.Master.FindControl("stylehome") as HtmlGenericControl;
            if (stylehome != null)
            {
                stylehome.InnerHtml = stringStyle;
                stylehome.Visible = true;
            }
        }

        public static string RemoveHTMLCommentTag(string htmlString)
        {
            if (htmlString == null) return String.Empty;
            string pattern = @"<!--(.*?)-->";
            string text = System.Text.RegularExpressions.Regex.Replace(htmlString, pattern, " ");
            return text;
        }

        public static string RemoveHTMLTag(string htmlString)
        {
            if (htmlString == null) return String.Empty;
            string pattern = @"(<[^>]+>)";
            string text = System.Text.RegularExpressions.Regex.Replace(htmlString, pattern, string.Empty);
            return text;
        }

        public static bool isMobileBrowser(HttpContext context)
        {
            //GETS THE CURRENT USER CONTEXT
            // HttpContext context = HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NT CHECK

            if (context.Request.RawUrl.IndexOf("?ismobile=true") > 0)
            {
                context.Session.Add("ViewInWeb", true);
                context.Session.Timeout = 30;
            }

            if (context.Session["ViewInWeb"] != null && Convert.ToBoolean(context.Session["ViewInWeb"].ToString()))
                return false;


            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //Create a list of all mobile types
                string userAgent = context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower();

                if (userAgent.Contains("ipad"))
                {
                    return false;
                }

                string[] mobiles =
                   new string[]
                {
                    "midp", "j2me", "avant", "docomo", 
                    "novarra", "palmos", "palmsource", 
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/", 
                    "blackberry", "mib/", "symbian", 
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio", 
                    "SIE-", "SEC-", "samsung", "HTC", 
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx", 
                    "NEC-", "philips", "mmm", "xx", 
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java", 
                    "pt", "pg", "vox", "amoi", 
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo", 
                    "sgh", "gradi", "jb", "dddi", 
                    "moto", "iphone", "android",
                    "iphone", "opera mini", "sony", "HTC", "eric", "moto", "panasonic", "sharp", "philips", "samsung", "erics", "ericsson", "SonyEricsson"
                };


                //Loop through each item in the list created above 
                //and check if the header contains that text
                foreach (string s in mobiles)
                {
                    if (userAgent.Contains(s.ToLower()))
                    {
                        return true;
                    }
                }


            }

            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }

            return false;
        }

        const string PORT_CACHE = "22";

        private static bool AllowDistCache = ConfigurationSettings.AppSettings["AllowDistCache"] != null &&
                                      ConfigurationSettings.AppSettings["AllowDistCache"].ToString().Equals("1");
        public static string CreateKey(string key)
        {
            return PORT_CACHE + "_" + key;
        }
        public static bool Add_MemCache(string key, object value)
        {
            if (!AllowDistCache)
            {
                HttpRuntime.Cache.Insert(CreateKey(key), value, null, DateTime.Now.AddDays(15), TimeSpan.Zero);
                return true;
            }

            return DistCached.GetInstance(PORT_CACHE).Add(key, value, 99999999999);
        }

        public static bool Add_MemCache(string key, object value, int timeOutInMinutes)
        {
            if (!AllowDistCache)
            {

                HttpRuntime.Cache.Insert(CreateKey(key), value, null, DateTime.Now.AddDays(15), TimeSpan.Zero);
                return true;
            }
            return DistCached.GetInstance(PORT_CACHE).Add(key, value, (long)1000 * 60 * timeOutInMinutes);
        }

        public static void Remove_MemCache(string key)
        {
            if (!AllowDistCache)
            {

                HttpRuntime.Cache.Remove(CreateKey(key));
                return;
            }
            DistCached.GetInstance(PORT_CACHE).Remove(key);
        }

        public static void MonitorCache()
        {
            /*  {0}_NewsDetail
                Danh_Sach_Tin_Theo_Cat-{0}-20-1-213 
                GetListNewsByCatAndDate-{0}-1-2-150
                GetListNewsByCatAndDate-{0}-1-9-140
                GetListNewsByCatAndDate-{0}-1-3-280
                GetListNewsByCatAndDate-{0}-1-3-150
                GetListNewsByNewsMode3-{0}-1-5-6-1-310
                GetListNewsByNewsMode3-{0}-1-5-6-1-440
                GetListNewsByNewsMode3-{0}-1-5-6-1-453
                NewsPublishEntity_Sao_Danh_Sach_Tin-{0}-0-4-1-150
                NP_Sao_Danh_Sach_Tin_Count-{0}-20
                NP_Select_Tin_Tieu_Diem-{0}-47-5-75
                TTOL-GetListBonBaiNoibat-6-440
                NP_Tin_Moi_Trong_Ngay-5-75
                NP_Tin_Moi_Trong_Ngay-7-0
                NP_Tin_Nong-{0}-3-12-90
                NP_Xem_Nhieu_Nhat-7-75-{0}

                Select_Tin_Khac-{0}
                {0}_NewsDetail*/
        }
        public static void Reset_MemCache()
        {
            if (!AllowDistCache)
            {
                var keys = new List<string>();
                // retrieve application Cache enumerator
                var cache = HttpRuntime.Cache;
                var enumerator = cache.GetEnumerator();
                // copy all keys that currently exist in Cache
                while (enumerator.MoveNext())
                {
                    keys.Add(enumerator.Key.ToString());
                }
                // delete every key from cache
                foreach (var t in keys)
                {
                    cache.Remove(t);
                }
            }

            DistCached.GetInstance(PORT_CACHE).RemoveAll();
        }

        public static T Get_MemCache<T>(string key)
        {
            return default(T);

            try
            {
                if (!AllowDistCache)
                {

                    return (T)HttpRuntime.Cache.Get(CreateKey(key));
                }
                return DistCached.GetInstance(PORT_CACHE).Get<T>(key);
            }
            catch
            {
                return default(T);
            }

        }

        public static string Get_MemCache(string key)
        {
            return null;
            try
            {
                if (!AllowDistCache)
                {
                    return HttpRuntime.Cache.Get(CreateKey(key)).ToString();
                }
                return DistCached.GetInstance(PORT_CACHE).Get(key).ToString();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public static int News_ID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(HttpContext.Current.Request.QueryString["News_ID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }


        static int InsertTime = ConfigurationSettings.AppSettings["InsertTime"] != null ? Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["InsertTime"]) : 60;

        public static bool IsAllowInsert
        {
            get
            {
                IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] addr = ipEntry.AddressList;
                string localIP = addr[0].ToString();
                bool result = Utils.GetFromCache(localIP) == null;
                return result;
            }
        }

        public static void InsertFinish()
        {

            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            string localIP = addr[0].ToString();
            HttpContext.Current.Cache.Insert(localIP, localIP, null, DateTime.Now.AddSeconds(InsertTime), TimeSpan.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <param name="cacheName"></param>
        /// <param name="data"></param>
        public static void SaveToCacheDependency(string database, string tableName, string cacheName, object data)
        {
            try
            {
                SqlCacheDependency sqlDep = new SqlCacheDependency(database, tableName);
                if (data != null)
                    HttpContext.Current.Cache.Insert(cacheName, data, sqlDep);

                KeyCacheUsed.Enqueue(cacheName);
            }
            catch { }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheName"></param>
        public static void RemoveFromCache(string cacheName)
        {
            HttpContext.Current.Cache.Remove(cacheName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public static T GetFromCache<T>(string cacheName)
        {
            if (HttpContext.Current == null) return default(T);

            try
            {
                object obj = HttpContext.Current.Cache[cacheName];
                if (obj != null && obj != DBNull.Value)
                    return (T)obj;
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        public static string GetFromCache(string cacheName)
        {

            if (HttpContext.Current == null) return string.Empty;

            try
            {
                object obj = HttpContext.Current.Cache[cacheName];
                if (obj != null && obj != DBNull.Value)
                    return obj.ToString();
                return null;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <param name="cacheName"></param>
        /// <param name="data"></param>
        public static void SaveToCacheDependency(string database, string[] tableName, string cacheName, object data)
        {
            try
            {
                AggregateCacheDependency aggCacheDep = new AggregateCacheDependency();
                SqlCacheDependency[] sqlDepGroup = new SqlCacheDependency[tableName.Length];
                for (int i = 0; i < tableName.Length; i++)
                {
                    sqlDepGroup[i] = new SqlCacheDependency(database, tableName[i]);

                }
                aggCacheDep.Add(sqlDepGroup);
                HttpContext.Current.Cache.Insert(cacheName, data, aggCacheDep);

                KeyCacheUsed.Enqueue(cacheName);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumber(string s)
        {
            try
            {
                int i = Convert.ToInt32(s);
                return true;
            }
            catch { return false; }

        }
        public static T GetObj<T>(object key)
        {
            if (key != null && key != DBNull.Value)
            {
                try
                {
                    return ((T)key);
                }
                catch
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        /* Rewrite*/
        public static string GenerateURL(object Title, object strId)
        {
            string strTitle = Title.ToString();
            strTitle = strTitle.Trim();
            strTitle = strTitle.ToLower();
            char[] chars = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
            strTitle = strTitle.Replace(".", "-");
            for (int i = 0; i < chars.Length; i++)
            {
                string strChar = chars.GetValue(i).ToString();
                if (strTitle.Contains(strChar))
                { strTitle = strTitle.Replace(strChar, string.Empty); }
            }
            strTitle = strTitle.Replace(" ", "-");
            strTitle = strTitle.Replace("--", "-");
            strTitle = strTitle.Replace("---", "-");
            strTitle = strTitle.Replace("----", "-");
            strTitle = strTitle.Replace("-----", "-");
            strTitle = strTitle.Replace("----", "-");
            strTitle = strTitle.Replace("---", "-");
            strTitle = strTitle.Replace("--", "-");
            strTitle = strTitle.Trim();
            strTitle = strTitle.Trim('-');
            strTitle = "~/Article/" + strTitle + "-" + strId + ".aspx";
            return strTitle;
        }
        public const string uniChars = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";
        public const string KoDauChars = "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";
        public static string UnicodeToKoDauAndGach(string s)
        {
            if (s == null) return string.Empty;
            s = HttpUtility.HtmlDecode(s);
            string retVal = String.Empty;
            int pos;
            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += KoDauChars[pos];
                else
                    retVal += s[i];
            }
            retVal = retVal.Replace("  ", "");
            retVal = retVal.Replace(" ", "-");
            retVal = retVal.Replace("--", "-");
            retVal = retVal.Replace(":", "");
            retVal = retVal.Replace(";", "");
            retVal = retVal.Replace("+", "");
            retVal = retVal.Replace("@", "");
            retVal = retVal.Replace(">", "");
            retVal = retVal.Replace("<", "");
            retVal = retVal.Replace("*", "");
            retVal = retVal.Replace("{", "");
            retVal = retVal.Replace("}", "");
            retVal = retVal.Replace("|", "");
            retVal = retVal.Replace("^", "");
            retVal = retVal.Replace("~", "");
            retVal = retVal.Replace("]", "");
            retVal = retVal.Replace("[", "");
            retVal = retVal.Replace("`", "");
            retVal = retVal.Replace(".", "");
            retVal = retVal.Replace("'", "");
            retVal = retVal.Replace("(", "");
            retVal = retVal.Replace(")", "");
            retVal = retVal.Replace(",", "");
            retVal = retVal.Replace("”", "");
            retVal = retVal.Replace("“", "");
            retVal = retVal.Replace("?", "");
            retVal = retVal.Replace("\"", "");
            retVal = retVal.Replace("&", "");
            retVal = retVal.Replace("$", "");
            retVal = retVal.Replace("#", "");
            retVal = retVal.Replace("_", "");
            retVal = retVal.Replace("=", "");
            retVal = retVal.Replace("%", "");
            retVal = retVal.Replace("…", "");
            retVal = retVal.Replace("/", "");
            retVal = retVal.Replace("\\", "");
            retVal = retVal.Replace(" ", "-");
            retVal = retVal.Replace("--", "-");
            retVal = retVal.Replace("---", "-");
            retVal = retVal.Replace("----", "-");
            retVal = retVal.Replace("-----", "-");
            return retVal.ToLower().TrimEnd('-').TrimStart('-');
        }

        public static string UnicodeToKhongDau(string s)
        {
            if (s == null) return string.Empty;
            s = HttpUtility.HtmlDecode(s);
            string retVal = String.Empty;
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

        /* End Rewrite*/
        public static void SetFocusDropdown(DropDownList ddl, string value)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (ddl.Items[i].Value == value)
                {
                    ddl.SelectedIndex = i;
                    return;
                }
            }
            return;
        }

        public static string CatSapo(string input, int sotu)
        {
            if (input == null) return "";
            string[] arr = input.Split(' ');
            if (arr.Length <= sotu) return input;
            else return String.Join(" ", arr, 0, sotu - 1) + " ...";
        }

        public static string CatCharactor(string input, int sokytu)
        {
            if (input == null)
                return "";
            string result = String.Empty;
            string[] arr = input.Split(' ');
            bool needAdd = true;
            int i = 0;
            while (needAdd && i < arr.Length)
            {

                result = String.Concat(result, " ", arr[i]);
                i++;
                if (result.Length > sokytu)
                    needAdd = false;
            }
            return result;
        }


        public static string ImagesThumbUrl = System.Configuration.ConfigurationSettings.AppSettings["ImageUrl"].ToString().TrimEnd('/');
        public static string ImagesStorageUrl = System.Configuration.ConfigurationSettings.AppSettings["ImagesStorageUrl"].ToString().TrimEnd('/');

        public static string GetThumbNail(string title, string url, string img, int width)
        {
            try
            {
                if (img == null || String.IsNullOrEmpty(img))
                    return String.Empty;
                //return String.Format("<a alt=\"{2}\" href=\"{0}\"><img src=\"{1}?width={3}&crop=auto&scale=both\" alt=\"{2}\" border=\"0\"/></a>", url, (img.StartsWith(ImagesStorageUrl) ? img : ImagesStorageUrl + "/" + img), HttpUtility.HtmlEncode(title), width);
                return String.Format("<a alt=\"{2}\" href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" border=\"0\"/></a>", url, (img.StartsWith(ImagesStorageUrl) ? img : ImagesStorageUrl + "/" + img), HttpUtility.HtmlEncode(title), width);
            }
            catch
            {
                return string.Empty;
            }
        }


        public static string ChangeToVietNamDate(DateTime dt)
        {
            string strVietNameDate = "";
            try
            {
                string t = dt.DayOfWeek.ToString();
                string ngay = "";
                switch (t.ToLower())
                {
                    case "monday":
                        ngay = "Thứ hai";
                        break;
                    case "tuesday":
                        ngay = "Thứ ba";
                        break;
                    case "wednesday":
                        ngay = "Thứ tư";
                        break;
                    case "thursday":
                        ngay = "Thứ năm";
                        break;
                    case "friday":
                        ngay = "Thứ sáu";
                        break;
                    case "saturday":
                        ngay = "Thứ bảy";
                        break;
                    case "sunday":
                        ngay = "Chủ nhật";
                        break;
                }
                strVietNameDate = ngay + ", " + dt.ToString("dd/MM/yyyy HH:mm");
            }
            catch { }
            return strVietNameDate;
        }
        public static string ChangeToVietNamDate2(DateTime dt)
        {
            string strVietNameDate = "";
            try
            {
                string t = dt.DayOfWeek.ToString();
                string ngay = "";
                switch (t.ToLower())
                {
                    case "monday":
                        ngay = "Thứ hai";
                        break;
                    case "tuesday":
                        ngay = "Thứ ba";
                        break;
                    case "wednesday":
                        ngay = "Thứ tư";
                        break;
                    case "thursday":
                        ngay = "Thứ năm";
                        break;
                    case "friday":
                        ngay = "Thứ sáu";
                        break;
                    case "saturday":
                        ngay = "Thứ bảy";
                        break;
                    case "sunday":
                        ngay = "Chủ nhật";
                        break;
                }
                strVietNameDate = ngay + ", " + dt.ToString("dd/MM/yyyy");
            }
            catch { }
            return strVietNameDate;
        }

        public static string AddDomain2Link(string PartnerRssLink, string link)
        {
            try
            {
                if (link.ToLower().Trim().StartsWith("http"))
                    return link.Trim();

                Uri r = new Uri(PartnerRssLink);
                return String.Format("http://{0}/{1}", r.DnsSafeHost, link.TrimStart('/'));
            }
            catch { }

            return string.Empty;
        }
        public static string IMAGE_UPLOADED_PATH_ERROR = ConfigurationManager.AppSettings["ImagesStorageUrl"];
        public static string Original_Link(String ImagePath)
        {
            string result = string.Empty;
            if (!String.IsNullOrEmpty(ImagePath) && ImagePath.IndexOf(".") != -1)
            {
                if (ImagePath.IndexOf("http://") == -1)
                    result = IMAGE_UPLOADED_PATH_ERROR + ImagePath;
                else result = ImagePath;
            }
            return result;
        }
        public static string BuildLinkDetail(int cat_parentId, int cat_id, long news_id, string news_title)
        {
            return String.Format("/{0}/{1}p{2}c{3}.html", Utils.UnicodeToKoDauAndGach(news_title), news_id, cat_parentId, cat_id);
        }

        public static void Move301(string url)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Status = "301 Moved Permanently";
            HttpContext.Current.Response.AddHeader("Location", url);
            HttpContext.Current.Response.End();
        }
    }
}