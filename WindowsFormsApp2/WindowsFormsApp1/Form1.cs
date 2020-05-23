using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {   
        public Form1()
        {
            InitializeComponent();  
            networker = new Network();
            networker.GetForm(this);
            _player.Add(null);
            _player.Add(null);
        }
        
        ManualResetEvent Problem = new ManualResetEvent(false);
        private static Network networker;
        private static Tank tank = new Tank();
        private static Cons back_ground = new Cons();
        private System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        private  Map map = new Map();
        private  Map map2 = new Map();

        List<Tank> _player = new List<Tank>();
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            back_ground.Draw(this);
            tank.Draw(this);
            map.Draw(this);
            map2 = map;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                
                networker.Send("1");
            }
            if (e.KeyCode == Keys.Down)
            {
                
                networker.Send("2");
            }
            if (e.KeyCode == Keys.Left)
            {
                
                networker.Send("3");
            }
            if (e.KeyCode == Keys.Right)
            {
                
                networker.Send("4");
            }
            
            if(e.KeyCode == Keys.Space) 
            {
                
                networker.Send("5");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            networker.Start();
            _player.Add(tank);
            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            Label ChatBox = new Label();
            ChatBox.Location = new Point(this.Width - 290, 0);
            ChatBox.Size = new Size(300, Height - 500);
            ChatBox.Text = "Player chat box and information in this game";
            ChatBox.Font = new Font("Lucida Console", 10);
            
            Button SignOut = new Button();
            SignOut.Location = new Point(this.Width - 200, this.Height - 100);
            SignOut.Text = "Sign Out";
            SignOut.Size = new Size(150, 30);
            SignOut.TabStop = false;
            SignOut.Font = new Font("Lucida Console", 10);

            Button Tank_Buy = new Button();
            Tank_Buy.Location = new Point(this.Width - 200, this.Height - 150);
            Tank_Buy.Text = "Buy new tank";
            Tank_Buy.Size = new Size(150, 30);
            Tank_Buy.TabStop = false;
            Tank_Buy.Font = new Font("Lucida Console", 10);

            //Stop pressing button using keyboard
            Tank_Buy.DisableSelect();
            SignOut.DisableSelect();

            this.Controls.Add(SignOut);
            this.Controls.Add(Tank_Buy);
            this.Controls.Add(ChatBox);

            networker.Send("0");
        }
        public void Execute(object _currentData)
        {
            string s = _currentData as string;
            int[] b = s.Split(';').Select(int.Parse).ToArray();
            if (b[0] == 7)
            {
                networker.Send("8;" + _player[0].x[0].ToString() + ";" + _player[0].y[0].ToString()
                        + ";" + _player[0].Tank_Current_Status.ToString() + ";" + _player.IndexOf(_player[0]));
            }
            if (b[1] == 0)
            {
                if (b[0] == 0)
                {
                    _player[b[2]] = tank;
                    if (b[2] != 2) _player[2] = null;
                }

                if (b[0] == 6)
                {
                    Tank otherTank = new Tank();
                    Draw_Tank(otherTank);

                    foreach (Tank item in _player)
                    {
                        if (item == null)
                        {
                            _player[_player.IndexOf(item)] = otherTank;
                            return;
                        }
                    }
                }
                if (b[0] == 1) { _player[0].Go_Up(this, map); }
                if (b[0] == 2) { _player[0].Go_Down(this, map); }
                if (b[0] == 3) { _player[0].Go_Left(this, map); }
                if (b[0] == 4) { _player[0].Go_Right(this, map); }
                if (b[0] == 5)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        _player[0].Shot(this);
                        Bullet bull = new Bullet(_player[0], this);
                        bull.X_Map(map);
                        map.Wall_damged(bull);
                        bull.fly(this);
                    });
                }
            }
            if (b[1] == 1)
            {
                if (b[0] == 0)
                {
                    _player[b[2]] = tank;
                    _player[2] = null;
                }
                if (b[0] == 6)
                {
                    Tank otherTank = new Tank();
                    Draw_Tank(otherTank);
                    foreach (Tank item in _player)
                    {
                        if (item == null)
                        {
                            _player.Add(otherTank);
                            return;
                        }
                    }
                }
                if (b[0] == 1) { _player[1].Go_Up(this, map); }
                if (b[0] == 2) { _player[1].Go_Down(this, map); }
                if (b[0] == 3) { _player[1].Go_Left(this, map); }
                if (b[0] == 4) { _player[1].Go_Right(this, map); }
                if (b[0] == 5)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        _player[1].Shot(this);
                        Bullet bull = new Bullet(_player[1], this);
                        bull.X_Map(map);
                        map.Wall_damged(bull);
                        bull.fly(this);
                    });
                }
                if (b[0] == 8)
                {
                    _player[0] = new Tank();
                    _player[0].x[0] = b[2];
                    _player[0].y[0] = b[3];
                    if (b[4] == 1)
                    {
                        _player[0].Tank_Current_Status = 1;
                        _player[0].x[1] = b[2] - 1;
                        _player[0].y[1] = b[3] + 1;
                        _player[0].x[2] = b[2];
                        _player[0].y[2] = b[3] + 1;
                        _player[0].x[3] = b[2] + 1;
                        _player[0].y[3] = b[3] + 1;
                        _player[0].x[4] = b[2] - 1;
                        _player[0].y[4] = b[3] + 2;
                        _player[0].x[5] = b[2] + 1;
                        _player[0].y[5] = b[3] + 2;

                    }
                    if (b[4] == 2)
                    {
                        _player[0].Tank_Current_Status = 2;
                        _player[0].x[1] = b[2] + 1;
                        _player[0].y[1] = b[3] - 1;
                        _player[0].x[2] = b[2];
                        _player[0].y[2] = b[3] - 1;
                        _player[0].x[3] = b[2] - 1;
                        _player[0].y[3] = b[3] - 1;
                        _player[0].x[4] = b[2] + 1;
                        _player[0].y[4] = b[3] - 2;
                        _player[0].x[5] = b[2] - 1;
                        _player[0].y[5] = b[3] - 2;
                    }
                    if (b[4] == 3)
                    {
                        _player[0].Tank_Current_Status = 3;
                        _player[0].x[1] = b[2] + 1;
                        _player[0].y[1] = b[3] + 1;
                        _player[0].x[2] = b[2] + 1;
                        _player[0].y[2] = b[3];
                        _player[0].x[3] = b[2] + 1;
                        _player[0].y[3] = b[3] - 1;
                        _player[0].x[4] = b[2] + 2;
                        _player[0].y[4] = b[3] + 1;
                        _player[0].x[5] = b[2] + 2;
                        _player[0].y[5] = b[3] - 1;
                    }
                    if (b[4] == 4)
                    {
                        _player[0].Tank_Current_Status = 4;
                        _player[0].x[1] = b[2] - 1;
                        _player[0].y[1] = b[3] - 1;
                        _player[0].x[2] = b[2] - 1;
                        _player[0].y[2] = b[3];
                        _player[0].x[3] = b[2] - 1;
                        _player[0].y[3] = b[3] + 1;
                        _player[0].x[4] = b[2] - 2;
                        _player[0].y[4] = b[3] - 1;
                        _player[0].x[5] = b[2] - 2;
                        _player[0].y[5] = b[3] + 1;
                    }
                    Draw_Tank(_player[0]);
                }
            }
        }
        private int[] Receive_Tanks_Data(int x, int y, int tanksIndex)
        {
            int[] tankData = new int[3];
            return tankData;
        }

        private void Draw_Tank(Tank otherTank)
        {
            otherTank.Draw(this);
        }
    }


}
