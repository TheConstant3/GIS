using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MiniGIS
{
    public class Line : MapObject
    {
        private Vertex begin;
        private Vertex end;

        #region constructors

        public Line(Vertex begin, Vertex end)
        {
            this.begin = begin;
            this.end = end;
            objectType = MapObjectType.PolyLine;
        }

        public Line(double beginX, double beginY, double endX, double endY)
        {
            begin = new Vertex(beginX, beginY);
            end = new Vertex(endX, endY);
            objectType = MapObjectType.Line;
        }

        #endregion constructors

        #region properties

        public Vertex Begin
        {
            get { return begin; }
            set { begin = value; }
        }

        public Vertex End
        {
            get { return end; }
            set { end = value; }
        }
        public double BeginX
        {
            get { return begin.X; }
            set { begin.X = value; }
        }
        public double EndX
        {
            get { return end.X; }
            set { end.X = value; }
        }
        public double BeginY
        {
            get { return begin.Y; }
            set { begin.Y = value; }
        }
        public double EndY
        {
            get { return end.Y; }
            set { end.Y = value; }
        }

        #endregion properties

        internal override void Draw(PaintEventArgs e)
        {
            Pen pen = GetCurrentPen();
            var beginPoint = Layer.Map.MapToScreen(begin);
            var endPoint = Layer.Map.MapToScreen(end);
            e.Graphics.DrawLine(pen, beginPoint, endPoint);

        }

        protected override Bounds GetBounds()
        {
            return new Bounds(
                (begin.X < end.X) ? begin.X : end.X,
                (begin.Y > end.Y) ? begin.Y : end.Y,
                (begin.X > end.X) ? begin.X : end.X,
                (begin.Y < end.Y) ? begin.Y : end.Y
                );
        }

        internal override bool IsIntersectsWithQuad(Vertex searchPoint, double d)
        {
            if (IsSegmentIntersectsWithQuad(begin, end, searchPoint, d))
                return true;
            return false;
        }

        public override double Area()
        {
            return 0;
        }
    }
}
