//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//Egzamino užduotis

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pvz1
{
    public partial class Form1 : Form
    {
        private abstract class Figure
        {
            public int X;
            public int Y;
            public abstract void Move();
        }
        private class Rook : Figure
        {
            public Rook(int x, int y)
            {
                X = x;
                Y = y;
            }
            public override void Move()
            {
                List<int> list = new List<int>();
                if (X != 7) list.Add(6);
                if (X != 0) list.Add(4);
                if (Y != 7) list.Add(8);
                if (Y != 0) list.Add(2);
                Random rnd = new Random();
                int move = rnd.Next(0, list.Count);
                System.Diagnostics.Debug.WriteLine(string.Format("Rnd:{0}, list.Count: {1}, list[rnd]: {2}.", move, list.Count, list[move]));
                switch (list[move])
                {
                    case 6:
                        int length = 7 - X;
                        int step = rnd.Next(1, length);
                        X += step;
                        break;
                    case 4:
                        length = X;
                        step = rnd.Next(1, length);
                        X -= step;
                        break;
                    case 8:
                        length = 7 - Y;
                        step = rnd.Next(1, length);
                        Y += step;
                        break;
                    case 2:
                        length = Y;
                        step = rnd.Next(1, length);
                        Y -= step;
                        break;
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
            Initialize();

            Solution();
        }
        Series z1, z2, p1;
        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm1();
            //---
            richTextBox1.AppendText("Paleidimas\n");
            //---
            /*PictureBox pb = new PictureBox();
            pb.Location = new Point(0, 0);
            pb.Size = new Size(400, 400);
            pb.Image = Image.FromFile(@"Images/WK.jpg");
            pb.Visible = true;
            chart1.Controls.Add(pb);
            */
            //---
            //---
            //---
            //---
        }
        private void fillValues(int[,] A)
        {
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int u = 0; u < A.GetLength(1); u++)
                {
                    A[i, u] = 0;
                }
            }

        } 
        private void printMatrix(int[,] A)
        {
            for (int i = A.GetLength(1) - 1; i >= 0 ; i--)
            {
                for (int u = 0; u < A.GetLength(0); u++)
                {
                    Write(A[u, i].ToString());
                }
                WriteLine("");
            }

        }
        private void Write(string text)
        {
            System.Diagnostics.Debug.Write(text);
        }
        private void WriteLine(string text)
        {
            Write(text + "\n");
        }
       
        private void Solution()
        {
            int[,] A = new int[8, 8];
            fillValues(A);
            Rook rook = new Rook(7, 7);

            A[4, 0] = 8;
            A[4, 7] = 1;
            A[5, 7] = 3;
            A[7, 7] = 2;
            A[1, 7] = 4;

            printMatrix(A);

            for (int i = 0; i < 10; i++)
            {
                A[rook.X, rook.Y] = 0;
                rook.Move();
                A[rook.X, rook.Y] = 2;
                printMatrix(A);
                WriteLine("");
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            ClearForm1();
        }
        public void ClearForm1()
        {
            richTextBox1.Clear();
            chart1.Series.Clear();
        }
    }
}