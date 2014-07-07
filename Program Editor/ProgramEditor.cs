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
	public enum RefreshStatus
	{
		PROCESS = 1,
		UPDATEUI = 2,
	}

	public partial class ProgramEditor : Form
	{
		private List<Record> m_FileList = new List<Record>();
		private int m_nProcessingID;

		private int m_nOccurance = 0;		// how many occurance of the target marker L and M
		private int m_nHeadLine = -1;		// L position
		private int m_nTailLine = -1;		// M position
		private int m_nTotalLine = -1;		// total lines

		public ProgramEditor( )
		{
			InitializeComponent();

			// reset ui
			btnStart.Text = "Start";
			btnStart.Enabled = false;
			StatusLabel.Text = Status.STATUSSTRIP_WAITOPEN.ToString();
		}

		private void OpenMenuItem_Click(object sender, EventArgs e)
		{
			// clear selection list
			m_FileList.Clear();
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
				//Record DummyValue;

				// save file list to m_fileList
				foreach( string ReadOut in openFileDialog.FileNames )
				{
					//DummyValue.Path = ReadOut;
					//DummyValue.StatusFlag = Status.FILE_LOADED;
					//DummyValue.ErrorFlag = Status.NONE;
					// add items to m_FileList
					m_FileList.Add( new Record( ReadOut, Status.FILE_LOADED, Status.NONE ) );

					// refresh ListView
					FileListView.Items.Add( new ListViewItem( new string[]
																{
																	m_FileList[m_FileList.Count - 1].GetFileName(),
																	m_FileList[m_FileList.Count - 1].GetStatusFlag().ToString()
																} ) );
				}

				// file selected, enable start button
				btnStart.Enabled = true;
				SelectAllMenuItem.Enabled = true;
			}
			else
			{
				btnStart.Enabled = false;
				SelectAllMenuItem.Enabled = false;
			}

			// refresh status bar label
			StatusLabel.Text = ( btnStart.Enabled ) ? Status.STATUSSTRIP_WAITSTART.ToString() :
														 Status.STATUSSTRIP_WAITOPEN.ToString();

#if DEBUG
			Debug.WriteLine( "\n==FILE INPUT==" );
			foreach( Record ReadOut in FileList )
			{
				Debug.WriteLine( ReadOut.GetPath() );
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
				btnStart.Enabled = false;
				btnStart.Text = "Stopping...";

				SelectAllMenuItem.Enabled = false;

				Debug.WriteLine( "==TRYING TO STOP BG THREAD==" );
				// notify the background worker that a cancel has been requested
				BackgroundEditor.CancelAsync();
			}
			else
			{
				btnStart.Text = "Cancel";

				// disable open menu item
				OpenMenuItem.Enabled = false;

				Debug.WriteLine( "==INITIATE BG THREAD==" );
				// start the background worker
				BackgroundEditor.RunWorkerAsync();
			}
		}

		private void BackgroundEditor_DoWork(object sender, DoWorkEventArgs e)
		{
			for( m_nProcessingID = 0; m_nProcessingID < m_FileList.Count; m_nProcessingID++ )
			{
				string Path = m_FileList[ m_nProcessingID ].GetPath();

				m_FileList[ m_nProcessingID ].SetStatusFlag( Status.FILE_PROCESSING );
				BackgroundEditor.ReportProgress( (int)RefreshStatus.UPDATEUI );
				// set ui status
				BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_OPEN );

				// search for markers in file
				SearchMarkers( Path );

				// check if marker format is valid
				IfFormatValid();

				// check if all the needed information are there
				if( m_FileList[ m_nProcessingID ].GetErrorFlag() == Status.NONE )
				{
					// moving segments in the file
					MoveSegment( Path );
					m_FileList[ m_nProcessingID ].SetStatusFlag( Status.FILE_PROCESSED );
					// BackgroundEditor.ReportProgress( (int)RefreshStatus.UPDATEUI, Status.FILE_PROCESSED );
				}
				else
				{
					m_FileList[ m_nProcessingID ].SetStatusFlag( Status.FILE_SKIP );
					//BackgroundEditor.ReportProgress( (int)RefreshStatus.UPDATEUI, Status.FILE_SKIP );
				}

				// update ui and reset status strip
				BackgroundEditor.ReportProgress( (int)RefreshStatus.UPDATEUI );
				BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.NONE );
			}
		}

		private void BackgroundEditor_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			// identifying which status to update
			switch( e.ProgressPercentage )
			{
				case (int)RefreshStatus.PROCESS: // updating process
					{
						Debug.WriteLine( "==REFRESH_PROCESS==" );

						StatusLabel.Text = ( (Status)( e.UserState ) ).ToString();
						break;
					}
				case (int)RefreshStatus.UPDATEUI: // updating result, from status flag
					{
						Debug.WriteLine( "==REFRESH_RESULT==" );

						//FileListView.Items[ m_nProcessingID ].SubItems[ 1 ].Text = m_FileList[m_nProcessingID].GetStatusFlagString();

						FileListView.Items.Clear();
						foreach( Record DummyValue in m_FileList )
						{
							FileListView.Items.Add( new ListViewItem( new string[]
																		{
																			DummyValue.GetFileName(),
																			DummyValue.GetStatusFlag().ToString()
																		} ) );

							// change colour if error occurred
							if( DummyValue.GetErrorFlag() != Status.NONE )
							{
								FileListView.Items[ FileListView.Items.Count - 1 ].BackColor = Color.Yellow;
							}
							else
							{
								FileListView.Items[ FileListView.Items.Count - 1 ].BackColor = Color.White;
							}
						}

						break;
					}
			}
		}

		private void BackgroundEditor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// ask if user wants to have a copy of log
