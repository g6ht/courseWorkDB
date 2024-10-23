using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWorkDB
{
    public class User
    {
        private int id;
        private string username;
        private string role;
        private string email;
        private string phone_number;

        public User(int id, string username, string role, string email, string phone_number)
        {
            this.id = id;
            this.username = username;
            this.role = role;
            this.email = email;
            this.phone_number = phone_number;
        }
        public int getId() { return id; }
        public string Username { get { return this.username; } set { this.username = value; } }
        public string getRole() { return role; }
        public string Email { get { return this.email; } set { this.email = value; } }
        public string PhoneNumber { get { return this.phone_number; } set { this.phone_number = value; } }

    }
}
