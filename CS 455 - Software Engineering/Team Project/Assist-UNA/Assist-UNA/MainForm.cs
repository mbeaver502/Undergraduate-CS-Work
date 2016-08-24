using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;
using Assist_UNA;

/**************************************************************************************************
 * 
 * Name: MainForm class
 * 
 * ================================================================================================
 * 
 * Description: This class is the main form user interface window. 
 *                            
 * ================================================================================================
 * 
 * Modification History
 * --------------------
 * 03/09/2014   ACA     Original version.
 * 03/09/2014   ACA     Added data members and options form functionality. 
 *                      Documented code.
 * 03/10/2014   ACA     Added more data members and methods.
 * 03/11/2014   ACA     Added more functionality and documentation.
 * 03/13/2014   ACA     Added/fixed documentation.
 * 03/13/2014   CLB     Added data members and methods.
 * 03/14/2014   CLB     Added functionality to some of the buttons on the Main Form.
 * 03/14/2014   ACA     Added functionality to line and column labels. Added column ruler.
 *                      Added first version of line numbers.
 *                      Added custom textbox component.
 *                      Added data members and some event methods. Added/fixed documentation.
 * 03/15/2014   ACA     Added saving and opening projects and importing .txt code with THH.
 * 03/16/2014   THH     Some formatting, code and commenting cleanup. 
 *                      Added some functionality to the printing functions.
 * 03/17/2014   ACA     Added tab stops. 
 *                      Commented out line numbers. 
 *                      Added "are you sure" upon closing without saving. 
 *                      Added/fixed documentation. 
 *                      Added functionality to the save menu button. 
 *                      Added the ability to insert into both the memory listview and the 
 *                          symbol table listview. 
 *                      Added the ability to update the register displays. 
 *                      Added functionality to various methods. 
 *                      Fixed some oversights. 
 *                      Added view PRT functionality. 
 *                      Added exporting to .txt and .rtf files.
 * 03/17/2014   THH     More code cleanup, modulized some of the functions that are used on 
 *                          multiple points (ex. print, open, save, etc.).
 * 03/17/2014   ACA     Added shortcuts and tooltips. 
 * 03/19/2014   ACA     Changed to rich text box and fixed most features back.
 *                      Added minimal, slightly buggy syntax highlighting.
 *                      Disabled zoom and font size change.
 *                      Added many data members for instructions colors and strings.
 *                      Added some convenience data members.
 *                       Disabled word wrap and disallowed typing past line 79, for highlights.
 *                      Other various tweaks.
 * 03/20/2014   ACA     Disabled some buggy features and made some minor tweaks.
 *                      Made some visual changes per client feedback.
 * 03/22/2014   ACA     Removed unneeded data members and added some color ones.
 *                      Syntax highlighting functional, but flashy.
 *                      Added input form and updated all assemble methods.
 *                      Fixed some new bugs from recent changes.
 * 03/23/2014   ACA     Fixed syntax highlighting bugs, but still flashy.
 *                      Added ASSIST/UNA options to option method.
 * 03/24/2014   ACA     Added the option to set colors in the options menu.
 *                      Syntax highlighting still really buggy - next on the list.
 *                      Added # before lines in .una file.
 *                      Added pathPRT to .una lines so .PRT can be in a separate directory.
 *                      The path to the PRT can be set in the options.
 * 03/27/2014   CLB     Worked on printing the source code and .PRT file.
 * 03/28/2014   ACA     Added output form functionality.
 *                      Fixed end of line commenting while typing finally. Maybe? Hopefully?
 *                      Added syntax highlighting check for all text - mainly for use with 
 *                          open, import, and paste.
 *                      Fixed some documentation errors and took out some testing actions.
 * 03/29/2014   ACA     Added some temporary/unfinished debugging options.
 *                      Fixed syntax highlighting AGAIN. For the last time?
 *                      Updated input box.
 *                      Added a way to avoid textchanged event if needed.
 *                      Other various minor changes.
 * 03/31/2014   ACA     Added a method to only check a given range for highlighting.
 *                      Made checking format on pasted text only check the pasted text.
 *                      Added tabstops every 4 spaces past column 16.
 *                      Fixed some documentation and code which was not to standards.
 *                      There are STILL some minor bugs in syntax highlighting.
 *                      Nevermind, I think I may have just fixed them. Perhaps?
 * 04/01/2014   CLB     Added printing functionality.
 * 04/01/2014   ACA     Added search and replace functionality.
 *                      Fixed going past line 79, other than pasting.
 *                      Options menu no longer formats all text unless comment color is changed.
 * 04/03/2014   ACA     Code format updated and minor changes.
 * 04/04/2014   ACA     Minor bug fixes and efficiency update.
 * 04/05/2014   ACA     Major documentation update - All code should be to standards.
 *                      Some helper methods added to clean up code.
 *                      Tab stops should be more efficient.
 * 04/05/2014   THH     Added possible implementation with backend using the Processing class.
 *                      (See Assemble, AssembleDebug, and AssembleFinalRun methods.)
 *                      Fixed issue when printing source code, it would always print to default.
 * 04/08/2014   ACA     Added accessors, changed how import works, and fixed a PRT bug.
 *                      Worked on assembly integration with THH.
 *                      Fixed a syntax highlighting known minor bug with character literals.
 * 04/09/2014   ACA     Assemble now opens a save dialog if the project does not exist.
 *                      Took out print PRT button from main form, as nobody should really print
 *                          without looking at the PRT first - it also simplified printing PRT.
 *                      Added ctrl + P functionality back for printing.
 *                      Updated version in about form to v.03 since integration to main project.
 * 04/10/2014   ACA     Text no longer flashes!
 *                      Program no longer scrolls when loading source - status says loading now.
 * 04/15/2014   ACA     Worked on integration error handling.
 *                      Fixed some view PRT bugs.
 * 04/17/2014   THH     Fixed the update symbol table entry method.
 * 04/17/2014   ACA     Identifier is now forced uppercase.
 *                      Display now clears before assembly.
 *                      Updated PSW.
 *                      Register display is now empty by default.
 *                      Changed sizing.
 * 04/18/2014   CAF     Added a force save before assembly to all assembly methods.
 *              JMB     Assisted in editing assembly methods.
 * 04/20/2014   JMB     Implemented InsertMemoryEntry and RemoveMemoryEntry.
 * 04/21/2014   THH     Added cursor changes to opening a file.
 * 04/21/2014   ACA     Syntax Highlighting bug fixed (They never stop).
 *                      Some minor updates and improvements.
 * 04/22/2014   ACA     Worked on debugging and other integration.
 * 04/23/2014   ACA     Worked on debugging some more.
 *                      Added a few features that are commented out for now for debugging.
 *                      Should no longer be able to paste past line 79 (was tough to figure out)!
 *                      Should no longer get an exception on Outform disposal (hopefully).
 * 04/23/2014   JMB     Added BeginUpdate() and EndUpdate() handling to lvMemory and lvSymbolTable
 *                          setters to hopefully avoid flickering.
 * 04/23/2014   ACA     Worked on debugging some more.
 *                      Fixed a pasting bug, hopefully (I seem to say that a lot...).
 *                      Made sure assembler option inputs are not >9999.
 *                      Added a couple status label updates/changes.
 *                      Organized newer code.
 * 04/24/2014   CAF     Added icon for all forms/taskbars/etc.
 * 04/25/2014   ACA     Debug mode now seems to be working!
 *                      Changed how output form works in debug mode slightly.
 *                      Added buttons back to mainform for debugging, instead of output form.
 *                      Added a third debug option to run to end.
 *                      Minor visual glitch while debugging for me when scrolling memory quickly.
 *                      Added in ability to double click .una files to open the program with
 *                          them loaded (THH figured out most of the code for this).
 *                      Fixed a bug that would set the PRT path to null.
 * 04/26/2014   ACA     Fixed a debugging bug.
 *                      Fixed yet another syntax highlighting bug.
 *                      Changed the display mode for debugging.
 *                      Minimum size bug while debugging fixed.
 *                      All colored forms now use main form scheme.
 *                      Updated new button.
 *                      Added Debug menu and debug option shortcuts.
 *                      Modularized debug methods.
 *                      Debug icons added.
 *                      Fixed a few bugs that allowed going past line 79.
 *                      Added a data member/method/ability to keep up with last instruction number.
 *                      Added instruction number output in status label.
 *                      Updated documentation with more explanations.
 *                      View PRT can now be used in unison with main form, and while debugging.
 * 04/26/2014   THH     Fixed issue where View PRT button was disabled when it shouldn't.
 * 04/27/2014   ACA     Added Run to instruction while debugging.
 *                      Debugging now displays more information.
 *                      All lines longer than 79 when importing now truncate to 79.
 *                      Fixed a bug in printing.
 *                      Printing button is disabled if form is empty.
 *                      Fixed a bug in syntax highlighting when putting quotes in comments.
 *                      Updated how syntax highlighting works, should be more efficient.
 * 04/27/2014   JMB     Fixed the AppendOutputText method so that output always starts on the
 *                          first line of the Output box. Updated MessageBox calls.
 * 04/28/2014   CAF     Updated the online help method to open the online user manual.
 * 04/28/2014   ACA     Online help was throwing an exception if manual not found, try/catch added.
 *                      Threw together a method to search along the pathway of the .exe to try to
 *                          find the user manual folder or the .html file to open online help.
 *                      Added more detailed save exception messages.
 *                      Possible save exception fix.
 *                      Print source was cutting off lines, also got a arare exception.
 *                      Print source should be fixed, and try/catch added (with JMB).
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    public partial class MainForm : Form
    {
        /* Constants. */
        private const int CHARACTERS_PER_PAGE = 5355;
        private const int DEFAULT_MAX_INSTRUCTIONS = 5000;
        private const int DEFAULT_MAX_LINES = 500;
        private const int DEFAULT_MAX_PAGES = 100;
        private const int DEFAULT_MAX_SIZE = 2700;
        private const int MAINFORM_WIDTH = 1048;
        private const int MAINFORM_HEIGHT = 648;
        private const int MAINFORM_MIN_HEIGHT = 625;


        /* Private members. */
        private bool activateTextChanged = true;
        private bool debugClicked = false;
        private bool debugMode = false;
        private bool isSaved = false;
        private bool projectExists = false;
        private bool runToLocation = false;
        private bool stopDebugging = false;
        private Color backColorAccent = Color.FromArgb(255, 255, 162);
        private Color backColorMain = Color.FromArgb(152, 0, 230);
        private Color backColorMain2 = Color.FromArgb(180, 100, 255);
        private Color commentColor = Color.DeepPink;
        private Color defaultSourceColor = Color.Black;
        private Color formLabelTextColor = SystemColors.Control;
        private Color formTextColor = Color.FromArgb(120, 0, 120);
        private int cursorColumn = 0;
        private int cursorLine = 0;
        private int cursorLineLength = 0;
        private int cursorLineStartIndex = 0;
        private int instructionNumber = 0;
        private int maxInstructions = 5000;
        private int maxLines = 500;
        private int maxPages = 100;
        private int maxSize = 2700;
        private int minimumWidth = 0;
        OutputForm outForm;
        private Point memoryLocation;
        private PrintDocument printDoc = new PrintDocument();
        private Size memorySize;
        private string directory = "";
        private string fileName = "Unsaved Project";
        private string identifier = "";
        private string outputText = "";
        private string pathPRT = "";
        private string pageToPrint = "";
        private string stringToPrint = "";
        private string totalSourceString = "";
        private uint inputLocation = 0;
        private List<string>.Enumerator pageEnumerator;

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        MainForm
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the main form.
         *  
         *****************************************************************************************/
        public MainForm()
        {
            /* Show the loading image. */
            LoadingForm load = new LoadingForm();
            load.Show();
            Cursor.Current = Cursors.AppStarting;

            /* Make sure that the logo shows for at least 2 seconds. */
            Thread.Sleep(2000);

            /* Logo will still be shown while the main form is initializing. */
            InitializeComponent();

            /* Make sure that the user cannot make the form so small that it breaks display. */
            minimumWidth = Screen.PrimaryScreen.WorkingArea.Width / 2;
            this.MinimumSize = new Size(minimumWidth, MAINFORM_MIN_HEIGHT);

            /* Start the program with the source editor in focus. */
            txtSource.BringToFront();
            lvMemory.BringToFront();
            txtSource.Focus();
            txtSource.Select();

            /* Initialize the printing and output objects. */
            printDoc.PrintPage += new PrintPageEventHandler(PrintDocumentOnPrintPage);
            outForm = new OutputForm(this);

            menuFilePrint.Enabled = false;
            tsPrint.Enabled = false;

            /* Once the form has loaded, close the loading image. */
            Cursor.Current = Cursors.Default;
            load.Close();
        }

        /******************************************************************************************
         * 
         * Name:        MainForm
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the main form.
         *  
         *****************************************************************************************/
        public MainForm(string openPath)
        {
            /* Show the loading image. */
            LoadingForm load = new LoadingForm();
            load.Show();
            Cursor.Current = Cursors.AppStarting;

            /* Make sure that the logo shows for at least 1 second. */
            Thread.Sleep(1000);

            /* Logo will still be shown while the main form is initializing. */
            InitializeComponent();

            /* Make sure that the user cannot make the form so small that it breaks display. */
            minimumWidth = Screen.PrimaryScreen.WorkingArea.Width / 2;
            this.MinimumSize = new Size(minimumWidth, MAINFORM_MIN_HEIGHT);

            /* Start the program with the source editor in focus. */
            txtSource.BringToFront();
            lvMemory.BringToFront();
            txtSource.Focus();
            txtSource.Select();

            /* Initialize the printing and output objects. */
            printDoc.PrintPage += new PrintPageEventHandler(PrintDocumentOnPrintPage);
            outForm = new OutputForm(this);

            /* Open the file that was double-clicked on. */
            OpenProject(openPath);

            if (txtSource.Text == "")
            {
                menuFilePrint.Enabled = false;
                tsPrint.Enabled = false;
            }

            else
            {
                menuFilePrint.Enabled = true;
                tsPrint.Enabled = true;
            }

            /* Once the form has loaded, close the loading image. */
            Cursor.Current = Cursors.Default;
            load.Close();
        }

        /******************************************************************************************
         * 
         * Name:        GetDebugMode()
         * 
         * Author(s):   Chad Farley
         *              
         * Input:       N/A
         * Return:      The debugMode data member (bool).
         * Description: This method will return current state of the debug mode flag.
         *  
         *****************************************************************************************/
        public bool GetDebugMode()
        {
            return debugMode;
        }

        /******************************************************************************************
         * 
         * Name:        WaitForDebugClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The number of the instruction currently being debugged, and if it is the 
         *              last instruction.
         * Return:      Flag to stop debugging.
         * Description: This method will put the form into debug mode.
         *  
         *****************************************************************************************/
        public bool WaitForDebugClick(int instrNumber, bool lastInstr, uint locationCounter)
        {
            /* Unlock window if it is locked, and update output form. */
            LockWindowUpdate(IntPtr.Zero);
            outForm.SetDebug(debugMode);
            outForm.SetText(outputText);

            /* Set the instruction number and update status accordingly. */
            instructionNumber = instrNumber;
            UpdateStatusLabel("Debugging instruction " + instrNumber + " at location " + locationCounter.ToString("X").PadLeft(6,'0'));

            /* Disable the Next debug button when on the last instruction. */
            if (lastInstr)
            {
                tsDebugNext.Enabled = false;
                menuDebugNext.Enabled = false;
                menuDebugRunToEnd.Enabled = false;
                menuDebugRunToLocation.Enabled = false;
                tsViewPRT.Enabled = false;
                menuToolsViewPRT.Enabled = false;
            }                

            /* Turn on debugging mode when first called. */
            if (instrNumber == 1)
            {
                DisplayDebugMode(true);
                outForm.Show();
                outForm.BringToFront();
            }

            /* Set the values to be used in the debug loop. */
            stopDebugging = false;
            debugClicked = false;

            if (runToLocation)
            {
                if (locationCounter == inputLocation)
                {
                    runToLocation = false;
                }

                else
                    debugClicked = true;
            }

            /* Halt all execution until a debugging button is clicked. */
            while ((debugMode) && (debugClicked == false))
                Application.DoEvents();
            
            debugClicked = false;

            if (debugMode)
            {
                /* Try to minimize flashing. */
                LockWindowUpdate(txtSource.Handle);
                LockWindowUpdate(lvMemory.Handle);
            }

            else
            {
                /* Update status label according to button press. */
                if (stopDebugging)
                    UpdateStatusLabel("Debugging stopped. Last instruction number executed: " 
                        + instrNumber);
                else
                    UpdateStatusLabel("Assembly and debug complete. Last instruction number " 
                        + "executed: " + instrNumber);
            }
                
            return stopDebugging;
        }

        /******************************************************************************************
         * 
         * Name:        GetInstructionNumber
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The instructionNumber data member (integer).
         * Description: This method will return current instruction number being debugged.
         *  
         *****************************************************************************************/
        public int GetInstructionNumber()
        {
            return instructionNumber;
        }

        /******************************************************************************************
         * 
         * Name:        GetMaxInstructions
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The maxInstructions data member (integer).
         * Description: This method will return current value of the maximum number of instructions
         *               ASSIST option.
         *  
         *****************************************************************************************/
        public int GetMaxInstructions()
        {
            return maxInstructions;
        }

        /******************************************************************************************
         * 
         * Name:        GetMaxLines
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The maxLines data member (integer).
         * Description: This method will return current value for the maximum number of lines
         *              ASSIST option.
         *  
         *****************************************************************************************/
        public int GetMaxLines()
        {
            return maxLines;
        }

        /******************************************************************************************
         * 
         * Name:        GetMaxPages
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The maxPages data member (integer).
         * Description: This method will return current value for the maximum number of pages
         *              ASSIST option.
         *  
         *****************************************************************************************/
        public int GetMaxPages()
        {
            return maxPages;
        }

        /******************************************************************************************
         * 
         * Name:        GetMaxSize
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The maxSize data member (integer).
         * Description: This method will return current value for the maximum size ASSIST option.
         *  
         *****************************************************************************************/
        public int GetMaxSize()
        {
            return maxSize;
        }

        /******************************************************************************************
         * 
         * Name:        GetDirectory
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The path to the folder containing the project (string).
         * Description: This method will return the path to the project.
         *  
         *****************************************************************************************/
        public string GetDirectory()
        {
            return directory;
        }

        /******************************************************************************************
         * 
         * Name:        GetFileNameProject
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The fileNameProject (string).
         * Description: This method will return the project name.
         *  
         *****************************************************************************************/
        public string GetFileNameProject()
        {
            return fileName;
        }

        /******************************************************************************************
         * 
         * Name:        GetID
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The identifier data member (string).
         * Description: This method will return the identifier.
         *  
         *****************************************************************************************/
        public string GetID()
        {
            return identifier;
        }

        /******************************************************************************************
         * 
         * Name:        GetPathPRT
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The pathPRT data member (string).
         * Description: This method will return the PRT file path.
         *  
         *****************************************************************************************/
        public string GetPathPRT()
        {
            return pathPRT;
        }

        /******************************************************************************************
         * 
         * Name:        AddMemoryEntry
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       Three strings, representing the three columns
         * Return:      N/A
         * Description: This method will add an item to the memory display on the GUI.
         *  
         *****************************************************************************************/
        public void AddMemoryEntry(string address, string contents, string charRepresentation)
        {
            /* List views need arrays to input. */
            string[] arr = new string[3];
            ListViewItem lvItem;

            arr[0] = address;
            arr[1] = contents;
            arr[2] = charRepresentation;

            /* Try to minimize flashing. */
            lvMemory.BeginUpdate();
            try
            {
                lvItem = new ListViewItem(arr);
                lvMemory.Items.Add(lvItem);
            }

            finally
            {
                lvMemory.EndUpdate();
            }
        }

        /******************************************************************************************
         * 
         * Name:        AddSymbolTableEntry
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              Michael Beaver
         *              
         * Input:       Two strings, representing the two columns.
         * Return:      N/A
         * Description: This method will add an item to the symbol table display on the GUI.
         *  
         *****************************************************************************************/
        public void AddSymbolTableEntry(string symbol, string location)
        {
            string[] arr = new string[2];
            ListViewItem lvItem;
            arr[0] = symbol;
            arr[1] = location;

            lvSymbolTable.BeginUpdate();
            try
            {
                lvItem = new ListViewItem(arr);
                lvSymbolTable.Items.Add(lvItem);
            }

            finally
            {
                lvSymbolTable.EndUpdate();
            }
        }

        /******************************************************************************************
         * 
         * Name:        AppendOutputText
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       New string to be appended to the output text.
         * Return:      N/A
         * Description: This method will append text to the output text.
         *  
         *****************************************************************************************/
        public void AppendOutputText(string newText)
        {
            /* Avoid "blank" lines at the beginning of the Output box. */
            if (String.IsNullOrEmpty(outputText))
                outputText = newText.Replace(Environment.NewLine, "");
            
            /* Strip form feed escape sequences. */
            else if (newText.Contains("\f"))
                outputText += newText.Replace("\f", Environment.NewLine);
                
            else
                outputText += newText; 
        }

        /******************************************************************************************
         * 
         * Name:        AppendToTopOutputText
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be appended to the beginning of output text.
         * Return:      N/A
         * Description: This method will append text to the beginning of the output text.
         *  
         *****************************************************************************************/
        public void AppendToTopOutputText(string newtext)
        {
            outputText = newtext + outputText;
        }

        /******************************************************************************************
         * 
         * Name:        DisplayDebugMode
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              
         * Input:       True to put the form into debug mode, false to put it into normal mode.
         * Return:      N/A
         * Description: This method will put the GUI into debug mode.
         *  
         *****************************************************************************************/
        public void DisplayDebugMode(bool debugModeOn)
        {
            /* Disable or re-enable all non-debugging controls. */
            menuFile.Enabled = !(debugModeOn);
            menuAssemble.Enabled = !(debugModeOn);
            menuEdit.Enabled = !(debugModeOn);
            menuToolsOptions.Enabled = !(debugModeOn);
            menuAssembleAssemble.Enabled = !(debugModeOn);
            menuAssembleDebug.Enabled = !(debugModeOn);
            menuAssembleFinalRun.Enabled = !(debugModeOn);
            menuEditCopy.Enabled = !(debugModeOn);
            menuEditCut.Enabled = !(debugModeOn);
            menuEditPaste.Enabled = !(debugModeOn);
            menuEditReplace.Enabled = !(debugModeOn);
            menuEditSearch.Enabled = !(debugModeOn);
            menuFileNew.Enabled = !(debugModeOn);
            menuFileOpen.Enabled = !(debugModeOn);
            menuFilePrint.Enabled = !(debugModeOn);
            menuFileSave.Enabled = !(debugModeOn);
            tsAssembleDebug.Enabled = !(debugModeOn);
            tsAssemble.Enabled = !(debugModeOn);
            tsAssembleFinalRun.Enabled = !(debugModeOn);
            tsNew.Enabled = !(debugModeOn);
            tsOpen.Enabled = !(debugModeOn);
            tsPrint.Enabled = !(debugModeOn);
            tsSave.Enabled = !(debugModeOn);
            tsSaveSaveAs.Enabled = !(debugModeOn);
            txtSource.Enabled = !(debugModeOn);

            menuDebug.Visible = debugModeOn;
            menuDebug.Enabled = debugModeOn;
            menuDebugNext.Enabled = debugModeOn;
            menuDebugNext.Visible = debugModeOn;
            menuDebugRunToEnd.Enabled = debugModeOn;
            menuDebugRunToEnd.Visible = debugModeOn;
            menuDebugStop.Enabled = debugModeOn;
            menuDebugStop.Visible = debugModeOn;
            menuDebugRunToLocation.Enabled = debugModeOn;
            menuDebugRunToLocation.Visible = debugModeOn;

            tsDebugNext.Enabled = debugModeOn;
            tsDebugNext.Visible = debugModeOn;
            tsDebugStop.Enabled = debugModeOn;
            tsDebugStop.Visible = debugModeOn;
            tsDebugRunToEnd.Enabled = debugModeOn;
            tsDebugRunToEnd.Visible = debugModeOn;
            tsDebugRunToLocation.Enabled = debugModeOn;
            tsDebugRunToLocation.Visible = debugModeOn;

            menuToolsViewPRT.Enabled = true;
            tsViewPRT.Enabled = true;          
            
            if (debugModeOn)
            {
                lvMemory.Focus();

                /* Don't allow resize in debug mode. */
                this.MinimumSize = this.Size;
                this.MaximumSize = this.Size;
                this.MaximizeBox = false;

                /* Save old memory location and size. */
                memoryLocation = lvMemory.Location;
                memorySize = lvMemory.Size;

                /* Resize memory and move it to new location. */
                lvMemory.Location = new Point(pnlSourcePanel.Location.X, 
                    pnlSourcePanel.Location.Y);

                lvMemory.Size = new Size(lvMemory.Width, 
                    pnlSourcePanel.Height + lvMemory.Height + 12);
            }

            else
            {
                /* Put memory display back to normal mode. */
                this.MinimumSize = new Size(minimumWidth, MAINFORM_MIN_HEIGHT);
                this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
                this.MaximizeBox = true;

                lvMemory.Location = memoryLocation;
                lvMemory.Size = memorySize;

                /* Close the debug output window and refocus on the source editor. */
                outForm.SetText("");
                outForm.Hide();
                LockWindowUpdate(IntPtr.Zero);
                txtSource.Focus();
            }
        }

        /******************************************************************************************
         * 
         * Name:        InsertMemoryEntry
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       The index is an integer. Three strings, representing the three columns.
         * Return:      N/A
         * Description: This method will add an item to the memory display on the GUI.
         *  
         *****************************************************************************************/
        public void InsertMemoryEntry(int index, string address, string contents, string charRep)
        {
            string[] arr = new string[3];
            ListViewItem lvItem;

            arr[0] = address;
            arr[1] = contents;
            arr[2] = charRep;

            lvItem = new ListViewItem(arr);

            lvMemory.Items.Insert(index, lvItem);
        }

        /******************************************************************************************
         * 
         * Name:        RemoveMemoryEntry
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       The index is an integer.
         * Return:      N/A
         * Description: This method will remove a row in the memory contents at the given index.
         *  
         *****************************************************************************************/
        public void RemoveMemoryEntry(int index)
        {
            lvMemory.Items.RemoveAt(index);
        }

        /******************************************************************************************
         * 
         * Name:        SetFileNameProject
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be the project name.
         * Return:      N/A
         * Description: This method will set the project name.
         *  
         *****************************************************************************************/
        public void SetFileNameProject(string newFileName)
        {
            fileName = newFileName;
        }

        /******************************************************************************************
         * 
         * Name:        SetID
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be the identifier.
         * Return:      N/A
         * Description: This method will set the identifier.
         *  
         *****************************************************************************************/
        public void SetID(string newID)
        {
            identifier = newID;
        }

        /******************************************************************************************
         * 
         * Name:        SetInstructionNumber
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New number to be the current instruction number.
         * Return:      N/A
         * Description: This method will set the instruction number.
         *  
         *****************************************************************************************/
        public void SetInstructionNumber(int instrNumber)
        {
            instructionNumber = instrNumber;
        }

        /******************************************************************************************
         * 
         * Name:        SetOutputText
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       Text to be set as the output text.
         * Return:      N/A
         * Description: This method will set the output form text.
         *  
         *****************************************************************************************/
        public void SetOutputText(string newText)
        {
            outputText = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetPathPRT
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be the PRT file name.
         * Return:      N/A
         * Description: This method will set the PRT file name.
         *  
         *****************************************************************************************/
        public void SetPathPRT(string newpath)
        {
            pathPRT = newpath;
            isSaved = false;
            if (this.Text.Contains("*") == false)
                this.Text += "*";
        }

        /******************************************************************************************
         * 
         * Name:        SetPSW
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for PSW.
         * Return:      N/A
         * Description: This method will set the PSW text box text.
         *  
         *****************************************************************************************/
        public void SetPSW(string newPSWText)
        {
            txtRegisterPSW.Text = newPSWText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister0
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register0.
         * Return:      N/A
         * Description: This method will set the Register00 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister0(string newText)
        {
            txtRegister00.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister1
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register1.
         * Return:      N/A
         * Description: This method will set the Register01 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister1(string newText)
        {
            txtRegister01.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister2
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register2.
         * Return:      N/A
         * Description: This method will set the Register02 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister2(string newText)
        {
            txtRegister02.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister3
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register3.
         * Return:      N/A
         * Description: This method will set the Register03 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister3(string newText)
        {
            txtRegister03.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister4
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register4.
         * Return:      N/A
         * Description: This method will set the Register04 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister4(string newText)
        {
            txtRegister04.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister5
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register5.
         * Return:      N/A
         * Description: This method will set the Register05 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister5(string newText)
        {
            txtRegister05.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister6
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register6.
         * Return:      N/A
         * Description: This method will set the Register06 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister6(string newText)
        {
            txtRegister06.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister7
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register7.
         * Return:      N/A
         * Description: This method will set the Register07 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister7(string newText)
        {
            txtRegister07.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister8
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register8.
         * Return:      N/A
         * Description: This method will set the Register08 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister8(string newText)
        {
            txtRegister08.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister9
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register9.
         * Return:      N/A
         * Description: This method will set the Register09 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister9(string newText)
        {
            txtRegister09.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister10
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register10.
         * Return:      N/A
         * Description: This method will set the Register10 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister10(string newText)
        {
            txtRegister10.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister11
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register11.
         * Return:      N/A
         * Description: This method will set the Register11 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister11(string newText)
        {
            txtRegister11.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister12
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register12.
         * Return:      N/A
         * Description: This method will set the Register12 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister12(string newText)
        {
            txtRegister12.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister13
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register13.
         * Return:      N/A
         * Description: This method will set the Register13 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister13(string newText)
        {
            txtRegister13.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister14
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register14.
         * Return:      N/A
         * Description: This method will set the Register14 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister14(string newText)
        {
            txtRegister14.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        SetRegister15
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be displayed for Register15.
         * Return:      N/A
         * Description: This method will set the Register15 text box text.
         *  
         *****************************************************************************************/
        public void SetRegister15(string newText)
        {
            txtRegister15.Text = newText;
        }

        /******************************************************************************************
         * 
         * Name:        ShowOutput
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will show the output form.
         *  
         *****************************************************************************************/
        public void ShowOutput()
        {
            OutputForm output = new OutputForm();
            output.SetDebug(debugMode);
            output.SetText(outputText);
            output.Show();
        }

        /******************************************************************************************
         * 
         * Name:        UpdateStatusLabel
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       String to be the new status label text.
         * Return:      N/A
         * Description: This method will update the text of the status label.
         *  
         *****************************************************************************************/
        public void UpdateStatusLabel(string newText)
        {
            ssStatusStatusLabel.Text = newText;
            this.Refresh();
        }


        /* Private methods. */

        /******************************************************************************************
         * 
         * Name:        LockWindowUpdate
         * 
         * Author(s):   Drew Aaron
         *              Reference 2
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will import lockwindow to remove flashing when highlighting.
         *  
         *****************************************************************************************/
        [DllImport("user32.dll")]
        private static extern bool LockWindowUpdate(IntPtr hWndLock);

        /******************************************************************************************
         * 
         * Name:        OkayToAssemble
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will check if the project is prepared to be assembled.
         *  
         *****************************************************************************************/
        private bool OkayToAssemble()
        {
            if ((projectExists == false) || (directory == ""))
            {
                /* Automatically open save dialog if project does not exist. */
                SaveProject();

                /* If the user still did not save the project, show an error. */
                if ((projectExists == false) || (directory == ""))
                {
                    MessageBox.Show("Project must be saved at least once before assembling.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
            }

            /* Sending in a blank source would confuse the assembler. */
            if (txtSource.Text == "")
            {
                MessageBox.Show("Cannot assemble an empty project!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }

            /* If all conditions are met, then it is okay to assemble the source code. */
            else
                return true;
        }

        /******************************************************************************************
         * 
         * Name:        Assemble
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              Michael Beaver
         *              Chad Farley
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will assemble the source code and save a PRT file.
         *              The project is saved before assembly.
         *  
         *****************************************************************************************/
        private void Assemble()
        {
            /* Make sure the project has been saved before assembling. */
            if (OkayToAssemble())
            {
                /* Set path to the PRT file if it has not been set yet. */
                if (pathPRT == "")
                    pathPRT = directory + "\\" + fileName + ".PRT";

                /* Save the project before assembling. */
                if (isSaved == false)
                    SaveProject();

                /* Set members to be used by backend. */
                identifier = "";
                debugMode = false;

                /* Clear old data. */
                ClearDisplay();
                outputText = "";

                Processing.Assemble(this);
            }
        }

        /******************************************************************************************
         * 
         * Name:        AssembleDebug
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              Michael Beaver
         *              Chad Farley
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will assemble the source code and step through the execution.
         *  
         *****************************************************************************************/
        private void AssembleDebug()
        {
            /* Make sure the project has been saved before assembling. */
            if (OkayToAssemble())
            {
                /* Save the project before assembling. */
                if (isSaved == false)
                    SaveProject();
                
                /* Set members to be used by backend. */
                debugMode = true;
                runToLocation = false;
                inputLocation = 0;

                /* Clear old data. */
                ClearDisplay();
                outputText = "";

                /* Assembly function here */
                Processing.AssembleDebug(this);

                debugMode = false;
            }
        }

        /******************************************************************************************
         * 
         * Name:        AssembleFinalRun
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              Michael Beaver
         *              Chad Farley
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will assemble the source code,save a PRT file, and execute the 
         *              program.
         *  
         *****************************************************************************************/
        private void AssembleFinalRun()
        {
            /* Make sure the project has been saved before assembling. */
            if (OkayToAssemble())
            {
                /* Set path to the PRT file if it has not been set yet. */
                if (pathPRT == "")
                    pathPRT = directory + "\\" + fileName + ".PRT";

                /* Save the project before assembling. */
                if (isSaved == false)
                    SaveProject();

                /* Set members to be used by backend. */
                debugMode = false;
                InputForm input = new InputForm("ASSIST/UNA", "Enter Identifer", 39);
                input.ShowDialog();

                if (input.Canceled())
                    return;
                else
                    identifier = input.GetInput().ToUpper();

                /* Clear old data. */
                ClearDisplay();
                outputText = "";

                /* Assembly function here */
                Processing.AssembleFinalRun(this);
            }
        }

        /******************************************************************************************
         * 
         * Name:        ClearDisplay
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will set the register display, symbol table display, and memory 
         *              display back to their default states.              
         *  
         *****************************************************************************************/
        private void ClearDisplay()
        {
            /* Clear litview displays. */
            lvMemory.Items.Clear();
            lvSymbolTable.Items.Clear();

            /* Set default values. */
            txtRegister00.Text = "";
            txtRegister01.Text = "";
            txtRegister02.Text = "";
            txtRegister03.Text = "";
            txtRegister04.Text = "";
            txtRegister05.Text = "";
            txtRegister06.Text = "";
            txtRegister07.Text = "";
            txtRegister08.Text = "";
            txtRegister09.Text = "";
            txtRegister10.Text = "";
            txtRegister11.Text = "";
            txtRegister12.Text = "";
            txtRegister13.Text = "";
            txtRegister14.Text = "";
            txtRegister15.Text = "";
            txtRegisterPSW.Text = "";
        }

        /******************************************************************************************
         * 
         * Name:        DebugNext
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will step to the next instruction while debugging.            
         *  
         *****************************************************************************************/
        private void DebugNext()
        {
            debugClicked = true;
            stopDebugging = false;
            debugMode = true;
        }

        /******************************************************************************************
         * 
         * Name:        DebugRunToEnd
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will run to the end of the program while debugging.            
         *  
         *****************************************************************************************/
        private void DebugRunToEnd()
        {
            debugClicked = true;
            stopDebugging = false;
            debugMode = false;

            DisplayDebugMode(false);
        }

        /******************************************************************************************
         * 
         * Name:        DebugRunToLocation
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will run to a specified location while debugging.            
         *  
         *****************************************************************************************/
        private void DebugRunToLocation()
        {
            try
            {
                /* Get location counter as input from the user. */
                InputForm locationInput = new InputForm("Insert location", "Insert location counter number to run to: ", 6);
                locationInput.ShowDialog();

                if (locationInput.Canceled())
                    return;
                else
                    inputLocation = Convert.ToUInt32(locationInput.GetInput(), 16);

                runToLocation = true;
                debugClicked = true;
                stopDebugging = false;
            }

            /* If location entered is invalid, show error message. */
            catch
            {
                MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /******************************************************************************************
         * 
         * Name:        DebugStop
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will stop debugging.            
         *  
         *****************************************************************************************/
        private void DebugStop()
        {
            debugClicked = true;
            stopDebugging = true;
            debugMode = false;

            DisplayDebugMode(false);
        }

        /******************************************************************************************
         * 
         * Name:        FormatAllText
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       The index location to place the cursor after formatting (integer).
         * Return:      N/A
         * Description: This method will check the entire source editor and highlight certain 
         *              syntax strings.
         *  
         *****************************************************************************************/
        private void FormatAllText(int position)
        {
            /* Temporary variables to assist in highlighting syntax. */
            bool enableColor = true;
            bool eolComment = false;
            int endOfOperand = 0;
            int entryCardLocation = 0;
            int highlightStart = 16;
            int lineSpaceIndex = 16;
            int lineStartIndex = 0;

            activateTextChanged = false;

            /* Loop through every line and set syntax highlighting accordingly. */
            for (int i = 0; i < txtSource.Lines.Length; i++)
            {
                if (txtSource.Lines[i].Length > 0)
                {
                    lineStartIndex = txtSource.GetFirstCharIndexFromLine(i);

                    /* Highlight comments of line starting with an asterisk. */
                    if (txtSource.Lines[i][0] == '*')
                    {
                        txtSource.Select(lineStartIndex, txtSource.Lines[i].Length);
                        txtSource.SelectionColor = commentColor;
                    }

                    /* Highlight end of line comments. */
                    else if (txtSource.Lines[i].Length > 16)
                    {
                        try
                        {
                            /* Don't highlight after $ENTRY. */
                            entryCardLocation = txtSource.Text.IndexOf("$ENTRY");
                            if ((entryCardLocation > 0) &&
                                (txtSource.GetFirstCharIndexFromLine(i) > entryCardLocation))
                                    enableColor = false;

                            /* Check for character strings. */
                            if (txtSource.Lines[i].IndexOf('\'') > 14)
                            {
                                endOfOperand = txtSource.Lines[i].IndexOf('\'',
                                    txtSource.Lines[i].IndexOf('\'') + 1);

                                if (endOfOperand < 1 || txtSource.Lines[i].IndexOf(
                                    ' ', 15, txtSource.Lines[i].IndexOf('\'') - 15) > 0)
                                    endOfOperand = 15;
                            }

                            else
                                endOfOperand = 15;

                            /* Find if there is a space after the last field. */
                            try
                            {
                                lineSpaceIndex = txtSource.Lines[i].IndexOf(' ', endOfOperand,
                                    (txtSource.Lines[i].TrimEnd().Length - 1) - endOfOperand);

                                if (lineSpaceIndex >= 0)
                                {
                                    highlightStart = lineStartIndex + lineSpaceIndex + 1;
                                    eolComment = true;
                                }
                            }

                            catch
                            {
                                for (int j = txtSource.Lines[i].Length - 1; j >= endOfOperand; j--)
                                {
                                    if (txtSource.Lines[i][j] == ' ')
                                    {
                                        lineSpaceIndex = j;
                                        highlightStart = lineStartIndex + j + 1;
                                        eolComment = true;
                                    }
                                }
                            }

                            /* Highlight text following first space after last field. */
                            if (eolComment && enableColor)
                            {
                                txtSource.Select(highlightStart,
                                    txtSource.Lines[i].Length - lineSpaceIndex);
                                txtSource.SelectionColor = commentColor;
                            }

                            eolComment = false;
                        }

                        catch
                        {
                            MessageBox.Show("Highlighting error.");
                        }
                    }
                }
            }

            /* Set the cursor to given location and set text color to default. */
            txtSource.Select(position, 0);
            txtSource.SelectionColor = defaultSourceColor;
            activateTextChanged = true;
        }

        /******************************************************************************************
         * 
         * Name:        FormatLineText
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will check the current line of the editor and highlight certain
         *              syntax strings.
         *  
         *****************************************************************************************/
        private void FormatLineText()
        {
            /* Temporary variables for finding syntax to highlight. */
            bool checkColors = false;
            bool colorChanged = false;
            bool eolComment = false;
            bool enableColor = true;
            char firstChar = 'x';
            int endOfOperand = 0;
            int entryCardLocation = 0;
            int currentLineIndex = cursorColumn;
            int highlightStart = 0;
            int lastCharIndex = 1;
            int lineSpaceIndex = 0;
            string currentLine = "";

            activateTextChanged = false;

            /* Get the current line. */
            if (txtSource.Text != "")
            {
                try
                {
                    currentLine = txtSource.Lines[cursorLine];
                }

                catch
                {
                    return;
                }

                /* Find the first character of the current line. */
                if (currentLine != "")
                    firstChar = currentLine[0];
            }

            /* Highlight comment lines. */
            if (firstChar == '*')
            {
                txtSource.Select(cursorLineStartIndex, currentLine.Length);
                txtSource.SelectionColor = commentColor;
                colorChanged = true;
            }

            else
            {
                /* Highlight end of line comments. */
                if (currentLine.Length > 16)
                {
                    eolComment = false;

                    try
                    {
                        /* Don't highlight after $ENTRY. */
                        entryCardLocation = txtSource.Text.IndexOf("$ENTRY");
                        if ((entryCardLocation > 0) && 
                            (txtSource.GetFirstCharIndexFromLine(cursorLine) > entryCardLocation))
                                enableColor = false;

                        /* Check for character strings. */
                        if (txtSource.Lines[cursorLine].IndexOf('\'') > 14)
                        {
                            endOfOperand = txtSource.Lines[cursorLine].IndexOf('\'',
                                txtSource.Lines[cursorLine].IndexOf('\'') + 1);

                            if (endOfOperand < 1 || txtSource.Lines[cursorLine].IndexOf(
                                ' ', 15, txtSource.Lines[cursorLine].IndexOf('\'') - 15) > 0)
                                    endOfOperand = 15;
                        }

                        else if (txtSource.Lines[cursorLine].IndexOf(' ', 15) > 1)
                            endOfOperand = txtSource.Lines[cursorLine].IndexOf(' ', 15) - 1;

                        else
                            endOfOperand = 15;

                        /* All text before eol comment should be black. */
                        try
                        {
                            txtSource.Select(cursorLineStartIndex, endOfOperand + 1);
                        }

                        catch
                        {
                            txtSource.Select(cursorLineStartIndex, endOfOperand);
                        }

                        txtSource.SelectionColor = defaultSourceColor;

                        /* Find last non-space character. */
                        try
                        {
                            lastCharIndex = currentLine.TrimEnd().Length - 1;
                        }

                        catch
                        {
                            for (int i = 14; i < currentLine.Length; i++)
                            {
                                if (currentLine[i] != ' ')
                                    lastCharIndex = i;
                            }
                        }
                        
                        /* Find if there is a space after the last field. */
                        try
                        {
                            lineSpaceIndex = currentLine.IndexOf(' ', endOfOperand,
                                lastCharIndex - endOfOperand);

                            if (lineSpaceIndex >= 0)
                            {
                                highlightStart = cursorLineStartIndex + lineSpaceIndex + 1;
                                eolComment = true;
                            }
                        }

                        catch
                        {
                            for (int i = lastCharIndex - 1; i >= endOfOperand + 1; i--)
                            {
                                if (currentLine[i] == ' ')
                                {
                                    lineSpaceIndex = i;
                                    highlightStart = cursorLineStartIndex + i + 1;
                                    eolComment = true;
                                }
                            }
                        }

                        /* If so, highlight the rest of the line. */
                        if (eolComment && enableColor)
                        {
                            txtSource.Select(highlightStart, lastCharIndex - lineSpaceIndex);
                            txtSource.SelectionColor = commentColor;
                            colorChanged = true;
                        }

                        else
                            checkColors = true;
                    }

                    catch
                    {
                        MessageBox.Show("Highlighting error.");
                    }
                }

                else
                    checkColors = true;

                /* Make sure no comment color is where it shouldn't be. */
                if (checkColors)
                {
                    txtSource.Select(cursorLineStartIndex, currentLine.Length);
                    txtSource.SelectionColor = defaultSourceColor;
                    colorChanged = true;
                }
            }

            /* Change text color back to default afterwards. */
            if (colorChanged)
            {
                txtSource.Select(cursorLineStartIndex + currentLineIndex, 0);
                txtSource.SelectionColor = defaultSourceColor;
            }

            activateTextChanged = true;
        }

        /******************************************************************************************
         * 
         * Name:        FormatText
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The line numbers to start and stop checking formatting and the index 
         *              location to place the cursor after formatting (integer).
         * Return:      N/A
         * Description: This method will check the specified section of the source editor and 
         *              highlight certain syntax strings.
         *  
         *****************************************************************************************/
        private void FormatText(int start, int numLinesToCheck, int cursorPosition)
        {
            /* Temporary variables to assist in highlighting syntax. */
            bool enableColor = true;
            bool eolComment = false;
            int endOfOperand = 0;
            int entryCardLocation = 0;
            int highlightStart = 16;
            int lineOverflow;
            int lineSpaceIndex = 16;
            int lineStartIndex = 0;
            string temp = txtSource.Lines[cursorLine];

            activateTextChanged = false;

            try
            {
                /* Determine if the line goes past line 79. */
                lineOverflow = cursorLineLength - 78;
            }

            catch
            {
                lineOverflow = 0;
            }

            /* Loop through given lines and set syntax highlighting accordingly. */
            for (int i = start; (i < txtSource.Lines.Length) && (i < start + numLinesToCheck); i++)
            {
                if (txtSource.Lines[i].Length > 0)
                {
                    lineOverflow = txtSource.Lines[i].Length - 78;
                    lineStartIndex = txtSource.GetFirstCharIndexFromLine(i);
                    txtSource.Select(lineStartIndex, txtSource.Lines[i].Length);
                    txtSource.SelectionColor = defaultSourceColor;

                    /* Don't allow lines to go past column 79. */
                    if (txtSource.Lines[i].Length > 78)
                    {
                        txtSource.Select(lineStartIndex + 78, lineOverflow);
                        txtSource.SelectedText = "";
                        UpdateCursorLocation();
                        cursorPosition -= lineOverflow;
                    }

                    /* Highlight comments of line starting with an asterisk. */
                    if (txtSource.Lines[i][0] == '*')
                    {
                        txtSource.Select(lineStartIndex, txtSource.Lines[i].Length);
                        txtSource.SelectionColor = commentColor;
                    }

                    /* Highlight end of line comments. */
                    else if (txtSource.Lines[i].Length > 16)
                    {
                        try
                        {
                            /* Don't highlight after $ENTRY. */
                            entryCardLocation = txtSource.Text.IndexOf("$ENTRY");
                            if ((entryCardLocation > 0) &&
                                (txtSource.GetFirstCharIndexFromLine(i) > entryCardLocation))
                                    enableColor = false;

                            /* Account for spaces in operands. */
                            if ((txtSource.Lines[i].Length > 17) &&
                                (txtSource.Lines[i].Substring(15, 2) == "C'"))
                            {
                                endOfOperand = txtSource.Lines[i].IndexOf('\'', 17);

                                if (endOfOperand < 1)
                                    endOfOperand = 15;
                            }

                            else if (txtSource.Lines[i].Substring(9, 7) == "TITLE '")
                            {
                                endOfOperand = txtSource.Lines[i].IndexOf('\'', 16);

                                if (endOfOperand < 1)
                                    endOfOperand = 15;
                            }

                            else if (txtSource.Lines[i].IndexOf("C'") > 14)
                            {
                                endOfOperand = txtSource.Lines[i].IndexOf('\'',
                                    txtSource.Lines[i].IndexOf("C'") + 2);

                                if (endOfOperand < 1)
                                    endOfOperand = 15;
                            }

                            else
                                endOfOperand = 15;

                            /* Find if there is a space after the last field. */
                            for (int j = txtSource.Lines[i].Length - 1; j >= endOfOperand; j--)
                            {
                                if (txtSource.Lines[i][j] == ' ')
                                {
                                    lineSpaceIndex = j;
                                    highlightStart = lineStartIndex + j + 1;
                                    eolComment = true;
                                }
                            }

                            /* Highlight text following first space after last field. */
                            if (eolComment && enableColor)
                            {
                                txtSource.Select(highlightStart,
                                    txtSource.Lines[i].Length - lineSpaceIndex);
                                txtSource.SelectionColor = commentColor;
                            }

                            eolComment = false;
                        }

                        catch
                        {
                            MessageBox.Show("Highlighting error.");
                        }
                    }
                }
            }

            /* Set the cursor to given location and set text color to default. */
            txtSource.Select(cursorPosition, 0);
            txtSource.SelectionColor = defaultSourceColor;
            activateTextChanged = true;
        }

        /******************************************************************************************
         * 
         * Name:        MainFormClosing
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User closes the main form.
         * Return:      N/A
         * Description: This method will open an "are you sure" box if closing while unsaved.
         *  
         *****************************************************************************************/
        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            /* Don't ask if it's a new and empty project. */
            if (!((projectExists == false) && (txtSource.Text == "")))
            {
                /* This will pop up the "are you sure" box if the save is not current. */
                if (isSaved == false)
                {
                    DialogResult choice = MessageBox.Show("Your source code contains unsaved " +
                        "changes. Save before closing?", "Save Project", MessageBoxButtons.YesNoCancel);

                    if (choice == DialogResult.Yes)
                        SaveProject();

                    else if (choice == DialogResult.Cancel)
                        e.Cancel = true;
                }
            }

            /* Unlock window if locked, and make sure outform closes. */
            LockWindowUpdate(IntPtr.Zero);
            debugMode = false;
            outForm.SetDebug(false);
            outForm.Close();
        }

        /******************************************************************************************
         * 
         * Name:        MenuAssembleAssembleClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the Assemble method when the menu option is clicked.             
         *  
         *****************************************************************************************/
        private void MenuAssembleAssembleClick(object sender, EventArgs e)
        {
            Assemble();
        }

        /******************************************************************************************
         * 
         * Name:        MenuAssembleDebugClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the AssembleDebug method when the menu option is
         *              clicked.
         *  
         *****************************************************************************************/
        private void MenuAssembleDebugClick(object sender, EventArgs e)
        {
            AssembleDebug();
        }

        /******************************************************************************************
         * 
         * Name:        MenuAssembleFinalRunClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the AssembleFinalRun method when the menu option is
         *              clicked.
         *  
         *****************************************************************************************/
        private void MenuAssembleFinalRunClick(object sender, EventArgs e)
        {
            AssembleFinalRun();
        }

        /******************************************************************************************
         * 
         * Name:        MenuDebugNextClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will step to next instruction when debugging.
         *  
         *****************************************************************************************/
        private void MenuDebugNextClick(object sender, EventArgs e)
        {
            DebugNext();
        }

        /******************************************************************************************
         * 
         * Name:        MenuDebugRunToEndClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will run to the end of the program when debugging.
         *  
         *****************************************************************************************/
        private void MenuDebugRunToEndClick(object sender, EventArgs e)
        {
            DebugRunToEnd();
        }

        /******************************************************************************************
         * 
         * Name:        MenuDebugRunToLocation
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will run to a specified location of the program when debugging.
         *  
         *****************************************************************************************/
        private void MenuDebugRunToLocation(object sender, EventArgs e)
        {
            DebugRunToLocation();
        }

        /******************************************************************************************
         * 
         * Name:        MenuDebugStopClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method stop debugging.
         *  
         *****************************************************************************************/
        private void MenuDebugStopClick(object sender, EventArgs e)
        {
            DebugStop();
        }

        /******************************************************************************************
         * 
         * Name:        MenuEditCopyClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will place the selected text on the clipboard.
         *  
         *****************************************************************************************/
        private void MenuEditCopyClick(object sender, EventArgs e)
        {
            txtSource.Copy();
        }

        /******************************************************************************************
         * 
         * Name:        MenuEditCutClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will delete the selected text from the source box and place it
         *              on the clipboard.
         *  
         *****************************************************************************************/
        private void MenuEditCutClick(object sender, EventArgs e)
        {
            txtSource.Cut();
        }

        /*****************************************************************************************
         * 
         * Name:        MenuEditPasteClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will paste contents of the clipboard to the cursor location.              
         *  
         *****************************************************************************************/
        private void MenuEditPasteClick(object sender, EventArgs e)
        {
            /* Do not allow pasting over a selection, or else it confuses syntax highlighting. */
            if ((txtSource.SelectionLength < 1) && (Clipboard.ContainsText()
                    && (cursorColumn < 78)))
            {
                /* Variables used to tell where to check for highlighting. */
                int lineCount = txtSource.Lines.Length;
                int position = 0;
                int startLine = cursorLine;

                if (lineCount > 0)
                    lineCount--;

                /* Convert text in the clipboard to uppercase and paste it into the editor. */
                Clipboard.SetText(Clipboard.GetText().ToUpper());
                txtSource.Paste();
                
                /* Format text to standards. */
                if (Clipboard.GetText().Contains("\t"))
                    RemoveTabs();

                /* Update variables after pasting and format the pasted text. */
                lineCount = txtSource.Lines.Length - lineCount;
                position = txtSource.GetFirstCharIndexFromLine(cursorLine) + cursorColumn;

                /* Apply syntax highlighting. */
                LockWindowUpdate(txtSource.Handle);
                FormatText(startLine, lineCount, position);
                LockWindowUpdate(IntPtr.Zero);
            }
        }

        /******************************************************************************************
         * 
         * Name:        MenuEditReplaceClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will open the search and replace form when the menu option is 
         *              clicked.
         *  
         *****************************************************************************************/
        private void MenuEditReplaceClick(object sender, EventArgs e)
        {
            activateTextChanged = false;
            FindReplaceForm find = new FindReplaceForm(true, txtSource);
            find.Show();

            activateTextChanged = true;
        }

        /******************************************************************************************
         * 
         * Name:        MenuEditSearchClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will open the search form when the menu option is clicked.
         *  
         *****************************************************************************************/
        private void MenuEditSearchClick(object sender, EventArgs e)
        {
            FindReplaceForm find = new FindReplaceForm(false, txtSource);
            find.Show();
        }
        
        /******************************************************************************************
         * 
         * Name:        MenuFileExitClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will exit ASSIST/UNA if the project's save is current, if not
         *              it will ask to save first.
         *  
         *****************************************************************************************/
        private void MenuFileExitClick(object sender, EventArgs e)
        {
            /* The MainFormClosing method will handle the "are you sure" box. */
            this.Close();
        }

        /******************************************************************************************
         * 
         * Name:        MenuFileExportClick
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will export the contents of the source code editor into 
         *              either a .txt or .rtf file.
         *  
         *****************************************************************************************/
        private void MenuFileExportClick(object sender, EventArgs e)
        {
            /* Set the options and settings for the export dialog. */
            DialogResult choice;
            SaveFileDialog dlgExport = new SaveFileDialog();
            dlgExport.DefaultExt = ".txt";
            dlgExport.FileName = fileName + ".txt";
            dlgExport.Filter = "Text Files |*.txt | Rich Text Format |*.rtf";
            dlgExport.Title = "Save Project As";
            choice = dlgExport.ShowDialog();

            if (choice == DialogResult.OK)
            {
                /* Write source code to file. */
                StreamWriter unaFile = new StreamWriter(@dlgExport.FileName);
                unaFile.Write(txtSource.Text);
                unaFile.Close();
            }
        }

        /******************************************************************************************
         * 
         * Name:        MenuFileImportClick
         * 
         * Author(s):   Drew Aaron
         *              Clay Boren
         *              Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will import the contents of a txt file into the source editor.
         *  
         *****************************************************************************************/
        private void MenuFileImportClick(object sender, EventArgs e)
        {
            DialogResult choice;

            /* Check to see if the project save is current. */
            if (isSaved == false && txtSource.Text != "")
            {
                choice = MessageBox.Show("Your source code contains unsaved changes. " +
                "Would you like to save first?", "Save Project", MessageBoxButtons.YesNoCancel);

                if (choice == DialogResult.Yes)
                    SaveProject();

                else if (choice == DialogResult.Cancel)
                    return;
            }

            /* The following code will always execute as long as cancel is not pressed. */

            /* Open file dialog and set filter. */
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Text Files (*.txt)|*.txt|Rich Text Files (*.rtf)|*.rtf";
            choice = dlgOpen.ShowDialog();

            if (choice == DialogResult.OK)
            {
                /* Update status label and freeze window. */
                UpdateStatusLabel("Loading Import...");
                LockWindowUpdate(txtSource.Handle);
                
                /* Set file name and directory data members. */
                fileName = "Unsaved Project";
                directory = "";

                /* Read in the lines and set the data members. */
                StreamReader unaFileReader = new StreamReader(dlgOpen.FileName);

                /* Fill the source editor with saved source code. */
                txtSource.Text = unaFileReader.ReadToEnd();

                unaFileReader.Close();

                /* Format the text and check for syntax highlighting. */
                txtSource.Text = txtSource.Text.ToUpper();
                RemoveTabs();

                /* Set all text to default color initially. */
                txtSource.Select(0, txtSource.Text.Length);
                txtSource.SelectionColor = defaultSourceColor;
                txtSource.Select(0, 0);

                /* Add syntax highlighting and unfreeze window, and set status label. */
                if (txtSource.Text != "")
                {
                    RemoveOverflow();
                    FormatAllText(0);
                }

                LockWindowUpdate(IntPtr.Zero);
                UpdateStatusLabel("Text Imported.");
                
                /* Update title bar. */
                this.Text = "ASSIST/UNA - Unsaved Project*";

                /* Project does not exist with an import. */
                projectExists = false;
                isSaved = false;
            }
        }

        /******************************************************************************************
         * 
         * Name:        MenuFileNewClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the NewProject method to create a new project from 
         *              the menu option.
         *  
         *****************************************************************************************/
        private void MenuFileNewClick(object sender, EventArgs e)
        {
            NewProject();
        }

        /******************************************************************************************
         * 
         * Name:        MenuFileOpenClick
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the OpenProject method to open an existing project
         *              when clicking the menu option.
         *  
         *****************************************************************************************/
        private void MenuFileOpenClick(object sender, EventArgs e)
        {
            OpenProject();
        }

        /******************************************************************************************
         * 
         * Name:        MenuFilePrintClick
         * 
         * Author(s):   Travis Hunt
         *              Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the PrintSource method when the menu option is 
         *              clicked.
         *  
         *****************************************************************************************/
        private void MenuFilePrintClick(object sender, EventArgs e)
        {
            PrintSource();
        }

        /******************************************************************************************
         * 
         * Name:        MenuFileSaveAsClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the saveProjectAs method.
         *  
         *****************************************************************************************/
        private void MenuFileSaveAsClick(object sender, EventArgs e)
        {
            SaveProjectAs();
        }

        /******************************************************************************************
         * 
         * Name:        MenuFileSaveClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the saveProject method if the project exists, else it
         *              will call the saveProjectAs method.
         *  
         *****************************************************************************************/
        private void MenuFileSaveClick(object sender, EventArgs e)
        {
            SaveProject();
        }

        /******************************************************************************************
         * 
         * Name:        MenuHelpAboutClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will open the about form. 
         *  
         *****************************************************************************************/
        private void MenuHelpAboutClick(object sender, EventArgs e)
        {
            AboutForm abt = new AboutForm();
            abt.SetBackColorAccent(backColorAccent);
            abt.SetBackColorMain(backColorMain);
            abt.SetBackColorMain2(backColorMain2);
            abt.SetFormLabelTextColor(formLabelTextColor);
            abt.SetFormTextColor(formTextColor);
            abt.ShowDialog();
        }

        /******************************************************************************************
         * 
         * Name:        MenuHelpOnlineClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will open the user's default web browser to the online help 
         *              manual. 
         *  
         *****************************************************************************************/
        private void MenuHelpOnlineClick(object sender, EventArgs e)
        {
            try
            {
                int numLoops = 0;
                string HelpPath = System.Reflection.Assembly.GetEntryAssembly().Location;

                while (!HelpPath.EndsWith("OnlineHelp.html"))
                {
                    if (File.Exists(HelpPath + "\\OnlineHelp.html"))
                        HelpPath += "\\OnlineHelp.html";

                    else if (Directory.Exists(HelpPath + "\\User Manual\\"))
                        HelpPath += "\\User Manual\\";

                    else
                        HelpPath = HelpPath.Substring(0, HelpPath.LastIndexOf('\\'));

                    if (numLoops++ >= 30)
                        throw new FileNotFoundException();
                }

                Process.Start(HelpPath);
            }

            catch
            {
                MessageBox.Show("Online Manual not found.", "Error");
            }
        }

        /******************************************************************************************
         * 
         * Name:        MenuToolsOptionsClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will open the options menu and apply the given options to the
         *              main program. 
         *  
         *****************************************************************************************/
        private void MenuToolsOptionsClick(object sender, EventArgs e)
        {
            OptionsForm opt = new OptionsForm();

            /* Set textbox options to correct values. */
            opt.SetMaxLines(maxLines);
            opt.SetMaxInstructions(maxInstructions);
            opt.SetMaxPages(maxPages);
            opt.SetMaxSize(maxSize);
            opt.SetPathPRT(pathPRT);
            opt.SetBackColorAccent(backColorAccent);
            opt.SetBackColorMain(backColorMain);
            opt.SetBackColorMain2(backColorMain2);
            opt.SetCommentColor(commentColor);
            opt.SetFormLabelTextColor(formLabelTextColor);
            opt.SetFormTextColor(formTextColor);


            opt.ShowDialog();

            /* Get the new values from options form and update colors. */
            maxLines = opt.GetMaxLines();
            maxInstructions = opt.GetMaxInstructions();
            maxPages = opt.GetMaxPages();
            maxSize = opt.GetMaxSize();
            pathPRT = opt.GetPathPRT();
            backColorAccent = opt.GetBackColorAccent();
            backColorMain = opt.GetBackColorMain();
            backColorMain2 = opt.GetBackColorMain2();
            commentColor = opt.GetCommentColor();
            formLabelTextColor = opt.GetFormLabelTextColor();
            formTextColor = opt.GetFormTextColor();
            UpdateColors();

            if (opt.commentColorChanged())
            {
                LockWindowUpdate(txtSource.Handle);
                FormatAllText(cursorLineStartIndex + cursorColumn);
                LockWindowUpdate(IntPtr.Zero);
            }

            /* Save is no longer current. */
            isSaved = false;
            this.Text = "ASSIST/UNA - " + fileName + "*";
        }

        /******************************************************************************************
         * 
         * Name:        MenuToolsOutputClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will open the output form when the menu option is clicked.
         *  
         *****************************************************************************************/
        private void MenuToolsOutputClick(object sender, EventArgs e)
        {
            if (projectExists){
                if (debugMode)
                {
                    /* Debug mode uses outForm, which never closes until main closes. */
                    outForm.Show();
                    outForm.BringToFront();
                }
                else
                    ShowOutput();
            }

            else
            {
                MessageBox.Show("Project must be assembled first.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /******************************************************************************************
         * 
         * Name:        MenuToolsViewPRTClick
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the ViewPRT method to open the .PRT file when the 
         *              menu option is clicked. 
         *  
         *****************************************************************************************/
        private void MenuToolsViewPRTClick(object sender, EventArgs e)
        {
            ViewPRT();
        }

        /******************************************************************************************
         * 
         * Name:        NewProject
         * 
         * Author(s):   Travis Hunt
         *              Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will restore all defaults if the project has been saved. If it
         *              has not been saved, then it will ask to save first.
         *  
         *****************************************************************************************/
        private void NewProject()
        {
            /* Don't ask if it's a new and empty project. */
            if ((projectExists == false) && (txtSource.Text == ""))
                isSaved = true;

            /* Make sure that the project is saved before making a new project. */
            if (isSaved == false)
            {
                DialogResult choice = MessageBox.Show("Your source code contains unsaved changes."
                    + " Would you like to save first?", "Save?", MessageBoxButtons.YesNoCancel);

                if (choice == DialogResult.Yes)
                    SaveProject();

                else if (choice == DialogResult.Cancel)
                    return;
            }

            /* This code will always be executed, unless the cancel button is pressed. */
            RestoreDefaults();
        }

        /******************************************************************************************
         * 
         * Name:        OpenProject
         * 
         * Author(s):   Travis Hunt
         *              Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will open a .una file and load its settings and text into 
         *              ASSIST/UNA.
         *  
         *****************************************************************************************/
        private void OpenProject()
        {
            DialogResult choice;

            /* Check to see if the project save is current. */
            if (isSaved == false && txtSource.Text != "")
            {
                choice = MessageBox.Show("Your source code contains unsaved changes. " +
                "Would you like to save first?", "Save Project", MessageBoxButtons.YesNoCancel);

                if (choice == DialogResult.Yes)
                    SaveProject();

                else if (choice == DialogResult.Cancel)
                    return;
            }

            /* The following code will always execute as long as cancel is not pressed. */

            /* Open file dialog and set filter. */
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "ASSIST/UNA Files (*.una)|*.una";
            choice = dlgOpen.ShowDialog();

            if (choice == DialogResult.OK)
            {
                /* Update cursor, status label and freeze window. */
                Cursor.Current = Cursors.AppStarting;
                UpdateStatusLabel("Loading Project...");
                LockWindowUpdate(txtSource.Handle);

                /* Set file name and directory data members. */
                fileName = Path.GetFileNameWithoutExtension(dlgOpen.FileName);
                directory = Path.GetDirectoryName(dlgOpen.FileName);

                /* Read in the lines and set the data members. */
                StreamReader unaFileReader = new StreamReader(dlgOpen.FileName);
                unaFileReader.ReadLine();

                try
                {
                    maxInstructions = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                    if (maxInstructions > 9999)
                        maxInstructions = 9999;
                }

                catch
                {
                    maxInstructions = DEFAULT_MAX_INSTRUCTIONS;
                }

                try
                {
                    maxLines = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                    if (maxLines > 9999)
                        maxLines = 9999;
                }

                catch
                {
                    maxLines = DEFAULT_MAX_LINES;
                }

                try
                {
                    maxPages = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                    if (maxPages > 9999)
                        maxPages = 9999;
                }
                catch

                {
                    maxPages = DEFAULT_MAX_PAGES;
                }

                try
                {
                    maxSize = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                    if (maxSize > 9999)
                        maxSize = 9999;
                }

                catch
                {
                    maxSize = DEFAULT_MAX_SIZE;
                }

                try
                {
                    pathPRT = unaFileReader.ReadLine().Substring(1);
                }

                catch
                {
                    pathPRT = "";
                }

                unaFileReader.ReadLine();

                /* Fill the source editor with saved source code. */
                txtSource.Text = unaFileReader.ReadToEnd();

                unaFileReader.Close();

                /* Format the text and check for syntax highlighting. */
                txtSource.Text = txtSource.Text.ToUpper();
                RemoveTabs();

                /* Set all text to default color initially. */
                txtSource.Select(0, txtSource.Text.Length);
                txtSource.SelectionColor = defaultSourceColor;
                txtSource.Select(0, 0);

                LockWindowUpdate(txtSource.Handle);

                /* Add syntax highlighting and unfreeze window, and set cursor and status label. */
                FormatAllText(0);
                LockWindowUpdate(IntPtr.Zero);
                Cursor.Current = Cursors.Arrow;
                UpdateStatusLabel("Project Loaded.");

                /* Update title bar. */
                this.Text = "ASSIST/UNA - " + fileName;

                /* Project now exists and save any reformatting. */
                projectExists = true;
                SaveProject();
            }
        }

        /******************************************************************************************
         * 
         * Name:        OpenProject (overloaded)
         * 
         * Author(s):   Travis Hunt
         *              Drew Aaron
         *              
         * Input:       Path to the file to open (string).
         * Return:      N/A
         * Description: This method will open a .una file and load its settings and text into 
         *              ASSIST/UNA.
         *  
         *****************************************************************************************/
        private void OpenProject(string filePath)
        {
            directory = Path.GetDirectoryName(filePath);
            fileName = Path.GetFileNameWithoutExtension(filePath);

            /* Update cursor, status label and freeze window. */
            Cursor.Current = Cursors.AppStarting;
            UpdateStatusLabel("Loading Project...");
            LockWindowUpdate(txtSource.Handle);

            /* Set file name and directory data members. */
            directory = Path.GetDirectoryName(filePath);
            fileName = Path.GetFileNameWithoutExtension(filePath);

            /* Read in the lines and set the data members. */
            StreamReader unaFileReader = new StreamReader(@filePath);
            unaFileReader.ReadLine();

            try
            {
                maxInstructions = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                if (maxInstructions > 9999)
                    maxInstructions = 9999;
            }

            catch
            {
                maxInstructions = DEFAULT_MAX_INSTRUCTIONS;
            }

            try
            {
                maxLines = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                if (maxLines > 9999)
                    maxLines = 9999;
            }

            catch
            {
                maxLines = DEFAULT_MAX_LINES;
            }

            try
            {
                maxPages = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                if (maxPages > 9999)
                    maxPages = 9999;
            }
            catch
            {
                maxPages = DEFAULT_MAX_PAGES;
            }

            try
            {
                maxSize = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                if (maxSize > 9999)
                    maxSize = 9999;
            }

            catch
            {
                maxSize = DEFAULT_MAX_SIZE;
            }

            try
            {
                pathPRT = unaFileReader.ReadLine().Substring(1);
            }

            catch
            {
                pathPRT = "";
            }

            unaFileReader.ReadLine();

            /* Fill the source editor with saved source code. */
            txtSource.Text = unaFileReader.ReadToEnd();

            unaFileReader.Close();

            /* Format the text and check for ivalid characters. */
            txtSource.Text = txtSource.Text.ToUpper();
            RemoveTabs();

            /* Set all text to default color initially. */
            txtSource.Select(0, txtSource.Text.Length);
            txtSource.SelectionColor = defaultSourceColor;
            txtSource.Select(0, 0);

            LockWindowUpdate(txtSource.Handle);
            /* Add syntax highlighting and unfreeze window, and set cursor and status label. */
            FormatAllText(0);
            LockWindowUpdate(IntPtr.Zero);
            Cursor.Current = Cursors.Arrow;
            UpdateStatusLabel("Project Loaded.");

            /* Update title bar. */
            this.Text = "ASSIST/UNA - " + fileName;

            /* Project now exists and the save is current. */
            projectExists = true;
            isSaved = true;
        }

        /******************************************************************************************
         * 
         * Name:        PrintDocumentOnPrintPage
         * 
         * Author(s):   Clay Boren
         *              Michael Beaver
         *              Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will format the source code to a printable format.
         *  
         *****************************************************************************************/
        private void PrintDocumentOnPrintPage(object sender, PrintPageEventArgs e)
        {

            int charactersOnPage = 0;
            int linesPerPage = 0;

            /* Sets the value of charactersOnPage to the number of characters  
               of printToString that will fit within the bounds of the page. */
            e.Graphics.MeasureString(totalSourceString, txtSource.Font,
                e.MarginBounds.Size, StringFormat.GenericDefault,
                out charactersOnPage, out linesPerPage);

            try
            {
                pageToPrint = stringToPrint;

                e.Graphics.DrawString(pageToPrint, txtSource.Font, Brushes.Black,
                    e.MarginBounds, StringFormat.GenericDefault);

                if (stringToPrint.Length > CHARACTERS_PER_PAGE)
                {
                    e.HasMorePages = true;
                    stringToPrint = stringToPrint.Substring(CHARACTERS_PER_PAGE + 1);
                }

                else if (pageEnumerator.MoveNext())
                {
                    e.HasMorePages = true;
                    stringToPrint = pageEnumerator.Current;
                }

                else
                    e.HasMorePages = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print Error");
            }
        }

        /******************************************************************************************
         * 
         * Name:        PrintSource
         * 
         * Author(s):   Clay Boren
         *              Travis Hunt
         *              Drew Aaron
         *              Michael Beaver
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will print the source code.
         *  
         *****************************************************************************************/
        private void PrintSource()
        {
            ReadFile();

            string[] pages = totalSourceString.Split('\f');
            List<string> pageList = new List<string>(pages);
            PrintDialog printOptions = new PrintDialog();

            printDoc.PrinterSettings = printOptions.PrinterSettings;

            try
            {
                if (printOptions.ShowDialog() == DialogResult.OK)
                {
                    printDoc.PrinterSettings = printOptions.PrinterSettings;
                    printDoc.DefaultPageSettings.Landscape = false;

                    pageEnumerator = pageList.GetEnumerator();
                    pageEnumerator.MoveNext();
                    stringToPrint = pageEnumerator.Current;

                    printDoc.Print();
                }
            }

            catch
            {
                MessageBox.Show("Printing error. Check printing drivers.", "Printing error.");
            }
        }

        /******************************************************************************************
         * 
         * Name:        ReadFile
         * 
         * Author(s):   Clay Boren
         *              Drew Aaron
         *              Michael Beaver
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method writes the text from txtSource to a temporary text file.
         *  
         *****************************************************************************************/
        private void ReadFile()
        {
            int count = 0;
            int pageBreak = 0;
            string docPath = @Path.GetTempFileName();
            
            printDoc.DocumentName = docPath;

            using (StreamWriter writer = File.CreateText(docPath))
            {
                for (int i = 0; i < txtSource.Lines.Length; i++)
                {
                    writer.WriteLine(txtSource.Lines[i]);

                    if (i > 1 && pageBreak % 56 == 1)
                    {
                        if (count % 29 == 0 && i + 10 < txtSource.Lines.Length)
                            writer.WriteLine("\f");
                        pageBreak -= 2;
                        count++;
                    }
                    pageBreak++;
                }
            }

            try
            {
                StreamReader reader = new StreamReader(docPath);
                totalSourceString = reader.ReadToEnd();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /******************************************************************************************
         * 
         * Name:        RemoveOverflow
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will cut off all lines longer than 79.
         *  
         *****************************************************************************************/
        private void RemoveOverflow()
        {
            for (int i = 0; i < txtSource.Lines.Length - 1; i++)
            {
                if (txtSource.Lines[i].Length > 79)
                {
                    txtSource.Select(txtSource.GetFirstCharIndexFromLine(i) + 79, 
                        txtSource.Lines[i].Length - 79);

                    txtSource.SelectedText = "";
                }

                if (i == txtSource.Lines.Length - 1)
                    txtSource.Select(0, 0);
            }
        }

        /******************************************************************************************
         * 
         * Name:        RemoveTabs
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will replace all tabs from source code with spaces.
         *  
         *****************************************************************************************/
        private void RemoveTabs()
        {
            int pos = txtSource.GetFirstCharIndexFromLine(cursorLine) + cursorColumn;
            txtSource.Text = txtSource.Text.Replace("\t", " ");
            txtSource.Select(pos, 0);
        }

        /******************************************************************************************
         * 
         * Name:        RestoreDefaults
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will restore all data members and display elements to their
         *              default state.
         *  
         *****************************************************************************************/
        private void RestoreDefaults()
        {
            isSaved = false;
            projectExists = false;
            debugMode = false;
            debugClicked = false;
            maxInstructions = DEFAULT_MAX_INSTRUCTIONS;
            maxLines = DEFAULT_MAX_LINES;
            maxPages = DEFAULT_MAX_PAGES;
            maxSize = DEFAULT_MAX_SIZE;
            directory = "";
            fileName = "Unsaved Project";
            pathPRT = "";
            identifier = "";
            outputText = "";
            outForm.Text = "";
            txtSource.Clear();
            this.Text = "ASSIST/UNA - Unsaved Project*";
            ClearDisplay();
        }

        /******************************************************************************************
         * 
         * Name:        SaveProject
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will overwrite the old version of the project with the new one.
         *  
         *****************************************************************************************/
        private void SaveProject()
        {
            /* Overwrite previous if the project exists. If not, open the dialog to save as. */
            if (projectExists)
            {
                try
                {
                    /* Set options into a string. */
                    string fileContents = "#\n#" + maxInstructions + "\n#" + maxLines + "\n#" 
                        + maxPages + "\n#" + maxSize + "\n#" + pathPRT + "\n#\n";

                    /* Set source text into the string. */
                    fileContents += txtSource.Text;

                    /* Write string to file. */
                    StreamWriter unaFile = new StreamWriter(directory + "\\" + fileName + ".una");
                    unaFile.Write(fileContents);
                    unaFile.Close();

                    /* The project is now saved. */
                    isSaved = true;
                    this.Text = "ASSIST/UNA - " + fileName;
                    UpdateStatusLabel("Saved.");
                }
                
                catch (FileLoadException)
                {
                    MessageBox.Show("Save file could not be loaded in order to be saved.", 
                        "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                catch (System.Security.SecurityException)
                {
                    MessageBox.Show("Save does not have permission to access save file. Is it "
                        + "open in another program?", "Save Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Save does not have permission to access save file. Is it "
                        + "open in another program?", "Save Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                catch (IOException)
                {
                    MessageBox.Show("Save cannot open save file. Is it open in another program? "
                        + "Currently using old save.", "Save Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                catch
                {
                    MessageBox.Show("Save Failed", "Save Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    
                }
            }

            else
                SaveProjectAs();
        }

        /******************************************************************************************
         * 
         * Name:        SaveProjectAs
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will open a save file dialog which allows the user to save the
         *              project as a .una file to a specified location.
         *  
         *****************************************************************************************/
        private void SaveProjectAs()
        {
            try 
            { 
                /* Save project to a specified location. */
                DialogResult choice = dlgSave.ShowDialog();

                if ((dlgSave.FileName != "") && (choice == DialogResult.OK))
                {
                    /* Set data members. */
                    directory = Path.GetDirectoryName(dlgSave.FileName);
                    fileName = Path.GetFileNameWithoutExtension(dlgSave.FileName);

                    /* Set options into a string. */
                    string fileContents = "#\n#" + maxInstructions + "\n#" + maxLines + "\n#" + 
                        maxPages + "\n#" + maxSize + "\n#" + pathPRT + "\n#\n"; ;

                    /* Set source text into the string. */
                    fileContents += txtSource.Text;

                    /* Write string to file. */
                    StreamWriter unaFile = new StreamWriter(@dlgSave.FileName);
                    unaFile.Write(fileContents);
                    unaFile.Close();

                    /* The project now exists and is saved. */
                    projectExists = true;
                    isSaved = true;

                    /* Set initial directory for next time. */
                    dlgSave.InitialDirectory = directory;

                    /* Update title bar. */
                    this.Text = "ASSIST/UNA - " + fileName;
                }
            }

            catch
            {
                MessageBox.Show("Save Failed", "Save Error", MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /******************************************************************************************
         * 
         * Name:        TsAssembleClick
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the Assemble method when the option is clicked from 
         *              the toolbar.
         *  
         *****************************************************************************************/
        private void TsAssembleClick(object sender, EventArgs e)
        {
            Assemble();
        }

        /******************************************************************************************
         * 
         * Name:        TsAssembleDebugClick
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the AssembleDebug method when the option is clicked
         *              from the toolbar.
         *  
         *****************************************************************************************/
        private void TsAssembleDebugClick(object sender, EventArgs e)
        {
            AssembleDebug();
        }

        /******************************************************************************************
         * 
         * Name:        TsAssembleFinalRunClick
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the AssembleFinalRun method when the option is 
         *              clicked from the toolbar.
         *  
         *****************************************************************************************/
        private void TsAssembleFinalRunClick(object sender, EventArgs e)
        {
            AssembleFinalRun();
        }

        /******************************************************************************************
         * 
         * Name:        TsDebugNextClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method step to next instruction when debugging.
         *  
         *****************************************************************************************/
        private void TsDebugNextClick(object sender, EventArgs e)
        {
            DebugNext();
        }

        /******************************************************************************************
         * 
         * Name:        TsDebugRunToEndClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method run through all instructions when debugging.
         *  
         *****************************************************************************************/
        private void TsDebugRunToEndClick(object sender, EventArgs e)
        {
            DebugRunToEnd();
        }

        /******************************************************************************************
         * 
         * Name:        TsDebugRunToLocationClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will run to the specifiec location when debugging.
         *  
         *****************************************************************************************/
        private void TsDebugRunToLocationClick(object sender, EventArgs e)
        {
            DebugRunToLocation();
        }

        /******************************************************************************************
         * 
         * Name:        TsDebugStopClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will stop debugging when clicked.
         *  
         *****************************************************************************************/
        private void TsDebugStopClick(object sender, EventArgs e)
        {
            DebugStop();
        }

        /******************************************************************************************
         * 
         * Name:        TsNewClick
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the NewProject method to create a new .una project
         *              when the tool bar option is clicked.
         *  
         *****************************************************************************************/
        private void TsNewClick(object sender, EventArgs e)
        {
            NewProject();
        }

        /******************************************************************************************
         * 
         * Name:        TsOpenClick
         * 
         * Author(s):   Clay Boren
         *              Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the OpenProject method to open a .una project when
         *              the toolbar option is clicked.
         *  
         *****************************************************************************************/
        private void TsOpenClick(object sender, EventArgs e)
        {
            OpenProject();
        }

        /******************************************************************************************
         * 
         * Name:        TsPrintClick
         * 
         * Author(s):   Clay Boren
         *              Travis Hunt
         *              Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will print the source code.
         *  
         *****************************************************************************************/
        private void TsPrintClick(object sender, EventArgs e)
        {
            PrintSource();
        }

        /******************************************************************************************
         * 
         * Name:        TsSaveAsClick
         * 
         * Author(s):   Clay Boren
         *              Travis Hunt
         *              Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the SaveProjectAs method which opens a save dialog.
         *              This handles when the toolbar option is clicked.
         *  
         *****************************************************************************************/
        private void TsSaveAsClick(object sender, EventArgs e)
        {
            SaveProjectAs();
        }

        /******************************************************************************************
         * 
         * Name:        TsSaveClick
         * 
         * Author(s):   Clay Boren
         *              Travis Hunt
         *              Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the SaveProject method to save the project.
         *              This handles when the toolbar option is clicked.
         *  
         *****************************************************************************************/
        private void TsSaveClick(object sender, EventArgs e)
        {
            SaveProject();
        }

        /******************************************************************************************
         * 
         * Name:        TsViewPRTClick
         * 
         * Author(s):   Drew Aaron
         *              Clay Boren
         *              Travis Hunt
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will call the ViewPRT method to open the .PRT file whenever the
         *              option in the toolbar is clicked.
         *  
         *****************************************************************************************/
        private void TsViewPRTClick(object sender, EventArgs e)
        {
            ViewPRT();
        }

        /******************************************************************************************
         * 
         * Name:        TxtSourceAddSpaces
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The number of spaces to add to the cursor location (int).
         * Return:      N/A
         * Description: This method will add spaces to the cursor selection in the source editor.
         *  
         *****************************************************************************************/
        private void TxtSourceAddSpaces(int numSpaces)
        {
            if (numSpaces == 1)
                txtSource.SelectedText = " ";

            else if (numSpaces == 2)
                txtSource.SelectedText = "  ";

            else if (numSpaces == 3)
                txtSource.SelectedText = "   ";

            else if (numSpaces == 4)
                txtSource.SelectedText = "    ";

            else if (numSpaces == 5)
                txtSource.SelectedText = "     ";

            else if (numSpaces == 6)
                txtSource.SelectedText = "      ";

            else if (numSpaces == 7)
                txtSource.SelectedText = "       ";

            else if (numSpaces == 8)
                txtSource.SelectedText = "        ";

            else if (numSpaces == 9)
                txtSource.SelectedText = "         ";
        }

        /******************************************************************************************
         * 
         * Name:        TxtSourceKeyDown
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User key press event.
         * Return:      N/A
         * Description: This method will perform various actions depending on which key is pressed.
         *  
         *****************************************************************************************/
        private void TxtSourceKeyDown(object sender, KeyEventArgs e)
        {
            /* Do not allow font size change shortcut. */
            e.SuppressKeyPress = e.Control && e.Shift &
                (e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.OemPeriod);

            /* Disable control + z. */
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.Z)
                e.Handled = true;

            /* Do not allow pulling up lines which are too long. */
            if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    if (txtSource.Lines.Length > cursorLine + 1)
                    {
                        if (cursorLineLength == cursorColumn && txtSource.Lines[
                            cursorLine].Length + txtSource.Lines[cursorLine + 1].Length >= 79)
                                e.Handled = true;
                    }
                }

                catch
                {
                    /* Perform normal actions if exception. */
                }
            }

            /* Do not allow pulling up lines which are too long. */
            if (e.KeyCode == Keys.Back)
            {
                try
                {
                    if ((cursorColumn == 0 && cursorLine != 0))
                    {
                        if (cursorLineLength + txtSource.Lines[cursorLine - 1].Length >= 79)
                                e.Handled = true;
                    }
                }

                catch
                {
                    /* Perform normal actions if exception. */
                }
            }
        }

        /******************************************************************************************
         * 
         * Name:        TxtSourceKeyPress
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User key press event.
         * Return:      N/A
         * Description: This method will capture key presses.
         *  
         *****************************************************************************************/
        private void TxtSourceKeyPress(object sender, KeyPressEventArgs e)
        {
            /* This will disable the normal tab key function. */
            if (e.KeyChar == '\t')
                e.Handled = true;

           /* Force uppercase characters. */
            e.KeyChar = Char.ToUpper(e.KeyChar);

            /* Do not allow typing past line 79. */
            if (((cursorColumn >= 79) || cursorLineLength >= 79) &&
                (e.KeyChar != '\n') && (e.KeyChar != Convert.ToChar(Keys.Back)))
                e.Handled = true;

            UpdateStatusLabel("Ready.");
        }

        /******************************************************************************************
         * 
         * Name:        TxtSourceKeyUp
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User key press event.
         * Return:      N/A
         * Description: This method will perform various actions depending on which key is pressed.
         *  
         *****************************************************************************************/
        private void TxtSourceKeyUp(object sender, KeyEventArgs e)
        {
            /* On arrow key release, set the new cursor location and update it in the labels */
            if ((e.KeyData == Keys.Up) || (e.KeyData == Keys.Down) ||
                (e.KeyData == Keys.Left) || (e.KeyData == Keys.Right))
                    UpdateCursorLocation();

            /* Set tab stops. */
            if ((e.KeyCode == Keys.Tab) && (Control.ModifierKeys != Keys.Shift))
            {
                /* On tab press, add enough spaces to get to the next tab stop line. */
                if (cursorColumn <= 8)
                    TxtSourceAddSpaces(9 - cursorColumn);

                else if (cursorColumn < 15)
                    TxtSourceAddSpaces(15 - cursorColumn);

                else if ((cursorColumn >= 15) && (cursorLineLength < 76))
                    TxtSourceAddSpaces(4 - ((cursorColumn + 1) % 4));
            }
        }

        /******************************************************************************************
         * 
         * Name:        TxtSourceMouseUp
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will set the line and column numbers to the new cursor location
         *              whenever the mouse button is released.
         *  
         *****************************************************************************************/
        private void TxtSourceMouseUp(object sender, MouseEventArgs e)
        {
            UpdateCursorLocation();
            UpdateStatusLabel("Ready.");
        }

        /******************************************************************************************
         * 
         * Name:        TxtSourceTextChanged
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              Reference 2
         *              
         * Input:       User event (changes text in txtSource).
         * Return:      N/A
         * Description: This method will perform various actions whenever the source code text is
         *              changed. 
         *              
         *              This includes: 
         *                  setting the isSaved boolean,
         *                  updating the line and column labels, and
         *                  formatting the text.            
         *  
         *****************************************************************************************/
        private void TxtSourceTextChanged(object sender, EventArgs e)
        {
            if (activateTextChanged)
            {
                /* Since the text has been changed, its save is no longer current. */
                isSaved = false;
                this.Text = "ASSIST/UNA - " + fileName + "*";

                /* Set the new cursor location and update it in the labels. */
                UpdateCursorLocation();

                /* Text formatting. */
                LockWindowUpdate(txtSource.Handle);
                FormatLineText();
                LockWindowUpdate(IntPtr.Zero);

                if (txtSource.Text == "")
                {
                    menuFilePrint.Enabled = false;
                    tsPrint.Enabled = false;
                }

                else
                {
                    menuFilePrint.Enabled = true;
                    tsPrint.Enabled = true;
                }
            }
        }

        /******************************************************************************************
         * 
         * Name:        UpdateCursorLocation
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will set the line and column labels to the current cursor 
         *              position.
         *  
         *****************************************************************************************/
        private void UpdateCursorLocation()
        {
            /* Update all relevant information of cursor(caret) location. */
            cursorLineStartIndex = txtSource.GetFirstCharIndexOfCurrentLine();

            cursorLine =
                txtSource.GetLineFromCharIndex(cursorLineStartIndex);

            cursorColumn = txtSource.SelectionStart -
                txtSource.GetFirstCharIndexFromLine(cursorLine);

            if (txtSource.Text == "")
                cursorLineLength = 0;

            else
                cursorLineLength = txtSource.Lines[cursorLine].Length;

            lblLine.Text = "Line: " + Convert.ToString(cursorLine + 1);
            lblColumn.Text = "Column: " + Convert.ToString(cursorColumn + 1);
        }

        /******************************************************************************************
         * 
         * Name:        UpdateColors
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will update the form's colors with the new colors.
         *  
         *****************************************************************************************/
        private void UpdateColors()
        {
            this.BackColor = backColorMain;
            pnlRegister.BackColor = backColorMain2;

            pnlSourcePanel.BackColor = backColorAccent;
            txtRegister00.BackColor = backColorAccent;
            txtRegister01.BackColor = backColorAccent;
            txtRegister02.BackColor = backColorAccent;
            txtRegister03.BackColor = backColorAccent;
            txtRegister04.BackColor = backColorAccent;
            txtRegister05.BackColor = backColorAccent;
            txtRegister06.BackColor = backColorAccent;
            txtRegister07.BackColor = backColorAccent;
            txtRegister08.BackColor = backColorAccent;
            txtRegister09.BackColor = backColorAccent;
            txtRegister10.BackColor = backColorAccent;
            txtRegister11.BackColor = backColorAccent;
            txtRegister12.BackColor = backColorAccent;
            txtRegister13.BackColor = backColorAccent;
            txtRegister14.BackColor = backColorAccent;
            txtRegister15.BackColor = backColorAccent;
            txtRegisterPSW.BackColor = backColorAccent;
            ssStatusStatusLabel.BackColor = backColorAccent;
            lblColumn.BackColor = backColorAccent;
            lblLine.BackColor = backColorAccent;

            txtRegister00.ForeColor = formTextColor;
            txtRegister01.ForeColor = formTextColor;
            txtRegister02.ForeColor = formTextColor;
            txtRegister03.ForeColor = formTextColor;
            txtRegister04.ForeColor = formTextColor;
            txtRegister05.ForeColor = formTextColor;
            txtRegister06.ForeColor = formTextColor;
            txtRegister07.ForeColor = formTextColor;
            txtRegister08.ForeColor = formTextColor;
            txtRegister09.ForeColor = formTextColor;
            txtRegister10.ForeColor = formTextColor;
            txtRegister11.ForeColor = formTextColor;
            txtRegister12.ForeColor = formTextColor;
            txtRegister13.ForeColor = formTextColor;
            txtRegister14.ForeColor = formTextColor;
            txtRegister15.ForeColor = formTextColor;
            txtRegisterPSW.ForeColor = formTextColor;
            ssStatusStatusLabel.ForeColor = formTextColor;
            lblColumn.ForeColor = formTextColor;
            lblLine.ForeColor = formTextColor;

            lblRegister00.ForeColor = formLabelTextColor;
            lblRegister01.ForeColor = formLabelTextColor;
            lblRegister02.ForeColor = formLabelTextColor;
            lblRegister03.ForeColor = formLabelTextColor;
            lblRegister04.ForeColor = formLabelTextColor;
            lblRegister05.ForeColor = formLabelTextColor;
            lblRegister06.ForeColor = formLabelTextColor;
            lblRegister07.ForeColor = formLabelTextColor;
            lblRegister08.ForeColor = formLabelTextColor;
            lblRegister09.ForeColor = formLabelTextColor;
            lblRegister10.ForeColor = formLabelTextColor;
            lblRegister11.ForeColor = formLabelTextColor;
            lblRegister12.ForeColor = formLabelTextColor;
            lblRegister13.ForeColor = formLabelTextColor;
            lblRegister14.ForeColor = formLabelTextColor;
            lblRegister15.ForeColor = formLabelTextColor;
            lblRegisterPSW.ForeColor = formLabelTextColor;
        }

        /******************************************************************************************
         * 
         * Name:        ViewPRT
         * 
         * Author(s):   Travis Hunt
         *              Drew Aaron
         *              Clay Boren
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will open the PRT, if it exists.
         *  
         *****************************************************************************************/
        private void ViewPRT()
        {
            if (pathPRT != "")
            {
                ViewPRTForm PRT = new ViewPRTForm();
                try
                {
                    PRT.SetBackColorMain(backColorMain);
                    PRT.SetButtonBackColor(backColorMain2);
                    PRT.SetButtonTextColor(formLabelTextColor);
                    PRT.LoadPRT(pathPRT);
                    PRT.Show();
                }

                catch (FileNotFoundException)
                {
                    MessageBox.Show("PRT file has been moved or deleted. Please reassemble or set "
                        + "the path under Tools-->Options-->Associated PRT.");
                }

                catch (IOException)
                {
                    MessageBox.Show("Your PRT cannot be opened as it is open with another " + 
                        "application. Close the other application and try again.");
                    PRT.Close();
                }
            }

            else
                MessageBox.Show("No PRT associated with this project. Please assemble first", 
                    "Error");
        }      
    }
}