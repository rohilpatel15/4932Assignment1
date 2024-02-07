using System;
using System.Drawing;
using System.Windows.Forms;

namespace _4932Assignment1
{
    /* Line class creates a start and end point */
    public class Line
    {
        private int startX;
        private int startY;
        private int endX;
        private int endY;
        private int id;
        private static int counter = 0;
        private bool moving;
        private bool resizingStart;
        private bool resizingEnd;
        private Pen pen;
        private Pen circlePen;

        public Line(int startX, int startY, int endX = 0, int endY = 0)
        {
            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
            id = counter / 2;
            counter++;
            pen = new Pen(Color.Red, 4F);
            circlePen = new Pen(Color.Red, 4F);
        }

        public void Draw(PaintEventArgs e)
        {
            Point p1 = new Point(startX, startY);
            Point p2 = new Point(endX, endY);

            e.Graphics.DrawLine(pen, p1, p2);

            e.Graphics.DrawCircle(circlePen, startX, startY, 3.5F);
            e.Graphics.DrawCircle(circlePen, (startX + endX) / 2, (startY + endY) / 2, 3.5F);
            e.Graphics.DrawCircle(circlePen, endX, endY, 3.5F);
        }

        public int LineAction(MouseEventArgs e)
        {
            double lineCenterX = (startX + endX) / 2;
            double lineCenterY = (startY + endY) / 2;

            // Target the middle of the line to move
            if (Math.Abs(e.Location.X - lineCenterX) < 5 && Math.Abs(e.Location.Y - lineCenterY) < 5)
            {
                moving = true;
                resizingStart = false;
                resizingEnd = false;

                return Action.Move;
            }
            // Target the start of the line to resize
            else if (Math.Abs(e.Location.X - startX) < 6 && Math.Abs(e.Location.Y - startY) < 6)
            {
                moving = false;
                resizingStart = true;
                resizingEnd = false;

                return Action.ResizeStart;
            }
            // Target the end of the line to resize
            else if (Math.Abs(e.Location.X - endX) < 6 && Math.Abs(e.Location.Y - endY) < 6)
            {
                moving = false;
                resizingStart = false;
                resizingEnd = true;

                return Action.ResizeEnd;
            }

            // If the user is not interracting with an existing line, create a new one
            else return Action.CreateLine;
        }

        public void Resize(MouseEventArgs e)
        {
            if (moving)
            {
                int offsetX = e.Location.X - (StartX + EndX) / 2;
                int offsetY = e.Location.Y - (StartY + EndY) / 2;

                StartX += offsetX;
                StartY += offsetY;
                EndX += offsetX;
                EndY += offsetY;
            }
            else if (resizingStart)
            {
                StartX = e.Location.X;
                StartY = e.Location.Y;
            }
            else
            {
                EndX = e.Location.X;
                EndY = e.Location.Y;
            }
        }

        public void UpdateEndPoints(int endX, int endY)
        {
            this.endX = endX;
            this.endY = endY;
        }

        public int getId()
        {
            return id;
        }

        public int StartX
        {
            get { return startX; }
            set { startX = value; }
        }

        public int StartY
        {
            get { return startY; }
            set { startY = value; }
        }

        public int EndX
        {
            get { return endX; }
            set { endX = value; }
        }

        public int EndY
        {
            get { return endY; }
            set { endY = value; }
        }
    }
}
