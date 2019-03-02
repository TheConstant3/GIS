namespace MiniGIS
{
    partial class LayersControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView = new System.Windows.Forms.ListView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.ButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonUp = new System.Windows.Forms.ToolStripButton();
            this.ButtonDown = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listView.CheckBoxes = true;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(292, 262);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            this.listView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView_ItemChecked);
            this.listView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_ItemSelectionChanged);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonAdd,
            this.ButtonRemove,
            this.toolStripSeparator1,
            this.ButtonUp,
            this.ButtonDown});
            this.toolStrip.Location = new System.Drawing.Point(0, 262);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(292, 25);
            this.toolStrip.TabIndex = 1;
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonAdd.Image = global::MiniGIS.Properties.Resources.add;
            this.ButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(23, 22);
            this.ButtonAdd.Text = "Add new layer";
            this.ButtonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // ButtonRemove
            // 
            this.ButtonRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonRemove.Image = global::MiniGIS.Properties.Resources.remove;
            this.ButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonRemove.Name = "ButtonRemove";
            this.ButtonRemove.Size = new System.Drawing.Size(23, 22);
            this.ButtonRemove.Text = "Remove layer";
            this.ButtonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonUp
            // 
            this.ButtonUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonUp.Image = global::MiniGIS.Properties.Resources.up;
            this.ButtonUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonUp.Name = "ButtonUp";
            this.ButtonUp.Size = new System.Drawing.Size(23, 22);
            this.ButtonUp.Text = "Move up";
            this.ButtonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // ButtonDown
            // 
            this.ButtonDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonDown.Image = global::MiniGIS.Properties.Resources.down;
            this.ButtonDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonDown.Name = "ButtonDown";
            this.ButtonDown.Size = new System.Drawing.Size(23, 22);
            this.ButtonDown.Text = "Move down";
            this.ButtonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // LayersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView);
            this.Controls.Add(this.toolStrip);
            this.Name = "LayersControl";
            this.Size = new System.Drawing.Size(292, 287);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton ButtonAdd;
        private System.Windows.Forms.ToolStripButton ButtonRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ButtonDown;
        private System.Windows.Forms.ToolStripButton ButtonUp;
    }
}
