﻿using System;
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

            string login = textBox1.Text;
            string pass = textBox2.Text;
            string connStr = "server=localhost;user=" + login + ";database=mydb;password=" + pass + ";port=3306;";
            return connStr;
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

                //string sql = "SELECT name FROM men WHERE id = 2";

               // MySqlCommand command = new MySqlCommand(sql, conn);

               // string name = command.ExecuteScalar().ToString();

               // Console.WriteLine(name);

                

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
