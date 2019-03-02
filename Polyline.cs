using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScreenPoint = System.Drawing.Point;
using System.Drawing;

namespace MiniGIS
{
    public class Polyline : MapObject
    {
        protected List<Vertex> nodes = new List<Vertex>();
        private Bounds bounds = new Bounds();//
        public Bounds Bounds
        {
            get
            {
                if (!bounds.Valid) GetBounds();
                return bounds;
            }
        }

        public Polyline()
        {
            objectType = MapObjectType.PolyLine;
        }

        public void AddNode(Vertex node)
        {
            nodes.Add(node);
        }

        /// <summary>
        /// Добавляет вершину
        /// </summary>
        /// <param name="x">координата x</param>
        /// <param name="y">координата y</param>
        public void AddNode(double x, double y)
        {
            nodes.Add(new Vertex(x, y));
        }

        public void RemoveNode(int index)
        {
            nodes.RemoveAt(index);
        }

        public void RemoveNode(Vertex item)
        {
            nodes.Remove(item);
        }

        public void RemoveAllNode()
        {
            nodes.Clear();
        }

        internal override void Draw(PaintEventArgs e)
        {
            if (nodes.Count < 2) return;

            Pen pen = GetCurrentPen();
            List<ScreenPoint> points = new List<ScreenPoint>();
            foreach (var node in nodes)
            {
                var point = Layer.Map.MapToScreen(node);
                points.Add(point);
            }
            e.Graphics.DrawLines(pen, points.ToArray());
        }

        protected override Bounds GetBounds()
        {
            return bounds = CalcBounds();
        }

        public Bounds CalcBounds()
        {
            Bounds tempBounds1 = new Bounds();
            Bounds tempBounds2 = new Bounds();
            foreach (var node in nodes)
            {
                tempBounds2.SetBounds(node, node);
                tempBounds1 += tempBounds2;
            }
            return tempBounds1;
        }

        internal override bool IsIntersectsWithQuad(Vertex searchPoint, double d)
        {
            if (nodes.Count == 0) return false;
            if (nodes.Count == 1) return IsSegmentIntersectsWithQuad(nodes[0], nodes[0], searchPoint, d);
            for (int x = 0; x < nodes.Count - 1; x++)
                if (IsSegmentIntersectsWithQuad(nodes[x], nodes[x + 1], searchPoint, d))
                    return true;
            return false;
        }

        public override double Area()
        {
            return 0;
        }
    }
}
