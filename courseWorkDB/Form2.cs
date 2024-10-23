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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // log in
        {
            string username = textBox1.Text;
            byte[] password = Form1.sha256_hash(textBox2.Text);
            
            string connectionString = "Server=KATEPC\\SQLEXPRESS;Database=FreelancersEmployers;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); // Открытие соединения
                using (SqlCommand command = new SqlCommand("SELECT * FROM Пользователи WHERE [Имя пользователя] = @username " +
                    "AND Пароль = @password", connection))
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
                                    using (SqlCommand command1 = new SqlCommand("SELECT [Id Фрилансера] FROM Фрилансеры WHERE [Id пользователя] = @id;", connection))
                                    {
                                        command1.Parameters.AddWithValue("@id", id);
                                        int fId = (int)command1.ExecuteScalar();
                                        Form5 form5 = new Form5(false, id, fId);
                                        form5.Show();
                                        this.Close();
                                    }
                                        
                                }
                                else if (reader[3].ToString() == "Наниматель")
                                {
                                    int id = reader.GetInt32(0);
                                    reader.Close();
                                    using(SqlCommand command1 = new SqlCommand("SELECT [Id нанимателя] FROM Наниматели WHERE [Id пользователя] = @id", connection))
                                    {
                                        command1.Parameters.AddWithValue("@id", id);
                                        int eId = (int)command1.ExecuteScalar();
                                        Form12 form12 = new Form12(false, id, eId);
                                        form12.Show();
                                        this.Close();
                                    }
                                    
                                }
                            }
                        }
                        else { 
                            MessageBox.Show("Wrong username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                
            }
        }
    }
}
