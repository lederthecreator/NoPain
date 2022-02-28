namespace NoPain
{
    public partial class Form1 : Form
    {
        private Painter p;
        private TextBox textBox;
        private string filename = "";
        public Form1()
        {
            InitializeComponent();

            p = new Painter(pic);
            pic.Image = p.bm;

        }

        /// <summary>
        /// Выбор инструментов 1 - Brush/Pencil, 2 - Eraser, 3 - Pipette, 4 - Text
        /// 5 - Fill, 6 - Line, 7 - Ellipse, 8 - Rectangle, 9 - ArrowLine, 10 - Hexagon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Selection
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

        private void btn_hexagon_Click(object sender, EventArgs e)
        {
            p.Instrument = 10;
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
            p.PainterMouseUp(ModifierKeys == Keys.Shift);
            mainClrButton.BackColor = p.MainColor;

            if (p.Instrument == 4)
            {
                textBox = new TextBox();
                textBox.Location = new Point(p.rec.Location.X + pic.Location.X, p.rec.Location.Y + pic.Location.Y);
                textBox.Name = "textBox";
                textBox.Size = new Size(p.CCS.X, 22);
                textBox.KeyPress += TextBox_KeyPress;
                Controls.Add(textBox);
                textBox.BringToFront();
            }
        }

        private void TextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            p.TextEventHandler(this, e.KeyChar, textBox);
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
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_palette_pick.BackColor = colorDialog1.Color;
            }
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            p.PainterPaint(e.Graphics, ModifierKeys == Keys.Shift);
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

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (p.Instrument == 5)
            {
                p.Fill(e.Location, p.bm);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (p.Instrument == 4) return;
            switch (e.KeyCode)
            {
                case Keys.X:
                    var tmp = mainClrButton.BackColor;
                    mainClrButton.BackColor = backClrButton.BackColor;
                    backClrButton.BackColor = tmp;
                    break;
                case Keys.D:
                    mainClrButton.BackColor = Color.Black;
                    backClrButton.BackColor = Color.White;
                    break;
            }
        }

        private void saveAsToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            p.SaveAs(saveFileDialog1, pic.Image);
        }

        private void saveToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            p.Save(pic);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            p.Open(pic, openFileDialog1);
            p.g = Graphics.FromImage(pic.Image);
            pic.Refresh();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            p.RefreshPainter(pic);
            pic.Image = p.bm;
            pic.Refresh();
        }


    }
}

//TODO: CTRL+Z 
//TODO: Разбивка по классам, отдельный класс для сохранения
//TODO: Не передавать PictureBox в функции Painter'а