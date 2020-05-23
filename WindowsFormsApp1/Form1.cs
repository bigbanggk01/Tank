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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //tạo form full màn hình 
            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            networker= new Network();
        }
        Network networker;
        Tank tank = new Tank();
        Cons back_ground = new Cons();
        Timer t = new Timer();
        Map map = new Map();
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            back_ground.Draw(this);
            tank.Draw(this);
            map.Draw(this);
        }

        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                tank.Go_Up(this, map);
                networker.Send("1");
            }
            if (e.KeyCode == Keys.Down)
            {
                tank.Go_Down(this,map );
                networker.Send("2");
            }
            if (e.KeyCode == Keys.Right)
            {
                tank.Go_Right(this,map);
                networker.Send("3");
            }
            if (e.KeyCode == Keys.Left)
            {
                tank.Go_Left(this,map);
                networker.Send("4");
            }
            if(e.KeyCode == Keys.Space) 
            {
                tank.Shot(this);
                Bullet b = new Bullet(tank, this);
                
                b.fly(this);
                networker.Send("5");
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
                networker.Send("0");
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

            networker.GetForm(this);
            networker.GetMap(map);
        }    
      
    }


}
