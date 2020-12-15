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
        int X_max = 9;
        int Y_max = 9;
        int X_min = 0;
        int Y_min = 0;
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
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        Series z1, z2, p1;
        private int[][] BFS(Cell[,] A, int[] start, int[] end)
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
        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm1();
            //---
            PreparareForm(0, 12, 0, 12);
            richTextBox1.AppendText("Paleidimas\n");
            //---
            /* Cell[,] A = new Cell[X_max + 1, Y_max + 1];
             //---
             for (int i = 0; i < A.GetLength(0); i++)
             {
                 for (int u = 0; u < A.GetLength(1); u++)
                 {
                     A[i, u] = new Cell(true);
                 }
             }
             //---
             printMatrix(A);
             int[] start = { 0, 1 };
             int[] end = { 9, 9 };
             A[2, 3].Valid = false;
             A[3, 3].Valid = false;
             int[][] route = BFS(A, start, end);
             for (int i = 0; i < route.GetLength(0); i++)
             {
                 System.Diagnostics.Debug.WriteLine(route[i][0] + " " + route[i][1]);
             }
             printMatrix(A);
             */
            z1 = chart1.Series.Add("Pradine figura");
            z1.ChartType = SeriesChartType.Line;
            z1.Color = Color.Blue;
            //---
            z2 = chart1.Series.Add("Pasukta figura");
            z2.ChartType = SeriesChartType.Line;
            z2.Color = Color.Red;
            //---
            p1 = chart1.Series.Add("O");
            p1.ChartType = SeriesChartType.Point;
            p1.Color = Color.Black;
            //---
            z1.BorderWidth = 1;
            p1.BorderWidth = 3;

            double[] O = { 5, 5 };
            double r = 4;
            double n = 5;
            double angle = 0;

            p1.Points.AddXY(O[0], O[1]);
            for (int i = 0; i < n+1; i++)
            {
                double x = O[0] + Math.Cos(angle) * r;
                double y = O[1] + Math.Sin(angle) * r;
                z1.Points.AddXY(x, y);
                angle += 2 * Math.PI / n;
            }
            angle = Math.PI/6;
            for (int i = 0; i < n+1; i++)
            {
                double x = O[0] + Math.Cos(angle) * r;
                double y = O[1] + Math.Sin(angle) * r;
                z2.Points.AddXY(x, y);
                angle += 2 * Math.PI / n;
            }

            
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