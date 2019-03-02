using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScreenPoint = System.Drawing.Point;
using System.Drawing;


namespace MiniGIS
{
    public class Polygon:Polyline
    {
        public Polygon()
        {
            objectType = MapObjectType.Polygon;
        }

        internal override void Draw(PaintEventArgs e)
        {
            if (nodes.Count < 2) return;

            List<ScreenPoint> points = new List<ScreenPoint>();
            foreach (var node in nodes)
            {
                var point = Layer.Map.MapToScreen(node);
                points.Add(point);
            }
            e.Graphics.FillPolygon(GetCurrentBrush(), points.ToArray());
            e.Graphics.DrawPolygon(GetCurrentPen(), points.ToArray());
        }

        internal override bool IsIntersectsWithQuad(Vertex searchPoint, double d)
        {
            if (base.IsIntersectsWithQuad(searchPoint, d)) return true;

            if (IsSegmentIntersectsWithQuad(nodes.Last(), nodes[0], searchPoint, d))
                return true;

            if (IsContainPoint(searchPoint.X, searchPoint.Y)) return true;

            return false;
        }

        public bool IsContainPoint(double x, double y)
        {
            bool c = false; //https://ru.wikibooks.org/wiki/%D0%A0%D0%B5%D0%B0%D0%BB%D0%B8%D0%B7%D0%B0%D1%86%D0%B8%D0%B8_%D0%B0%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC%D0%BE%D0%B2/%D0%97%D0%B0%D0%B4%D0%B0%D1%87%D0%B0_%D0%BE_%D0%BF%D1%80%D0%B8%D0%BD%D0%B0%D0%B4%D0%BB%D0%B5%D0%B6%D0%BD%D0%BE%D1%81%D1%82%D0%B8_%D1%82%D0%BE%D1%87%D0%BA%D0%B8_%D0%BC%D0%BD%D0%BE%D0%B3%D0%BE%D1%83%D0%B3%D0%BE%D0%BB%D1%8C%D0%BD%D0%B8%D0%BA%D1%83#%D0%9E%D1%87%D0%B5%D0%BD%D1%8C_%D0%B1%D1%8B%D1%81%D1%82%D1%80%D1%8B%D0%B9_%D0%B0%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC
            for (int i = 0, j = nodes.Count - 1; i < nodes.Count; j = i++)
            {
                if ((((nodes[i].Y <= y) && (y < nodes[j].Y)) || ((nodes[j].Y <= y) && (y < nodes[i].Y))) &&
                  (x > (nodes[j].X - nodes[i].X) * (y - nodes[i].Y) / (nodes[j].Y - nodes[i].Y) + nodes[i].X))
                    c = !c;
            }
            return c;
            //foreach (var node in nodes)
        }

        public override double Area()
        {
            double area = 0.0;
            int n = nodes.Count;
            for (int i = 0; i < n - 1; ++i)
            {
                area += nodes[i].X * nodes[i + 1].Y - nodes[i + 1].X * nodes[i].Y;
            }
            area += nodes[n - 1].X * nodes[0].Y;
            area -= nodes[0].X * nodes[n - 1].Y;
            return Math.Abs(area) / 2.0;
        }
    }
}
