namespace Program_Editor
{
	partial class HelpForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.btnOK = new System.Windows.Forms.Button();
			( (System.ComponentModel.ISupportInitialize)( this.pictureBox1 ) ).BeginInit();
			( (System.ComponentModel.ISupportInitialize)( this.pictureBox2 ) ).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 19, 25 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 176, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "1. Click                (Open) to load file.";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 19, 78 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 296, 13 );
			this.label2.TabIndex = 1;
			this.label2.Text = "2. Click                (Start Conversion) to begin the conversions.";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Program_Editor.Properties.Resources.open;
			this.pictureBox1.Location = new System.Drawing.Point( 65, 16 );
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size( 35, 36 );
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::Program_Editor.Properties.Resources.start;
			this.pictureBox2.Location = new System.Drawing.Point( 65, 67 );
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size( 35, 36 );
			this.pictureBox2.TabIndex = 3;
			this.pictureBox2.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point( 132, 125 );
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size( 75, 23 );
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
			// 
			// HelpForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 338, 165 );
			this.ControlBox = false;
			this.Controls.Add( this.btnOK );
			this.Controls.Add( this.pictureBox2 );
			this.Controls.Add( this.pictureBox1 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HelpForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Help";
			( (System.ComponentModel.ISupportInitialize)( this.pictureBox1 ) ).EndInit();
			( (System.ComponentModel.ISupportInitialize)( this.pictureBox2 ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Button btnOK;
	}
}