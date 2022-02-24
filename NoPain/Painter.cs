using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoPain
{
    internal class Painter
    {
        private Color maincolor = Color.Black, backgroundcolor = Color.White;
        private int penwidth = 3;
        public Color MainColor
        {
            get
            {
                return maincolor;
            }
            set
            {
                if (value.Name == "0") maincolor = BackGroundColor;
                else maincolor = value;
            }
        }
        public Color BackGroundColor
        {
            get { return backgroundcolor; }
            set
            {
                backgroundcolor = value;
            }
        }

        public int PenWidth
        {
            get { return penwidth; }
            set
            {
                penwidth = value switch
                {
                    <= 0 => 3,
                    > 0 => value
                };
            }
        }

        public Point StartPoint, EndPoint, CCS;
        public bool isPaint = false, isFillChecked = false;
        public Pen? Pen;
        public int Instrument;
        private Rectangle rec;

        public Bitmap? bm;
        public Graphics? g;

        public Painter(PictureBox pic)
        {
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            g.Clear(BackGroundColor);

            Pen = new Pen(MainColor, PenWidth);
        }

        public void PainterMouseDown(Point e, Image img)
        {
            Pen.Color = MainColor;
            Pen.Width = PenWidth;
            isPaint = true;
            StartPoint = e;

            if(Instrument == 3)
            {
                MainColor = PickColor(e, img);
            }
        }

        public void PainterMouseMove(Point e, bool isShift)
        {
            if (isPaint)
            {
                EndPoint = e;
                /// Делаем декартову систему координат, далее см. рисунок Paint2.sai2
                CCS = new Point(EndPoint.X - StartPoint.X, StartPoint.Y - EndPoint.Y);
                Pen.EndCap = Pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                switch (Instrument)
                {
                    case 1:
                        g.DrawLine(Pen, StartPoint, EndPoint);
                        StartPoint = EndPoint;
                        break;
                    case 2:
                        Pen.Color = BackGroundColor;
                        g.DrawLine(Pen, StartPoint, EndPoint);
                        StartPoint = EndPoint;
                        break;
                    case 5:
                        break;
                    case 7:
                        break;

                }
            }
        }

        public void PainterMouseUp()
        {
            isPaint = false;

            switch (Instrument)
            {
                case 5:
                    break;
                case 6:
                    g.DrawLine(Pen, StartPoint, EndPoint);
                    break;
                case 7:
                    rec = new Rectangle
                            (CCS.X > 0 ? StartPoint.X : StartPoint.X + CCS.X,
                            CCS.Y > 0 ? StartPoint.Y - CCS.Y : StartPoint.Y,
                            Math.Abs(CCS.X), Math.Abs(CCS.Y));
                    if (isFillChecked)
                    {
                        var br = new SolidBrush(BackGroundColor);
                        g.FillEllipse(br, rec);
                    }
                    g.DrawEllipse(Pen, rec);
                    break;
                case 8:
                    rec = new Rectangle
                            (CCS.X > 0 ? StartPoint.X : StartPoint.X + CCS.X,
                            CCS.Y > 0 ? StartPoint.Y - CCS.Y : StartPoint.Y,
                            Math.Abs(CCS.X), Math.Abs(CCS.Y));
                    if (isFillChecked)
                    {
                        var br = new SolidBrush(BackGroundColor);
                        g.FillRectangle(br, rec);
                    }
                    g.DrawRectangle(Pen, rec);
                    break;
                case 9:
                    Pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    g.DrawLine(Pen, StartPoint, EndPoint);
                    break;

            }
            
        }

        public void PainterPaint(Graphics pg)
        {
            if (isPaint)
            {
                switch (Instrument)
                {
                    case 5:
                        break;
                    case 6:
                        pg.DrawLine(Pen, StartPoint, EndPoint);
                        break;
                    case 7:
                        rec = new Rectangle
                                (CCS.X > 0 ? StartPoint.X : StartPoint.X + CCS.X,
                                CCS.Y > 0 ? StartPoint.Y - CCS.Y : StartPoint.Y,
                                Math.Abs(CCS.X), Math.Abs(CCS.Y));
                        pg.DrawEllipse(Pen, rec);
                        break;
                    case 8:
                        rec = new Rectangle
                                (CCS.X > 0 ? StartPoint.X : StartPoint.X + CCS.X,
                                CCS.Y > 0 ? StartPoint.Y - CCS.Y : StartPoint.Y,
                                Math.Abs(CCS.X), Math.Abs(CCS.Y));
                        pg.DrawRectangle(Pen, rec);
                        break;
                    case 9:
                        Pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        pg.DrawLine(Pen, StartPoint, EndPoint);
                        break;

                }
            }
        }

        public Color PickColor(Point e, Image img)
        {
            Color clr;
            using (var bmp = new Bitmap(img))
            {
                clr = bmp.GetPixel(e.X, e.Y);
            }
            return clr;
        }

        public void Clear()
        {
            g.Clear(Color.White);
            Instrument = 0;
        }

    }
}
