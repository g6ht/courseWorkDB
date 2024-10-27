using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form13 : Form
    {
        private int employerId;
        public Form13()
        {
            employerId = Init.getFeId();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // save changes
        {
            string newCompanyName, newInfo;
            newCompanyName = textBox1.Text;
            newInfo = richTextBox2.Text;
            if (newCompanyName.Length > 100) { MessageBox.Show("Company name is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else if (newInfo.Length > 1000) { MessageBox.Show("Bio is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
                string query = "UPDATE Наниматели " + // запрос 23 (редактирование портфолио нанимателя)
                                "SET [Название компании] = @new_name, [Информация о компании]  = @new_info " +
                                "WHERE [Id нанимателя] = @employer_id;";

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("@new_name", newCompanyName);
                    command.Parameters.AddWithValue("@new_info", newInfo);
                    command.Parameters.AddWithValue("@employer_id", employerId);
                    command.ExecuteNonQuery();
                }

                Form12.ChangeInfo(newCompanyName, newInfo);
                this.Close();
            }
        }
    }
}
