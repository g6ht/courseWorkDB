using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form16 : Form
    {
        private int employerId;
        private int contractId;
        private bool newContract = false;
        public Form16()
        {
            InitializeComponent();
            employerId = Init.getFeId();
            UpdateTable();
        }
        public void UpdateTable()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id", "Contract id");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("freelancer", "Freelancer");
            dataGridView1.Columns.Add("start", "Start date");
            dataGridView1.Columns.Add("end", "End date");
            dataGridView1.Columns.Add("status", "Status");

            string query = "SELECT c.[Id контракта], p.[Название проекта], u.[Имя пользователя], c.[Дата начала], c.[Дата окончания], c.[Статус контракта] " +
                            "FROM Контракты c " +
                            "JOIN Проекты p ON c.[Id проекта] = p.[Id проекта] " +
                            "LEFT JOIN Фрилансеры f ON c.[Id фрилансера] = f.[Id фрилансера] " +
                            "LEFT JOIN Пользователи u ON f.[Id пользователя] = u.[Id пользователя] " +
                            "WHERE p.[Id нанимателя] = @employer_id;"; // запрос 29 (просмотр информации о контрактах нанимателя)

            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("employer_id", employerId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int cid = 0;
                            string pname = "";
                            string freelancer = "";
                            string start = "";
                            string end = "";
                            string status = reader.GetString(5);
                            if (!reader.IsDBNull(0)) { cid = reader.GetInt32(0); }
                            if (!reader.IsDBNull(1)) { pname = reader.GetString(1); }
                            if (!reader.IsDBNull(2)) { freelancer = reader.GetString(2); }
                            if (!reader.IsDBNull(3)) { start = reader.GetDateTime(3).ToShortDateString(); }
                            if (!reader.IsDBNull(4)) { end = reader.GetDateTime(4).ToShortDateString(); }
                            if (reader.IsDBNull(2)) { freelancer = "deleted account"; }
                            dataGridView1.Rows.Add(cid, pname, freelancer, start, end, status);
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) // new contract
        {
            label3.Visible = true;
            label2.Visible = true;
            label5.Visible = true;
            label4.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            dateTimePicker1.Visible = true;
            dateTimePicker2.Visible = true;
            button5.Visible = true;
            newContract = true;
        }

        private void button5_Click(object sender, EventArgs e) // save
        {
            if (int.TryParse(textBox1.Text, out int project_id) && int.TryParse(textBox2.Text, out int freelancer_id))
            {
                DateTime start = dateTimePicker2.Value;
                DateTime end = dateTimePicker1.Value;

                string com;
                if (newContract)
                {
                    // запрос 30 (создание контракта)
                    com = "INSERT INTO Контракты ([Id проекта], [Id фрилансера], [Дата начала], [Дата окончания]) VALUES (@project_id, @freelancer_id, @start, @end);";
                }
                else
                {
                    // запрос 31 (редактирование контракта)
                    com = "UPDATE Контракты SET [Дата начала] = @new_start, [Дата окончания] = @new_end WHERE [Id контракта] = @contract_id;";
                }

                if (newContract)
                {
                    string query = "SELECT * FROM Проекты WHERE [Id проекта] = @id"; // запрос 17 (проверка на существование проекта)

                    using (SqlCommand command1 = new SqlCommand(query, ConnectionManager.GetConnection()))
                    {
                        command1.Parameters.AddWithValue("id", project_id);
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Close();

                                query = "SELECT * FROM Фрилансеры WHERE [Id фрилансера] = @fid"; // запрос 32 (проверка на существование фрилансера)

                                using (SqlCommand command2 = new SqlCommand(query, ConnectionManager.GetConnection()))
                                {
                                    command2.Parameters.AddWithValue("fid", freelancer_id);
                                    using (SqlDataReader reader1 = command2.ExecuteReader())
                                    {
                                        if (reader1.HasRows)
                                        {
                                            reader1.Close();
                                            using (SqlCommand command = new SqlCommand(com, ConnectionManager.GetConnection()))
                                            {
                                                command.Parameters.AddWithValue("project_id", project_id);
                                                command.Parameters.AddWithValue("freelancer_id", freelancer_id);
                                                command.Parameters.AddWithValue("start", start);
                                                command.Parameters.AddWithValue("end", end);
                                                command.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Freelancer with this id doesn`t exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Project with this id doesn`t exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                        }
                    }
                }
                else
                {
                    using (SqlCommand command = new SqlCommand(com, ConnectionManager.GetConnection()))
                    {
                        command.Parameters.AddWithValue("new_start", start);
                        command.Parameters.AddWithValue("new_end", end);
                        command.Parameters.AddWithValue("contract_id", contractId);
                        command.ExecuteNonQuery();
                    }
                }

                UpdateTable();

                label3.Visible = false;
                label2.Visible = false;
                label5.Visible = false;
                label4.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;
                button5.Visible = false;
                newContract = false;
                textBox1.Text = "";
                textBox2.Text = "";
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
            }

            else
            {
                MessageBox.Show("Project id and bet should be integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) // edit contract
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int contract_id))
            {

                string query = "SELECT * FROM Контракты WHERE [Id контракта] = @id"; // запрос 33 (проверка на существование контракта)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", contract_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            textBox1.Text = reader.GetInt32(1).ToString();
                            textBox2.Text = reader.GetInt32(2).ToString();
                            contractId = contract_id;
                            label3.Visible = true;
                            label2.Visible = true;
                            label5.Visible = true;
                            label4.Visible = true;
                            textBox1.Visible = true;
                            textBox1.Enabled = false;
                            textBox2.Enabled = false;
                            textBox2.Visible = true;
                            dateTimePicker1.Visible = true;
                            dateTimePicker2.Visible = true;
                            button5.Visible = true;
                            newContract = false;
                        }
                        else
                        {
                            MessageBox.Show("To edit or cancel contract, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit or cancel contract, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // cancel contract
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int contract_id))
            {
                string query = "SELECT * FROM Контракты WHERE [Id контракта] = @id"; // запрос 33 (проверка на существование контракта)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", contract_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            contractId = contract_id;
                            DialogResult result = MessageBox.Show("Are you sure that you want to cancel this contract?", "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                reader.Close();

                                query = "DELETE FROM Контракты WHERE [Id контракта] = @id"; // запрос 34 (удаление (отмена) контракта)

                                using (SqlCommand command1 = new SqlCommand(query, ConnectionManager.GetConnection()))
                                {
                                    command1.Parameters.AddWithValue("id", contractId);
                                    command1.ExecuteNonQuery();
                                    UpdateTable();

                                    label3.Visible = false;
                                    label2.Visible = false;
                                    label5.Visible = false;
                                    label4.Visible = false;
                                    textBox1.Visible = false;
                                    textBox2.Visible = false;
                                    dateTimePicker1.Visible = false;
                                    dateTimePicker2.Visible = false;
                                    button5.Visible = false;
                                    newContract = false;
                                    textBox1.Text = "";
                                    textBox2.Text = "";
                                    dateTimePicker1.Value = DateTime.Now;
                                    dateTimePicker2.Value = DateTime.Now;
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("To edit or cancel contract, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit or cancel contract, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form16_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
