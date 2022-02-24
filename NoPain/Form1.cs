namespace NoPain
{
    public partial class Form1 : Form
    {
        private Painter p;
        public Form1()
        {
            InitializeComponent();

            p = new Painter(pic);
            pic.Image = p.bm;

        }

        /// <summary>
        /// Выбор инструментов 1 - Brush/Pencil, 2 - Eraser, 3 - Pipette, 4 - Text
        /// 5 - Fill, 6 - Line, 7 - Ellipse, 8 - Rectangle, 9 - ArrowLine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Peek
        private void btn_pencil_Click(object sender, EventArgs e)
        {
            p.Instrument = 1;
        }

        private void btn_eraser_Click(object sender, EventArgs e)
        {
            p.Instrument = 2;
        }

        private void btn_pipette_Click(object sender, EventArgs e)
        {
            p.Instrument = 3;
        }

        private void btn_text_Click(object sender, EventArgs e)
        {
            p.Instrument = 4;
        }

        private void btn_fill_Click(object sender, EventArgs e)
        {
            p.Instrument = 5;
        }

        private void btn_line_Click(object sender, EventArgs e)
        {
            p.Instrument = 6;
        }

        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            p.Instrument = 7;
        }

        private void btn_rectangle_Click(object sender, EventArgs e)
        {
            p.Instrument = 8;
        }

        private void btn_arrow_Click(object sender, EventArgs e)
        {
            p.Instrument = 9;
        }



        #endregion

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            p.MainColor = mainClrButton.BackColor;
            p.BackGroundColor = backClrButton.BackColor;
            p.PenWidth = trackBar1.Value;
            p.PainterMouseDown(e.Location, pic.Image);
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            p.PainterMouseMove(e.Location, false);
            pic.Refresh();
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            p.PainterMouseUp();
            mainClrButton.BackColor = p.MainColor;
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            if (((Button)sender).BackColor != Color.Transparent)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        mainClrButton.BackColor = ((Button)sender).BackColor;
                        break;
                    case MouseButtons.Right:
                        backClrButton.BackColor = ((Button)sender).BackColor;
                        break;
                }
            }
        }

        private void btn_palette_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_palette_pick.BackColor = colorDialog1.Color;
            }
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            p.PainterPaint(e.Graphics);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            p.isFillChecked = true;
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            p.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            p.isFillChecked = checkBox1.Checked;
        }
    }
}