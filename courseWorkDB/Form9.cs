using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form9 : Form
    {
        private int freelancerId;
        private int bidId;
        private bool newBid = false;
        public Form9()
        {
            InitializeComponent();
            freelancerId = Init.getFeId();
            UpdateTable();
        }

        public void UpdateTable()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id", "Bid id");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("bid_text", "Bid text");
            dataGridView1.Columns.Add("bet", "Bet");
            dataGridView1.Columns.Add("status", "Status");

            string query = "SELECT Предложения.[Id предложения], Проекты.[Название проекта], " +
                "Предложения.[Текст предложения], Предложения.[Сумма ставки],  Предложения.[Статус предложения] " +
                "FROM Предложения JOIN Проекты ON Предложения.[Id проекта] = Проекты.[Id проекта] " +
                "WHERE Предложения.[Id фрилансера] = @freelancer_id;"; // запрос 14 (просмотр информации о предложениях фрилансера)

            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("freelancer_id", freelancerId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader.GetInt32(0), reader.GetString(1),
                                reader.GetString(2), reader.GetInt32(3), reader.GetString(4));
                        }
                    }
                }
            }

        }

        private void button3_Click(object sender, EventArgs e) // new bid
        {
            label3.Visible = true;
            label3.Text = "Project id: ";
            label2.Visible = true;
            label5.Visible = true;
            textBox1.Visible = true;
            textBox1.Enabled = true;
            textBox2.Visible = true;
            richTextBox1.Visible = true;
            button5.Visible = true;
            newBid = true;
        }

        private void button5_Click(object sender, EventArgs e) // save
        {
            if (int.TryParse(textBox1.Text, out int project_id) && int.TryParse(textBox2.Text, out int bet))
            {
                string text = richTextBox1.Text;
                if (text.Length > 1000) { MessageBox.Show("Text is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else
                {
                    string com;
                    if (newBid)
                    {
                        com = "INSERT INTO Предложения ([Id проекта], [Id фрилансера], [Текст предложения], [Сумма ставки]) " +
                            "VALUES (@project_id, @freelancer_id, @text, @sum);"; // запрос 15 (создание предложения)
                    }
                    else
                    {
                        com = "UPDATE Предложения SET [Текст предложения] = @text, [Сумма ставки] = @sum " +
                            "WHERE [Id предложения] = @bid_id;"; // запрос 16 (редактирование предложения)
                    }

                    if (newBid)
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
                                    using (SqlCommand command = new SqlCommand(com, ConnectionManager.GetConnection()))
                                    {
                                        command.Parameters.AddWithValue("project_id", project_id);
                                        command.Parameters.AddWithValue("freelancer_id", freelancerId);
                                        command.Parameters.AddWithValue("text", text);
                                        command.Parameters.AddWithValue("sum", bet);
                                        command.ExecuteNonQuery();
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
                            command.Parameters.AddWithValue("text", text);
                            command.Parameters.AddWithValue("sum", bet);
                            command.Parameters.AddWithValue("bid_id", bidId);
                            command.ExecuteNonQuery();
                        }
                    }

                    UpdateTable();

                    label3.Visible = false;
                    label2.Visible = false;
                    label5.Visible = false;
                    textBox1.Visible = false;
                    textBox1.Enabled = false;
                    textBox2.Visible = false;
                    richTextBox1.Visible = false;
                    button5.Visible = false;
                    newBid = false;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    richTextBox1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Project id and bet should be integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) // edit bid
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int bid_id))
            {
                string query = "SELECT * FROM Предложения WHERE [Id предложения] = @id"; // запрос 18 (проверка на существование предложения)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", bid_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            textBox1.Text = bid_id.ToString();
                            bidId = bid_id;
                            label3.Visible = true;
                            label3.Text = "Bid id: ";
                            label2.Visible = true;
                            label5.Visible = true;
                            textBox1.Visible = true;
                            textBox1.Enabled = false;
                            textBox2.Visible = true;
                            richTextBox1.Visible = true;
                            button5.Visible = true;
                            newBid = false;
                        }
                        else
                        {
                            MessageBox.Show("To edit or delete bid, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit or delete bid, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // delete bid
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int bid_id))
            {
                string query = "SELECT * FROM Предложения WHERE [Id предложения] = @id"; // запрос 18 (проверка на существование предложения)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", bid_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            bidId = bid_id;
                            DialogResult result = MessageBox.Show("Are you sure that you want to delete this bid?", "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                reader.Close();

                                query = "DELETE FROM Предложения WHERE [Id предложения] = @id"; // запрос 19 (удаление предложения)

                                using (SqlCommand command1 = new SqlCommand(query, ConnectionManager.GetConnection()))
                                {
                                    command1.Parameters.AddWithValue("id", bidId);
                                    command1.ExecuteNonQuery();

                                    label3.Visible = false;
                                    label2.Visible = false;
                                    label5.Visible = false;
                                    textBox1.Visible = false;
                                    textBox1.Enabled = false;
                                    textBox2.Visible = false;
                                    richTextBox1.Visible = false;
                                    button5.Visible = false;
                                    newBid = false;
                                    textBox1.Text = "";
                                    textBox2.Text = "";
                                    richTextBox1.Text = "";
                                    UpdateTable();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("To edit or delete bid, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit or delete bid, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form9_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
