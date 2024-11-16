using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form15 : Form
    {
        private int employerId;
        private int projectId;
        private bool newProject = false;
        public Form15()
        {
            InitializeComponent();
            employerId = Init.getFeId();
            UpdateTable();
        }
        public void UpdateTable()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id", "Project id");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("description", "Description");
            dataGridView1.Columns.Add("budget", "Budget");
            dataGridView1.Columns.Add("deadline", "Deadline");
            dataGridView1.Columns.Add("status", "Status");

            string query = "SELECT [Id проекта], [Название проекта], [Описание проекта], Бюджет, [Срок выполнения], [Статус проекта] " +
                            "FROM Проекты WHERE [Id нанимателя] = @employer_id;"; // запрос 25 (просмотр информации о проектах нанимателя)

            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("employer_id", employerId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader.GetInt32(0), reader.GetString(1),
                                reader.GetString(2), reader.GetInt32(3), reader.GetDateTime(4).ToShortDateString(), reader.GetString(5));
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) // new project
        {
            label3.Visible = true;
            label3.Text = "Project name: ";
            label2.Visible = true;
            label5.Visible = true;
            label4.Visible = true;
            textBox1.Visible = true;
            textBox1.Enabled = true;
            textBox2.Visible = true;
            richTextBox1.Visible = true;
            dateTimePicker1.Visible = true;
            button5.Visible = true;
            newProject = true;
        }

        private void button5_Click(object sender, EventArgs e) // save
        {
            if (int.TryParse(textBox2.Text, out int budget))
            {
                string name = textBox1.Text;
                string info = richTextBox1.Text;
                DateTime deadline = dateTimePicker1.Value;
                if (name.Length > 100) { MessageBox.Show("Project name is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else if (info.Length > 1000) { MessageBox.Show("Project info is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else if (name == "") { MessageBox.Show("Project name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else
                {
                    string com;
                    if (newProject)
                    {
                        com = "INSERT INTO Проекты ([Id нанимателя], [Название проекта], [Описание проекта], Бюджет, [Срок выполнения]) " +
                            "VALUES (@employer_id, @name, @info, @budget, @date);"; // запрос 26 (создание проекта)
                    }
                    else
                    {
                        com = "UPDATE Проекты SET [Описание проекта] = @new_info, Бюджет = @new_budget, [Срок выполнения] = @new_date " +
                            "WHERE [Id проекта] = @project_id;"; // запрос 27 (редактирование проекта)
                    }
                    if (newProject)
                    {

                        using (SqlCommand command = new SqlCommand(com, ConnectionManager.GetConnection()))
                        {
                            command.Parameters.AddWithValue("employer_id", employerId);
                            command.Parameters.AddWithValue("name", name);
                            command.Parameters.AddWithValue("info", info);
                            command.Parameters.AddWithValue("budget", budget);
                            command.Parameters.AddWithValue("date", deadline);
                            command.ExecuteNonQuery();
                        }

                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand(com, ConnectionManager.GetConnection()))
                        {

                            command.Parameters.AddWithValue("new_info", info);
                            command.Parameters.AddWithValue("new_budget", budget);
                            command.Parameters.AddWithValue("new_date", deadline);
                            command.Parameters.AddWithValue("project_id", projectId);
                            command.ExecuteNonQuery();
                        }
                    }
                    UpdateTable();
                    label3.Visible = false;
                    label2.Visible = false;
                    label5.Visible = false;
                    label4.Visible = false;
                    textBox1.Visible = false;
                    textBox1.Enabled = false;
                    textBox2.Visible = false;
                    richTextBox1.Visible = false;
                    dateTimePicker1.Visible = false;
                    button5.Visible = false;
                    newProject = false;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    richTextBox1.Text = "";
                    dateTimePicker1.Value = DateTime.Now;
                }
            }
            else
            {
                MessageBox.Show("Budget should be integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) // edit project
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int project_id))
            {

                string query = "SELECT * FROM Проекты WHERE [Id проекта] = @id"; // запрос 17 (проверка на существование проекта)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", project_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            textBox1.Text = project_id.ToString();
                            projectId = project_id;
                            label3.Text = "Project id: ";
                            label3.Visible = true;
                            label2.Visible = true;
                            label5.Visible = true;
                            label4.Visible = true;
                            textBox1.Visible = true;
                            textBox1.Enabled = false;
                            textBox2.Visible = true;
                            richTextBox1.Visible = true;
                            dateTimePicker1.Visible = true;
                            button5.Visible = true;
                            newProject = false;
                        }
                        else
                        {
                            MessageBox.Show("To edit, cancel or end project, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit, cancel or end project, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // cancel project
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int project_id))
            {
                string query = "SELECT * FROM Проекты WHERE [Id проекта] = @id"; // запрос 17 (проверка на существование проекта)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", project_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            projectId = project_id;
                            DialogResult result = MessageBox.Show("Are you sure that you want to cancel this project?", "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                reader.Close();

                                query = "DELETE FROM Проекты WHERE [Id проекта] = @id"; // запрос 28 (удаление (отмена) проекта)

                                using (SqlCommand command1 = new SqlCommand(query, ConnectionManager.GetConnection()))
                                {
                                    command1.Parameters.AddWithValue("id", projectId);
                                    command1.ExecuteNonQuery();
                                    label3.Visible = false;
                                    label2.Visible = false;
                                    label5.Visible = false;
                                    label4.Visible = false;
                                    textBox1.Visible = false;
                                    textBox1.Enabled = false;
                                    textBox2.Visible = false;
                                    richTextBox1.Visible = false;
                                    dateTimePicker1.Visible = false;
                                    button5.Visible = false;
                                    newProject = false;
                                    textBox1.Text = "";
                                    textBox2.Text = "";
                                    richTextBox1.Text = "";
                                    dateTimePicker1.Value = DateTime.Now;
                                    UpdateTable();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("To edit, cancel or end project, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit, cancel or end project, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e) // end project
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int project_id))
            {
                string query = "SELECT * FROM Проекты WHERE [Id проекта] = @id"; // запрос 17 (проверка на существование проекта)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", project_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            projectId = project_id;
                            DialogResult result = MessageBox.Show("Are you sure that you want to end this project?", "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                reader.Close();

                                using (SqlCommand command1 = new SqlCommand("CompleteProjectAndContracts", ConnectionManager.GetConnection()))
                                {
                                    command1.CommandType = CommandType.StoredProcedure;
                                    SqlParameter idParam = new SqlParameter { ParameterName = "@projectId", Value = projectId };
                                    command1.Parameters.Add(idParam);
                                    command1.ExecuteNonQuery();
                                    UpdateTable();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("To edit, cancel or end project, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit, cancel or end project, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form15_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
