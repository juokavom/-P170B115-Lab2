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
        private static int[,] A = new int[8, 8];
        private abstract class Figure
        {
            public int X;
            public int Y;
            public int Name;
            public List<int[]> possiblePaths;

            public void Move()
            {
                Random rnd = new Random();
                int move, x1, y1;
                do
                {
                    move = rnd.Next(0, possiblePaths.Count - 1);
                    x1 = possiblePaths[move][0];
                    y1 = possiblePaths[move][1];
                } while (A[x1, y1] != 0);
                A[X, Y] = 0;
                X = x1;
                Y = y1;
                A[X, Y] = Name;
            }
            public void drawPaths(int[,] B)
            {
                possiblePaths.ForEach(i => B[i[0], i[1]] = 9);
            }            
            public abstract void GeneratePaths();
        }
        private class King : Figure
        {
            public King(int x, int y, int name)
            {
                X = x;
                Y = y;
                Name = name;
            }

            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                if (X != 7 && Y != 7) possiblePaths.Add(new int[] { X + 1, Y + 1 });
                if (X != 0 && Y != 7) possiblePaths.Add(new int[] { X - 1, Y + 1 });
                if (X != 7 && Y != 0) possiblePaths.Add(new int[] { X + 1, Y - 1 });
                if (X != 0 && Y != 0) possiblePaths.Add(new int[] { X - 1, Y - 1 });
                if (X != 7) possiblePaths.Add(new int[] { X + 1, Y });
                if (X != 0) possiblePaths.Add(new int[] { X - 1, Y });
                if (Y != 7) possiblePaths.Add(new int[] { X, Y + 1 });
                if (Y != 0) possiblePaths.Add(new int[] { X, Y - 1 });
            }
        }
        private class Bishop : Figure
        {
            public Bishop(int x, int y, int name)
            {
                X = x;
                Y = y;
                Name = name;
            }

            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                if (X != 7 && Y != 7)
                {
                    int deltaX = 7 - X;
                    int deltaY = 7 - Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    for (int i = 1; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X + i, Y + i });
                    }
                }
                if (X != 0 && Y != 7)
                {
                    int deltaX = X;
                    int deltaY = 7 - Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    for (int i = 1; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X - i, Y + i });
                    }
                }
                if (X != 7 && Y != 0)
                {
                    int deltaX = 7 - X;
                    int deltaY = Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    for (int i = 1; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X + i, Y - i });
                    }
                }
                if (X != 0 && Y != 0)
                {
                    int deltaX = X;
                    int deltaY = Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    for (int i = 1; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X - i, Y - i });
                    }
                }
            }
        }
        private class Horse : Figure
        {
            public Horse(int x, int y, int name)
            {
                X = x;
                Y = y;
                Name = name;
            }

            public override void GeneratePaths()
            {
                throw new NotImplementedException();
            }
        }
        private class Rook : Figure
        {
            public Rook(int x, int y, int name)
            {
                X = x;
                Y = y;
                Name = name;
            }
            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                if (X != 7)
                {
                    for (int x1 = X+1; x1 <= 7; x1++)
                    {
                        possiblePaths.Add(new int[] { x1, Y });
                    }
                }
                if (X != 0)
                {
                    for (int x1 = X-1; x1 >= 0; x1--)
                    {
                        possiblePaths.Add(new int[] { x1, Y });
                    }
                }
                if (Y != 7)
                {
                    for (int y1 = Y+1; y1 <= 7; y1++)
                    {
                        possiblePaths.Add(new int[] { X, y1 });
                    }
                }
                if (Y != 0)
                {
                    for (int y1 = Y-1; y1 >= 0; y1--)
                    {
                        possiblePaths.Add(new int[] { X, y1 });
                    }
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
            for (int i = A.GetLength(1) - 1; i >= 0; i--)
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
            fillValues(A);
            List<Figure> blacks = new List<Figure>();
            blacks.Add(new King(4, 7, 1));
            blacks.Add(new Rook(7, 7, 2));
            blacks.Add(new Bishop(5, 7, 3));
            blacks.Add(new Horse(1, 7, 4));
            blacks.ForEach(figure => {A[figure.X, figure.Y] = figure.Name;});

            King whiteKing = new King(4, 0, 8);
            A[whiteKing.X, whiteKing.Y] = whiteKing.Name;

            printMatrix(A);
            King rook = (King)blacks[0];

            rook.GeneratePaths();
            for (int i = 0; i < 10; i++)
            {
                WriteLine("");
                rook.Move();
                rook.GeneratePaths();

                int[,] B = (int[,])A.Clone();
                rook.drawPaths(B);
                printMatrix(B);
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