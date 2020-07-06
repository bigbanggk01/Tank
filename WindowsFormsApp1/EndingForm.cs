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
    public partial class EndingForm : Form
    {
        public EndingForm()
        {
            InitializeComponent();
        }
        Form1 form;
        public void Get(Form1 f)
        {
            form = f;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            form.Close();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
