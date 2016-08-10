using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ATVEntity
{
    public class CineEntity
    {
        public CineEntity()
        {
            this._CineID = 0;
            this._Content = String.Empty;
            this._CreatedDate = DateTime.Now;
            this._Desc = String.Empty;
            this._Image = String.Empty;
            this._IsActive = false;
            this._Link = String.Empty;
            this._Type = 1;
            this._Order = 0;
        }

        int _CineID;
        string _Title;
        string _Content;
        string _Link;
        int _Order;
        string _Desc;
        bool _IsActive;
        string _Image;
        int _Type;
        DateTime _CreatedDate;

        public int CineID { set { _CineID = value; } get { return _CineID; } }
        public string Title { set { _Title = value; } get { return HttpUtility.HtmlEncode(_Title); } }
        public string Content { set { _Content = value; } get { return _Content; } }
        public string Link { set { _Link = value; } get { return _Link; } }
        public int Order { set {_Order  = value; } get { return _Order; } }
        public string Desc { set {_Desc  = value; } get { return _Desc; } }
        public bool IsActive { set {_IsActive  = value; } get { return _IsActive; } }
        public string Image { set {_Image  = value; } get { return _Image; } }
        public int Type { set { _Type = value; } get { return _Type; } }
        public DateTime CreatedDate { set { _CreatedDate = value; } get { return _CreatedDate; } }
    }
}
