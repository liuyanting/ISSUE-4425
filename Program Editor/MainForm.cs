using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Program_Editor
{
	public struct Record
	{
		public string Path;
		public Status StatusFlag;
		public Status ErrorFlag;
	}

	public enum RefreshStatus
	{
		PROCESS = 1,
		RESULT = 2
	}
	
	public partial class MainForm : Form
	{
		private List<Record> FileList = new List<Record>();
		private int ProcessingID;

		private int Occurance = 0;		// how many occurance of the target marker L and M
		private int HeadLine = -1;		// L position
		private int TailLine = -1;		// M position
		private int TotalLine = -1;		// total lines
		
		public MainForm( )
		{
			InitializeComponent();
			
			// reset ui
			StartButton.Text = "Start";
			StartButton.Enabled = false;
			StatusLabel.Text = Status.STATUSSTRIP_WAITOPEN.ToString();
		}

		private void OpenMenuItem_Click(object sender, EventArgs e)
		{
			// clear selection list
			FileList.Clear();
			// clear ListView
			FileListView.Items.Clear();

			// displays an OpenFileDialog for user to select files
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "All Files|*.*";		// set filter
			openFileDialog.Title = "Select the File(s)";
			openFileDialog.Multiselect = true;				// enable multiselect

			// show the dialog
			if( openFileDialog.ShowDialog() == DialogResult.OK )
			{
				Record DummyValue;
				
				// save file list to m_fileList
				foreach( string ReadOut in openFileDialog.FileNames )
				{
					DummyValue.Path = ReadOut;
					DummyValue.StatusFlag = Status.FILE_LOADED;
					DummyValue.ErrorFlag = Status.NONE;
					// add items to FileList
					FileList.Add( DummyValue );

					// refresh ListView
					FileListView.Items.Add( Path.GetFileName( ReadOut ) );
				}

				// file selected, enable start button
				StartButton.Enabled = true;
			}
			else
			{
				StartButton.Enabled = false;
			}

			// refresh status bar label
			StatusLabel.Text = ( StartButton.Enabled ) ? Status.STATUSSTRIP_WAITSTART.ToString() : 
														 Status.STATUSSTRIP_WAITOPEN.ToString();

#if DEBUG
			Debug.WriteLine( "\n==FILE INPUT==" );
			foreach( Record ReadOut in FileList )
			{
				Debug.WriteLine( ReadOut.Path );
			}
			Debug.WriteLine( "==============\n" );
#endif
		}

		private void StartButton_Click(object sender, EventArgs e)
		{
			// if background worker is running, then click this button cause cancelling
			// otherwise, launch the thread
			if( BackgroundEditor.IsBusy )
			{
				StartButton.Enabled = false;
				StartButton.Text = "Stopping...";

				Debug.WriteLine( "==TRYING TO STOP BG THREAD==" );
				// notify the background worker that a cancel has been requested
				BackgroundEditor.CancelAsync();
			}
			else
			{
				StartButton.Text = "Cancel";

				// disable open menu item
				OpenMenuItem.Enabled = false;

				Debug.WriteLine( "==INITIATE BG THREAD==" );
				// start the background worker
				BackgroundEditor.RunWorkerAsync();
			}
		}

		private void BackgroundEditor_DoWork(object sender, DoWorkEventArgs e)
		{
			for( ProcessingID = 0; ProcessingID < FileList.Count; ProcessingID++ )
			{
				string Path = FileList[ ProcessingID ].Path;

				BackgroundEditor.ReportProgress( (int)RefreshStatus.RESULT, Status.FILE_PROCESSING );

				BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_OPEN );

				// search for markers in file
				SearchMarkers( Path );

				// check if marker format is valid
				IfFormatValid();

				// check if all the needed information are there
				if( FileList[ProcessingID].ErrorFlag == Status.NONE )
				{
					// moving segments in the file
					MoveSegment( Path );
				}
			}
		}

		private void BackgroundEditor_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			// identifying which status to update
			//switch( e.UserState.GetType().ToString() as string )
			switch( e.ProgressPercentage )
			{
				case (int)RefreshStatus.PROCESS: // updating process
					{
						StatusLabel.Text = ( (Status)( e.UserState ) ).ToString();
						break;
					}
				case (int)RefreshStatus.RESULT: // updating result, from status flag
					{
						FileListView.Items[ ProcessingID ].SubItems[ 1 ].Text = ( (Status)( e.UserState ) ).ToString();

						// change colour if error occurred
						if( FileList[ ProcessingID ].ErrorFlag != Status.NONE )
						{
							FileListView.Items[ ProcessingID ].BackColor = Color.Red;
						}
						else
						{
							FileListView.Items[ ProcessingID ].BackColor = Color.White;
						}

						break;
					}
			}
		}

		private void BackgroundEditor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// ask if user wants to have a copy of log
			RequestLog();
		}

		private void SearchMarkers(string Path)
		{
			// reset markers
			Occurance = 0;
			HeadLine = -1;
			TailLine = -1;
			TotalLine = -1;

			// incdicator for current position in file
			int LineCounter = 0;
			string Line;

			using( StreamReader Reader = new StreamReader( Path ) )
			{
				// start finding marker until eof
				while( ( Line = Reader.ReadLine() ) != null )
				{
					Debug.WriteLine( LineCounter.ToString() + " : " + Line );

					BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_SEARCHHEAD );
					// check start marker
					if( ContainMarker( Line, "*L*00*" ) )
					{
						if( IfFormatValid( Line ) )
						{
							Debug.WriteLine( "==FOUND START MARKER @ " + LineCounter.ToString() );
							HeadLine = LineCounter;
							Occurance++;
						}
					}

					BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_SEARCHTAIL );
					// check end marker
					if( ContainMarker( Line, "*M30*" ) )
					{
						Debug.WriteLine( "==FOUND END MARKER @ " + LineCounter.ToString() );
						TailLine = LineCounter;
						Occurance++;
					}

					// move indicator to next line
					LineCounter++;
				}
			}

			// store the total line count
			TotalLine = LineCounter;
		}

		private Status IfFormatValid( )
		{
			Record DummyValue;
			DummyValue = FileList[ ProcessingID ];


			
			return Status.NONE;
		}

		private void MoveSegment(string Path)
		{
		}

		private void RequestLog()
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		
	}

	public sealed class Status
	{
		private readonly string name;
		private readonly int value;

		public static readonly Status NONE = new Status( 0, "" );

		public static readonly Status MARKER_MULTI = new Status( 1, "Multiple markers in file." );
		public static readonly Status MARKER_MIS_HEAD = new Status( 2, "Missing head marker LN00." );
		public static readonly Status MARKER_MIS_TAIL = new Status( 3, "Missing tail marker M30." );
		public static readonly Status MARKER_OB = new Status( 4, "Head maker LN00, N shall falls in the range 1~89." );
		public static readonly Status MARKER_REVERSE = new Status( 5, "Markers in reverse order." );

		public static readonly Status FILE_PROCESSING = new Status( 10, "..." );
		public static readonly Status FILE_PROCESSED = new Status( 11, "Processed" );
		public static readonly Status FILE_SKIP = new Status( 12, "Skipped" );
		public static readonly Status FILE_LOADED = new Status( 13, "" );

		public static readonly Status EDITOR_OPEN = new Status( 30, "Opening file." );
		public static readonly Status EDITOR_SEARCHHEAD = new Status( 31, "Searching head marker LN00..." );
		public static readonly Status EDITOR_SEARCHTAIL = new Status( 32, "Searching tail marker M30..." );
		public static readonly Status EDITOR_MOVING = new Status( 33, "Moving quoted segments." );
		public static readonly Status EDITOR_SAVE = new Status( 34, "Saving file." );

		public static readonly Status STATUSSTRIP_WAITOPEN = new Status( 20, "Select files from: File > Open." );
		public static readonly Status STATUSSTRIP_WAITSTART = new Status( 21, "Click start to begin." );

		private Status(int value, String name)
		{
			this.name = name;
			this.value = value;
		}

		public override string ToString( )
		{
			return name;
		}
	}
}