using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        static ServerSocket server;

        public Form1()
        {
            InitializeComponent();
            server = new ServerSocket();
            server.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            foreach(Socket item in server._clientList)
            {
                server.Send("hello",item);
            }
        }
    }
}
