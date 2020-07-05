using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace WindowsFormsApp1
{
    class Cons
    {
        public void Draw(Form f,PaintEventArgs e)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.White);
            e.Graphics.DrawRectangle(p, 0, 0, 1600, 900);
            for (int i = 0; i < 80; i++)
                for (int j = 0; j < 45; j++)
                {
                    e.Graphics.FillRectangle(sb, i * 20 + 1, j * 20 + 1, 18, 18);
                }
            p.Dispose();
            sb.Dispose();
        }
        public void Draw(Form f)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.White);
            Graphics g = f.CreateGraphics();
            g.DrawRectangle(p, 0, 0, 1600, 900);
            for (int i = 0; i < 80; i++)
                for (int j = 0; j < 45; j++)
                {
                    g.FillRectangle(sb, i * 20 + 1, j * 20 + 1, 18, 18);
                }
            g.Dispose();
            p.Dispose();
            sb.Dispose();
        }
    }
}
