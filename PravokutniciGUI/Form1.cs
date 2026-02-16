using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PravokutniciGUI
{
    public partial class Form1 : Form
    {
        private readonly List<Rectangle> _rectangles = new List<Rectangle>();

        private bool _isDrawing = false;
        private Point _startPoint;
        private Point _currentPoint;
        private Rectangle _previewRect;

        private Label lblTotalArea;

        public Form1()
        {
            InitializeComponent();
            SetupUi();

            DoubleBuffered = true;

            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            this.Paint += Form1_Paint;
        }

        private void SetupUi()
        {
            Text = "Pravokutnici - klik/povuci/pusti";
            Width = 900;
            Height = 600;

            lblTotalArea = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                BackColor = Color.WhiteSmoke
            };

            Controls.Add(lblTotalArea);
            UpdateTotalAreaLabel();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //  DESNI KLIK → brisanje pravokutnika
            if (e.Button == MouseButtons.Right)
            {
                for (int i = _rectangles.Count - 1; i >= 0; i--)
                {
                    if (_rectangles[i].Contains(e.Location))
                    {
                        _rectangles.RemoveAt(i);
                        UpdateTotalAreaLabel();
                        Invalidate();
                        break;
                    }
                }
                return;
            }

            if (e.Button != MouseButtons.Left) return;

            _isDrawing = true;
            _startPoint = e.Location;
            _currentPoint = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing) return;

            _currentPoint = e.Location;
            _previewRect = MakeNormalizedRectangle(_startPoint, _currentPoint);
            Invalidate();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!_isDrawing) return;

            _isDrawing = false;

            Rectangle finalRect = MakeNormalizedRectangle(_startPoint, e.Location);

            if (finalRect.Width > 2 && finalRect.Height > 2)
                _rectangles.Add(finalRect);

            _previewRect = Rectangle.Empty;

            UpdateTotalAreaLabel();
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.DodgerBlue, 2))
            using (var fill = new SolidBrush(Color.FromArgb(40, Color.DodgerBlue)))
            {
                foreach (var r in _rectangles)
                {
                    e.Graphics.FillRectangle(fill, r);
                    e.Graphics.DrawRectangle(pen, r);
                }
            }

            if (_isDrawing && !_previewRect.IsEmpty)
            {
                using (var pen = new Pen(Color.OrangeRed, 2)
                { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    e.Graphics.DrawRectangle(pen, _previewRect);
                }
            }
        }

        private void UpdateTotalAreaLabel()
        {
            long totalArea = _rectangles.Sum(r => (long)r.Width * r.Height);
            lblTotalArea.Text = $"Ukupna površina: {totalArea} px²   |   Broj pravokutnika: {_rectangles.Count}";
        }

        private static Rectangle MakeNormalizedRectangle(Point a, Point b)
        {
            int x1 = Math.Min(a.X, b.X);
            int y1 = Math.Min(a.Y, b.Y);
            int x2 = Math.Max(a.X, b.X);
            int y2 = Math.Max(a.Y, b.Y);
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }
    }
}

