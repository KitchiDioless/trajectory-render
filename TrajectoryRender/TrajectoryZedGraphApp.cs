using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace TrajectoryZedGraphApp
{
    public partial class MainForm : Form
    {
        private TextBox txtVelocity;
        private TextBox txtAngle;
        private TextBox txtGravity;
        private TextBox txtInitialHight;
        
        private ZedGraphControl zedGraphControl1;
        private ZedGraphControl zedGraphControl2;
        private ZedGraphControl zedGraphControl3;
        private ZedGraphControl zedGraphControl4;

        private double v0 = 0.0;
        private double angleDegrees = 0.0;
        private double g = 9.81;
        private double initHight = 0.0;

        private PointPairList listOfTrajectoryPoints = new PointPairList();
        private PointPairList listOfXTimePoints = new PointPairList();
        private PointPairList listOfYTimePoints = new PointPairList();
        private PointPairList listOfVelocityPoints = new PointPairList();

        private int windowWight = 960;
        private int windowHight = 540;


        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm());
        }

        public MainForm()
        {
            InitializeComponent();
            SetupGraphs();
            SetupInputControls();
            ApplyCustomStyles();

            this.Resize += MainFormResize;
        }

        private void InitializeComponent()
        {
            this.zedGraphControl1 = new ZedGraphControl();
            this.zedGraphControl2 = new ZedGraphControl();
            this.zedGraphControl3 = new ZedGraphControl();
            this.zedGraphControl4 = new ZedGraphControl();

            this.txtVelocity = new TextBox 
            { 
                Location = new Point(windowWight - 112, 20), Width = 100, PlaceholderText = "v0"
            };

            this.txtAngle = new TextBox 
            { 
                Location = new Point(windowWight - 112, 60), Width = 100, PlaceholderText = "Angle of launch" 
            };

            this.txtGravity = new TextBox 
            { 
                Location = new Point(windowWight - 112, 100), Width = 100, PlaceholderText = "g" 
            };

            this.txtInitialHight = new TextBox 
            {
                Location = new Point(windowWight - 112, 140), Width = 100, PlaceholderText = "Initial hight" 
            };

            this.SuspendLayout();

            this.zedGraphControl1.Location = new Point(12, 12);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.Size = new Size((windowWight - 112) - 24, windowHight - 250);
            this.zedGraphControl1.TabIndex = 0;

            this.zedGraphControl2.Location = new Point(12, windowHight - 224);
            this.zedGraphControl2.Name = "zedGraphControl2";
            this.zedGraphControl2.Size = new Size(300, 200);
            this.zedGraphControl2.TabIndex = 1;

            this.zedGraphControl3.Location = new Point(324, windowHight - 224);
            this.zedGraphControl3.Name = "zedGraphControl3";
            this.zedGraphControl3.Size = new Size(300, 200);
            this.zedGraphControl3.TabIndex = 2;

            this.zedGraphControl4.Location = new Point(640, windowHight - 224);
            this.zedGraphControl4.Name = "zedGraphControl4";
            this.zedGraphControl4.Size = new Size(300, 200);
            this.zedGraphControl4.TabIndex = 3;

            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(windowWight, windowHight);

            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.zedGraphControl2);
            this.Controls.Add(this.zedGraphControl3);
            this.Controls.Add(this.zedGraphControl4);

            this.Controls.Add(this.txtVelocity);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.txtGravity);
            this.Controls.Add(this.txtInitialHight);

            this.Name = "MainForm";
            this.Text = "Trajectory Visualization";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            zedGraphControl1.Size = new Size(formWidth - 300, (formHeight - 250) / 2);
            zedGraphControl2.Size = new Size((formWidth - 30) / 3, (formHeight - 250) / 2);
            zedGraphControl3.Size = new Size((formWidth - 30) / 3, (formHeight - 250) / 2);
            zedGraphControl4.Size = new Size((formWidth - 30) / 3, (formHeight - 250) / 2);

            zedGraphControl1.Location = new Point(12, 12);
            zedGraphControl2.Location = new Point(12, 250);
            zedGraphControl3.Location = new Point(zedGraphControl2.Right + 10, 250);
            zedGraphControl4.Location = new Point(zedGraphControl3.Right + 10, 250);

            txtVelocity.Location = new Point(formWidth - 150, 20);
            txtAngle.Location = new Point(formWidth - 150, 60);
            txtGravity.Location = new Point(formWidth - 150, 100);
            txtInitialHight.Location = new Point(formWidth - 150, 140);
        }

        private void SetupInputControls()
        {
            txtVelocity.TextChanged += (sender, e) => UpdateGraphs();
            txtAngle.TextChanged += (sender, e) => UpdateGraphs();
            txtGravity.TextChanged += (sender, e) => UpdateGraphs();
            txtInitialHight.TextChanged += (sender, e) => UpdateGraphs();
        }

        private void ApplyCustomStyles()
        {
            this.BackColor = Color.FromArgb(35, 35, 45);

            SetTextBoxStyles(txtVelocity);
            SetTextBoxStyles(txtAngle);
            SetTextBoxStyles(txtGravity);
            SetTextBoxStyles(txtInitialHight);

            SetGraphControlStyles(zedGraphControl1);
            SetGraphControlStyles(zedGraphControl2);
            SetGraphControlStyles(zedGraphControl3);
            SetGraphControlStyles(zedGraphControl4);
        }

        private void SetTextBoxStyles(TextBox textBox)
        {
            textBox.BackColor = Color.FromArgb(30, 30, 40);
            textBox.ForeColor = Color.White;
            textBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void SetGraphControlStyles(ZedGraphControl zedGraphControl)
        {
            zedGraphControl.BackColor = Color.FromArgb(20, 20, 30);
        }

        private void SetupGraphs()
        {
            SetupGraph(zedGraphControl1, "Trajectory", "Time(s)", "Height(m)");
            SetupGraph(zedGraphControl2, "X vs Time", "Time (s)", "X (m)");
            SetupGraph(zedGraphControl3, "Y vs Time", "Time (s)", "Y (m)");
            SetupGraph(zedGraphControl4, "Velocity vs Time", "Time (s)", "Velocity (m/s)");
            
            UpdateGraphs();
        }

        private void SetupGraph(ZedGraphControl zedGraphControl, string title,
            string xAxisTitle, string yAxisTitle)
        {
            GraphPane pane = zedGraphControl.GraphPane;

            pane.Title.Text = title;
            pane.XAxis.Title.Text = xAxisTitle;
            pane.YAxis.Title.Text = yAxisTitle;

            pane.Title.FontSpec.FontColor = Color.White;
            pane.XAxis.Title.FontSpec.FontColor = Color.White;
            pane.YAxis.Title.FontSpec.FontColor = Color.White;

            pane.Fill = new Fill(Color.FromArgb(30, 30, 40));
            pane.Chart.Fill = new Fill(Color.FromArgb(30, 30, 40));

            pane.XAxis.Color = Color.White;
            pane.YAxis.Color = Color.White;

            pane.XAxis.Scale.FontSpec.FontColor = Color.White;
            pane.YAxis.Scale.FontSpec.FontColor = Color.White;

            pane.XAxis.MajorGrid.IsVisible = true;
            pane.XAxis.MajorGrid.Color = Color.White;
            pane.YAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.Color = Color.White;

            pane.Chart.Border.Color = Color.White;
            pane.Chart.Border.Width = 2;
            pane.Chart.Border.IsAntiAlias = true;
        }

        private void UpdateGraphs()
        {
            if (!double.TryParse(txtVelocity.Text, out v0)) v0 = 20.0;
            if (!double.TryParse(txtAngle.Text, out angleDegrees)) angleDegrees = 45.0;
            if (!double.TryParse(txtGravity.Text, out g)) g = 9.81;
            if (!double.TryParse(txtInitialHight.Text, out initHight)) initHight = 0.0;

            listOfTrajectoryPoints.Clear();
            listOfXTimePoints.Clear();
            listOfYTimePoints.Clear();
            listOfVelocityPoints.Clear();

            UpdateGraph(zedGraphControl1, CalculateVariableTrajectory());
            UpdateGraph(zedGraphControl2, listOfXTimePoints);
            UpdateGraph(zedGraphControl3, listOfYTimePoints);
            UpdateGraph(zedGraphControl4, listOfVelocityPoints);
        }

        private void UpdateGraph(ZedGraphControl zedGraphControl, PointPairList points)
        {
            GraphPane pane = zedGraphControl.GraphPane;
            pane.CurveList.Clear();

            LineItem curve = pane.AddCurve("Trajectory", points,
                Color.FromArgb(199, 54, 89), SymbolType.None);
            curve.Line.Width = 2.5F;
            curve.Line.IsAntiAlias = true;
            curve.Line.IsSmooth = true;

            zedGraphControl.AxisChange();
            zedGraphControl.Invalidate();
        }

        private PointPairList CalculateVariableTrajectory(){
            double h = initHight;
            double angleRadians = angleDegrees * Math.PI / 180.0;
            double v0x = v0 * Math.Cos(angleRadians);
            double v0y = v0 * Math.Sin(angleRadians);
            double tMax = (v0y + Math.Sqrt(v0y * v0y + 2 * g * h)) / g;

            double dt = 0.1;
            for (double t = 0; t <= tMax; t += dt)
            {
                double x = v0x * t;
                double y = h + x * Math.Tan(angleRadians) - (g * x * x) 
                    / (2 * v0 * v0 * Math.Cos(angleRadians) * Math.Cos(angleRadians));

                if (y >= 0)
                {
                    listOfTrajectoryPoints.Add(x, y);

                    listOfXTimePoints.Add(x, t);

                    listOfYTimePoints.Add(t, y);

                    double vy = v0y - g * t;
                    double velocity = Math.Sqrt(v0x * v0x + vy * vy);
                    listOfVelocityPoints.Add(t, velocity);
                }
            }

            return listOfTrajectoryPoints;
        }
    }
}
