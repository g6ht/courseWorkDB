using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form6 : Form // freelancer change portfolio
    {
        private int freelancerId;
        public Form6()
        {
            freelancerId = Init.getFeId();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // save changes
        {
            string newExperince, newSkills, newInfo;
            int newRate;
            if (int.TryParse(textBox2.Text, out newRate))
            {
                newExperince = textBox1.Text;
                newSkills = richTextBox1.Text;
                newInfo = richTextBox2.Text;
                if (newExperince.Length > 10) { MessageBox.Show("Work experience is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else if (newSkills.Length > 1000) { MessageBox.Show("Skills is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else if (newInfo.Length > 1000) { MessageBox.Show("Bio is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else
                {
                    using (SqlCommand command = new SqlCommand("UPDATE Фрилансеры " +
                        "SET Навыки = @newSkills, [Опыт работы] = @newExperince, [Часовая ставка] = @newRate, [Информация о себе] = @newInfo " +
                        "WHERE [Id фрилансера] = @freelancerId;", ConnectionManager.GetConnection()))
                    {
                        command.Parameters.AddWithValue("@newSkills", newSkills);
                        command.Parameters.AddWithValue("@newExperince", newExperince);
                        command.Parameters.AddWithValue("@newRate", newRate);
                        command.Parameters.AddWithValue("@newInfo", newInfo);
                        command.Parameters.AddWithValue("@freelancerId", freelancerId);
                        command.ExecuteNonQuery();
                    }

                    Form5.ChangeInfo(newSkills, newExperince, newRate, newInfo);
                    this.Close();
                }
            }
            else {
                MessageBox.Show("Hourly rate should be integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
