using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{

    public class AdvItemEntity
    {
        public AdvItemEntity()
        {
            _ID = 0;
            _Name = String.Empty;
            _SourceFile = String.Empty;
            _TargetUrl = String.Empty;
            _Type = 0;
            _isActive = false;
            _STT = 0;
            _IsRoate = false;
        }

        int _ID;
        bool _IsRoate;
        string _Name;
        string _SourceFile;
        string _TargetUrl;
        int _Type;
        bool _isActive;
        int _STT;
        int defaultWidth;
        string html;
        int height;
        public string Html { get { return html; } set { html = value; } }
        public int Width { get { return defaultWidth; } set { defaultWidth = value; } }
        public int Height { get { return height; } set { height = value; } }
        public int ID { get { return _ID; } set { _ID = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string SourceFile { get { return _SourceFile; } set { _SourceFile = value; } }
        public string TargetUrl { get { return _TargetUrl; } set { _TargetUrl = value; } }
        public int Type { get { return _Type; } set { _Type = value; } }
        public bool isActive { get { return _isActive; } set { _isActive = value; } }
        public int STT { get { return _STT; } set { _STT = value; } }
        public bool IsRotate { get { return _IsRoate; } set { _IsRoate = value; } }
    }

}

