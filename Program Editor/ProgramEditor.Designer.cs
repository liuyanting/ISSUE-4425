namespace Program_Editor
{
	partial class ProgramEditor
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
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			this.MenuStrip = new System.Windows.Forms.MenuStrip();
			this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SelectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RemoveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnStart = new System.Windows.Forms.Button();
			this.StatusStrip = new System.Windows.Forms.StatusStrip();
			this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.FileListView = new System.Windows.Forms.ListView();
			this.FileNameColumn = new System.Windows.Forms.ColumnHeader();
			this.StatusColumn = new System.Windows.Forms.ColumnHeader();
			this.BackgroundEditor = new System.ComponentModel.BackgroundWorker();
			this.MenuStrip.SuspendLayout();
			this.StatusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// MenuStrip
			// 
			this.MenuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenuItem,
            this.SelectAllMenuItem,
            this.RemoveMenuItem,
            this.AboutMenuItem} );
			this.MenuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.MenuStrip.Location = new System.Drawing.Point( 0, 0 );
			this.MenuStrip.Name = "MenuStrip";
			this.MenuStrip.Size = new System.Drawing.Size( 583, 40 );
			this.MenuStrip.TabIndex = 0;
			this.MenuStrip.Text = "menuStrip1";
			// 
			// OpenMenuItem
			// 
			this.OpenMenuItem.Image = global::Program_Editor.Properties.Resources.open;
			this.OpenMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.OpenMenuItem.Name = "OpenMenuItem";
			this.OpenMenuItem.Size = new System.Drawing.Size( 44, 36 );
			this.OpenMenuItem.ToolTipText = "Open";
			this.OpenMenuItem.MouseLeave += new System.EventHandler( this.OpenMenuItem_MouseLeave );
			this.OpenMenuItem.MouseEnter += new System.EventHandler( this.OpenMenuItem_MouseEnter );
			this.OpenMenuItem.Click += new System.EventHandler( this.OpenMenuItem_Click );
			// 
			// SelectAllMenuItem
			// 
			this.SelectAllMenuItem.Image = global::Program_Editor.Properties.Resources.select_all;
			this.SelectAllMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.SelectAllMenuItem.Name = "SelectAllMenuItem";
			this.SelectAllMenuItem.Size = new System.Drawing.Size( 44, 36 );
			this.SelectAllMenuItem.ToolTipText = "Select All";
			this.SelectAllMenuItem.MouseLeave += new System.EventHandler( this.SelectAllMenuItem_MouseLeave );
			this.SelectAllMenuItem.MouseEnter += new System.EventHandler( this.SelectAllMenuItem_MouseEnter );
			this.SelectAllMenuItem.Click += new System.EventHandler( this.SelectAllMenuItem_Click );
			// 
			// RemoveMenuItem
			// 
			this.RemoveMenuItem.Image = global::Program_Editor.Properties.Resources.remove;
			this.RemoveMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.RemoveMenuItem.Name = "RemoveMenuItem";
			this.RemoveMenuItem.Size = new System.Drawing.Size( 44, 36 );
			this.RemoveMenuItem.MouseLeave += new System.EventHandler( this.RemoveMenuItem_MouseLeave );
			this.RemoveMenuItem.MouseEnter += new System.EventHandler( this.RemoveMenuItem_MouseEnter );
			this.RemoveMenuItem.Click += new System.EventHandler( this.RemoveMenuItem_Click );
			// 
			// AboutMenuItem
			// 
			this.AboutMenuItem.Image = global::Program_Editor.Properties.Resources.about;
			this.AboutMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.AboutMenuItem.Name = "AboutMenuItem";
			this.AboutMenuItem.Size = new System.Drawing.Size( 44, 36 );
			this.AboutMenuItem.ToolTipText = "About";
			this.AboutMenuItem.MouseLeave += new System.EventHandler( this.AboutMenuItem_MouseLeave );
			this.AboutMenuItem.MouseEnter += new System.EventHandler( this.AboutMenuItem_MouseEnter );
			this.AboutMenuItem.Click += new System.EventHandler( this.AboutMenuItem_Click );
			// 
			// btnStart
			// 
			this.btnStart.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
			this.btnStart.Location = new System.Drawing.Point( 213, 254 );
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size( 157, 23 );
			this.btnStart.TabIndex = 2;
			this.btnStart.Text = "Start Conversion";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler( this.StartButton_Click );
			// 
			// StatusStrip
			// 
			this.StatusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel} );
			this.StatusStrip.Location = new System.Drawing.Point( 0, 280 );
			this.StatusStrip.Name = "StatusStrip";
			this.StatusStrip.Size = new System.Drawing.Size( 583, 22 );
			this.StatusStrip.TabIndex = 3;
			this.StatusStrip.Text = "statusStrip1";
			// 
			// StatusLabel
			// 
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Size = new System.Drawing.Size( 113, 17 );
			this.StatusLabel.Text = "Processing BLAH.txt";
			// 
			// FileListView
			// 
			this.FileListView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.FileNameColumn,
            this.StatusColumn} );
			this.FileListView.FullRowSelect = true;
			this.FileListView.GridLines = true;
			this.FileListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.FileListView.HideSelection = false;
			this.FileListView.Location = new System.Drawing.Point( 0, 43 );
			this.FileListView.Name = "FileListView";
			this.FileListView.Size = new System.Drawing.Size( 583, 205 );
			this.FileListView.TabIndex = 4;
			this.FileListView.UseCompatibleStateImageBehavior = false;
			this.FileListView.View = System.Windows.Forms.View.Details;
			this.FileListView.DoubleClick += new System.EventHandler( this.FileListView_DoubleClick );
			// 
			// FileNameColumn
			// 
			this.FileNameColumn.Text = "File Name";
			this.FileNameColumn.Width = 312;
			// 
			// StatusColumn
			// 
			this.StatusColumn.Text = "Status";
			this.StatusColumn.Width = 207;
			// 
			// BackgroundEditor
			// 
			this.BackgroundEditor.WorkerReportsProgress = true;
			this.BackgroundEditor.WorkerSupportsCancellation = true;
			this.BackgroundEditor.DoWork += new System.ComponentModel.DoWorkEventHandler( this.BackgroundEditor_DoWork );
			this.BackgroundEditor.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.BackgroundEditor_RunWorkerCompleted );
			this.BackgroundEditor.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler( this.BackgroundEditor_ProgressChanged );
			// 
			// ProgramEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 583, 302 );
			this.Controls.Add( this.FileListView );
			this.Controls.Add( this.StatusStrip );
			this.Controls.Add( this.btnStart );
			this.Controls.Add( this.MenuStrip );
			this.MainMenuStrip = this.MenuStrip;
			this.Name = "ProgramEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sub-Program Converter";
			this.MenuStrip.ResumeLayout( false );
			this.MenuStrip.PerformLayout();
			this.StatusStrip.ResumeLayout( false );
			this.StatusStrip.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MenuStrip;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.StatusStrip StatusStrip;
		private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
		private System.Windows.Forms.ListView FileListView;
		private System.Windows.Forms.ColumnHeader FileNameColumn;
		private System.Windows.Forms.ColumnHeader StatusColumn;
		private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
		private System.ComponentModel.BackgroundWorker BackgroundEditor;
		private System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SelectAllMenuItem;
		private System.Windows.Forms.ToolStripMenuItem RemoveMenuItem;
	}
}