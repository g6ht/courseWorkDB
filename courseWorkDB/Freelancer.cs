using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWorkDB
{
    public class Freelancer : User
    {
        private int freelancerId;
        private string skills;
        private string experience;
        private int rate;
        private string info;
        public Freelancer(int id, string username, string role, string email, string phone_number, int freelancerId) : base(id, username, role, email, phone_number)
        {
            this.freelancerId = freelancerId;
            this.skills = "";
            this.experience = "";
            this.rate = 0;
            this.info = "";
        }

        public Freelancer(int id, string username, string role, string email, string phone_number, int freelancerId, string skills, string experience, int rate, string info) : base(id, username, role, email, phone_number)
        {
            this.freelancerId = freelancerId;
            this.skills = skills;
            this.experience = experience;
            this.rate = rate;
            this.info = info;
        }
        public int FreelancerId { get { return this.freelancerId; } set { this.freelancerId = value; } }
        public string Skills { get { return this.skills; } set { this.skills = value; } }
        public string Experience { get { return this.experience; } set { this.experience = value; } }
        public int Rate { get { return this.rate; } set { this.rate = value; } }
        public string Info { get { return this.info; } set { this.info = value; } }
    }
}
