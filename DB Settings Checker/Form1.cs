using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;

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
            //�����������, ����������� ���� � INI-�����
            public INIManager(string aPath)
            {
                path = aPath;
            }

            //����������� ��� ���������� (���� � INI-����� ����� ����� ������ ��������)
            public INIManager() : this("") { }

            //���������� �������� �� INI-����� (�� ��������� ������ � �����) 
            public string GetPrivateString(string aSection, string aKey)
            {
                //��� ��������� ��������
                StringBuilder buffer = new StringBuilder(SIZE);

                //�������� �������� � buffer
                GetPrivateString(aSection, aKey, null, buffer, SIZE, path);

                //������� ���������� ��������
                return buffer.ToString();
            }

            //����� �������� � INI-���� (�� ��������� ������ � �����) 
            public void WritePrivateString(string aSection, string aKey, string aValue)
            {
                //�������� �������� � INI-����
                WritePrivateString(aSection, aKey, aValue, path);
            }

            //���������� ��� ������������� ���� � INI �����
            public string Path { get { return path; } set { path = value; } }

            //���� ������
            private const int SIZE = 1024; //������������ ������ (��� ������ �������� �� �����)
            private string path = null; //��� �������� ���� � INI-�����

            //������ ������� GetPrivateProfileString (��� ������ ��������) �� ���������� kernel32.dll
            [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
            private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

            //������ ������� WritePrivateProfileString (��� ������ ��������) �� ���������� kernel32.dll
            [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
            private static extern int WritePrivateString(string section, string key, string str, string path);



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //�������� �������, ��� ������ � ������
            INIManager manager = new INIManager("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\my.ini");

            //�������� �������� �� ����� name �� ������ main
            string port = manager.GetPrivateString("mysqld", "port");
            label14.Text = port;
            /*
            //�������� �������� �� ����� age � ������ main
            manager.WritePrivateString("main", "age", "21");
            */
        }
    } 
        
}
