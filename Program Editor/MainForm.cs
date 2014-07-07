using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

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
					FileListView.Items.Add( new ListViewItem( new string[]
																{
																	Path.GetFileName( DummyValue.Path ),
																	DummyValue.StatusFlag.ToString()
																} ));
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

				// set ui status
				BackgroundEditor.ReportProgress( (int)RefreshStatus.RESULT, Status.FILE_PROCESSING );
				BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_OPEN );

				// search for markers in file
				SearchMarkers( Path );

				// check if marker format is valid
				IfFormatValid();

				// check if all the needed information are there
				if( FileList[ ProcessingID ].ErrorFlag == Status.NONE )
				{
					// moving segments in the file
					MoveSegment( Path );
					BackgroundEditor.ReportProgress( (int)RefreshStatus.RESULT, Status.FILE_PROCESSED );
				}
				else
				{
					BackgroundEditor.ReportProgress( (int)RefreshStatus.RESULT, Status.FILE_SKIP );
				}

				// reset status strip
				BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.NONE );
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
						Debug.WriteLine( "==REFRESH_PROCESS==" );

						StatusLabel.Text = ( (Status)( e.UserState ) ).ToString();
						break;
					}
				case (int)RefreshStatus.RESULT: // updating result, from status flag
					{
						Debug.WriteLine( "==REFRESH_RESULT==" );

						ListViewItem DummyLVT = new ListViewItem( Path.GetFileName( FileList[ ProcessingID ].Path ) );
						DummyLVT.SubItems.Add( "WRITED" );
						FileListView.Items[ ProcessingID ] = DummyLVT;

						// the minus 1 is cause by the end of array
						//FileListView.Items[ ProcessingID - 1 ].SubItems[ 0 ].Text = "WRITED";

						//// change colour if error occurred
						//if( FileList[ ProcessingID ].ErrorFlag != Status.NONE )
						//{
						//    FileListView.Items[ ProcessingID ].BackColor = Color.Red;
						//}
						//else
						//{
						//    FileListView.Items[ ProcessingID ].BackColor = Color.White;
						//}

						break;
					}
			}
		}

		private void BackgroundEditor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// ask if user wants to have a copy of log
			RequestLog();

			// reset ui
			StartButton.Enabled = true;
			StartButton.Text = "Start";
			OpenMenuItem.Enabled = true;

			// check to see if error exists
			if( e.Error != null )
			{
				MessageBox.Show( e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			// check to see if the worker is cancelled
			if( e.Cancelled )
			{
				StatusLabel.Text = Status.STATUSSTRIP_CANCELLED.ToString();
			}
			else
			{
				// everything completed normally
				StatusLabel.Text = Status.STATUSSTRIP_COMPLETE.ToString();
			}
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
						// check if L marker is in range
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

		// extract and modified from SysExpand.Text library
		private bool ContainMarker(string input, string pattern)
		{
			char wildcard = '*';

			// stack containing input positions that should be tested for further matching
			int[] inputPosStack = new int[ ( input.Length + 1 ) * ( pattern.Length + 1 ) ];
			// stack containing pattern positions that should be tested for further matching
			int[] patternPosStack = new int[ inputPosStack.Length ];

			// points to last occupied entry in stack
			int stackPos = -1;

			// indicates that input position vs. pattern position has been tested                             
			bool[ , ] pointTested = new bool[ input.Length + 1, pattern.Length + 1 ];

			// position in input matched up to the first multiple wildcard in pattern
			int inputPos = 0;
			// position in pattern matched up to the first multiple wildcard in pattern
			int patternPos = 0;

			// match beginning of the string until first multiple wildcard in pattern
			while( inputPos < input.Length && patternPos < pattern.Length &&
				   pattern[ patternPos ] != wildcard &&
				   ( input[ inputPos ] == pattern[ patternPos ] || pattern[ patternPos ] == wildcard ) )
			{
				inputPos++;
				patternPos++;
			}

			// push this position to stack if it points to end of pattern or to a general wildcard character
			if( patternPos == pattern.Length || pattern[ patternPos ] == wildcard )
			{
				pointTested[ inputPos, patternPos ] = true;
				inputPosStack[ ++stackPos ] = inputPos;
				patternPosStack[ stackPos ] = patternPos;
			}

			bool matched = false;

			// repeat matching until either string is matched against the pattern or no more parts remain on stack to test
			while( stackPos >= 0 && !matched )
			{

				inputPos = inputPosStack[ stackPos ];         // Pop input and pattern positions from stack
				patternPos = patternPosStack[ stackPos-- ];   // Matching will succeed if rest of the input string matches rest of the pattern

				if( inputPos == input.Length && patternPos == pattern.Length )
					matched = true;     // Reached end of both pattern and input string, hence matching is successful
				else if( patternPos == pattern.Length - 1 )
					matched = true;     // Current pattern character is multiple wildcard and it will match all the remaining characters in the input string
				else
				{
					// First character in next pattern block is guaranteed to be multiple wildcard
					// So skip it and search for all matches in input string until next multiple wildcard character is reached in pattern

					for( int curInputStart = inputPos; curInputStart < input.Length; curInputStart++ )
					{

						int curInputPos = curInputStart;
						int curPatternPos = patternPos + 1;

						while( curInputPos < input.Length && curPatternPos < pattern.Length &&
							   pattern[ curPatternPos ] != wildcard &&
							   ( input[ curInputPos ] == pattern[ curPatternPos ] || pattern[ curPatternPos ] == wildcard ) )
						{
							curInputPos++;
							curPatternPos++;
						}

						// If we have reached next multiple wildcard character in pattern without breaking the matching sequence,
						// then we have another candidate for full match.
						// This candidate should be pushed to stack for further processing.
						// At the same time, pair (input position, pattern position) will be marked as tested,
						// so that it will not be pushed to stack later again.
						if( ( ( curPatternPos == pattern.Length && curInputPos == input.Length ) ||
							 ( curPatternPos < pattern.Length && pattern[ curPatternPos ] == wildcard ) ) &&
							!pointTested[ curInputPos, curPatternPos ] )
						{
							pointTested[ curInputPos, curPatternPos ] = true;
							inputPosStack[ ++stackPos ] = curInputPos;
							patternPosStack[ stackPos ] = curPatternPos;
						}
					}
				}
			}
			return matched;
		}

		private void IfFormatValid( )
		{
			Record DummyValue;
			// pull out the processing item from FileList
			DummyValue = FileList[ ProcessingID ];

			if( Occurance > 2 )
			{
#if DEBUG
				MessageBox.Show( "Multiple markers in file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
#endif
				DummyValue.ErrorFlag = Status.MARKER_MULTI;
			}
			else
			{
				if( Occurance < 2 )
				{
					if( HeadLine == -1 )
					{
#if DEBUG
						MessageBox.Show( "Missing head marker: LN00", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
#endif
						DummyValue.ErrorFlag = Status.MARKER_MIS_HEAD;
					}
					if( TailLine == -1 )
					{
#if DEBUG
						MessageBox.Show( "Missing end marker: M30", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
#endif
						DummyValue.ErrorFlag = Status.MARKER_MIS_TAIL;
					}
					if( HeadLine > TailLine )
					{
#if DEBUG
						MessageBox.Show("Marker in reverse order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
#endif
						DummyValue.ErrorFlag = Status.MARKER_REVERSE;
					}
				}
			}
			
			// write dummy value back to FileList
			FileList[ ProcessingID ] = DummyValue;
		}

		// check if L marker in range
		private bool IfFormatValid(string rawString)
		{
			int nN;
			// parsing number from LN00 marker
			Regex numberParser = new Regex( @"L\S+\d{2}?", RegexOptions.Compiled );
			foreach( Match match in numberParser.Matches( rawString ) )
			{
				if( int.TryParse( match.Value.Remove( 0, 1 ).Replace( "00", string.Empty ), out nN ) )
				{
					// check if N is in range
					if( nN >= 1 && nN <= 89 )
						return true;
				}
			}

			// write error flag
			Record DummyValue = FileList[ ProcessingID ];
			DummyValue.ErrorFlag = Status.MARKER_OB;
			FileList[ ProcessingID ] = DummyValue;

			return false;
		}

		private void MoveSegment(string Path)
		{
			Debug.WriteLine( "==PRINT SELECTED LINES : " + HeadLine.ToString() + " : " + TailLine.ToString() );

			// report current progress
			BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_MOVING );

			// position to pickup after the moving
			int LineCounter = 0;
			string Line;

			// creating tmp file
			string TempFile = System.IO.Path.GetTempFileName();

			using( StreamReader Reader = new StreamReader( Path ) )
			{
				using( StreamWriter Writer = new StreamWriter( TempFile ) )
				{
					// wrtie none moving region
					while( ( Line = Reader.ReadLine() ) != null )
					{
						if( ( LineCounter < HeadLine ) || ( LineCounter > TailLine ) )
						{
							// write content to file
							Writer.WriteLine( Line );
							Writer.Flush();
						}

						LineCounter++;
					}

					// reset reader and counter
					LineCounter = 0;
					Reader.DiscardBufferedData();
					Reader.BaseStream.Seek( 0, SeekOrigin.Begin );

					Debug.WriteLine( "==WRITING QUOTED REGION==" );
					while( ( Line = Reader.ReadLine() ) != null )
					{
						if( ( LineCounter >= HeadLine ) && ( LineCounter <= TailLine ) )
						{
							// write content to file
							Writer.WriteLine( Line );
							Writer.Flush();
						}

						LineCounter++;
					}
				}
			}

			// save file by overwriting current file using temporary file
			BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_SAVE );
			File.Delete( Path );
			File.Move( TempFile, Path );
		}

		private void RequestLog()
		{
			// throw new Exception( "The method or operation is not implemented." );
			Debug.WriteLine( "==INSIDE REQUEST LOG FUNCTION==" );
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
		public static readonly Status FILE_LOADED = new Status( 13, "FILE LOADED" );

		public static readonly Status EDITOR_OPEN = new Status( 30, "Opening file." );
		public static readonly Status EDITOR_SEARCHHEAD = new Status( 31, "Searching head marker LN00..." );
		public static readonly Status EDITOR_SEARCHTAIL = new Status( 32, "Searching tail marker M30..." );
		public static readonly Status EDITOR_MOVING = new Status( 33, "Moving quoted segments." );
		public static readonly Status EDITOR_SAVE = new Status( 34, "Saving file." );

		public static readonly Status STATUSSTRIP_WAITOPEN = new Status( 20, "Select files from: File > Open." );
		public static readonly Status STATUSSTRIP_WAITSTART = new Status( 21, "Click start to begin." );
		public static readonly Status STATUSSTRIP_CANCELLED = new Status( 22, "Cancelled..." );
		public static readonly Status STATUSSTRIP_COMPLETE = new Status( 23, "Completed!" );

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