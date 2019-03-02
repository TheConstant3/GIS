namespace MiniGIS
{
    partial class Map
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
            this.SuspendLayout();
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "Map";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Map_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Map_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_MouseUp);
            this.Resize += new System.EventHandler(this.Map_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
