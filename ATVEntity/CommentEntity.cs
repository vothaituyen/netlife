using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    [Serializable]
    public class CommentEntity
    {
        private Int64 _Comment_ID;
        private long _News_ID;
        private long _Rate;
        private string _Comment_User;
        private string _Comment_Content;
        private DateTime _Comment_Date;
        private string _Comment_Email;
        private string _Avatar;
        private Int64 _commentParent;
        private int _CountLike;
        private int _Status;
        private Int64 _ParentID;
        public Int64 Comment_ID { get { return _Comment_ID; } set { _Comment_ID = value; } }
        public long News_ID { get { return _News_ID; } set { _News_ID = value; } }
        public long Rate { get { return _Rate; } set { _Rate = value; } }
        public string Comment_User { get { return _Comment_User; } set { _Comment_User = value; } }
        public string Comment_Content { get { return _Comment_Content; } set { _Comment_Content = value; } }
        public DateTime Comment_Date { get { return _Comment_Date; } set { _Comment_Date = value; } }
        public string Comment_Email { get { return _Comment_Email; } set { _Comment_Email = value; } }
        public string Avatar { get { return _Avatar; } set { _Avatar = value; } }
        public Int64 CommentParent { get { return _commentParent; } set { _commentParent = value; } }
        public int CommentLike { get { return _CountLike; } set { _CountLike = value; } }
        public int Status { get { return _Status; } set { _Status = value; } }
        public Int64 ParentID { get { return _ParentID; } set { _ParentID = value; } }
    }
}
