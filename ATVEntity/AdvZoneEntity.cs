using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class AdvZoneEntity
    {
        public AdvZoneEntity()
        {
            _ID = 0;
            _Name = String.Empty;
            _Cat_ID = 0;
            _isActive = false;
            _WidthDefault = 0;
        }

        int _ID;
        string _Name;
        int _Cat_ID;
        bool _isActive;
        int _WidthDefault;

        public int ID { get { return _ID; } set { _ID = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public int Cat_ID { get { return _Cat_ID; } set { _Cat_ID = value; } }
        public bool isActive { get { return _isActive; } set { _isActive = value; } }
        public int WidthDefault { get { return _WidthDefault; } set { _WidthDefault = value; } }

    }

}
