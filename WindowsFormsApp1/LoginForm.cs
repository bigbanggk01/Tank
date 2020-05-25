using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public string s;
        Form1 form;

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            //SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-AB6F94G;Initial Catalog=TankDB;Integrated Security=True");
            string query = "Select * from Usertable Where username = '" + textBox1.Text.Trim() + "' and passwo = '" + textBox2.Text.Trim()+"'";
            form.networker.Send("Login;"+ query);
            form.networker.GetLogin(this);
            //s = textBox1.Text;
            //SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon);
            //DataTable dtbl = new DataTable();
            //sda.Fill(dtbl);
            //if(dtbl.Rows.Count ==1)
            //{
            //    dtbl = new DataTable();
            //    string query2 = "select tank from usertable where username ='" + textBox1.Text.Trim() + "' and tank ='1'";
            //    sda = new SqlDataAdapter(query2, sqlcon);
            //    sda.Fill(dtbl);
            //    if (dtbl.Rows.Count == 1) 
            //    {
            //        f = new Form1();
            //        f.GetForm(this);
            //        f.Show();
            //        this.Hide();
            //    }
            //    else
            //    {
            //        BuyForm bf = new BuyForm();
            //        bf.Show();
            //    }

            //}
            //else
            //{
            //    MessageBox.Show("Check your username and password");
            //}
        }
        //public void OpenF()
        //{
        //    Form1 f = new Form1();
        //    f.Show();
        //}
        public void GetForm(Form1 f)
        {
            form = f;
        }
    }
}
