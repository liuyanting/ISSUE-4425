using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Program_Editor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( )
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			// Application.Run( new ProgramEditor() );
			// Application.Run( new MainForm() );

			// initiate form
			MainForm Form = new MainForm();
			// lock form size
			Form.FormBorderStyle = FormBorderStyle.FixedSingle;
			Form.MaximizeBox = false;
			Form.SizeGripStyle = SizeGripStyle.Hide;

			Application.Run( Form );
		}
	}
}