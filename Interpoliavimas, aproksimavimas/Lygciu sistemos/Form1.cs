//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//III projektinė užduotis

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
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;

namespace Pvz1
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        Series z1, p1, z2, z3;
        //------------------------------------------------------------PIRMA UZDUOTIS----------------------------------------------------------------------------
        private double F(double x)
        {
            return Math.Cos(2 * x) * (Math.Sin(2 * x) + 1.5) - Math.Cos(x / 5);
        }
        private double T(double x, int j)
        {
            if (j == 0)
            {
                return 1;
            }
            else if (j == 1)
            {
                return x;
            }
            else
            {
                return 2 * x * T(x, j - 1) - T(x, j - 2);
            }
        }
        private double CiobysevoForma(double X, double a, double b)
        {
            return ((2 * X) / (b - a)) - ((b + a) / (b - a));
        }
        private double NormaliForma(double X, double a, double b)
        {
            return (((b - a) / 2) * X) + ((b + a) / 2);
        }
        static void Gausas(double[,] a, int n)
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
        private void Ciobysevas(double n, double[] X, double[] taskai, Series z, out double[] XValues, out double[] FValues)
        {
            //Čiobyševo daugianarių suvedimas į matricą su y reikšme paskutiniame stulpelyje
            double[,] TT = new double[(int)n, (int)n + 1];
            for (int i = 0; i < TT.GetLength(0); i++)
            {
                for (int u = 0; u < TT.GetLength(1) - 1; u++)
                {
                    TT[i, u] = T(CiobysevoForma(taskai[i], X[0], X[1]), u);
                }
                TT[i, TT.GetLength(1) - 1] = F(taskai[i]);
            }
            //Čiobyševo daugianarių matricos sprendimas gauso metodu
            Gausas(TT, (int)n);
            //Išsprendus matricą galime gauti daugiklių reikšmes
            double[] AValues = new double[(int)n];
            for (int i = 0; i < AValues.Length; i++)
            {
                AValues[i] = TT[i, (int)n] / TT[i, i];
            }
            //Interpoliuotos funkcijos reikšmių surašymas į masyvus pagal (x) reikšmes
            double deltaX = 0.1;
            int N = (int)Math.Round((X[1] - X[0]) / deltaX) + 1;
            XValues = new double[N];
            FValues = new double[N];
            for (int i = 0; i < N; i++)
            {
                XValues[i] = CiobysevoForma(X[0] + i * deltaX, X[0], X[1]);
                FValues[i] = 0;
                for (int u = 0; u < (int)n; u++)
                {
                    FValues[i] += T(XValues[i], u) * AValues[u];
                }
            }
            //Gautos interpoliuotos funkcijos braižymas ekrane
            if (z != null)
            {
                for (int i = 0; i < FValues.Length; i++)
                {
                    XValues[i] = NormaliForma(XValues[i], X[0], X[1]);
                    z.Points.AddXY(XValues[i], FValues[i]);
                }
            }
        }
        private double[] taskuRinkinys(double n, int pozymis, double[] X)
        {
            double[] taskai = new double[(int)n];
            if (pozymis == 0)
            {
                //---Taskai pasiskirste tolygiai
                double zingsnis = (X[1] - X[0]) / (n - 1);
                for (int i = 0; i < n; i++)
                {
                    taskai[i] = X[0] + zingsnis * i;
                }
            }
            else
            {
                //---Taskai pasiskirste pagal ciobyseva
                for (int i = 0; i < n; i++)
                {
                    taskai[i] = ((X[1] - X[0]) / 2) * Math.Cos(Math.PI * (2 * i + 1) / (2 * n)) + ((X[1] + X[0]) / 2);
                }
            }
            return taskai;
        }
        private void button2_Click(object sender, EventArgs e)
        {

            ClearForm1();
            PreparareForm(-3, 4, -4, 2);
            //---
            //double n = 15; //Tasku skaicius
            double n = (double)numericUpDown1.Value;
            int taskuMetodas = 0; //0 - tolygiai, 1 - pagal ciobyseva
            richTextBox1.AppendText("Pirma užduotis\n");
            richTextBox1.AppendText("Taškų kiekis = " + n + "\n");
            if (radioButton2.Checked)
            {
                richTextBox1.AppendText("Taškai apskaičiuojami naudojant Čiobyševo abscises.\n");
                taskuMetodas = 1;
            }
            else 
            {
                richTextBox1.AppendText("Taškai pasiskirstę tolygiai.\n");
            }
            //---
            double[] X = { -2, 3 }; //Abscises reziai
            //---
            double[] taskai = taskuRinkinys(n, taskuMetodas, X);
            double[] taskai2 = taskuRinkinys(n + 1, taskuMetodas, X);
            //---
            z1 = chart1.Series.Add("Pradinė funkcija");
            z1.ChartType = SeriesChartType.Line;
            z1.Color = Color.Blue;
            //---
            p1 = chart1.Series.Add("Pradiniai taškai");
            p1.ChartType = SeriesChartType.Point;
            p1.Color = Color.Black;
            //---
            z2 = chart1.Series.Add("Gauta funkcija");
            z2.ChartType = SeriesChartType.Line;
            z2.Color = Color.Red;
            //---
            z3 = chart1.Series.Add("Netiktis");
            z3.ChartType = SeriesChartType.Line;
            z3.Color = Color.Black;
            //---      
            for (int i = 0; i < taskai.Length; i++)
            {
                p1.Points.AddXY(taskai[i], F(taskai[i]));
            }
            //---
            z1.BorderWidth = 1;
            p1.BorderWidth = 3;
            z2.BorderWidth = 1;
            //---
            double[] XValues1;
            double[] XValues2;
            double[] FValues1;
            double[] FValues2;
            //---
            Ciobysevas(n, X, taskai, z2, out XValues1, out FValues1); //Interpoliuojama funkcija su n mazgų
            Ciobysevas(n + 1, X, taskai2, null, out XValues2, out FValues2); //Interpoliuojama funkcija su n+1 mazgų
            //---
            for (int i = 1; i < XValues1.Length; i++) //Netikties braižymas
            {
                z3.Points.AddXY(XValues1[i], FValues1[i] - FValues2[i]);
            }
            for (double i = X[0]; i <= X[1] + 0.01; i += 0.01) //Funkcijos braižymas
            {
                z1.Points.AddXY(i, F(i));
            }
        }
        //------------------------------------------------------------ANTRA UZDUOTIS----------------------------------------------------------------------------
        private void Ciobysevas(double[] X, double[] taskaiX, double[] taskaiY, Series z)
        {
            //Čiobyševo daugianarių suvedimas į matricą su y reikšme paskutiniame stulpelyje
            int n = taskaiX.Length;
            double[,] TT = new double[n, n + 1];
            for (int i = 0; i < TT.GetLength(0); i++)
            {
                for (int u = 0; u < TT.GetLength(1) - 1; u++)
                {
                    TT[i, u] = T(CiobysevoForma(taskaiX[i], X[0], X[1]), u);
                }
                TT[i, TT.GetLength(1) - 1] = taskaiY[i];
            }
            //Čiobyševo daugianarių matricos sprendimas gauso metodu
            Gausas(TT, n);
            //Išsprendus matricą galime gauti daugiklių reikšmes
            double[] AValues = new double[n];
            for (int i = 0; i < AValues.Length; i++)
            {
                AValues[i] = TT[i, n] / TT[i, i];
            }
            //Interpoliuotos funkcijos reikšmių surašymas į masyvus pagal (x) reikšmes
            double deltaX = 0.1;
            int N = (int)Math.Round((X[1] - X[0]) / deltaX) + 1;
            double[] XValues = new double[N];
            double[] FValues = new double[N];
            for (int i = 0; i < N; i++)
            {
                XValues[i] = CiobysevoForma(X[0] + i * deltaX, X[0], X[1]);
                FValues[i] = 0;
                for (int u = 0; u < n; u++)
                {
                    FValues[i] += T(XValues[i], u) * AValues[u];
                }
            }
            //Gautos interpoliuotos funkcijos braižymas ekrane
            if (z != null)
            {
                for (int i = 0; i < FValues.Length; i++)
                {
                    XValues[i] = NormaliForma(XValues[i], X[0], X[1]);
                    z.Points.AddXY(XValues[i], FValues[i]);
                }
            }
        }
        private void GlobalusSplainas(double[] x, double[] y, Series z)
        {
            int n = x.Length;
            double[] d = new double[n - 1];
            for (int i = 0; i < d.Length; i++) d[i] = x[i + 1] - x[i];
            double[,] T = new double[n - 2, n];
            double[] YY = new double[n - 2];
            //Splaino daugianarių matricos [n-2 x n] sudarymas
            for (int i = 0; i < T.GetLength(0); i++) 
            {
                for (int u = 0; u < T.GetLength(1); u++) T[i, u] = 0;
                T[i, i] = d[i] / 6;
                T[i, i + 1] = (d[i] + d[i + 1]) / 3;
                T[i, i + 2] = d[i + 1] / 6;
                YY[i] = ((y[i + 2] - y[i + 1]) / d[i + 1]) - ((y[i + 1] - y[i]) / d[i]);
            }
            double[,] TT = new double[n - 2, n - 1];
            //Splaino daugianarių matricos be 1 ir n-1 reiksmes stulpeliu pervedimas gauso metodu spręsti ir rezultatais paskutiniame stulpelyje
            for (int i = 0; i < TT.GetLength(0); i++) 
            {
                for (int u = 0; u < TT.GetLength(1) - 1; u++)
                {
                    TT[i, u] = T[i, u + 1];
                }
                TT[i, n - 2] = YY[i];
            }
            //Splaino daugianarių TLS sprendžiama gauso metodu
            Gausas(TT, n - 2); 
            double[] f_2 = new double[n];
            f_2[0] = 0;
            f_2[n - 1] = 0;
            //Apskaičiuojamos antros eilės išvestinės iš apskaičiuotos matricos ir y reikšmių vektoriaus
            for (int i = 0; i < n - 2; i++) 
            {
                f_2[i + 1] = TT[i, n - 2] / T[i, i];
            }
            for (int i = 0; i < n - 1; i++)
            {
                double xmax = x[i + 1];
                double xmin = x[i];
                double deltaX = 0.01;
                int N = (int)Math.Round((xmax - xmin) / deltaX) + 1;

                double[] XValues = new double[N];
                double[] FValues = new double[N];
                //Pagal apskaičiuotas antros eilės išvestines formuojamos vaizdavimo taškų reikšmės XValues ir FValues vektoriuose
                for (int u = 0; u < N; u++) 
                {
                    XValues[u] = xmin + u * deltaX;
                    double s = XValues[u] - x[i];
                    FValues[u] = (f_2[i] * (Math.Pow(s, 2) / 2)) - (f_2[i] * (Math.Pow(s, 3) / (6 * d[i]))) + (f_2[i + 1] * (Math.Pow(s, 3) / (6 * d[i]))) + (((y[i + 1] - y[i]) / d[i]) * s) - (f_2[i] * (d[i] / 3) * s) - (f_2[i + 1] * (d[i] / 6) * s) + y[i];
                }

                if (z != null)
                {
                    //Gautos interpoliuotos funkcijos braižymas ekrane
                    for (int u = 0; u < FValues.Length; u++) 
                    {
                        z.Points.AddXY(XValues[u], FValues[u]); 
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearForm1();
            PreparareForm(-1, 13, 17, 23);
            double[] X = { 0, 11 }; //Abscises reziai
            double[] taskaiX = taskuRinkinys(12, 0, X); //Taskai suskirstomi pagal - Tiesiogiai (opt.: 0), Čiobyševo abscises (opt.: 1)
            double[] taskaiY = File.ReadLines(@"Data/PER_2008.txt").Select(l => l.Split('\n')).Select(l2 => l2[0].Split(',')).Select(l3 => Double.Parse(l3[0])).ToArray(); //Nuskaitomos temperatūrų reikšmės
            //---
            p1 = chart1.Series.Add("Pradiniai taskai");
            p1.ChartType = SeriesChartType.Point;
            p1.Color = Color.Black;
            //---
            z2 = chart1.Series.Add("Gauta funkcija");
            z2.ChartType = SeriesChartType.Line;
            z2.Color = Color.Red;
            //---      
            for (int i = 0; i < taskaiX.Length; i++)
            {
                p1.Points.AddXY(taskaiX[i], taskaiY[i]);
            }
            //---
            p1.BorderWidth = 3;
            z2.BorderWidth = 1;
            //---               
            richTextBox1.AppendText("Antra užduotis\n");
            if (radioButton3.Checked)
            {
                Ciobysevas(X, taskaiX, taskaiY, z2);
                richTextBox1.AppendText("Sprendžiama Čiobyševo metodu.\n");
            }
            else if (radioButton4.Checked)
            {
                GlobalusSplainas(taskaiX, taskaiY, z2);
                richTextBox1.AppendText("Sprendžiama Globalaus splaino metodu.\n");
            }
        }
        //------------------------------------------------------------TRECIA UZDUOTIS----------------------------------------------------------------------------


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
