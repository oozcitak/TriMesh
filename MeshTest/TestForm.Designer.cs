namespace MeshTest
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.meshView = new SimpleCAD.CADWindow();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.coordLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerAnim = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // meshView
            // 
            this.meshView.AllowItemClick = true;
            this.meshView.AllowZoomAndPan = true;
            this.meshView.BackColor = System.Drawing.Color.White;
            this.meshView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.meshView.Cursor = System.Windows.Forms.Cursors.Cross;
            this.meshView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meshView.Location = new System.Drawing.Point(0, 0);
            this.meshView.Name = "meshView";
            this.meshView.Size = new System.Drawing.Size(990, 514);
            this.meshView.TabIndex = 0;
            this.meshView.ItemClick += new SimpleCAD.ItemClickEventHandler(this.meshView_ItemClick);
            this.meshView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.meshView_KeyDown);
            this.meshView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.meshView_KeyUp);
            this.meshView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.meshView_MouseMove);
            this.meshView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.meshView_PreviewKeyDown);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.meshView);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(990, 514);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(990, 561);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.coordLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(990, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // coordLabel
            // 
            this.coordLabel.Name = "coordLabel";
            this.coordLabel.Size = new System.Drawing.Size(936, 17);
            this.coordLabel.Spring = true;
            this.coordLabel.Text = "0, 0";
            this.coordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timerAnim
            // 
            this.timerAnim.Enabled = true;
            this.timerAnim.Tick += new System.EventHandler(this.timerAnim_Tick);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 561);
            this.Controls.Add(this.toolStripContainer1);
            this.KeyPreview = true;
            this.Name = "TestForm";
            this.Text = "Mesh Test Form";
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SimpleCAD.CADWindow meshView;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel coordLabel;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Timer timerAnim;
    }
}

