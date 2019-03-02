using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MiniGIS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ButtonSelect.Checked = true;
            toolStripStatusLabelMouse.Text = "";
            toolStripStatusLabelArea.Text = "";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Layer layer2 = new Layer("Test2");
            
            Line line1 = new Line(new Vertex(0, -50), new Vertex(0, 50));
            Line line2 = new Line(new Vertex(-50, 0), new Vertex(50, 0));
            line1.Pen = new Pen(Color.Red, 2);
            line1.Pen.DashStyle = DashStyle.Dash;
            line1.UseOwnStyle = true;
            line2.Pen = new Pen(Color.Red, 2);
            line2.Pen.DashStyle = DashStyle.Dash;
            line2.UseOwnStyle = true;

            layer2.AddObject(line1);
            layer2.AddObject(line2);
            map.AddLayer(layer2);
            layersControl.Map = map;
            layersControl.AddLayer += new System.EventHandler(this.AddLayersFromFiles);
            layersControl.UpdateLayers();
        }

        private void AddLayersFromFiles(object sender, EventArgs e)
        {
            openFileDialog.Filter = "MIF|*.mif";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] filePathName = openFileDialog.FileNames;
                string[] filename = openFileDialog.SafeFileNames;
                for (int i = 0; i < filePathName.Length; ++i)
                {
                    int extansIndex = filename[i].IndexOf(".");
                    string layerName = filename[i].Substring(0, extansIndex);
                    var layer = new Layer(layerName);
                    var parser = new MIFParser(filePathName[i]);
                    foreach (var mapObject in parser.Data)
                    {
                        layer.AddObject(mapObject);
                    }
                    map.AddLayer(layer);
                    layersControl.UpdateLayers();
                }
            }
        }

        private void ButtonZoomIn_Click(object sender, EventArgs e)
        {
            map.ActiveTool = MapToolType.ZoomIn;
            ButtonSelect.Checked = false;
            ButtonPan.Checked = false;
            ButtonZoomIn.Checked = true;
            ButtonZoomOut.Checked = false;
        }

        private void ButtonZoomOut_Click(object sender, EventArgs e)
        {
            map.ActiveTool = MapToolType.ZoomOut;
            ButtonSelect.Checked = false;
            ButtonPan.Checked = false;
            ButtonZoomIn.Checked = false;
            ButtonZoomOut.Checked = true;
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            map.ActiveTool = MapToolType.Select;
            ButtonSelect.Checked = true;
            ButtonPan.Checked = false;
            ButtonZoomIn.Checked = false;
            ButtonZoomOut.Checked = false;
        }

        private void ButtonPan_Click(object sender, EventArgs e)
        {
            map.ActiveTool = MapToolType.Pan;
            ButtonSelect.Checked = false;
            ButtonPan.Checked = true;
            ButtonZoomIn.Checked = false;
            ButtonZoomOut.Checked = false;
        }

        private void map_MouseMove(object sender, MouseEventArgs e)
        {
            var mapCursorPosition = map.ScreenToMap(e.Location);
            toolStripStatusLabelMouse.Text = "x=" + Math.Round(mapCursorPosition.X,4) + "; y=" + Math.Round(mapCursorPosition.Y, 4);
        }

        private void ButtonZoomAll_Click(object sender, EventArgs e)
        {
            if (layersControl.SelectedItemsCount == 0)
                map.ZoomAll();
            else
                map.ZoomLayers(layersControl.GetSelectedLayers());
        }

        private void map_MouseUp(object sender, MouseEventArgs e)
        {
            toolStripStatusLabelArea.Text = "";
            if (map.SelectedObjects.Count!=1) return;
            if (map.SelectedObjects[0].ObjectType!=MapObjectType.Polygon) return;
            toolStripStatusLabelArea.Text += "Polygon's Area = ";
            toolStripStatusLabelArea.Text += Math.Round(map.SelectedObjects[0].Area(), 4);
        }
    }
}
