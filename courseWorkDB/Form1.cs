using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

/*
TODO:   
        глазик на пароль
*/

namespace courseWorkDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //string trigger1 = "CREATE TRIGGER [dbo].[NewBid] ON [dbo].[Предложения] INSTEAD OF INSERT " +
            //    "AS BEGIN " +
            //    "INSERT INTO Предложения ([Id проекта], [Id фрилансера], [Текст предложения], [Сумма ставки], [Статус предложения]) " +
            //    "SELECT [Id проекта], [Id фрилансера], [Текст предложения], [Сумма ставки], 'На рассмотрении' FROM inserted; " +
            //    "END;";

            //string connectionString = "Server=KATEPC\\SQLEXPRESS;Database=FreelancersEmployers;Integrated Security=True";
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();
            //    using (SqlCommand command = new SqlCommand(trigger1, connection))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //}
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

        private void button1_Click(object sender, EventArgs e) // log in
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
            this.SendToBack();
        }

        private void button2_Click(object sender, EventArgs e) // create account
        {
            Form18 f18 = new Form18();
            f18.ShowDialog();
            this.SendToBack();
        }
    }
}
