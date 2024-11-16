using System.Data;
using System.Data.SqlClient;
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
            UpdateTable();
        }

        private void UpdateTable()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id", "Bid id");
            dataGridView1.Columns.Add("name", "Project name");
            dataGridView1.Columns.Add("freelancer", "Freelancer");
            dataGridView1.Columns.Add("text", "Bid text");
            dataGridView1.Columns.Add("sum", "Bet");
            dataGridView1.Columns.Add("status", "Status");

            string query = "SELECT П.[Id предложения], Пр.[Название проекта], Пол.[Имя пользователя], П.[Текст предложения], П.[Сумма ставки], П.[Статус предложения], П.[Id фрилансера] " +
                            "FROM Предложения П " +
                            "JOIN Проекты Пр ON П.[Id проекта] = Пр.[Id проекта] " +
                            "LEFT JOIN Фрилансеры Ф ON П.[Id фрилансера] = Ф.[Id фрилансера] " +
                            "LEFT JOIN Пользователи Пол ON Ф.[Id пользователя] = Пол.[Id пользователя] " +
                            "WHERE Пр.[Id нанимателя] = @employer_id;"; // запрос 24 (поиск информации о предложениях)

            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("employer_id", employerId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int pid = 0;
                            string freelancer = "";
                            string pname = "";
                            string text = "";
                            int sum = 0;
                            string status = reader.GetString(5);
                            if (!reader.IsDBNull(0)) { pid = reader.GetInt32(0); }
                            if (!reader.IsDBNull(2)) { freelancer = reader.GetString(2); }
                            if (!reader.IsDBNull(1)) { pname = reader.GetString(1); }
                            if (!reader.IsDBNull(3)) { text = reader.GetString(3); }
                            if (!reader.IsDBNull(4)) { sum = reader.GetInt32(4); }
                            if (reader.IsDBNull(2)) { freelancer = "deleted account"; }
                            else { freelancer += " (id " + reader.GetInt32(6) + ")"; }
                            dataGridView1.Rows.Add(pid, pname, freelancer, text, sum, status);
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, System.EventArgs e) // accept bid
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int bidId))
            {
                string query = "SELECT * FROM Предложения WHERE [Id предложения] = @id"; // запрос 18 (проверка на существование предложения)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", bidId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            reader.Close();

                            using (SqlCommand command1 = new SqlCommand("NewBidStatus", ConnectionManager.GetConnection()))
                            {
                                command1.CommandType = CommandType.StoredProcedure;
                                SqlParameter idParam = new SqlParameter { ParameterName = "@bidId", Value = bidId };
                                SqlParameter statusParam = new SqlParameter { ParameterName = "@newStatus", Value = "Принято" };
                                command1.Parameters.Add(idParam);
                                command1.Parameters.Add(statusParam);
                                command1.ExecuteNonQuery();
                                UpdateTable();
                            }
                        }

                        else
                        {
                            MessageBox.Show("To accept or decline bid, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To accept or decline bid, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, System.EventArgs e) // decline bid
        {
            if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int bidId))
            {
                string query = "SELECT * FROM Предложения WHERE [Id предложения] = @id"; // запрос 18 (проверка на существование предложения)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("id", bidId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            reader.Close();

                            using (SqlCommand command1 = new SqlCommand("NewBidStatus", ConnectionManager.GetConnection()))
                            {
                                command1.CommandType = CommandType.StoredProcedure;
                                SqlParameter idParam = new SqlParameter { ParameterName = "@bidId", Value = bidId };
                                SqlParameter statusParam = new SqlParameter { ParameterName = "@newStatus", Value = "Отклонено" };
                                command1.Parameters.Add(idParam);
                                command1.Parameters.Add(statusParam);
                                command1.ExecuteNonQuery();
                                UpdateTable();
                            }
                        }

                        else
                        {
                            MessageBox.Show("To accept or decline bid, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("To accept or decline bid, click on its id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form14_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
