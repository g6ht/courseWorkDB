using System;
using System.Windows.Forms;

namespace courseWorkDB
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConnectionManager.OpenConnection();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            Application.Run(form);
            ConnectionManager.CloseConnection();
        }
    }
}
