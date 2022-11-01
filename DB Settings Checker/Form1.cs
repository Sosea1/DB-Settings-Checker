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
            if (label29.Text != "3306") label29.BackColor = Color.Lime;
            else label29.BackColor = Color.Red;

            string symb = manager.GetPrivateString("mysqld", "symbolic-links");
            if(String.IsNullOrEmpty(symb)) label24.Text = "1"; else label24.Text = symb;
            if (label24.Text != "1" || String.IsNullOrEmpty(symb)) label24.BackColor = Color.Lime;
            else label24.BackColor = Color.Red;

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
            label28.Text = z + "IP address";

            if (bind_address.Length == 0) label28.Text = "любой";

            if (label28.Text == "любой") label28.BackColor = Color.Red;
            else label28.BackColor = Color.Lime;

           
            MySqlConnection conn = new MySqlConnection(form2.str());
            conn.Open();

            string sql = "select @@max_connections;";
            MySqlCommand command1 = new MySqlCommand(sql, conn);
            string max_con = command1.ExecuteScalar().ToString();
            label27.Text = max_con;

            sql = "select @@connect_timeout;";
            MySqlCommand command2 = new MySqlCommand(sql, conn);
            string conn_time = command2.ExecuteScalar().ToString();
            label26.Text = conn_time;

            sql = "select @@local_infile;";
            MySqlCommand command3 = new MySqlCommand(sql, conn);
            string loc_file = command2.ExecuteScalar().ToString();
            label25.Text = conn_time;



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ini_reader();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini"); // путь до INI файла

            //if (string.IsNullOrEmpty(textBox1.Text) == false) //если поле не пустое, в файл my.ini записывается новое значение
           // {
           //     manager.WritePrivateString("mysqld", "port", textBox1.Text); // записываем прот
          //  }
           // if (string.IsNullOrEmpty(textBox2.Text) == false)
          //  {
          //      manager.WritePrivateString("mysqld", "bind-address", textBox2.Text); // записываем доступные для подключения IP адреса
          //  }
        
          
            
                //перезагрузка сервиса MySQL, чтобы настройки вступили в силу
                ServiceController ser = new ServiceController("MYSQL80");
                ser.Stop();
                Thread.Sleep(5000);
                ser.Start();
                ser.Close();
                Thread.Sleep(5000);

                Ini_reader();

            }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }

    }

