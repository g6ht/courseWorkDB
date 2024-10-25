using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            using (SqlCommand command = new SqlCommand("SELECT c.[Id контракта], p.[Название проекта], u.[Имя пользователя], c.[Дата начала], c.[Дата окончания], c.[Статус контракта] " +
                "FROM Контракты c " +
                "JOIN Проекты p ON c.[Id проекта] = p.[Id проекта] " +
                "JOIN Фрилансеры f ON c.[Id фрилансера] = f.[Id фрилансера] " +
                "JOIN Пользователи u ON f.[Id пользователя] = u.[Id пользователя] " +
                "WHERE p.[Id нанимателя] = @employer_id;", ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("employer_id", employerId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader.GetInt32(0), reader.GetString(1),
                                reader.GetString(2), reader.GetDateTime(3).ToShortDateString(), reader.GetDateTime(4).ToShortDateString(), reader.GetString(5));
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
                    com = "INSERT INTO Контракты ([Id проекта], [Id фрилансера], [Дата начала], [Дата окончания]) VALUES (@project_id, @freelancer_id, @start, @end);";
                }
                else
                {
                    com = "UPDATE Контракты SET [Дата начала] = @new_start, [Дата окончания] = @new_end WHERE [Id контракта] = @contract_id;";
                }

                if (newContract)
                {
                    using (SqlCommand command1 = new SqlCommand("SELECT * FROM Проекты WHERE [Id проекта] = @id", ConnectionManager.GetConnection()))
                    {
                        command1.Parameters.AddWithValue("id", project_id);
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Close();
                                using (SqlCommand command2 = new SqlCommand("SELECT * FROM Фрилансеры WHERE [Id фрилансера] = @fid", ConnectionManager.GetConnection()))
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

                using (SqlCommand command = new SqlCommand("SELECT * FROM Контракты WHERE [Id контракта] = @id", ConnectionManager.GetConnection()))
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
                            MessageBox.Show("To edit or delete contract, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit or delete contract, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // delete contract
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int contract_id))
            {

                using (SqlCommand command = new SqlCommand("SELECT * FROM Контракты WHERE [Id контракта] = @id", ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", contract_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            
                            contractId = contract_id;
                            DialogResult result = MessageBox.Show("Are you sure that you want to delete this contract?", "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                reader.Close();
                                using (SqlCommand command1 = new SqlCommand("DELETE FROM Контракты WHERE [Id контракта] = @id", ConnectionManager.GetConnection()))
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
                            MessageBox.Show("To edit or delete contract, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit or delete contract, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
