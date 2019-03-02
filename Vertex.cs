using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS
{
    public class Vertex
    {
        private double x;
        private double y;

        public Vertex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #region properties

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        #endregion properties
    }
}
