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
        
       
        public void Ini_reader()
        {

            

            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini");

            //получение значения по ключу port из секции mysqld
            string port = manager.GetPrivateString("mysqld", "port");

            label14.Text = port; //для отображения текущего установленного значение
                                 //красный цвет - небезопасные настройки, зеленый - безопасные
            if (label14.Text == "3306")
            {
                label14.BackColor = Color.Red;
            }
            else
            {
                label14.BackColor = Color.Lime;
            }
            string bind_address = manager.GetPrivateString("mysqld", "bind-address"); //разрешенные IP-адреса
            label15.Text = bind_address;
            if (bind_address.Length == 0)
            {
                label15.Text = "Разрешены все адреса";
            }
            if (label15.Text == "Разрешены все адреса")
            {
                label15.BackColor = Color.Red;
            }
            else
            {
                label15.BackColor = Color.Lime;
            }

            string max_connections = manager.GetPrivateString("mysqld", "max_connections"); //количество одновременных подключений
            label17.Text = max_connections;
            string max_conn = label17.Text;
            if (int.Parse(max_conn) > 20)
            {
                label17.BackColor = Color.Red;
            }
            else
            {
                label17.BackColor = Color.Lime;
            }
            string connect_timeout = manager.GetPrivateString("mysqld", "connect_timeout"); //время для аутентификации
            label16.Text = connect_timeout;
            if (connect_timeout.Length == 0)
            {
                label16.Text = "10";
            }
            string conn_time = label16.Text;
            if (int.Parse(conn_time) > 10)
            {
                label16.BackColor = Color.Red;
            }
            else
            {
                label16.BackColor = Color.Lime;
            }
            string local_infile = manager.GetPrivateString("mysqld", "local-infile"); //чтение файлов
            label21.Text = local_infile;
            if (int.Parse(local_infile) == 1)
            {
                label21.BackColor = Color.Red;
            }
            else
            {
                label21.BackColor = Color.Lime;
            }

            if (local_infile.Length == 0)
            {
                label21.Text = "Включено";
            }
            else if (local_infile == "0")
            {
                label21.Text = "Выключено";
            }
            else if (local_infile == "1")
            {
                label21.Text = "Включено";
            }

            string symbolic_links = manager.GetPrivateString("mysqld", "symbolic_links"); // символические ссылки
            label20.Text = symbolic_links;
            if (symbolic_links == "1")
            {
                label20.BackColor = Color.Red;
            }
            if (string.IsNullOrEmpty(symbolic_links) == true)
            {
                label20.BackColor = Color.Red;
            }

            else
            {
                label20.BackColor = Color.Lime;
            }
            if (symbolic_links.Length == 0)
            {
                label20.Text = "Включено";
            }
            if (symbolic_links.Length == 1)
            { label20.Text = "Выключено"; }


            string safe_user_create = manager.GetPrivateString("mysqld", "safe_user_create"); // возможность модификации пользователей 
            label7.Text = safe_user_create;
            if (safe_user_create == "off")
            {
                label7.BackColor = Color.Red;
            }
            if (string.IsNullOrEmpty(safe_user_create) == true)
            {
                label7.BackColor = Color.Red;
            }

            else
            {
                label7.BackColor = Color.Lime;
            }
            if (safe_user_create.Length == 0)
            {
                label7.Text = "Включено";
            }
            if (safe_user_create.Length == 1)
            { label7.Text = "Выключено"; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ini_reader();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini"); // путь до INI файла

            if (string.IsNullOrEmpty(textBox1.Text) == false) //если поле не пустое, в файл my.ini записывается новое значение
            {
                manager.WritePrivateString("mysqld", "port", textBox1.Text); // записываем прот
            }
            if (string.IsNullOrEmpty(textBox2.Text) == false)
            {
                manager.WritePrivateString("mysqld", "bind-address", textBox2.Text); // записываем доступные для подключения IP адреса
            }
            if (string.IsNullOrEmpty(textBox3.Text) == false)
            {
                manager.WritePrivateString("mysqld", "max_connections", textBox3.Text); // записываем макисмальное кол-во подключений
            }
            if (string.IsNullOrEmpty(textBox4.Text) == false)
            {
                manager.WritePrivateString("mysqld", "connect_timeout", textBox4.Text); // время на подключение 
            }
            if (string.IsNullOrEmpty(textBox5.Text) == false)
            {
                manager.WritePrivateString("mysqld", "symbolic_links", textBox5.Text); //Символические ссылки
            }
            if (string.IsNullOrEmpty(textBox6.Text) == false)
            {
                manager.WritePrivateString("mysqld", "local-infile", textBox6.Text); // работа с файлами 
            }
            if (string.IsNullOrEmpty(textBox7.Text) == false)
            {
                manager.WritePrivateString("mysqld", "safe_user_create", textBox7.Text); // модификация пользователей
            }
            
                //перезагрузка сервиса MySQL, чтобы настройки вступили в силу
                ServiceController ser = new ServiceController("MYSQL80");
                ser.Stop();
                Thread.Sleep(5000);
                ser.Start();
                ser.Close();
                Thread.Sleep(5000);

                Ini_reader();

            }


    }

    }

