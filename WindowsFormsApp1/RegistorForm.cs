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
    public partial class RegistorForm : Form
    {
        public RegistorForm()
        {
            InitializeComponent();
        }
        Form1 form;
        public void GetForm(Form1 f)
        {
            form = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Equals("")==false&& textBox2.Text.Equals("")==false && textBox3.Text.Equals("")==false)
            {
                if (textBox2.Text.Equals(textBox3.Text) == false)
                {
                    MessageBox.Show("Vui lòng xác nhận lại mật khẩu !");
                }
            }    
            if (checkBox1.Checked == true) 
            {
                form.networker.Send("register;" + textBox1.Text + ";" + textBox2.Text);
            }
        }
    }
}
