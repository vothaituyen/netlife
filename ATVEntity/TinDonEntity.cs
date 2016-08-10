using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class TinDonEntity
    {
        public TinDonEntity()
        {
            this._Content = "";
            this._Email = "";
            this._TinDon_ID = 0;
            this._User = "";
            this.Type = 1;
        }

        int _TinDon_ID;
        string _User;
        string _Content;
        string _Email;
        int _Type;

        public int TinDon_ID { get { return _TinDon_ID; } set { _TinDon_ID = value; } }
        public string User { get { return _User; } set { _User = value; } }
        public string Content { get { return _Content; } set { _Content = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }
        public int Type { get { return _Type; } set { _Type = value; } }
    }
}
