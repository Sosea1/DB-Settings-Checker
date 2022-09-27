using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.ServiceProcess;
using System.Drawing.Text;


namespace DB_Settings_Checker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class INIManager
        {
            //Конструктор, принимающий путь к INI-файлу
            public INIManager(string aPath)
            {
                path = aPath;
            }

            //Конструктор без аргументов (путь к INI-файлу нужно будет задать отдельно)
            public INIManager() : this("") { }

            //Возвращает значение из INI-файла (по указанным секции и ключу) 
            public string GetPrivateString(string aSection, string aKey)
            {
                //Для получения значения
                StringBuilder buffer = new StringBuilder(SIZE);

                //Получить значение в buffer
                GetPrivateString(aSection, aKey, null, buffer, SIZE, path);

                //Вернуть полученное значение
                return buffer.ToString();
            }

            //Пишет значение в INI-файл (по указанным секции и ключу) 
            public void WritePrivateString(string aSection, string aKey, string aValue)
            {
                //Записать значение в INI-файл
                WritePrivateString(aSection, aKey, aValue, path);
            }

            //Возвращает или устанавливает путь к INI файлу
            public string Path { get { return path; } set { path = value; } }

            //Поля класса
            private const int SIZE = 1024; //Максимальный размер (для чтения значения из файла)
            private string path = null; //Для хранения пути к INI-файлу

            //Импорт функции GetPrivateProfileString (для чтения значений) из библиотеки kernel32.dll
            [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
            private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

            //Импорт функции WritePrivateProfileString (для записи значений) из библиотеки kernel32.dll
            [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
            private static extern int WritePrivateString(string section, string key, string str, string path);



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Создание объекта, для работы с файлом
            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini");

            //Получить значение по ключу name из секции main
            string port = manager.GetPrivateString("mysqld", "port");
            label14.Text = port;
            //
            string bind_address = manager.GetPrivateString("mysqld", "bind-address");
            label15.Text = bind_address;
            if (bind_address.Length == 0) {
                label15.Text = "Разрешены все адреса";
            }
            string max_connections = manager.GetPrivateString("mysqld", "max_connections");
            label17.Text = max_connections;

            string connect_timeout = manager.GetPrivateString("mysqld", "connect_timeout");
            label16.Text = connect_timeout;
            if (connect_timeout.Length == 0)
            {
                label16.Text = "10";
            }
            string local_infile = manager.GetPrivateString("mysqld", "local-infile");
            label21.Text = local_infile;
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

            string symbolic_links = manager.GetPrivateString("mysqld", "symbolic_links");
            label20.Text = symbolic_links;
            if (symbolic_links.Length == 0)
            {
                label20.Text = "Включено";
            }


            /*
            //Записать значение по ключу age в секции main
            manager.WritePrivateString("main", "age", "21");
            */
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini");
            if (string.IsNullOrEmpty(textBox1.Text) == false)
            {
                manager.WritePrivateString("mysqld", "port", textBox1.Text);
            }
            if (string.IsNullOrEmpty(textBox2.Text) == false) 
            {
                manager.WritePrivateString("mysqld", "bind-address", textBox2.Text);
            }
            if (string.IsNullOrEmpty(textBox3.Text) == false)
            {
                manager.WritePrivateString("mysqld", "max-connections", textBox3.Text);
            }
            if (string.IsNullOrEmpty(textBox4.Text) == false)
            {
                manager.WritePrivateString("mysqld", "connect_timeout", textBox4.Text);
            }
            if (string.IsNullOrEmpty(textBox5.Text) == false)
            {
                manager.WritePrivateString("mysqld", "symbolic_links", textBox5.Text);
            }
            if (string.IsNullOrEmpty(textBox6.Text) == false)
            {
                manager.WritePrivateString("mysqld", "local-infile", textBox6.Text);
            }
           
            
            ServiceController ser = new ServiceController("MYSQL80");
            ser.Stop();
            Thread.Sleep(10000);
            ser.Start();
            ser.Close();


        }

    } 
        
}
