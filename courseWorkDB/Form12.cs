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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace courseWorkDB
{
    public partial class Form12 : Form // employer
    {
        private Employer currentUser;
        public Form12(bool newAccount, int id, int eId)
        {
            InitializeComponent();
            string connectionString = "Server=KATEPC\\SQLEXPRESS;Database=FreelancersEmployers;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string username, role, email, phone_number;
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Пользователи WHERE [Id пользователя] = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        username = reader.GetString(1);
                        role = reader.GetString(3);
                        email = reader.GetString(4);
                        phone_number = reader.GetString(5);
                    }
                }

                if (!newAccount)
                {
                    string companyName = "", info = "";
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Наниматели WHERE [Id нанимателя] = @id;", connection))
                    {
                        command.Parameters.AddWithValue("@id", eId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (!reader.IsDBNull(2)) { companyName = reader.GetString(2); }
                            if (!reader.IsDBNull(3)) { info = reader.GetString(3); }
                        }
                    }
                    currentUser = new Employer(id, username, role, email, phone_number, eId, companyName, info);
                }
                else { currentUser = new Employer(id, username, role, email, phone_number, eId); }
            }
            label1.Text += currentUser.Username;
            textBox1.Text = currentUser.CompanyName;
            richTextBox1.Text = currentUser.Info;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
