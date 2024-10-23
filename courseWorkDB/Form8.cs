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
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id", "Project id");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("description", "Description");
            dataGridView1.Columns.Add("budget", "Budget");
            dataGridView1.Columns.Add("deadline", "Deadline");
            dataGridView1.Columns.Add("status", "Status");
            string connectionString = "Server=KATEPC\\SQLEXPRESS;Database=FreelancersEmployers;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Проекты", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    { 
                    if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                dataGridView1.Rows.Add(reader.GetInt32(0), reader.GetString(2),
                                    reader.GetString(3), reader.GetInt32(4), reader.GetDateTime(5).ToShortDateString(), reader.GetString(6));
                            }
                        }
                    }
                }
            }
        }
    }
}
