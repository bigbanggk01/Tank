using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Map
    {
        public int[,] Bit_map= new int[80,80];
        
        private Pen p;
        private SolidBrush sb;
        private Graphics g;
        Cons con = new Cons();
        public Map()
        {
            Map_Construct();
        }
        public void Wall_damged(Bullet b)
        {
            if (this.isWall(b.bulletX, b.bulletY) == true)
            {
                Bit_map[b.bulletX, b.bulletY] = 0;
            }
            else return;
        }
        
        public void Map_Construct()
        {
            for (int i = 0; i < 15; i++)
            {
                Bit_map[15, i] = 1;
                Bit_map[16, i] = 1;
                Bit_map[i, 16] = 1;
                Bit_map[i, 17] = 1;
            }
            for (int i = 18; i < 30; i++)
            {
                Bit_map[10, i] = 1;
                Bit_map[11, i] = 1;
            }
            for (int i = 12; i < 40; i++)
            {
                Bit_map[i, 28] = 1;
                Bit_map[i, 29] = 1;

            }
            for (int i = 0; i < 18; i++)
            {
                Bit_map[40, i] = 1;
                Bit_map[41, i] = 1;
                Bit_map[42, i] = 1;
            }
            for (int i = 30; i < 40; i++)
            {
                Bit_map[i, 16] = 1;
                Bit_map[i, 17] = 1;
            }
            for (int i = 16; i < 25; i++)
            {
                Bit_map[28, i] = 1;
                Bit_map[29, i] = 1;
            }
            for (int i = 44; i > 20; i--)
            {
                Bit_map[40, i] = 1;
                Bit_map[41, i] = 1;
                Bit_map[42, i] = 1;
            }
            for (int i = 43; i < 60; i++)
            {
                Bit_map[i, 25] = 1;
                Bit_map[i, 26] = 1;
            }
            for (int i = 0; i < 17; i++)
            {
                Bit_map[63, i] = 1;
                Bit_map[64, i] = 1;
            }
            for (int i = 79; i > 62; i--)
            {
                Bit_map[i, 19] = 1;
                Bit_map[i, 20] = 1;
            }
            for (int i = 21; i < 33; i++)
            {
                Bit_map[63, i] = 1;
                Bit_map[64, i] = 1;
            }
            for (int i = 62; i > 50; i--)
            {
                Bit_map[i, 31] = 1;
                Bit_map[i, 32] = 1;
            }
            for (int i = 30; i < 40; i++)
            {
                Bit_map[21, i] = 1;
                Bit_map[20, i] = 1;
            }
        }
        public void Draw(Form f)
        {
            p = new Pen(Color.Black);
            sb = new SolidBrush(Color.Black);
            g = f.CreateGraphics();
            for(int i =0;i<80;i++)
                for(int j = 0; j < 80; j++)
                {
                    if (Bit_map[i, j] == 1)
                    {
                        g.FillRectangle(sb, i * 20 + 1, j * 20 + 1, 18, 18);
                    }
                }
            g.Dispose();
            p.Dispose();
            sb.Dispose();
        }
        public bool isWall(int a, int b)
        {
            if (Bit_map[a, b] == 1) 
            { 
                return true; 
            }
            else { 
                return false;
            }
        }
    }
}
