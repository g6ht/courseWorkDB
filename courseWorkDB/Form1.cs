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
TODO:   (сделано что-то похожее)вынести юзера в неймспейс
        (типо сделано)создавать юзера в отдельном классе (мб все статики туда вынести)
        (done)подключаться к бд в главной функции 1 раз

        (да нахуй надо)вынести запросы в отдельную строчку и добавить комментарии с номером запроса

        (сделано)выводить таблицы красивее (можно отображать имя фрилансера/нанимателя, они уникальны)
        (сделано)проверять че там чел вводит когда хочет что то поменять (чтобы за границы не вышел в бд)

        осталось только платежи, оформить триггеры в отчёте, мб придется менять запросы, что то сделать с тем, что я не удаляю проекты и контракты
        пристроить хранимые процедуры
        как то надо статусы менять контрактам, проектам и предложениям (мб все таки вернуть кнопки нанимателю но хз тогда что с контрактами)

        
        глазик на пароль

        если будет время то организовать запросы транзакциями
        мб сделать типо сумма заработанных бабок фрилансером?
*/

namespace courseWorkDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //string trigger1 = "CREATE TRIGGER [dbo].[NewContract] ON [dbo].[Контракты] INSTEAD OF INSERT " +
            //    "AS BEGIN INSERT INTO Контракты ([Id проекта], [Id фрилансера], [Дата начала], [Дата окончания], [Статус контракта]) " +
            //    "SELECT [Id проекта], [Id фрилансера], [Дата начала], [Дата окончания], 'Активен' FROM inserted; " +
            //    "END;";

            //using (SqlCommand command = new SqlCommand(trigger1, ConnectionManager.GetConnection()))
            //{
            //    command.ExecuteNonQuery();
            //}

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
