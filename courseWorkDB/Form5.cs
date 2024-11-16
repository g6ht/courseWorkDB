using System;
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form5 : Form // freelancer
    {
        private static Freelancer currentUser;
        public Form5()
        {
            InitializeComponent();
            currentUser = (Freelancer)Init.CurrentUser;

            label1.Text += currentUser.Username;
            textBox1.Text = currentUser.Experience;
            textBox2.Text = currentUser.Rate.ToString();
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
            Init.CurrentUser = null;
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
            Form6 form6 = new Form6();
            form6.ShowDialog();
            UpdatePortfolio();
        }

        private void button2_Click(object sender, EventArgs e) // manage account
        {
            Form7 form7 = new Form7();
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
            Form9 form9 = new Form9();
            form9.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e) // my contracts
        {
            Form10 form10 = new Form10();
            form10.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e) // my payments
        {
            Form11 form11 = new Form11();
            form11.ShowDialog();
        }

        private void Form5_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
