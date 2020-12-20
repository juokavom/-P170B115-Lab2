//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//Egzamino užduotis

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pvz1
{
    public partial class Form1 : Form
    {
        static int X_max = 110;
        static int Y_max = 110;
        static int X_min = 0;
        static int Y_min = 0;
        private class Cell
        {
            public char Symbol { get; set; }
            public bool Visited { get; set; }
            public bool Valid { get; set; }
            public int[] Parent { get; set; }

            public Cell(bool valid)
            {
                Symbol = '*';
                Valid = valid;
                Visited = false;
            }

            public bool Visit(int[] parent)
            {
                if (Visited || !Valid) return false;
                Parent = parent;
                Visited = true;
                return true;
            }
        }
        private class Figure
        {
            public double[] O { get; set; }
            public double[,] XY { get; set; }
            public double R { get; set; }
            public int N { get; set; }
            public double Angle { get; set; }
            public Cell[,] A { get; set; }

            public Figure(Cell[,] A, double xmin, double xmax, double ymin, double ymax)
            {
                this.A = A;
                O = new double[2];
                Random rnd = new Random();
                double d = xmax - xmin;
                O[0] = xmin + d / 2;
                O[1] = ymin + d / 2;
                R = d * 0.9 / 2;
                // N = rnd.Next(3, 8);
                N = 5;
                XY = new double[N, 2];
                Angle = 0;
                CalculateXY();
                shapeMatrix();
            }

            public void CalculateXY()
            {
                for (int i = 0; i < N; i++)
                {
                    XY[i, 0] = O[0] + Math.Cos(Angle) * R;
                    XY[i, 1] = O[1] + Math.Sin(Angle) * R;
                    Angle += 2 * Math.PI / N;
                }
            }

            public void Draw(Series z, Series p)
            {
                p.Points.AddXY(O[0], O[1]);
                for (int i = 0; i < N; i++)
                {
                    z.Points.AddXY(XY[i, 0], XY[i, 1]);
                }
                z.Points.AddXY(XY[0, 0], XY[0, 1]);
            }

            public void shapeMatrix()
            {
                for (int i = 0; i < N; i++)
                {
                    int startX = (int)XY[i, 0];
                    int startY = (int)XY[i, 1];
                    int endX;
                    int endY;
                    if (i + 1 == N)
                    {
                        endX = (int)XY[0, 0];
                        endY = (int)XY[0, 1];
                    }
                    else
                    {
                        endX = (int)XY[i + 1, 0];
                        endY = (int)XY[i + 1, 1];
                    }
                    int[][] route = BFS(A, new int[] { startX, startY }, new int[] { endX, endY });
                    for (int u = 0; i < route.GetLength(0); u++)
                    {
                        A[route[i][0], route[i][1]].Valid = false;
                        A[route[i][0], route[i][1]].Symbol = 'Q';
                    }
                }
            }

            public bool isInside(double x, double y)
            {
                double sum = 0;
                for (int i = 0; i < N; i++)
                {
                    sum += Math.Sqrt(Math.Pow(XY[i, 0] - x, 2) + Math.Pow(XY[i, 1] - y, 2));
                }
                System.Diagnostics.Debug.WriteLine(string.Format("sum = {0}, R*N = {1}", sum, R * N));

                if (sum <= R * (N + 2)) return true;
                return false;
            }

        }
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        Series z1, z2, p1;
        private static int[][] BFS(Cell[,] A, int[] start, int[] end)
        {
            int currentIndex = 0;
            int endIndex = 1;

            int[][] visited = new int[A.GetLength(0) * A.GetLength(1)][];
            visited[0] = start;
            A[start[0], start[1]].Symbol = '-';
            A[start[0], start[1]].Visited = true;
            A[start[0], start[1]].Parent = new int[] { -1, -1 };

            while (true)
            {
                int x = visited[currentIndex][0];
                int y = visited[currentIndex][1];

                if (x > X_min && x <= X_max)
                {
                    // # # #
                    // x o #
                    // # # #
                    if (A[x - 1, y].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x - 1, y };
                    }
                }
                if (x < X_max && x >= X_min)
                {
                    // # # #
                    // # o x
                    // # # #
                    if (A[x + 1, y].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x + 1, y };
                    }
                }
                if (y > Y_min && y <= Y_max)
                {
                    // # # #
                    // # o #
                    // # x #
                    if (A[x, y - 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x, y - 1 };
                    }
                }
                if (y < Y_max && y >= Y_min)
                {
                    // # x #
                    // # o #
                    // # # #
                    if (A[x, y + 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x, y + 1 };
                    }
                }
                if (x > X_min && x <= X_max && y > Y_min && y <= Y_max)
                {
                    // # # #
                    // # o #
                    // x # #
                    if (A[x - 1, y - 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x - 1, y - 1 };
                    }
                }
                if (x > X_min && x <= X_max && y < Y_max && y >= Y_min)
                {
                    // x # #
                    // # o #
                    // # # #
                    if (A[x - 1, y + 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x - 1, y + 1 };
                    }
                }
                if (x < X_max && x >= X_min && y > Y_min && y <= Y_max)
                {
                    // # # #
                    // # o #
                    // # # x
                    if (A[x + 1, y - 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x + 1, y - 1 };
                    }
                }
                if (x < X_max && x >= X_min && y < Y_max && y >= Y_min)
                {
                    // # # x
                    // # o #
                    // # # #
                    if (A[x + 1, y + 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x + 1, y + 1 };
                    }
                }
                if (currentIndex < visited.GetLength(0)) currentIndex++;
                if (A[end[0], end[1]].Visited) break;
            }

            List<int[]> list = new List<int[]>();
            list.Add(new int[] { end[0], end[1] });
            for (Cell curr = A[end[0], end[1]]; curr.Parent[0] != -1; curr = A[curr.Parent[0], curr.Parent[1]])
            {
                curr.Symbol = '-';
                list.Add(new int[] { curr.Parent[0], curr.Parent[1] });
            }
            list.Reverse();
            return list.ToArray();

        }
        private void printMatrix(Cell[,] array)
        {
            string line = new string('-', array.GetLength(0) * 2 - 1);
            System.Diagnostics.Debug.WriteLine(line);
            for (int y = array.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    System.Diagnostics.Debug.Write(string.Format("{0} ", array[x, y].Symbol));
                }
                System.Diagnostics.Debug.WriteLine("");
            }
            System.Diagnostics.Debug.WriteLine(line);
        }
        private int[] getFigureLayout(int n)
        {
            double value = Math.Sqrt(n);
            int upper = (int)value + 1;
            int lower = value < 0.5 ? (int)value : upper;
            return new int[] { lower, upper };
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm1();
            //---
            PreparareForm(0, 110, 0, 110);
            richTextBox1.AppendText("Paleidimas\n");
            //---
            Cell[,] A = new Cell[X_max + 1, Y_max + 1];
            //---
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int u = 0; u < A.GetLength(1); u++)
                {
                    A[i, u] = new Cell(true);
                }
            }
            //---
            //---
            z1 = chart1.Series.Add("Pradine figura");
            z1.ChartType = SeriesChartType.Line;
            z1.Color = Color.Blue;
            //---
            z2 = chart1.Series.Add("Maršrutas");
            z2.ChartType = SeriesChartType.Line;
            z2.Color = Color.Red;
            //---
            p1 = chart1.Series.Add("O");
            p1.ChartType = SeriesChartType.Point;
            p1.Color = Color.Black;
            //---
            z1.BorderWidth = 1;
            p1.BorderWidth = 3;

            //int N = 7;
            //int[] Layout = getFigureLayout(N);

            Figure f = new Figure(A, 10, 90, 10, 90);
            f.Draw(z1, p1);

            int[] start = { 0, 0 };
            int[] end = { 99, 99 };

            printMatrix(A);

            /*
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int u = 0; u < A.GetLength(1); u++)
                {
                    if (f.isInside(i, u))
                    {
                        A[i, u].Valid = false;
                        A[i, u].Symbol = 'Q';
                    }
                }
            }
            */

            int[][] route = BFS(A, start, end);
            for (int i = 0; i < route.GetLength(0); i++)
            {
                System.Diagnostics.Debug.WriteLine(route[i][0] + " " + route[i][1]);
                z2.Points.AddXY(route[i][0], route[i][1]);
            }
            printMatrix(A);
            //---
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