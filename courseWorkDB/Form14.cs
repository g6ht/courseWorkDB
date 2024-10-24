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
    public partial class Form14 : Form
    {
        private int employerId;
        public Form14()
        {
            InitializeComponent();
            employerId = Init.getFeId();

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id", "Bid id");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("freelancer", "Freelancer");
            dataGridView1.Columns.Add("text", "Bid text");
            dataGridView1.Columns.Add("sum", "Bet");
            dataGridView1.Columns.Add("status", "Status");

            using (SqlCommand command = new SqlCommand("SELECT П.[Id предложения], Пр.[Название проекта], Пол.[Имя пользователя], П.[Текст предложения], П.[Сумма ставки], П.[Статус предложения] " +
                "FROM Предложения П " +
                "JOIN Проекты Пр ON П.[Id проекта] = Пр.[Id проекта] " +
                "JOIN Фрилансеры Ф ON П.[Id фрилансера] = Ф.[Id фрилансера] " +
                "JOIN Пользователи Пол ON Ф.[Id пользователя] = Пол.[Id пользователя] " +
                "WHERE Пр.[Id нанимателя] = @employer_id;", ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("employer_id", employerId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                                reader.GetString(3), reader.GetInt32(4), reader.GetString(5));
                        }
                    }
                }
            }
        }
    }
}
