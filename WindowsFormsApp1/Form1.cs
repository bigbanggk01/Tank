using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
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
        public Tank tank2 = new Tank(2);
        public ListView Online;
        public ListView Room;
        public Map map = new Map();
        public LoginForm LoginForm = new LoginForm();

        Cons back_ground = new Cons();
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        EndingForm e = new EndingForm();
        EndingForm2 e2 = new EndingForm2();
        ListViewNF ChatBox;
        public Label signal;
        public PictureBox image;
        Button SignOut;
        Button CreateRoom;
        Button Join;
        Button Invite;
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
                        textBox1.Hide();
                        button2.Hide();
                        if (textBox1.Text.Equals("") != true) 
                        {
                            EventArgs E = new EventArgs();
                            button2_Click(sender, E); 
                        }
                            
                        return;
                    }
                    textBox1.Show();
                    button2.Show();
                    textBox1.Focus();
                    textBox1.TabIndex = 1;
                }
                if (networker._client2 == null) return;
                if (e.KeyCode == Keys.Up)
                {
                    tank1.Go_Up(this,map);
                    try { networker.Send("press;1", networker._client2); }
                    catch (Exception D){ MessageBox.Show(D.ToString()); }
                
                }
                if (e.KeyCode == Keys.Down)
                {
                    tank1.Go_Down(this, map);
                    networker.Send("press;2", networker._client2);
                }
                if (e.KeyCode == Keys.Left)
                {
                    tank1.Go_Left(this, map);
                    networker.Send("press;3", networker._client2);
                }
                if (e.KeyCode == Keys.Right)
                {
                    tank1.Go_Right(this, map);
                    networker.Send("press;4", networker._client2);
                }
                
                if (e.KeyCode == Keys.Space)
                {
                    tank1.Shot(this);
                    Bullet b = new Bullet(tank1, this);
                    b.GetForm(this);
                    b.fly(this);
                    networker.Send("press;5", networker._client2);
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
                    networker.Send("press;1", networker._client1);
                }
                if (e.KeyCode == Keys.Down)
                {
                    tank2.Go_Down(this, map);
                    networker.Send("press;2", networker._client1);
                }
                if (e.KeyCode == Keys.Left)
                {
                    tank2.Go_Left(this, map);
                    networker.Send("press;3", networker._client1);
                }
                if (e.KeyCode == Keys.Right)
                {
                    tank2.Go_Right(this, map);
                    networker.Send("press;4", networker._client1);
                }
                if (e.KeyCode == Keys.Space)
                {
                    tank2.Shot(this);
                    Bullet b = new Bullet(tank2, this);
                    b.GetForm(this);
                    b.fly(this);
                    networker.Send("press;5", networker._client1);
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

            ChatBox = new ListViewNF();
            ChatBox.Location = new Point(this.Width - 320, -9);
            ChatBox.Size = new Size(300, Height - 500);
            ChatBox.View = View.List;
            ChatBox.Font = new Font("Lucida Console", 10);
            ChatBox.Anchor = AnchorStyles.Right;
            ChatBox.BorderStyle = BorderStyle.FixedSingle;
            ChatBox.CausesValidation = false;
            
            Online = new ListView();
            Online.Location = new Point(Width - 320, Height-490);
            Online.Size = new Size(300, 353);
            Online.View = View.List;
            Online.Font = new Font("Lucida Console", 10);
            Online.Anchor = AnchorStyles.Right;
            Online.BorderStyle = BorderStyle.FixedSingle;

            Room = new ListView();
            Room.Location = new Point(290, -9);
            Room.Size = new Size(1300, 901);
            Room.View = View.List;
            Room.Font = new Font("Lucida Console", 10);
            Room.Anchor = AnchorStyles.Right;
            Room.Click += new System.EventHandler(this.Room_Click);
            Room.View = View.List;
            Room.BorderStyle = BorderStyle.FixedSingle;

            SignOut = new Button();
            SignOut.Location = new System.Drawing.Point(this.Width - 160, this.Height - 100);
            SignOut.Text = "Sign Out";
            SignOut.Size = new System.Drawing.Size(150, 30);
            SignOut.TabStop = false;
            SignOut.Font = new System.Drawing.Font("Lucida Console", 10);
            SignOut.Anchor = AnchorStyles.Bottom;
            SignOut.Click += new System.EventHandler(this.SignOut_Click);
            SignOut.FlatStyle = FlatStyle.Flat;

            CreateRoom = new Button();
            CreateRoom.Location = new Point(this.Width - 160, this.Height - 140);
            CreateRoom.Text = "New room";
            CreateRoom.Size = new Size(150, 30);
            CreateRoom.TabStop = false;
            CreateRoom.Font = new Font("Lucida Console", 10);
            CreateRoom.Anchor = AnchorStyles.Bottom;
            CreateRoom.Click += new System.EventHandler(this.CreateRoom_Click);
            CreateRoom.FlatStyle = FlatStyle.Flat;

            Join = new Button();
            Join.Location = new Point(this.Width - 312, this.Height - 100);
            Join.Text = "Joint room";
            Join.Size = new Size(150, 30);
            Join.TabStop = false;
            Join.Font = new Font("Lucida Console", 10);
            Join.Anchor = AnchorStyles.Bottom;
            Join.Click += new System.EventHandler(this.Joint_Click);
            Join.FlatStyle = FlatStyle.Flat;
            Invite = new Button();
            Invite.Location = new Point(this.Width - 312, this.Height - 140);
            Invite.Text = "Invite";
            Invite.Size = new Size(150, 30);
            Invite.TabStop = false;
            Invite.Font = new Font("Lucida Console", 10);
            Invite.Anchor = AnchorStyles.Bottom;
            Invite.Click += new System.EventHandler(this.Invite_Click);
            Invite.FlatStyle = FlatStyle.Flat;

            CreateRoom.FlatStyle = FlatStyle.Flat;
            image = new PictureBox();
            image.Location= new Point(5, 0);
            image.Size = new Size(300,901);
            image.Name = "Image";
            //image.SizeMode = PictureBoxSizeMode.CenterImage;
            image.ImageLocation = @"image.jpg";

            signal = new Label();
            signal.Location = new Point(5, 840);
            signal.Text = "Khuyến mãi 50% giá trị thẻ nạp cho sv UIT";
            signal.Size = new Size(230, 50);
            signal.TabStop = false;
            signal.Font = new Font("Lucida Console", 10);
            signal.Anchor = AnchorStyles.Bottom;

            //Stop pressing button using keyboard
            SignOut.DisableSelect();
            button2.DisableSelect();
            CreateRoom.DisableSelect();
            Join.DisableSelect();
            Invite.DisableSelect();
            signal.DisableSelect();

            this.Controls.Add(signal);
            this.Controls.Add(image);
            this.Controls.Add(Invite);
            this.Controls.Add(Join);
            this.Controls.Add(SignOut);
            this.Controls.Add(ChatBox);
            this.Controls.Add(Online);
            this.Controls.Add(CreateRoom);
            this.Controls.Add(Room);
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
                    networker.Send("gameover;lose");
                    e2.Show();
                    e2.Get(this);
                }
                if (tank2.Died == true)
                {
                    networker.Send("gameover;win");
                    e.Show();
                    e.Get(this);
                }
            }
            if (networker._identification == 1)
            {
                if (tank1.Died == true)
                {
                    networker.Send("gameover;win");
                    e.Show();
                    e.Get(this);
                }
                if (tank2.Died == true)
                {
                    networker.Send("gameover;lose");
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
            textBox1.Hide();
            button2.Hide();
            var row = new ListViewItem("You: "+textBox1.Text);
            ChatBox.BeginUpdate();
            ChatBox.Items.Add(row);
            ChatBox.DisableSelect();
            ChatBox.EndUpdate();
            if (networker._identification == 0) { networker.Send("message;" + textBox1.Text, networker._client2); }
            else if(networker._identification == 1) { networker.Send("message;" + textBox1.Text, networker._client1); }
            textBox1.Clear();
            textBox1.DisableSelect();
        }

        public void EnemyChat_Show(string enemyMessage)
        {
            var row = new ListViewItem("Enemy: " + enemyMessage);
            ChatBox.Items.Add(row);
            ChatBox.DisableSelect();
        }

        private void SignOut_Click(object sender, EventArgs e)
        {
            networker.Send("disconnect");
            this.Close();
        }
        public TextBox RoomName;
        public Button Create;
        public TextBox Title;
        public Label label1;
        public Label label2;
        private void CreateRoom_Click(object sender, EventArgs e)
        {
            Room.Hide();
            image.Dispose();
            signal.Dispose();
            label1 = new Label();
            label1.Font = new System.Drawing.Font("Lucida Console", 15);
            label1.Location = new System.Drawing.Point(10, 100);
            label1.Text = "Room's name: ";
            label1.Size = new System.Drawing.Size(200, 20);
            label1.TabIndex = 0;
            RoomName = new TextBox();
            RoomName.Font = new System.Drawing.Font("Lucida Console", 15);
            RoomName.Location = new System.Drawing.Point(250, 100);
            RoomName.Multiline = false;
            RoomName.Name = "RoomName";
            RoomName.Size = new System.Drawing.Size(1000, 20);
            RoomName.TabIndex = 0;
            RoomName.BorderStyle = BorderStyle.FixedSingle;
            
            label2 = new Label();
            label2.Font = new System.Drawing.Font("Lucida Console", 15);
            label2.Location = new System.Drawing.Point(10, 150);
            label2.Text = "Title";
            label2.Size = new System.Drawing.Size(100, 20);
            label2.TabIndex = 0;
            
            Title = new TextBox();
            Title.Font = new System.Drawing.Font("Lucida Console", 15);
            Title.Location = new System.Drawing.Point(250, 150);
            Title.Multiline = false;
            Title.Name = "RoomName";
            Title.Size = new System.Drawing.Size(1000, 20);
            Title.TabIndex = 0;
            Title.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(Title);
            this.Controls.Add(label2);
            this.Controls.Add(RoomName);
            this.Controls.Add(label1);
            Create = new Button();
            Create.Location = new Point(1260, 100);
            Create.Text = "Create";
            Create.Size = new Size(100, 30);
            Create.TabStop = false;
            Create.Font = new Font("Lucida Console", 10);
            Create.Anchor = AnchorStyles.Bottom;
            Create.FlatStyle = FlatStyle.Flat;
            Create.Click += new System.EventHandler(this.Create_Click);
            Controls.Add(Create);
            if (RoomName.Text.Equals("") == false) 
            { 
                networker.Send("create;"+RoomName.Text+";"+Title.Text); 
            }            
        }
        private void Create_Click(object sender, EventArgs e)
        {
            if (RoomName.Text.Equals("") == false)
            {
                for(int i=0; i< Room.Items.Count; i++)
                {
                    if (Room.Items.ToString().Equals(RoomName.Text) == true)
                    {
                        MessageBox.Show("Room name already exists");
                        return;
                    }
                }
                string s = "create;" + RoomName.Text + ";" + Title.Text;
                RoomName.Dispose();
                label1.Dispose();
                label2.Dispose();
                Title.Dispose();
                Create.Dispose();
                MessageBox.Show("Waiting enemy!");
                networker.Send(s);
                
            }
            else
            {
                MessageBox.Show("Room name not entered");
                return;
            }
            CreateRoom.Enabled = false;
        }
        public void StartGame()
        {
            back_ground.Draw(this);
            tank1.Draw(this);
            tank1.GetForm(this);
            tank2.Draw(this);
            tank2.GetForm(this);
            map.Draw(this);
            networker.myTank = tank1;
            networker.enemyTank = tank2;
            networker._identification = 0;
        }
        public void StartGame2()
        {
            back_ground.Draw(this);
            tank1.Draw(this );
            tank1.GetForm(this);
            tank2.Draw(this);
            tank2.GetForm(this);
            map.Draw(this);
            networker.myTank = tank2;
            networker.enemyTank = tank1;
            networker._identification = 1;
        }
        private void Joint_Click(object sender, EventArgs e)
        {

        }
        private void Invite_Click(object sender, EventArgs e)
        {

        }
        private void Room_Click(object sender, EventArgs e)
        {
            image.Dispose();
            signal.Dispose();
            string s = Room.SelectedItems[0].Text;
            Room.Dispose();
            
            char[] b = { '.' };
            Int32 count = 100;
            String[] strList = s.Split(b, count, StringSplitOptions.RemoveEmptyEntries);
            networker.Send("joint;" + strList[0]);
        }
        public void Form1_Paint(object sender, PaintEventArgs e)
        {
            back_ground.Draw(this,e);
            tank1.Draw(this,e);
            tank1.GetForm(this);
            tank2.Draw(this,e);
            tank2.GetForm(this);
            map.Draw(this,e);
            networker.myTank = tank1;
            networker.enemyTank = tank2;
            networker._identification = 0;
        }

        public void Form1_Paint2(object sender, PaintEventArgs e)
        {
            back_ground.Draw(this,e);
            tank1.Draw(this,e);
            tank1.GetForm(this);
            tank2.Draw(this,e);
            tank2.GetForm(this);
            map.Draw(this,e);
            networker.myTank = tank2;
            networker.enemyTank = tank1;
            networker._identification = 1;
        }
    }
    class ListViewNF : System.Windows.Forms.ListView
    {
        public ListViewNF()
        {
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
    }
}
