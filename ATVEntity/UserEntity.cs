using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class UserEntity
    {
        public UserEntity()
        {
            this.Email = string.Empty;
            this.Phone = string.Empty;
            this.Phone = string.Empty;
            this.UserId = 0;
            this.ResetPass = new Guid();
            this.FullName = string.Empty;
            this.IsActive = false;
            this.Address = String.Empty;
            this.Created = DateTime.Now;
            this.PayDate = DateTime.Now;
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int UserId { get; set; }
        public Guid ResetPass { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime PayDate { get; set; }
        public string Address { get; set; }
    }
}
