//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//IV projektinė užduotis

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
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

        Series z1a, z1b, z2a, z2b, p1a, p1b, p2a, p2b, p3a, p3b;
        private static string line = new string('-', 102);
        private double[] t_step_pilnas, h_step_pilnas, v_step_pilnas;
        private double[] t_step_pusiau, h_step_pusiau, v_step_pusiau;
        int it_iskleidimas_pilnas, it_nusileidimas_pilnas, it_pilnas;
        int it_iskleidimas_pusiau, it_nusileidimas_pusiau, it_pusiau;
        bool uzbaigta;
        private double f(double v, double k)
        {
            int m1 = 70, m2 = 15;
            double g = 9.8, m = m1 + m2;
            //---
            return g - ((k * Math.Pow(v, 2)) / m);
        }
        private double Eulerio(double v, double step, double k)
        {
            return v + step * f(v, k);
        }
        private double RK(double v, double step, double k)
        {
            double v1 = v + (step / 2) * f(v, k);
            double v2 = v + (step / 2) * f(v1, k);
            double v3 = v + step * f(v2, k);
            double v4 = v + (step / 6) * (f(v, k) + 2 * f(v1, k) + 2 * f(v2, k) + f(v3, k));
            return v4;
        }
        private void Sprendimas(double step, ref double[] t_array, ref double[] h_array, ref double[] v_array, out int it_iskleidimas, out int it_nusileidimas)
        {
            double k1 = 0.1, k2 = 5, h = 4000, tg = 40, v = 0, k = k1, t = 0;
            //---
            List<double> h_list = new List<double>();
            List<double> v_list = new List<double>();
            List<double> t_list = new List<double>();
            //---
            it_iskleidimas = 0;
            it_nusileidimas = 0;
            //---
            h_list.Add(h);
            v_list.Add(v);
            t_list.Add(t);
            //---
            for (int i = 0; i < 5000; i++)
            {
                if (t >= tg && k == k1)
                {
                    k = k2;
                    it_iskleidimas = i;
                }
                //---
                h -= step * v;
                if (radioButton3.Checked) v = Eulerio(v, step, k);
                else if (radioButton4.Checked) v = RK(v, step, k);
                t += step;
                //---
                if (h > 0)
                {
                    h_list.Add(h);
                    v_list.Add(v);
                    t_list.Add(t);
                }
                else
                {
                    it_nusileidimas = i;
                    t_array = t_list.ToArray();
                    h_array = h_list.ToArray();
                    v_array = v_list.ToArray();
                    break;
                }
            }
        }
        private double tikslumas()
        {
            int pilnas = t_step_pilnas.Length;
            int pusiau = t_step_pusiau.Length;
            int it_size = (pilnas > pusiau/2) ? pusiau/2 : pilnas;
            double suma = 0;
            for (int i = 0; i < it_size; i++)
            {
                suma += (v_step_pilnas[i] - v_step_pusiau[i * 2]);
            }
            return Math.Abs(suma / it_size);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm1();
            //---
            //0,34 < runges ir kutos
            //0,21 < eulerio
            //double step = 0.2;
            double step = Double.Parse((string)listBox1.SelectedItem);
            //---
            PreparareForm(chart1, -10, 120, -10, 4500);
            PreparareForm(chart2, -10, 120, -10, 100);
            //---
            richTextBox1.AppendText("Paprastųjų diferencialinių lygčių sprendimas (7var).\n");
            richTextBox1.AppendText(line + "\n");
            //---
            z1a = chart1.Series.Add("Aukštis h (m), žingsnis = step");
            z1a.ChartType = SeriesChartType.Line;
            z1a.Color = Color.Blue;
            //---
            z1b = chart1.Series.Add("Aukštis h (m), žingsnis = step/2");
            z1b.ChartType = SeriesChartType.Line;
            z1b.Color = Color.Green;
            //---
            z2a = chart2.Series.Add("Greitis v (m/s), žingsnis = step");
            z2a.ChartType = SeriesChartType.Line;
            z2a.Color = Color.Blue;
            //---
            z2b = chart2.Series.Add("Greitis v (m/s), žingsnis = step/2");
            z2b.ChartType = SeriesChartType.Line;
            z2b.Color = Color.Green;
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
            p3a.Color = Color.DeepPink;
            //---
            p3b = chart2.Series.Add("Nusileidimas ant žemės");
            p3b.ChartType = SeriesChartType.Point;
            p3b.Color = Color.DeepPink;
            //---
            Sprendimas(step, ref t_step_pilnas, ref h_step_pilnas, ref v_step_pilnas, out it_iskleidimas_pilnas, out it_nusileidimas_pilnas);
            Sprendimas(step / 2, ref t_step_pusiau, ref h_step_pusiau, ref v_step_pusiau, out it_iskleidimas_pusiau, out it_nusileidimas_pusiau);
            //---     
            richTextBox1.AppendText("žingsnis(step) = " + step + "\n");
            if (radioButton3.Checked) richTextBox1.AppendText("Sprendžiama Eulerio metodu\n");
            else if (radioButton4.Checked) richTextBox1.AppendText("Sprendžiama 4 eilės Rungės ir Kutos metodu\n");
            richTextBox1.AppendText("tikslumas = " + tikslumas() + "\n");
            richTextBox1.AppendText(line + "\n");
            //---
            z1a.BorderWidth = 1;
            z2a.BorderWidth = 1;
            z1b.BorderWidth = 1;
            z2b.BorderWidth = 1;
            p1a.BorderWidth = 3;
            p1b.BorderWidth = 3;
            p2a.BorderWidth = 3;
            p2b.BorderWidth = 3;
            p3a.BorderWidth = 3;
            p3b.BorderWidth = 3;
            //---
            uzbaigta = false;
            it_pilnas = 0;
            timer1.Enabled = true;
            timer1.Interval = 1;
            timer1.Start();
            //---   
            uzbaigta = false;
            it_pusiau = 0;
            timer2.Enabled = true;
            timer2.Interval = 1;
            timer2.Start();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            int i = it_pilnas;
            z1a.Points.AddXY(t_step_pilnas[i], h_step_pilnas[i]);
            z2a.Points.AddXY(t_step_pilnas[i], v_step_pilnas[i]);
            if (i == 0)
            {
                richTextBox1.AppendText(string.Format("(step)     Iššokta iš lėktuvo, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h_step_pilnas[0], t_step_pilnas[0], v_step_pilnas[0]));
                p1a.Points.AddXY(t_step_pilnas[0], h_step_pilnas[0]);
                p1b.Points.AddXY(t_step_pilnas[0], v_step_pilnas[0]);
            }
            else if (i == it_iskleidimas_pilnas)
            {
                richTextBox1.AppendText(string.Format("(step)     Išskleistas parašiutas, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h_step_pilnas[i], t_step_pilnas[i], v_step_pilnas[i]));
                p2a.Points.AddXY(t_step_pilnas[i], h_step_pilnas[i]);
                p2b.Points.AddXY(t_step_pilnas[i], v_step_pilnas[i]);
            }
            else if (i == it_nusileidimas_pilnas)
            {
                richTextBox1.AppendText(string.Format("(step)     Nusileidimas ant žemės, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h_step_pilnas[i], t_step_pilnas[i], v_step_pilnas[i]));
                p3a.Points.AddXY(t_step_pilnas[i], h_step_pilnas[i]);
                p3b.Points.AddXY(t_step_pilnas[i], v_step_pilnas[i]);
                if (uzbaigta) richTextBox1.AppendText(line + "\n");
                uzbaigta = true;
                timer1.Stop();
            }
            it_pilnas++;
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            int i = it_pusiau;
            z1b.Points.AddXY(t_step_pusiau[i], h_step_pusiau[i]);
            z2b.Points.AddXY(t_step_pusiau[i], v_step_pusiau[i]);
            if (i == 0)
            {
                richTextBox1.AppendText(string.Format("(step)/2   Iššokta iš lėktuvo, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h_step_pusiau[0], t_step_pusiau[0], v_step_pusiau[0]));
                p1a.Points.AddXY(t_step_pusiau[0], h_step_pusiau[0]);
                p1b.Points.AddXY(t_step_pusiau[0], v_step_pusiau[0]);
            }
            else if (i == it_iskleidimas_pusiau)
            {
                richTextBox1.AppendText(string.Format("(step)/2   Išskleistas parašiutas, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h_step_pusiau[i], t_step_pusiau[i], v_step_pusiau[i]));
                p2a.Points.AddXY(t_step_pusiau[i], h_step_pusiau[i]);
                p2b.Points.AddXY(t_step_pusiau[i], v_step_pusiau[i]);
            }
            else if (i == it_nusileidimas_pusiau)
            {
                richTextBox1.AppendText(string.Format("(step)/2   Nusileidimas ant žemės, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", h_step_pusiau[i], t_step_pusiau[i], v_step_pusiau[i]));
                p3a.Points.AddXY(t_step_pusiau[i], h_step_pusiau[i]);
                p3b.Points.AddXY(t_step_pusiau[i], v_step_pusiau[i]);
                if(uzbaigta) richTextBox1.AppendText(line + "\n");
                uzbaigta = true;
                timer2.Stop();
            }
            it_pusiau++;
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
            chart2.Series.Clear();
        }
    }
}