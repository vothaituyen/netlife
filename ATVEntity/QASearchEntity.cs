using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class QASearchEntity
    {
        public QASearchEntity()
        {
            fdate = DateTime.Now.AddYears(-10);
            edate = DateTime.Now;
            keySearch = "";
            type = 0;
            category = 1005;
            order = 1;
            pageIndex = 1;
            pageSize = 10;
        }
        public QASearchEntity(DateTime fdate, DateTime edate, string keySearch, int type, int category, int order, int pageIndex, int pageSize)
        {
            this.fdate = fdate;
            this.edate = edate;
            this.keySearch = keySearch;
            this.type = type;
            this.category = category;
            this.order = order;
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
        }
        private DateTime fdate;
        private DateTime edate;
        private string keySearch;
        private int type;
        private int category;
        private int rows = 20;
        private int order;
        private int pageIndex;
        private int pageSize;

        public DateTime FDate
        {
            get
            {
                if (fdate.Year < 2000)
                    return new DateTime(2000, 01, 01);
                else return fdate;
            }
            set { fdate = value; }
        }
        public DateTime EDate { get 
        {
            if (edate > DateTime.Now)
                return DateTime.Now;
            else return edate;
        }
        set { edate = value; }
    }
        public string KeySearch { get { return keySearch; } set { keySearch = value; } }
        public int Type { get { return type; } set { type = value; } }
        public int Cat { get { return category; } set { category = value; } }
        public int Rows { get { return rows; } set { rows = value; } }
        public int Order { get { return order; } set { order = value; } }
        public int PageIndex { get { return pageIndex; } set { pageIndex = value; } }
        public int PageSize { get { return pageSize; } set { pageSize = value; } }

    }
}
