using System.Data.SqlClient;
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

            string query = "SELECT Проекты.*, Наниматели.[Название компании] " + // запрос 13 (поиск информации о проектах)
                            "FROM Проекты LEFT JOIN Наниматели ON Проекты.[Id нанимателя] = Наниматели.[Id нанимателя];";

            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int pid = 0;
                            string employer = "";
                            string pname = "";
                            string pdesc = "";
                            int pbudget = 0;
                            string deadline = reader.GetDateTime(5).ToShortDateString();
                            string status = reader.GetString(6);
                            if (!reader.IsDBNull(0)) { pid = reader.GetInt32(0); }
                            if (!reader.IsDBNull(7)) { employer = reader.GetString(7); }
                            if (!reader.IsDBNull(2)) { pname = reader.GetString(2); }
                            if (!reader.IsDBNull(3)) { pdesc = reader.GetString(3); }
                            if (!reader.IsDBNull(4)) { pbudget = reader.GetInt32(4); }
                            if (reader.IsDBNull(1)) { employer = "deleted account"; }
                            dataGridView1.Rows.Add(pid, employer, pname, pdesc, pbudget, deadline, status);

                        }
                    }
                }
            }

        }

        private void Form8_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
