using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class FlashHomeEntity
    {
        long _News_ID = 0;
        string _News_Image = "";
        int _Top = 0;
        int _Left = 0;
        int _Width = 0;
        int _Height = 0;
        string _News_Title = "";
        string _News_InitContent = "";
        string _Url = "";
        public string News_Title { get { return _News_Title; } set { _News_Title = value; } }
        public string News_InitContent { get { return _News_InitContent; } set { _News_InitContent = value; } }
        public string Url { get { return _Url; } set { _Url = value; } }
        public long News_ID { get { return _News_ID; } set { _News_ID = value; } }
        public string News_Image { get { return _News_Image; } set { _News_Image = value; } }
        public int Top { get { return _Top; } set { _Top = value; } }
        public int Left { get { return _Left; } set { _Left = value; } }
        public int Width { get { return _Width; } set { _Width = value; } }
        public int Height { get { return _Height; } set { _Height = value; } }
    }
}
