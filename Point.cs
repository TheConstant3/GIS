using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MiniGIS
{
    /// <summary>
    /// Класс для работы с точечными объектами
    /// </summary>
    public class Point : MapObject
    {
        private Vertex position;

        #region constructors

        public Point(double x, double y)
        {
            position = new Vertex(x, y);
            objectType = MapObjectType.Point;
        }

        public Point(Vertex vertex)
        {
            position = vertex;
            objectType = MapObjectType.Point;
        }

        #endregion constructors

        #region properties

        public double X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public double Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        #endregion properties

        internal override void Draw(PaintEventArgs e)
        {
            Char str = (Char)Symbol.Number;
            e.Graphics.DrawString(str.ToString(), Symbol.Font, Brush, Layer.Map.MapToScreen(position), Symbol.drawFormat);
        }

        protected override Bounds GetBounds()
        {
            return new Bounds(position.X, position.Y, position.X, position.Y);
        }

        internal override bool IsIntersectsWithQuad(Vertex searchPoint, double d)
        {
            return false;
        }

        public override double Area()
        {
            return 0;
        }
    }
}
