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
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
            int fId = Init.getFeId();

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id", "Contract id");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("start", "Start date");
            dataGridView1.Columns.Add("end", "End date");
            dataGridView1.Columns.Add("status", "Status");

                using (SqlCommand command = new SqlCommand("SELECT Контракты.[Id контракта], Проекты.[Название проекта], " +
                    "Контракты.[Дата начала], Контракты.[Дата окончания],  Контракты.[Статус контракта] " +
                    "FROM Контракты JOIN Проекты ON Контракты.[Id проекта] = Проекты.[Id проекта] " +
                    "WHERE Контракты.[Id фрилансера] = @freelancer_id;", ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("freelancer_id", fId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                dataGridView1.Rows.Add(reader.GetInt32(0), reader.GetString(1),
                                    reader.GetDateTime(2).ToShortDateString(), reader.GetDateTime(3).ToShortDateString(), reader.GetString(4));
                            }
                        }
                    }
                }
            
        }
    }
}
