using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BOATV
{
    /// <summary>
    /// Sample: string s = GetQuote.GetQuoteText("Cộng hòa xã hội chủ nghĩa Việt Nam. Chúng tôi đầu tư nhiều vào chứng khoán nhưng chỉ thua lỗ triền miên. 
    /// Vì chuyện đời không như tháng năm mơ mộng",new string[] { "đầu tư", "mơ mộng" }, "<b>", "</b>", 400);
    /// </summary>
    public class GetQuote
    {
        public static string GetQuoteText(string input, string keyword, bool phraseMarkup, string openMarkup, string closeMarkup, int maxLength)
        {
            if (phraseMarkup)
            {
                string[] k = (CutStringNo(keyword)).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return GetQuoteText(input, k, openMarkup, closeMarkup, maxLength);
            }
            else
                return GetQuoteText(input, new string[] { keyword }, openMarkup, closeMarkup, maxLength);
        }

        public static string GetQuoteText(string input, string[] keywords, string openMarkup, string closeMarkup, int maxLength)
        {
            string quoteText = "";
            string fullPlainContent = CutStringNo(input);
            string s = " " + input + " ";
            string fullPlainContentLower = fullPlainContent.ToLower();

            List<KeyValuePair<string, int>> foundSentences = new List<KeyValuePair<string, int>>();

            List<string> foundEntity = new List<string>();

            for (int i = 0; i < keywords.Length; i++)
            {
                int spIndex = fullPlainContentLower.LastIndexOf(CutStringNo(keywords[i]).ToLower());
                if (spIndex >= 0)
                {
                    foundEntity.Add(keywords[i]);
                    foundSentences.Add(new KeyValuePair<string, int>(GetSentenceFromIndex(s, spIndex), 1));
                }
            }

            if (foundEntity.Count > 0)
            {
                foundEntity = foundEntity.Distinct().ToList();
                quoteText = _GetQuoteText(input, foundSentences, foundEntity, openMarkup, closeMarkup, maxLength);
            }
            return quoteText;
        }

        private static string _GetQuoteText(string fullText, List<KeyValuePair<string, int>> kvp, List<string> foundEntity, string openMarkup, string closeMarkup, int maxLength)
        {
            string s = "";

            List<StringSegment> sseg = new List<StringSegment>();

            for (int i = 0; i < kvp.Count; i++)
            {
                int begin = fullText.IndexOf(kvp[i].Key);
                if (begin >= 0)
                {
                    int end = begin + kvp[i].Key.Length;
                    sseg.Add(new StringSegment(begin, end));
                }
            }

            StringSegmentComparer sc = new StringSegmentComparer();
            sseg.Sort(sc.Compare);


            int index = 0;
            int beginPos = sseg[index].Begin;
            while (index < sseg.Count - 1 && s.Length < maxLength)
            {
                if (sseg[index + 1].Begin - sseg[index].End > 20)
                {
                    if (s.TrimEnd().EndsWith("...") || beginPos == 0)
                        s += fullText.Substring(beginPos, sseg[index].End - beginPos + 1) + "... ";
                    else
                        s += "..." + fullText.Substring(beginPos, sseg[index].End - beginPos + 1) + "... ";
                    index++;
                    beginPos = sseg[index].Begin;
                }
                else
                {
                    index++;
                }
            }
            if (s.TrimEnd().EndsWith("...") || beginPos == 0)
                s += fullText.Substring(beginPos, Math.Min(sseg[sseg.Count - 1].End - beginPos + 1, fullText.Length - beginPos)) + "... ";
            else
                s += "..." + fullText.Substring(beginPos, Math.Min(sseg[sseg.Count - 1].End - beginPos + 1, fullText.Length - beginPos)) + "... ";

            for (int i = 0; i < foundEntity.Count; i++)
            {
                s = Regex.Replace(s, foundEntity[i], String.Format("{0}{1}{2}", openMarkup, foundEntity[i], closeMarkup), RegexOptions.IgnoreCase);
                //s = s.Replace(foundEntity[i], String.Format("<b>{0}</b>", foundEntity[i])); //<font color=\"red\">{0}</font>"
            }

            return s.Replace("\r\n", ". ");
        }

        private static string CutStringNo(string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            return " " + source.Replace('!', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('/', ' ').Replace('\'', ' ').Replace('"', ' ').Replace(';', ' ').Replace(':', ' ').Replace(',', ' ').Replace('.', ' ').Replace('?', ' ').Replace('+', ' ').Replace('”', ' ').Replace('“', ' ') + " ";
        }

        private static string GetSentenceFromIndex(string s, int index)
        {
            int begin = index;
            int end = index + 1;
            int L = s.Length - 1;

            if (index < 0 || index > L)
                return "";

            try
            {
                while (begin > 0 && !(s[begin] == '.' && s[begin + 1] == ' ') && !(s[begin] == '!' && s[begin + 1] == ' ') && !(s[begin] == '?' && s[begin + 1] == ' ') && s[begin] != '\n')
                {
                    //if (index - begin > 100)
                    //    if (s[begin] == ' ')
                    //        break;
                    begin--;
                }
            }
            catch (Exception) { }

            if (begin > 0) begin++;

            try
            {
                while (end < L - 1 && !(s[end] == '.' && s[end + 1] == ' ') && s[end] != '!' && s[end] != '?' && s[end] != '\n')
                {
                    //if (end - index > 100)
                    //    if (s[end] == ' ')
                    //        break;
                    end++;
                }
            }
            catch (Exception) { }

            return s.Substring(begin, end - begin).Trim();
        }
    }

    public class StringSegment
    {
        public int Begin { get; set; }
        public int End { get; set; }

        public StringSegment(int _begin, int _end)
        {
            this.Begin = _begin;
            this.End = _end;
        }
    }

    public class StringSegmentComparer : IComparer<StringSegment>
    {
        public int Compare(StringSegment x, StringSegment y)
        {
            if (x == null)
            {
                if (y == null) { return 0; }
                else { return -1; }
            }
            else
            {
                if (y == null) { return 1; }
                else
                {
                    if (x.Begin == y.Begin)
                        return 0;
                    else if (x.Begin > y.Begin)
                        return 1;
                    else return -1;
                }
            }
        }
    }
}
