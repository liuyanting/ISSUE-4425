using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Program_Editor
{
	public partial class ProgramEditor : Form
	{
		private bool m_bFileSelected = false;
		private List<string> m_fileList = new List<string>();
		private List<string> m_errorLog = new List<string>();

		private int m_nOccurance = 0;	// how many occurance of the target marker L and M
		private int m_nStartLine = -1;	// L position
		private int m_nEndLine = -1;		// M position
		private int m_nTotalLine = -1;	// total lines

		public ProgramEditor( )
		{
			InitializeComponent();

			startButton.Enabled = false;
			fileProgressLabel.Text = "Nothing selected...";
			kitchenProgressLabel.Text = "";
		}

		public bool ContainMarker(string input, string pattern)
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
		// extract and modified from SysExpand.Text library

		private void SearchMarkers(string path)
		{
			// reset markers
			m_nOccurance = 0;
			m_nStartLine = -1;
			m_nEndLine = -1;
			m_nTotalLine = -1;
			
			// incdicator for current position in file
			int mLineCounter = 0;
			string line;

			using( StreamReader reader = new StreamReader( path ) )
			{
				// start finding marker until eof
				while( ( line = reader.ReadLine() ) != null )
				{
					Debug.WriteLine( mLineCounter.ToString() + " : " + line );

					worker.ReportProgress( 2, "kitchen" );
					// check start marker
					if( ContainMarker( line, "*L*00*" ) )
					{
						if( IfFormatValid( line ) )
						{
							Debug.WriteLine( "==FOUND START MARKER @ " + mLineCounter.ToString() );
							m_nStartLine = mLineCounter;
							m_nOccurance++;
						}
					}

					worker.ReportProgress( 3, "kitchen" );
					// check end marker
					if( ContainMarker( line, "*M30*" ) )
					{
						Debug.WriteLine( "==FOUND END MARKER @ " + mLineCounter.ToString() );
						m_nEndLine = mLineCounter;
						m_nOccurance++;
					}

					// move indicator to next line
					mLineCounter++;
				}
			}

			// store the total line count
			m_nTotalLine = mLineCounter;
		}

		// check marker existance and position
		private bool IfFormatValid( )
		{
			if( m_nOccurance > 2 )
			{
				//MessageBox.Show( "Multiple markers in file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				worker.ReportProgress( 6, "kitchen" );
				m_errorLog.Add( "Multiple markers in file!" );
				return false;
			}
			else
			{
				if( m_nOccurance < 2 )
				{
					if( m_nStartLine == -1 )
					{
						//MessageBox.Show( "Missing start marker: LN00, 1 <= N <= 89", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
						worker.ReportProgress( 6, "kitchen" );
						m_errorLog.Add( "Missing start marker: LN00, 1 <= N <= 89" );
						return false;
					}
					if( m_nEndLine == -1 )
					{
						//MessageBox.Show( "Missing end marker: M30", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
						worker.ReportProgress( 6, "kitchen" );
						m_errorLog.Add( "Missing end marker: M30" );
						return false;
					}
					if( m_nStartLine > m_nEndLine )
					{
						//MessageBox.Show("Marker in reverse order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
						worker.ReportProgress( 6, "kitchen" );
						m_errorLog.Add( "Marker in reverse order." );
						return false;
					}
				}
			}

			return true;
		}

		// check if L marker in range
		private bool IfFormatValid(string rawString)
		{
			int nN;
			Regex numberParser = new Regex( @"L\S+\d{2}?", RegexOptions.Compiled );
			foreach( Match match in numberParser.Matches( rawString ) )
			{
				if( int.TryParse( match.Value.Remove( 0, 1 ).Replace( "00", string.Empty ), out nN ) )
				{
					if( nN >= 1 && nN <= 89 )
						return true;
				}
			}
			return false;
		}

		private void MoveSegment(string path)
		{
			Debug.WriteLine( "==PRINT SELECTED LINES : " + m_nStartLine.ToString() + " : " + m_nEndLine.ToString());

			// report current progress
			worker.ReportProgress( 4, "kitchen" );

			// position to pickup after the moving
			int nLineCounter = 0;
			string line;

			// creating tmp file
			string tempFile = Path.GetTempFileName();

			using( StreamReader reader = new StreamReader( path ) )
			{
				using( StreamWriter writer = new StreamWriter( tempFile ) )
				{
					// wrtie none moving region
					while( ( line = reader.ReadLine() ) != null ) 
					{
						if( ( nLineCounter < m_nStartLine ) ||
							( nLineCounter > m_nEndLine ) )
						{
							// write content to file
							writer.WriteLine( line );
							writer.Flush();
						}

						nLineCounter++;
					}

					// reset reader and counter
					nLineCounter = 0;
					reader.DiscardBufferedData();
					reader.BaseStream.Seek(0, SeekOrigin.Begin);

					Debug.WriteLine( "==WRITING QUOTED REGION==" );
					while( ( line = reader.ReadLine() ) != null )
					{
						if( ( nLineCounter >= m_nStartLine ) &&
							( nLineCounter <= m_nEndLine ) )
						{
							// write content to file
							writer.WriteLine( line );
							writer.Flush();
						}

						nLineCounter++;
					}
				}
			}

			worker.ReportProgress( 5, "kitchen" );

			File.Delete( path );
			File.Move( tempFile, path );
		}

		private void RequestLog( )
		{
			if( m_errorLog.Count != 0 )
			{
				if( MessageBox.Show( "Some error occurred during the process,\nwould you like to know them?",
									 "Log Output", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
				{
					string logPath = "C:\\log-" + 
								  DateTime.Now.ToString( "yyyy-MM-dd-HH-mm-ss" ) + 
								  ".txt";
					
					StreamWriter writer = new StreamWriter( logPath );
					for( int counter = 0; counter < m_errorLog.Count - 1; counter += 2 )
					{
						writer.WriteLine( m_errorLog[ counter + 1 ] + " :: " + m_errorLog[ counter ] );
					}

					writer.Close();

					MessageBox.Show( "Log is generated at " + logPath , "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information );
				}
			}
			else
			{
				MessageBox.Show( "Process complete!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information );
			}
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			for(int fileCalled = 0; fileCalled<m_fileList.Count; fileCalled++)
			{
				string path = m_fileList[ fileCalled ];

				worker.ReportProgress( fileCalled, "fileCount" );
				worker.ReportProgress( -1, path );

				worker.ReportProgress( 1, "kitchen" );

				// search for markers in file
				SearchMarkers( path );

				// check if all the needed information are there
				if( IfFormatValid() )
				{
					// moving segments in the file
					MoveSegment( path );
				}
				else
				{
					m_errorLog.Add( path );
				}				
			}
		}

		// background worker process is complete
		// inspect response to see if an error occured
		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// ask users if they want error log(if exist)
			RequestLog();
			
			startButton.Enabled = true;
			startButton.Text = "Start";

			openButton.Enabled = true;

			// check to see if error exists
			if( e.Error != null )
			{
				MessageBox.Show( e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			// check to see if the worker is cancelled
			if( e.Cancelled )
			{
				fileProgressLabel.Text = "Cancelled...";
				kitchenProgressLabel.Text = "";
			}
			else
			{
				// everything completed normally
				fileProgressLabel.Text = "Completed!";
				kitchenProgressLabel.Text = "";
			}
		}
	
		private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			switch( e.UserState.GetType().ToString() as string )
			{
				case "fileCount":
					{
						fileProgressbar.Value = e.ProgressPercentage / m_fileList.Count * 100;
						break;
					}
				case "kitchen":
					{
						switch( e.ProgressPercentage )
						{
							case 1:
								kitchenProgressLabel.Text = "1/5 Opening file";
								break;
							case 2:
								kitchenProgressLabel.Text = "2/5 Searching for L marker...";
								break;
							case 3:
								kitchenProgressLabel.Text = "3/5 Searching for M marker...";
								break;
							case 4:
								kitchenProgressLabel.Text = "4/5 Moving quoted segment";
								break;
							case 5:
								kitchenProgressLabel.Text = "5/5 Save file";
								break;
							case 6:
								kitchenProgressLabel.Text = "ERROR!";
								break;
						}

						if( e.ProgressPercentage <= 5 )
							kitchenProgressbar.Value = e.ProgressPercentage * 20;
						else
							kitchenProgressbar.Value = 100;

						break;
					}
				default:
					{
						fileProgressLabel.Text = "Processing " + Path.GetFileName( e.UserState.ToString() );
						break;
					}
			}
		}

		private void openButton_Click(object sender, EventArgs e)
		{
			// clear selection list
			m_fileList.Clear();

			// displays an OpenFileDialog for user to select files
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "All Files|*.*";		// set filter
			openFileDialog.Title = "Select the File(s)";
			openFileDialog.Multiselect = true;				// enable multiselect

			// show the dialog
			if( openFileDialog.ShowDialog() == DialogResult.OK )
			{
				// save file list to m_fileList
				foreach( string readOut in openFileDialog.FileNames )
				{
					m_fileList.Add( readOut );
				}

				// file selected
				m_bFileSelected = true;
			}
			else
			{
				m_bFileSelected = false;
			}

			// enable start button for user to click
			startButton.Enabled = m_bFileSelected;
			fileProgressLabel.Text = (m_bFileSelected) ? "Waiting to start." : "Nothing selected...";

#if DEBUG
			Debug.WriteLine( "\n==FILE INPUT==" );
			foreach( string readOut in m_fileList )
			{
				Debug.WriteLine( readOut );
			}
			Debug.WriteLine( "==============\n" );
#endif
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			// if background worker is running, then click this button cause cancelling
			// otherwise, launch the thread
			if( worker.IsBusy )
			{
				startButton.Enabled = false;
				startButton.Text = "Stopping...";

				Debug.WriteLine( "==TRYING TO STOP BG THREAD==" );
				// notify the background worker that a cancel has been requested
				worker.CancelAsync();
			}
			else
			{
				startButton.Text = "Cancel";

				openButton.Enabled = false;
				
				// wipe the error log
				m_errorLog.Clear();

				Debug.WriteLine( "==INITIATE BG THREAD==" );
				// start the background worker
				worker.RunWorkerAsync();
			}
		}
	}
}