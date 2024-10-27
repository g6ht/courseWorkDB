using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace courseWorkDB
{
    public enum Role { Freelancer, Employer };
    public static class Init
    {
        private static object currentUser;
        private static Role role;
        private static int feId;
        private static int userId;
        public static object CurrentUser
        {
            get { if (role == Role.Freelancer) { return (Freelancer)currentUser; } else { return (Employer)currentUser; } }
            set { currentUser = value; }
        }
        public static int getFeId() { return feId; }
        public static int getUserId() { return userId; }
        public static Role getRole() { return role; }
        public static void InitUser(Role _role, bool newAccount, int _userId, int _feId)
        {
            userId = _userId;
            feId = _feId;
            role = _role;
            if (role == Role.Freelancer)
            {
                InitFreelancer(newAccount, userId, feId);
            }
            else if (role == Role.Employer)
            {
                InitEmployer(newAccount, userId, feId);
            }
        }

        private static void InitFreelancer(bool newAccount, int userId, int fId)
        {

            string username, role, email, phone_number;

            string query = "SELECT * FROM Пользователи WHERE [Id пользователя] = @id"; // запрос 39 (получение всей информации о пользователе)

            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("@id", userId);
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
                string skills = "", experience = "", info = "";
                int rate = 0;

                query = "SELECT * FROM Фрилансеры WHERE [Id фрилансера] = @id;"; // запрос 40 (получение всей информации о фрилансере)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("@id", fId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.IsDBNull(2)) { skills = reader.GetString(2); }
                        if (!reader.IsDBNull(3)) { experience = reader.GetString(3); }
                        if (!reader.IsDBNull(4)) { rate = reader.GetInt32(4); }
                        if (!reader.IsDBNull(5)) { info = reader.GetString(5); }
                    }
                }
                currentUser = new Freelancer(userId, username, role, email, phone_number, fId, skills, experience, rate, info);
            }
            else { currentUser = new Freelancer(userId, username, role, email, phone_number, fId); }

        }

        private static void InitEmployer(bool newAccount, int userId, int eId)
        {
            string username, role, email, phone_number;

            string query = "SELECT * FROM Пользователи WHERE [Id пользователя] = @id"; // запрос 39 (получение всей информации о пользователе)

            using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
            {
                command.Parameters.AddWithValue("@id", userId);
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

                query = "SELECT * FROM Наниматели WHERE [Id нанимателя] = @id;"; // запрос 41 (получение всей информвции о нанимателе)

                using (SqlCommand command = new SqlCommand(query, ConnectionManager.GetConnection()))
                {
                    command.Parameters.AddWithValue("@id", eId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.IsDBNull(2)) { companyName = reader.GetString(2); }
                        if (!reader.IsDBNull(3)) { info = reader.GetString(3); }
                    }
                }
                currentUser = new Employer(userId, username, role, email, phone_number, eId, companyName, info);
            }
            else { currentUser = new Employer(userId, username, role, email, phone_number, eId); }

        }

        public static byte[] sha256_hash(string value)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                byte[] hash = sha256.ComputeHash(bytes);
                return hash;
            }
        }
    }
}
