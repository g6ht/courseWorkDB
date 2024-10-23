using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWorkDB
{
    public class Employer : User
    {
        private int employerId;
        private string companyName;
        private string info;
        public Employer(int id, string username, string role, string email, string phone_number, int employerId) : base(id, username, role, email, phone_number)
        {
            this.employerId = employerId;
            this.companyName = "";
            this.info = "";
        }

        public Employer(int id, string username, string role, string email, string phone_number, int employerId, string companyName, string info) : base(id, username, role, email, phone_number)
        {
            this.employerId = employerId;
            this.companyName = companyName;
            this.info = info;
        }
        public int EmployerId { get { return this.employerId; } set { this.employerId = value; } }
        public string CompanyName { get { return this.companyName; } set { this.companyName = value; } }
        public string Info { get { return this.info; } set { this.info = value; } }
    }
}
