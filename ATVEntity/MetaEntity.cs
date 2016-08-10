using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    [Serializable]
    public class MetaEntity
    {
        string title = string.Empty;
        string keyword = string.Empty;
        public string Description { set { title = value; } get { return title; } }
        public string Keyword { set { keyword = value; } get { return keyword; } }
    }
}
