//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//Egzamino užduotis

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pvz1
{
    public partial class Form1 : Form
    {
        private static int[,] A = new int[8, 8];
        private static PictureBox[,] pbErr = new PictureBox[8, 8];
        private static List<Figure> blacks;
        private static List<int[]> PATHS;

        private abstract class Figure
        {
            public int X;
            public int Y;
            public int Name;
            public List<int[]> possiblePaths;
            public PictureBox pb;

            public static void imageLocation(int x, int y, PictureBox pb)
            {
                pb.Location = new Point(5 + x * 50, 355 - y * 50);
            }
            public void addImage(string image)
            {
                pb = new PictureBox();
                imageLocation(X, Y, pb);
                pb.Size = new Size(40, 40);
                pb.Image = Image.FromFile(image);
                pb.Visible = true;
            }
            public void Move()
            {
                Random rnd = new Random();
                int move, x1, y1;
                do
                {
                    move = rnd.Next(0, possiblePaths.Count);
                    x1 = possiblePaths[move][0];
                    y1 = possiblePaths[move][1];
                } while (A[x1, y1] != 0);
                A[X, Y] = 0;
                X = x1;
                Y = y1;
                A[X, Y] = Name;
                imageLocation(X, Y, pb);
            }
            public abstract void GeneratePaths();
        }
        private class King : Figure
        {
            public King(int x, int y, int name, string image)
            {
                X = x;
                Y = y;
                Name = name;
                addImage(image);
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
            public Bishop(int x, int y, int name, string image)
            {
                X = x;
                Y = y;
                Name = name;
                addImage(image);
            }

            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                if (X != 7 && Y != 7)
                {
                    int deltaX = 7 - X;
                    int deltaY = 7 - Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    for (int i = 0; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X + i, Y + i });
                    }
                }
                if (X != 0 && Y != 7)
                {
                    int deltaX = X;
                    int deltaY = 7 - Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    for (int i = 0; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X - i, Y + i });
                    }
                }
                if (X != 7 && Y != 0)
                {
                    int deltaX = 7 - X;
                    int deltaY = Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    for (int i = 0; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X + i, Y - i });
                    }
                }
                if (X != 0 && Y != 0)
                {
                    int deltaX = X;
                    int deltaY = Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    for (int i = 0; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X - i, Y - i });
                    }
                }
            }
        }
        private class Horse : Figure
        {
            public Horse(int x, int y, int name, string image)
            {
                X = x;
                Y = y;
                Name = name;
                addImage(image);
            }

            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                if (X < 7 && Y < 6) possiblePaths.Add(new int[] { X + 1, Y + 2 });
                if (X < 6 && Y < 7) possiblePaths.Add(new int[] { X + 2, Y + 1 });
                if (X > 0 && Y < 6) possiblePaths.Add(new int[] { X - 1, Y + 2 });
                if (X > 1 && Y < 7) possiblePaths.Add(new int[] { X - 2, Y + 1 });
                if (X < 7 && Y > 1) possiblePaths.Add(new int[] { X + 1, Y - 2 });
                if (X < 6 && Y > 0) possiblePaths.Add(new int[] { X + 2, Y - 1 });
                if (X > 0 && Y > 1) possiblePaths.Add(new int[] { X - 1, Y - 2 });
                if (X > 1 && Y > 0) possiblePaths.Add(new int[] { X - 2, Y - 1 });
            }
        }
        private class Rook : Figure
        {
            public Rook(int x, int y, int name, string image)
            {
                X = x;
                Y = y;
                Name = name;
                addImage(image);
            }
            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                if (X != 7)
                {
                    for (int x1 = X; x1 <= 7; x1++)
                    {
                        possiblePaths.Add(new int[] { x1, Y });
                    }
                }
                if (X != 0)
                {
                    for (int x1 = X; x1 >= 0; x1--)
                    {
                        possiblePaths.Add(new int[] { x1, Y });
                    }
                }
                if (Y != 7)
                {
                    for (int y1 = Y; y1 <= 7; y1++)
                    {
                        possiblePaths.Add(new int[] { X, y1 });
                    }
                }
                if (Y != 0)
                {
                    for (int y1 = Y; y1 >= 0; y1--)
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

            DrawModels();
        }
        Series z1, z2, p1;

        public void pathVisibility(bool val, List<int[]> paths)
        {
            paths.ForEach(i => pbErr[i[0], i[1]].Visible = val);
        }
        public List<int[]> combinePaths()
        {
            //possiblePaths.ForEach(i => B[i[0], i[1]] = 9);
            List<int[]> paths = new List<int[]>();

            blacks[0].possiblePaths.ForEach(i => paths.Add(i));
            blacks[3].possiblePaths.ForEach(i => paths.Add(i));

            for (int q = 1; q < 3; q++)
            {
                bool skip = false;
                blacks[q].possiblePaths.ForEach(i =>
                {
                    if (i[0] == blacks[q].X && i[1] == blacks[q].Y) skip = false;
                    if (!skip)
                    {
                        int sum = 0;
                        for (int u = 0; u < blacks.Count; u++)
                        {
                            if (u == q) continue;
                            else
                            {
                                if (blacks[u].X == i[0] && blacks[u].Y == i[1]) sum++;
                            }
                        }
                        skip = (sum > 0) ? true : false;
                    }
                    if (!skip) paths.Add(i);
                });
            }

            return paths;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked) pathVisibility(false, PATHS);
            //---
            Random rnd = new Random();
            int u = rnd.Next(0, 4);
            WriteLine("");
            blacks[u].Move();
            blacks[u].GeneratePaths();
            PATHS = combinePaths();
            //---
            /*int[,] B = (int[,])A.Clone();
            blacks.ForEach(figure => { A[figure.X, figure.Y] = figure.Name; figure.combinePaths(B); });*/
            if (checkBox1.Checked) pathVisibility(true, PATHS);
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

        private void DrawModels()
        {
            fillValues(A);
            blacks = new List<Figure>();
            blacks.Add(new King(4, 7, 1, "Data/BK.jpg"));
            blacks.Add(new Rook(7, 7, 2, "Data/BR.jpg"));
            blacks.Add(new Bishop(5, 7, 3, "Data/BB.jpg"));
            blacks.Add(new Horse(1, 7, 4, "Data/BH.jpg"));
            blacks.ForEach(figure => { A[figure.X, figure.Y] = figure.Name; figure.GeneratePaths(); chart1.Controls.Add(figure.pb); });
            PATHS = combinePaths();

            King whiteKing = new King(4, 0, 8, @"Data/WK.jpg");
            chart1.Controls.Add(whiteKing.pb);
            A[whiteKing.X, whiteKing.Y] = whiteKing.Name;

            for (int i = 0; i < 8; i++)
            {
                for (int u = 0; u < 8; u++)
                {
                    PictureBox pb1 = new PictureBox();
                    pb1.Location = new Point(0, 0);
                    Figure.imageLocation(i, u, pb1);
                    pb1.Size = new Size(40, 40);
                    pb1.Image = Image.FromFile("Data/checked.png");
                    pb1.Visible = false;
                    pbErr[i, u] = pb1;
                    chart1.Controls.Add(pbErr[i, u]);
                }
            }

            PictureBox pb = new PictureBox();
            pb.Location = new Point(0, 0);
            pb.Size = new Size(400, 400);
            pb.Image = Image.FromFile(@"Data/Board.jpg");
            pb.Visible = true;
            pb.BackColor = Color.Transparent;
            chart1.Controls.Add(pb);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pathVisibility(checkBox1.Checked, PATHS);
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