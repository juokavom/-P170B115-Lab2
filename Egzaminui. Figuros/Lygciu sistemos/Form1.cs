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
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        Series z1, p1;
        
        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm1();
            //---
            PreparareForm(-10, 120, -10, 100);
            richTextBox1.AppendText("Paleidimas\n");
            //---
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