using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
            //tạo form full màn hình 
            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            networker= new Network();
            networker.GetForm(this);
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            LoginForm.Show();
            LoginForm.GetForm(this);
        }

        public Network networker;
        public Tank tank1 = new Tank();
        public Tank tank2 = new Tank();
        Cons back_ground = new Cons();
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        public Map map = new Map();
        EndingForm e = new EndingForm();
        EndingForm2 e2 = new EndingForm2();
        LoginForm LoginForm = new LoginForm();
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
                //networker.Send("0");
            }
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
                    });                }
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
    }


}
