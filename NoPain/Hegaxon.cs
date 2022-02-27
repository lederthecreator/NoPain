using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoPain
{
    internal class Hegaxon
    {
        private Point start, end;

        private Point[] points;

        public Point Start { get { return start; } }    
        public Point End { get { return end; } }

        public Point Center => new(Start.X + Radius, (Start.Y + End.Y) / 2);

        public int Radius => (int)Painter.Distance(Start, End) / 2;

        public Point[] Points
        {
            get
            {
                for (int i = 0; i < 7; i += 1)
                {
                    points[i] = new Point(Center.X + (int)(Radius * Math.Cos(i * Math.PI / 3)),
                        Center.Y + (int)(Radius * Math.Sin(i * Math.PI / 3)));
                }
                return points;
            }
        }

        public Hegaxon(Point start, Point end)
        {
            this.start = start;
            this.end = end;
            points = new Point[7];
        }

    }
}
