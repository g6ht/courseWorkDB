using System;
using System.Data.SqlClient; // Для SQL Server
using System.Drawing;
using System.Windows.Forms;

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

                            query = "DECLARE @user_id INT; " + // запрос 5 (создание учётной записи фрилансера)
                                            "INSERT INTO Пользователи ([Имя пользователя], Пароль, Роль, [Электронная почта], [Номер телефона]) " +
                                            "VALUES (@username, @password, 'Фрилансер', @email, @phone_num); " +
                                            "SET @user_id = SCOPE_IDENTITY(); " +
                                            "INSERT INTO Фрилансеры ([Id пользователя]) VALUES (@user_id); " +
                                            "SELECT @user_id";

                            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                            {
                                command.Parameters.AddWithValue("@username", username);
                                command.Parameters.AddWithValue("@password", password);
                                command.Parameters.AddWithValue("@email", email);
                                command.Parameters.AddWithValue("@phone_num", phone_num);
                                int userId = (int)command.ExecuteScalar();

                                query = "SELECT [Id фрилансера] FROM Фрилансеры WHERE [Id пользователя] = @id;"; // запрос 2 (получение Id фрилансера)

                                using (SqlCommand command2 = new SqlCommand(query, ConnectionManager.GetConnection()))
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

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\0';
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
    }
}
