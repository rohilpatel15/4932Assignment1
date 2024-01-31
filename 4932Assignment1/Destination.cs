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
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Numerics;
using System.Diagnostics;

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
        private Line selectedLine = null;
        private ResizePoint resizePoint = ResizePoint.None;
        private Point previousMousePosition;
        private int formid = 2;

        private enum ResizePoint
        {
            None,
            Start,
            End
        }
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
                        if (line.IsMovable)
                        {
                            e.Graphics.DrawLine(pen, line.StartPoint, line.EndPoint);
                            e.Graphics.DrawCircle(pen, line.StartPoint.X, line.StartPoint.Y, 5);
                            e.Graphics.DrawCircle(pen, line.EndPoint.X, line.EndPoint.Y, 5);
                            e.Graphics.DrawCircle(pen, (line.StartPoint.X + line.EndPoint.X) / 2, (line.StartPoint.Y + line.EndPoint.Y) / 2, 5);
                        }
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
                // Check for line selection for moving or resizing
                foreach (var line in linesToSource) // Use linesToSource in Destination form
                {
                    if (line.IsNearStart(e.Location))
                    {
                        selectedLine = line;
                        resizePoint = ResizePoint.Start;
                        return; // Stop checking after the first match
                    }
                    else if (line.IsNearEnd(e.Location))
                    {
                        selectedLine = line;
                        resizePoint = ResizePoint.End;
                        return; // Stop checking after the first match
                    }
                    else if (line.IsNear(e.Location))
                    {
                        selectedLine = line;
                        previousMousePosition = e.Location;
                        resizePoint = ResizePoint.None; // Indicates that the whole line is being moved
                        return; // Stop checking after the first match
                    }
                }

                foreach (var line in linesFromSource) // Use linesToSource in Destination form
                {
                    if (line.IsNearStart(e.Location))
                    {
                        selectedLine = line;
                        resizePoint = ResizePoint.Start;
                        return; // Stop checking after the first match
                    }
                    else if (line.IsNearEnd(e.Location))
                    {
                        selectedLine = line;
                        resizePoint = ResizePoint.End;
                        return; // Stop checking after the first match
                    }
                    else if (line.IsNear(e.Location))
                    {
                        selectedLine = line;
                        previousMousePosition = e.Location;
                        resizePoint = ResizePoint.None; // Indicates that the whole line is being moved
                        return; // Stop checking after the first match
                    }
                }

                if (e.Button == MouseButtons.Left)
                {
                    currentLine = new Line(e.Location, e.Location);
                    linesToSource.Add(currentLine);
                }
            }
        }

        /* Sends lines drawn to be duplicated */
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentLine != null)
            {
                ((Form1)this.MdiParent).DuplicateLine(currentLine, formid);
                currentLine = null;
            }
            else if (selectedLine != null)
            {
                selectedLine = null;
                resizePoint = ResizePoint.None;
            }
        }

        /* Draws the length of the line when the mouse moves */
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedLine != null && e.Button == MouseButtons.Left)
            {
                if (resizePoint != ResizePoint.None) // Resizing logic
                {
                    if (resizePoint == ResizePoint.Start)
                    {
                        selectedLine.ResizeStartPoint(e.Location);
                    }
                    else if (resizePoint == ResizePoint.End)
                    {
                        selectedLine.ResizeEndPoint(e.Location);
                    }
                }
                else // Moving logic
                {
                    int deltaX = e.X - previousMousePosition.X;
                    int deltaY = e.Y - previousMousePosition.Y;
                    selectedLine.Move(deltaX, deltaY);
                    previousMousePosition = e.Location;
                }

                pictureBox1.Invalidate();
                return;
            }

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

        /* Morphs based on lines drawn from source */
        public void Morph(List<Line> sourceLines)
        {
            Bitmap transition = new Bitmap(formMapDest.Width, formMapDest.Height);
            List<Vector2> sourcePoints = new List<Vector2>();
            List<Color> colors = new List<Color>();

            for (int y = 0; y < formMapDest.Height; ++y)
            {
                for (int x = 0; x < formMapDest.Width; ++x)
                {
                    double weight_sum = 0;
                    Vector2 delta_sum = new Vector2(0, 0);
                    for (int k = 0; k < linesFromSource.Count; k++)
                    {
                        Line line = linesFromSource[k];

                        Vector2 P = new Vector2(line.StartPoint.X, line.StartPoint.Y);
                        Vector2 Q = new Vector2(line.EndPoint.X, line.EndPoint.Y);
                        Vector2 PQ = new Vector2(line.EndPoint.X - line.StartPoint.X, line.EndPoint.Y - line.StartPoint.Y);
                        Vector2 n = new Vector2(-PQ.Y, PQ.X);
                        Vector2 XP = new Vector2(line.StartPoint.X - x, line.StartPoint.Y - y);
                        Vector2 PX = new Vector2(x - line.StartPoint.X, y - line.StartPoint.Y);

                        float d = Vector2.Dot(XP, n) / n.Length();

                        float f = Vector2.Dot(PX, PQ) / PQ.Length();

                        float fl = f / PQ.Length();

                        Line sourceLine = sourceLines[k];

                        Vector2 PPrime = new Vector2(sourceLine.StartPoint.X, sourceLine.StartPoint.Y);
                        Vector2 NPrime = new Vector2(-1 * (sourceLine.EndPoint.Y - sourceLine.StartPoint.Y), sourceLine.EndPoint.X - sourceLine.StartPoint.X);
                        Vector2 PQPrime = new Vector2(sourceLine.EndPoint.X - sourceLine.StartPoint.X, sourceLine.EndPoint.Y - sourceLine.StartPoint.Y);

                        Vector2 XPrime = PPrime + Vector2.Multiply(fl, PQPrime) - Vector2.Multiply(d, Vector2.Divide(NPrime, NPrime.Length()));

                        Vector2 X = new Vector2(x, y);
                        Vector2 delta1 = XPrime - X;
                        double weight = 0;
                        if (fl >= 0 && fl <= 1) weight = Math.Pow(1 / (d + 0.01), 2);
                        else if (fl < 0)
                        {
                            float dxp = Vector2.Distance(X, P);
                            weight = Math.Pow(1 / (dxp + 0.01), 2);
                        }
                        else if (fl > 1)
                        {
                            float dxq = Vector2.Distance(X, Q);
                            weight = Math.Pow(1 / (dxq + 0.01), 2);
                        }

                        weight_sum += weight;
                        delta_sum += Vector2.Multiply((float)weight, delta1);

                    }
                    Vector2 delta_avg = Vector2.Divide(delta_sum, (float)weight_sum);

                    Vector2 XPrime_avg = new Vector2(x, y) + delta_avg;
                    XPrime_avg = validatePixel(XPrime_avg, formMapDest.Width, formMapDest.Height);
                    transition.SetPixel(x, y, formMapDest.GetPixel((int)XPrime_avg.X, (int)XPrime_avg.Y));
                }
            }
            ((Form1)MdiParent).UpdateTransition(transition, transition);
        }

        /* Resets values of pixel to 0 if negative */
        private Vector2 validatePixel(Vector2 coord, int width, int height)
        {
            if (coord.X < 0)
            {
                coord.X = 0;
            }
            else if (coord.X >= width)
            {
                coord.X = width - 1;
            }
            if (coord.Y < 0)
            {
                coord.Y = 0;
            }
            else if (coord.Y >= height)
            {
                coord.Y = height - 1;
            }
            return coord;
        }

    }
}