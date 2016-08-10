namespace Channelvn.Cached.UrlRewrite
{
    class CachedKeyword
    {
        public static string Log_getkeyword(string referrer)
        {
            string keyword = "";
            string sr = Log_getParam(referrer);
            if (sr != null) keyword = getQueryString(referrer, sr);
            return keyword;
        }

        protected static string getQueryString(string referrer, string param)
        {
            if (referrer.IndexOf("?") == -1) return "";
            string temp = referrer.Split('?')[1];
            string[] arrTemp = temp.Split('&');
            string[] atemp = new string[2];
            for (int i = 0; i < arrTemp.Length; i++)
            {
                atemp = arrTemp[i].ToString().Split('=');
                if (atemp.Length >= 2 && atemp[0] == param) { return atemp[1]; }
            }
            return "";
        }

        protected static string Log_getParam(string referrer)
        {
            string[] _uOsr = new string[29];
            string[] _uOkw = new string[29];
            _uOsr[0] = "google"; _uOkw[0] = "q";
            _uOsr[1] = "yahoo"; _uOkw[1] = "p";
            _uOsr[2] = "msn"; _uOkw[2] = "q";
            _uOsr[3] = "aol"; _uOkw[3] = "query";
            _uOsr[4] = "aol"; _uOkw[4] = "encquery";
            _uOsr[5] = "lycos"; _uOkw[5] = "query";
            _uOsr[6] = "ask"; _uOkw[6] = "q";
            _uOsr[7] = "altavista"; _uOkw[7] = "q";
            _uOsr[8] = "netscape"; _uOkw[8] = "s";
            _uOsr[9] = "cnn"; _uOkw[9] = "query";
            _uOsr[10] = "looksmart"; _uOkw[10] = "qt";
            _uOsr[11] = "about"; _uOkw[11] = "terms";
            _uOsr[12] = "mamma"; _uOkw[12] = "query";
            _uOsr[13] = "alltheweb"; _uOkw[13] = "q";
            _uOsr[14] = "gigablast"; _uOkw[14] = "q";
            _uOsr[15] = "voila"; _uOkw[15] = "rdata";
            _uOsr[16] = "virgilio"; _uOkw[16] = "qs";
            _uOsr[17] = "live"; _uOkw[17] = "q";
            _uOsr[18] = "baidu"; _uOkw[18] = "wd";
            _uOsr[19] = "alice"; _uOkw[19] = "qs";
            _uOsr[20] = "seznam"; _uOkw[20] = "w";
            _uOsr[21] = "yandex"; _uOkw[21] = "text";
            _uOsr[22] = "najdi"; _uOkw[22] = "q";
            _uOsr[23] = "aol"; _uOkw[23] = "q";
            _uOsr[24] = "club-internet"; _uOkw[24] = "q";
            _uOsr[25] = "mama"; _uOkw[25] = "query";
            _uOsr[26] = "seznam"; _uOkw[26] = "q";
            _uOsr[27] = "search"; _uOkw[27] = "q";
            _uOsr[28] = "localhost"; _uOkw[28] = "KeySearch";
            for (int i = 0; i < _uOsr.Length; i++)
            {
                if (referrer.IndexOf(_uOsr[i]) > -1) return _uOkw[i];
            }
            return null;
        }
    }
}
