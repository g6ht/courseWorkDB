using System;
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form12 : Form // employer
    {
        private static Employer currentUser;
        public Form12()
        {
            InitializeComponent();
            currentUser = (Employer)Init.CurrentUser;

            label1.Text += currentUser.Username;
            textBox1.Text = currentUser.CompanyName;
            richTextBox1.Text = currentUser.Info;
        }
        public static void ChangeInfo(string newCompanyName, string newInfo)
        {
            currentUser.CompanyName = newCompanyName;
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
            textBox1.Text = currentUser.CompanyName;
            richTextBox1.Text = currentUser.Info;
        }
        private void button1_Click(object sender, EventArgs e) // edit portfolio
        {
            Form13 form13 = new Form13();
            form13.ShowDialog();
            UpdatePortfolio();
        }

        private void button2_Click(object sender, EventArgs e) // manage account
        {
            Form7 form7 = new Form7();
            form7.ShowDialog();
            if (currentUser == null) { this.Close(); return; }
        }

        private void button3_Click(object sender, EventArgs e) // search for bids
        {
            Form14 form14 = new Form14();
            form14.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) // manage projects
        {
            Form15 form15 = new Form15();
            form15.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e) // manage contracts
        {
            Form16 form16 = new Form16();
            form16.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e) // manage payments
        {
            Form17 form17 = new Form17();
            form17.ShowDialog();
        }

        private void Form12_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
