using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace DB_Settings_Checker
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            
            try
            {
               string connStr = "server=localhost;user=" + textBox1.Text +";database=mydb;password=" + textBox2.Text + ";port=3360;";


                MySqlConnection conn = new MySqlConnection(connStr);

                conn.Open();
                this.Hide();
                Form1 frm = new Form1();   
                frm.Show();

                //string sql = "SELECT name FROM men WHERE id = 2";

               // MySqlCommand command = new MySqlCommand(sql, conn);

               // string name = command.ExecuteScalar().ToString();

               // Console.WriteLine(name);

                conn.Close();

                MessageBox.Show("Всё путём!");

            }
            catch (Exception)
            {
                MessageBox.Show("Не сработало");

            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
        }
    }
}
