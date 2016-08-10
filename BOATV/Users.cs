using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ATVEntity;
using DALATV;

namespace BOATV
{
    public class Users
    {
        public void Insert(UserEntity userEntity)
        {
            using (MainDB db = new MainDB())
            {
                db.StoredProcedures.UsersInsert(userEntity);
            }
        }

        /// <summary>
        /// Lấy ra Usre
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public UserEntity GetOne(string email)
        {
            var tbl = new DataTable();
            using (var db = new MainDB())
            {
                tbl = db.StoredProcedures.UsersGetOne(email);
            }
            if (tbl.Rows.Count > 0)
                return MapDatarow(tbl.Rows[0]);
            return new UserEntity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="phone">Điệnthoại</param>
        /// <returns></returns>
        public List<UserEntity> GetOne(string email, string phone)
        {
            var lst = new List<UserEntity>();
            var tbl = new DataTable();
            using (var db = new MainDB())
            {
                tbl = db.StoredProcedures.UsersGetOne(email, phone);
            }
            if (tbl.Rows.Count > 0)
            {
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    lst.Add(MapDatarow(tbl.Rows[i]));
                }
            }
            return lst;
        }


        public UserEntity GetOneByPhone(string phone)
        {
            var tbl = new DataTable();
            using (var db = new MainDB())
            {
                tbl = db.StoredProcedures.GetOneByPhone(phone);
            }
            return tbl.Rows.Count > 0 ? MapDatarow(tbl.Rows[0]) : null;
        }

        public void Update(UserEntity userEntity)
        {
            using (var db = new MainDB())
            {
                db.StoredProcedures.UsersUpdate(userEntity);
            }
        }

        public DataTable GetAll(int pageSize, int pageIndex)
        {
            var tbl = new DataTable();
            using (var db = new MainDB())
            {
                tbl = db.StoredProcedures.UsersGetAll(pageSize, pageIndex);
            }

            return tbl ?? (tbl = new DataTable());
        }

        public UserEntity MapDatarow(DataRow row)
        {
            var entity = new UserEntity();
            entity.UserId = Convert.ToInt32(row["UserId"]);
            entity.Password = row["Password"].ToString();
            entity.Phone = row["Phone"].ToString();
            entity.Email = row["Email"].ToString();
            entity.FullName = row["FullName"].ToString();
            entity.Address = row["Address"].ToString();
            entity.ResetPass = (Guid)row["ResetPass"];
            entity.Created = row["Created"] != null ? Convert.ToDateTime(row["Created"]) : DateTime.Now;
            entity.PayDate = row["PayDate"] != null ? Convert.ToDateTime(row["PayDate"]) : DateTime.Now.AddDays(-1);
            return entity;
        }

        public UserEntity Payment(string Email, int Price)
        {
            DataTable tblResutl = new DataTable();   
            using (var db = new MainDB())
            {
               tblResutl = db.StoredProcedures.Payment(Email, Price, DateTime.Now);
            }
            return (tblResutl != null && tblResutl.Rows.Count > 0) ? MapDatarow(tblResutl.Rows[0]) : new UserEntity();
        }
    }
}
