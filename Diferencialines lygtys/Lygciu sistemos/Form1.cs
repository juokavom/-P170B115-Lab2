//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//IV projektinė užduotis

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pvz1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        Series z1, z2, p1a, p1b, p2a, p2b, p3a, p3b;
        private void rodytiGrafika(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                chart2.Visible = false;
                chart1.Visible = true;
            }
            else if (radioButton2.Checked)
            {
                chart1.Visible = false;
                chart2.Visible = true;
            }
        }

        private static int m1 = 70, m2 = 15, tg = 40;
        private static double k1 = 0.1, k2 = 5, h = 4000, v = 0, g = 9.8, k = k1, m = m1 + m2, t = 0;
        //step < 2/|alpha|
        private static double step = 0.21;
        private static string line = new string('-', 94);

        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm1();
            PreparareForm(chart1, -10, 120, -10, 4500);
            PreparareForm(chart2, -10, 120, -10, 100);
            //---
            richTextBox1.AppendText("Paprastųjų diferencialinių lygčių sprendimas (7var).\n");
            richTextBox1.AppendText(line + "\n");
            //---
            z1 = chart1.Series.Add("Aukštis h (m)");
            z1.ChartType = SeriesChartType.Line;
            z1.Color = Color.Blue;
            //---
            z2 = chart2.Series.Add("Greitis v (m/s)");
            z2.ChartType = SeriesChartType.Line;
            z2.Color = Color.Green;
            //---
            p1a = chart1.Series.Add("Iššokimas iš lėktuvo");
            p1a.ChartType = SeriesChartType.Point;
            p1a.Color = Color.Black;
            //---
            p1b = chart2.Series.Add("Iššokimas iš lėktuvo");
            p1b.ChartType = SeriesChartType.Point;
            p1b.Color = Color.Black;
            //---
            p2a = chart1.Series.Add("Parašiuto išskleidimas");
            p2a.ChartType = SeriesChartType.Point;
            p2a.Color = Color.Red;
            //---
            p2b = chart2.Series.Add("Parašiuto išskleidimas");
            p2b.ChartType = SeriesChartType.Point;
            p2b.Color = Color.Red;
            //---
            p3a = chart1.Series.Add("Nusileidimas ant žemės");
            p3a.ChartType = SeriesChartType.Point;
            p3a.Color = Color.Green;
            //---
            p3b = chart2.Series.Add("Nusileidimas ant žemės");
            p3b.ChartType = SeriesChartType.Point;
            p3b.Color = Color.Blue;
            //---
            richTextBox1.AppendText(string.Format("Iššokta iš lėktuvo, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h, t, v));
            z1.Points.AddXY(t, h);
            z2.Points.AddXY(t, v);
            p1a.Points.AddXY(t, h);
            p1b.Points.AddXY(t, v);
            //---
            z1.BorderWidth = 1;
            z2.BorderWidth = 1;
            p1a.BorderWidth = 3;
            p1b.BorderWidth = 3;
            p2a.BorderWidth = 3;
            p2b.BorderWidth = 3;
            p3a.BorderWidth = 3;
            p3b.BorderWidth = 3;
            //---
            timer1.Enabled = true;
            timer1.Interval = 1;
            timer1.Start();
            //---
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //---
            if (t >= tg && k == k1)
            {
                k = k2;
                p2a.Points.AddXY(t, h);
                p2b.Points.AddXY(t, v);
                richTextBox1.AppendText(string.Format("Išskleistas parašiutas, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h, t, v));
            }
            //---
            h -= step * (v);
            v += step * (g - ((k * Math.Pow(v, 2)) / m));
            t += step;
            //---
            //System.Diagnostics.Debug.WriteLine(string.Format("it: {3}, t: {2}, Aukstis: {0}, Greitis: {1}", h, v, t, i));
            //---
            z1.Points.AddXY(t, h);
            z2.Points.AddXY(t, v);
            //---
            if (h <= 0)
            {
                richTextBox1.AppendText(string.Format("Nusileidimas ant žemės, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h, t, v));
                richTextBox1.AppendText(line + "\n");
                richTextBox1.AppendText("Programa baigė darbą.\n");
                p3a.Points.AddXY(t, 0);
                p3b.Points.AddXY(t, v);
                timer1.Stop();
            }
        }

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
            chart2.Series.Clear();
        }
    }
}
