using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Для SQL Server

namespace courseWorkDB
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // create freelancer
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
                using (SqlCommand command1 = new SqlCommand("SELECT * FROM Пользователи WHERE [Электронная почта] = @email " +
                    "OR [Номер телефона] = @phone_num " +
                    "OR [Имя пользователя] = @username;", ConnectionManager.GetConnection()))
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
                            using (SqlCommand command = new SqlCommand("DECLARE @user_id INT; " +
                                "INSERT INTO Пользователи ([Имя пользователя], Пароль, Роль, [Электронная почта], [Номер телефона]) " +
                                "VALUES (@username, @password, 'Фрилансер', @email, @phone_num); " +
                                "SET @user_id = SCOPE_IDENTITY(); " +
                                "INSERT INTO Фрилансеры ([Id пользователя]) VALUES (@user_id); " +
                                "SELECT @user_id", ConnectionManager.GetConnection()))
                            {
                                command.Parameters.AddWithValue("@username", username);
                                command.Parameters.AddWithValue("@password", password);
                                command.Parameters.AddWithValue("@email", email);
                                command.Parameters.AddWithValue("@phone_num", phone_num);
                                int userId = (int)command.ExecuteScalar();
                                using (SqlCommand command2 = new SqlCommand("SELECT [Id фрилансера] FROM Фрилансеры WHERE [Id пользователя] = @id;", ConnectionManager.GetConnection()))
                                {
                                    command2.Parameters.AddWithValue("@id", userId);
                                    int fId = (int)command2.ExecuteScalar();
                                    Init.InitUser(Role.Freelancer, true, userId, fId);
                                    Form5 form5 = new Form5();
                                    form5.Show();
                                    this.Close();
                                }

                            }

                        }
                    }
                }

            }
            
        }
    }
}
