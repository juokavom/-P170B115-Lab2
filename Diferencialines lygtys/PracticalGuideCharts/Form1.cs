using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
// parengta pagal Jack Xu. 2007. Practical C# Charts and Graphics. Unicomp, Incorporated.
namespace PracticalGuideCharts
{
    public partial class Form1 : Form
    {
        ChartStyle cs;
        ChartStyle2D cs2d;
        DataSeries ds;
        DrawChart dc;
        ChartFunctions cf;
        ColorMap cm;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            // Subscribing to a paint eventhandler to drawingPanel:
            PlotPanel.Paint += new PaintEventHandler(PlotPanelPaint);
            // Specify chart style parameters:
            cs = new ChartStyle(this);
            cs2d = new ChartStyle2D(this);
            ds = new DataSeries();

            dc = new DrawChart(this);
            dc.IsInterp = true;
            dc.NumberInterp = 5;
            cf = new ChartFunctions();
            cm = new ColorMap();

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (dc.ChartType == DrawChart.ChartTypeEnum.XYColor ||
            dc.ChartType == DrawChart.ChartTypeEnum.Contour ||
            dc.ChartType == DrawChart.ChartTypeEnum.FillContour)
            {
                Rectangle rect = this.ClientRectangle;
                cs2d.ChartArea = new Rectangle(rect.X, rect.Y, 19 * rect.Width / 30, 19 * rect.Height / 30);
                //cs2d.ChartArea = new Rectangle(PlotPanel.Location.X, PlotPanel.Location.Y, PlotPanel.Size.Width * 15 / 30, PlotPanel.Size.Height * 15 / 30);
                cf.Peak3D(ds, cs);
                //cf.SinROverR3D(ds,cs);
                cs2d.SetPlotArea(g, cs);
                dc.AddColorBar(g, ds, cs, cs2d);
            }
        }
        private void PlotPanelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            if (dc.ChartType == DrawChart.ChartTypeEnum.XYColor ||
            dc.ChartType == DrawChart.ChartTypeEnum.Contour ||
            dc.ChartType == DrawChart.ChartTypeEnum.FillContour)
            {
                cf.Peak3D(ds, cs);
                cs2d.AddChartStyle2D(g, cs);
                dc.AddChart(g, ds, cs, cs2d);
            }
            else
            {

                if (var1.Checked) cf.F13D(ds, cs);
                else if (var2.Checked) cf.F23D(ds, cs);

                cs.Elevation = trkElevation.Value;
                cs.Azimuth = trkAzimuth.Value;
                //cf.Peak3D(ds, cs);
                //cf.SinROverR3D(ds, cs);
                cs.AddChartStyle(g);
                dc.AddChart(g, ds, cs, cs2d);
            }
        }

        private void trkElevation_Scroll(object sender, EventArgs e)
        {
            tbElevation.Text = trkElevation.Value.ToString();
            PlotPanel.Invalidate();

        }

        private void trkAzimuth_Scroll(object sender, EventArgs e)
        {
            tbAzimuth.Text = trkAzimuth.Value.ToString();
            PlotPanel.Invalidate();
        }

        private void tbElevation_KeyUp(object sender, KeyEventArgs e)
        {
            int value;
            bool result = Int32.TryParse(tbElevation.Text, out value);
            if (result)
            {
                if (value <= -90)
                    value = -90;
                else if (value >= 90)
                    value = 90;
                trkElevation.Value = value;
            }
            PlotPanel.Invalidate();
        }

        private void tbAzimuth_KeyUp(object sender, KeyEventArgs e)
        {
            int value;
            bool result = Int32.TryParse(tbAzimuth.Text, out value);
            if (result)
            {
                if (value <= -180)
                    value = -180;
                else if (value >= 180)
                    value = 180;
                trkAzimuth.Value = value;
            }
            PlotPanel.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            dc.ChartType = DrawChart.ChartTypeEnum.Bar3D;
            Prepare3DAxis();
            PlotPanel.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            PlotPanel.Size = new Size(0, 0);
            dc.ChartType = DrawChart.ChartTypeEnum.Contour;
            ds.LineStyle.Thickness = 3;
            Prepare3DAxis();
            //PlotPanel.Invalidate();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            PlotPanel.Size = new Size(0, 0);
            dc.ChartType = DrawChart.ChartTypeEnum.FillContour;
            Prepare3DAxis();
            //PlotPanel.Invalidate();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void button6_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            dc.ChartType = DrawChart.ChartTypeEnum.Mesh;
            Prepare3DAxis();
            PlotPanel.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            dc.ChartType = DrawChart.ChartTypeEnum.MeshContour;
            Prepare3DAxis();
            PlotPanel.Invalidate();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            dc.ChartType = DrawChart.ChartTypeEnum.MeshZ;
            Prepare3DAxis();
            PlotPanel.Invalidate();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            dc.ChartType = DrawChart.ChartTypeEnum.Surface;
            Prepare3DAxis();
            PlotPanel.Invalidate();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            dc.ChartType = DrawChart.ChartTypeEnum.SurfaceContour;
            Prepare3DAxis();
            PlotPanel.Invalidate();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            dc.ChartType = DrawChart.ChartTypeEnum.Waterfall;
            Prepare3DAxis();
            PlotPanel.Invalidate();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PlotPanel.Dispose();
            CreateNewPlotPanel();
            PlotPanel.Size = new Size(0, 0);
            dc.ChartType = DrawChart.ChartTypeEnum.XYColor;
            PlotPanel.Invalidate();
        }
        private void Prepare3DAxis()
        {
            dc.NumberInterp = 5;
            cs.GridStyle.LineColor = Color.LightGray;
            cs.GridStyle.Pattern = DashStyle.Dash;
            cs.Title = "No Title";
            cs.IsColorBar = true;
            //dc.ChartType = DrawChart.ChartTypeEnum.Mesh;
            dc.IsColorMap = true;
            dc.IsHiddenLine = false;
            ds.LineStyle.IsVisible = true;
            dc.CMap = cm.Jet();
            cs.GridStyle.LineColor = Color.LightGray;
            cs.GridStyle.Pattern = DashStyle.Dash;
            cs.AxisStyle.LineColor = Color.Blue;
            cs.AxisStyle.Thickness = 2;
            cs.YTick = 1f;
        }
        private void CreateNewPlotPanel()
        {
            PlotPanel = new Panel();
            PlotPanel.Location = new Point(0, 0);
            PlotPanel.Name = "PlotPanel";
            Rectangle rect = ClientRectangle;
            PlotPanel.Size = new Size(19 * rect.Width / 30, 3 * rect.Height / 5);
            PlotPanel.TabIndex = 0;
            Controls.Add(PlotPanel);
            ds.LineStyle.Thickness = 1;
            PlotPanel.Paint += new PaintEventHandler(PlotPanelPaint);
        }
    }
}
