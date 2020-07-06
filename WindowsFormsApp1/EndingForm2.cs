﻿using System;
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
    public partial class EndingForm2 : Form
    {
        public EndingForm2()
        {
            InitializeComponent();
            label2.Hide();
            textBox1.Hide();
            button3.Hide();
            buyBtn.Hide();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        public void button3_Click(object sender, EventArgs e)
        {
            label2.Hide();
            textBox1.Hide();
            button3.Hide();
            buyBtn.Hide();
            label1.Show();
            angainBtn.Show();
            button2.Show();
        }

        public void Buy_Show()
        {
            label2.Show();
            textBox1.Show();
            button3.Show();
            buyBtn.Show();
            label1.Hide();
            angainBtn.Hide();
            button2.Hide();
        }

        Form1 form;
        public void Get(Form1 f)
        {
            form = f;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            form.networker.Send("buy;" + textBox1.Text.Trim());
        }
    }
}
