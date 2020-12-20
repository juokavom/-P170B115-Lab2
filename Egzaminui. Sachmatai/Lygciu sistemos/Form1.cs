//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//Egzamino užduotis

using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pvz1
{
    public partial class Form1 : Form
    {
        static int[] BK = {0, 4};
        static int[] XY = { 8, 8};
        public Form1()
        {
            InitializeComponent();
            Initialize();

            Solution();
        }
        Series z1, z2, p1;
        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm1();
            //---
            richTextBox1.AppendText("Paleidimas\n");
            //---
            /*PictureBox pb = new PictureBox();
            pb.Location = new Point(0, 0);
            pb.Size = new Size(400, 400);
            pb.Image = Image.FromFile(@"Images/WK.jpg");
            pb.Visible = true;
            chart1.Controls.Add(pb);
            */
            //---
            //---
            //---
            //---
        }
        private void p(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
        private void Solution() 
        {
            p("Hello, world!");
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