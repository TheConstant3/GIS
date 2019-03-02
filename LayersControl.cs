using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniGIS
{
    public partial class LayersControl : UserControl
    {
        public LayersControl()
        {
            InitializeComponent();
        }

        public Map Map;

        public int SelectedItemsCount
        {
            get { return listView.SelectedItems.Count; }
        }

        public event EventHandler AddLayer;

        private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var layer = (Layer)e.Item.Tag;
            if (layer == null) return;
            layer.Visible = !layer.Visible;
            Map.Invalidate();
        }

        public void UpdateLayers()
        {
            if (Map == null) return;
            listView.Items.Clear();
            foreach (var layer in Map.Layers)
            {
                var listViewItem = new ListViewItem();
                listViewItem.Text = layer.Name;
                listViewItem.Checked = layer.Visible;
                listViewItem.Selected = layer.Selected;
                listViewItem.Tag = layer;
                listView.Items.Insert(0, listViewItem);
                var tmplayer = (Layer)listViewItem.Tag;
                tmplayer.Visible = listViewItem.Checked;
            }
            CheckButtoons();
        }

        private void RemoveSelectedLayers()
        {
            if (Map == null) return;
            if (listView.SelectedItems.Count == 0) return;
            foreach (ListViewItem item in listView.SelectedItems)
            {
                Layer layer = (Layer)item.Tag;
                if (layer != null)
                {
                    Map.RemoveLayer(layer);
                }
            }
            UpdateLayers();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedLayers();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (AddLayer == null) return;
            AddLayer(sender, e);
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            MoveSelectedLayerUp();
        }

        private void MoveSelectedLayerUp()
        {
            if (Map == null) return;
            if (listView.SelectedItems.Count != 1) return;
            Map.MoveLayerUp((Layer)listView.SelectedItems[0].Tag);
            UpdateLayers();
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            MoveSelectedLayerDown();
        }

        private void MoveSelectedLayerDown()
        {
            if (Map == null) return;
            if (listView.SelectedItems.Count != 1) return;
            Map.MoveLayerDown((Layer)listView.SelectedItems[0].Tag);
            UpdateLayers();
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckButtoons();
        }

        private void CheckButtoons()
        {
            ButtonRemove.Enabled = listView.SelectedItems.Count > 0;
            ButtonUp.Enabled = listView.SelectedItems.Count == 1 && listView.SelectedItems[0].Index > 0;
            ButtonDown.Enabled = listView.SelectedItems.Count == 1 && listView.SelectedItems[0].Index < listView.Items.Count - 1;
        }

        public List<Layer> GetSelectedLayers()
        {
            List<Layer> layers = new List<Layer>();
            if (listView.SelectedItems.Count == 0) return layers;
            foreach (ListViewItem item in listView.SelectedItems)
            {
                Layer layer = (Layer)item.Tag;
                if (layer != null)
                {
                    layers.Add(layer);
                }
            }
            return layers;
        }

        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            Layer layer = (Layer)e.Item.Tag;
            layer.Selected = e.IsSelected;
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            UpdateLayers();
        }
    }
}
