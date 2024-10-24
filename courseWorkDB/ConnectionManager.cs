using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWorkDB
{
    public class ConnectionManager
    {
        private static string _connectionString = "Server=KATEPC\\SQLEXPRESS;Database=FreelancersEmployers;Integrated Security=True";
        private static SqlConnection _connection;

        public static void OpenConnection()
        {
            if (_connection == null || _connection.State == ConnectionState.Closed)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
        }

        public static void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public static SqlConnection GetConnection()
        {
            return _connection;
        }
    }
}
