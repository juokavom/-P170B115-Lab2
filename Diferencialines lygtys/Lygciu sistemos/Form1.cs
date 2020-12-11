//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//IV projektinė užduotis

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

        Series z1, z2;

        private void rodytiGrafika(object sender, EventArgs e)
        {
            if (radioButton1.Checked) 
            {
                chart2.Visible = false;
                chart1.Visible = true;
            } else if (radioButton2.Checked) 
            {
                chart1.Visible = false;
                chart2.Visible = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

            ClearForm1();
            PreparareForm(chart1, -2, 120, -2, 4500);
            PreparareForm(chart2, -2, 120, -2, 100);
            //---
            int m1 = 70, m2 = 15, tg = 40;
            double k1 = 0.1, k2 = 5, h = 4000, v = 0, g = 9.8, k = k1, m = m1 + m2, t = 0;
            double step = 0.03;
            //---
            z1 = chart1.Series.Add("Aukštis (h)");
            z1.ChartType = SeriesChartType.Line;
            z1.Color = Color.Blue;
            //---
            z2 = chart2.Series.Add("Greitis (v)");
            z2.ChartType = SeriesChartType.Line;
            z2.Color = Color.Green;
            //---
            z1.Points.AddXY(0, h);
            z2.Points.AddXY(0, v);
            for (double i = 0; h > 0; i++)
            {
                if (t >= tg) k = k2;
                h -= step * (v);
                v += step * (g - ((k * Math.Pow(v, 2)) / m)); 
                t += step;
                System.Diagnostics.Debug.WriteLine(string.Format("it: {3}, t: {2}, Aukstis: {0}, Greitis: {1}", h, v, t, i));
                z1.Points.AddXY(t, h);
                z2.Points.AddXY(t, v);
                if (i == 100000) break;
            }

            z1.BorderWidth = 1;
            z2.BorderWidth = 1;
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
