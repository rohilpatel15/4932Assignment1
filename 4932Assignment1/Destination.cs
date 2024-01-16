using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static _4932Assignment1.Form1;

namespace _4932Assignment1
{
    public partial class Destination : Form
    {
        /* The bitmap that the image is displayed on */
        private Bitmap formMapDest;
        /* A line object that represents the line the user draws with */
        private Line currentLine = null;
        /* Boolean to represent if an image is loaded or not */
        private bool isImageLoaded = false;
        /* Lists that contain lines drawn and lines duplicated */
        private List<Line> linesFromSource = new List<Line>();
        private List<Line> linesToSource = new List<Line>();
        private int reference = 0;
        public Destination()
        {
            InitializeComponent();
            /* Creates bitmap the size of the form */
            formMapDest = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        /* Allows the user to open an image */
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files | *.jpg; *.jpeg; *.png; *.bmp; *.bmp| All Files |*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                /* Creates a bitmap of the image with the size of the form */
                Bitmap originalBitmap = new Bitmap(openFileDialog.FileName);
                formMapDest = new Bitmap(originalBitmap, new Size(400, 350));
                pictureBox1.Image = formMapDest;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                isImageLoaded = true;
            }

        }

        private void Destination_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (isImageLoaded)
            {
                /* Draws duplicated lines */
                using (Pen pen = new Pen(Color.Red, 5))
                {
                    foreach (Line line in linesFromSource)
                    {
                        e.Graphics.DrawLine(pen, line.StartPoint, line.EndPoint);
                        e.Graphics.DrawCircle(pen, line.StartPoint.X, line.StartPoint.Y, 5);
                        e.Graphics.DrawCircle(pen, line.EndPoint.X, line.EndPoint.Y, 5);
                        e.Graphics.DrawCircle(pen, (line.StartPoint.X + line.EndPoint.X) / 2, (line.StartPoint.Y + line.EndPoint.Y) / 2, 5);
                    }
                }
                /* Draws new lines */
                using (Pen pen2 = new Pen(Color.CornflowerBlue, 5))
                {
                    foreach (Line line in linesToSource)
                    {
                        e.Graphics.DrawLine(pen2, line.StartPoint, line.EndPoint);
                        e.Graphics.DrawCircle(pen2, line.StartPoint.X, line.StartPoint.Y, 5);
                        e.Graphics.DrawCircle(pen2, line.EndPoint.X, line.EndPoint.Y, 5);
                        e.Graphics.DrawCircle(pen2, (line.StartPoint.X + line.EndPoint.X) / 2, (line.StartPoint.Y + line.EndPoint.Y) / 2, 5);
                    }
                }
            }
        }

        /* Creates a new line when the left click is down */
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isImageLoaded)
            {
                if (e.Button == MouseButtons.Left)
                {
                    currentLine = new Line(e.Location, e.Location, reference);
                    linesToSource.Add(currentLine);
                    reference++;
                }
            }
        }

        /* Sends lines drawn to be duplicated */
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentLine != null)
            {
                ((Form1)this.MdiParent).DuplicateLine(currentLine);
                currentLine = null;
            }
        }

        /* Draws the length of the line when the mouse moves */
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentLine != null)
            {
                currentLine.EndPoint = e.Location;
                pictureBox1.Invalidate();
            }
        }

        /* Adds duplicated lines to be redrawn */
        public void ReplicateLine(Line line)
        {
            if (isImageLoaded)
            {
                linesFromSource.Add(line);
                pictureBox1.Invalidate();
            }
        }

    }
}