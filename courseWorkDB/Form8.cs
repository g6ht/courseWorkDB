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
            dataGridView1.Columns.Add("employer", "Employer");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("description", "Description");
            dataGridView1.Columns.Add("budget", "Budget");
            dataGridView1.Columns.Add("deadline", "Deadline");
            dataGridView1.Columns.Add("status", "Status");

            using (SqlCommand command = new SqlCommand("SELECT Проекты.*, Наниматели.[Название компании] " +
                "FROM Проекты JOIN Наниматели ON Проекты.[Id нанимателя] = Наниматели.[Id нанимателя];", ConnectionManager.GetConnection()))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    { 
                    if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                dataGridView1.Rows.Add(reader.GetInt32(0), reader.GetString(7), reader.GetString(2),
                                    reader.GetString(3), reader.GetInt32(4), reader.GetDateTime(5).ToShortDateString(), reader.GetString(6));
                            }
                        }
                    }
                }
            
        }
    }
}
