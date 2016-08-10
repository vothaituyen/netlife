using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    [Serializable]
    public class CategoryEntity
    {

        public CategoryEntity()
        {
            this._Cat_ParentID = 0;
            this._Cat_isHidden = false;
            this._Cat_Order = 0;
            this._Cat_isColumn = false;
            href = String.Empty;
        }

        public int Cat_ID { get { return _Cat_ID; } set { _Cat_ID = value; } }
        public string Cat_Name { get { return _Cat_Name; } set { _Cat_Name = value; } }
        public string Cat_Description { get { return _Cat_Description; } set { _Cat_Description = value; } }
        public string Cat_DisplayURL { get { return _Cat_DisplayURL; } set { _Cat_DisplayURL = value; } }
        public int Cat_ParentID { get { return _Cat_ParentID; } set { _Cat_ParentID = value; } }
        public bool Cat_isColumn { get { return _Cat_isColumn; } set { _Cat_isColumn = value; } }
        public bool Cat_isHidden { get { return _Cat_isHidden; } set { _Cat_isHidden = value; } }
        public string Cat_Icon { get { return _Cat_Icon; } set { _Cat_Icon = value; } }
        public string HREF
        {
            get
            {
                return String.Format("/{0}.html",_Cat_DisplayURL.Trim().ToLower());
            }

        }
        public int Cat_Order { get { return _Cat_Order; } set { _Cat_Order = value; } }
        private int _Cat_ID;
        private string _Cat_Name;
        private string _Cat_Description;
        private string _Cat_DisplayURL;
        private int _Cat_ParentID;
        private bool _Cat_isHidden;
        private bool _Cat_isColumn;
        private string _Cat_Icon;
        private int _Cat_Order;
        private string href;
    }
}
