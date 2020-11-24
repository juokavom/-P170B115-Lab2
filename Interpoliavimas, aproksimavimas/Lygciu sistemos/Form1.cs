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
using MathNet.Numerics.LinearAlgebra.Double;

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

        //PIRMA UZDUOTIS
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
            return (((b - a)/2)*X) + ((b + a) / 2);
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
            double[,] TT = new double[(int)n, (int)n+1];
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
            int N = (int)Math.Round((X[1] - X[0]) / deltaX);
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
            if (radioButton2.Checked) 
            {
                taskuMetodas = 1;
            }
            //---
            double[] X = { -2, 3 }; //Abscises reziai
            //---
            double[] taskai = taskuRinkinys(n, taskuMetodas, X);
            double[] taskai2 = taskuRinkinys(n+1, taskuMetodas, X);
            //---
            z1 = chart1.Series.Add("Pradinė funkcija");
            z1.ChartType = SeriesChartType.Line;
            z1.Color = Color.Blue;
            //---
            p1 = chart1.Series.Add("Pradiniai taskai");
            p1.ChartType = SeriesChartType.Point;
            p1.Color = Color.Black;
            //---
            z2 = chart1.Series.Add("Gauta funkcija");
            z2.ChartType = SeriesChartType.Line;
            z2.Color = Color.Red;
            //---
            z3 = chart1.Series.Add("Skirtumas");
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
            Ciobysevas(n+1, X, taskai2, null, out XValues2, out FValues2); //Interpoliuojama funkcija su n+1 mazgų
            //---
            for(int i = 1; i < XValues1.Length; i++) //Netikties braižymas
            {
                z3.Points.AddXY(XValues1[i], FValues1[i] - FValues2[i]);
            }
            for (double i = X[0]; i <= X[1] + 0.01; i += 0.01) //Funkcijos braižymas
            {
                z1.Points.AddXY(i, F(i));
            }
        }



        /*
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
//TRECIA UZDUOTIS
private void button5_Click(object sender, EventArgs e)
{
   ClearForm1();
   PreparareForm(-10, 10, -10, 10);
   //---
   double s = 500;
   int count = 6;
   //---
   Random randNum = new Random();
   double[] x = Enumerable.Repeat(0, count).Select(i => randNum.NextDouble()*20-10).ToArray();
   double[] y = Enumerable.Repeat(0, count).Select(i => randNum.NextDouble()*20-10).ToArray();
   x[0] = 0;
   y[0] = 0;

   //double[] x = { 0, 5, 8, 4 }; 
   //double[] y = { 0, 8, 2, 4 };
   //---
   z1 = chart1.Series.Add("Pradiniai kontūrai");
   z1.ChartType = SeriesChartType.Line;
   z1.Color = Color.Blue;
   //---
   p1 = chart1.Series.Add("Pradiniai taskai");
   p1.ChartType = SeriesChartType.Point;
   p1.Color = Color.Black;
   //---
   z2 = chart1.Series.Add("Galutiniai kontūrai");
   z2.ChartType = SeriesChartType.Line;
   z2.Color = Color.Red;
   //---
   p2 = chart1.Series.Add("Galutiniai taskai");
   p2.ChartType = SeriesChartType.Point;
   p2.Color = Color.Blue;
   //---
   for (int i = 0; i < x.Length; i++)
   {
       p1.Points.AddXY(x[i], y[i]);
       for (int u = i+1; u < x.Length; u++)
       {
           z1.Points.AddXY(x[i], y[i]);
           z1.Points.AddXY(x[u], y[u]);
           p1.Points.AddXY(x[u], y[u]);
           z1.Points.AddXY(x[i], y[i]);
       }
   }
   z1.BorderWidth = 1;
   p1.BorderWidth = 3;
   p2.BorderWidth = 3;
   z2.BorderWidth = 1;

   //Sprendimas
   optimization(x, y, s);
}
public void printPoints(double[] x, double[] y)
{
   z2.Points.Clear();
   p2.Points.Clear();
   for (int i = 0; i < x.Length; i++)
   {
       p1.Points.AddXY(x[i], y[i]);
       for (int u = i + 1; u < x.Length; u++)
       {
           z2.Points.AddXY(x[i], y[i]);
           z2.Points.AddXY(x[u], y[u]);
           p2.Points.AddXY(x[u], y[u]);
           z2.Points.AddXY(x[i], y[i]);
       }
   }
}

private void optimization(double[] x, double[] y, double s) 
{
   double eps = 1e-6;
   int maxIter = 500;
   double zingsnis = 0.1;
   double tikslumas = Double.MaxValue;
   int iteracija = 0;

   for (; iteracija < maxIter; iteracija++) 
   {
       //printPoints(x, y);
       double vid = vidurkis(x, y);
       int n = x.Length;
       double[,] grad = gradientas(x, y, vid, s);
       double f0 = tikslo(x, y, vid, s);
       double[,] deltaX = gradiento_norma(grad, zingsnis);
       for(int u = 1; u < n; u++)
       {
           x[u] -= deltaX[u, 0];
           y[u] -= deltaX[u, 1];
       }
       double f1 = tikslo(x, y, vid, s);
       if (f1 > f0)
       {
           for (int u = 1; u < n; u++)
           {
               x[u] += deltaX[u, 0];
               y[u] += deltaX[u, 1];
           }
           zingsnis /= 2;
       }
       else 
       {
           zingsnis *= 2;
       }
       tikslumas = Math.Abs(f0-f1)/(Math.Abs(f0)+Math.Abs(f1));
       if (tikslumas < eps)
       {
           richTextBox1.AppendText("Baigta sekmingai\n");
           break;
       }
       else if (iteracija == maxIter - 1)
       {
           richTextBox1.AppendText("Baigta nesekmingai\n");
       }
       richTextBox1.AppendText(string.Format("Iteracija: {0}, tikslumas: {1}, tikslo f-ija: {2, 0:F2}\n", iteracija, tikslumas, f1));
   }
   richTextBox1.AppendText(string.Format("Iteracijų skaičius = {0}, tikslumas = {1}\n", iteracija, tikslumas));
   printPoints(x, y);
}

private double[,] gradiento_norma(double[,] gradientas, double zingsnis)
{
   double suma = 0;
   for (int i = 0; i < gradientas.GetLength(0); i++)
   {
       for (int u = 0; u < gradientas.GetLength(1); u++)
       {
           suma += Math.Pow(gradientas[i, u], 2);
       }
   }
   double normale = Math.Sqrt(suma);
   double[,] copy = new double[gradientas.GetLength(0), gradientas.GetLength(1)];
   for (int i = 0; i < gradientas.GetLength(0); i++)
   {
       for (int u = 0; u < gradientas.GetLength(1); u++)
       {
           copy[i, u] = gradientas[i, u] / normale * zingsnis;
       }
   }
   return copy;
}

private double vidurkis(double[] x, double[] y)
{
   double n = x.Length;
   double suma = 0;
   int count = 0;
   for (int i = 0; i < n; i++) 
   {
       for (int u = i + 1; u < n; u++) 
       {
           suma += Math.Sqrt(Math.Pow(x[u]-x[i], 2) + Math.Pow(y[u] - y[i], 2));
           count++;
       }
   }
   return suma / count;
}
private double ilgis(double[] x, double[] y)
{
   double n = x.Length;
   double suma = 0;
   for (int i = 0; i < n; i++)
   {
       for (int u = i + 1; u < n; u++)
       {
           suma += Math.Sqrt(Math.Pow(x[u] - x[i], 2) + Math.Pow(y[u] - y[i], 2));
       }
   }
   return suma;
}

private double[,] gradientas(double[] x, double[] y, double vid, double s)
{
   int n = x.Length;
   double zingsnis = 0.0001;
   double[,] grad = new double[n, 2]; 
   double f0 = tikslo(x, y, vid, s);

   for (int i = 0; i < n; i++)
   {
       grad[i, 0] = (tikslo(f1(x, i, zingsnis), y, vid, s) - f0) / zingsnis;
       grad[i, 1] = (tikslo(x, f1(y, i, zingsnis), vid, s) - f0) / zingsnis;
   }

   return grad;
}

private double[] f1(double[] a, int i, double zing)
{
   int n = a.Length;
   double[] copy = new double[n];
   Array.Copy(a, copy, n);
   copy[i] += zing;
   return copy;
}

private double tikslo(double[] x, double[] y, double vid, double s)
{
   int n = x.Length;
   double suma = 0;
   for (int i = 0; i < n; i++)
   {
       for (int u = i + 1; u < n; u++)
       {
           suma += Math.Pow(Math.Sqrt(Math.Pow(x[u] - x[i], 2) + Math.Pow(y[u] - y[i], 2)) - vid, 2);
       }
   }
   return suma + Math.Abs(ilgis(x, y) - s);
}
*/
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
