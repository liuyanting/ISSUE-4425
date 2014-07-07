namespace Program_Editor
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
			this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FileMenuSeperator1 = new System.Windows.Forms.ToolStripSeparator();
			this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.SelectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.RemoveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StartButton = new System.Windows.Forms.Button();
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
            this.FileMenu,
            this.EditMenu,
            this.AboutMenuItem} );
			this.MenuStrip.Location = new System.Drawing.Point( 0, 0 );
			this.MenuStrip.Name = "MenuStrip";
			this.MenuStrip.Size = new System.Drawing.Size( 284, 24 );
			this.MenuStrip.TabIndex = 0;
			this.MenuStrip.Text = "menuStrip1";
			// 
			// FileMenu
			// 
			this.FileMenu.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenuItem,
            this.FileMenuSeperator1,
            this.ExitMenuItem} );
			this.FileMenu.Name = "FileMenu";
			this.FileMenu.Size = new System.Drawing.Size( 37, 20 );
			this.FileMenu.Text = "File";
			// 
			// OpenMenuItem
			// 
			this.OpenMenuItem.Name = "OpenMenuItem";
			this.OpenMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.OpenMenuItem.Text = "Open";
			this.OpenMenuItem.Click += new System.EventHandler( this.OpenMenuItem_Click );
			// 
			// FileMenuSeperator1
			// 
			this.FileMenuSeperator1.Name = "FileMenuSeperator1";
			this.FileMenuSeperator1.Size = new System.Drawing.Size( 149, 6 );
			// 
			// ExitMenuItem
			// 
			this.ExitMenuItem.Name = "ExitMenuItem";
			this.ExitMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.ExitMenuItem.Text = "Exit";
			// 
			// EditMenu
			// 
			this.EditMenu.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.SelectAllMenuItem,
            this.toolStripSeparator1,
            this.RemoveMenuItem} );
			this.EditMenu.Name = "EditMenu";
			this.EditMenu.Size = new System.Drawing.Size( 39, 20 );
			this.EditMenu.Text = "Edit";
			// 
			// SelectAllMenuItem
			// 
			this.SelectAllMenuItem.Name = "SelectAllMenuItem";
			this.SelectAllMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.SelectAllMenuItem.Text = "Select All";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 149, 6 );
			// 
			// RemoveMenuItem
			// 
			this.RemoveMenuItem.Name = "RemoveMenuItem";
			this.RemoveMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.RemoveMenuItem.Text = "Remove";
			// 
			// AboutMenuItem
			// 
			this.AboutMenuItem.Enabled = false;
			this.AboutMenuItem.Name = "AboutMenuItem";
			this.AboutMenuItem.Size = new System.Drawing.Size( 52, 20 );
			this.AboutMenuItem.Text = "About";
			// 
			// StartButton
			// 
			this.StartButton.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
			this.StartButton.Location = new System.Drawing.Point( 12, 254 );
			this.StartButton.Name = "StartButton";
			this.StartButton.Size = new System.Drawing.Size( 260, 23 );
			this.StartButton.TabIndex = 2;
			this.StartButton.Text = "Start";
			this.StartButton.UseVisualStyleBackColor = true;
			this.StartButton.Click += new System.EventHandler( this.StartButton_Click );
			// 
			// StatusStrip
			// 
			this.StatusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel} );
			this.StatusStrip.Location = new System.Drawing.Point( 0, 280 );
			this.StatusStrip.Name = "StatusStrip";
			this.StatusStrip.Size = new System.Drawing.Size( 284, 22 );
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
			this.FileListView.GridLines = true;
			this.FileListView.Location = new System.Drawing.Point( 0, 27 );
			this.FileListView.Name = "FileListView";
			this.FileListView.Size = new System.Drawing.Size( 284, 221 );
			this.FileListView.TabIndex = 4;
			this.FileListView.UseCompatibleStateImageBehavior = false;
			this.FileListView.View = System.Windows.Forms.View.Details;
			// 
			// FileNameColumn
			// 
			this.FileNameColumn.Text = "File Name";
			this.FileNameColumn.Width = 103;
			// 
			// StatusColumn
			// 
			this.StatusColumn.Text = "Status";
			this.StatusColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.StatusColumn.Width = 170;
			// 
			// BackgroundEditor
			// 
			this.BackgroundEditor.WorkerReportsProgress = true;
			this.BackgroundEditor.WorkerSupportsCancellation = true;
			this.BackgroundEditor.DoWork += new System.ComponentModel.DoWorkEventHandler( this.BackgroundEditor_DoWork );
			this.BackgroundEditor.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.BackgroundEditor_RunWorkerCompleted );
			this.BackgroundEditor.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler( this.BackgroundEditor_ProgressChanged );
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 284, 302 );
			this.Controls.Add( this.FileListView );
			this.Controls.Add( this.StatusStrip );
			this.Controls.Add( this.StartButton );
			this.Controls.Add( this.MenuStrip );
			this.MainMenuStrip = this.MenuStrip;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MainForm";
			this.MenuStrip.ResumeLayout( false );
			this.MenuStrip.PerformLayout();
			this.StatusStrip.ResumeLayout( false );
			this.StatusStrip.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MenuStrip;
		private System.Windows.Forms.ToolStripMenuItem FileMenu;
		private System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
		private System.Windows.Forms.ToolStripSeparator FileMenuSeperator1;
		private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
		private System.Windows.Forms.ToolStripMenuItem EditMenu;
		private System.Windows.Forms.ToolStripMenuItem SelectAllMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem RemoveMenuItem;
		private System.Windows.Forms.Button StartButton;
		private System.Windows.Forms.StatusStrip StatusStrip;
		private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
		private System.Windows.Forms.ListView FileListView;
		private System.Windows.Forms.ColumnHeader FileNameColumn;
		private System.Windows.Forms.ColumnHeader StatusColumn;
		private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
		private System.ComponentModel.BackgroundWorker BackgroundEditor;
	}
}