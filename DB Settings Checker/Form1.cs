using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.ServiceProcess;
using System.Drawing.Text;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using MySqlConnector;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.DataFormats;
using System.ComponentModel;

namespace DB_Settings_Checker
{
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Используем уже разработанный класс. INIManager Источник: http://plssite.ru/csharp/csharp_ini_files_article.html
        //Для работы с ini файлами 
        
       
        public void Ini_reader() // метод для чтения глобальных переменных в mysql и ини файле конфигурации
        {
            Form2 form2 = (Form2)this.Owner;
            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini"); //путь до ини файла

            //получение значения по ключу port из секции mysqld
            string port = manager.GetPrivateString("mysqld", "port");

            label29.Text = port; //для отображения текущего установленного значение                 
            if (label29.Text != "3306") label29.ForeColor = Color.Green;
            else label29.ForeColor = Color.Red; //красный цвет - небезопасные настройки, зеленый - безопасные

            string symb = manager.GetPrivateString("mysqld", "symbolic-links");
            if(String.IsNullOrEmpty(symb)) label24.Text = "1"; else label24.Text = symb;
            if (label24.Text != "1" || String.IsNullOrEmpty(symb)) label24.ForeColor = Color.Green;
            else label24.ForeColor = Color.Red;

            string bind_address = manager.GetPrivateString("mysqld", "bind-address"); //разрешенные IP-адреса
            int z = 0;
            if (bind_address == "0.0.0.0") label28.Text = "любой";
            else
            {
                for (int i = 0; i < bind_address.Length; i++)
                {
                    if (bind_address[i] == ',') 
                    {
                        z++;
                    }
                }
            }
            label28.Text = z + " IP address(es)";// проверка  сколько IP адрессов введено 
            if (bind_address.Length == 0) label28.Text = "любой";
            if (label28.Text == "любой") label28.ForeColor = Color.Red;
            else label28.ForeColor = Color.Green;

            MySqlConnection conn = new MySqlConnection(form2.str()); // создание подключения к БД
            conn.Open();

            string sql = "select @@max_connections;"; //команда для получения значения
            MySqlCommand command1 = new MySqlCommand(sql, conn);
            string max_con = command1.ExecuteScalar().ToString();
            label27.Text = max_con; // получение значения 
            if (int.Parse(max_con) >= 100) label27.ForeColor = Color.Red; else label27.ForeColor = Color.Green;

            sql = "select @@connect_timeout;";
            command1 = new MySqlCommand(sql, conn);
            string conn_time = command1.ExecuteScalar().ToString();
            label26.Text = conn_time;
            if (int.Parse(conn_time) > 10) label26.ForeColor = Color.Red; else label26.ForeColor = Color.Green;

            sql = "select @@local_infile;";
            command1 = new MySqlCommand(sql, conn);
            string loc_file = command1.ExecuteScalar().ToString();
            if(string.IsNullOrEmpty(loc_file) || loc_file=="0")
            {
                label25.Text = "0";
                label25.ForeColor = Color.Green;
            }
            else
            {
                label25.Text = loc_file;
                label25.ForeColor = Color.Red;
            }
            
            sql = "select @@max_user_connections;";
            command1 = new MySqlCommand(sql, conn);
            string max_user = command1.ExecuteScalar().ToString();
            label22.Text = max_user;
            if (int.Parse(max_user) > 0) label22.ForeColor = Color.Red; else label22.ForeColor = Color.Green;
           
            if (form2.pass().Length < 8) // проверка длины пароля
            {
                label21.ForeColor = Color.Red;
                label21.Text = "Не надежный";
            }
            else
            {
                label21.ForeColor = Color.Green;
                label21.Text = "Надежный";
            }
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ini_reader(); // вызов метода

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // выход из приложения если была закрыта первая форма, так как основная 2
        }

        private void button1_Click(object sender, EventArgs e) // кнопка прменить 
        {
            Form2 form2 = (Form2)this.Owner;
           
            MySqlConnection conn = new MySqlConnection(form2.str());
            conn.Open(); // открываем соединение

            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini"); // путь до INI файла
            string query;
            MySqlCommand comm;

            try
            {
                //если поле не пустое, в файл my.ini записывается новое значение
                if (string.IsNullOrEmpty(textBox18.Text) == false) manager.WritePrivateString("mysqld", "port", textBox18.Text); // записываем порт
                if (string.IsNullOrEmpty(textBox17.Text) == false) manager.WritePrivateString("mysqld", "bind-address", textBox17.Text);// записываем доступные для подключения IP адреса
                if (string.IsNullOrEmpty(textBox2.Text) == false) manager.WritePrivateString("mysqld", "symbolic-links", textBox2.Text); // аналогично символические ссылки




                if (string.IsNullOrEmpty(textBox16.Text) == false) // устанавливаем новое глобальное значение
                {
                    query = "set persist max_connections = " + textBox16.Text + "; SET @@PERSIST.max_connections = " + textBox16.Text + ";" ;
                    comm = new MySqlCommand(query, conn);
                    comm.ExecuteNonQuery();
                }

                if (string.IsNullOrEmpty(textBox15.Text) == false)
                {
                    query = "set persist connect_timeout = " + textBox15.Text + ";SET @@PERSIST.connect_timeout = " + textBox15.Text + ";";
                    comm = new MySqlCommand(query, conn);
                    comm.ExecuteNonQuery();
                }


                if (string.IsNullOrEmpty(textBox1.Text) == false) // устанавливаем новое глобальное значение
                {
                    query = "set persist local_infile = " + textBox1.Text + ";SET @@PERSIST.local_infile = " + textBox1.Text + ";";
                    comm = new MySqlCommand(query, conn);
                    comm.ExecuteNonQuery();
                }


                if (string.IsNullOrEmpty(textBox11.Text) == false)
                {
                    query = "set persist max_user_connections = " + textBox11.Text + ";SET @@PERSIST.max_user_connections = " + textBox11.Text + ";";
                    comm = new MySqlCommand(query, conn);
                    comm.ExecuteNonQuery();
                }

                if (string.IsNullOrEmpty(textBox10.Text) == false)
                {
                    if (textBox10.TextLength >= 8)
                    {
                        query = "ALTER USER 'root'@'localhost' IDENTIFIED BY '" + textBox10.Text + "';"; // устанавливаем новый пароль для рута
                        comm = new MySqlCommand(query, conn);
                        comm.ExecuteNonQuery();
                    }
                    else MessageBox.Show("Пароль должен содержать 8 символов или больше");
                }
            }
            catch (MySqlException ex)
            {
              MessageBox.Show("Произошла ошибка: " + ex.Message,"ERROR",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                //перезагрузка сервиса MySQL, чтобы настройки вступили в силу
                ServiceController ser = new ServiceController("MYSQL80");
                ser.Stop();
                Thread.Sleep(5000);
                ser.Start();
                ser.Close();
                Thread.Sleep(5000);

            }
            catch (InvalidOperationException ex) // выявление ошибок если не найдена служба или нет прав администратора и т.п.
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Ini_reader();
            conn.Close();
        }
        public void num(KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) // запретим вводить символы в поля принимающие числа
            {
                e.Handled = true;
            }
        }
        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            num(e);  
        }
    }

    }

