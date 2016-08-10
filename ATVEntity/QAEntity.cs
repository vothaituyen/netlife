using System;
using System.Collections.Generic;
using System.Text;

namespace ATVEntity
{
    public class QAEntity
    {

        public QAEntity()
        {
            this.CatId = 0;
            this.CreatedDate = DateTime.Now;
            this.Email = String.Empty;
            this.Name = String.Empty;
            this.Question = String.Empty;
            this.Status = 0;
            this._Answer = string.Empty;

        }
        private int CatId;
        private string Name;
        private string Email;
        DateTime CreatedDate;
        string Question;
        int Status;
        string _Answer;


        public int CATID { get { return CatId; } set { CatId = value; } }
        public string NAME { get { return Name.Length > 100 ? Name.Substring(0, 100) : Name; } set { Name = value; } }
        public string EMAIL { get { return Email.Length > 100 ? Email.Substring(0, 100) : Email; } set { Email = value; } }
        public DateTime CREATEDDATE { get { return CreatedDate; } set { CreatedDate = value; } }
        public string QUESTION { get { return Question; } set { Question = value; } }
        public int STATUS { get { return Status; } set { Status = value; } }
        public string Answer { get { return _Answer; } set { _Answer = value; } }

    }
}
