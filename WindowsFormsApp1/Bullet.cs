using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing; 

namespace WindowsFormsApp1
{
    /// <summary>
    /// Class viên đạn
    /// </summary>
    class Bullet
    {

        public int bulletX;
        public int bulletY;
        private int bulletDirection; 
        private Timer timer;
        private Tank tank;
        private Pen p;
        private SolidBrush sb;
        private Graphics g;
        public Bullet(Tank t,Form f)
        {
            bulletX = t.a;
            bulletY = t.b;
            tank = t;
            bulletDirection = tank.Tank_Current_Status;
            p = new Pen(Color.Black);
            sb = new SolidBrush(Color.White);
            g = f.CreateGraphics();
            timer = new Timer();
            timer.Tick += new System.EventHandler(t_tick);
            timer.Interval = 20;
        }
        /// <summary>
        /// Mỗi viên đạn gắn với 1 timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void t_tick(object sender, EventArgs e)
        {
            
            if (bulletDirection == 1)
            {
                if (bulletY == 0) 
                {
                    g.FillRectangle(sb, bulletX * 20 + 1, bulletY * 20 + 1, 18, 18);
                    timer.Stop();
                    timer.Dispose();
                    return;
                }
                g.FillRectangle(sb, bulletX*20 + 1, bulletY * 20 + 1, 18, 18);
                bulletY--;
                g.FillRectangle((new SolidBrush(Color.Black)), bulletX * 20 + 1, bulletY * 20 + 1, 18, 18);
            }
            if (bulletDirection == -1)
            {
                if (bulletY == 44) 
                {
                    g.FillRectangle(sb, bulletX * 20 + 1, bulletY * 20 + 1, 18, 18);
                    timer.Stop();
                    timer.Dispose();
                    return;
                }
                g.FillRectangle(sb, bulletX*20 + 1, bulletY * 20 + 1, 18, 18);
                bulletY++;
                g.FillRectangle((new SolidBrush(Color.Black)), bulletX * 20 + 1, bulletY * 20 + 1, 18, 18);
            }
            if (bulletDirection == 2)
            {
                if (bulletX == 0)
                {
                    g.FillRectangle(sb, bulletX * 20 + 1, bulletY * 20 + 1, 18, 18);
                    timer.Stop();
                    timer.Dispose();
                    return;

                }
                g.FillRectangle(sb, bulletX*20 + 1, bulletY * 20 + 1, 18, 18);
                bulletX--;
                g.FillRectangle((new SolidBrush(Color.Black)), bulletX * 20 + 1, bulletY * 20 + 1, 18, 18);
            }
            if (bulletDirection == -2)
            {
                if (bulletX == 79)
                {
                    g.FillRectangle(sb, bulletX * 20 + 1, bulletY * 20 + 1, 18, 18);
                    timer.Stop();
                    timer.Dispose();
                    return;
                }
                g.FillRectangle(sb, bulletX*20 + 1, bulletY * 20 + 1, 18, 18);
                bulletX++;
                g.FillRectangle((new SolidBrush(Color.Black)), bulletX * 20 + 1, bulletY * 20 + 1, 18, 18);
            }
        }

        /// <summary>
        /// Viên đạn bay :v
        /// </summary>
        /// <param name="f"></param>
        public void fly(Form f)
        {
            timer.Start();
        }
    }
}
