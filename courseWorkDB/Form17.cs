using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form17 : Form
    {
        private int employerId;
        private int paymentId;
        private bool newPayment = false;
        public Form17()
        {
            InitializeComponent();
            employerId = Init.getFeId();
            UpdateTable();
        }

        public void UpdateTable()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id", "Payment id");
            dataGridView1.Columns.Add("id2", "Contract id");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("sum", "Amount");
            dataGridView1.Columns.Add("date", "Date");
            dataGridView1.Columns.Add("status", "Status");

            string query = "SELECT Платежи.[Id платежа], Платежи.[Id контракта], Проекты.[Название проекта], " +
                            "Платежи.[Сумма платежа], Платежи.[Дата платежа],  Платежи.[Статус платежа] " +
                            "FROM Платежи JOIN Контракты ON Платежи.[Id контракта] = Контракты.[Id контракта] " +
                            "JOIN Проекты ON Контракты.[Id проекта] = Проекты.[Id проекта] " +
                            "WHERE Проекты.[Id нанимателя] = @employer_id;"; // запрос 35 (просмотр информации о платежах нанимателя)

            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("employer_id", employerId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader.GetInt32(0), reader.GetInt32(1),
                                reader.GetString(2), reader.GetInt32(3), reader.GetDateTime(4).ToShortDateString(), reader.GetString(5));
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) // new payment
        {
            label3.Visible = true;
            label2.Visible = true;
            label5.Visible = true;
            label4.Visible = true;
            textBox1.Visible = true;
            textBox1.Enabled = true;
            textBox2.Visible = true;
            dateTimePicker2.Visible = true;
            comboBox1.Visible = true;
            button5.Visible = true;
            newPayment = true;
        }

        private void button5_Click(object sender, EventArgs e) // save
        {
            if (int.TryParse(textBox1.Text, out int contractId) && int.TryParse(textBox2.Text, out int amount))
            {
                DateTime date = dateTimePicker2.Value;
                string status = comboBox1.Text;
                if (status == "") { MessageBox.Show("Payment status is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else
                {
                    string com;
                    if (newPayment)
                    {
                        com = "INSERT INTO Платежи ([Id контракта], [Сумма платежа], [Дата платежа], [Статус платежа]) " +
                            "VALUES (@contract_id, @sum, @date, @status);"; // запрос 36 (создание платежа)
                    }
                    else
                    {
                        com = "UPDATE Платежи SET [Сумма платежа] = @new_sum, [Дата платежа] = @new_date, [Статус платежа] = @new_status " +
                            "WHERE [Id платежа] = @payment_id;"; // запрос 37 (редактирование платежа)
                    }
                    if (newPayment)
                    {

                        string query = "SELECT * FROM Контракты WHERE [Id контракта] = @id"; // запрос 34 (проверка на существование контракта)

                        using (SqlCommand command1 = new SqlCommand(query, ConnectionManager.GetConnection()))
                        {
                            command1.Parameters.AddWithValue("id", contractId);
                            using (SqlDataReader reader = command1.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Close();
                                    using (SqlCommand command = new SqlCommand(com, ConnectionManager.GetConnection()))
                                    {
                                        command.Parameters.AddWithValue("contract_id", contractId);
                                        command.Parameters.AddWithValue("sum", amount);
                                        command.Parameters.AddWithValue("date", date);
                                        command.Parameters.AddWithValue("status", status);
                                        command.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Contract with this id doesn`t exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                        }

                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand(com, ConnectionManager.GetConnection()))
                        {

                            command.Parameters.AddWithValue("new_sum", amount);
                            command.Parameters.AddWithValue("new_date", date);
                            command.Parameters.AddWithValue("new_status", status);
                            command.Parameters.AddWithValue("payment_id", paymentId);
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
                    dateTimePicker2.Visible = false;
                    button5.Visible = false;
                    comboBox1.Visible = false;
                    newPayment = false;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    dateTimePicker2.Value = DateTime.Now;
                }
            }
            else
            {
                MessageBox.Show("Contract id and amount should be integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) // edit payment
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int payment_id))
            {
                string query = "SELECT * FROM Платежи WHERE [Id платежа] = @id"; // запрос 38 (проверка на существование платежа)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", payment_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            textBox1.Text = reader.GetInt32(1).ToString();
                            paymentId = payment_id;
                            label3.Visible = true;
                            label2.Visible = true;
                            label5.Visible = true;
                            label4.Visible = true;
                            textBox1.Visible = true;
                            textBox1.Enabled = false;
                            textBox2.Visible = true;
                            dateTimePicker2.Visible = true;
                            comboBox1.Visible = true;
                            button5.Visible = true;
                            newPayment = false;
                        }
                        else
                        {
                            MessageBox.Show("To edit payment, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To edit payment, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form17_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
