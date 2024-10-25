using System;
using System.Data.SqlClient; // Для SQL Server
using System.Windows.Forms;

namespace courseWorkDB
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // log in
        {
            string username = textBox1.Text;
            byte[] password = Init.sha256_hash(textBox2.Text);

            using (SqlCommand command = new SqlCommand("SELECT * FROM Пользователи WHERE [Имя пользователя] = @username " +
                    "AND Пароль = @password", ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            if (reader[3].ToString() == "Фрилансер")
                            {
                                int id = reader.GetInt32(0);
                                reader.Close();
                                using (SqlCommand command1 = new SqlCommand("SELECT [Id Фрилансера] FROM Фрилансеры WHERE [Id пользователя] = @id;", ConnectionManager.GetConnection()))
                                {
                                    command1.Parameters.AddWithValue("@id", id);
                                    int fId = (int)command1.ExecuteScalar();
                                    Init.InitUser(Role.Freelancer, false, id, fId);
                                    Form5 form5 = new Form5();
                                    form5.Show();
                                    this.Close();
                                }

                            }
                            else if (reader[3].ToString() == "Наниматель")
                            {
                                int id = reader.GetInt32(0);
                                reader.Close();
                                using (SqlCommand command1 = new SqlCommand("SELECT [Id нанимателя] FROM Наниматели WHERE [Id пользователя] = @id", ConnectionManager.GetConnection()))
                                {
                                    command1.Parameters.AddWithValue("@id", id);
                                    int eId = (int)command1.ExecuteScalar();
                                    Init.InitUser(Role.Employer, false, id, eId);
                                    Form12 form12 = new Form12();
                                    form12.Show();
                                    this.Close();
                                }

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wrong username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\0';
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
    }
}
