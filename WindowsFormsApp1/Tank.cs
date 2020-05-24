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
    public class Tank
    {
        public static int GO_UP = 1;
        public static int GO_DOWN = -1;
        public static int GO_LEFT = 2;
        public static int GO_RIGHT = -2;
        public int Tank_Current_Status = GO_UP;
        public int a, b;
        public int[] x, y;
        public bool Died = false;

        Form1 form;
        public void GetForm(Form1 f)
        {
            form = f;
        }

        SolidBrush sb = new SolidBrush(Color.White);
        public Tank()
        {
            x = new int[6];
            y = new int[6];
            x[0] = 10;
            y[0] = 10;
            x[1] = 9;
            y[1] = 11;
            x[2] = 10;
            y[2] = 11;
            x[3] = 11;
            y[3] = 11;
            x[4] = 9;
            y[4] = 12;
            x[5] = 11;
            y[5] = 12;
        }
        
        public void Draw(Form f)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.White);
            Graphics g = f.CreateGraphics();
            g.FillRectangle(new SolidBrush(Color.Black), x[0] * 20 + 1, y[0] * 20 + 1, 18, 18);
            g.FillRectangle(new SolidBrush(Color.Black), x[1] * 20 + 1, y[1] * 20 + 1, 18, 18);
            g.FillRectangle(new SolidBrush(Color.Black), x[2] * 20 + 1, y[2] * 20 + 1, 18, 18);
            g.FillRectangle(new SolidBrush(Color.Black), x[3] * 20 + 1, y[3] * 20 + 1, 18, 18);
            g.FillRectangle(new SolidBrush(Color.Black), x[4] * 20 + 1, y[4] * 20 + 1, 18, 18);
            g.FillRectangle(new SolidBrush(Color.Black), x[5] * 20 + 1, y[5] * 20 + 1, 18, 18);
            g.Dispose();
            sb.Dispose();
        }
        /// <summary>
        /// Chuyển thành hướng lên
        /// </summary>
        /// <param name="_direction"></param>
        public void Redirected_To_Up(int _direction,Map m)
        {
            if (_direction == -1)
            {
                y[0] -= 3;
                y[2] -= 1;
                x[1] -= 2;
                y[1]--;
                x[3] += 2;
                y[3]--;
                x[4] -= 2;
                y[4]++;
                x[5] += 2;
                y[5]++;

            }
            if (_direction == 2)
            {
                x[0]++;
                y[0]--;
                x[1]--;
                y[1]--;
                x[4] -= 2;
                x[3]++;
                y[3]++;
                y[5] += 2;
                
            }
            if (_direction == -2)
            {
                x[0]--;
                y[0]--;
                x[1]--;
                y[1]++;
                y[4] += 2;
                x[3]++;
                y[3]--;
                x[5] += 2;
            }
        }
        /// <summary>
        /// Chuyển thành hướng qua phải
        /// </summary>
        /// <param name="_direction"></param>
        public void Redirected_To_Right(int _direction,Map m)
        {
            if (_direction == 2)
            {
                x[0] += 3;
                x[2]++;
                x[1]++;
                x[3]++;
                x[4] --;
                x[5] --;
                y[4] -= 2;
                y[1] -= 2;
                y[3] += 2;
                y[5] += 2;
                
            }
            if (_direction == 1)
            {
                x[0]++;
                y[0]++;
                x[1]++;
                y[1]--;
                y[4] -= 2;
                x[3]--;
                y[3]++;
                x[5]-=2;

            }
            if (_direction == -1)
            {

                x[0]++;
                y[0]--;
                x[1]--;
                y[1]--;
                x[4] -= 2;
                x[3]++;
                y[3]++;
                y[5] += 2;    
            }
        }
        /// <summary>
        /// Chuyển thành hướng qua trái
        /// </summary>
        /// <param name="_direction"></param>
        public void Redirected_To_Left(int _direction,Map m)
        {
            if (_direction == -2)
            {
                x[0] -= 3;
                x[2] -= 1;
                x[3] -= 1;
                x[1] -= 1;
                x[4] ++;
                x[5] ++;
                y[4] += 2;
                y[1] += 2;
                y[3] -= 2;
                y[5] -= 2;
            }
            if (_direction == 1)
            {
                x[0]--;
                y[0]++;
                x[1]++;
                y[1]++;
                x[4] += 2;
                x[3]--;
                y[3]--;
                y[5] -= 2;
                if (m.isWall(x[3] + 1, y[3]) == true || m.isWall(x[5] + 1, y[5]) == true)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        x[i]--;
                    }
                }

            }
            if (_direction == -1)
            {
                x[0]--;
                y[0]--;
                x[1]--;
                y[1]++;
                y[4] += 2;
                x[3]++;
                y[3]--;
                x[5] += 2;
                if (m.isWall(x[1] + 1, y[1]) == true || m.isWall(x[4] + 1, y[4]) == true)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        x[i]--;
                    }
                }
            }
        }
        /// <summary>
        /// Chuyển thành hướng xuống
        /// </summary>
        /// <param name="_direction"></param>
        public void Redirected_To_Down(int _direction,Map m)
        {
            if (_direction == 2)
            {
                x[0]++;
                y[0]++;
                x[1]++;
                y[1]--;
                y[4] -= 2;
                x[3]--;
                y[3]++;
                x[5] -= 2;
            }
            if (_direction == 1)
            {
                y[0] += 3;
                y[2] += 1;
                x[1] += 2;
                y[1]++;
                x[3] -= 2;
                y[3]++;
                x[4] += 2;
                y[4]--;
                x[5] -= 2;
                y[5]--;
            }
            if (_direction == -2)
            {
                x[0]--;
                y[0]++;
                x[1]++;
                y[1]++;
                x[4] += 2;
                x[3]--;
                y[3]--;
                y[5] -= 2;
            }
        }

        public void Go_Up(Form f, Map m)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.White);
            Graphics g = f.CreateGraphics();
            if ((y[0]) == 0||m.isWall(this.x[0],this.y[0]-1)==true || m.isWall(this.x[1], this.y[1] - 1) == true || m.isWall(this.x[3], this.y[3] - 1) == true)
            {
                return;
            }
            for (int i = 0; i < 6; i++)
            {
                g.FillRectangle(sb, x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);
            }
            Redirected_To_Up(Tank_Current_Status,m);
            Tank_Current_Status = GO_UP;
            if ((y[0] - 1) < 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    g.FillRectangle((new SolidBrush(Color.Black)), x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);

                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    y[i] = y[i] - 1;
                    g.FillRectangle((new SolidBrush(Color.Black)), x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);

                }
            }
            g.Dispose();
            sb.Dispose();
        }
        public void Go_Down(Form f,Map m)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.White);
            Graphics g = f.CreateGraphics();
            if (y[0] == 44 || m.isWall(this.x[0], this.y[0] + 1) == true ) { return; }
            for (int i = 0; i < 6; i++)
            {
                g.FillRectangle(sb, x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);
            }
            Redirected_To_Down(Tank_Current_Status,m);
            Tank_Current_Status = GO_DOWN;
            if (y[0] == 45 || m.isWall(this.x[0], this.y[0] + 1)  == true)
            {
                for (int i = 0; i < 6; i++)
                {
                    y[i] = y[i] - 1;
                    g.FillRectangle((new SolidBrush(Color.Black)), x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    y[i] = y[i] + 1;
                    g.FillRectangle((new SolidBrush(Color.Black)), x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);
                }
            }
            g.Dispose();
            sb.Dispose();
        }
        public void Go_Right(Form f,Map m)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.White);
            Graphics g = f.CreateGraphics();
            if(x[0] == 79||m.isWall(this.x[0] + 1, this.y[0]) == true ) { return; }
            for (int i = 0; i < 6; i++)
            {
                g.FillRectangle(sb,x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);
            }
            Redirected_To_Right(Tank_Current_Status,m);
            Tank_Current_Status = GO_RIGHT;
            if ((x[0] + 1) > 79)
            {
                for (int i = 0; i < 6; i++)
                {
                    g.FillRectangle((new SolidBrush(Color.Black)), x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    x[i]++;
                    g.FillRectangle((new SolidBrush(Color.Black)), x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);
                }
            }
            g.Dispose();
            sb.Dispose();
        }
        public void Go_Left(Form f, Map m)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.White);
            Graphics g = f.CreateGraphics();
            if (x[0] == 0 || m.isWall(x[0] - 1,y[0])==true)
            {
                return;
            }
            for (int i = 0; i < 6; i++)
            {
                g.FillRectangle(sb, x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);

            }
            Redirected_To_Left(Tank_Current_Status,m);
            Tank_Current_Status = GO_LEFT;
            
            if ((x[0] - 1) < 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    g.FillRectangle((new SolidBrush(Color.Black)), x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    x[i] = x[i] - 1;
                    g.FillRectangle((new SolidBrush(Color.Black)), x[i] * 20 + 1, y[i] * 20 + 1, 18, 18);
                }
            }
            g.Dispose();
            sb.Dispose();
        }
        
        /// <summary>
        /// Bắn viên đạn
        /// </summary>
        /// <param name="f"></param>
        public void Shot(Form f)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.White);
            Graphics g = f.CreateGraphics();
            
            if (Tank_Current_Status == GO_UP)
            {
                a = this.x[0];
                b = this.y[0]-1;
            }
            if (Tank_Current_Status == GO_DOWN)
            {
                a = this.x[0];
                b = this.y[0] + 1;
            }
            if (Tank_Current_Status == GO_RIGHT)
            {
                a = this.x[0]+1;
                b = this.y[0];
            }
            if (Tank_Current_Status == GO_LEFT)
            {
                a = this.x[0]-1;
                b = this.y[0] ;
            }
            g.FillRectangle((new SolidBrush(Color.Black)), a* 20 + 1, b * 20 + 1, 18, 18);
        }
        public bool isTank(int a, int b)
        {
            if (
                (a == x[0] && b == y[0]) || (a == x[1] && b == y[1]) 
                || (a == x[2] && b == y[2]) || (a == x[3] && b == y[4]) || (a == x[5] && b == y[5])
                ) { return true; }
            else { return false; }
        }
        public void Tank_damged(Bullet b)
        {
            if (this.isTank(b.bulletX, b.bulletY) == true)
            {
                form.Game_Over();
            }
            else return;
        }
    }
}
