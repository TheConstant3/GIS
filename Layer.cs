using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MiniGIS
{
    public class Layer
    {
        protected List<MapObject> objects = new List<MapObject>();
        public string Name;
        public bool Visible;
        public bool Selected = false;
        internal Map map = null;
        public Pen Pen = new Pen(Color.Black);
        public Brush Brush = new SolidBrush(Color.LightGray);
        public Symbol Symbol = new Symbol();

        private Bounds bounds = new Bounds();
        public Bounds Bounds
        {
            get
            {
                if (!bounds.Valid) GetBounds();
                return bounds;
            }
        }

        public Map Map
        {
            get { return map; }
            set
            {
                if (map != null)
                    map.RemoveLayer(this);
                map = value;
            }
        }

        public Layer(string name)
        {
            Name = name;
            Visible = true;
        }

        public void AddObject(MapObject obj)
        {
            objects.Add(obj);
            obj.Layer = this;
        }

        public void RemoveObject(int index) => objects.RemoveAt(index);

        public void RemoveObject(MapObject item) => objects.Remove(item);

        public void RemoveAllObject() => objects.Clear();

        internal void Draw(PaintEventArgs e)
        {
            if (!Visible) return;
            foreach (var obj in objects)
            {
                obj.Draw(e);
            }
        }

        protected Bounds GetBounds()
        {
            return bounds = CalcBounds();
        }

        public Bounds CalcBounds()
        {
            bounds = new Bounds();
            foreach (var obj in objects)
            {
                bounds += obj.Bounds;
            }
            return bounds;
        }

        public MapObject FindObject(Vertex searchPoint, double d)
        {
            if (Visible)
                for (int i = objects.Count - 1; i >= 0; i--)
                {
                    if (objects[i].IsIntersectsWithQuad(searchPoint, d))
                    {
                        return objects[i];
                    }
                }
            return null;
        }

        internal void ClearSelection()
        {
            foreach (var obj in objects)
                obj.Selected = false;
        }
    }
}
