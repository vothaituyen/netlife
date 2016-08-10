using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace ATVCommon
{
    public class ControlBase : UserControl
    {
        private int _Cat_ID;
        public int Cat_ID { get { return _Cat_ID; } set { _Cat_ID = value; } }
        private int _Cat_ParentID;
        public int Cat_ParentID { get { return _Cat_ParentID; } set { _Cat_ParentID = value; } }
    }
}
