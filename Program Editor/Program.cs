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

			// initiate form
			MainForm MyForm = new MainForm();
			// lock form size
			MyForm.FormBorderStyle = FormBorderStyle.FixedSingle;
			MyForm.MaximizeBox = false;
			MyForm.SizeGripStyle = SizeGripStyle.Hide;

			Application.Run( MyForm );
		}
	}
}