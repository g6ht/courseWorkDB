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
    public partial class Form11 : Form
    {
        public Form11()
        {
            InitializeComponent();
            int fId = Init.getFeId();

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id1", "Payment id");
            dataGridView1.Columns.Add("id2", "Contract id");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("sum", "Payment amount");
            dataGridView1.Columns.Add("date", "Payment date");
            dataGridView1.Columns.Add("status", "Status");

                using (SqlCommand command = new SqlCommand("SELECT Платежи.[Id платежа], Платежи.[Id контракта],  Проекты.[Название проекта], " +
                    "Платежи.[Сумма платежа], Платежи.[Дата платежа],  Платежи.[Статус платежа] " + 
                    "FROM Платежи JOIN Контракты ON Платежи.[Id контракта] = Контракты.[Id контракта] " +
                    "JOIN Проекты ON Контракты.[Id проекта] = Проекты.[Id проекта] WHERE Контракты.[Id фрилансера] = @freelancer_id;", ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("freelancer_id", fId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                dataGridView1.Rows.Add(reader.GetInt32(0), reader.GetInt32(1),
                                    reader.GetString(2), reader.GetInt32(3), reader.GetDateTime(4).ToShortDateString(), reader.GetString(5));
                            }
                        }
                    }
                }
            
        }
    }
}
