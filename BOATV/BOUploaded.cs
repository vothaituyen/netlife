using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATVEntity;
using DALATV;

namespace BOATV
{
    public class BOUploaded
    {
        public static void Upload_Insert(UploadEntity ue)
        {
            using (MainDB db = new MainDB(BOAdv.MasterConnectionString))
            {
                db.StoredProcedures.Upload_Insert(ue);
            }
        }
    }
}