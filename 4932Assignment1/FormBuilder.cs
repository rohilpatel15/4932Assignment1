using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4932Assignment1
{
    public partial class FormBuilder : Form
    {
        private List<Line> lines;
        protected Line currentLine;
        protected Line selectedLine;
        protected Bitmap backgroundImage;
        protected int type;
        protected bool deleting;

        public FormBuilder(int type, int index)
        {
            InitializeComponent();
            UpdateStyles();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            lines = new List<Line>();
            currentLine = null;
            selectedLine = null;
            backgroundImage = null;
            deleting = false;
            this.type = type;
            Text = TypeToString();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            int startX = (index % 4) * (Width + 10);
            int startY = (index / 4) * (Height + 10);
            Location = new Point(startX, startY);
        }

        // Sets the image of the form
        public void SetImage(Bitmap image)
        {
            backgroundImage = image;
            Refresh();
        }

        // Distinguishes which form is being used
        private string TypeToString()
        {
            if (type != FormType.FINAL)
            {
                return type == FormType.SOURCE ? FormType.SOURCE_STR : FormType.DESTINATION_STR;
            }
            else return FormType.FINAL_STR;
        }

        // Paints onto form
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (backgroundImage != null)
            {
                e.Graphics.DrawImage(backgroundImage, 0, 0, ClientSize.Width, ClientSize.Height);
            }

            foreach (Line line in lines)
            {
                line.Draw(e);
            }

            if (currentLine != null)
            {
                currentLine.Draw(e);
            }
        }

        private void FormBuilder_MouseDown(object sender, MouseEventArgs e)
        {
            if (type != FormType.FINAL)
            {
                bool hoveringOverLine = false;

                foreach (Line line in lines)
                {
                    if (line.LineAction(e) != Action.CreateLine)
                    {
                        hoveringOverLine = true;
                        selectedLine = line;
                        if (e.Button == MouseButtons.Right)
                        {
                            deleting = true;
                        }
                    }
                }

                if (!hoveringOverLine)
                {
                    if (backgroundImage != null && e.Button == MouseButtons.Left)
                    {
                        currentLine = new Line(e.Location.X, e.Location.Y);
                    }
                }
            }
        }

        private void FormBuilder_MouseMove(object sender, MouseEventArgs e)
        {
            // creating new line
            if (currentLine != null)
            {
                currentLine.UpdateEndPoints(e.Location.X, e.Location.Y);
                Refresh();
            }

            // resizing existing line
            else if (selectedLine != null)
            {
                selectedLine.Resize(e);
                Refresh();
            }
        }

        private void FormBuilder_MouseUp(object sender, MouseEventArgs e)
        {
            if (backgroundImage != null && e.Button == MouseButtons.Left && currentLine != null && !deleting)
            {
                lines.Add(currentLine);
                ((Form1)MdiParent).Reflect(currentLine, this.type, Action.CreateLine);
                currentLine = null;
                selectedLine = null;
                Refresh();
            }
            else if (selectedLine != null && !deleting)
            {
                selectedLine = null;
                Refresh();
            }
        }

        // Gets an individual line based off an id
        public Line getLine(int lineId)
        {
            return lines.Find(line => line.getId() == lineId);
        }

        // Add lines to the line list
        public void AddLines(Line line)
        {
            lines.Add(line);
        }

        // Resizes the bitmap assigned to the form image
        private Bitmap ResizeBitmap(Bitmap originalBitmap, int width, int height)
        {
            Bitmap resizedBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.DrawImage(originalBitmap, new Rectangle(0, 0, width, height));
            }
            return resizedBitmap;
        }

        // Generates line morphing information
        public List<Bitmap> Morph(List<Bitmap> frames, List<Line> sourceLines, int num_frames, int num_threads)
        {
            if (((Form1)MdiParent).GetFrames().Count != 0)
            {
                ((Form1)MdiParent).GetFrames().Clear();
            }

            Bitmap transition = new Bitmap(backgroundImage.Width, backgroundImage.Height);
            List<Vector2> dest_points = new List<Vector2>();
            List<Color> dest_colors = new List<Color>();
            List<Vector2> source_points = new List<Vector2>();
            List<Color> source_colors = new List<Color>();

            int threadWidth = backgroundImage.Width / num_threads + 1;
            int width = backgroundImage.Width;
            int height = backgroundImage.Height;

            // Create tasks for threads
            List<Task> tasks = new List<Task>();

            for (int t = 0; t < num_threads; t++)
            {
                int startX = t * threadWidth;
                int endX = Math.Min((t + 1) * threadWidth, width);

                Task task = Task.Run(() =>
                {
                    MorphThread(startX, 0, endX, height, sourceLines, dest_points, dest_colors, source_points, source_colors, transition);
                });

                tasks.Add(task);
            }

            // Wait for all tasks to complete
            Task.WaitAll(tasks.ToArray());

            frames = ((Form1)MdiParent).GenerateIntermediateFrames(frames, dest_points, source_points, transition, backgroundImage, dest_colors, source_colors);
            return frames;
        }

        private readonly object locker = new Object();

        // Line morphing logic
        private void MorphThread(int startX, int startY, int endX, int endY, List<Line> sourceLines, List<Vector2> dest_points, List<Color> dest_colors, List<Vector2> source_points, List<Color> source_colors, Bitmap transition)
        {
            for (int y = startY; y < endY; ++y)
            {
                for (int x = startX; x < endX; x++)
                {
                    double weight_sum = 0;
                    Vector2 delta_sum = new Vector2(0, 0);
                    for (int k = 0; k < lines.Count; k++)
                    {
                        Line line = lines[k];

                        Vector2 P = new Vector2(line.StartX, line.StartY);
                        Vector2 Q = new Vector2(line.EndX, line.EndY);
                        Vector2 PQ = new Vector2(line.EndX - line.StartX, line.EndY - line.StartY);
                        Vector2 n = new Vector2(-PQ.Y, PQ.X);
                        Vector2 XP = new Vector2(line.StartX - x, line.StartY - y);
                        Vector2 PX = new Vector2(x - line.StartX, y - line.StartY);

                        float d = Vector2.Dot(XP, n) / n.Length();
                        float f = Vector2.Dot(PX, PQ) / PQ.Length();
                        float fl = f / PQ.Length();

                        Line sourceLine = sourceLines[k];

                        Vector2 PPrime = new Vector2(sourceLine.StartX, sourceLine.StartY);
                        Vector2 NPrime = new Vector2(-1 * (sourceLine.EndY - sourceLine.StartY), sourceLine.EndX - sourceLine.StartX);
                        Vector2 PQPrime = new Vector2(sourceLine.EndX - sourceLine.StartX, sourceLine.EndY - sourceLine.StartY);
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
                    lock (locker)
                    {
                        Vector2 delta_avg = Vector2.Divide(delta_sum, (float)weight_sum);
                        Vector2 XPrime_avg = new Vector2(x, y) + delta_avg;

                        XPrime_avg = validatePixel(XPrime_avg, backgroundImage.Width, backgroundImage.Height);
                        transition.SetPixel(x, y, backgroundImage.GetPixel((int)XPrime_avg.X, (int)XPrime_avg.Y));

                        dest_points.Add(new Vector2(x, y));
                        dest_colors.Add(backgroundImage.GetPixel(x, y));
                        source_points.Add(new Vector2((int)XPrime_avg.X, (int)XPrime_avg.Y));
                        source_colors.Add(backgroundImage.GetPixel((int)XPrime_avg.X, (int)XPrime_avg.Y));
                    }
                }
            }
        }

        // Makes sure that a pixel's negative value is set to 0
        public Vector2 validatePixel(Vector2 coord, int width, int height)
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

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff|All files|*.*";
                openFileDialog.Title = "Select an Image File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap loadedBitmap = new Bitmap(openFileDialog.FileName);

                    backgroundImage = ResizeBitmap(loadedBitmap, ClientSize.Width, ClientSize.Height);
                    if (type == FormType.SOURCE) ((Form1)MdiParent).TransitionInit(backgroundImage);
                    Refresh();
                }
            }
        }

        public void DeleteLines(Line line)
        {
            lines.Remove(line);
            deleting = false;
            Refresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedLine != null)
            {
                ((Form1)MdiParent).Reflect(selectedLine, type, Action.Delete);
                Refresh();
            }
        }

        public List<Line> GetLines()
        {
            return lines;
        }
        private void ImageBase_Load(object sender, EventArgs e)
        {
        }
    }
}