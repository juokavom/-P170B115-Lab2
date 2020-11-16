//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//II projektinė užduotis

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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
            return Math.Sqrt(-((-Math.Pow(x, 2) + 1 + Math.Sqrt(Math.Pow(x, 4) + 2 * Math.Pow(x, 2) + 40 * x + 1)) / 2));

        }
        private double Y12(double x)
        {
            return -Math.Sqrt(-((-Math.Pow(x, 2) + 1 + Math.Sqrt(Math.Pow(x, 4) + 2 * Math.Pow(x, 2) + 40 * x + 1)) / 2));

        }
        private double Y13(double x)
        {
            return Math.Sqrt(-((-Math.Pow(x, 2) + 1 - Math.Sqrt(Math.Pow(x, 4) + 2 * Math.Pow(x, 2) + 40 * x + 1)) / 2));

        }
        private double Y14(double x)
        {
            return -Math.Sqrt(-((-Math.Pow(x, 2) + 1 - Math.Sqrt(Math.Pow(x, 4) + 2 * Math.Pow(x, 2) + 40 * x + 1)) / 2));

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
        Series z1, z2, line1, line2, line3, line4;
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

            line1 = chart1.Series.Add("Sprendinys 1");
            line2 = chart1.Series.Add("Sprendinys 2");
            line3 = chart1.Series.Add("Sprendinys 3");
            line4 = chart1.Series.Add("Sprendinys 4");

            Niutono(2, -8, line1);
            Niutono(5, 10, line2);
            Niutono(-7, 5, line3);
            Niutono(0, 1, line4);
        }

        private double f211(double x, double y)
        {
            return 10 * x / (Math.Pow(y, 2) + 1) + Math.Pow(x, 2) - Math.Pow(y, 2);
        }
        private double dFx211(double x, double y)
        {
            return 10 / (Math.Pow(y, 2) + 1) + 2 * Math.Pow(x, 2);
        }
        private double dFy211(double x, double y)
        {
            return -20 * x * y / (Math.Pow((Math.Pow(y, 2) + 1), 2)) - 2 * y;
        }
        private double f212(double x, double y)
        {
            return Math.Pow(x, 2) + 2 * Math.Pow(y, 2) - 32;
        }
        private double deltaFx212(double x, double y)
        {
            return 2 * x;
        }
        private double dFy212(double x, double y)
        {
            return 4 * y;
        }

        double[] x = new double[2]; //Pradinis artinys
        double[,] J = new double[2, 2]; //Jakobio matrica
        double[] F = new double[2]; //Funkcijos reiksmes
        double[] deltaX = new double[2]; //Delta X vektorius
        int it;
        int count = 0;

        private void Niutono(double x0_art, double x1_art, Series line)
        {
            it = 0;
            line.ChartType = SeriesChartType.Line;
            x[0] = x0_art;
            x[1] = x1_art;

            double tikslumas = Double.MaxValue;
            while (tikslumas > 1e-3)
            {

                J[0, 0] = dFx211(x[0], x[1]);
                J[0, 1] = dFy211(x[0], x[1]);
                J[1, 0] = deltaFx212(x[0], x[1]);
                J[1, 1] = dFy212(x[0], x[1]);

                F[0] = f211(x[0], x[1]);

                F[1] = f212(x[0], x[1]);

                double k = J[1, 0] / J[0, 0]; //Jakobio matrica sprendžiama gauso metodu
                J[1, 0] -= J[0, 0] * k;
                J[1, 1] -= J[0, 1] * k;
                F[1] -= F[0] * k;

                deltaX[1] = -F[1] / J[1, 1];
                deltaX[0] = (-F[0] - deltaX[1] * J[0, 1]) / J[0, 0];

                line.Points.AddXY(x[0], x[1]);

                tikslumas = Math.Abs(f211(x[0], x[1]) - f212(x[0], x[1]));
                it++;

                if (tikslumas < 0.001 || it > 1000) break;
                else
                {
                    x[0] += deltaX[0];
                    x[1] += deltaX[1];
                    Thread.Sleep(300);
                }
            }

            richTextBox1.AppendText(string.Format("Pradinis artinys: [{0}; {1}], tikslumas: {2, 0:F5}, iteracijų sk.: {3}\nSprendinys {6}: [{4, 0:F5}; {5, 0:F5}]\n",
                x0_art, x1_art, tikslumas, it, x[0], x[1], ++count));
            line.BorderWidth = 3;

        }

        private string print2DMatrix(double[,] a)
        {
            string sb = string.Format("[{0, 3:F2} {1, 3:F2}\n{2, 3:F2} {3, 3:F2}]", a[0, 0], a[0, 1], a[1, 0], a[1, 1]);

            return sb;
        }
        private string print4DMatrix(double[,] a)
        {
            string sb = "[\n";
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int u = 0; u < a.GetLength(1); u++)
                {
                    sb += string.Format("{0, 3:F2} ", a[i, u]);
                }
                sb += "\n";
            }
            sb += "]";
            return sb;
        }

        private double[] f221(double[] x)
        {
            double[] ret = new double[4];
            ret[0] = x[0] + 4 * x[1] + x[2] - 22;
            ret[1] = x[1] * x[2] - 2 * x[2] - 18;
            ret[2] = -Math.Pow(x[1], 2) + 2 * Math.Pow(x[3], 3) - 3 * x[0] * x[3] + 335;
            ret[3] = 2 * x[2] - 12 * x[1] + 2 * x[3] + 58;

            return ret;
        }
        private double[,] df221(double[] x, double[] f)
        {
            double[,] ret = new double[4, 5];
            ret[0, 0] = 1; ret[0, 1] = 4; ret[0, 2] = 1; ret[0, 3] = 0; ret[0, 4] = f[0];
            ret[1, 0] = 0; ret[1, 1] = x[2]; ret[1, 2] = x[1] - 2; ret[1, 3] = 0; ret[1, 4] = f[1];
            ret[2, 0] = -3 * x[3]; ret[2, 1] = -2 * x[1]; ret[2, 2] = 0; ret[2, 3] = 6 * Math.Pow(x[3], 2) - 3 * x[0]; ret[2, 4] = f[2];
            ret[3, 0] = 0; ret[3, 1] = -12; ret[3, 2] = 2; ret[3, 3] = 2; ret[3, 4] = f[3];

            return ret;
        }

        static void SolveGaussian(double[,] a, int n)
        {
            int i, j, k = 0, c;

            for (i = 0; i < n; i++)
            {
                if (a[i, i] == 0)
                {
                    c = 1;
                    while ((i + c) < n && a[i + c, i] == 0)
                        c++;
                    if ((i + c) == n)
                    {
                        break;
                    }
                    for (j = i, k = 0; k <= n; k++)
                    {
                        double temp = a[j, k];
                        a[j, k] = a[j + c, k];
                        a[j + c, k] = temp;
                    }
                }

                for (j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        double p = a[j, i] / a[i, i];

                        for (k = 0; k <= n; k++)
                            a[j, k] = a[j, k] - (a[i, k]) * p;
                    }
                }
            }
        }


        private void nlsAntras()
        {
            ClearForm1();
            double[] x = { 1, 1, 1, 1 };
            double[] x_prad = new double[x.GetLength(0)];
            Array.Copy(x, x_prad, x.GetLength(0));
            double[] deltaX = new double[4];
            int n = x.GetLength(0);
            int it = 0;
            double tikslumas = Double.MaxValue;

            while (tikslumas > 1e-3)
            {
                double[] F = f221(x);
                double[,] J = df221(x, F);
                SolveGaussian(J, 4);

                for (int i = 0; i < n; i++)
                {
                    deltaX[i] = -J[i, n] / J[i, i];
                }

                tikslumas = Math.Abs(F.Max() - F.Min());
                it++;

                x[0] += deltaX[0];
                x[1] += deltaX[1];
                x[2] += deltaX[2];
                x[3] += deltaX[3];

                richTextBox1.AppendText(string.Format("Artinys: [{0, 0:F5}; {1, 0:F5}; {2, 0:F5}; {3, 0:F5}], tikslumas: {4, 0:F5}, iteracija : {5}\n",
                    x[0], x[1], x[2], x[3], tikslumas, it));
            }
            richTextBox1.AppendText(string.Format("Pradinis artinys: [{0}; {1}; {2}; {3}], tikslumas: {4, 0:F5}, iteracijų sk.: {5}\nSprendinys:  [{6, 0:F5}; {7, 0:F5}; {8, 0:F5}; {9, 0:F5}]\n",
                x_prad[0], x_prad[1], x_prad[2], x_prad[3], tikslumas, it, x[0], x[1], x[2], x[3]));
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

            // isvalomos visos nubreztos kreives
            chart1.Series.Clear();
        }
    }
}
