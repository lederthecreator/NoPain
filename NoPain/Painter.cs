using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
        public Rectangle rec, rec_shift;
        private double alpha; // sheeeeeeeeeeeeeeeeesh - Sin угла наклона линии
        private string text, filename;

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

        public void PainterMouseUp(bool isShift)
        {
            isPaint = false;

            switch (Instrument)
            {
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    if (isShift)
                    {
                        Point perp = new Point(StartPoint.X, EndPoint.Y);
                        alpha = Math.Sin(Math.Abs(Distance(StartPoint, perp) / Distance(StartPoint, EndPoint)));
                        ShiftLineCorrect();
                        g.DrawLine(Pen, StartPoint, EndPoint);
                    }
                    else
                    {
                        g.DrawLine(Pen, StartPoint, EndPoint);
                    }
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
                    if (isShift)
                    {

                        rec.Height = (int)(Distance(StartPoint, EndPoint) * Math.Sin(Math.PI / 4));
                        rec.Width = (int)(Distance(StartPoint, EndPoint) * Math.Sin(Math.PI / 4));
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
                    if (isShift)
                    {
                        rec.Height = (int)(Distance(StartPoint, EndPoint) * Math.Sin(Math.PI / 4));
                        rec.Width = (int)(Distance(StartPoint, EndPoint) * Math.Sin(Math.PI / 4));
                    }
                    g.DrawRectangle(Pen, rec);
                    break;
                case 9:
                    Pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    if (isShift)
                    {
                        Point perp = new Point(StartPoint.X, EndPoint.Y);
                        alpha = Math.Sin(Math.Abs(Distance(StartPoint, perp) / Distance(StartPoint, EndPoint)));
                        ShiftLineCorrect();
                        g.DrawLine(Pen, StartPoint, EndPoint);
                    }
                    else
                    {
                        g.DrawLine(Pen, StartPoint, EndPoint);
                    }
                    break;
                case 10:
                    Point perp1 = new Point(StartPoint.X, EndPoint.Y);
                    alpha = Math.Sin(Math.Abs(Distance(StartPoint, perp1) / Distance(StartPoint, EndPoint)));
                    CreateHexagonLine();
                    Hegaxon hx = new Hegaxon(StartPoint, EndPoint);
                    if (isFillChecked)
                    {
                        var br = new SolidBrush(BackGroundColor);
                        g.FillPolygon(br, hx.Points);
                    }
                    g.DrawPolygon(Pen, hx.Points);
                    break;

            }
            
        }

        public void PainterPaint(Graphics pg, bool isShift)
        {
            if (isPaint)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                switch (Instrument)
                {
                    case 4:
                        rec = new Rectangle
                            (CCS.X > 0 ? StartPoint.X : StartPoint.X + CCS.X,
                            CCS.Y > 0 ? StartPoint.Y - CCS.Y : StartPoint.Y,
                            Math.Abs(CCS.X), Math.Abs(CCS.Y));
                        rec.Height = 22;
                        Pen.Width = 2;
                        pg.DrawRectangle(Pen, rec);
                        break;
                    case 5:
                        break;
                    case 6:
                        if (isShift)
                        {
                            Point perp = new Point(StartPoint.X, EndPoint.Y);
                            alpha = Math.Sin(Math.Abs(Distance(StartPoint, perp) / Distance(StartPoint, EndPoint)));
                            ShiftLineCorrect();
                            pg.DrawLine(Pen, StartPoint, EndPoint);
                        }
                        else
                        {
                            pg.DrawLine(Pen, StartPoint, EndPoint);
                        }
                        break;
                    case 7:
                        rec = new Rectangle
                                (CCS.X > 0 ? StartPoint.X : StartPoint.X + CCS.X,
                                CCS.Y > 0 ? StartPoint.Y - CCS.Y : StartPoint.Y,
                                Math.Abs(CCS.X), Math.Abs(CCS.Y));
                        if (isShift)
                        {
                            rec.Height = (int)(Distance(StartPoint, EndPoint) * Math.Sin(Math.PI / 4));
                            rec.Width = (int)(Distance(StartPoint, EndPoint) * Math.Sin(Math.PI / 4));
                        }
                        pg.DrawEllipse(Pen, rec);
                        break;
                    case 8:
                        rec = new Rectangle
                                (CCS.X > 0 ? StartPoint.X : StartPoint.X + CCS.X,
                                CCS.Y > 0 ? StartPoint.Y - CCS.Y : StartPoint.Y,
                                Math.Abs(CCS.X), Math.Abs(CCS.Y));

                        if (isShift)
                        {
                            rec.Height = (int)(Distance(StartPoint, EndPoint) * Math.Sin(Math.PI / 4));
                            rec.Width = (int)(Distance(StartPoint, EndPoint) * Math.Sin(Math.PI / 4));
                        }
                        pg.DrawRectangle(Pen, rec);
                        break;
                    case 9:
                        Pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        if (isShift)
                        {
                            Point perp = new Point(StartPoint.X, EndPoint.Y);
                            alpha = Math.Sin(Math.Abs(Distance(StartPoint, perp) / Distance(StartPoint, EndPoint)));
                            ShiftLineCorrect();
                            pg.DrawLine(Pen, StartPoint, EndPoint);
                        }
                        else
                        {
                            pg.DrawLine(Pen, StartPoint, EndPoint);
                        }
                        break;
                    case 10:
                        Point perp1 = new Point(StartPoint.X, EndPoint.Y);
                        alpha = Math.Sin(Math.Abs(Distance(StartPoint, perp1) / Distance(StartPoint, EndPoint)));
                        CreateHexagonLine();
                        Hegaxon hx = new Hegaxon(StartPoint, EndPoint);
                        pg.DrawPolygon(Pen, hx.Points);
                        break;
                }
            }
        }

        private static void Validate_Pix(Point e, Bitmap bmp, Stack<Point> stack, Color new_color, Color old_color)
        {
            Color cur_color = bmp.GetPixel(e.X, e.Y);
            if(cur_color == old_color)
            {
                stack.Push(e);
                bmp.SetPixel(e.X, e.Y, new_color);
            }
        }
        public void Fill(Point e, Bitmap bm)
        {

            Color old_color = bm.GetPixel(e.X, e.Y);
            Stack<Point> stack = new Stack<Point>();

            stack.Push(e);
            bm.SetPixel(e.X, e.Y, MainColor);

            if (old_color == MainColor) return;

            while(stack.Count > 0)
            {
                var cur_pt = stack.Pop();
                if(cur_pt.X > 0 && cur_pt.Y > 0 && cur_pt.X < bm.Width - 1 && cur_pt.Y < bm.Height - 1)
                {
                    Validate_Pix(new Point(cur_pt.X, cur_pt.Y + 1), bm, stack, MainColor, old_color); // выше
                    //Validate_Pix(new Point(cur_pt.X + 1, cur_pt.Y + 1), bm, stack, MainColor, old_color); // выше правее
                    Validate_Pix(new Point(cur_pt.X + 1, cur_pt.Y), bm, stack, MainColor, old_color); // правее
                    //Validate_Pix(new Point(cur_pt.X + 1, cur_pt.Y - 1), bm, stack, MainColor, old_color); // правее ниже
                    Validate_Pix(new Point(cur_pt.X, cur_pt.Y - 1), bm, stack, MainColor, old_color); // ниже
                    //Validate_Pix(new Point(cur_pt.X - 1, cur_pt.Y - 1), bm, stack, MainColor, old_color); // левее ниже
                    Validate_Pix(new Point(cur_pt.X - 1, cur_pt.Y), bm, stack, MainColor, old_color); // левее
                    //Validate_Pix(new Point(cur_pt.X - 1, cur_pt.Y + 1), bm, stack, MainColor, old_color); // выше левее
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

        public static double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        private void ShiftLineCorrect()
        {
            //См. рисунок Paint2.sai2
            int AC = (int)Distance(EndPoint, StartPoint);
            double Sin45 = Math.Sin(Math.PI / 4);
            // I quarter
            if(CCS.X > 0 && CCS.Y > 0)
            {
                if(alpha >= 0 && alpha <= 0.5)
                {
                    EndPoint.Y = StartPoint.Y;
                }
                if(alpha > 0.5 && alpha <= 0.8)
                {
                    EndPoint.X = StartPoint.X + (int)(AC * Sin45);
                    EndPoint.Y = StartPoint.Y - (int)(AC * Sin45);
                }
                if(alpha > 0.8 && alpha <= 1)
                {
                    EndPoint.X = StartPoint.X;
                }
            }
            // II quarter
            if(CCS.X < 0 && CCS.Y > 0)
            {
                if (alpha >= 0 && alpha <= 0.5)
                {
                    EndPoint.Y = StartPoint.Y;
                }
                if (alpha > 0.5 && alpha <= 0.8)
                {
                    EndPoint.X = StartPoint.X - (int)(AC * Sin45);
                    EndPoint.Y = StartPoint.Y - (int)(AC * Sin45);
                }
                if (alpha > 0.8 && alpha <= 1)
                {
                    EndPoint.X = StartPoint.X;
                }
            }
            // III quarter
            if(CCS.X > 0 && CCS.Y < 0)
            {
                if (alpha >= 0 && alpha <= 0.5)
                {
                    EndPoint.Y = StartPoint.Y;
                }
                if (alpha >= 0.5 && alpha <= 0.8)
                {
                    EndPoint.X = StartPoint.X + (int)(AC * Sin45);
                    EndPoint.Y = StartPoint.Y + (int)(AC * Sin45);
                }
                if (alpha > 0.8 && alpha <= 1)
                {
                    EndPoint.X = StartPoint.X;
                }
            }
            // IV quarter
            if(CCS.X < 0 && CCS.Y < 0)
            {
                if (alpha >= 0 && alpha <= 0.5)
                {
                    EndPoint.Y = StartPoint.Y;
                }
                if (alpha >= 0.5 && alpha <= 0.8)
                {
                    EndPoint.X = StartPoint.X - (int)(AC * Sin45);
                    EndPoint.Y = StartPoint.Y + (int)(AC * Sin45);
                }
                if (alpha > 0.8 && alpha <= 1)
                {
                    EndPoint.X = StartPoint.X;
                }
            }
        }

        /// <summary>
        /// То же самое что и метод для Shift, только по направлениям осей ( - => | => - => | => -)
        /// </summary>
        private void CreateHexagonLine()
        {
            int AC = (int)Distance(EndPoint, StartPoint);
            double Sin45 = Math.Sin(Math.PI / 4);
            // I quarter
            if(CCS.X > 0 && CCS.Y > 0)
            {
                if (alpha >= 0 && alpha <= 0.7)
                {
                    EndPoint.Y = StartPoint.Y;
                }
                if (alpha > 0.7 && alpha <= 1)
                {
                    EndPoint.X = StartPoint.X;
                }
            }
            // II quarter
            if(CCS.X < 0 && CCS.Y > 0)
            {
                if (alpha >= 0 && alpha <= 0.7)
                {
                    EndPoint.Y = StartPoint.Y;
                }
                if (alpha > 0.7 && alpha <= 1)
                {
                    EndPoint.X = StartPoint.X;
                }
            }
            // III quarter
            if(CCS.X < 0 && CCS.Y < 0)
            {
                if (alpha >= 0 && alpha <= 0.7)
                {
                    EndPoint.Y = StartPoint.Y;
                }
                if (alpha > 0.7 && alpha <= 1)
                {
                    EndPoint.X = StartPoint.X;
                }
            }
            // IV quarter
            if(CCS.X > 0 && CCS.Y < 0)
            {
                if (alpha >= 0 && alpha <= 0.7)
                {
                    EndPoint.Y = StartPoint.Y;
                }
                if (alpha > 0.7 && alpha <= 1)
                {
                    EndPoint.X = StartPoint.X;
                }
            }
        }

        public void TextEventHandler(Form1 form, char e, TextBox textbox)
        {
            if(e == (char)13)
            {
                text = textbox.Text;
                var br = new SolidBrush(MainColor);
                g.DrawString(text, new Font("Robotica", textbox.Font.Size + 2,  FontStyle.Regular), br, rec.Location);
                form.Controls.Remove(textbox);
            }
        }

        public void SaveAs(SaveFileDialog sfd, Image img)
        {
            sfd.Filter = "Image (*.jpg) | *.jpg  | Image (*.png) | *.png";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName;
                Bitmap btm = bm.Clone(new Rectangle(0, 0, (int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height), bm.PixelFormat);
                btm.Save(filename);
            }

        }

        public void Save(PictureBox pic)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, pic.Width, pic.Height), bm.PixelFormat);
                btm.Save(filename);
            }
        }

        public void Open(PictureBox pic, OpenFileDialog ofd)
        {
            ofd.Filter = "Image (*.jpg)|*.jpg|Image (*.png)|*.png";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                //pic.Image = Image.FromFile(ofd.FileName);
                var img_loaded = Image.FromFile(ofd.FileName);
                g.DrawImage(img_loaded, 0, 0);
            }
            RefreshPainter(pic);
        }

        public void RefreshPainter(PictureBox pic)
        {
            var bmtmp = new Bitmap(pic.Width, pic.Height);
            
            var ig = Graphics.FromImage(bmtmp);
            ig.Clear(Color.White);
            ig.DrawImage(bm, new Point(0, 0));
            bm = (Bitmap)bmtmp.Clone();
            g = Graphics.FromImage(bm);
            bmtmp.Dispose();
        }

    }
}
