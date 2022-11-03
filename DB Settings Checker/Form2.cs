using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.Logging;
using MySqlConnector;

namespace DB_Settings_Checker
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public string str() // получаем строку подключения, это для 1 формы
        {
            return "server=localhost;user=" + textBox1.Text + ";database=mydb;password=" + textBox2.Text + ";port=" + textBox3.Text + ";";
        }
        public string pass() //получаем пароль, так же для 1 формы 
        {
            return textBox2.Text;
        }
        public void button1_Click(object sender, EventArgs e)
        {
            Form1 newform = new Form1();
            newform.Owner = this;
            //string connStr = "server=localhost;user=" + login +";database=mydb;password=" + pass + ";port=3360;";
            try // пробуем подключиться к БД
            {
                MySqlConnection conn = new MySqlConnection(str());

                conn.Open();
                conn.Close();
                this.Hide();
                newform.Show(); // всё отлично открываем 1 форму для работы с приложением
            }
            catch (Exception) //ошибка 
            {
                MessageBox.Show("Неправильные параметры для входа");
            }
        }
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick(); // в поле ввода пароль можно нажать Enter чтобы не нажимать на кнопку
        }
    }
}
