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
            public INIManager(string aPath)     //�����������, ����������� ���� � INI-�����
            {
                path = aPath;           
            }

            public INIManager() : this("") { }

            //���������� �������� �� INI-����� (�� ��������� ������ � �����) 
            public string GetPrivateString(string aSection, string aKey)
            {
                //��� ��������� ��������
                StringBuilder buffer = new StringBuilder(SIZE);

                //�������� �������� � buffer
                GetPrivateString(aSection, aKey, null, buffer, SIZE, path);

                return buffer.ToString();
            }

            //����� �������� � INI-���� (�� ��������� ������ � �����) 
            public void WritePrivateString(string aSection, string aKey, string aValue)
            {
                WritePrivateString(aSection, aKey, aValue, path);
            }

            //���������� ��� ������������� ���� � INI �����
            public string Path { get { return path; } set { path = value; } }

            //���� ������
            private const int SIZE = 1024; //������������ ������ (��� ������ �������� �� �����)
            private string path = null; //��� �������� ���� � INI-�����

            //������ ������� GetPrivateProfileString (��� ������ ��������) 
            [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
            private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

            //������ ������� WritePrivateProfileString (��� ������ ��������) 
            [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
            private static extern int WritePrivateString(string section, string key, string str, string path);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini");

            //��������� �������� �� ����� port �� ������ mysqld
            string port = manager.GetPrivateString("mysqld", "port");
            
            label14.Text = port; //��� ����������� �������� �������������� ��������
            //������� ���� - ������������ ���������, ������� - ����������
                if (label14.Text == "3306")
                {
                    label14.ForeColor = Color.Red;
                }
                else
                {
                    label14.ForeColor = Color.Green;
                }
            
            string bind_address = manager.GetPrivateString("mysqld", "bind-address"); //����������� IP-������
            label15.Text = bind_address;
                if (bind_address.Length == 0) 
                {
                    label15.Text = "��������� ��� ������";
                }
            if (label15.Text == "��������� ��� ������")
            {
                label15.ForeColor = Color.Red;
            }
            else
            {
                label15.ForeColor = Color.Green;
            }

            string max_connections = manager.GetPrivateString("mysqld", "max_connections"); //���������� ������������� �����������
            label17.Text = max_connections;
            string max_conn = label17.Text;
            if (int.Parse(max_conn) > 20)
            {
                label17.ForeColor = Color.Red;
            }
            else
            {
                label17.ForeColor = Color.Green;
            }


            string connect_timeout = manager.GetPrivateString("mysqld", "connect_timeout"); //����� ��� ��������������
            label16.Text = connect_timeout;
            if (connect_timeout.Length == 0)
            {
                label16.Text = "10";
            }
            string conn_time = label16.Text;
            if (int.Parse(conn_time) > 10)
            {
                label16.ForeColor = Color.Red;
            }
            else
            {
                label16.ForeColor = Color.Green;
            }

            string local_infile = manager.GetPrivateString("mysqld", "local-infile"); //������ ������
            label21.Text = local_infile;

            
            if (int.Parse(local_infile) == 1)
            {
                label21.ForeColor = Color.Red;
            }
            else
            {
                label21.ForeColor = Color.Green;
            }

            if (local_infile.Length == 0)
            {
                label21.Text = "��������";
            }
            else if (local_infile == "0") 
            {
                label21.Text = "���������";
            }
            else if (local_infile == "1")
            {
                label21.Text = "��������";
            }

            

            string symbolic_links = manager.GetPrivateString("mysqld", "symbolic_links"); // ������������� ������
            label20.Text = symbolic_links;
            if (symbolic_links == "1")
            {
                label20.ForeColor = Color.Red;
            }
            if (string.IsNullOrEmpty(symbolic_links) == true)
            {
                label20.ForeColor = Color.Red;
            }

            else
            {
                label20.ForeColor = Color.Green;
            }
            if (symbolic_links.Length == 0)
            {
                label20.Text = "��������";
            }
            if (symbolic_links.Length == 1) 
            { label20.Text = "���������"; }
          
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini");

            if (string.IsNullOrEmpty(textBox1.Text) == false) //���� ���� �� ������, � ���� my.ini ������������ ����� ��������
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
           
            //������������ ������� MySQL, ����� ��������� �������� � ����
            ServiceController ser = new ServiceController("MYSQL80");
            ser.Stop();
            Thread.Sleep(5000);
            ser.Start();
            ser.Close();
            Thread.Sleep(5000);
            

            //��������� �������� �� ����� port �� ������ mysqld
            string port = manager.GetPrivateString("mysqld", "port");

            label14.Text = port; //��� ����������� �������� �������������� ��������
                                 //������� ���� - ������������ ���������, ������� - ����������
            if (label14.Text == "3306")
            {
                label14.ForeColor = Color.Red;
            }
            else
            {
                label14.ForeColor = Color.Green;
            }

            string bind_address = manager.GetPrivateString("mysqld", "bind-address"); //����������� IP-������
            label15.Text = bind_address;
            if (bind_address.Length == 0)
            {
                label15.Text = "��������� ��� ������";
            }
            if (label15.Text == "��������� ��� ������")
            {
                label15.ForeColor = Color.Red;
            }
            else
            {
                label15.ForeColor = Color.Green;
            }

            string max_connections = manager.GetPrivateString("mysqld", "max_connections"); //���������� ������������� �����������
            label17.Text = max_connections;
            string max_conn = label17.Text;
            if (int.Parse(max_conn) > 20)
            {
                label17.ForeColor = Color.Red;
            }
            else
            {
                label17.ForeColor = Color.Green;
            }


            string connect_timeout = manager.GetPrivateString("mysqld", "connect_timeout"); //����� ��� ��������������
            label16.Text = connect_timeout;
            if (connect_timeout.Length == 0)
            {
                label16.Text = "10";
            }
            string conn_time = label16.Text;
            if (int.Parse(conn_time) > 10)
            {
                label16.ForeColor = Color.Red;
            }
            else
            {
                label16.ForeColor = Color.Green;
            }

            string local_infile = manager.GetPrivateString("mysqld", "local-infile"); //������ ������
            label21.Text = local_infile;


            if (int.Parse(local_infile) == 1)
            {
                label21.ForeColor = Color.Red;
            }
            else
            {
                label21.ForeColor = Color.Green;
            }

            if (local_infile.Length == 0)
            {
                label21.Text = "��������";
            }
            else if (local_infile == "0")
            {
                label21.Text = "���������";
            }
            else if (local_infile == "1")
            {
                label21.Text = "��������";
            }



            string symbolic_links = manager.GetPrivateString("mysqld", "symbolic_links"); // ������������� ������
            label20.Text = symbolic_links;
            if (symbolic_links == "1")
            {
                label20.ForeColor = Color.Red;
            }
            if (string.IsNullOrEmpty(symbolic_links) == true)
            {
                label20.ForeColor = Color.Red;
            }

            else
            {
                label20.ForeColor = Color.Green;
            }
            if (symbolic_links.Length == 0)
            {
                label20.Text = "��������";
            }
            if (symbolic_links.Length == 1)
            { label20.Text = "���������"; }

        }


    } 
        
}
