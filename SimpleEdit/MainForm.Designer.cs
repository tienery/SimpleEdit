namespace SimpleEdit
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlCode = new System.Windows.Forms.Panel();
            this.MainEditor = new SimpleEdit.Editor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.lblCurrentDir = new System.Windows.Forms.Label();
            this.txtCommand = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainEditor)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlCode);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.lblCurrentDir);
            this.splitContainer1.Panel2.Controls.Add(this.txtCommand);
            this.splitContainer1.Size = new System.Drawing.Size(784, 561);
            this.splitContainer1.SplitterDistance = 408;
            this.splitContainer1.TabIndex = 0;
            // 
            // pnlCode
            // 
            this.pnlCode.Controls.Add(this.MainEditor);
            this.pnlCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCode.Location = new System.Drawing.Point(0, 0);
            this.pnlCode.Name = "pnlCode";
            this.pnlCode.Size = new System.Drawing.Size(784, 408);
            this.pnlCode.TabIndex = 0;
            // 
            // MainEditor
            // 
            this.MainEditor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.MainEditor.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.MainEditor.BackBrush = null;
            this.MainEditor.CharHeight = 14;
            this.MainEditor.CharWidth = 8;
            this.MainEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MainEditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.MainEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainEditor.IsReplaceMode = false;
            this.MainEditor.Location = new System.Drawing.Point(0, 0);
            this.MainEditor.Name = "MainEditor";
            this.MainEditor.Paddings = new System.Windows.Forms.Padding(0);
            this.MainEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.MainEditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("MainEditor.ServiceColors")));
            this.MainEditor.Size = new System.Drawing.Size(784, 408);
            this.MainEditor.TabIndex = 0;
            this.MainEditor.Zoom = 100;
            this.MainEditor.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.MainEditor_TextChanged);
            this.MainEditor.Load += new System.EventHandler(this.MainEditor_Load);
            this.MainEditor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainEditor_KeyUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtResults);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 116);
            this.panel1.TabIndex = 2;
            // 
            // txtResults
            // 
            this.txtResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResults.Location = new System.Drawing.Point(0, 0);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ReadOnly = true;
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResults.Size = new System.Drawing.Size(784, 116);
            this.txtResults.TabIndex = 0;
            // 
            // lblCurrentDir
            // 
            this.lblCurrentDir.AutoSize = true;
            this.lblCurrentDir.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCurrentDir.Location = new System.Drawing.Point(0, 116);
            this.lblCurrentDir.Name = "lblCurrentDir";
            this.lblCurrentDir.Size = new System.Drawing.Size(92, 13);
            this.lblCurrentDir.TabIndex = 1;
            this.lblCurrentDir.Text = "Current Directory: ";
            // 
            // txtCommand
            // 
            this.txtCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtCommand.Location = new System.Drawing.Point(0, 129);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(784, 20);
            this.txtCommand.TabIndex = 0;
            this.txtCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainEditor_KeyUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simple Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainEditor)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlCode;
        private Editor MainEditor;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Label lblCurrentDir;
    }
}

