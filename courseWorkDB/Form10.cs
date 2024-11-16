using System.Data.SqlClient;
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

            string query = "SELECT Контракты.[Id контракта], Проекты.[Название проекта], " + // запрос 20 (просмотр информации о контрактах фрилансера)
                            "Контракты.[Дата начала], Контракты.[Дата окончания],  Контракты.[Статус контракта] " +
                            "FROM Контракты JOIN Проекты ON Контракты.[Id проекта] = Проекты.[Id проекта] " +
                            "WHERE Контракты.[Id фрилансера] = @freelancer_id;";

            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
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

        private void Form10_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
