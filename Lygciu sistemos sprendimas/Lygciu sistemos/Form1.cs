﻿//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//I projektinė užduotis

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Statistics;

namespace Pvz1
{
   
    public partial class Form1 : Form
    {
        List<Timer> Timerlist = new List<Timer>();

        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        //PIRMA UZDUOTIS
        public static decimal[,] mulMatrix(decimal[,] a, decimal[,] b)
        {
            decimal[,] ret = new decimal[a.GetLength(1), b.GetLength(0)];
            for (int i = 0; i < ret.GetLength(0); i++)
            {
                for (int j = 0; j < ret.GetLength(1); j++)
                {
                    ret[i, j] = 0;
                    for (int k = 0; k < ret.GetLength(0); k++)
                    {
                        ret[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return ret;
        }
        public static string printMatrix(decimal[,] A)
        {
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int u = 0; u < A.GetLength(1); u++)
                {
                    ret.Append(string.Format("{0, 12:F6}", A[i, u]));
                }
                ret.Append("\n");
            }
            ret.Append("\n");
            return ret.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm1();
            richTextBox1.AppendText("Sprendžiama lygčių sistema: [A][X]=[B]\n\n");
            decimal[,] A = { { 9, 1, -2, 1 }, { 0, 11, 3, 4 }, { 1, 3, 12, -3 }, { 0, -1, 2, 2 } };
            decimal[,] A_test = { { 9, 1, -2, 1 }, { 0, 11, 3, 4 }, { 1, 3, 12, -3 }, { 0, -1, 2, 2 } };
            decimal[] B = { 47, -24, 27, -5 };
            decimal[] X = new decimal[B.Length];
            Array.ForEach(X, element => element = 0);
            richTextBox1.AppendText("[A] = \n");
            richTextBox1.AppendText(printMatrix(A));
            richTextBox1.AppendText("[B] = \n");
            Array.ForEach(B, element => richTextBox1.AppendText(string.Format("{0, 12}\n", element)));
            int n = A.GetLength(0);
            //---
            decimal[,] L = new decimal[n, n];
            decimal[,] U = new decimal[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int u = 0; u < n; u++)
                {
                    U[i, u] = 0;
                    L[i, u] = (i == u) ? 1 : 0;
                }
            }
            //---
            richTextBox1.AppendText("[L] = \n");
            richTextBox1.AppendText(printMatrix(L));
            richTextBox1.AppendText("[U] = \n");
            richTextBox1.AppendText(printMatrix(U));
            //---
            for (int i = 0; i < n; i++) U[0, i] = A[0, i];
            decimal r = 0;
            for (int i = 0; i < n - 1; i++) //Eilute
            {
                for (int u = i + 1; u < n; u++) //Likusios eilutes
                {
                    r = A[u, i] / A[i, i];
                    for (int y = i; y < n; y++)
                    {
                        U[u, y] = A[u, y] - A[i, y] * r;
                        A[u, y] = A[u, y] - A[i, y] * r;
                    }
                    L[u, i] = r;
                }
                richTextBox1.AppendText(string.Format("Nuliai po {0} stulp. Pertvarkyta matrica [A]=\n", i + 1));
                richTextBox1.AppendText(printMatrix(A));
            }
            //---
            richTextBox1.AppendText("Skaičiavimai baigti. Rezultatai:\n\n");
            richTextBox1.AppendText("[L] = \n");
            richTextBox1.AppendText(printMatrix(L));
            richTextBox1.AppendText("[U] = \n");
            richTextBox1.AppendText(printMatrix(U));
            //---
            for (int i = n - 1; i >= 0; i--)
            {
                decimal value = 0;
                for (int u = n - 1; u > i; u--)
                {
                    value += U[i, u] * X[u];
                }
                X[i] = (B[i] - value) / U[i, i];
            }
            richTextBox1.AppendText("Sprendinys [X] = \n");
            Array.ForEach(X, element => richTextBox1.AppendText(string.Format("{0, 12:F6}\n", element)));
            //---
            //Tikrinimas
            richTextBox1.AppendText("\nTikrinimas = \n");
            //1. L*U
            decimal[,] ats = mulMatrix(L, U);
            richTextBox1.AppendText("1) [L]*[U] = \n");
            richTextBox1.AppendText(printMatrix(ats));
            //2. Reiksmiu istatymas
            richTextBox1.AppendText("2) Reikšmių įstatymas į pradinę matricą =\n");
            for (int i = 0; i < A_test.GetLength(0); i++)
            {
                richTextBox1.AppendText(string.Format("Tikrinama {0} pradinės matricos eilutė = \n", i + 1));
                decimal ats2 = 0;
                for (int u = 0; u < A_test.GetLength(1); u++)
                {
                    ats2 += A_test[i, u] * X[u];
                    richTextBox1.AppendText(string.Format("{0, 0:F2} * {1, 0:F2}", A_test[i, u], X[u]));
                    if (u + 1 < A_test.GetLength(1)) richTextBox1.AppendText(" + ");
                }
                richTextBox1.AppendText(string.Format(" = {0, 0:F2}. B[{1}] = {2, 0:F2}\n", ats2, i + 1, B[i]));
            }

        }

        //ANTRA UZDUOTIS

        //2.1
        private double Y11(double x)
        {
            return Math.Sqrt(-((-Math.Pow(x, 2)+1+Math.Sqrt(Math.Pow(x, 4)+2*Math.Pow(x, 2)+40*x+1))/2));

        }
        private double Y12(double x)
        {
            return -Math.Sqrt(-((-Math.Pow(x, 2)+1+Math.Sqrt(Math.Pow(x, 4)+2*Math.Pow(x, 2)+40*x+1))/2));

        }
        private double Y13(double x)
        {
            return Math.Sqrt(-((-Math.Pow(x, 2)+1-Math.Sqrt(Math.Pow(x, 4)+2*Math.Pow(x, 2)+40*x+1))/2));

        }
        private double Y14(double x)
        {
            return -Math.Sqrt(-((-Math.Pow(x, 2)+1-Math.Sqrt(Math.Pow(x, 4)+2*Math.Pow(x, 2)+40*x+1))/2));

        }
        private double Y21(double x)
        {
            return Math.Sqrt((-Math.Pow(x, 2) + 32) / 2);
        }
        private double Y22(double x)
        {
            return -Math.Sqrt((-Math.Pow(x, 2) + 32) / 2);
        }
        private double Z1(double x, double y) 
        {
            return (10 * x) / (Math.Pow(y, 2) + 1) + Math.Pow(x, 2) - Math.Pow(y, 2);
        }
        Series z1, z2;
        private void button3_Click(object sender, EventArgs e)
        {
            if (var1.Checked) nlsPirmas();
            else if (var2.Checked) nlsAntras();
            
        }

        private void nlsPirmas() 
        {
            PracticalGuideCharts.Form1 trimateForma = new PracticalGuideCharts.Form1();
            trimateForma.ShowDialog();
            ClearForm1();
            PreparareForm(-15, 10, -5, 5);
            z1 = chart1.Series.Add("Pirma lygtis");
            z1.ChartType = SeriesChartType.Point;
            z2 = chart1.Series.Add("Antra lygtis");
            z2.ChartType = SeriesChartType.Point;
            double x = -5;
            int count = 0;
            for (double i = -15; i < 8; i += 0.001f)
            {
                z1.Points.AddXY(i, Y11(i));
                z1.Points.AddXY(i, Y12(i));
                z1.Points.AddXY(i, Y13(i));
                z1.Points.AddXY(i, Y14(i));
                z2.Points.AddXY(i, Y21(i));
                z2.Points.AddXY(i, Y22(i));
            }
            richTextBox1.AppendText("Nupiesta\n");

            z1.BorderWidth = 1;
            z2.BorderWidth = 1;
        }

        private void nlsAntras() 
        {
            ClearForm1();
        }

        // ---------------------------------------------- KITI METODAI ----------------------------------------------

        /// <summary>
        /// Uždaroma programa
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Išvalomas grafikas ir consolė
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            ClearForm1();
        }


        public void ClearForm1()
        {
            richTextBox1.Clear(); // isvalomas richTextBox1
            // sustabdomi timeriai jei tokiu yra
            foreach (var timer in Timerlist)
            {
                timer.Stop();
            }

            // isvalomos visos nubreztos kreives
            chart1.Series.Clear();
        }
    }
}
