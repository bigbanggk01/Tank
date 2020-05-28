using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //tạo form full màn hình 
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width = screen.Width; //>= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height = screen.Height;// >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            
            networker = new Network();
            networker.GetForm(this);
            LoginForm.GetForm(this);
            LoginForm.Show();
        }
        public Form1(int a) 
        {
            if (a == 1)
            {
                InitializeComponent();
            }
            if (a == 2)
            {
                InitializeComponent();
                //tạo form full màn hình 
                Rectangle screen = Screen.PrimaryScreen.WorkingArea;
                int w = Width = screen.Width; //>= screen.Width ? screen.Width : (screen.Width + Width) / 2;
                int h = Height = screen.Height;// >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
                this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
                this.Size = new Size(w, h);
                button2.Hide();
                textBox1.Hide();
                button2.Anchor = AnchorStyles.Bottom;
                textBox1.Anchor = AnchorStyles.Bottom;
                networker = new Network();
                networker.GetForm(this);
                networker.Start();
                this.WindowState = FormWindowState.Maximized;
                this.ShowInTaskbar = true;

            }
        }
        
        public Network networker;
        public Tank tank1 = new Tank();
        public Tank tank2 = new Tank();
        Cons back_ground = new Cons();
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        public Map map = new Map();
        EndingForm e = new EndingForm();
        EndingForm2 e2 = new EndingForm2();
        public LoginForm LoginForm = new LoginForm();
        ListView ChatBox;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            back_ground.Draw(this);
            tank1.Draw(this);
            tank1.GetForm(this);
            tank2.x[0] = 10 + 60;
            tank2.y[0] = 10 + 30;
            tank2.x[1] = 9 + 60;
            tank2.y[1] = 11 + 30;
            tank2.x[2] = 10 + 60;
            tank2.y[2] = 11 + 30;
            tank2.x[3] = 11 + 60;
            tank2.y[3] = 11 + 30;
            tank2.x[4] = 9 + 60;
            tank2.y[4] = 12 + 30;
            tank2.x[5] = 11 + 60;
            tank2.y[5] = 12 + 30;
            tank2.Draw(this);
            tank2.GetForm(this);
            map.Draw(this);
        }
        /// <summary>
        /// Các nút bấm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (networker._identification == 0)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (textBox1.Visible == true)
                    {
                        if (textBox1.Text.Equals("") == true) return;
                        EventArgs E = new EventArgs();
                        textBox1.Hide();
                        button2.Hide();
                        button2_Click(sender, E);
                        textBox1.Clear();
                        textBox1.DisableSelect();
                        networker.Send("6", networker.client2);
                        return;
                    }
                    textBox1.Show();
                    button2.Show();
                    textBox1.Focus();
                    textBox1.TabIndex = 1;
                }
                if (networker.client2 == null) return;
                if (e.KeyCode == Keys.Up)
                {
                    tank1.Go_Up(this,map);
                    networker.Send("1",networker.client2);
                }
                if (e.KeyCode == Keys.Down)
                {
                    tank1.Go_Down(this, map);
                    networker.Send("2", networker.client2);
                }
                if (e.KeyCode == Keys.Left)
                {
                    tank1.Go_Left(this, map);
                    networker.Send("3", networker.client2);
                }
                if (e.KeyCode == Keys.Right)
                {
                    tank1.Go_Right(this, map);
                    networker.Send("4", networker.client2);
                }
                
                if (e.KeyCode == Keys.Space)
                {
                    tank1.Shot(this);
                    Bullet b = new Bullet(tank1, this);
                    b.GetForm(this);
                    b.fly(this);
                    networker.Send("5", networker.client2);
                }
            }
            if (networker._identification == 1)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (textBox1.Visible == true)
                    {
                        if (textBox1.Text.Equals("") == true) return;
                        EventArgs E = new EventArgs();
                        textBox1.Hide();
                        button2.Hide();
                        button2_Click(sender, E);
                        textBox1.Clear();
                        textBox1.DisableSelect();
                        networker.Send("6");
                        return;
                    }
                    textBox1.Show();
                    button2.Show();
                    textBox1.Focus();
                    textBox1.TabIndex = 1;
                }
                if (e.KeyCode == Keys.Up)
                {
                    tank2.Go_Up(this, map);
                    networker.Send("1");
                }
                if (e.KeyCode == Keys.Down)
                {
                    tank2.Go_Down(this, map);
                    networker.Send("2");
                }
                if (e.KeyCode == Keys.Left)
                {
                    tank2.Go_Left(this, map);
                    networker.Send("3");
                }
                if (e.KeyCode == Keys.Right)
                {
                    tank2.Go_Right(this, map);
                    networker.Send("4");
                }
                if (e.KeyCode == Keys.Space)
                {
                    tank2.Shot(this);
                    Bullet b = new Bullet(tank2, this);
                    b.GetForm(this);
                    b.fly(this);
                    networker.Send("5");
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (networker.Start() == false)
            {
                this.Close();
            }
            else
            {
                
            }

            button2.Hide();
            textBox1.Hide();
            button2.Anchor = AnchorStyles.Bottom;
            textBox1.Anchor = AnchorStyles.Bottom;

            ChatBox = new ListView();
            ChatBox.Location = new Point(this.Width - 320, 0);
            ChatBox.Size = new Size(300, Height - 500);
            ChatBox.View = View.List;
            ChatBox.Font = new Font("Lucida Console", 10);
            ChatBox.Anchor = AnchorStyles.Right;

            Button SignOut = new Button();
            SignOut.Location = new Point(this.Width - 200, this.Height - 100);
            SignOut.Text = "Sign Out";
            SignOut.Size = new Size(150, 30);
            SignOut.TabStop = false;
            SignOut.Font = new Font("Lucida Console", 10);
            SignOut.Anchor = AnchorStyles.Bottom;
            
            Button Tank_Buy = new Button();
            Tank_Buy.Location = new Point(this.Width - 200, this.Height - 150);
            Tank_Buy.Text = "Buy new tank";
            Tank_Buy.Size = new Size(150, 30);
            Tank_Buy.TabStop = false;
            Tank_Buy.Font = new Font("Lucida Console", 10);
            Tank_Buy.Anchor = AnchorStyles.Bottom;
            //Stop pressing button using keyboard
            Tank_Buy.DisableSelect();
            SignOut.DisableSelect();
            button2.DisableSelect();
            this.Controls.Add(SignOut);
            this.Controls.Add(Tank_Buy);
            this.Controls.Add(ChatBox);

            this.ShowInTaskbar = false;
            this.Hide();
        }    
        public void GetForm(LoginForm lf)
        {
            LoginForm = lf;
        }
        /// <summary>
        /// Cập nhật tình trạng xe của đối thủ, hành động của đối thủ
        /// </summary>
        /// <param name="instruction"></param>
        public void Enemy_Control(int instruction)
        {
            if(instruction == 1)
            {
                if (networker._identification == 0)
                {
                    tank2.Go_Up(this,map);
                }
                if (networker._identification == 1)
                {
                    tank1.Go_Up(this, map);
                }
            }
            if (instruction == 2)
            {
                if (networker._identification == 0)
                {
                    tank2.Go_Down(this, map);
                }
                if (networker._identification == 1)
                {
                    tank1.Go_Down(this, map);
                }
            }
            if (instruction == 3)
            {
                if (networker._identification == 0)
                {
                    tank2.Go_Left(this, map);
                }
                if (networker._identification == 1)
                {
                    tank1.Go_Left(this, map);
                }
            }
            if (instruction == 4)
            {
                if (networker._identification == 0)
                {
                    tank2.Go_Right(this, map);
                }
                if (networker._identification == 1)
                {
                    tank1.Go_Right(this, map);
                }
            }
            if (instruction == 5)
            {
                if (networker._identification == 0)
                {
                    this.Invoke((MethodInvoker)delegate {
                        tank2.Shot(this);
                        Bullet b = new Bullet(tank2, this);
                        b.fly(this);
                        b.GetForm(this);
                    });
                }
                if (networker._identification == 1)
                {
                    this.Invoke((MethodInvoker)delegate {
                        tank1.Shot(this);
                        Bullet b = new Bullet(tank1, this);
                        b.fly(this);
                        b.GetForm(this);
                    });               
                }
                
            }
        }
        /// <summary>
        /// Xử lý kết thúc
        /// </summary>
        public void Game_Over()
        {
            if (networker._identification == 0)
            {
                if (tank1.Died == true)
                {
                    e2.Show();
                    e2.Get(this);
                    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    client.Connect(IPAddress.Parse("127.0.0.1"), 11000);
                    //client.Send(Serialize("update;update usertable set tank='0' where username='" + LoginForm.s + "'"));
                }
                if (tank2.Died == true)
                {
                    e.Show();
                    e.Get(this);
                }
            }
            if (networker._identification == 1)
            {
                if (tank1.Died == true)
                {
                    e.Show();
                    e.Get(this);
                }
                if (tank2.Died == true)
                {
                    e2.Show();
                    e2.Get(this);
                }
            }
        }
        byte[] Serialize(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);
            return ms.ToArray();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var row = new ListViewItem(textBox1.Text);
            ChatBox.Items.Add(row);
            ChatBox.DisableSelect();
        }
    }


}
