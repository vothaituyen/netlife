using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class ThreadEntity
    {
        int threadId;
        string threadRc = "0";
        string title;
        string url;
        string img;
        string desc = string.Empty;

        public int ThreadId { set { threadId = value; } get { return threadId; } }
        public string Title { set { title = value; } get { return title; } }
        public string Desc { set { desc = value; } get { return desc; } }
        public string ThreadRc { set { threadRc = value; } get { return threadRc; } }
        public string Url { set { url = value; } get { return url; } }
        public string Image { set { img = value; } get { return img; } }
    }
}
