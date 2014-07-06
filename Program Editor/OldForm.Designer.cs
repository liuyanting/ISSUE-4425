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
			this.kitchenProgressbar = new System.Windows.Forms.ProgressBar();
			this.kitchenProgressLabel = new System.Windows.Forms.Label();
			this.fileProgressLabel = new System.Windows.Forms.Label();
			this.fileProgressbar = new System.Windows.Forms.ProgressBar();
			this.openButton = new System.Windows.Forms.Button();
			this.startButton = new System.Windows.Forms.Button();
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.SuspendLayout();
			// 
			// kitchenProgressbar
			// 
			this.kitchenProgressbar.Location = new System.Drawing.Point( 28, 40 );
			this.kitchenProgressbar.Name = "kitchenProgressbar";
			this.kitchenProgressbar.Size = new System.Drawing.Size( 226, 23 );
			this.kitchenProgressbar.Step = 4;
			this.kitchenProgressbar.TabIndex = 0;
			// 
			// kitchenProgressLabel
			// 
			this.kitchenProgressLabel.AutoSize = true;
			this.kitchenProgressLabel.Location = new System.Drawing.Point( 56, 24 );
			this.kitchenProgressLabel.Name = "kitchenProgressLabel";
			this.kitchenProgressLabel.Size = new System.Drawing.Size( 150, 13 );
			this.kitchenProgressLabel.TabIndex = 1;
			this.kitchenProgressLabel.Text = "Searching for L mark... Found!";
			this.kitchenProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// fileProgressLabel
			// 
			this.fileProgressLabel.AutoSize = true;
			this.fileProgressLabel.Location = new System.Drawing.Point( 28, 83 );
			this.fileProgressLabel.Name = "fileProgressLabel";
			this.fileProgressLabel.Size = new System.Drawing.Size( 104, 13 );
			this.fileProgressLabel.TabIndex = 2;
			this.fileProgressLabel.Text = "Processing BLAH.txt";
			// 
			// fileProgressbar
			// 
			this.fileProgressbar.Location = new System.Drawing.Point( 28, 99 );
			this.fileProgressbar.Name = "fileProgressbar";
			this.fileProgressbar.Size = new System.Drawing.Size( 226, 23 );
			this.fileProgressbar.Step = 4;
			this.fileProgressbar.TabIndex = 3;
			// 
			// openButton
			// 
			this.openButton.Location = new System.Drawing.Point( 57, 144 );
			this.openButton.Name = "openButton";
			this.openButton.Size = new System.Drawing.Size( 75, 23 );
			this.openButton.TabIndex = 4;
			this.openButton.Text = "Open";
			this.openButton.UseVisualStyleBackColor = true;
			this.openButton.Click += new System.EventHandler( this.openButton_Click );
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point( 138, 144 );
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size( 75, 23 );
			this.startButton.TabIndex = 5;
			this.startButton.Text = "Start";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler( this.startButton_Click );
			// 
			// worker
			// 
			this.worker.WorkerReportsProgress = true;
			this.worker.WorkerSupportsCancellation = true;
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler( this.worker_DoWork );
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.worker_RunWorkerCompleted );
			this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler( this.worker_ProgressChanged );
			// 
			// ProgramEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 285, 201 );
			this.Controls.Add( this.startButton );
			this.Controls.Add( this.openButton );
			this.Controls.Add( this.fileProgressbar );
			this.Controls.Add( this.fileProgressLabel );
			this.Controls.Add( this.kitchenProgressLabel );
			this.Controls.Add( this.kitchenProgressbar );
			this.Name = "ProgramEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Program Editor";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar kitchenProgressbar;
		private System.Windows.Forms.Label kitchenProgressLabel;
		private System.Windows.Forms.Label fileProgressLabel;
		private System.Windows.Forms.ProgressBar fileProgressbar;
		private System.Windows.Forms.Button openButton;
		private System.Windows.Forms.Button startButton;
		private System.ComponentModel.BackgroundWorker worker;
	}
}

