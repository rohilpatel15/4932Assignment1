﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4932Assignment1
{
    /* Line class creates a start and end point */
    public class Line
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public int id;
        public Line(Point startpoint, Point endpoint, int id)
        {
            StartPoint = startpoint;
            EndPoint = endpoint;
            this.id = id;
        }

        public bool IsNear(Point p, int threshold = 5)
        {
            float dx = EndPoint.X - StartPoint.X;
            float dy = EndPoint.Y - StartPoint.Y;
            float lengthSquared = dx * dx + dy * dy;

            float t = ((p.X - StartPoint.X) * dx + (p.Y - StartPoint.Y) * dy) / lengthSquared;
            t = Math.Max(0, Math.Min(1, t));

            float closestPointX = StartPoint.X + t * dx;
            float closestPointY = StartPoint.Y + t * dy;

            float distanceSquared = (p.X - closestPointX) * (p.X - closestPointX) +
                                    (p.Y - closestPointY) * (p.Y - closestPointY);

            return distanceSquared <= threshold * threshold;
        }

        // Move the line
        public void Move(int deltaX, int deltaY)
        {
            StartPoint = new Point(StartPoint.X + deltaX, StartPoint.Y + deltaY);
            EndPoint = new Point(EndPoint.X + deltaX, EndPoint.Y + deltaY);
        }
     }
}