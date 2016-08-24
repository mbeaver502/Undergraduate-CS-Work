using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
 *  
 *************************************************************************************************/
namespace Assist_UNA
{
    public partial class MainForm : Form
    {

        /* Constants. */
        const int DEFAULT_MAX_INSTRUCTIONS = 5000;
        const int DEFAULT_MAX_LINES = 500;
        const int DEFAULT_MAX_PAGES = 100;
        const int DEFAULT_MAX_SIZE = 2700;
        const int MAINFORM_WIDTH = 1048;
        const int MAINFORM_HEIGHT = 648;

        /* Private members. */
        private bool activateTextChanged = true;
        private bool debugMode = false;
        private bool isSaved = false;
        private bool projectExists = false;
        private bool savePRT = true;
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
        private int maxInstructions = 5000;
        private int maxLines = 500;
        private int maxPages = 100;
        private int maxSize = 2700;
        private PrintDocument printDoc = new PrintDocument();
        private string directory = "";
        private string fileName = "Unsaved Project";
        private string identifier = "";
        private string pathPRT = "";
        private string outputText = "";
        private string stringToPrint;

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
            InitializeComponent();

            txtSource.BringToFront();
            txtSource.Focus();
            txtSource.Select();
            savePRT = true;

            printDoc.PrintPage += new PrintPageEventHandler(PrintDocumentOnPrintPage);
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
         *              
         * Input:       Three strings, representing the three columns
         * Return:      N/A
         * Description: This method will add an item to the memory display on the GUI.
         *  
         *****************************************************************************************/
        public void AddMemoryEntry(string address, string contents, string charRepresentation)
        {
            string[] arr = new string[3];
            ListViewItem lvItem;
            arr[0] = address;
            arr[1] = contents;
            arr[2] = charRepresentation;
            lvItem = new ListViewItem(arr);
            lvMemory.Items.Add(lvItem);
        }


        /******************************************************************************************
         * 
         * Name:        AddSymbolTableEntry
         * 
         * Author(s):   Drew Aaron
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
            lvItem = new ListViewItem(arr);
            lvMemory.Items.Add(lvItem);
        }


        /******************************************************************************************
         * 
         * Name:        AppendOutputText
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New string to be appended to the output text.
         * Return:      N/A
         * Description: This method will append text to the output text.
         *  
         *****************************************************************************************/
        public void AppendOutputText(string newtext)
        {
            outputText += newtext;
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
            OutputForm outForm = new OutputForm();
            outForm.setDebug(debugMode);
            outForm.SetText(outputText);
            outForm.ShowDialog();

            /* Temporary until integration. */
            while (outForm.ContinueDebugging()) 
            {
                /* Do something. */

                outForm.ShowDialog();
            }
        }


        /* Private methods. */


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
                MessageBox.Show("Project must be saved at least once before assembling", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            else
                return true;
        }


        /******************************************************************************************
         * 
         * Name:        Assemble
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will assemble the source code and save a PRT file.             
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

                /* Set members to be used by assembler. */
                identifier = "";
                savePRT = true;
                debugMode = false;

