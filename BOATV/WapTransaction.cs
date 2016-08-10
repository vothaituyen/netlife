using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALATV;

namespace BOATV
{
    public class WapTransaction
    {
        public void WapTransactionInsert(string ReturnParam, string Phone, int ServiceType, int CategoryId, Int64 ArticleId, int Price)
        {
            using (MainDB db = new MainDB())
            {
                db.StoredProcedures.WapTransaction_Insert(ReturnParam, Phone, ServiceType, CategoryId, ArticleId, Price);
            }
        }
    }
}
