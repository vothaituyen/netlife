using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATVEntity
{
    [Serializable]
    public class UploadEntity
    {
        private Int64 _Upload_ID;
        private string _Upload_User;
        private string _Upload_Content;
        private DateTime _Upload_Date;
        private string _Upload_Email;
        private string _Avatar;

        public Int64 UploadId { get { return _Upload_ID; } set { _Upload_ID = value; } }
        public string Upload_User { get { return _Upload_User; } set { _Upload_User = value; } }
        public string Upload_Content { get { return _Upload_Content; } set { _Upload_Content = value; } }
        public DateTime Upload_Date { get { return _Upload_Date; } set { _Upload_Date = value; } }
        public string Upload_Email { get { return _Upload_Email; } set { _Upload_Email = value; } }
        public string Avatar { get { return _Avatar; } set { _Avatar = value; } }
    }
}