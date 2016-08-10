using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class SinhNhatSaoEntity
    {
        private string sao_Name;
        string note;
        DateTime sao_Date;
        int saoId = 0;
        string images;
        public string SAO_NAME { set { sao_Name = value; } get { return sao_Name; } }
        public string IMAGES { set { images = value; } get { return images; } }
        public string NOTE { set { note = value; } get { return note; } }
        public int SAOID { set { saoId = value; } get { return saoId; } }
        public DateTime SAO_DATE { set { sao_Date = value; } get { return sao_Date; } }

    }
}
