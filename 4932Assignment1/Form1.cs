using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace _4932Assignment1
{
    public partial class Form1 : Form
    {
        // Create form objects individually
        private FormBuilder destination;
        private FormBuilder transition;
        private FormBuilder source;
        private Controller controller;
        // Create bitmaps for intermediate frames in both directions
        private List<Bitmap> forwardframes;
        private List<Bitmap> backwardframes;
        private List<Bitmap> frames;
        private int num_frames;
        private int num_threads;
        private double time;
        private double time2;
        public Form1()
        {
            InitializeComponent();
            forwardframes = new List<Bitmap>();
            backwardframes = new List<Bitmap>();
            frames = new List<Bitmap>();

            // Start the program with 5 frames and 2 threads default
            num_frames = 5;
            num_threads = 2;
            t2.Checked = true;
            frames5.Checked = true;

            // Values to make space between forms
            int offsetX = 20;
            int offsetY = 20;

            // Create forms
            source = new FormBuilder(FormType.SOURCE, 0);
            source.MdiParent = this;
            source.Show();
            source.Location = new Point(0, 0);

            destination = new FormBuilder(FormType.DESTINATION, 1);
            destination.MdiParent = this;
            destination.Show();
            destination.Location = new Point(source.Right + offsetX, 0);

            transition = new FormBuilder(FormType.FINAL, 2);
            transition.MdiParent = this;
            transition.Show();
            transition.Location = new Point(destination.Right + offsetX, 0);
        }

        // Cross dissolves frames
        public List<Bitmap> crossDissolve(List<Bitmap> a, List<Bitmap> b)
        {
            List<Bitmap> result = new List<Bitmap>();
            for (int i = 0; i < a.Count; i++)
            {
                result.Add(new Bitmap(a[i].Width, a[i].Height));
                for (int x = 0; x < a[i].Width; x++)
                {
                    for (int y = 0; y < a[i].Height; y++)
                    {
                        double t = (double)(i) / (a.Count - 1);
                        Color colorA = a[i].GetPixel(x, y);
                        Color colorB = b[i].GetPixel(x, y);
                        int newR = (int)Math.Round((1 - t) * colorA.R + t * colorB.R);
                        int newG = (int)Math.Round((1 - t) * colorA.G + t * colorB.G);
                        int newB = (int)Math.Round((1 - t) * colorA.B + t * colorB.B);
                        int newA = (int)Math.Round((1 - t) * colorA.A + t * colorB.A);
                        result[i].SetPixel(x, y, Color.FromArgb(newA, newR, newG, newB));
                    }
                }
            }
            return result;
        }

        // Function handles adding and deleting lines between the two forms
        public void Reflect(Line line, int origin, int intention)
        {
            if (intention == Action.Delete)
            {
                if (origin == FormType.SOURCE)
                {
                    Line destinationLine = destination.getLine(line.getId());
                    destination.DeleteLines(destinationLine);
                    source.DeleteLines(line);
                }
                else if (origin == FormType.DESTINATION)
                {
                    Line sourceLine = source.getLine(line.getId());
                    source.DeleteLines(sourceLine);
                    destination.DeleteLines(line);
                }
                return;
            }

            Line copiedLine = new Line(line.StartX, line.StartY, line.EndX, line.EndY);
            if (origin == FormType.SOURCE)
            {
                destination.AddLines(copiedLine);
            }
            else if (origin == FormType.DESTINATION)
            {
                source.AddLines(copiedLine);
            }
            source.Invalidate();
            destination.Invalidate();
        }

        // Creates intermediate frames between two images
        public List<Bitmap> GenerateIntermediateFrames(List<Bitmap> frames, List<Vector2> dest_points, List<Vector2> source_points, Bitmap final_image, Bitmap destination_image, List<Color> dest_colors, List<Color> source_colors)
        {
            List<Vector2> new_dest_points = new List<Vector2>(dest_points);
            frames.Add(destination_image);
            for (int frameIndex = 0; frameIndex < num_frames - 1; frameIndex++)
            {
                Bitmap frame = new Bitmap(destination_image.Width, destination_image.Height);

                for (int i = 0; i < dest_points.Count; i++)
                {
                    float diff_X = (dest_points[i].X - source_points[i].X) / num_frames;
                    float diff_Y = (dest_points[i].Y - source_points[i].Y) / num_frames;
                    Vector2 diffVector = new Vector2(diff_X, diff_Y);

                    new_dest_points[i] = Vector2.Subtract(new_dest_points[i], diffVector);
                    new_dest_points[i] = destination.validatePixel(new_dest_points[i], frame.Width, frame.Height);

                    frame.SetPixel((int)dest_points[i].X, (int)dest_points[i].Y, destination_image.GetPixel((int)new_dest_points[i].X, (int)new_dest_points[i].Y));
                }

                frames.Add(frame);
            }
            frames.Add(final_image);
            return frames;
        }

        // Creates the controller and shows how long it takes to morph
        private void beginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            List<Bitmap> dummy = new List<Bitmap>();

            sw.Start();
            dummy = destination.Morph(dummy, source.GetLines(), num_frames, 1);
            dummy = source.Morph(dummy, destination.GetLines(), num_frames, 1);
            sw.Stop();
            time2 = sw.Elapsed.TotalSeconds;

            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            backwardframes = destination.Morph(backwardframes, source.GetLines(), num_frames, num_threads);
            forwardframes = source.Morph(forwardframes, destination.GetLines(), num_frames, num_threads);
            sw2.Stop();
            time = sw2.Elapsed.TotalSeconds;

            backwardframes.Reverse();
            frames = crossDissolve(forwardframes, backwardframes);

            controller = new Controller(frames, transition);
            controller.MdiParent = this;
            controller.Show();
            controller.Location = new Point(0, Math.Max(source.Bottom, Math.Max(destination.Bottom, transition.Bottom)) + 10);
        }

        private void frames5_Click(object sender, EventArgs e)
        {
            frames5.Checked = true;
            frames10.Checked = false;
            num_frames = 5;
        }

        private void frames10_Click(object sender, EventArgs e)
        {
            frames5.Checked = false;
            frames10.Checked = true;
            num_frames = 10;
        }

        private void t1_Click(object sender, EventArgs e)
        {
            t1.Checked = true;
            t2.Checked = false;
            t3.Checked = false;
            t4.Checked = false;
            t8.Checked = false;
            num_threads = 1;
        }

        private void t2_Click(object sender, EventArgs e)
        {
            t1.Checked = false;
            t2.Checked = true;
            t3.Checked = false;
            t4.Checked = false;
            t8.Checked = false;
            num_threads = 2;
        }

        private void t3_Click(object sender, EventArgs e)
        {
            t1.Checked = false;
            t2.Checked = false;
            t3.Checked = true;
            t4.Checked = false;
            t8.Checked = false;
            num_threads = 3;
        }

        private void t4_Click(object sender, EventArgs e)
        {
            t1.Checked = false;
            t2.Checked = false;
            t3.Checked = false;
            t4.Checked = true;
            t8.Checked = false;
            num_threads = 4;
        }

        private void t8_Click(object sender, EventArgs e)
        {
            t1.Checked = false;
            t2.Checked = false;
            t3.Checked = false;
            t4.Checked = false;
            t8.Checked = true;
            num_threads = 8;
        }

        // Sets final image as source image
        public void TransitionInit(Bitmap img)
        {
            transition.SetImage(img);
        }

        public List<Bitmap> GetFrames()
        {
            return frames;
        }
        public int GetThreads()
        {
            return num_threads;
        }

        public double GetThreadedTime()
        {
            return time;
        }

        public double Get1ThreadedTime()
        {
            return time2;
        }

        private void Main_Load(object sender, EventArgs e)
        {
        }

        private void menu_strip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
    }
}