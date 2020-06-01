using System;
using System.Windows.Forms;
using System.Threading;
using System.Net.NetworkInformation;

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
            string query = "Select * from Usertable Where username = '" + textBox1.Text.Trim()
                + "' and passwo = '" + textBox2.Text.Trim()+"'";
            //NetworkInterfaceType type = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType type = NetworkInterfaceType.Ethernet;
            string Local = form.networker.GetLocalIP(type);
            form.networker.Send("Login;"+ query+";"+Local);
            form.networker.GetLogin(this);
            s = textBox1.Text;
            Thread.Sleep(500);
            label3.Text = "The password you entered \n\t\t is incorrect. Please try again.";
        }
        public void GetForm(Form1 f)
        {
            form = f;
        }
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            form.networker.Stop();
            form.Invoke((MethodInvoker)delegate
            {
                form.Close();
            });

        }
        public void CallBackToConnect()
        {
            string query = "Select * from Usertable Where username = '" + textBox1.Text.Trim()
                + "' and passwo = '" + textBox2.Text.Trim() + "'";
            //NetworkInterfaceType type = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType type = NetworkInterfaceType.Ethernet;
            string Local = form.networker.GetLocalIP(type);
            form.networker.Send("Login;" + query + ";" + Local, form.networker._client);
            s = textBox1.Text;
        }
    }
}
