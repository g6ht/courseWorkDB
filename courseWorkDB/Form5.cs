using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Для SQL Server

namespace courseWorkDB
{
    public partial class Form5 : Form // freelancer
    {
        private static Freelancer currentUser;
        public Form5(bool newAccount, int id, int fId)
        {
            InitializeComponent();
            string connectionString = "Server=KATEPC\\SQLEXPRESS;Database=FreelancersEmployers;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string username, role, email, phone_number;
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Пользователи WHERE [Id пользователя] = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        username = reader.GetString(1);
                        role = reader.GetString(3);
                        email = reader.GetString(4);
                        phone_number = reader.GetString(5);
                    }
                }
    
                if (!newAccount)
                {
                    string skills = "", experience = "", info = "";
                    int rate = 0;
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Фрилансеры WHERE [Id фрилансера] = @id;", connection))
                    {
                        command.Parameters.AddWithValue("@id", fId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (!reader.IsDBNull(2)) { skills = reader.GetString(2); }
                            if (!reader.IsDBNull(3)) { experience = reader.GetString(3); }
                            if (!reader.IsDBNull(4)) { rate = reader.GetInt32(4); }
                            if (!reader.IsDBNull(5)) { info = reader.GetString(5); }
                        }
                    }
                    currentUser = new Freelancer(id, username, role, email, phone_number, fId, skills, experience, rate, info);
                }
                else { currentUser = new Freelancer(id, username, role, email, phone_number, fId); }
            }
            label1.Text += currentUser.Username;
            textBox1.Text = currentUser.Experience;
            if (!newAccount) textBox2.Text = currentUser.Rate.ToString();
            richTextBox1.Text = currentUser.Skills;
            richTextBox2.Text = currentUser.Info;
        }

        public static void ChangeInfo(string newSkills, string newExperience, int newRate, string newInfo)
        {
            currentUser.Skills = newSkills;
            currentUser.Experience = newExperience;
            currentUser.Rate = newRate;
            currentUser.Info = newInfo;
        }
        public static void ChangeAccount(string newUsername, string newEmail, string newPhoneNumber)
        {
            currentUser.Username = newUsername;
            currentUser.Email = newEmail;
            currentUser.PhoneNumber = newPhoneNumber;
        }

        public static void DeleteAccount()
        {
            currentUser = null;
        }

        private void UpdatePortfolio()
        {
            textBox1.Text = currentUser.Experience;
            textBox2.Text = currentUser.Rate.ToString();
            richTextBox1.Text = currentUser.Skills;
            richTextBox2.Text = currentUser.Info;
        }

        private void button1_Click(object sender, EventArgs e) // edit portfolio
        {
            Form6 form6 = new Form6(currentUser.FreelancerId);
            form6.ShowDialog();
            this.UpdatePortfolio();
        }

        private void button2_Click(object sender, EventArgs e) // manage account
        {
            Form7 form7 = new Form7(currentUser.getId());
            form7.ShowDialog();
            if (currentUser == null) { this.Close(); return; }
        }

        private void button3_Click(object sender, EventArgs e) // search for project
        {
            Form8 form8 = new Form8();
            form8.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) // manage bids
        {
            Form9 form9 = new Form9(currentUser.FreelancerId);
            form9.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e) // my contracts
        {
            Form10 form10 = new Form10(currentUser.FreelancerId);
            form10.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form11 form11 = new Form11(currentUser.FreelancerId);
            form11.ShowDialog();
        }
    }
}
