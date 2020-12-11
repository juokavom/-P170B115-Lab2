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
            pradinesReiksmes(0.21);
        }

        Series z1, z2, p1a, p1b, p2a, p2b, p3a, p3b;

        private static int m1, m2, tg;
        private static double k1, k2, h, v, g, k, m, t, step;

        //step < 2/|alpha|
        private static string line = new string('-', 94);

        private void pradinesReiksmes(double local_step)
        {
            m1 = 70;
            m2 = 15;
            tg = 40;
            k1 = 0.1;
            k2 = 5;
            h = 4000;
            v = 0;
            g = 9.8;
            k = k1;
            m = m1 + m2;
            t = 0;
            step = local_step;
        }

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
            if (radioButton3.Checked) richTextBox1.AppendText("Sprendžiama Eulerio metodu\n");
            else if (radioButton4.Checked) richTextBox1.AppendText("Sprendžiama 4 eilės Rungės ir Kutos metodu\n");
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

        private double f(double v_local)
        {
            return g - ((k * Math.Pow(v_local, 2)) / m);
        }
        private double Eulerio()
        {
            return v + step * f(v);
        }
        private double RK()
        {
            double v1 = v + (step / 2) * f(v);
            double v2 = v + (step / 2) * f(v1);
            double v3 = v + step * f(v2);
            double v4 = v + (step / 6) * (f(v) + 2 * f(v1) + 2 * f(v2) + f(v3));
            return v4;
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
            if (radioButton3.Checked) v = Eulerio();
            else if (radioButton4.Checked) v = RK();
            t += step;
            //---
            z1.Points.AddXY(t, h);
            z2.Points.AddXY(t, v);
            //---
            if (h <= 0)
            {
                richTextBox1.AppendText(string.Format("Nusileidimas ant žemės, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h, t, v));
                richTextBox1.AppendText(line + "\n");
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
            pradinesReiksmes(0.21);
            // isvalomos visos nubreztos kreives
            chart1.Series.Clear();
            chart2.Series.Clear();
        }
    }
}
