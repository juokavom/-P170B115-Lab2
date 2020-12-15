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
        int X_max = 4;
        int Y_max = 4;
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
                if (Visited) return false;
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

        //Series z1, p1;
        private void BFS(Cell[,] A)
        {
            int[] start = { 0, 3 };
            int[] end = { 4, 0 };
            int currentIndex = 0;
            int endIndex = 1;

            int[][] visited = new int[A.GetLength(0) * A.GetLength(1)][];
            visited[0] = start;
            A[0, 3].Visited = true;
            A[0, 3].Parent = new int[] { -1, -1 };
            System.Diagnostics.Debug.WriteLine(visited.GetLength(0));

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
                for (int i = 0; i < currentIndex; i++)
                {
                    System.Diagnostics.Debug.Write("|" + visited[i][0] + "," + visited[i][1]);
                }
                System.Diagnostics.Debug.WriteLine("|");
                if (currentIndex < visited.GetLength(0)) currentIndex++;
                if (A[end[0], end[1]].Visited) break;
            }

            System.Diagnostics.Debug.WriteLine("Atgaline seka:");
            System.Diagnostics.Debug.Write(string.Format("|{0};{1}", end[0], end[1]));
            for (Cell curr = A[end[0], end[1]]; curr.Parent[0] != -1; curr = A[curr.Parent[0], curr.Parent[1]])
            {
                curr.Symbol = '-';
                System.Diagnostics.Debug.Write(string.Format("|{0};{1}", curr.Parent[0], curr.Parent[1]));
            }
            System.Diagnostics.Debug.WriteLine("|");

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
            printMatrix(A);
            BFS(A);
            printMatrix(A);
            /*
            z1 = chart1.Series.Add("Aukštis h (m), žingsnis = step");
            z1.ChartType = SeriesChartType.Line;
            z1.Color = Color.Blue;
            //---
            p1 = chart1.Series.Add("Iššokimas iš lėktuvo");
            p1.ChartType = SeriesChartType.Point;
            p1.Color = Color.Black;
            //---
            z1.BorderWidth = 1;
            p1.BorderWidth = 3;
            */
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