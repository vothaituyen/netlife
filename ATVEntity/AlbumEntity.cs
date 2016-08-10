using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class AlbumEntity
    {
        public AlbumEntity()
        {
            this._AlbumID = 0;
            this._AlbumName = "";
            this._imageURL = "";
        }
        string _AlbumName;
        int _AlbumID;
        string _imageURL;
        public string LinkImage { get { return String.Format("<a href=\"#\"><img src=\"{0}\"/></a>", _imageURL); } }


        public string AlbumName { set { _AlbumName = value; } get { return _AlbumName; } }
        public int AlbumID { set { _AlbumID = value; } get { return _AlbumID; } }
        public string imageURL { set { _imageURL = value; } get { return _imageURL; } }
   
    }
}
