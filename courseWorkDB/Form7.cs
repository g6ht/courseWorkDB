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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace courseWorkDB
{
    public partial class Form7 : Form
    {
        private int userId;
        private string oldUsername, oldEmail, oldPhonenumber;
        public Form7()
        {
            userId = Init.getUserId();
            string username, role, email, phone_number, password = "";
            InitializeComponent();

            
                using (SqlCommand command = new SqlCommand("SELECT * FROM Пользователи WHERE [Id пользователя] = @id", ConnectionManager.GetConnection()))
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
            
            textBox1.Text = username;
            textBox2.Text = password;
            textBox5.Text = role;
            textBox4.Text = email;
            textBox3.Text = phone_number;
            oldUsername = username;
            oldEmail = email;
            oldPhonenumber = phone_number;
        }

        private void button4_Click(object sender, EventArgs e) // delete account
        {
            DialogResult result = MessageBox.Show("Are you sure? This can`t be undone", "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                return;
            }
            else
            {

                    using (SqlCommand command = new SqlCommand("DELETE FROM Пользователи WHERE [Id пользователя] = @userId;", ConnectionManager.GetConnection()))
                    {
                        command.Parameters.AddWithValue("userId", userId);
                        command.ExecuteNonQuery();
                    }
                
                if (Init.getRole() == Role.Freelancer)
                {
                    Form5.DeleteAccount();
                }
                else if (Init.getRole() == Role.Employer)
                {
                    Form12.DeleteAccount();
                }
                MessageBox.Show("Your account deleted", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();   
            }
        }

        private void button1_Click(object sender, EventArgs e) // change
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e) //save
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Password is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string newUsername, newEmail, newPhoneNumber;
            byte[] newPassword = Init.sha256_hash(textBox2.Text);
            newUsername = textBox1.Text;
            newEmail = textBox4.Text;
            newPhoneNumber = textBox3.Text;
            if (newUsername.Length > 20) { MessageBox.Show("Username is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else if (newEmail.Length > 30) { MessageBox.Show("Email is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else if (newPhoneNumber.Length > 20) { MessageBox.Show("Phone number is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
                using (SqlCommand command1 = new SqlCommand("SELECT * FROM Пользователи WHERE [Электронная почта] = @email;", ConnectionManager.GetConnection()))
                {
                    command1.Parameters.AddWithValue("email", newEmail);
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {
                        if (reader.HasRows && newEmail != oldEmail)
                        {
                            MessageBox.Show("Account with this email already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            reader.Close();
                            using (SqlCommand command2 = new SqlCommand("SELECT * FROM Пользователи WHERE [Номер телефона] = @phoneNumber;", ConnectionManager.GetConnection()))
                            {
                                command2.Parameters.AddWithValue("phoneNumber", newEmail);
                                using (SqlDataReader reader1 = command2.ExecuteReader())
                                {
                                    if (reader1.HasRows && newPhoneNumber != oldPhonenumber)
                                    {
                                        MessageBox.Show("Account with this phone number already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        reader1.Close();
                                        using (SqlCommand command3 = new SqlCommand("SELECT * FROM Пользователи WHERE [Имя пользователя] = @username;", ConnectionManager.GetConnection()))
                                        {
                                            command3.Parameters.AddWithValue("username", newUsername);
                                            using (SqlDataReader reader2 = command3.ExecuteReader())
                                            {
                                                if (reader2.HasRows && newUsername != oldUsername)
                                                {
                                                    MessageBox.Show("Account with this username already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                                else
                                                {
                                                    reader2.Close();
                                                    using (SqlCommand command = new SqlCommand("UPDATE Пользователи " +
                                                        "SET [Электронная почта] = @newEmail, [Номер телефона] = @newPhoneNumber, [Имя пользователя] = @newUsername, Пароль = @newPassword " +
                                                        "WHERE [Id пользователя] = @userId;", ConnectionManager.GetConnection()))
                                                    {
                                                        command.Parameters.AddWithValue("newEmail", newEmail);
                                                        command.Parameters.AddWithValue("newPhoneNumber", newPhoneNumber);
                                                        command.Parameters.AddWithValue("newUsername", newUsername);
                                                        command.Parameters.AddWithValue("newPassword", newPassword);
                                                        command.Parameters.AddWithValue("userId", userId);

                                                        command.ExecuteNonQuery();
                                                        if (Init.getRole() == Role.Freelancer)
                                                        {
                                                            Form5.ChangeAccount(newUsername, newEmail, newPhoneNumber);
                                                        }
                                                        else if (Init.getRole() == Role.Employer)
                                                        {
                                                            Form12.ChangeAccount(newUsername, newEmail, newPhoneNumber);
                                                        }
                                                        this.Close();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }
    }
}
