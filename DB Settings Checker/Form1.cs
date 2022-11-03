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
        //���������� ��� ������������� �����. INIManager ��������: http://plssite.ru/csharp/csharp_ini_files_article.html
        //��� ������ � ini ������� 
        
       
        public void Ini_reader() // ����� ��� ������ ���������� ���������� � mysql � ��� ����� ������������
        {
            Form2 form2 = (Form2)this.Owner;
            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini"); //���� �� ��� �����

            //��������� �������� �� ����� port �� ������ mysqld
            string port = manager.GetPrivateString("mysqld", "port");

            label29.Text = port; //��� ����������� �������� �������������� ��������                 
            if (label29.Text != "3306") label29.ForeColor = Color.Green;
            else label29.ForeColor = Color.Red; //������� ���� - ������������ ���������, ������� - ����������

            string symb = manager.GetPrivateString("mysqld", "symbolic-links");
            if(String.IsNullOrEmpty(symb)) label24.Text = "1"; else label24.Text = symb;
            if (label24.Text != "1" || String.IsNullOrEmpty(symb)) label24.ForeColor = Color.Green;
            else label24.ForeColor = Color.Red;

            string bind_address = manager.GetPrivateString("mysqld", "bind-address"); //����������� IP-������
            int z = 0;
            if (bind_address == "0.0.0.0") label28.Text = "�����";
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
            label28.Text = z + " IP address(es)";// ��������  ������� IP �������� ������� 
            if (bind_address.Length == 0) label28.Text = "�����";
            if (label28.Text == "�����") label28.ForeColor = Color.Red;
            else label28.ForeColor = Color.Green;

            MySqlConnection conn = new MySqlConnection(form2.str()); // �������� ����������� � ��
            conn.Open();

            string sql = "select @@max_connections;"; //������� ��� ��������� ��������
            MySqlCommand command1 = new MySqlCommand(sql, conn);
            string max_con = command1.ExecuteScalar().ToString();
            label27.Text = max_con; // ��������� �������� 
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
           
            if (form2.pass().Length < 8) // �������� ����� ������
            {
                label21.ForeColor = Color.Red;
                label21.Text = "�� ��������";
            }
            else
            {
                label21.ForeColor = Color.Green;
                label21.Text = "��������";
            }
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ini_reader(); // ����� ������

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // ����� �� ���������� ���� ���� ������� ������ �����, ��� ��� �������� 2
        }

        private void button1_Click(object sender, EventArgs e) // ������ �������� 
        {
            Form2 form2 = (Form2)this.Owner;
           
            MySqlConnection conn = new MySqlConnection(form2.str());
            conn.Open(); // ��������� ����������

            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini"); // ���� �� INI �����
            string query;
            MySqlCommand comm;

            try
            {
                //���� ���� �� ������, � ���� my.ini ������������ ����� ��������
                if (string.IsNullOrEmpty(textBox18.Text) == false) manager.WritePrivateString("mysqld", "port", textBox18.Text); // ���������� ����
                if (string.IsNullOrEmpty(textBox17.Text) == false) manager.WritePrivateString("mysqld", "bind-address", textBox17.Text);// ���������� ��������� ��� ����������� IP ������
                if (string.IsNullOrEmpty(textBox2.Text) == false) manager.WritePrivateString("mysqld", "symbolic-links", textBox2.Text); // ���������� ������������� ������




                if (string.IsNullOrEmpty(textBox16.Text) == false) // ������������� ����� ���������� ��������
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


                if (string.IsNullOrEmpty(textBox1.Text) == false) // ������������� ����� ���������� ��������
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
                        query = "ALTER USER 'root'@'localhost' IDENTIFIED BY '" + textBox10.Text + "';"; // ������������� ����� ������ ��� ����
                        comm = new MySqlCommand(query, conn);
                        comm.ExecuteNonQuery();
                    }
                    else MessageBox.Show("������ ������ ��������� 8 �������� ��� ������");
                }
            }
            catch (MySqlException ex)
            {
              MessageBox.Show("��������� ������: " + ex.Message,"ERROR",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                //������������ ������� MySQL, ����� ��������� �������� � ����
                ServiceController ser = new ServiceController("MYSQL80");
                ser.Stop();
                Thread.Sleep(5000);
                ser.Start();
                ser.Close();
                Thread.Sleep(5000);

            }
            catch (InvalidOperationException ex) // ��������� ������ ���� �� ������� ������ ��� ��� ���� �������������� � �.�.
            {
                MessageBox.Show("��������� ������: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("��������� ������: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Ini_reader();
            conn.Close();
        }
        public void num(KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) // �������� ������� ������� � ���� ����������� �����
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

