using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class AdvZoneCategory
    {
        int _Zone_ID;
        int _Item_ID;
        int _Cat_ID;
        int _Order;
        int _Width;
        public int Width { get { return _Width; } set { _Width = value; } }
        public int Zone_ID { get { return _Zone_ID; } set {_Zone_ID = value; } }
        public int Item_ID { get { return _Item_ID;} set {_Item_ID = value; } }
        public int Cat_ID { get { return _Cat_ID; } set { _Cat_ID = value; } }
        public int Order { get { return _Order; } set { _Order = value; } }
    }
}
