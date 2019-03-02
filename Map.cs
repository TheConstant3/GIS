using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniGIS.Properties;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace MiniGIS
{
    public partial class Map : UserControl
    {
        protected List<Layer> layers = new List<Layer>();
        private List<MapObject> selectedObjects = new List<MapObject>();
        public List<Layer> Layers => layers;
        public string Name;
        private int snap = 3;
        public Color SelectoinColor { get; set; } = Color.Blue;
        private Vertex mapCenter = new Vertex(0, 0);
        private double mapScale = 1;
        public double MapScale
        {
            get { return mapScale; }
            set
            {
                if ((mapScale < 1000) || (value < mapScale))
                    mapScale = value;
                Invalidate();
            }
        }

        private bool IsMouseDown = false;
        private System.Drawing.Point mouseDownPosition = new System.Drawing.Point();
        public MapToolType activeTool;
        public MapToolType ActiveTool
        {
            get { return activeTool; }
            set
            {
                activeTool = value;
                switch (activeTool)
                {
                    case MapToolType.Select:
                        //Cursor = LoadCustomCursor(@"D:\Учеба\ГИС\MiniGIS\MiniGIS\Resources\arrow.cur");
                        Cursor = Cursors.Arrow;
                        break;
                    case MapToolType.Pan:
                        Cursor = LoadCustomCursor("hand.cur");
                        //Cursor = Cursors.Hand;
                        break;
                    case MapToolType.ZoomIn:
                        Cursor = LoadCustomCursor("zoom-in.cur");
                        //Cursor = Cursors.SizeAll;
                        break;
                    case MapToolType.ZoomOut:
                        Cursor = LoadCustomCursor("zoom-out.cur");
                        //Cursor = Cursors.SizeWE;
                        break;
                }
            }
        }
        
        public static Cursor LoadCustomCursor(string path)
        {
            IntPtr hCurs = LoadCursorFromFile(path);
            if (hCurs == IntPtr.Zero) throw new Win32Exception();
            var curs = new Cursor(hCurs);
            // Note: force the cursor to own the handle so it gets released properly
            var fi = typeof(Cursor).GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(curs, true);
            return curs;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);

        public Map()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(Map_MouseWheel);
        }

        public Vertex Center
        {
            get { return mapCenter; }
            set { mapCenter = value; }
        }
        
        public Bounds Bounds{ get { return CalcBounds(); } }//

        public List<MapObject> SelectedObjects { get => selectedObjects;}

        public Bounds CalcBounds()
        {
            Bounds bounds = new Bounds();
            foreach (var layer in layers)
            {
                if (layer.Visible)
                    bounds += layer.Bounds;
            }
            return bounds;
        }

        public void AddLayer(Layer layer)
        {
            layers.Add(layer);
            layer.map = this;
            //Invalidate();
        }

        public void RemoveLayer(int index) => layers.RemoveAt(index);

        public void RemoveLayer(Layer layer) => layers.Remove(layer);

        public void RemoveAllLayer() => layers.Clear();

        private void Map_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (var layer in layers)
            {
                if (layer.Visible)
                layer.Draw(e);
            }
        }

        public System.Drawing.Point MapToScreen(Vertex mapPoint)
        {
            System.Drawing.Point screenPoint = new System.Drawing.Point();
            screenPoint.X = (int)((mapPoint.X - Center.X) * MapScale + Width / 2 + 0.5);
            screenPoint.Y = (int)(-(mapPoint.Y - Center.Y) * MapScale + Height / 2 + 0.5);
            return screenPoint;
        }

        public Vertex ScreenToMap(System.Drawing.Point screenPoint)
        {
            Vertex mapPoint = new Vertex(0, 0);
            mapPoint.X = (screenPoint.X - Width / 2) / MapScale + Center.X;
            mapPoint.Y = -(screenPoint.Y - Height / 2) / MapScale + Center.Y;
            return mapPoint;
        }

        private void Map_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Map_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            mouseDownPosition = e.Location;
            //if (ActiveTool != MapToolType.Pan) return;
            //Cursor = LoadCustomCursor(@"D:\Учеба\ГИС\MiniGIS\MiniGIS\Resources\dnd-move.cur");
            switch (ActiveTool)
            {
                case MapToolType.Select:
                    break;
                case MapToolType.Pan:
                    Cursor = LoadCustomCursor("dnd-move.cur");
                    break;
                case MapToolType.ZoomIn:
                    break;
                case MapToolType.ZoomOut:
                    break;
            }
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            switch (ActiveTool)
            {
                case MapToolType.Select:
                    break;
                case MapToolType.Pan:
                    if (!IsMouseDown) return;
                    var _dX = (e.X - mouseDownPosition.X) / MapScale;
                    var _dY = (e.Y - mouseDownPosition.Y) / MapScale;
                    Center.X -= _dX;
                    Center.Y += _dY;
                    Invalidate();
                    mouseDownPosition = e.Location;
                    break;
                case MapToolType.ZoomIn:
                    if (!IsMouseDown) return;
                    var topLeft = new System.Drawing.Point
                    {
                        X = mouseDownPosition.X < e.X ? mouseDownPosition.X : e.X,
                        Y = mouseDownPosition.Y < e.Y ? mouseDownPosition.Y : e.Y
                    };
                    var bottomRight = new System.Drawing.Point
                    {
                        X = mouseDownPosition.X > e.X ? mouseDownPosition.X : e.X,
                        Y = mouseDownPosition.Y > e.Y ? mouseDownPosition.Y : e.Y
                    };
                    Graphics g = CreateGraphics();
                    g.DrawRectangle(new Pen(Color.Blue, 2), topLeft.X, topLeft.Y,
                    bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
                    Invalidate();
                    break;
                case MapToolType.ZoomOut:
                    break;
            }
        }

        private void Map_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            switch (ActiveTool)
            {
                case MapToolType.Select:
                    var dx = Math.Abs(mouseDownPosition.X - e.X);
                    var dy = Math.Abs(mouseDownPosition.Y - e.Y);

                    if (dx > snap && dy > snap) break;
                    var searchPoint = ScreenToMap(e.Location);
                    var d = snap / MapScale;

                    if (ModifierKeys != Keys.Control)
                    {
                        ClearSelection();
                        Invalidate();
                    }
                    var result = FindObject(searchPoint, d);

                    if (result == null) break;
                    result.Selected = true;
                    if(!selectedObjects.Contains(result))
                        selectedObjects.Add(result);

                    Invalidate();
                    break;
                case MapToolType.Pan:
                    Cursor = LoadCustomCursor("hand.cur");
                    break;
                case MapToolType.ZoomIn:
                    var _X = (mouseDownPosition.X + e.X) / 2;
                    var _Y = (mouseDownPosition.Y + e.Y) / 2;
                    Center = ScreenToMap(new System.Drawing.Point(_X, _Y));

                    var w = Math.Abs(mouseDownPosition.X - e.X);
                    var h = Math.Abs(mouseDownPosition.Y - e.Y);

                    if (w <= snap && h <= snap) MapScale *= 1.5;
                    if (w <= snap && h > snap) MapScale *= Height / h;
                    if (w > snap && h <= snap) MapScale *= Width / w;
                    if (w > snap && h > snap) MapScale *= Math.Min(Width / w, Height / h);

                    Invalidate();
                    break;
                case MapToolType.ZoomOut:
                    MapScale /= 1.5;
                    break;
            }
        }

        public MapObject FindObject(Vertex searchPoint, double d)
        {
            MapObject result = null;
            for (int i = Layers.Count - 1; i >= 0; i--)
            {
                MapObject searchObj = Layers[i].FindObject(searchPoint, d);
                if (searchObj != null)
                {
                    result = searchObj;
                    break;
                }
            }
            return result;
        }

        private void Map_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                MapScale *= 2;
            else
                MapScale /= 2;
        }

        public void ZoomAll()
        {
            if (!Bounds.Valid) return;
            var w = (Bounds.Xmax - Bounds.Xmin) * MapScale;
            var h = (Bounds.Ymax - Bounds.Ymin) * MapScale;
            Center = new Vertex((Bounds.Xmin + Bounds.Xmax) / 2, (Bounds.Ymin + Bounds.Ymax) / 2);
            if (!(w <= snap || h <= snap))
            mapScale *= Math.Min(Width / w, Height / h);
            Invalidate();
        }

        public void ZoomLayers(List<Layer> layers)
        {
            if (layers == null || layers.Count == 0) return;
            Bounds bounds = new Bounds();
            foreach (var layer in layers)
            {
                bounds += layer.Bounds;
            }
            if (!bounds.Valid) return;
            var w = (bounds.Xmax - bounds.Xmin) * MapScale;
            var h = (bounds.Ymax - bounds.Ymin) * MapScale;
            Center = new Vertex((bounds.Xmin + bounds.Xmax) / 2, (bounds.Ymin + bounds.Ymax) / 2);
            if (!(w == 0 || h == 0))
                mapScale *= Math.Min(Width / w, Height / h);
            Invalidate();
        }

        public void MoveLayerUp(Layer layer)
        {
            if (!layers.Contains(layer)) return;
            var index = layers.IndexOf(layer);
            if (index == layers.Count-1) return;
            layers.Remove(layer);
            layers.Insert(index + 1, layer);
            Invalidate();
        }
        
        public void MoveLayerDown(Layer layer)
        {
            if (!layers.Contains(layer)) return;
            var index = layers.IndexOf(layer);
            if (index == 0) return;
            layers.Remove(layer);
            layers.Insert(index - 1, layer);
            Invalidate();
        }

        public void ClearSelection()
        {
            foreach (var obj in selectedObjects)
                obj.Selected = false;
            selectedObjects.Clear();
        }

    }
}