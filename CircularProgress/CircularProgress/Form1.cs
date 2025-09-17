using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CircularProgress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            timer1.Start();
        }


        

        void Progress(int percent)
        {
            Bitmap bp = new Bitmap(300, 300);
            int cx = 150, cy = 150;
            double maxAngle = 2 * Math.PI * percent / 100.0;

            Color[] palette = new Color[]
            {
        Color.FromArgb(244, 67, 54),   // Red
        Color.FromArgb(156, 39, 176),  // Purple
        Color.FromArgb(33, 150, 243),  // Blue
        Color.FromArgb(0, 188, 212),   // Cyan
        Color.FromArgb(76, 175, 80),   // Green
        Color.FromArgb(139, 195, 74),  // Light Green
        Color.FromArgb(244, 67, 54),   // Red
            };

            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    int dx = x - cx;
                    int dy = y - cy;
                    int r2 = dx * dx + dy * dy;
                    int rmin = 80, rmax = 290;
                    if (r2 > rmin * rmin && r2 < rmax * rmax)
                    {
                        double angle = Math.Atan2(dy, dx);
                        if (angle < 0) angle += 2 * Math.PI;

                        if (angle <= maxAngle)
                        {
                            double dist = Math.Sqrt(r2);
                            double diff = Math.Abs(dist - 120);
                            int alpha = (int)(255 - Math.Min(255, diff * 10));

                            // زاویه رو نگاشت کن به پالت
                            double t = angle / (2 * Math.PI) * (palette.Length - 1);
                            int idx = (int)Math.Floor(t);
                            double frac = t - idx;

                            Color c1 = palette[idx];
                            Color c2 = palette[Math.Min(idx + 1, palette.Length - 1)];
                            Color blended = LerpColor(c1, c2, frac);

                            Color finalColor = Color.FromArgb(alpha, blended);
                            bp.SetPixel(x, y, finalColor);
                        }
                    }
                }
            }

           // درصد را وسط تصویر بنویس
           using (Graphics g = Graphics.FromImage(bp))
           {
               string text = percent + "%";
               using (Font font = new Font("Consolas", 30, FontStyle.Bold))
               {
                   SizeF textSize = g.MeasureString(text, font);
                   PointF location = new PointF(
                       (bp.Width - textSize.Width) / 2,
                       (bp.Height - textSize.Height) / 2
                   );
           
                   g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                   g.DrawString(text, font, Brushes.White, location);
               }
           }

            pictureBox1.Image = bp;
        }

        Color LerpColor(Color c1, Color c2, double t)
        {
            int r = (int)(c1.R + (c2.R - c1.R) * t);
            int g = (int)(c1.G + (c2.G - c1.G) * t);
            int b = (int)(c1.B + (c2.B - c1.B) * t);
            return Color.FromArgb(r, g, b);
        }

        int percent = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            percent++;
            if (percent > 100) percent = 0;
            Progress(percent);
            pictureBox1.Invalidate();
            Application.DoEvents();
        }
    }
}