#if ENABLE_LOG
			RequestLog();
#endif

			// reset ui
			btnStart.Enabled = true;
			btnStart.Text = "Start";
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
			m_nOccurance = 0;
			m_nHeadLine = -1;
			m_nTailLine = -1;
			m_nTotalLine = -1;

			// incdicator for current position in file
			int nLineCounter = 0;
			string Line;

			using( StreamReader Reader = new StreamReader( Path ) )
			{
				// start finding marker until eof
				while( ( Line = Reader.ReadLine() ) != null )
				{
					Debug.WriteLine( nLineCounter.ToString() + " : " + Line );

					BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_SEARCHHEAD );
					// check start marker
					if( ContainMarker( Line, "*L*00*" ) )
					{
						// check if L marker is in range
						if( IfFormatValid( Line ) )
						{
							Debug.WriteLine( "==FOUND START MARKER @ " + nLineCounter.ToString() );
							m_nHeadLine = nLineCounter;
							m_nOccurance++;
						}
					}

					BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_SEARCHTAIL );
					// check end marker
					if( ContainMarker( Line, "*M30*" ) )
					{
						Debug.WriteLine( "==FOUND END MARKER @ " + nLineCounter.ToString() );
						m_nTailLine = nLineCounter;
						m_nOccurance++;
					}

					// move indicator to next line
					nLineCounter++;
				}
			}

			// store the total line count
			m_nTotalLine = nLineCounter;
		}

		// extract and modified from SysExpand.Text library
		private bool ContainMarker(string input, string pattern)
		{
			char wildcard = '*';

			// stack containing input positions that should be tested for further matching
			int[] nInputPosStack = new int[ ( input.Length + 1 ) * ( pattern.Length + 1 ) ];
			// stack containing pattern positions that should be tested for further matching
			int[] nPatternPosStack = new int[ nInputPosStack.Length ];

			// points to last occupied entry in stack
			int nStackPos = -1;

			// indicates that input position vs. pattern position has been tested                             
			bool[ , ] bPointTested = new bool[ input.Length + 1, pattern.Length + 1 ];

			// position in input bMatched up to the first multiple wildcard in pattern
			int nInputPos = 0;
			// position in pattern bMatched up to the first multiple wildcard in pattern
			int nPatternPos = 0;

			// match beginning of the string until first multiple wildcard in pattern
			while( nInputPos < input.Length && nPatternPos < pattern.Length &&
				   pattern[ nPatternPos ] != wildcard &&
				   ( input[ nInputPos ] == pattern[ nPatternPos ] || pattern[ nPatternPos ] == wildcard ) )
			{
				nInputPos++;
				nPatternPos++;
			}

			// push this position to stack if it points to end of pattern or to a general wildcard character
			if( nPatternPos == pattern.Length || pattern[ nPatternPos ] == wildcard )
			{
				bPointTested[ nInputPos, nPatternPos ] = true;
				nInputPosStack[ ++nStackPos ] = nInputPos;
				nPatternPosStack[ nStackPos ] = nPatternPos;
			}

			bool bMatched = false;

			// repeat matching until either string is bMatched against the pattern or no more parts remain on stack to test
			while( nStackPos >= 0 && !bMatched )
			{

				nInputPos = nInputPosStack[ nStackPos ];         // Pop input and pattern positions from stack
				nPatternPos = nPatternPosStack[ nStackPos-- ];   // Matching will succeed if rest of the input string matches rest of the pattern

				if( nInputPos == input.Length && nPatternPos == pattern.Length )
					bMatched = true;     // Reached end of both pattern and input string, hence matching is successful
				else if( nPatternPos == pattern.Length - 1 )
					bMatched = true;     // Current pattern character is multiple wildcard and it will match all the remaining characters in the input string
				else
				{
					// First character in next pattern block is guaranteed to be multiple wildcard
					// So skip it and search for all matches in input string until next multiple wildcard character is reached in pattern

					for( int curInputStart = nInputPos; curInputStart < input.Length; curInputStart++ )
					{

						int curInputPos = curInputStart;
						int curPatternPos = nPatternPos + 1;

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
							!bPointTested[ curInputPos, curPatternPos ] )
						{
							bPointTested[ curInputPos, curPatternPos ] = true;
							nInputPosStack[ ++nStackPos ] = curInputPos;
							nPatternPosStack[ nStackPos ] = curPatternPos;
						}
					}
				}
			}
			return bMatched;
		}

		private void IfFormatValid( )
		{
			if( m_nOccurance > 2 )
			{
#if DEBUG
				MessageBox.Show( "Multiple markers in file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
#endif
				m_FileList[ m_nProcessingID ].SetErrorFlag( Status.MARKER_MULTI );
			}
			else
			{
				if( m_nOccurance < 2 )
				{
					if( m_nHeadLine == -1 )
					{
#if DEBUG
						MessageBox.Show( "Missing head marker: LN00", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
#endif
						m_FileList[ m_nProcessingID ].SetErrorFlag( Status.MARKER_MIS_HEAD );
					}
					if( m_nTailLine == -1 )
					{
#if DEBUG
						MessageBox.Show( "Missing end marker: M30", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
#endif
						m_FileList[ m_nProcessingID ].SetErrorFlag( Status.MARKER_MIS_TAIL );
					}
					if( m_nHeadLine > m_nTailLine )
					{
#if DEBUG
						MessageBox.Show( "Marker in reverse order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
#endif
						m_FileList[ m_nProcessingID ].SetErrorFlag( Status.MARKER_REVERSE );
					}
				}
			}
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
			m_FileList[ m_nProcessingID ].SetErrorFlag( Status.MARKER_OB );

			return false;
		}

		private void MoveSegment(string Path)
		{
			Debug.WriteLine( "==PRINT SELECTED LINES : " + m_nHeadLine.ToString() + " : " + m_nTailLine.ToString() );

			// report current progress
			BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_MOVING );

			// position to pickup after the moving
			int nLineCounter = 0;
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
						if( ( nLineCounter < m_nHeadLine ) || ( nLineCounter > m_nTailLine ) )
						{
							// write content to file
							Writer.WriteLine( Line );
							Writer.Flush();
						}

						nLineCounter++;
					}

					// reset reader and counter
					nLineCounter = 0;
					Reader.DiscardBufferedData();
					Reader.BaseStream.Seek( 0, SeekOrigin.Begin );

					Debug.WriteLine( "==WRITING QUOTED REGION==" );
					while( ( Line = Reader.ReadLine() ) != null )
					{
						if( ( nLineCounter >= m_nHeadLine ) && ( nLineCounter <= m_nTailLine ) )
						{
							// write content to file
							Writer.WriteLine( Line );
							Writer.Flush();
						}

						nLineCounter++;
					}
				}
			}

			// save file by overwriting current file using temporary file
			BackgroundEditor.ReportProgress( (int)RefreshStatus.PROCESS, Status.EDITOR_SAVE );
			File.Delete( Path );
			File.Move( TempFile, Path );
		}

		private void RequestLog( )
		{
			// throw new Exception( "The method or operation is not implemented." );
			Debug.WriteLine( "==INSIDE REQUEST LOG FUNCTION==" );

			List<Record> ErrorPreview = new List<Record>();

			foreach( Record DummyValue in m_FileList )
			{
				if( DummyValue.GetErrorFlag() != Status.NONE )
				{
					ErrorPreview.Add( DummyValue );
				}
			}

			if( ErrorPreview.Count > 0 )
			{
				if( MessageBox.Show( "Some file isn't processed, would you like to know them?",
									 "Log Output", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
				{
					string LogPath = "C:\\log-" +
								  DateTime.Now.ToString( "yyyy-MM-dd-HH-mm-ss" ) +
								  ".txt";
				
					using( StreamWriter Writer = new StreamWriter( LogPath ) )
					{
						foreach( Record DummyValue in ErrorPreview )
						{
							Writer.WriteLine( DummyValue.GetPath() + " :: " + DummyValue.GetErrorFlag().ToString() );
						}
					}

					MessageBox.Show( "Log is generated at " + LogPath, "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information );
				}
			}
			else
			{
				MessageBox.Show( "Process complete!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information );
			}
		}

		private void SelectAllMenuItem_Click(object sender, EventArgs e)
		{
			if( FileListView.Items.Count == 0 )
			{
				MessageBox.Show( "Please open some files first!", "No Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			}

			foreach( ListViewItem DummyValue in FileListView.Items )
			{
				DummyValue.Selected = true;
			}
		}

		private void RemoveMenuItem_Click(object sender, EventArgs e)
		{
			if( FileListView.SelectedItems.Count > 0 )
			{
				if( MessageBox.Show( "Would you like to delete selected items?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
				{
					for( int i = FileListView.SelectedItems.Count - 1; i >= 0; i-- )
					{
						ListViewItem DummyValue = FileListView.SelectedItems[ i ];
						m_FileList.RemoveAt( DummyValue.Index );
						FileListView.Items[ DummyValue.Index ].Remove();
					}
				}
			}
			else
			{
				MessageBox.Show( "Please select at least on items.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
			}
		}

		private void ExitMenuItem_Click(object sender, EventArgs e)
		{
			if( MessageBox.Show( "Are you sure?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
			{
				this.Close();
			}
		}
	}

	public sealed class Record
	{
		private string Path;
		private Status StatusFlag;
		private Status ErrorFlag;

		public Record(string Path, Status StatusFlag, Status ErrorFlag)
		{
			this.Path = Path;
			this.StatusFlag = StatusFlag;
			this.ErrorFlag = ErrorFlag;
		}

		public string GetPath( )
		{
			return Path;
		}

		public string GetFileName( )
		{
			return System.IO.Path.GetFileName( Path );
		}

		public Status GetStatusFlag( )
		{
			return StatusFlag;
		}

		public Status GetErrorFlag( )
		{
			return ErrorFlag;
		}

		public void SetStatusFlag(Status StatusFlag)
		{
			this.StatusFlag = StatusFlag;
		}

		public void SetErrorFlag(Status ErrorFlag)
		{
			this.ErrorFlag = ErrorFlag;
		}
	}

	public sealed class Status
	{
		private readonly string name;
		private readonly int nValue;

		public static readonly Status NONE = new Status( 0, "" );

		public static readonly Status MARKER_MULTI = new Status( 1, "Multiple markers in file." );
		public static readonly Status MARKER_MIS_HEAD = new Status( 2, "Missing head marker LN00." );
		public static readonly Status MARKER_MIS_TAIL = new Status( 3, "Missing tail marker M30." );
		public static readonly Status MARKER_OB = new Status( 4, "Head maker LN00, N shall falls in the range 1~89." );
		public static readonly Status MARKER_REVERSE = new Status( 5, "Markers in reverse order." );

		public static readonly Status FILE_PROCESSING = new Status( 10, "..." );
		public static readonly Status FILE_PROCESSED = new Status( 11, "Modified" );
		public static readonly Status FILE_SKIP = new Status( 12, "No need to modify" );
		public static readonly Status FILE_LOADED = new Status( 13, "" );

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
			this.nValue = value;
		}

		public override string ToString( )
		{
			return name;
		}
	}
}