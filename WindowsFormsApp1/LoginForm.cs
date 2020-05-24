using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            
        }
        
        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text.Equals("tudeptrai2k") && textBox2.Text.Equals("123456789")) == true)
            {
                Form1 f = new Form1();
                f.Show();
                this.Hide();
                return;
            }
            else
            {
                MessageBox.Show("Username or password is incorrect");
                textBox1.Clear();
                textBox2.Clear();
                return;
            }
        }
    }
}
