﻿using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // create employer
        {
            string username = textBox1.Text;
            byte[] password = Init.sha256_hash(textBox2.Text);
            string email = textBox3.Text;
            string phone_num = textBox4.Text;
            if (username.Length > 20) { MessageBox.Show("Username is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else if (email.Length > 30) { MessageBox.Show("Email is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else if (phone_num.Length > 20) { MessageBox.Show("Phone number is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
                string query = "SELECT * FROM Пользователи WHERE [Электронная почта] = @email " + // запрос 4 (проверка уникальности введённых данных)
                                "OR [Номер телефона] = @phone_num " +
                                "OR [Имя пользователя] = @username;";

                using (SqlCommand command1 = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command1.Parameters.AddWithValue("email", email);
                    command1.Parameters.AddWithValue("phone_num", phone_num);
                    command1.Parameters.AddWithValue("username", username);
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Account with this username, email or phone number already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            reader.Close();

                                query = "DECLARE @user_id INT; " + // запрос 6 (создание учётной записи нанимателя)
                                            "INSERT INTO Пользователи ([Имя пользователя], Пароль, Роль, [Электронная почта], [Номер телефона]) " +
                                            "VALUES (@username, @password, 'Наниматель', @email, @phone_num); " +
                                            "SET @user_id = SCOPE_IDENTITY(); " + "" +
                                            "INSERT INTO Наниматели ([Id пользователя]) VALUES (@user_id); " +
                                            "SELECT @user_id";

                            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                            {
                                command.Parameters.AddWithValue("@username", username);
                                command.Parameters.AddWithValue("@password", password);
                                command.Parameters.AddWithValue("@email", email);
                                command.Parameters.AddWithValue("@phone_num", phone_num);
                                int userId = (int)command.ExecuteScalar();

                                query = "SELECT [Id нанимателя] FROM Наниматели WHERE [Id пользователя] = @id;"; // запрос 3 (получение Id нанимателя)

                                using (SqlCommand command2 = new SqlCommand(query, ConnectionManager.GetConnection()))
                                {
                                    command2.Parameters.AddWithValue("@id", userId);
                                    int eId = (int)command2.ExecuteScalar();
                                    Init.InitUser(Role.Employer, true, userId, eId);
                                    Form12 form12 = new Form12();
                                    form12.Show();
                                    this.Close();
                                }
                            }

                        }

                    }
                }

            }
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\0';
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void Form4_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