                /* Assembly function here. 
                string sourcePath = directory + "\\" + fileName;
                LiteralTable testLiteralTable = new LiteralTable();
                SymbolTable testSymbolTable = new SymbolTable();
                AssemblerTest testAssembler = new AssemblerTest(identifier, sourcePath + ".una", 
                    pathPRT, sourcePath + ".imf", sourcePath + ".obj", testSymbolTable, 
                    testLiteralTable); */
            }
        }


        /******************************************************************************************
         * 
         * Name:        AssembleDebug
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
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
                /* Set members to be used by assembler. */
                savePRT = false;
                debugMode = true;

                /* Assembly function here */


                /* Run in debug mode. */
                MessageBox.Show("Assemble with debugger option!");
            }
        }


        /******************************************************************************************
         * 
         * Name:        AssembleFinalRun
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
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

                /* Set members to be used by assembler. */
                savePRT = true;
                debugMode = false;
                InputForm input = new InputForm("ASSIST/UNA", "Enter Identifer", 39);
                input.ShowDialog();
                if (input.Canceled())
                    return;
                else
                    identifier = input.GetInput();

                /* Assembly function here */


                /* Run the user's program */
                MessageBox.Show("Assemble with final run option!");
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
            txtRegister00.Text = "F4F4F4F4";
            txtRegister01.Text = "F4F4F4F4";
            txtRegister02.Text = "F4F4F4F4";
            txtRegister03.Text = "F4F4F4F4";
            txtRegister04.Text = "F4F4F4F4";
            txtRegister05.Text = "F4F4F4F4";
            txtRegister06.Text = "F4F4F4F4";
            txtRegister07.Text = "F4F4F4F4";
            txtRegister08.Text = "F4F4F4F4";
            txtRegister09.Text = "F4F4F4F4";
            txtRegister10.Text = "F4F4F4F4";
            txtRegister11.Text = "F4F4F4F4";
            txtRegister12.Text = "F4F4F4F4";
            txtRegister13.Text = "F4F4F4F4";
            txtRegister14.Text = "F4F4F4F4";
            txtRegister15.Text = "F4F4F4F4";
            txtRegisterPSW.Text = "F4F4F4F4 F4F4F4F4";

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
            bool eolComment = false;
            int endOfOperand = 0;
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
                        else if (txtSource.Lines[i].IndexOf("=C'") > 14)
                        {
                            endOfOperand = txtSource.Lines[i].IndexOf('\'',
                                txtSource.Lines[i].IndexOf("=C'") + 3);

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
                        if (eolComment)
                        {
                            txtSource.Select(highlightStart,
                                txtSource.Lines[i].Length - lineSpaceIndex);
                            txtSource.SelectionColor = commentColor;
                        }

                        eolComment = false;
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
            char firstChar = 'x';
            int endOfOperand = 0;
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
                catch (Exception)
                {

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

                    /* Account for spaces in operands. */
                    if ((cursorLineLength > 17) &&
                        (txtSource.Lines[cursorLine].Substring(15, 2) == "C'"))
                    {
                        endOfOperand = txtSource.Lines[cursorLine].IndexOf('\'', 17);

                        if (endOfOperand < 1)
                            endOfOperand = 15;
                    }

                    else if (txtSource.Lines[cursorLine].Substring(9, 7) == "TITLE '")
                    {
                        endOfOperand = txtSource.Lines[cursorLine].IndexOf('\'', 16);

                        if (endOfOperand < 1)
                            endOfOperand = 15;
                    }
                    else if (txtSource.Lines[cursorLine].IndexOf("=C'") > 14)
                    {
                        endOfOperand = txtSource.Lines[cursorLine].IndexOf('\'',
                            txtSource.Lines[cursorLine].IndexOf("=C'") + 3);

                        if (endOfOperand < 1)
                            endOfOperand = 15;
                    }

                    else if (txtSource.Lines[cursorLine].IndexOf(' ', 15) > 1)
                        endOfOperand = txtSource.Lines[cursorLine].IndexOf(' ', 15);

                    else
                        endOfOperand = 15;

                    /* All text before eol comment should be black. */
                    txtSource.Select(cursorLineStartIndex, endOfOperand);
                    txtSource.SelectionColor = defaultSourceColor;

                    /* Find last non-space character. */

                    for (int i = 14; i < currentLine.Length; i++)
                    {
                        if (currentLine[i] != ' ')
                            lastCharIndex = i;
                    }

                    /* Find if there is a space after the last field. */
                    for (int i = lastCharIndex - 1; i >= endOfOperand; i--)
                    {
                        if (currentLine[i] == ' ')
                        {
                            lineSpaceIndex = i;
                            highlightStart = cursorLineStartIndex + i + 1;
                            eolComment = true;
                        }
                    }

                    /* If so, highlight the rest of the line. */
                    if (eolComment)
                    {
                        txtSource.Select(highlightStart, lastCharIndex - lineSpaceIndex);
                        txtSource.SelectionColor = commentColor;
                        colorChanged = true;
                    }

                    else
                        checkColors = true;
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
         * Description: This method will check the entire source editor and highlight certain 
         *              syntax strings.
         *  
         *****************************************************************************************/
        private void FormatText(int start, int numLinesToCheck, int cursorPosition)
        {
            /* Temporary variables to assist in highlighting syntax. */
            bool eolComment = false;
            int endOfOperand = 0;
            int highlightStart = 16;
            int lineSpaceIndex = 16;
            int lineStartIndex = 0;

            activateTextChanged = false;


            /* Loop through given lines and set syntax highlighting accordingly. */
            for (int i = start; (i < txtSource.Lines.Length) && (i < start + numLinesToCheck); i++)
            {
                if (txtSource.Lines[i].Length > 0)
                {
                    lineStartIndex = txtSource.GetFirstCharIndexFromLine(i);
                    txtSource.Select(lineStartIndex, txtSource.Lines[i].Length);
                    txtSource.SelectionColor = defaultSourceColor;

                    /* Highlight comments of line starting with an asterisk. */
                    if (txtSource.Lines[i][0] == '*')
                    {
                        txtSource.Select(lineStartIndex, txtSource.Lines[i].Length);
                        txtSource.SelectionColor = commentColor;
                    }

                    /* Highlight end of line comments. */
                    else if (txtSource.Lines[i].Length > 16)
                    {
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
                        else if (txtSource.Lines[i].IndexOf("=C'") > 14)
                        {
                            endOfOperand = txtSource.Lines[i].IndexOf('\'',
                                txtSource.Lines[i].IndexOf("=C'") + 3);

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
                        if (eolComment)
                        {
                            txtSource.Select(highlightStart,
                                txtSource.Lines[i].Length - lineSpaceIndex);
                            txtSource.SelectionColor = commentColor;
                        }

                        eolComment = false;
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
            if ((projectExists == false) && (txtSource.Text == ""))
                isSaved = true;

            /* This will pop up the "are you sure" box if the save is not current. */
            if (isSaved == false)
            {
                DialogResult choice = MessageBox.Show("Your source code contains unsaved changes,"
                    + " would you like to save first?", "Save?", MessageBoxButtons.YesNoCancel);

                if (choice == DialogResult.Yes)
                    SaveProject();
                else if (choice == DialogResult.Cancel)
                    e.Cancel = true;
            }
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
            if ((txtSource.SelectionLength < 1) && (Clipboard.ContainsText()))
            {
                /* Variables used to tell where to check for highlighting. */
                int lineCount = txtSource.Lines.Length;
                int position = 0;
                int startLine = cursorLine;

                /* Convert text in the clipboard to uppercase and paste it into the editor. */
                Clipboard.SetText(Clipboard.GetText().ToUpper());
                txtSource.Paste();

                /* Format text to standards. */
                if (Clipboard.GetText().Contains("\t"))
                    RemoveTabs();

                /* Update variables after pasting and format the pasted text. */
                lineCount = txtSource.Lines.Length - lineCount;
                position = txtSource.GetFirstCharIndexFromLine(cursorLine) + cursorColumn;

                FormatText(startLine, lineCount, position);
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
                choice = MessageBox.Show("Your source code contains unsaved changes, " +
                "would you like to save first?", "Save?", MessageBoxButtons.YesNoCancel);

                if (choice == DialogResult.Yes)
                    SaveProject();
                else if (choice == DialogResult.Cancel)
                    return;
            }

            /* Import the source code. */
            OpenFileDialog dlgImport = new OpenFileDialog();
            dlgImport.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dlgImport.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(dlgImport.FileName);
                txtSource.Text = sr.ReadToEnd();
                sr.Close();
            }

            /* Remove all tabs and check for syntax highlighting. */
            txtSource.Text = txtSource.Text.ToUpper();
            RemoveTabs();

            /* Set all text to default color initially. */
            txtSource.Select(0, txtSource.Text.Length);
            txtSource.SelectionColor = defaultSourceColor;
            txtSource.Select(0, 0);

            FormatAllText(0);
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
         * Name:        MenuFilePrintPRTClick
         * 
         * Author(s):   Clay Boren
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will print the .PRT file.
         *  
         *****************************************************************************************/
        private void MenuFilePrintPRTClick(object sender, EventArgs e)
        {

        }


        /******************************************************************************************
         * 
         * Name:        MenuFilePrintSourceClick
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
        private void MenuFilePrintSourceClick(object sender, EventArgs e)
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
            Process.Start("http://www.una.edu");
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
                FormatAllText(cursorLineStartIndex + cursorColumn);



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
            if (outputText != "")
                ShowOutput();

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
                DialogResult choice = MessageBox.Show("Your source code contains unsaved changes,"
                    + " would you like to save first?", "Save?", MessageBoxButtons.YesNoCancel);

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
                choice = MessageBox.Show("Your source code contains unsaved changes, " +
                "would you like to save first?", "Save?", MessageBoxButtons.YesNoCancel);

                if (choice == DialogResult.Yes)
                    SaveProject();
                else if (choice == DialogResult.Cancel)
                    return;
            }

            /* This code will always execute as long as cancel is not pressed. */

            /* Open file dialog and set filter. */
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "ASSIST/UNA Files (*.una)|*.una";
            choice = dlgOpen.ShowDialog();

            if (choice == DialogResult.OK)
            {
                /* Set file name and directory data members. */
                fileName = Path.GetFileNameWithoutExtension(dlgOpen.FileName);
                directory = Path.GetDirectoryName(dlgOpen.FileName);

                /* Read in the lines and set the data members. */
                StreamReader unaFileReader = new StreamReader(dlgOpen.FileName);
                unaFileReader.ReadLine();

                try
                {
                    maxInstructions = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                }
                catch
                {
                    maxInstructions = DEFAULT_MAX_INSTRUCTIONS;
                }

                try
                {
                    maxLines = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                }
                catch
                {
                    maxLines = DEFAULT_MAX_LINES;
                }
                try
                {
                    maxPages = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
                }
                catch
                {
                    maxPages = DEFAULT_MAX_PAGES;
                }
                try
                {
                    maxSize = Convert.ToInt32(unaFileReader.ReadLine().Substring(1));
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

                FormatAllText(0);

                /* Update title bar. */
                this.Text = "ASSIST/UNA - " + fileName;

                /* Project now exists and the save is current. */
                projectExists = true;
                isSaved = true;
            }
        }


        /******************************************************************************************
         * 
         * Name:        PrintDocumentOnPrintPage
         * 
         * Author(s):   Clay Boren
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
            e.Graphics.MeasureString(stringToPrint, this.Font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            /* Draws the string within the bounds of the page */
            e.Graphics.DrawString(stringToPrint, txtSource.Font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericDefault);

            /* Remove the portion of the string that has been printed. */
            try
            {
                stringToPrint = stringToPrint.Substring(charactersOnPage);
                e.HasMorePages = (stringToPrint.Length > 0);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("error");
            }
        }


        /******************************************************************************************
         * 
         * Name:        PrintSource
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
        private void PrintSource()
        {
            printDoc.DefaultPageSettings.Landscape = true;
            ReadFile();

            PrintDialog printOptions = new PrintDialog();
            PrinterSettings(printOptions);

            if (printOptions.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        /******************************************************************************************
        * 
        * Name:        PrinterSettings
        * 
        * Author(s):   Clay Boren
        *              
        * Input:       User click event
        * Return:      N/A
        * Description: This method will gather all of the printer settings from the Print 
        *              Dialog Box.
        *  
        *****************************************************************************************/
        public void PrinterSettings(PrintDialog p)
        {
            var settings = p.PrinterSettings;
            string name = settings.PrinterName;
            p.AllowSomePages = true;
        }


        /******************************************************************************************
         * 
         * Name:        ReadFile
         * 
         * Author(s):   Clay Boren
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method writes the text from txtSource to a temporary text file.
         *  
         *****************************************************************************************/
        private void ReadFile()
        {
            string docPath = @"c:\temp\testPage.txt";

            printDoc.DocumentName = docPath;

            using (StreamWriter writer = File.CreateText(docPath))
            {
                foreach (string line in txtSource.Lines)
                    writer.WriteLine(line);
            }

            try
            {
                using (FileStream stream = new FileStream(docPath, FileMode.Open))
                using (StreamReader reader = new StreamReader(stream))
                {
                    stringToPrint = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Read Error");
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
            savePRT = true;
            maxInstructions = DEFAULT_MAX_INSTRUCTIONS;
            maxLines = DEFAULT_MAX_LINES;
            maxPages = DEFAULT_MAX_PAGES;
            maxSize = DEFAULT_MAX_SIZE;
            directory = "";
            fileName = "Unsaved Project";
            pathPRT = "";
            identifier = "";
            outputText = "";
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
                /* Set options into a string. */
                string fileContents = "#\n#" + maxInstructions + "\n#" + maxLines + "\n#" + maxPages
                    + "\n#" + maxSize + "\n#" + pathPRT + "\n#\n";

                /* Set source text into the string. */
                fileContents += txtSource.Text;

                /* Write string to file. */
                StreamWriter unaFile = new StreamWriter(directory + "\\" + fileName + ".una");
                unaFile.Write(fileContents);
                unaFile.Close();

                /* The project is now saved. */
                isSaved = true;
                this.Text = "ASSIST/UNA - " + fileName;
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
        }


        /******************************************************************************************
         * 
         * Name:        TxtSourceTextChanged
         * 
         * Author(s):   Drew Aaron
         *              Travis Hunt
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
                FormatLineText();
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
                    
                    PRT.LoadPRT(pathPRT);
                    PRT.ShowDialog();
                }
                catch (FileNotFoundException)
                {

                    MessageBox.Show("You've moved or deleted your .PRT, please reassemble or set " +
                                   "the path under Tools-->Options-->Associated PRT.");
                }
                catch (IOException)
                {
                   
                    MessageBox.Show("Your PRT cannot be opened as it is open with another " + 
                        "application. Close the other application and try again.");
                    PRT.Close();

                }
            }

            else
                MessageBox.Show("No PRT associate with this project, please assemble first", 
                    "Error");
        }
    }
}