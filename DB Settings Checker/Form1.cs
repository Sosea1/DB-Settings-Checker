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
            Form2 form2 = (Form2)this.Owner;
            
            

            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini");

            //получение значения по ключу port из секции mysqld
            string port = manager.GetPrivateString("mysqld", "port");

            label29.Text = port; //для отображения текущего установленного значение
                                  //красный цвет - небезопасные настройки, зеленый - безопасные
            if (label29.Text != "3306") label29.ForeColor = Color.Green;
            else label29.ForeColor = Color.Red;

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
            label28.Text = z + " IP address(es)";

            if (bind_address.Length == 0) label28.Text = "любой";

            if (label28.Text == "любой") label28.ForeColor = Color.Red;
            else label28.ForeColor = Color.Green;

           
            MySqlConnection conn = new MySqlConnection(form2.str());
            conn.Open();

            string sql = "select @@max_connections;";
            MySqlCommand command1 = new MySqlCommand(sql, conn);
            string max_con = command1.ExecuteScalar().ToString();
            label27.Text = max_con;
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
                label25.ForeColor = Color.Red;
            }
            else
            {
                label25.Text = loc_file;
                label25.ForeColor = Color.Green;
            }
            


            sql = "select @@max_user_connections;";
            command1 = new MySqlCommand(sql, conn);
            string max_user = command1.ExecuteScalar().ToString();
            label22.Text = max_user;
            if (int.Parse(max_user) > 0) label22.ForeColor = Color.Red; else label22.ForeColor = Color.Green;
            

            if (form2.pass().Length < 8)
            {
                label21.ForeColor = Color.Red;
                label21.Text = "Не надежный";
            }
            else
            {
                label21.ForeColor = Color.Green;
                label21.Text = "Надежный";
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ini_reader();

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini"); // путь до INI файла

            //если поле не пустое, в файл my.ini записывается новое значение
            if (string.IsNullOrEmpty(textBox18.Text) == false) manager.WritePrivateString("mysqld", "port", textBox18.Text); // записываем порт
            if (string.IsNullOrEmpty(textBox17.Text) == false) manager.WritePrivateString("mysqld", "bind-address", textBox17.Text); // записываем доступные для подключения IP адреса

            string query = "set @max_connections = "+ textBox16;

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

