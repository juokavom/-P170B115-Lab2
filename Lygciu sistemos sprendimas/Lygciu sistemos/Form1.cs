//Jokubas Akramas IFF-8/12 7 var.
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
            ClearForm1();
            PreparareForm(-15, 10, -5, 5);
            x1 = -5;
            x2 = 5;
            iii = 0;
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

        /// <summary>
        /// timer2 iteracijoje atliekami veiksmai
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer4_Tick(object sender, EventArgs e)
        {
            xtemp = x_nueita + zingsnis;

            X1X2.Points.Clear();
            XMid.Points.Clear();

            X1X2.Points.AddXY(x_nueita, 0);
            X1X2.Points.AddXY(xtemp, 0);

            if (Math.Sign((double)F(x_nueita)) != Math.Sign((double)F(xtemp)))
            {
                richTextBox1.AppendText(string.Format("{0}-asis intervalas: ({1}; {2}) \n", ++i_interval, x_nueita, xtemp));
                intervalsArray.Add(new Interval() { x1 = x_nueita, x2 = xtemp, done = false });
            }

            x_nueita = xtemp;

            if (xtemp >= x2)
            {
                timer4.Stop();
                current = 0;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                if (stygu.Checked)
                {
                    richTextBox1.AppendText("Iteracija         x            F(x)        x1          x2          F(x1)         F(x2)       \n");
                }
                else if (iteraciju.Checked)
                {
                    richTextBox1.AppendText("Iteracija         x1          F(x1)\n");
                }
                else if (skenavimo.Checked)
                {
                    richTextBox1.AppendText("Iteracija         x1          x2          F(x1)         F(x2)       \n");
                }
                intervals = intervalsArray.ToArray();
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                timer2.Enabled = true;
                timer2.Interval = 500;
                timer2.Start();
            }
        }
        private void StyguMetodas()
        {

            x1 = intervals[current].x1;
            x2 = intervals[current].x2;

            float k = Math.Abs((float)(F(x1) / F(x2)));
            xtemp = (x1 + k * x2) / (1 + k);

            double tikslumas = 1e-5;

            if (Math.Abs(F(xtemp)) > tikslumas & iii <= N)
            {
                X1X2.Points.Clear();
                XMid.Points.Clear();

                X1X2.Points.AddXY(x1, 0);
                X1X2.Points.AddXY(x2, 0);
                XMid.Points.AddXY(xtemp, 0);

                richTextBox1.AppendText(String.Format(" {0,6:d}   {1,12:f7}  {2,12:f7} {3,12:f7} {4,12:f7} {5,12:f7} {6,12:f7}\n",
                    iii, xtemp, F(xtemp), x1, x2, F(x1), F(x2)));
                if (Math.Sign((double)F(x1)) != Math.Sign((double)F(xtemp)))
                {
                    intervals[current].x2 = xtemp;
                }
                else
                {
                    intervals[current].x1 = xtemp;
                }
                iii = iii + 1;

            }
            else if (current < intervals.Length - 1)
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Stygų", x1prad, x2prad, xtemp, tikslumas, iii));
                current++;
                iii = 0;
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         x            F(x)        x1          x2          F(x1)         F(x2)       \n");
            }
            else
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Stygų", x1prad, x2prad, xtemp, tikslumas, iii));

                richTextBox1.AppendText("\nSkaičiavimai baigti.");
                timer2.Stop();
            }
        }
        private void IteracijuMetodas()
        {
            x1 = intervals[current].x1;
            x2 = intervals[current].x2;

            float[] alfa = { 70f, -48f, 22f, -24f };
            //float alfa = 70f; //-4.7 (1)
            //float alfa = -48f; //2.62 (2)
            //float alfa = 22f; //3.25 (3)
            //float alfa = -24f; //3.77 (4)

            xtemp = (float)(F(x1) / alfa[current]) + x1;

            double tikslumas = 1e-4;

            if (Math.Abs(F(x1)) > tikslumas & iii <= N)
            {
                X1X2.Points.Clear();
                XMid.Points.Clear();

                XMid.Points.AddXY(x1, 0);

                richTextBox1.AppendText(String.Format(" {0,6:d} {1,12:f7} {2,12:f7}\n",
                    iii, x1, F(x1)));
                iii = iii + 1;
                intervals[current].x1 = xtemp;

            }
            else if (current < intervals.Length - 1)
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}, alfa koeficientas: {7}\n",
                   1 + current, "Iteraciju", x1prad, x2prad, intervals[current].x1, tikslumas, iii, alfa[current]));
                current++;
                iii = 0;
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         x1          F(x1)\n");
            }
            else
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}, alfa koeficientas: {7}\n",
                   1 + current, "Iteraciju", x1prad, x2prad, intervals[current].x1, tikslumas, iii, alfa[current]));

                richTextBox1.AppendText("\nSkaičiavimai baigti.");
                timer2.Stop();
            }
        }
        private void SkenavimoMetodas()
        {
            x1 = intervals[current].x1;
            x2 = intervals[current].x2;
            double tikslumas = 1e-6;
            xtemp = x1 + zingsnis;

            if (Math.Abs(x2 - x1) > tikslumas & iii <= N)
            {
                X1X2.Points.Clear();
                XMid.Points.Clear();
                X1X2.Points.AddXY(x1, 0);
                X1X2.Points.AddXY(xtemp, 0);
                richTextBox1.AppendText(String.Format(" {0,6:d}   {1,12:f7} {2,12:f7} {3,12:f7} {4,12:f7}\n",
                    iii, x1, xtemp, F(x1), F(xtemp)));

                if (Math.Sign((double)F(x1)) != Math.Sign((double)F(xtemp)))
                {
                    intervals[current].x1 = x1;
                    intervals[current].x2 = xtemp;
                    zingsnis = (float)(xtemp - x1) / 5;
                }
                else
                {
                    intervals[current].x1 = xtemp;
                    intervals[current].x2 = xtemp + zingsnis;
                }
                iii++;

            }
            else if (current < intervals.Length - 1)
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Skenavimo su mažėjančiu žingsniu", x1prad, x2prad, x1, tikslumas, iii));
                current++;
                iii = 0;
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         x1          x2          F(x1)         F(x2)       \n");
            }
            else
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Skenavimo su mažėjančiu žingsniu", x1prad, x2prad, x1, tikslumas, iii));

                richTextBox1.AppendText("\nSkaičiavimai baigti.");
                timer2.Stop();
            }
        }








        private class Interval
        {
            public float x1;
            public float x2;
            public bool done;
        }


        float x1, x2, xtemp, zingsnis, x_nueita, x1prad, x2prad;
        int N = 1000;
        int iii, i_interval, current;

        Series Fx, X1X2, XMid, Iv;
        float[] a = { 128.68f, 4.03f, -23.05f, 0.07f, 0.89f };
        float[] a_neg = { 128.68f, -4.03f, -23.05f, -0.07f, 0.89f };
        List<Interval> intervalsArray;
        Interval[] intervals;

        /// <summary>
        /// Sprendžiama F(x) = 0.89𝑥^4 + 0.07𝑥^3 − 23.05𝑥^2 + 4.03𝑥 + 128.68
        /// </summary>
        /// <param name="x">daugianario argumentas</param>
        /// <returns></returns>
        private double F(double x)
        {
            return (double)(a[4] * Math.Pow(x, 4) + a[3] * Math.Pow(x, 3) + a[2] * Math.Pow(x, 2) + a[1] * x + a[0]);
        }

        /// <summary>
        /// Sprendžiama G(x) = e^-x (cos(x)/(x-6)); -5 <= x <= 5
        /// </summary>
        /// <param name="x">funkcijos argumentas</param>
        /// <returns></returns>
        private double G(double x)
        {
            return (double)(Math.Pow(Math.E, -x) * (Math.Cos(x) / (x - 6)));
        }
        private float PreciseInterval(float[] array)
        {
            int k = array.Length - 1;
            float B = Math.Abs(array[0]);
            for (int i = 1; i < array.Length; i++)
            {
                if (Math.Abs(array[i]) > B && array[i] < 0)
                {
                    B = Math.Abs(array[i]);
                }
            }
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (array[i] < 0)
                {
                    k = i;
                    i = -1;
                }
            }
            k = array.Length - k;
            return (float)(1 + Math.Pow((B / a[a.Length - 1]), 1.0 / k));
        }

        private void BigInterval(ref float x1, ref float x2)
        {
            float R, R_positive, R_negative;
            R = 1 + a.MaximumAbsolute() / a[a.Length - 1];
            R_positive = PreciseInterval(a);
            R_negative = PreciseInterval(a_neg);
            x2 = Math.Min(R, R_positive);
            x1 = -Math.Min(R, R_negative);
        }


        /// <summary>
        /// timer2 iteracijoje atliekami veiksmai
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (stygu.Checked)
            {
                StyguMetodas();
            }
            else if (iteraciju.Checked)
            {
                IteracijuMetodas();
            }
            else if (skenavimo.Checked)
            {
                x1 = intervals[current].x1;
                x2 = intervals[current].x2;
                zingsnis = (float)(x2 - x1) / 5;
                SkenavimoMetodas();
            }
        }

        /// <summary>
        /// timer2 iteracijoje atliekami veiksmai
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            xtemp = x_nueita + zingsnis;

            X1X2.Points.Clear();
            XMid.Points.Clear();

            X1X2.Points.AddXY(x_nueita, 0);
            X1X2.Points.AddXY(xtemp, 0);

            if (Math.Sign((double)G(x_nueita)) != Math.Sign((double)G(xtemp)))
            {
                richTextBox1.AppendText(string.Format("{0}-asis intervalas: ({1}; {2}) \n", ++i_interval, x_nueita, xtemp));
                intervalsArray.Add(new Interval() { x1 = x_nueita, x2 = xtemp, done = false });
            }

            x_nueita = xtemp;

            if (xtemp >= x2)
            {
                timer3.Stop();
                current = 0;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                if (stygu.Checked)
                {
                    richTextBox1.AppendText("Iteracija         x            G(x)        x1          x2          G(x1)         G(x2)       \n");
                }
                else if (iteraciju.Checked)
                {
                    richTextBox1.AppendText("Iteracija         x1          G(x1)\n");
                }
                else if (skenavimo.Checked)
                {
                    richTextBox1.AppendText("Iteracija         x1          x2          G(x1)         G(x2)       \n");
                }
                intervals = intervalsArray.ToArray();
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                timer5.Enabled = true;
                timer5.Interval = 500;
                timer5.Start();
            }
        }
        /// <summary>
        /// timer2 iteracijoje atliekami veiksmai
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer5_Tick(object sender, EventArgs e)
        {
            if (stygu.Checked)
            {
                StyguMetodasF();
            }
            else if (iteraciju.Checked)
            {
                IteracijuMetodasF();
            }
            else if (skenavimo.Checked)
            {
                x1 = intervals[current].x1;
                x2 = intervals[current].x2;
                zingsnis = (float)(x2 - x1) / 5;
                SkenavimoMetodasF();
            }
        }
        private void StyguMetodasF()
        {

            x1 = intervals[current].x1;
            x2 = intervals[current].x2;

            float k = Math.Abs((float)(G(x1) / G(x2)));
            xtemp = (x1 + k * x2) / (1 + k);

            double tikslumas = 1e-5;

            if (Math.Abs(G(xtemp)) > tikslumas & iii <= N)
            {
                X1X2.Points.Clear();
                XMid.Points.Clear();

                X1X2.Points.AddXY(x1, 0);
                X1X2.Points.AddXY(x2, 0);
                XMid.Points.AddXY(xtemp, 0);

                richTextBox1.AppendText(String.Format(" {0,6:d}   {1,12:f7}  {2,12:f7} {3,12:f7} {4,12:f7} {5,12:f7} {6,12:f7}\n",
                    iii, xtemp, G(xtemp), x1, x2, G(x1), G(x2)));
                if (Math.Sign((double)G(x1)) != Math.Sign((double)G(xtemp)))
                {
                    intervals[current].x2 = xtemp;
                }
                else
                {
                    intervals[current].x1 = xtemp;
                }
                iii = iii + 1;

            }
            else if (current < intervals.Length - 1)
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Stygų", x1prad, x2prad, xtemp, tikslumas, iii));
                current++;
                iii = 0;
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         x            G(x)        x1          x2          G(x1)         G(x2)       \n");
            }
            else
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Stygų", x1prad, x2prad, xtemp, tikslumas, iii));

                richTextBox1.AppendText("\nSkaičiavimai baigti.");
                timer5.Stop();
            }
        }
        private void IteracijuMetodasF()
        {

            x1 = intervals[current].x1;
            x2 = intervals[current].x2;

            float[] alfa = { -7.5f, 0.5f, -0.2f, 0.01f };

            //float alfa = 7.5f; //-4.7 (1)
            //float alfa = 0.5f; //-1.57 (2)
            //float alfa = -0.2f; //1.57 (3)
            //float alfa = 0.01f; //4.7 (4)

            xtemp = (float)(G(x1) / alfa[current]) + x1;

            double tikslumas = 1e-4;

            if (Math.Abs(G(x1)) > tikslumas & iii <= N)
            {
                X1X2.Points.Clear();
                XMid.Points.Clear();

                XMid.Points.AddXY(x1, 0);

                richTextBox1.AppendText(String.Format(" {0,6:d} {1,12:f7} {2,12:f7}\n",
                    iii, x1, G(x1)));
                iii = iii + 1;
                intervals[current].x1 = xtemp;

            }
            else if (current < intervals.Length - 1)
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                     " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}, alfa koeficientas: {7}\n",
                    1 + current, "Iteraciju", x1prad, x2prad, intervals[current].x1, tikslumas, iii, alfa[current]));
                current++;
                iii = 0;
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         x1          G(x1)\n");
            }
            else
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}, alfa koeficientas: {7}\n",
                   1 + current, "Iteraciju", x1prad, x2prad, intervals[current].x1, tikslumas, iii, alfa[current]));

                richTextBox1.AppendText("\nSkaičiavimai baigti.");
                timer5.Stop();
            }
        }
        private void SkenavimoMetodasF()
        {
            x1 = intervals[current].x1;
            x2 = intervals[current].x2;
            double tikslumas = 1e-6;
            xtemp = x1 + zingsnis;

            if (Math.Abs(x2 - x1) > tikslumas & iii <= N)
            {
                X1X2.Points.Clear();
                XMid.Points.Clear();
                X1X2.Points.AddXY(x1, 0);
                X1X2.Points.AddXY(xtemp, 0);
                richTextBox1.AppendText(String.Format(" {0,6:d}   {1,12:f7} {2,12:f7} {3,12:f7} {4,12:f7}\n",
                    iii, x1, xtemp, G(x1), G(xtemp)));

                if (Math.Sign((double)G(x1)) != Math.Sign((double)G(xtemp)))
                {
                    intervals[current].x1 = x1;
                    intervals[current].x2 = xtemp;
                    zingsnis = (float)(xtemp - x1) / 5;
                }
                else
                {
                    intervals[current].x1 = xtemp;
                    intervals[current].x2 = xtemp + zingsnis;
                }
                iii++;

            }
            else if (current < intervals.Length - 1)
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Skenavimo su mažėjančiu žingsniu", x1prad, x2prad, x1, tikslumas, iii));
                current++;
                iii = 0;
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         x1          x2          G(x1)         G(x2)       \n");
            }
            else
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Skenavimo su mažėjančiu žingsniu", x1prad, x2prad, x1, tikslumas, iii));

                richTextBox1.AppendText("\nSkaičiavimai baigti.");
                timer5.Stop();
            }
        }
        ///
        /// Antra uzduotis
        /// <summary>

        private double V(double c)
        {
            double m = 80;
            double t = 4;
            double v = 36;
            double g = 9.8;
            if (c == 0) c = 0.00000001;
            return ((m * g) / c) * (1 - Math.Pow(Math.E, -(c / m) * t)) - v;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearForm1();
            PreparareForm(-10, 10, -10, 10);
            x1 = -5;
            x2 = 5;
            iii = 0;
            Fx = chart1.Series.Add("V(c)");
            Fx.ChartType = SeriesChartType.Line;
            double x = -5;
            for (int i = 0; i < 250; i++)
            {
                if (x > 5) break;
                Fx.Points.AddXY(x, V(x));
                x = x + (2 * Math.PI) / 50;
            }
            Fx.BorderWidth = 3;

            X1X2 = chart1.Series.Add("X1X2");
            X1X2.MarkerStyle = MarkerStyle.Circle;
            X1X2.MarkerSize = 8;
            X1X2.ChartType = SeriesChartType.Point;
            X1X2.ChartType = SeriesChartType.Line;

            XMid = chart1.Series.Add("Šaknies pozicija");
            XMid.MarkerStyle = MarkerStyle.Circle;
            X1X2.ChartType = SeriesChartType.Point;
            X1X2.ChartType = SeriesChartType.Line;
            XMid.MarkerSize = 8;

            Iv = chart1.Series.Add("Šaknų intervalas");
            Iv.MarkerStyle = MarkerStyle.Cross;
            Iv.MarkerSize = 8;
            Iv.ChartType = SeriesChartType.Point;
            Iv.Points.AddXY(x1, 0);
            Iv.Points.AddXY(x2, 0);

            intervalsArray = new List<Interval>();
            x_nueita = x1;
            zingsnis = 1f;
            i_interval = 0;
            richTextBox1.AppendText(string.Format("Skenuojamas intervalas ({0}, {1}) su žingsniu: {2} atskirti šaknų intervalus\n", x1, x2, zingsnis));
            timer6.Enabled = true;
            timer6.Interval = 500;
            timer6.Start();
        }
        private void timer6_Tick(object sender, EventArgs e)
        {
            xtemp = x_nueita + zingsnis;

            X1X2.Points.Clear();
            XMid.Points.Clear();

            X1X2.Points.AddXY(x_nueita, 0);
            X1X2.Points.AddXY(xtemp, 0);

            if (Math.Sign((double)V(x_nueita)) != Math.Sign((double)V(xtemp)))
            {
                richTextBox1.AppendText(string.Format("{0}-asis intervalas: ({1}; {2}) \n", ++i_interval, x_nueita, xtemp));
                intervalsArray.Add(new Interval() { x1 = x_nueita, x2 = xtemp, done = false });
            }

            x_nueita = xtemp;

            if (xtemp >= x2)
            {
                timer6.Stop();
                current = 0;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         x            G(x)        x1          x2          G(x1)         G(x2)       \n");
                intervals = intervalsArray.ToArray();
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                timer7.Enabled = true;
                timer7.Interval = 500;
                timer7.Start();
            }
        }
        private void timer7_Tick(object sender, EventArgs e)
        {

            x1 = intervals[current].x1;
            x2 = intervals[current].x2;

            float k = Math.Abs((float)(V(x1) / V(x2)));
            xtemp = (x1 + k * x2) / (1 + k);

            double tikslumas = 1e-5;

            if (Math.Abs(V(xtemp)) > tikslumas & iii <= N)
            {
                X1X2.Points.Clear();
                XMid.Points.Clear();

                X1X2.Points.AddXY(x1, 0);
                X1X2.Points.AddXY(x2, 0);
                XMid.Points.AddXY(xtemp, 0);

                richTextBox1.AppendText(String.Format(" {0,6:d}   {1,12:f7}  {2,12:f7} {3,12:f7} {4,12:f7} {5,12:f7} {6,12:f7}\n",
                    iii, xtemp, V(xtemp), x1, x2, V(x1), V(x2)));
                if (Math.Sign((double)V(x1)) != Math.Sign((double)V(xtemp)))
                {
                    intervals[current].x2 = xtemp;
                }
                else
                {
                    intervals[current].x1 = xtemp;
                }
                iii = iii + 1;

            }
            else if (current < intervals.Length - 1)
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Stygų", x1prad, x2prad, xtemp, tikslumas, iii));
                current++;
                iii = 0;
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         x            G(x)        x1          x2          G(x1)         G(x2)       \n");
            }
            else
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Stygų", x1prad, x2prad, xtemp, tikslumas, iii));

                richTextBox1.AppendText("\nSkaičiavimai baigti.");
                timer7.Stop();
            }
        }
        ///
        /// Gynimo uzduotis
        /// <summary>
        // f1(y) = sqrt(4 - y^2)
        // f2(y) = -y^2
        List<double> y_gynimo = new List<double>();
        private double Z(double y)
        {
            return -Math.Sqrt(4 - Math.Pow(y, 2)) + Math.Pow(y, 2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClearForm1();
            PreparareForm(-10, 10, -10, 10);
            x1 = -2;
            x2 = 2;
            iii = 0;
            Fx = chart1.Series.Add("Z(y)");
            Fx.ChartType = SeriesChartType.Line;
            double x = -5;
            for (int i = 0; i < 250; i++)
            {
                if (x > 5) break;
                Fx.Points.AddXY(x, Z(x));
                x = x + (2 * Math.PI) / 50;
            }
            Fx.BorderWidth = 3;

            X1X2 = chart1.Series.Add("Y1Y2");
            X1X2.MarkerStyle = MarkerStyle.Circle;
            X1X2.MarkerSize = 8;
            X1X2.ChartType = SeriesChartType.Point;
            X1X2.ChartType = SeriesChartType.Line;

            XMid = chart1.Series.Add("Šaknies pozicija");
            XMid.MarkerStyle = MarkerStyle.Circle;
            X1X2.ChartType = SeriesChartType.Point;
            X1X2.ChartType = SeriesChartType.Line;
            XMid.MarkerSize = 8;

            Iv = chart1.Series.Add("Šaknų intervalas");
            Iv.MarkerStyle = MarkerStyle.Cross;
            Iv.MarkerSize = 8;
            Iv.ChartType = SeriesChartType.Point;
            Iv.Points.AddXY(x1, 0);
            Iv.Points.AddXY(x2, 0);

            intervalsArray = new List<Interval>();
            x_nueita = x1;
            zingsnis = 0.5f;
            i_interval = 0;
            richTextBox1.AppendText(string.Format("Skenuojamas intervalas ({0}, {1}) su žingsniu: {2} atskirti šaknų intervalus\n", x1, x2, zingsnis));
            timer8.Enabled = true;
            timer8.Interval = 500;
            timer8.Start();
        }
        private void timer8_Tick(object sender, EventArgs e)
        {
            xtemp = x_nueita + zingsnis;

            X1X2.Points.Clear();
            XMid.Points.Clear();

            X1X2.Points.AddXY(x_nueita, 0);
            X1X2.Points.AddXY(xtemp, 0);

            if (Math.Sign((double)Z(x_nueita)) != Math.Sign((double)Z(xtemp)))
            {
                richTextBox1.AppendText(string.Format("{0}-asis intervalas: ({1}; {2}) \n", ++i_interval, x_nueita, xtemp));
                intervalsArray.Add(new Interval() { x1 = x_nueita, x2 = xtemp, done = false });
            }

            x_nueita = xtemp;

            if (xtemp >= x2)
            {
                timer8.Stop();
                current = 0;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         y            Z(y)        y1          y2          Z(y1)         Z(y2)       \n");
                intervals = intervalsArray.ToArray();
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                timer9.Enabled = true;
                timer9.Interval = 500;
                timer9.Start();
            }
        }
        private void timer9_Tick(object sender, EventArgs e)
        {

            x1 = intervals[current].x1;
            x2 = intervals[current].x2;

            float k = Math.Abs((float)(Z(x1) / Z(x2)));
            xtemp = (x1 + k * x2) / (1 + k);

            double tikslumas = 1e-5;

            if (Math.Abs(Z(xtemp)) > tikslumas & iii <= N)
            {
                X1X2.Points.Clear();
                XMid.Points.Clear();

                X1X2.Points.AddXY(x1, 0);
                X1X2.Points.AddXY(x2, 0);
                XMid.Points.AddXY(xtemp, 0);

                richTextBox1.AppendText(String.Format(" {0,6:d}   {1,12:f7}  {2,12:f7} {3,12:f7} {4,12:f7} {5,12:f7} {6,12:f7}\n",
                    iii, xtemp, Z(xtemp), x1, x2, Z(x1), Z(x2)));
                if (Math.Sign((double)Z(x1)) != Math.Sign((double)Z(xtemp)))
                {
                    intervals[current].x2 = xtemp;
                }
                else
                {
                    intervals[current].x1 = xtemp;
                }
                iii = iii + 1;

            }
            else if (current < intervals.Length - 1)
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Stygų", x1prad, x2prad, xtemp, Z(xtemp), iii));
                y_gynimo.Add(xtemp);
                current++;
                iii = 0;
                x1prad = intervals[current].x1;
                x2prad = intervals[current].x2;
                richTextBox1.AppendText(string.Format("\n{0}-tos šaknies paieška\n", 1 + current));
                richTextBox1.AppendText("Iteracija         y            Z(y)        y1          y2          Z(y1)         Z(y2)       \n");
            }
            else
            {
                richTextBox1.AppendText(string.Format("\n{0}-ta šaknis rasta. Metodas: {1}, pradinis šaknies intervalas: ({2}; {3})," +
                    " gauta šaknis: x={4, 2}, tikslumas: {5}, iteracijų skaičius: {6}\n",
                   1 + current, "Stygų", x1prad, x2prad, xtemp, Z(xtemp), iii));
                y_gynimo.Add(xtemp);
                richTextBox1.AppendText("\nSkaičiavimai baigti.\n\n");
                double[] y_rez = y_gynimo.ToArray();
                richTextBox1.AppendText("Rasti sprendiniai (x, y):\n");
                for (int i = 0; i < y_rez.Length; i++)
                {
                    richTextBox1.AppendText(string.Format("({0}, {1})\n", -Math.Pow(y_rez[i], 2), y_rez[i]));
                }
                timer9.Stop();
            }
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
