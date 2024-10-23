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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // create employer
        {
            string username = textBox1.Text;
            byte[] password = Form1.sha256_hash(textBox2.Text);
            string email = textBox3.Text;
            string phone_num = textBox4.Text;

            string connectionString = "Server=KATEPC\\SQLEXPRESS;Database=FreelancersEmployers;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); // Открытие соединения
                using (SqlCommand command1 = new SqlCommand("SELECT * FROM Пользователи WHERE [Электронная почта] = @email " +
                    "OR [Номер телефона] = @phone_num " +
                    "OR [Имя пользователя] = @username;", connection))
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
                                "VALUES (@username, @password, 'Наниматель', @email, @phone_num); " +
                                "SET @user_id = SCOPE_IDENTITY(); " + "" +
                                "INSERT INTO Наниматели ([Id пользователя]) VALUES (@user_id); " +
                                "SELECT @user_id", connection))
                            {
                                command.Parameters.AddWithValue("@username", username);
                                command.Parameters.AddWithValue("@password", password);
                                command.Parameters.AddWithValue("@email", email);
                                command.Parameters.AddWithValue("@phone_num", phone_num);
                                int userId = (int)command.ExecuteScalar();
                                using (SqlCommand command2 = new SqlCommand("SELECT [Id нанимателя] FROM Наниматели WHERE [Id пользователя] = @id;", connection))
                                {
                                    command2.Parameters.AddWithValue("@id", userId);
                                    int eId = (int)command2.ExecuteScalar();
                                    Form12 form12 = new Form12(true, userId, eId);
                                    form12.Show();
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
