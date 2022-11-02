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
        public string str() 
        {
            return "server=localhost;user=" + textBox1.Text + ";database=mydb;password=" + textBox2.Text + ";port=3306;";
        }

        public string pass()
        {
            return textBox2.Text;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            Form1 newform = new Form1();
            newform.Owner = this;
            //string connStr = "server=localhost;user=" + login +";database=mydb;password=" + pass + ";port=3360;";
            try
            {
                MySqlConnection conn = new MySqlConnection(str());

                conn.Open();
                conn.Close();
                this.Hide();
                newform.Show();
            }

            catch (Exception)
            {
                MessageBox.Show("Неправильные параметры для входа");
            }
        }
        
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
        }
    }
}
