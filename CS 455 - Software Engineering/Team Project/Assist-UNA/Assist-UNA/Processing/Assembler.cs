using Assist_UNA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

/**************************************************************************************************
 * 
 * Name: Assembler
 * 
 * ================================================================================================
 * 
 * Description: This class is to serve as the working test model of the final Assembler Class for
 *              the ASSIST/UNA project.
 *                            
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/18/2014   THH     Original version.
 * 03/22/2014   THH     Added prt.
 *                      Started pass 2.
 *                      Added assembler directives TITLE, SPACE, EJECT.
 *                      Added symbol table integration.
 * 03/23/2014   THH     Edited the format of the intermediate file.
 * 03/24/2014   THH     More work on the intermediate file.
 *                      Worked with AAH for machine op table integration.
 * 03/25/2014   THH     More work on pass 1 and pass 2.
 * 03/26/2014   THH     More work on pass 1 and pass 2.
 * 03/27/2014   THH     More work on pass 1 and pass 2.
 *                      RR instructions now generate correct object code (still needs formal test).
 *                      RX instructions w/ labels only are working as well (also test).
 * 03/28/2014   THH     Handled RX and X instructions when generating object code.
 *                      SS code still needs to be addressed.
 *                      Try/Catch blocks for syntax checking needs to be addressed as well.
 *                      PRT has been formatted.
 * 03/29/2014   THH     Clean up on the RX instructions.
 *                      Reformatted how object code (and location counter) is obtained for the 
 *                          PRT printing (now directly linked to intermediate file).
 * 03/31/2014   THH     More character code can now be generated w/ revamped ToEBCDIC method.
 *                      Added some of the assembler option limits in (lines, pages, instructions).
 *                      Most all SS format instructions may be handled now.
 *                      Needs some code clean up and cleaner commenting.
 *                      Need to solve issue with literal table and addresses.
 * 04/01/2014   THH     Added object code for literals.
 *                      Need to determine correct location of literals.
 * 04/03/2014   THH     Code cleanup.
 *                      Made processing intermediate file more uniform when generating object code.
 * 04/04/2014   THH     Completed cleaning up the processing of the intermediate file.
 *                      Began adding $ENTRY funtionality.
 * 04/05/2014   THH     Finished $ENTRY.
 *                      Added half-word boundary alignment for each instruction.
 *                      Fixed issues with SS instructions and having only one length.
 *                      Removed the parameters of Symbol and LIteral tables from the constructor.
 * 04/06/2014   THH     Started adding error handling in the form of try/catch blocks to take care
 *                          of valid assembler errors.
 * 04/07/2014   THH     Added some cases for getting more differe parameter formats.
 *                      Added halfword, fullword and doubleword literals.
 * 04/08/2014   THH     Finished handling of all SS instruction formats that do not incude labels.
 * 04/08/2014   ACA     Fixed a few runtime exception bugs - no longer an exception but assembles
 *                          even with invalid instructions - all changed code has slashes after it.
 *                      Fixed some documentation/coding standards.
 * 04/09/2014   THH     Fixed some RS formatted instructions.
 *                      In SS instructions, labels that do not have a label that require them
 *                          default to length of 1 (stored as 0).
 * 04/10/2014   THH     Tested more instructions.
 * 04/13/2014   THH     Added handling of all special branch cases.
 *                      Handles default label lengths now to size of the memory location when a 
 *                          length is not given.
 * 04/14/2014   THH     Finished LTORG, ready for testing.
 *                      Added accessor method for seeing if there are any errors.
 * 04/15/2014   THH     Finished a couple small errors not being handled correctly.
 * 04/16/2014   THH     Code clean up and commenting.
 * 04/17/2014   THH     Handled issue with commenting on SPACE.
 *                      Handled location counter issues with address types.
 *                      Handled various other small assembling issues.
 *                      (All with Andrew H.)
 *                      Added accessor for the symbol table.
 * 04/18/2014   JMB     Updated Pass1's data handling (after $ENTRY) to always put data in a .inp
 *                          file, regardless of whether it is specified by a user's data file or
 *                          placed after the $ENTRY line. Added an accessor method to retrieve the
 *                          number of data lines.
 *                      NOTICE: THE DATA FILES NEED TO BE ABLE TO BE RELATIVE AND ABSOLUTE PATHS,
 *                          NOT JUST IN THE CURRENT WORKING DIRECTORY!
 * 04/20/2014   THH     Added slack bytes to object code when needed.
 *                      Addressed other issues.
 *                      Added some assembler errors.
 *                      This needs testing!
 * 04/21/2014   THH     Added more assembly error messages.
 * 04/22/2014   THH     Added more assebmly error messages.
 *                      Fixed some assembling issues involving object code.
 * 04/23/2014   THH     Added more assembly error messages.
 *                      Bug fixes.
 * 04/24/2014   THH     Assembly error messages.
 *                      Bug fixes.
 *                      As of today, the following error messages have been integrated and 
 *                          approved:
 *                              End card missing
 *                              Addressibility error
 *                              Illegal constant type
 *                              Too many operands in DC
 *                              Label not allowed
 *                              Invalid symbol
 *                              Invalid op-code
 *                              Previously defined symbol
 *                              Missing delimiter
 *                              Missing operand
 *                              Label required
 *                              Illegal start card
 *                              Illegal use of lteral
 *                              Undefined symbol
 *                              Unresolved external reference
 *                              Syntax.
 * 04/25/2014   THH     Added ORG capabilites.
 *                          Had to change how object code is stored and accessed.
 *                      Bug fixes.
 * 04/26/2014   THH     Max pages exception fixed now.
 *                      PRT now creates automatic eject/title when 45 lines have been reached.
 *                      Bug fixes.
 * 04/27/2014   JMB     Added GetNumLinesOnLastPage accessor method.  
 * 04/27/2014   THH     Bug fixes.
 *                      Added missing constant, storage and literal representations.
 * 
 *************************************************************************************************/

namespace Assist_UNA
{
    class Assembler
    {
        /* Constants. */
        private const int CONSTANT_MAX_LENGTH = 112;
        private const int ERROR_COLUMNS = 3;
        private const int ERROR_DETAIL = 1;
        private const int ERROR_LINE = 0;
        private const int ERROR_SOURCE = 2;
        private const int MAX_DUPLICATION = 32767;
        private const int MAX_ERRORS = 300;
        private const int MAX_LINES_PER_PAGE = 45;
        private const int MAX_LITERALS = 50;
        private const int MAX_PACK_LENGTH = 16;
        private const int MAX_SYMBOLS = 100;
        private const string END_OF_PROGRAM_BYTE = "";
        private const string PRT_FOOTER = "*** PROGRAM EXECUTION BEGINNING -\nANY OUTPUT " +
                        "BEFORE EXECUTION COMPLETE MESSAGE IS PRODUCED BY USER PROGRAM ***";
        private const string PRT_HEADER = 
                        "  LOC     OBJECT CODE    STMT   SOURCE STATEMENT \t\t\t\t PAGE ";
        private const string VALID_CHARS_BINARY = "10";
        private const string VALID_CHARS_HEX = "0123456789ABCDEF";
        private const string VALID_CHARS_NUMBERS = "+-0123456789";
        private const uint MAX_HALF = UInt16.MaxValue;
        private const ulong MAX_FULL = UInt32.MaxValue;
        private const ulong MAX_DOUBLE = UInt64.MaxValue;


        /* Private members. */
        private bool endCardFound;
        private bool entryCardFound;
        private bool orgFound;
        private bool startCardFound;
        private bool titleFlag;
        private int intermediatePrevLocCounter;
        private int lineNumber;
        private int locationCounter;
        private int numDataLines;
        private int numErrors;
        private int numErrorsPrinted;
        private int numInstructions;
        private int numLines;
        private int numLinesPrintedPerPage;
        private int numLiterals;
        private int numLiteralPools;
        private int numLiteralPoolsPrinted;
        private int pageNumber;
        private int previousLocation;
        private int optionsInstructions;
        private int optionsLines;
        private int optionsPages;
        private List<LiteralTable> literalPools;
        private string[] assemblerDirectives;
        private string baseRegister;
        private string baseRegisterContents;
        private string[,] errorStream;
        private string label;
        private string line;
        private string identifier;
        private string inputFile;
        private string instruction;
        private string intermediateFile;
        private string objCode1;
        private string objCode2;
        private string objCode3;
        private string objCode;
        private string objectFile;
        private string parameters;
        private string programLength;
        private string prtFile;
        private string prtTitleHeader;
        private string sourceFile;
        private string validChars;
        private SymbolTable symTable;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        Assembler (Constructor)     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       The source, intermediate and object file paths as strings, and external 
         *              SymbolTable and LiteralTable objects. 
         * Return:      N/A 
         * Description: The constructor for Assembler. It initializes all the required variables.          
         *              
         *****************************************************************************************/
        public Assembler(string id, string source, string prt, string intermediate, string obj,
                             int instructions, int lines, int pages)
        {
            /* Initialize all the data members to default values. */
            numErrors = 0;
            numErrorsPrinted = 0;
            lineNumber = 0;
            locationCounter = 0;
            numInstructions = 0;
            numLiterals = 0;
            numLiteralPoolsPrinted = 0;
            numDataLines = 0;
            intermediatePrevLocCounter = -1;
            numLinesPrintedPerPage = 0;

            /* Set the user options sent from the frontend. */
            optionsInstructions = instructions;
            optionsLines = lines;
            optionsPages = pages;

            /* The size of this will need to figured out later... */
            errorStream = new string[MAX_ERRORS, ERROR_COLUMNS];

            /* Holds the characters that are valid for labels. */
            validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            /* Holds the directives the assembler uses, not for execution. */
            assemblerDirectives = new string[] {"START","END","CSECT","LTORG","USING","SPACE",
                                                "TITLE","EJECT","DS","DC"};

            /* Initialize the string variables to empty strings. */
            baseRegisterContents = "000000";
            label = "";
            line = "";
            instruction = "";
            objCode = "";
            objCode1 = "";
            objCode2 = "";
            objCode3 = "";
            parameters = "";
            prtTitleHeader = "";
            programLength = "";
            inputFile = "";

            identifier = id;
            pageNumber = 1;

            /* Initialize the flags for end and entry identifiers. */
            endCardFound = false;
            entryCardFound = false;
            startCardFound = false;
            orgFound = false;

            /* Flag for when the TITLE directive is used. */
            titleFlag = false;

            /* Set the file paths. */
            sourceFile = source;
            prtFile = prt;
            intermediateFile = intermediate;
            objectFile = obj;

            /* Set the literal and symbol tables. */
            symTable = new SymbolTable();

            /* Initialize the literal pool list and add the initial literal pool. */
            numLiteralPools = 0;
            literalPools = new List<LiteralTable>();
            literalPools.Add(new LiteralTable());
        }

        /******************************************************************************************
         * 
         * Name:        GetNumDataLines 
         * 
         * Author(s):   Michael Beaver
         *              
         * Input:       N/A     
         * Return:      The number of data lines is an integer.
         * Description: This method returns the number of data lines in a data file or after the
         *              $ENTRY section.
         *              
         *****************************************************************************************/
        public int GetNumDataLines()
        {
            return numDataLines;
        }

        /******************************************************************************************
         * 
         * Name:        GetNumLinesOnLastPage 
         * 
         * Author(s):   Michael Beaver
         *              
         * Input:       N/A     
         * Return:      The number of lines printed on the last page of the PRT file.
         * Description: This method returns the number of lines printed on the last page of the
         *              PRT file.
         *              
         *****************************************************************************************/
        public int GetNumLinesOnLastPage()
        {
            return numLinesPrintedPerPage;
        }

        /******************************************************************************************
        * 
        * Name:        GetInputFile 
        * 
        * Author(s):   Travis Hunt
        *              
        * Input:       N/A     
        * Return:      The input file location as a string.
        * Description: This method is the getter function for the input file location.              
        *              
        *****************************************************************************************/
        public string GetInputFile()
        {
            return inputFile;
        }

        /******************************************************************************************
        * 
        * Name:        GetIntermediateFile 
        * 
        * Author(s):   Travis Hunt
        *              
        * Input:       N/A     
        * Return:      The intermediate file location as a string.
        * Description: This method is the getter function for the intermediate file location.              
        *              
        *****************************************************************************************/
        public string GetIntermediateFile()
        {
            return intermediateFile;
        }

        /******************************************************************************************
        * 
        * Name:        GetObjectFile 
        * 
        * Author(s):   Travis Hunt
        *              
        * Input:       N/A     
        * Return:      The object file location as a string.
        * Description: This method is the getter function for the object file location.              
        *              
        *****************************************************************************************/
        public string GetObjectFile()
        {
            return objectFile;
        }

        /******************************************************************************************
         * 
         * Name:        GetSymbolTable    
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A   
         * Return:      N/A 
         * Description: This method returns the symbol table as a hash table.
         *              
         *****************************************************************************************/
        public SymbolTable GetSymbolTable()
        {
            return symTable;
        }

        /******************************************************************************************
        * 
        * Name:        HasErrors 
        * 
        * Author(s):   Travis Hunt
        *              
        * Input:       N/A     
        * Return:      True or false, if errors were found.
        * Description: This method returns true if there were assembler errors found.              
        *              
        *****************************************************************************************/
        public bool HasErrors()
        {
            return (numErrors > 0);
        }

        /******************************************************************************************
         * 
         * Name:        Pass1     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method drives the assembling pass1 process. It will read through each
         *              line of the source code file and send it to the ProcessLine method.
         *              The intermediate file will be set during pass 1.
         *              
         *****************************************************************************************/
        public string Pass1()
        {
            /* 
             * Open up streams to read from the source and write to the intermediate file. 
             * The intermediate file is created here so it may overwrite any existing file and then
             * append to it when processing each line.
             */
            StreamReader inStream = new StreamReader(sourceFile);
            StreamWriter intermediateOutStream = new StreamWriter(intermediateFile);
            intermediateOutStream.Close();
            StreamWriter inputFileOutStream;
            
            /* Skip the options portion of the project file. */
            string line = inStream.ReadLine();
            if (line == "")
                line = inStream.ReadLine(); 

            while (line != "" && line[0] == '#')
                line = inStream.ReadLine();

            /* Process each line of the source file. */
            while (!inStream.EndOfStream && !line.StartsWith("$ENTRY"))
            {
                if (!ProcessLineSourceCode(line))
                {
                    inStream.Close();
                    return "ExceedMaxLinesException";
                }
                    
                line = inStream.ReadLine();
            }

            if (line.Contains("END"))
            {
                endCardFound = true;
                if (!ProcessLineSourceCode(line))
                {
                    inStream.Close();
                    return "ExceedMaxLinesException";
                }
                    
                line = inStream.ReadLine();
            }

            /* Handle the entry point to the program. */
            if ( line != null && line.StartsWith("$ENTRY"))
            {
                entryCardFound = true;

                /* The program specifies it's own input file. */
                if (inStream.EndOfStream && line.TrimEnd() != "$ENTRY")
                {
                    int spaceIndex = line.IndexOf(' ', 7);
                    if (spaceIndex >= 0)
                        inputFile = line.Substring(7, spaceIndex - 8);
                    else
                        inputFile = line.Substring(7);

                    inputFile = inputFile.Trim();
                    string currDirectory = sourceFile.Substring(0, sourceFile.LastIndexOf('\\') + 1);

                    if (!inputFile.Contains(":"))
                        inputFile = currDirectory + inputFile;

                    /* Copy the user's data to our own input file for Simulator simplicity. */
                    try
                    {
                        string inputFile2 = sourceFile.Substring(0, sourceFile.LastIndexOf('.')) + ".inp";
                        inStream = new StreamReader(inputFile);
                        inputFileOutStream = new StreamWriter(inputFile2);

                        while (!inStream.EndOfStream)
                        {
                            inputFileOutStream.WriteLine(inStream.ReadLine());
                            numDataLines++;
                        }

                        inputFileOutStream.Close();
                    }
                    
                    catch (FileNotFoundException)
                    {
                        inStream.Close();
                        return "FileNotFoundException";
                    }
                }

                /* The input is line by line beneath the $ENTRY point. */
                else
                {
                    inputFile = sourceFile.Substring(0, sourceFile.LastIndexOf('.')) + ".inp";
                    inputFileOutStream = new StreamWriter(inputFile);

                    while (!inStream.EndOfStream)
                    {
                        inputFileOutStream.WriteLine(inStream.ReadLine());
                        numDataLines++;
                    }
                        
                    inputFileOutStream.Close();
                }
            }
            
            /* Error if no $ENTRY found. */
            if (!entryCardFound)
            {
                inStream.Close();
                return "EntryCardNotFound";
            }
                
            
            /* Close all the streams. */
            inStream.Close();
            return "";
        }

        /******************************************************************************************
         * 
         * Name:        Pass2     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       N/A
         * Return:      Whether there were issues with the .prt stream.
         * Description: This method drives the assembling pass2 process. If the path to the .prt is
         *              not valid, it will retry with a default directory and return the exception
         *              that occurred.
         *              
         *****************************************************************************************/
        public string Pass2()
        {
            bool unsuccessful = true;
            string returnValue = "success";
            
            /* Open up streams to read from the source and intermediate files. */
            StreamReader inStream = null;
            StreamReader intermediateInStream = null;
            StreamWriter prtOutStream = null;
            StreamWriter objectOutStream = null;

            /* 
             * Try to open all the streams once, if the .PRT was deleted and assigned to a directory
             * that does not exist, it will reset with a default location and try again.
             */
            while (unsuccessful && returnValue != "UnauthorizedAccessException")
            {
                try
                {
                    /* Initialize the streams. */
                    inStream = new StreamReader(sourceFile);
                    intermediateInStream = new StreamReader(intermediateFile);
                    prtOutStream = new StreamWriter(prtFile);
                    objectOutStream = new StreamWriter(objectFile);

                    /* Reset the line number and location counter values for writing to the prt. */
                    lineNumber = 0;
                    locationCounter = 0;
                    numLiteralPools = 0;

                    /* Generate the object code from the intermediate file. */
                    while (!intermediateInStream.EndOfStream)
                        ProcessLineIntermediateFile(intermediateInStream.ReadLine());

                    /* Initialize the prt file. */
                    prtOutStream.WriteLine("ASSIST/UNA Version 1.0 {0,20}{1} \n\n",
                        "GRADE RUN FOR: ", identifier);
                    prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");
                    numLinesPrintedPerPage += 5;

                    /* Close the stream to the prt so it can be written to in the processing method. */
                    prtOutStream.Close();

                    /* Send the source file line by line to the GeneratePRT method to print lines. */
                    line = inStream.ReadLine();
                    while (!inStream.EndOfStream && !line.StartsWith("$ENTRY"))
                    {
                        if (!GeneratePRT(line))
                        {
                            inStream.Close();
                            return "ExceededMaxPagesException";
                        }
                            
                        line = inStream.ReadLine();
                    }

                    if (line.Contains("END"))
                        if (!GeneratePRT(line))
                        {
                            inStream.Close();
                            return "ExceededMaxPagesException";
                        }
                            

                    /* Reopen the writing stream to the prt file to write the footer, then close it. */
                    prtOutStream = new StreamWriter(prtFile, true);

                    if (!endCardFound)
                    {
                        prtOutStream.WriteLine("\n{0,39} $\n{0,29}Error, no end card found", " ");
                        numLinesPrintedPerPage += 2;
                    }
                       

                    prtOutStream.WriteLine("\n\n*** {0} STATEMENTS FLAGGED - {1} ERRORS FOUND",
                        numErrorsPrinted, numErrors);
                    numLinesPrintedPerPage += 3;

                    if (!entryCardFound)
                        prtOutStream.WriteLine("\n\n*** Error, no entry card found. Execution terminated. ***\n");
                    else if (numErrors == 0)
                        prtOutStream.WriteLine("\n\n{0}", PRT_FOOTER);
                    numLinesPrintedPerPage += 4;

                    /* Clear the seperating character in the object code and write to the object file. */
                    objCode = objCode.Replace("|", "");
                    objCode = objCode.Replace(" ", "");
                    objCode += END_OF_PROGRAM_BYTE;
                    objectOutStream.WriteLine(objCode);

                    /* Set the flag to false to get out of the while loop. */
                    unsuccessful = false;
                }

                /* The .prt directory did not exist, so make default then retry. */
                catch (IOException)
                {
                    MessageBox.Show("The specified .prt directory is inaccessible. The path was " +
                                    "set to the default directory of the source code.", "Error - Invalid .PRT Path");
                    prtFile = sourceFile.Substring(0, sourceFile.LastIndexOf('.')) + ".PRT";
                    returnValue = "IOException";
                }

                /* The object file was read only. */
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("The object file is set to read-only. Please delete or change" +
                                    " the permissions and try again.", "Error - Object File " +
                                    "Read-Only");
                    returnValue = "UnauthorizedAccessException";
                }

                finally
                {
                    /* Close the streams. */
                    if (objectOutStream != null)
                        objectOutStream.Close();

                    if (prtOutStream != null)
                        prtOutStream.Close();

                    if (inStream != null)
                        inStream.Close();

                    if (intermediateInStream != null)
                        intermediateInStream.Close();
                }
            }
            return returnValue;
        }

        /******************************************************************************************
         * 
         * Name:        PrintErrorStream     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A        
         * Return:      N/A
         * Description: This method prints the items in the errorStream array to the console. 
         *              For testing purposes only.
         *              
         *****************************************************************************************/
        public void PrintErrorStream()
        {
            if (numErrors > 0)
            {
                Console.WriteLine("\n****ERRORS****");
                Console.WriteLine("---------------");
                for (int i = 0; i < numErrors; i++)
                {
                    Console.WriteLine("Line: " + errorStream[i, ERROR_LINE] + "\nError: " +
                                      errorStream[i, ERROR_DETAIL] + "\nSource:  " +
                                      errorStream[i, ERROR_SOURCE] + "\n");
                }
                Console.WriteLine("**************\n");
            }

            else
                Console.WriteLine("No errors detected.");
        }


        /* Private methods. */

        /******************************************************************************************
         * 
         * Name:        AddError    
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       Line number as integer, error detail and source line as strings.    
         * Return:      N/A 
         * Description: This method adds the specified error to the error stream.
         *              
         *****************************************************************************************/
        private void AddError(int lineNum, string error, string sourceLine)
        {
            if (numErrors < MAX_ERRORS)
            {
                errorStream[numErrors, ERROR_LINE] = lineNum.ToString();
                errorStream[numErrors, ERROR_DETAIL] = error;
                errorStream[numErrors, ERROR_SOURCE] = sourceLine;
                numErrors++;
            }
        }

        /******************************************************************************************
         * 
         * Name:        GeneratePRT    
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       Line from source code as string.    
         * Return:      N/A 
         * Description: This method takes in a line from the source code and prints it to the prt.
         *              It also reads through the intermediate file and object code to print.
         *              
         *****************************************************************************************/
        private bool GeneratePRT(string inputLine)
        {
            /* Reset variables. */
            line = inputLine.TrimEnd();
            label = "";
            instruction = "";
            objCode1 = "";
            objCode2 = "";
            objCode3 = "";
            parameters = "";

            string locationCounterHex = "";

            /* Open writing stream to the prt and reading stream to intermediate file. */
            StreamWriter prtOutStream = new StreamWriter(prtFile, true);
            StreamReader intermediateInStream = new StreamReader(intermediateFile);

            try
            {                
                /* If #, the row is an option line so ignore. */
                if (line != "" && line[0] != '#')
                {
                    /* The line is a comment so just write the line. */
                    if (line[0] == '*')
                    {
                        prtOutStream.WriteLine("                        " +
                                           String.Format("{0,4} {1}", (lineNumber + 1), line));
                        numLinesPrintedPerPage++;
                    }

                    /* The line is not a comment line. */
                    else
                    {
                        string[] tempIntermediateLine;
                        string[] objectCode;

                        /* Store the label and instruction portions of the line. */
                        label = line.Substring(0, 8).TrimEnd();

                        if (line.Length > 14)
                            instruction = line.Substring(9, 5).TrimEnd();
                        else
                            instruction = line.Substring(9).TrimEnd();

                        /* If the instruction is an assembler directive, handle the formatting. */
                        if (assemblerDirectives.Contains(instruction))
                        {
                            switch (instruction)
                            {
                                /*
                                * The instruction is START. Just print the line and line number,
                                * but do not increment the location counter.
                                */
                                case "CSECT":
                                case "START":
                                //case "ORG":
                                    tempIntermediateLine = intermediateInStream.ReadLine().Split('\a');

                                    while ((lineNumber + 1).ToString() != tempIntermediateLine[1])
                                        tempIntermediateLine = intermediateInStream.ReadLine().
                                                                                Split('\a');

                                    locationCounterHex = tempIntermediateLine[2];
                                    
                                    line = String.Format("{0,11} {1}", (lineNumber + 1), line);
                                    prtOutStream.WriteLine(String.Format("{0,6} {1,4} {2,4} {3}",
                                                                     locationCounterHex, objCode1,
                                                                     objCode2, line));
                                    numLinesPrintedPerPage++;
                                    break;

                                /*
                                 * The instruction is END. Just print the line and line number, but
                                 * after the END line print the literal table.
                                 */
                                case "END":
                                    line = String.Format("{0,11} {1}", (lineNumber + 1), line);
                                    prtOutStream.WriteLine(String.Format("{0,6} {1,4} {2,4} {3}",
                                                                     " ", " ",
                                                                     " ", line));
                                    numLinesPrintedPerPage++;
                                    if (!AddPagePRT(prtOutStream))
                                        throw new ExceededMaxPagesException();
                                    PrintError(prtOutStream, (lineNumber + 1));

                                    /* 
                                     * Temporary line number so it can be used to print without 
                                     * messing up the original line number counter. 
                                     */
                                    int tempLineNumber = lineNumber + 1;
                                    
                                    /* 
                                     * Get the current literal table, sort them by location counter,
                                     * and print them to the .prt. 
                                     */
                                    LiteralTable tempLitTable = literalPools[numLiteralPoolsPrinted];

                                    string[] tempLiterals = tempLitTable.GetLiteralsList();
                                    string[] tempAddresses = tempLitTable.GetAddressesList();
                                    int address = 0;
                                    int size = 0;

                                    Array.Sort(tempAddresses, tempLiterals);

                                    for (int i = 0; i < tempLiterals.Length; i++)
                                    {
                                        size = (tempLitTable.GetLiteralSize(tempLiterals[i]) * 2);

                                        address = (Convert.ToInt32(tempAddresses[i], 16) * 2);

                                        if (objCode.Length >= address + size)
                                        {
                                            if (size <= 16)
                                                objCode1 = objCode.Substring(address, size);
                                            else
                                                objCode1 = objCode.Substring(address, 16);
                                        }
                                        
                                        line = String.Format("{0,13} {1}", " ", tempLiterals[i]);

                                        prtOutStream.WriteLine("{0,6} {1,-16} {2}", tempAddresses[i],
                                                               objCode1, line);
                                        numLinesPrintedPerPage++;

                                        if (!AddPagePRT(prtOutStream))
                                            throw new ExceededMaxPagesException();
                                    }
                                    break;

                                /*
                                 * The instruction is USING. 
                                 * Set the base register and do not increment the location counter.
                                 */
                                case "USING":
                                    tempIntermediateLine = intermediateInStream.ReadLine().
                                                                    Split('\a');
                                    while ((lineNumber + 1).ToString() != tempIntermediateLine[1])
                                        tempIntermediateLine = intermediateInStream.ReadLine().
                                                                        Split('\a');

                                    locationCounterHex = tempIntermediateLine[2];

                                    line = String.Format("{0,11} {1}", (lineNumber + 1), line);
                                    prtOutStream.WriteLine(String.Format("{0,6} {1,4} {2,4} {3}",
                                                                     locationCounterHex, "",
                                                                     "", line));
                                    numLinesPrintedPerPage++;

                                    baseRegisterContents = symTable.GetAddress(baseRegisterContents);
                                    break;

                                /* The instruction is DS. */
                                case "DS":
                                    tempIntermediateLine = intermediateInStream.ReadLine().Split('\a');

                                    while ((lineNumber + 1).ToString() != tempIntermediateLine[1])
                                        tempIntermediateLine = intermediateInStream.ReadLine().
                                                                                Split('\a');

                                    locationCounterHex = tempIntermediateLine[2];
                                    line = String.Format("{0,11} {1}", (lineNumber + 1), line);

                                    prtOutStream.WriteLine(String.Format("{0,6} {1,4} {2,4} {3}",
                                                                     locationCounterHex, "",
                                                                     "", line));
                                    numLinesPrintedPerPage++;
                                    break;

                                /* The instruction is DC. */
                                case "DC":

                                    tempIntermediateLine = intermediateInStream.ReadLine().
                                                                    Split('\a');
                                    while ((lineNumber + 1).ToString() != tempIntermediateLine[1])
                                        tempIntermediateLine = intermediateInStream.ReadLine().
                                                                        Split('\a');

                                    locationCounterHex = tempIntermediateLine[2];
                                    line = String.Format("{0,3} {1}", (lineNumber + 1), line);

                                    int locationVal = (Convert.ToInt32(locationCounterHex, 16) * 2);
                                    int intermediateObjCodeSize = Convert.ToInt32(tempIntermediateLine[0], 16) * 2;

                                    if (intermediateObjCodeSize <= 16)
                                        objCode1 = objCode.Substring(locationVal, intermediateObjCodeSize);
                                    else
                                        objCode1 = objCode.Substring(locationVal, 16);

                                    prtOutStream.WriteLine(String.Format("{0,6} {1,-17} {2}",
                                                                     locationCounterHex, objCode1,
                                                                     line));
                                    numLinesPrintedPerPage++;
                                    break;

                                /* 
                                 * The instruction is SPACE.
                                 * Inserts blank line(s) into the prt. 
                                 * The number of lines is determined by the parameter passed.
                                 * If no parameter, one is inserted.
                                 */
                                case "SPACE":
                                    int spaces = 1;
                                    if (line.Length - 15 >= 1)
                                    {
                                        if (line[15] != ' ')
                                            spaces = Convert.ToInt32(line.Substring(15));
                                    }

                                    for (int i = 0; i < spaces; i++)
                                    {
                                        prtOutStream.WriteLine();
                                        numLinesPrintedPerPage++;
                                        if (!AddPagePRT(prtOutStream))
                                            throw new ExceededMaxPagesException();
                                    }
                                        
                                    break;

                                /* 
                                 * The instruction is TITLE.
                                 * Creates a title header and displays to the top of another page. 
                                 */
                                case "TITLE":
                                    int index = line.IndexOf('\'') + 1;
                                    int lastIndex = line.LastIndexOf('\'');

                                    prtTitleHeader = line.Substring(index, lastIndex - index);
                                    pageNumber++;

                                    /* Makes sure the number of pages does not exceed the limit. */
                                    if (pageNumber <= optionsPages)
                                    {
                                        prtOutStream.WriteLine("\f\n{0}", prtTitleHeader);
                                        prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");
                                        numLinesPrintedPerPage = 4;
                                    }

                                    /* The number of pages exceeded the limit. */
                                    else
                                        throw new ExceededMaxPagesException();
                                    
                                    break;

                                /*
                                 * The instruction is EJECT.
                                 * Starts writing to the prt on a new page. 
                                 */
                                case "EJECT":
                                    pageNumber++;

                                    /* Makes sure the number of pages does not exceed the limit. */
                                    if (pageNumber <= optionsPages)
                                        prtOutStream.WriteLine();

                                    /* The number of pages exceeded the limit. */
                                    else
                                        throw new ExceededMaxPagesException();

                                    break;

                                /*
                                 * The instruction is the directive LTORG.
                                 * 
                                 */
                                case "LTORG":
                                    line = String.Format("{0,11} {1}", (lineNumber + 1), line);
                                    prtOutStream.WriteLine(String.Format("{0,6} {1,4} {2,4} {3}",
                                                                     " ", " ",
                                                                     " ", line));
                                    numLinesPrintedPerPage++;
                                    if (!AddPagePRT(prtOutStream))
                                        throw new ExceededMaxPagesException();
                                    tempLineNumber = lineNumber + 1;

                                    objectCode = objCode.Split('|');

                                    tempLitTable = literalPools[numLiteralPoolsPrinted];
                                    tempLiterals = tempLitTable.GetLiteralsList();
                                    tempAddresses = tempLitTable.GetAddressesList();
                                    address = 0;
                                    Array.Sort(tempAddresses, tempLiterals);

                                    for (int i = 0; i < tempLiterals.Length; i++)
                                    {
                                        address = (Convert.ToInt32(tempAddresses[i], 16) * 2);
                                        size = (tempLitTable.GetLiteralSize(tempLiterals[i]) * 2);

                                        if (tempLiterals[i].StartsWith("=V"))
                                            objCode1 = "00000000";

                                        else
                                        {
                                            if (size <= 16)
                                                objCode1 = objCode.Substring(address, size);
                                            else
                                                objCode1 = objCode.Substring(address, 16);
                                        }
                                            
                                        line = String.Format("{0,13} {2}", " ", " ", tempLiterals[i]);

                                        prtOutStream.WriteLine("{0,6} {1,-16} {2}", tempAddresses[i],
                                                               objCode1, line);
                                        numLinesPrintedPerPage++;
                                        tempLineNumber++;

                                        if (!AddPagePRT(prtOutStream))
                                            throw new ExceededMaxPagesException();
                                    }
                                    
                                    numLiteralPoolsPrinted++;
                                    break;

                                default:
                                    break;
                            }

                            if (instruction != "END")
                                PrintError(prtOutStream, lineNumber + 1);
                        }

                        /* The instruction was not an assembler directive. */
                        else
                        {
                            tempIntermediateLine = intermediateInStream.ReadLine().
                                                                    Split('\a');
                            while (!intermediateInStream.EndOfStream &&
                                   (lineNumber + 1).ToString() != tempIntermediateLine[1])
                                tempIntermediateLine = intermediateInStream.ReadLine().
                                                                        Split('\a');

                            locationCounterHex = tempIntermediateLine[2];
                            line = String.Format("{0,6} {1}", (lineNumber + 1), line);

                            int locationVal = -1;
                            int intermediateObjCodeSize = -1;
                            if (locationCounterHex != "")
                                locationVal = (Convert.ToInt32(locationCounterHex, 16) * 2);
                            
                            if (tempIntermediateLine[0] != "-1" && locationVal >= 0)
                            {
                                intermediateObjCodeSize = Convert.ToInt32(
                                                            tempIntermediateLine[0], 16) * 2;
                                if (intermediateObjCodeSize <= 4)
                                    objCode1 = objCode.Substring(locationVal, 
                                                                 intermediateObjCodeSize);

                                else if (intermediateObjCodeSize <= 8)
                                {
                                    objCode1 = objCode.Substring(locationVal, 4);
                                    objCode2 = objCode.Substring(locationVal + 4, 
                                                                 intermediateObjCodeSize - 4);
                                }

                                else if (intermediateObjCodeSize <= 12)
                                {
                                    objCode1 = objCode.Substring(locationVal, 4);
                                    objCode2 = objCode.Substring(locationVal + 4, 4);
                                    objCode3 = objCode.Substring(locationVal + 8, 
                                                                 intermediateObjCodeSize - 8);
                                }
                            }                            

                            prtOutStream.WriteLine("{0,6} {1,4} {2,4} {3,4} {4}", locationCounterHex,
                                                   objCode1, objCode2, objCode3, line);
                            numLinesPrintedPerPage++;

                            PrintError(prtOutStream, (lineNumber + 1));
                        }
                    }

                    lineNumber++;

                    if (!AddPagePRT(prtOutStream))
                        throw new ExceededMaxPagesException();
                }

                /* Line comming in was an empty string. */
                else if (line == "")
                {
                    line = String.Format("{0,6}",(lineNumber + 1).ToString());
                    PrintError(prtOutStream, lineNumber + 1);
                    lineNumber++;
                }
            }

            catch (ArgumentOutOfRangeException)
            {
                /* Pad the line over just to help with alignment. */
                if (locationCounterHex == "")
                    line = String.Format("{0,3} {1} {2}", " ", (lineNumber + 1), line);
                prtOutStream.WriteLine("{0,6} {1,4} {2,4} {3,4} {4}", locationCounterHex,
                                                   objCode1, objCode2, objCode3, line);
                numLinesPrintedPerPage++;
                PrintError(prtOutStream, lineNumber + 1);
                lineNumber++;
            }

            catch (ExceededMaxPagesException)
            {
                MessageBox.Show(String.Format(
                    "The program has exceeded the max number ({0}) of pages, file truncated.",
                    optionsPages), "Error - Exceeded Max Pages");
                return false;

            }

            catch (FormatException)
            {
                if (locationCounterHex == "")
                    line = String.Format("{0,3} {1} {2}", " ", (lineNumber + 1), line);
                prtOutStream.WriteLine("{0,6} {1,4} {2,4} {3,4} {4}", locationCounterHex,
                                                   objCode1, objCode2, objCode3, line);
                numLinesPrintedPerPage++;
                PrintError(prtOutStream, lineNumber + 1);
            }

            catch (NullReferenceException)
            {
                if (locationCounterHex == "")
                    line = String.Format("{0,3} {1} {2}", " ", (lineNumber + 1), line);
                prtOutStream.WriteLine("{0,6} {1,4} {2,4} {3,4} {4}", locationCounterHex,
                                                   objCode1, objCode2, objCode3, line);
                numLinesPrintedPerPage++;
                PrintError(prtOutStream, lineNumber + 1);
            }

            /* Error occurred, but was handled earlier. */
            catch (Exception)
            {

            }

            finally
            {
                /* Close the prt and intermediate read/write streams. */
                prtOutStream.Close();
                intermediateInStream.Close();
            }
            return true;
        }

        /******************************************************************************************
         * 
         * Name:        AddPagePRT     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       Current page number as int, Streamwriter for the .prt file.      
         * Return:      N/A
         * Description: This method will write to the .prt file another page if the number of lines
         *              has increased over the allowed amount.
         *              
         *****************************************************************************************/
        private bool AddPagePRT(StreamWriter prtOutStream)
        {
            if (numLinesPrintedPerPage >= MAX_LINES_PER_PAGE)
            {
                pageNumber++;
                /* Makes sure the number of pages does not exceed the limit. */
                if (pageNumber <= optionsPages)
                {
                    prtOutStream.WriteLine("\f\n{0}", prtTitleHeader);
                    prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");
                    numLinesPrintedPerPage = 4;
                    return true;
                }

                /* The number of pages exceeded the limit. */
                else
                    return false;
            }
            return true;
        }

        /******************************************************************************************
        * 
        * Name:        PrintError     
        * 
        * Author(s):   Travis Hunt
        *              
        * Input:       Streamwriter for the .prt file.      
        * Return:      N/A
        * Description: This method will write to the .prt file any error assiciated with the 
         *             current line that is being processed by GeneratePRT.
        *              
        *****************************************************************************************/
        private void PrintError(StreamWriter prtOutStream, int lineNum)
        {
            try
            {
                /* If there was an error during assembly, print and set flag. */
                bool errorPrintedForLine = false;
                for (int i = 0; i < numErrors && !errorPrintedForLine; i++)
                {
                    int errorLine = Convert.ToInt32(errorStream[i, ERROR_LINE]);
                    if (lineNum == errorLine)
                    {
                        prtOutStream.WriteLine("{0,39} $\n{0,29}Error, {1}", " ",
                                               errorStream[i, ERROR_DETAIL]);
                        numErrorsPrinted++;

                        /* Need to check after each line added. */
                        numLinesPrintedPerPage++;
                        if (!AddPagePRT(prtOutStream))
                            throw new ExceededMaxPagesException();
                        numLinesPrintedPerPage++;
                        if (!AddPagePRT(prtOutStream))
                            throw new ExceededMaxPagesException();
                        errorPrintedForLine = true;
                    }
                }
            }

            catch (ExceededMaxPagesException)
            {
                MessageBox.Show(String.Format(
                    "The program has exceeded the max number ({0}) of pages.",
                    optionsInstructions), "Error - Exceeded Max Pages");
                return;
            
            }
        }

        /******************************************************************************************
         * 
         * Name:        ProcessLineIntermediateFile     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       Line from intermediate file as string.      
         * Return:      N/A
         * Description: This method takes in a line from the intermediate file and generates the
         *              machine code.
         *              
         *****************************************************************************************/
        private void ProcessLineIntermediateFile(string inputLine)
        {
            /* Extract the instruction and each parameter into an array. */
            string[] items = inputLine.Split('\a');

            int intermediateObjSize = Convert.ToInt32(items[0]);
            string intermediateLineNumber = items[1];
            string intermediateLocationCounter = items[2];
            string intermediateLabel = items[3];
            string intermediateInstruction = items[4];
            string intermediateInstructionOpCode = "";
            string intermediateParam1 = "";
            string intermediateParam2 = "";
            string intermediateParam3 = "";
            objCode1 = "";
            objCode2 = "";
            objCode3 = "";

            try
            {
                if (intermediateLocationCounter != "")
                {
                    /* Set the address of the base register. */
                    if (symTable.IsSymbol(baseRegisterContents))
                        baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                    /* There's only one parameter. */
                    if (items.Length == 6)
                        intermediateParam1 = items[5].TrimEnd();

                    /* There's only two parameters. */
                    else if (items.Length == 7)
                    {
                        intermediateParam1 = items[5].TrimEnd();
                        intermediateParam2 = items[6].TrimEnd();
                    }

                    /* There's three parameters. */
                    else if (items.Length == 8)
                    {
                        intermediateParam1 = items[5].TrimEnd();
                        intermediateParam2 = items[6].TrimEnd();
                        intermediateParam3 = items[7].TrimEnd();
                    }

                    if (intermediateInstruction == "DC")
                    {
                        if (items[4].StartsWith("A") || items[4].StartsWith("V"))
                        {
                            intermediateParam1 = "";
                            for (int i = 4; i < items.Length; i++)
                                intermediateParam1 += items[i];
                        }
                    }

                    /* Will need to do machine op look up here. */
                    int instructionIndex = MachineOpTable.IsOpcode(intermediateInstruction);

                    /* True if instruction is a valid operation. */
                    if (instructionIndex >= 0)
                    {
                        int register1 = 0;
                        int register2 = 0;
                        int register3 = 0;
                        int base1 = 0;
                        int base2 = 0;
                        int index1 = 0;
                        int displacement1 = 0;
                        int displacement2 = 0;
                        int length1 = 0;
                        int length2 = 0;
                        string immediate = "";

                        intermediateInstructionOpCode = MachineOpTable.GetObjectCode(instructionIndex);
                        string format = MachineOpTable.GetOpType(instructionIndex);

                        /* Generate the correct object code based on the instruction format. */
                        switch (format)
                        {
                            case "RR":
                                /* Convert the string decimal numbers to integers so they convert to hex. */
                                if (intermediateParam1.StartsWith("B'"))
                                    register1 = Convert.ToInt32(intermediateParam1.Substring(2, 4), 2);
                                else
                                    register1 = Convert.ToInt32(intermediateParam1);

                                objCode1 = register1.ToString("X");

                                if (intermediateParam2 != "")
                                    register2 = Convert.ToInt32(intermediateParam2);

                                /* If the instruction is a special branch, set the appropriate mask. */
                                if (intermediateInstruction == "NOPR")
                                {
                                    register2 = register1;
                                    register1 = 0;
                                }

                                else if (intermediateInstruction == "BOR")
                                {
                                    register2 = register1;
                                    register1 = 1;
                                }

                                else if (intermediateInstruction == "BHR" ||
                                         intermediateInstruction == "BPR")
                                {
                                    register2 = register1;
                                    register1 = 2;
                                }

                                else if (intermediateInstruction == "BLR" ||
                                         intermediateInstruction == "BMR")
                                {
                                    register2 = register1;
                                    register1 = 4;
                                }

                                else if (intermediateInstruction == "BNER" ||
                                         intermediateInstruction == "BNZR")
                                {
                                    register2 = register1;
                                    register1 = 7;
                                }

                                else if (intermediateInstruction == "BER" ||
                                         intermediateInstruction == "BZR")
                                {
                                    register2 = register1;
                                    register1 = 8;
                                }

                                else if (intermediateInstruction == "BHLR" ||
                                         intermediateInstruction == "BNLR" ||
                                         intermediateInstruction == "BNMR")
                                {
                                    register2 = register1;
                                    register1 = 11;
                                }

                                else if (intermediateInstruction == "BNHR" ||
                                         intermediateInstruction == "BNPR")
                                {
                                    register2 = register1;
                                    register1 = 13;
                                }

                                else if (intermediateInstruction == "BNOR")
                                {
                                    register2 = register1;
                                    register1 = 15;
                                }

                                else if (intermediateInstruction == "BR")
                                {
                                    register2 = register1;
                                    register1 = 15;
                                }

                                /* 
                                 * Register1 or register2 is not between 0 and 15 flag for error. 
                                 * It is broken up because register 2 is not always specified, only
                                 * when not a special branch.
                                 */
                                else if (register2 < 0 || register2 > 15)
                                    throw new RegisterOutOfRangeException();
                                else if (intermediateParam2 == "")
                                    throw new MissingOperandException();

                                if (register1 < 0 || register1 > 15)
                                    throw new RegisterOutOfRangeException();

                                /* Operands were valid. */
                                else
                                    objCode1 = register1.ToString("X") + register2.ToString("X");

                                break;

                            case "RX":

                                if ((intermediateInstruction == "ST") && 
                                    literalPools[numLiteralPools].IsLiteral(intermediateParam2))
                                    throw new IllegalUseOfLiteralException();

                                /* The first parameter is a mask. */
                                if (intermediateParam1.StartsWith("B'"))
                                    register1 = Convert.ToInt32(
                                                    intermediateParam1.Substring(2, 4), 2);

                                else if (intermediateInstructionOpCode == "47")
                                {
                                    /* The first parameter is a label for the B (opcode is 47). */
                                    if (symTable.IsSymbol(intermediateParam1))
                                    {
                                        base2 = Convert.ToInt32(baseRegister);
                                        index1 = 0;

                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(
                                                                    intermediateParam1), 16);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The second parameter is in D(,B) or D(L,B) format. */
                                    else if (intermediateParam1.Contains("(") && 
                                             intermediateParam1.Contains(","))
                                    {
                                        int openParen = intermediateParam1.IndexOf('(');
                                        int closeParen = intermediateParam1.IndexOf(')');
                                        int commaIndex = intermediateParam1.IndexOf(',');

                                        displacement2 = Convert.ToInt32(
                                                      intermediateParam1.Substring(0, openParen));

                                        if (openParen == commaIndex - 1)
                                            index1 = 0;
                                        else
                                            index1 = Convert.ToInt32(intermediateParam1.Substring(
                                                        openParen + 1, commaIndex - openParen - 1));

                                        base2 = Convert.ToInt32(intermediateParam1.Substring(
                                                       commaIndex + 1, closeParen - commaIndex - 1));
                                    }

                                    /* The second parameter is in D(L) format. */
                                    else if (intermediateParam1.Contains("("))
                                    {
                                        int openParen = intermediateParam1.IndexOf('(');
                                        int closeParen = intermediateParam1.IndexOf(')');

                                        displacement2 = Convert.ToInt32(
                                                        intermediateParam1.Substring(0, openParen));

                                        /* The item in the parenthesis is length. */
                                        base2 = Convert.ToInt32(intermediateParam2.Substring(
                                                        openParen + 1, closeParen - openParen - 1));
                                    }

                                    /* The second parameter is in D format. */
                                    else
                                    {
                                        displacement2 = Convert.ToInt32(intermediateParam2);
                                        index1 = 0;
                                        base2 = 0;
                                    }

                                    /* If the instruction is a special branch, set the appropriate mask. */
                                    if (intermediateInstruction == "NOP")
                                        register1 = 0;

                                    else if (intermediateInstruction == "BO")
                                        register1 = 1;

                                    else if (intermediateInstruction == "BH" ||
                                             intermediateInstruction == "BP")
                                        register1 = 2;

                                    else if (intermediateInstruction == "BL" ||
                                             intermediateInstruction == "BM")
                                        register1 = 4;

                                    else if (intermediateInstruction == "BNE" ||
                                             intermediateInstruction == "BNZ")
                                        register1 = 7;

                                    else if (intermediateInstruction == "BE" ||
                                             intermediateInstruction == "BZ")
                                        register1 = 8;

                                    else if (intermediateInstruction == "BHL" ||
                                             intermediateInstruction == "BNL" ||
                                             intermediateInstruction == "BNM")
                                        register1 = 11;

                                    else if (intermediateInstruction == "BNH" ||
                                             intermediateInstruction == "BNP")
                                        register1 = 13;

                                    else if (intermediateInstruction == "BNO")
                                        register1 = 14;

                                    else if (intermediateInstruction == "B")
                                        register1 = 15;
                                }

                                /* 
                                 * The first parameter is a register. 
                                 * Convert the string decimal numbers to integers so they 
                                 * convert to hex.
                                 */
                                else
                                    register1 = Convert.ToInt32(intermediateParam1);

                                if (intermediateParam2 != "")
                                {
                                    /* The second parameter is a label (with no extra increment). */
                                    if (symTable.IsSymbol(intermediateParam2.TrimEnd()))
                                    {
                                        index1 = 0;
                                        base2 = Convert.ToInt32(baseRegister);
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(intermediateParam2), 16);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The second parameter is a literal. */
                                    else if (literalPools[numLiteralPools].IsLiteral(
                                                                intermediateParam2.TrimEnd()))
                                    {
                                        string literalLocation = literalPools[numLiteralPools].
                                                                        GetAddress(intermediateParam2);

                                        int openParen = intermediateParam2.IndexOf('(');
                                        int closeParen = intermediateParam2.IndexOf(')');
                                        int firstDelimit = intermediateParam2.IndexOf('\'');
                                        int secondDelimit = 
                                                intermediateParam2.IndexOf('\'', firstDelimit + 1);
                                        int temp;

                                        string literalValueString = "";

                                        if (intermediateParam2[2] == '(')
                                            literalValueString = intermediateParam2.Substring
                                                       (openParen + 1, closeParen - openParen - 1);
                                        else
                                            literalValueString = intermediateParam2.Substring(
                                                firstDelimit + 1, secondDelimit - firstDelimit - 1);

                                        if (intermediateParam2.Contains("=V"))
                                        {
                                            string[] addresses = literalValueString.Split(',');
                                            foreach (string addr in addresses)
                                            {
                                                if (!symTable.IsSymbol(addr) && 
                                                    !Int32.TryParse(addr, out temp))
                                                    throw new UnresolvedExternalAddressException();
                                            }
                                        }

                                        else if (intermediateParam2.Contains("=A"))
                                        {
                                            string[] addresses = intermediateParam2.Substring(
                                                openParen + 1, closeParen - openParen - 1).Split(',');
                                            foreach (string addr in addresses)
                                            {
                                                if (!symTable.IsSymbol(addr) && 
                                                    !Int32.TryParse(addr, out temp))
                                                    throw new UnresolvedExternalAddressException();
                                            }
                                        }

                                        else if (intermediateParam2.Contains("=P"))
                                        {
                                            foreach (char character in literalValueString)
                                            {
                                                if (!VALID_CHARS_NUMBERS.Contains(character))
                                                    throw new IllegalCharacterException();
                                            }
                                        }

                                        else if (intermediateParam2.Contains("=H") ||
                                                 intermediateParam2.Contains("=F") ||
                                                 intermediateParam2.Contains("=D"))
                                        {
                                            foreach (char character in literalValueString)
                                            {
                                                if (!VALID_CHARS_NUMBERS.Contains(character))
                                                    throw new IllegalCharacterException();
                                            }
                                        }

                                        else if (intermediateParam2.Contains("=X"))
                                        {
                                            foreach (char character in literalValueString)
                                            {
                                                if (!VALID_CHARS_HEX.Contains(character))
                                                    throw new IllegalCharacterException();
                                            }
                                        }


                                        index1 = 0;
                                        base2 = Convert.ToInt32(baseRegister);

                                        displacement2 =
                                            Convert.ToInt32(literalPools[numLiteralPools].
                                                     GetAddress(intermediateParam2), 16);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The second parameter is a label (with extra increment). */
                                    else if (intermediateParam2.Contains('+'))
                                    {
                                        index1 = 0;
                                        base2 = Convert.ToInt32(baseRegister);

                                        int plusIndex = intermediateParam2.IndexOf('+');
                                        string first = intermediateParam2.Substring(0, plusIndex);
                                        string second = intermediateParam2.Substring(plusIndex + 1);

                                        /* The label is first. */
                                        if (symTable.IsSymbol(first))
                                        {
                                            displacement2 =
                                                Convert.ToInt32(symTable.GetAddress(first), 16);
                                            displacement2 += Convert.ToInt32(second);
                                            displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                        }

                                        /* The label is second. */
                                        else if (symTable.IsSymbol(second))
                                        {
                                            displacement2 =
                                                Convert.ToInt32(symTable.GetAddress(second), 16);
                                            displacement2 += Convert.ToInt32(first);
                                            displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                        }

                                        else
                                            throw new InvalidSymbolException();
                                    }

                                    /* The second parameter is a label (with extra decrement). */
                                    else if (intermediateParam2.Contains('-'))
                                    {
                                        index1 = 0;
                                        base2 = Convert.ToInt32(baseRegister);

                                        int minusIndex = intermediateParam2.IndexOf('-');
                                        string first = intermediateParam2.Substring(0, minusIndex);
                                        string second = intermediateParam2.Substring(minusIndex + 1);

                                        /* The label is first. */
                                        if (symTable.IsSymbol(first))
                                        {
                                            displacement2 =
                                                Convert.ToInt32(symTable.GetAddress(first), 16);
                                            displacement2 -= Convert.ToInt32(second);
                                            displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                        }

                                        /* The label is second. */
                                        else if (symTable.IsSymbol(second))
                                        {
                                            displacement2 =
                                                Convert.ToInt32(symTable.GetAddress(second), 16);
                                            displacement2 -= Convert.ToInt32(first);
                                            displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                        }

                                        else
                                            throw new InvalidSymbolException();
                                    }

                                    /* The second parameter is a label (with extra multiplication). */
                                    else if (intermediateParam2.Contains('*'))
                                    {
                                        index1 = 0;
                                        base2 = Convert.ToInt32(baseRegister);

                                        int multiplyIndex = intermediateParam2.IndexOf('*');
                                        string first = intermediateParam2.Substring(0, multiplyIndex);
                                        string second = intermediateParam2.Substring(multiplyIndex + 1);

                                        /* The label is first. */
                                        if (symTable.IsSymbol(first))
                                        {
                                            displacement2 =
                                                Convert.ToInt32(symTable.GetAddress(first), 16);
                                            displacement2 *= Convert.ToInt32(second);
                                            displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                        }

                                        /* The label is second. */
                                        else if (symTable.IsSymbol(second))
                                        {
                                            displacement2 =
                                                Convert.ToInt32(symTable.GetAddress(second), 16);
                                            displacement2 *= Convert.ToInt32(first);
                                            displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                        }

                                        else
                                            throw new InvalidSymbolException();
                                    }

                                    /* The second parameter is a label (with extra division). */
                                    else if (intermediateParam2.Contains('/'))
                                    {
                                        index1 = 0;
                                        base2 = Convert.ToInt32(baseRegister);

                                        int divideIndex = intermediateParam2.IndexOf('/');
                                        string first = intermediateParam2.Substring(0, divideIndex);
                                        string second = intermediateParam2.Substring(divideIndex + 1);

                                        /* The label is first. */
                                        if (symTable.IsSymbol(first))
                                        {
                                            displacement2 =
                                                Convert.ToInt32(symTable.GetAddress(first), 16);
                                            displacement2 /= Convert.ToInt32(second);
                                            displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                        }

                                        /* The label is second. */
                                        else if (symTable.IsSymbol(second))
                                        {
                                            displacement2 =
                                                Convert.ToInt32(symTable.GetAddress(second), 16);
                                            displacement2 /= Convert.ToInt32(first);
                                            displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                        }

                                        else
                                            throw new InvalidSymbolException();
                                    }

                                    /* Parameter is not a label, so calculate specified address. */
                                    else if (intermediateParam2.Contains('('))
                                    {
                                        int openParanIndex = intermediateParam2.IndexOf('(');
                                        int closeParanIndex = intermediateParam2.IndexOf(')');

                                        /* Format is either D(,B) or D(X,B). */
                                        if (intermediateParam2.Contains(','))
                                        {
                                            int commaIndex = intermediateParam2.IndexOf(',');

                                            /* Format is D(,B). */
                                            if (openParanIndex == commaIndex - 1)
                                            {
                                                displacement2 = Convert.ToInt32(
                                                    intermediateParam2.Substring(0, openParanIndex));
                                                base2 = Convert.ToInt32(intermediateParam2.Substring(
                                                    commaIndex + 1, closeParanIndex - commaIndex - 1));
                                                index1 = 0;
                                            }

                                            /* Format is D(X,B). */
                                            else
                                            {
                                                displacement2 = Convert.ToInt32(
                                                    intermediateParam2.Substring(0, openParanIndex));
                                                index1 = Convert.ToInt32(intermediateParam2.Substring(
                                                    openParanIndex + 1, commaIndex - openParanIndex - 1));
                                                base2 = Convert.ToInt32(intermediateParam2.Substring(
                                                    commaIndex + 1, closeParanIndex - commaIndex - 1));
                                            }
                                        }

                                        /* Format is D(X). */
                                        else
                                        {
                                            displacement2 = Convert.ToInt32(
                                                intermediateParam2.Substring(0, openParanIndex));
                                            index1 = Convert.ToInt32(intermediateParam2.Substring(
                                                openParanIndex + 1, closeParanIndex - openParanIndex - 1));
                                            base2 = 0;
                                        }
                                    }

                                    /* Format is D. */
                                    else if (Int32.TryParse(intermediateParam2, out displacement2))
                                    {
                                        displacement2 = Convert.ToInt32(intermediateParam2);
                                        index1 = 0;
                                        base2 = 0;
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* 
                                 * Throw the error if there is no second operand when it's not one of
                                 * the special branches.
                                 */
                                else if (intermediateInstructionOpCode != "47")
                                    throw new MissingOperandException();

                                /* Register1 or the index1 register is not in the valid range. */
                                if (register1 < 0 || register1 > 15 || index1 < 0 || index1 > 15)
                                    throw new RegisterOutOfRangeException();

                                /* Displacement is greater than 3 hex digits (FFF = 4095) */
                                else if (displacement2 < 0 || displacement2 > 4095)
                                    throw new FormatException();

                                else
                                    objCode1 = register1.ToString("X") + index1.ToString("X");

                                objCode2 = base2.ToString("X") + displacement2.ToString("X").PadLeft(3, '0');
                                objCode3 = "";
                                break;

                            case "RS":
                                register1 = Convert.ToInt32(intermediateParam1);
                                register3 = Convert.ToInt32(intermediateParam2);

                                if (literalPools[numLiteralPools].IsLiteral(intermediateParam3))
                                    throw new IllegalUseOfLiteralException();

                                else if (symTable.IsSymbol(intermediateParam3))
                                {
                                    base2 = Convert.ToInt32(baseRegister);

                                    string symbolLocation = symTable.GetAddress(intermediateParam3);

                                    if (symTable.IsSymbol(baseRegisterContents))
                                        baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                                    displacement2 = Convert.ToInt32(symbolLocation, 16) -
                                                    Convert.ToInt32(baseRegisterContents, 16);
                                }

                                else if (intermediateParam3.Contains("+"))
                                {
                                    base2 = Convert.ToInt32(baseRegister);

                                    int plusIndex = intermediateParam3.IndexOf('+');
                                    string first = intermediateParam3.Substring(0, plusIndex);
                                    string second = intermediateParam3.Substring(plusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement2 += Convert.ToInt32(second);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement2 += Convert.ToInt32(first);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                else if (intermediateParam3.Contains("-"))
                                {
                                    base2 = Convert.ToInt32(baseRegister);

                                    int minusIndex = intermediateParam3.IndexOf('-');
                                    string first = intermediateParam3.Substring(0, minusIndex);
                                    string second = intermediateParam3.Substring(minusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement2 -= Convert.ToInt32(second);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement2 = Convert.ToInt32(first) - displacement2;
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                else if (intermediateParam3.Contains("*"))
                                {
                                    base2 = Convert.ToInt32(baseRegister);

                                    int multiplyIndex = intermediateParam3.IndexOf('*');
                                    string first = intermediateParam3.Substring(0, multiplyIndex);
                                    string second = intermediateParam3.Substring(multiplyIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement2 *= Convert.ToInt32(second);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement2 *= Convert.ToInt32(first);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                else if (intermediateParam3.Contains("/"))
                                {
                                    base2 = Convert.ToInt32(baseRegister);

                                    int divideIndex = intermediateParam3.IndexOf('/');
                                    string first = intermediateParam3.Substring(0, divideIndex);
                                    string second = intermediateParam3.Substring(divideIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement2 /= Convert.ToInt32(second);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement2 = Convert.ToInt32(first) / displacement2;
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* Parameter 3 is either D or D(B). */
                                else if (!symTable.IsSymbol(intermediateParam3) &&
                                    !intermediateParam2.Contains("+") &&
                                    !symTable.IsSymbol(intermediateParam2))
                                {
                                    int openParen = intermediateParam3.IndexOf('(');
                                    int closeParen = intermediateParam3.IndexOf(')');

                                    register3 = Convert.ToInt32(intermediateParam2);

                                    if (intermediateParam3.Contains('('))
                                    {
                                        displacement2 = Convert.ToInt32(intermediateParam3.Substring(
                                                                                           0, openParen));
                                        base2 = Convert.ToInt32(intermediateParam3.Substring(
                                        openParen + 1, closeParen - openParen - 1));
                                    }

                                    else if (Int32.TryParse(intermediateParam3, out displacement2))
                                    {
                                        base2 = 0;
                                        displacement2 = Convert.ToInt32(intermediateParam3);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* Register1 or the register3 is not in the valid range. */
                                if (register1 < 0 || register1 > 15 ||
                                    register3 < 0 || register3 > 15)
                                    throw new RegisterOutOfRangeException();

                                /* Displacement is greater than 3 hex digits (FFF = 4095) */
                                else if (displacement2 < 0 || displacement2 > 4095)
                                    throw new FormatException();

                                objCode1 = register1.ToString("X") + register3.ToString("X");
                                objCode2 = base2.ToString("X") + displacement2.ToString("X").
                                           PadLeft(3, '0');
                                break;

                            case "SS":
                                /* D1(L1,B1),D2(L2,B2) */
                                base1 = 0;
                                base2 = 0;
                                displacement1 = 0;
                                displacement2 = 0;
                                length1 = 0;
                                length2 = 0;

                                /* First parameter is literal. */
                                if (literalPools[numLiteralPools].IsLiteral(intermediateParam1))
                                    throw new IllegalUseOfLiteralException();

                                /* The first parameter is just a label. */
                                else if (symTable.IsSymbol(intermediateParam1))
                                {
                                    string symbolLocation = symTable.GetAddress(intermediateParam1);

                                    if (symTable.IsSymbol(baseRegisterContents))
                                        baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                                    displacement1 = Convert.ToInt32(symbolLocation, 16) -
                                        Convert.ToInt32(baseRegisterContents, 16);
                                    base1 = Convert.ToInt32(baseRegister);

                                    string[] symbolsArray = symTable.GetSizesListLabels();
                                    int[] symbolsSizes = symTable.GetSizesList();

                                    for (int i = 0; i < symbolsArray.Length; i++)
                                    {
                                        if (symbolsArray[i] == intermediateParam1)
                                            length1 = symbolsSizes[i] - 1;
                                    }
                                }

                                /* The first parameter is a label and a length. */
                                else if (intermediateParam1.Contains('(') && symTable.IsSymbol(
                                    intermediateParam1.Substring(0, intermediateParam1.IndexOf('('))))
                                {
                                    string symbolLocation = symTable.GetAddress(
                                        intermediateParam1.Substring(0, intermediateParam1.IndexOf('(')));
                                    int openParen = intermediateParam1.IndexOf('(');
                                    int closeParen = intermediateParam1.IndexOf(')');

                                    if (symTable.IsSymbol(baseRegisterContents))
                                        baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                                    displacement1 = Convert.ToInt32(symbolLocation, 16) -
                                        Convert.ToInt32(baseRegisterContents, 16);
                                    length1 = Convert.ToInt32(intermediateParam1.Substring(
                                        openParen + 1, closeParen - openParen - 1)) - 1;
                                    base1 = Convert.ToInt32(baseRegister);
                                }


                                /* The first parameter is a label (with extra increment). */
                                else if (intermediateParam1.Contains('+'))
                                {
                                    int openParen = intermediateParam1.IndexOf('(');
                                    int closeParen = intermediateParam1.IndexOf(')');

                                    string tempIntermediateLabel;
                                    string tempIntermediateLength;

                                    /* Contains a specified length. */
                                    if (openParen >= 0 && closeParen >= 0)
                                    {
                                        tempIntermediateLabel =
                                        intermediateParam1.Substring(0, openParen);

                                        tempIntermediateLength = intermediateParam1.Substring(
                                            openParen + 1, closeParen - openParen - 1);
                                    }

                                    /* No specified length. */
                                    else
                                    {
                                        tempIntermediateLabel = intermediateParam1;
                                        tempIntermediateLength = "1";

                                        string[] symbolsArray = symTable.GetSizesListLabels();
                                        int[] symbolsSizes = symTable.GetSizesList();

                                        for (int i = 0; i < symbolsArray.Length; i++)
                                        {
                                            if (symbolsArray[i] == intermediateParam1)
                                            {
                                                tempIntermediateLength = symbolsSizes[i].ToString();
                                                break;
                                            }
                                        }
                                    }

                                    /* Save the length. */
                                    length1 = Convert.ToInt32(tempIntermediateLength) - 1;
                                    base1 = Convert.ToInt32(baseRegister);

                                    int plusIndex = tempIntermediateLabel.IndexOf('+');
                                    string first = tempIntermediateLabel.Substring(0, plusIndex);
                                    string second = tempIntermediateLabel.Substring(plusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 += Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 += Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is a label (with extra decrement). */
                                else if (intermediateParam1.Contains('-'))
                                {
                                    int openParen = intermediateParam1.IndexOf('(');
                                    int closeParen = intermediateParam1.IndexOf(')');

                                    string tempIntermediateLabel;
                                    string tempIntermediateLength;

                                    /* Contains a specified length. */
                                    if (openParen >= 0 && closeParen >= 0)
                                    {
                                        tempIntermediateLabel =
                                        intermediateParam1.Substring(0, openParen);

                                        tempIntermediateLength = intermediateParam1.Substring(
                                            openParen + 1, closeParen - openParen - 1);
                                    }

                                    /* No specified length. */
                                    else
                                    {
                                        tempIntermediateLabel = intermediateParam1;
                                        tempIntermediateLength = "1";

                                        string[] symbolsArray = symTable.GetSizesListLabels();
                                        int[] symbolsSizes = symTable.GetSizesList();

                                        for (int i = 0; i < symbolsArray.Length; i++)
                                        {
                                            if (symbolsArray[i] == intermediateParam1)
                                            {
                                                tempIntermediateLength = symbolsSizes[i].ToString();
                                                break;
                                            }
                                        }
                                    }

                                    /* Save the length. */
                                    length1 = Convert.ToInt32(tempIntermediateLength) - 1;
                                    base1 = Convert.ToInt32(baseRegister);

                                    int minusIndex = tempIntermediateLabel.IndexOf('-');
                                    string first = tempIntermediateLabel.Substring(0, minusIndex);
                                    string second = tempIntermediateLabel.Substring(minusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 -= Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 = Convert.ToInt32(first) - displacement1;
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is a label (with extra multiplication). */
                                else if (intermediateParam1.Contains('*'))
                                {
                                    int openParen = intermediateParam1.IndexOf('(');
                                    int closeParen = intermediateParam1.IndexOf(')');

                                    string tempIntermediateLabel;
                                    string tempIntermediateLength;

                                    /* Contains a specified length. */
                                    if (openParen >= 0 && closeParen >= 0)
                                    {
                                        tempIntermediateLabel =
                                        intermediateParam1.Substring(0, openParen);

                                        tempIntermediateLength = intermediateParam1.Substring(
                                            openParen + 1, closeParen - openParen - 1);
                                    }

                                    /* No specified length. */
                                    else
                                    {
                                        tempIntermediateLabel = intermediateParam1;
                                        tempIntermediateLength = "1";

                                        string[] symbolsArray = symTable.GetSizesListLabels();
                                        int[] symbolsSizes = symTable.GetSizesList();

                                        for (int i = 0; i < symbolsArray.Length; i++)
                                        {
                                            if (symbolsArray[i] == intermediateParam1)
                                            {
                                                tempIntermediateLength = symbolsSizes[i].ToString();
                                                break;
                                            }
                                        }
                                    }

                                    /* Save the length. */
                                    length1 = Convert.ToInt32(tempIntermediateLength) - 1;
                                    base1 = Convert.ToInt32(baseRegister);

                                    int multiplyIndex = tempIntermediateLabel.IndexOf('*');
                                    string first = tempIntermediateLabel.Substring(0, multiplyIndex);
                                    string second = tempIntermediateLabel.Substring(multiplyIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 *= Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 *= Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is a label (with extra division). */
                                else if (intermediateParam1.Contains('/'))
                                {
                                    int openParen = intermediateParam1.IndexOf('(');
                                    int closeParen = intermediateParam1.IndexOf(')');

                                    string tempIntermediateLabel;
                                    string tempIntermediateLength;

                                    /* Contains a specified length. */
                                    if (openParen >= 0 && closeParen >= 0)
                                    {
                                        tempIntermediateLabel =
                                        intermediateParam1.Substring(0, openParen);

                                        tempIntermediateLength = intermediateParam1.Substring(
                                            openParen + 1, closeParen - openParen - 1);
                                    }

                                    /* No specified length. */
                                    else
                                    {
                                        tempIntermediateLabel = intermediateParam1;
                                        tempIntermediateLength = "1";

                                        string[] symbolsArray = symTable.GetSizesListLabels();
                                        int[] symbolsSizes = symTable.GetSizesList();

                                        for (int i = 0; i < symbolsArray.Length; i++)
                                        {
                                            if (symbolsArray[i] == intermediateParam1)
                                            {
                                                tempIntermediateLength = symbolsSizes[i].ToString();
                                                break;
                                            }
                                        }
                                    }

                                    /* Save the length. */
                                    length1 = Convert.ToInt32(tempIntermediateLength) - 1;
                                    base1 = Convert.ToInt32(baseRegister);

                                    int divideIndex = tempIntermediateLabel.IndexOf('/');
                                    string first = tempIntermediateLabel.Substring(0, divideIndex);
                                    string second = tempIntermediateLabel.Substring(divideIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 /= Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 = Convert.ToInt32(first) / displacement1;
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is in D(,B) or D(L,B) format. */
                                else if (intermediateParam1.Contains("(") &&
                                    intermediateParam1.Contains(",") && !symTable.IsSymbol(
                                    intermediateParam1.Substring(0, intermediateParam1.IndexOf('('))))
                                {
                                    int openParen = intermediateParam1.IndexOf('(');
                                    int closeParen = intermediateParam1.IndexOf(')');
                                    int commaIndex = intermediateParam1.IndexOf(',');

                                    if (openParen == commaIndex - 1)
                                        length1 = 0;
                                    else
                                    {
                                        length1 = Convert.ToInt32(intermediateParam1.Substring(
                                            openParen + 1, commaIndex - openParen - 1)) - 1;
                                    }

                                    displacement1 =
                                        Convert.ToInt32(intermediateParam1.Substring(0, openParen));
                                    base1 = Convert.ToInt32(intermediateParam1.Substring(
                                        commaIndex + 1, closeParen - commaIndex - 1));
                                }

                                /* The first parameter is in D(L)format. */
                                else if (intermediateParam1.Contains("(") &&
                                             !symTable.IsSymbol(intermediateParam1.Substring(
                                             0, intermediateParam1.IndexOf('('))))
                                {
                                    int openParen = intermediateParam1.IndexOf('(');
                                    int closeParen = intermediateParam1.IndexOf(')');

                                    length1 = Convert.ToInt32(intermediateParam1.Substring(
                                        openParen + 1, closeParen - openParen - 1)) - 1;

                                    if (Int32.TryParse(intermediateParam1.Substring(0, openParen), out displacement1))
                                        displacement1 =
                                            Convert.ToInt32(intermediateParam1.Substring(0, openParen));
                                    else
                                        throw new InvalidSymbolException();

                                    base1 = 0;
                                }

                                else if (Int32.TryParse(intermediateParam1, out displacement1))
                                {
                                    displacement1 = Convert.ToInt32(intermediateParam1);
                                    base1 = 0;
                                    length1 = 0;
                                }

                                else
                                    throw new InvalidSymbolException();

                                /*-------------------------------------------------------------------*
                                 *-------------- Check the format of the second operand. ------------*
                                 *-------------------------------------------------------------------*
                                 */

                                /* Parameter 2 is a symbol. */
                                if (!intermediateParam2.StartsWith("=") && intermediateParam2.Contains(
                                    "(") && symTable.IsSymbol(intermediateParam2.Substring(
                                    0, intermediateParam2.IndexOf('('))))
                                {
                                    string symbolLocation =
                                        symTable.GetAddress(intermediateParam2.Substring(
                                        0, intermediateParam2.IndexOf('(')));
                                    int openParen = intermediateParam2.IndexOf('(');
                                    int closeParen = intermediateParam2.IndexOf(')');

                                    if (symTable.IsSymbol(baseRegisterContents))
                                        baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                                    displacement2 = Convert.ToInt32(symbolLocation, 16) -
                                        Convert.ToInt32(baseRegisterContents, 16);
                                    length2 = Convert.ToInt32(intermediateParam2.Substring(
                                        openParen + 1, closeParen - openParen - 1)) - 1;
                                    base2 = Convert.ToInt32(baseRegister);
                                }

                                /* Parameter is a literal. */
                                else if (literalPools[numLiteralPools].IsLiteral(intermediateParam2))
                                {
                                    string literalLocation = literalPools[numLiteralPools].GetAddress(intermediateParam2);

                                    int openParen = intermediateParam2.IndexOf('(');
                                    int closeParen = intermediateParam2.IndexOf(')');
                                    int firstDelimit = intermediateParam2.IndexOf('\'');
                                    int secondDelimit = intermediateParam2.IndexOf('\'', firstDelimit + 1);

                                    string literalValueString = "";

                                    if (intermediateParam2[2] == '(')
                                        literalValueString = intermediateParam2.Substring
                                                      (openParen + 1, closeParen - openParen - 1);
                                    else
                                        literalValueString = intermediateParam2.Substring(firstDelimit + 1, secondDelimit - firstDelimit - 1);


                                    if (intermediateParam2.Contains("=V"))
                                    {
                                        string[] addresses = literalValueString.Split(',');
                                        foreach (string addr in addresses)
                                        {
                                            if (!symTable.IsSymbol(addr))
                                                throw new UnresolvedExternalAddressException();
                                        }
                                    }

                                    else if (intermediateParam2.Contains("=A"))
                                    {
                                        string[] addresses = intermediateParam2.Substring(openParen + 1, closeParen - openParen - 1).Split(',');
                                        foreach (string addr in addresses)
                                        {
                                            if (!symTable.IsSymbol(addr))
                                                throw new UnresolvedExternalAddressException();
                                        }
                                    }

                                    else if (intermediateParam2.Contains("=P"))
                                    {
                                        foreach (char character in literalValueString)
                                        {
                                            if (!VALID_CHARS_NUMBERS.Contains(character))
                                                throw new IllegalCharacterException();
                                        }
                                    }

                                    else if (intermediateParam2.Contains("=H") || 
                                             intermediateParam2.Contains("=F") ||
                                             intermediateParam2.Contains("=D"))
                                    {
                                        foreach (char character in literalValueString)
                                        {
                                            if (!VALID_CHARS_NUMBERS.Contains(character))
                                                throw new IllegalCharacterException();
                                        }
                                    }

                                    else if (intermediateParam2.Contains("=X"))
                                    {
                                        foreach (char character in literalValueString)
                                        {
                                            if (!VALID_CHARS_HEX.Contains(character))
                                                throw new IllegalCharacterException();
                                        }
                                    }

                                    if (symTable.IsSymbol(baseRegisterContents))
                                        baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                                    displacement2 = Convert.ToInt32(literalLocation, 16) -
                                        Convert.ToInt32(baseRegisterContents, 16);
                                    base2 = Convert.ToInt32(baseRegister);

                                    length2 = (intermediateParam2.Length - 4);

                                    if (intermediateParam2[1] == 'P')
                                    {
                                        if (length2 % 2 == 0)
                                            length2 += 2;
                                        else
                                            length2++;
                                    }

                                    else if (intermediateParam2[1] == 'H')
                                        length2 = 2;
                                    else if (intermediateParam2[1] == 'F')
                                        length2 = 4;
                                    else if (intermediateParam2[1] == 'D')
                                        length2 = 8;

                                    //length2 *= 2;

                                    length2 = (length2 / 2) - 1;
                                }

                                else if (symTable.IsSymbol(intermediateParam2))
                                {
                                    string symbolLocation = symTable.GetAddress(intermediateParam2);

                                    if (symTable.IsSymbol(baseRegisterContents))
                                        baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                                    displacement2 = Convert.ToInt32(symbolLocation, 16) -
                                        Convert.ToInt32(baseRegisterContents, 16);
                                    base2 = Convert.ToInt32(baseRegister);

                                    string[] symbolsArray = symTable.GetSizesListLabels();
                                    int[] symbolsSizes = symTable.GetSizesList();

                                    for (int i = 0; i < symbolsArray.Length; i++)
                                    {
                                        if (symbolsArray[i] == intermediateParam2)
                                            length2 = symbolsSizes[i] - 1;
                                    }
                                }

                                /* The second parameter is a label (with extra increment). */
                                else if (intermediateParam2.Contains('+'))
                                {
                                    int openParen = intermediateParam2.IndexOf('(');
                                    int closeParen = intermediateParam2.IndexOf(')');

                                    string tempIntermediateLabel;
                                    string tempIntermediateLength;

                                    /* Contains a specified length. */
                                    if (openParen >= 0 && closeParen >= 0)
                                    {
                                        tempIntermediateLabel =
                                        intermediateParam2.Substring(0, openParen);

                                        tempIntermediateLength = intermediateParam2.Substring(
                                            openParen + 1, closeParen - openParen - 1);
                                    }

                                    /* No specified length. */
                                    else
                                    {
                                        tempIntermediateLabel = intermediateParam2;
                                        tempIntermediateLength = "1";

                                        string[] symbolsArray = symTable.GetSizesListLabels();
                                        int[] symbolsSizes = symTable.GetSizesList();

                                        for (int i = 0; i < symbolsArray.Length; i++)
                                        {
                                            if (symbolsArray[i] == intermediateParam2)
                                            {
                                                tempIntermediateLength = symbolsSizes[i].ToString();
                                                break;
                                            }
                                        }
                                    }

                                    /* Save the length. */
                                    length2 = Convert.ToInt32(tempIntermediateLength) - 1;
                                    base2 = Convert.ToInt32(baseRegister);

                                    int plusIndex = tempIntermediateLabel.IndexOf('+');
                                    string first = tempIntermediateLabel.Substring(0, plusIndex);
                                    string second = tempIntermediateLabel.Substring(plusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement2 += Convert.ToInt32(second);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement2 += Convert.ToInt32(first);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The second parameter is a label (with extra decrement). */
                                else if (intermediateParam2.Contains('-'))
                                {
                                    int openParen = intermediateParam2.IndexOf('(');
                                    int closeParen = intermediateParam2.IndexOf(')');

                                    string tempIntermediateLabel;
                                    string tempIntermediateLength;

                                    /* Contains a specified length. */
                                    if (openParen >= 0 && closeParen >= 0)
                                    {
                                        tempIntermediateLabel =
                                        intermediateParam2.Substring(0, openParen);

                                        tempIntermediateLength = intermediateParam2.Substring(
                                            openParen + 1, closeParen - openParen - 1);
                                    }

                                    /* No specified length. */
                                    else
                                    {
                                        tempIntermediateLabel = intermediateParam2;
                                        tempIntermediateLength = "1";

                                        string[] symbolsArray = symTable.GetSizesListLabels();
                                        int[] symbolsSizes = symTable.GetSizesList();

                                        for (int i = 0; i < symbolsArray.Length; i++)
                                        {
                                            if (symbolsArray[i] == intermediateParam2)
                                            {
                                                tempIntermediateLength = symbolsSizes[i].ToString();
                                                break;
                                            }
                                        }
                                    }

                                    /* Save the length. */
                                    length2 = Convert.ToInt32(tempIntermediateLength) - 1;
                                    base2 = Convert.ToInt32(baseRegister);

                                    int minusIndex = tempIntermediateLabel.IndexOf('-');
                                    string first = tempIntermediateLabel.Substring(0, minusIndex);
                                    string second = tempIntermediateLabel.Substring(minusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement2 -= Convert.ToInt32(second);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement2 = Convert.ToInt32(first) - displacement2;
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The second parameter is a label (with extra multiplication). */
                                else if (intermediateParam2.Contains('*'))
                                {
                                    int openParen = intermediateParam2.IndexOf('(');
                                    int closeParen = intermediateParam2.IndexOf(')');

                                    string tempIntermediateLabel;
                                    string tempIntermediateLength;

                                    /* Contains a specified length. */
                                    if (openParen >= 0 && closeParen >= 0)
                                    {
                                        tempIntermediateLabel =
                                        intermediateParam2.Substring(0, openParen);

                                        tempIntermediateLength = intermediateParam2.Substring(
                                            openParen + 1, closeParen - openParen - 1);
                                    }

                                    /* No specified length. */
                                    else
                                    {
                                        tempIntermediateLabel = intermediateParam2;
                                        tempIntermediateLength = "1";

                                        string[] symbolsArray = symTable.GetSizesListLabels();
                                        int[] symbolsSizes = symTable.GetSizesList();

                                        for (int i = 0; i < symbolsArray.Length; i++)
                                        {
                                            if (symbolsArray[i] == intermediateParam2)
                                            {
                                                tempIntermediateLength = symbolsSizes[i].ToString();
                                                break;
                                            }
                                        }
                                    }

                                    /* Save the length. */
                                    length2 = Convert.ToInt32(tempIntermediateLength) - 1;
                                    base2 = Convert.ToInt32(baseRegister);

                                    int multiplyIndex = tempIntermediateLabel.IndexOf('*');
                                    string first = tempIntermediateLabel.Substring(0, multiplyIndex);
                                    string second = tempIntermediateLabel.Substring(multiplyIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement2 *= Convert.ToInt32(second);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement2 *= Convert.ToInt32(first);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The second parameter is a label (with extra division). */
                                else if (intermediateParam2.Contains('/'))
                                {
                                    int openParen = intermediateParam2.IndexOf('(');
                                    int closeParen = intermediateParam2.IndexOf(')');

                                    string tempIntermediateLabel;
                                    string tempIntermediateLength;

                                    /* Contains a specified length. */
                                    if (openParen >= 0 && closeParen >= 0)
                                    {
                                        tempIntermediateLabel =
                                        intermediateParam2.Substring(0, openParen);

                                        tempIntermediateLength = intermediateParam2.Substring(
                                            openParen + 1, closeParen - openParen - 1);
                                    }

                                    /* No specified length. */
                                    else
                                    {
                                        tempIntermediateLabel = intermediateParam2;
                                        tempIntermediateLength = "1";

                                        string[] symbolsArray = symTable.GetSizesListLabels();
                                        int[] symbolsSizes = symTable.GetSizesList();

                                        for (int i = 0; i < symbolsArray.Length; i++)
                                        {
                                            if (symbolsArray[i] == intermediateParam2)
                                            {
                                                tempIntermediateLength = symbolsSizes[i].ToString();
                                                break;
                                            }
                                        }
                                    }

                                    /* Save the length. */
                                    length2 = Convert.ToInt32(tempIntermediateLength) - 1;
                                    base2 = Convert.ToInt32(baseRegister);

                                    int divideIndex = tempIntermediateLabel.IndexOf('/');
                                    string first = tempIntermediateLabel.Substring(0, divideIndex);
                                    string second = tempIntermediateLabel.Substring(divideIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement2 /= Convert.ToInt32(second);
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement2 =
                                            Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement2 = Convert.ToInt32(first) / displacement2;
                                        displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The second parameter is in D(,B) or D(L,B) format. */
                                else if (intermediateParam2.Contains("(") && intermediateParam2.Contains(",") &&
                                             !symTable.IsSymbol(intermediateParam2.Substring(0, intermediateParam2.IndexOf('('))))
                                {
                                    int openParen = intermediateParam2.IndexOf('(');
                                    int closeParen = intermediateParam2.IndexOf(')');
                                    int commaIndex = intermediateParam2.IndexOf(',');

                                    displacement2 = Convert.ToInt32(intermediateParam2.Substring(0, openParen));

                                    if (openParen == commaIndex - 1)
                                        length2 = 0;
                                    else
                                        length2 = Convert.ToInt32(intermediateParam2.Substring(openParen + 1, commaIndex - openParen - 1)) - 1;

                                    base2 = Convert.ToInt32(intermediateParam2.Substring(commaIndex + 1, closeParen - commaIndex - 1));
                                }

                                /* The second parameter is in D(L) format. */
                                else if (intermediateParam2.Contains("("))
                                {
                                    int openParen = intermediateParam2.IndexOf('(');
                                    int closeParen = intermediateParam2.IndexOf(')');

                                    if (Int32.TryParse(intermediateParam2.Substring(0, openParen), out displacement2))
                                        displacement2 = Convert.ToInt32(intermediateParam2.Substring(0, openParen));
                                    else
                                        throw new InvalidSymbolException();

                                    /* The item in the parenthesis is just a base register not length. */
                                    if (intermediateInstruction == "ED" || intermediateInstruction == "EDMK" ||
                                    intermediateInstruction == "MVC" || intermediateInstruction == "CLC")
                                        base2 = Convert.ToInt32(intermediateParam2.Substring(openParen + 1, closeParen - openParen - 1));

                                    /* The item in the parenthesis is length. */
                                    else
                                    {
                                        length2 = Convert.ToInt32(intermediateParam2.Substring(openParen + 1, closeParen - openParen - 1)) - 1;
                                        base2 = 0;
                                    }
                                }

                                /* The second parameter is in D format. */
                                else if (Int32.TryParse(intermediateParam2, out displacement2))
                                {
                                    displacement2 = Convert.ToInt32(intermediateParam2);
                                    length2 = 0;
                                    base2 = 0;
                                }

                                else
                                    throw new InvalidSymbolException();

                                /* 
                                 * If the instruction is either ED, EDMK, CLC, or MVC there's only one 
                                 * length, only use length1. 
                                 * Else use both. 
                                 */
                                if (intermediateInstruction == "ED" || intermediateInstruction == "EDMK" ||
                                    intermediateInstruction == "MVC" || intermediateInstruction == "CLC")
                                {
                                    if (length1 < 0 || length1 > 255)
                                        throw new FormatException();

                                    objCode1 = length1.ToString("X").PadLeft(2, '0');
                                }

                                else
                                {
                                    if (length1 < 0 || length1 > 15 || length2 < 0 || length2 > 15)
                                        throw new FormatException();

                                    objCode1 = (length1.ToString("X") + length2.ToString("X")).PadLeft(2, '0');
                                }

                                /* One or more base registers are out of range, or displacements are. */
                                if (base1 < 0 || base1 > 15 || base2 < 0 || base2 > 15)
                                    throw new RegisterOutOfRangeException();
                                else if (displacement1 < 0 || displacement1 > 4095 ||
                                         displacement2 < 0 || displacement2 > 4095)
                                    throw new FormatException();

                                objCode2 = base1.ToString("X") + displacement1.ToString("X").PadLeft(3, '0');
                                objCode3 = base2.ToString("X") + displacement2.ToString("X").PadLeft(3, '0');
                                break;

                            case "SI":
                                base1 = 0;
                                displacement1 = 0;
                                immediate = "";

                                /* The first parameter is a label (with no extra increment). */
                                if (symTable.IsSymbol(intermediateParam1))
                                {
                                    base1 = Convert.ToInt32(baseRegister);
                                    displacement1 = Convert.ToInt32(symTable.GetAddress(intermediateParam1), 16);
                                    displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                else if (literalPools[numLiteralPools].IsLiteral(intermediateParam1))
                                    throw new IllegalUseOfLiteralException();

                                /* The first parameter is a label (with extra increment). */
                                else if (intermediateParam1.Contains('+'))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    int plusIndex = intermediateParam1.IndexOf('+');
                                    string first = intermediateParam1.Substring(0, plusIndex);
                                    string second = intermediateParam1.Substring(plusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 += Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 += Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is a label (with extra decrement). */
                                else if (intermediateParam1.Contains('-'))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    int minusIndex = intermediateParam1.IndexOf('-');
                                    string first = intermediateParam1.Substring(0, minusIndex);
                                    string second = intermediateParam1.Substring(minusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 -= Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 -= Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is a label (with extra multiplication). */
                                else if (intermediateParam1.Contains('*'))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    int multiplyIndex = intermediateParam1.IndexOf('*');
                                    string first = intermediateParam1.Substring(0, multiplyIndex);
                                    string second = intermediateParam1.Substring(multiplyIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 *= Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 *= Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is a label (with extra division). */
                                else if (intermediateParam1.Contains('/'))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    int divideIndex = intermediateParam1.IndexOf('/');
                                    string first = intermediateParam1.Substring(0, divideIndex);
                                    string second = intermediateParam1.Substring(divideIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 /= Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 /= Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* Format is D(B). */
                                else if (intermediateParam1.Contains('(') &&
                                         !symTable.IsSymbol(intermediateParam1.Substring(0, intermediateParam1.IndexOf('('))))
                                {
                                    int openParanIndex = intermediateParam1.IndexOf('(');
                                    int closeParanIndex = intermediateParam1.IndexOf(')');

                                    displacement1 = Convert.ToInt32(intermediateParam1.Substring(0, openParanIndex));
                                    base1 = Convert.ToInt32(intermediateParam1.Substring(openParanIndex + 1, closeParanIndex - openParanIndex - 1));
                                }

                                /* Format is label and length, therefore not valid. */
                                else if (intermediateParam1.Contains('(') &&
                                         symTable.IsSymbol(intermediateParam1.Substring(0, intermediateParam1.IndexOf('('))))
                                    throw new FormatException();


                                /* Format is D. */
                                else if (Int32.TryParse(intermediateParam1, out displacement1))
                                {
                                    displacement1 = Convert.ToInt32(intermediateParam1);
                                    base1 = 0;
                                }

                                else
                                    throw new InvalidSymbolException();

                                int startIndex = 2;
                                int immediateLength = intermediateParam2.Length - startIndex - 1;

                                if (intermediateParam2.StartsWith("C"))
                                {
                                    string tempImmediate = intermediateParam2.Substring(startIndex, immediateLength);

                                    for (int i = 0; i < tempImmediate.Length; i++)
                                        immediate += ToEBCDIC(tempImmediate[i]);
                                }

                                else if (intermediateParam2.StartsWith("X"))
                                    immediate = intermediateParam2.Substring(startIndex, immediateLength);
                                else if (intermediateParam2.StartsWith("B"))
                                    immediate = Convert.ToInt32(intermediateParam2.Substring(startIndex, immediateLength), 2).ToString("X");
                                else if (intermediateParam2.StartsWith("P"))
                                    immediate = Convert.ToInt32(intermediateParam2.Substring(startIndex, immediateLength), 2).ToString("X");

                                /* 
                                 * The base register or displacement is out of range, 
                                 * or immediate is left out. 
                                 */
                                if (base1 < 0 || base1 > 15)
                                    throw new RegisterOutOfRangeException();
                                else if (displacement1 < 0 || displacement1 > 4095 || immediate == "" || immediate.Length > 2)
                                    throw new FormatException();

                                objCode1 = Convert.ToInt32(immediate, 16).ToString("X").PadLeft(2, '0');
                                objCode2 = base1.ToString("X") + displacement1.ToString("X").PadLeft(3, '0');
                                objCode3 = "";
                                break;

                            case "X":

                                index1 = 0;
                                base1 = 0;
                                displacement1 = 0;

                                /* The first parameter is a literal. */
                                if (literalPools[numLiteralPools].IsLiteral(intermediateParam1))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    displacement1 = Convert.ToInt32(literalPools[numLiteralPools].GetAddress(intermediateParam1), 16);
                                    displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                /* The first parameter is a label (with no extra increment). */
                                else if (symTable.IsSymbol(intermediateParam1))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    displacement1 = Convert.ToInt32(symTable.GetAddress(intermediateParam1), 16);
                                    displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                /* The first parameter is a label (with extra increment). */
                                else if (intermediateParam1.Contains('+'))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    int plusIndex = intermediateParam1.IndexOf('+');
                                    string first = intermediateParam1.Substring(0, plusIndex);
                                    string second = intermediateParam1.Substring(plusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 += Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 += Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is a label (with extra decrement). */
                                else if (intermediateParam1.Contains('-'))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    int minusIndex = intermediateParam1.IndexOf('-');
                                    string first = intermediateParam1.Substring(0, minusIndex);
                                    string second = intermediateParam1.Substring(minusIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 -= Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 -= Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is a label (with extra multiplication). */
                                else if (intermediateParam1.Contains('*'))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    int multiplyIndex = intermediateParam1.IndexOf('*');
                                    string first = intermediateParam1.Substring(0, multiplyIndex);
                                    string second = intermediateParam1.Substring(multiplyIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 *= Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 *= Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                /* The first parameter is a label (with extra division). */
                                else if (intermediateParam1.Contains('/'))
                                {
                                    base1 = Convert.ToInt32(baseRegister);

                                    int divideIndex = intermediateParam1.IndexOf('/');
                                    string first = intermediateParam1.Substring(0, divideIndex);
                                    string second = intermediateParam1.Substring(divideIndex + 1);

                                    /* The label is first. */
                                    if (symTable.IsSymbol(first))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                        displacement1 /= Convert.ToInt32(second);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    /* The label is second. */
                                    else if (symTable.IsSymbol(second))
                                    {
                                        displacement1 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                        displacement1 /= Convert.ToInt32(first);
                                        displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                                    }

                                    else
                                        throw new InvalidSymbolException();
                                }

                                else if (intermediateParam1.Contains('('))
                                {
                                    int openParanIndex = intermediateParam1.IndexOf('(');
                                    int closeParanIndex = intermediateParam1.IndexOf(')');

                                    /* Format is either D(,B) or D(X,B). */
                                    if (intermediateParam1.Contains(','))
                                    {
                                        int commaIndex = intermediateParam1.IndexOf(',');

                                        /* Format is D(,B). */
                                        if (openParanIndex == commaIndex - 1)
                                        {
                                            displacement1 = Convert.ToInt32(intermediateParam1.Substring(0, openParanIndex));
                                            base1 = Convert.ToInt32(intermediateParam1.Substring(commaIndex + 1, closeParanIndex - commaIndex - 1));
                                            index1 = 0;
                                        }

                                        /* Format is D(X,B). */
                                        else
                                        {
                                            displacement1 = Convert.ToInt32(intermediateParam1.Substring(0, openParanIndex));
                                            index1 = Convert.ToInt32(intermediateParam1.Substring(openParanIndex + 1, commaIndex - openParanIndex - 1));
                                            base1 = Convert.ToInt32(intermediateParam1.Substring(commaIndex + 1, closeParanIndex - commaIndex - 1));
                                        }
                                    }

                                    /* Format is D(X). */
                                    else
                                    {
                                        displacement1 = Convert.ToInt32(intermediateParam1.Substring(0, openParanIndex));
                                        index1 = Convert.ToInt32(intermediateParam1.Substring(openParanIndex + 1, closeParanIndex - openParanIndex - 1));
                                        base1 = 0;
                                    }
                                }

                                /* Format is D. */
                                else if (Int32.TryParse(intermediateParam1, out displacement1))
                                {
                                    displacement1 = Convert.ToInt32(intermediateParam1);
                                    index1 = 0;
                                    base1 = 0;
                                }

                                else
                                    throw new InvalidSymbolException();

                                /* Cannot modify storage when a literal is used instead of storage. */
                                if (literalPools[numLiteralPools].IsLiteral(intermediateParam2))
                                    throw new IllegalUseOfLiteralException();

                                if (intermediateParam2 != "")
                                    length2 = Convert.ToInt32(intermediateParam2);
                                else
                                {
                                    if (symTable.IsSymbol(intermediateParam1))
                                        length2 = symTable.GetSymbolSize(intermediateParam1);
                                    else if (literalPools[numLiteralPools].IsLiteral(intermediateParam1))
                                    {
                                        int delimiter = intermediateParam1.IndexOf('\'');
                                        if (delimiter < 0)
                                            delimiter = intermediateParam1.IndexOf('(');

                                        string type = intermediateParam1.Substring(1, delimiter - 1);
                                        string litVal = intermediateParam1.Substring(delimiter + 1, intermediateParam1.Length - delimiter - 2);

                                        switch (type)
                                        {
                                            case "C":
                                                length2 = litVal.Length;
                                                break;
                                            case "X":
                                                if (litVal.Length == 1)
                                                    length2 = 1;
                                                else
                                                    length2 = litVal.Length / 2;
                                                break;
                                            case "P":
                                                length2 = litVal.Length + 1;
                                                if (length2 % 2 == 1)
                                                    length2++;
                                                break;
                                            case "Z":
                                                length2 = litVal.Length * 2;
                                                break;
                                            case "H":
                                                length2 = 4;
                                                break;
                                            case "F":
                                                length2 = 4;
                                                break;
                                            case "D":
                                                length2 = 8;
                                                break;
                                        }
                                    }
                                }
                                    

                                /* 
                                 * The index or base registers are out of range, or the length is too
                                 * large. 
                                 */
                                if (index1 < 0 || index1 > 15 || base1 < 0 || base1 > 15)
                                    throw new RegisterOutOfRangeException();
                                else if (length2 < 0 || length2 > 65535)
                                    throw new FormatException();

                                objCode1 = index1.ToString("X");
                                objCode2 = base1.ToString("X") + displacement1.ToString("X").PadLeft(3, '0');
                                objCode3 = length2.ToString("X").PadLeft(4, '0');
                                break;

                            default:
                                break;
                        }
                    }

                    /* The instruction is an assembler directive. */
                    else if (assemblerDirectives.Contains(intermediateInstruction))
                    {
                        if (intermediateInstruction == "DC")
                        {
                            int openDelimiter = intermediateParam1.IndexOf('\'');
                            if (openDelimiter < 0)
                                openDelimiter = intermediateParam1.IndexOf('(');

                            int closeDelimiter = intermediateParam1.LastIndexOf('\'');
                            if (closeDelimiter < 0)
                                closeDelimiter = intermediateParam1.LastIndexOf(')');

                            string constantVal = "";
                            if (openDelimiter >= 0 && closeDelimiter >= 0)
                                constantVal = intermediateParam1.Substring(openDelimiter + 1, closeDelimiter - openDelimiter - 1);

                            int constantLength;
                            int addressTypeValue = 0;

                            /*
                             ********************************************************
                             *        The constant is the Address type.             *
                             ********************************************************
                             */
                            if (intermediateParam1.StartsWith("A("))
                            {
                                /* The constant was only one address. */
                                if (symTable.IsSymbol(constantVal))
                                    objCode1 = symTable.GetAddress(constantVal).PadLeft(8, '0');
                                else if (Int32.TryParse(constantVal, out addressTypeValue) && constantVal.Length <= 6)
                                    objCode1 = addressTypeValue.ToString("X");

                                /* The constant is multiple labels. */
                                else if (intermediateParam1.Contains(','))
                                {
                                    string[] multiSymbols = constantVal.Split(',');

                                    if (multiSymbols.Length > 10)
                                        throw new TooManyOperandsInDCException();

                                    foreach (string symbol in multiSymbols)
                                    {
                                        if (symTable.IsSymbol(symbol))
                                            objCode1 += symTable.GetAddress(symbol).PadLeft(8, '0');
                                        else if (Int32.TryParse(symbol, out addressTypeValue) && symbol.Length <= 6)
                                            objCode1 = addressTypeValue.ToString("X");
                                        else
                                            throw new InvalidSymbolException();
                                    }
                                }

                                else
                                    objCode1 = Convert.ToInt32(constantVal).ToString("X").PadLeft(8, '0');
                                
                            }

                            else if (intermediateParam1.StartsWith("AL"))
                            {
                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2)) * 2;

                                /* The constant was only one address. */
                                if (symTable.IsSymbol(constantVal))
                                    objCode1 = symTable.GetAddress(constantVal).PadLeft(8, '0');

                                /* The constant is multiple labels. */
                                else if (intermediateParam1.Contains(','))
                                {
                                    string[] multiSymbols = constantVal.Split(',');

                                    if (multiSymbols.Length > 10)
                                        throw new TooManyOperandsInDCException();

                                    foreach (string symbol in multiSymbols)
                                    {
                                        if (symTable.IsSymbol(symbol))
                                            objCode1 += symTable.GetAddress(symbol).PadLeft(8, '0');
                                        else
                                            throw new InvalidSymbolException();
                                    }
                                }

                                else
                                    objCode1 = Convert.ToInt32(constantVal).ToString("X").PadLeft(8, '0');

                                for (int i = objCode1.Length; i > constantLength; i--)
                                    objCode1 = objCode1.Remove(0, 1);

                                objCode1 = objCode1.PadLeft(constantLength, '0');
                            }

                            /*
                             ********************************************************
                             *        The constant is the Binary type.              *
                             ********************************************************
                             */
                            else if (intermediateParam1.StartsWith("B'"))
                            {
                                foreach (char character in constantVal)
                                {
                                    if (!VALID_CHARS_BINARY.Contains(character))
                                        throw new IllegalCharacterException();
                                }

                                objCode1 = Convert.ToInt32(constantVal, 2).ToString("X");
                                if (objCode1.Length % 2 != 0)
                                    objCode1 = "0" + objCode1;
                            }

                            else if (intermediateParam1.StartsWith("BL"))
                            {
                                foreach (char character in constantVal)
                                {
                                    if (!VALID_CHARS_BINARY.Contains(character))
                                        throw new IllegalCharacterException();
                                }

                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2)) * 2;

                                objCode1 = Convert.ToInt32(constantVal, 2).ToString("X");

                                for (int i = objCode1.Length; i > constantLength; i--)
                                    objCode1 = objCode1.Remove(0, 1);

                                objCode1 = objCode1.PadLeft(constantLength, '0');
                            }

                            /*
                             ********************************************************
                             *        The constant is the Character type.           *
                             ********************************************************
                             */
                            else if (intermediateParam1.StartsWith("C'"))
                            {
                                for (int i = 0; i < constantVal.Length; i++)
                                    objCode1 += ToEBCDIC(constantVal[i]);
                            }

                            else if (intermediateParam1.StartsWith("CL")) 
                            {
                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2));

                                for (int i = 0; i < constantLength && i < constantVal.Length; i++)
                                    objCode1 += ToEBCDIC(constantVal[i]);

                                for (int i = (objCode1.Length / 2); i < constantLength; i++)
                                    objCode1 += "40";
                            }

                            /*
                             ********************************************************
                             *        The constant is the Double type.              *
                             ********************************************************
                             */
                            else if (intermediateParam1.StartsWith("D'"))
                            {
                                foreach (char character in constantVal)
                                {
                                    if (!VALID_CHARS_NUMBERS.Contains(character))
                                        throw new IllegalCharacterException();
                                }

                                if ((ulong)Convert.ToInt64(constantVal) > MAX_DOUBLE)
                                    throw new ExpressionTooLargeException();

                                constantVal = Convert.ToInt64(constantVal).ToString("X");
                                objCode1 = constantVal.PadLeft(16, '0');
                            }

                            else if (intermediateParam1.StartsWith("DL"))
                            {
                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2)) * 2;

                                objCode1 = Convert.ToInt32(constantVal).ToString("X");

                                for (int i = objCode1.Length; i > constantLength; i--)
                                    objCode1 = objCode1.Remove(0, 1);

                                objCode1 = objCode1.PadLeft(constantLength, '0');
                            }

                            /*
                             ********************************************************
                             *        The constant is the Full type.                *
                             ********************************************************
                             */
                            else if (intermediateParam1.StartsWith("F'"))
                            {
                                foreach (char character in constantVal)
                                {
                                    if (!VALID_CHARS_NUMBERS.Contains(character))
                                        throw new IllegalCharacterException();
                                }

                                if ((ulong)Convert.ToInt64(constantVal) > MAX_FULL)
                                    throw new ExpressionTooLargeException();

                                constantVal = Convert.ToInt64(constantVal).ToString("X");
                                objCode1 = constantVal.PadLeft(8, '0');
                            }

                            else if (intermediateParam1.StartsWith("FL"))
                            {
                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2)) * 2;

                                objCode1 = Convert.ToInt32(constantVal).ToString("X");

                                for (int i = objCode1.Length; i > constantLength; i--)
                                    objCode1 = objCode1.Remove(0, 1);

                                objCode1 = objCode1.PadLeft(constantLength, '0');
                            }

                            /*
                             ********************************************************
                             *        The constant is the Half type.                *
                             ********************************************************
                             */
                            else if (intermediateParam1.StartsWith("H'"))
                            {
                                foreach (char character in constantVal)
                                {
                                    if (!VALID_CHARS_NUMBERS.Contains(character))
                                        throw new IllegalCharacterException();
                                }

                                if ((ulong)Convert.ToInt64(constantVal) > MAX_HALF)
                                    throw new ExpressionTooLargeException();

                                constantVal = Convert.ToInt64(constantVal).ToString("X");
                                objCode1 = constantVal.PadLeft(4, '0');
                            }

                            else if (intermediateParam1.StartsWith("HL"))
                            {
                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2)) * 2;

                                objCode1 = Convert.ToInt32(constantVal).ToString("X");

                                for (int i = objCode1.Length; i > constantLength; i--)
                                    objCode1 = objCode1.Remove(0, 1);

                                objCode1 = objCode1.PadLeft(constantLength, '0');
                            }

                            /*
                             ********************************************************
                             *        The constant is the Pack type.                *
                             ********************************************************
                             */
                            else if (intermediateParam1.StartsWith("P'"))
                            {
                                foreach (char character in constantVal)
                                {
                                    if (!VALID_CHARS_NUMBERS.Contains(character))
                                        throw new IllegalCharacterException();
                                }

                                Int64 packConstantVal = Convert.ToInt64(constantVal);

                                if (constantVal.Length > MAX_PACK_LENGTH)
                                    throw new ExpressionTooLargeException();

                                objCode1 = constantVal;
                                if (packConstantVal >= 0)
                                    objCode1 += "C";
                                else
                                    objCode1 += "D";

                                if (objCode1.Length % 2 != 0)
                                    objCode1 = "0" + objCode1;
                            }

                            else if (intermediateParam1.StartsWith("PL"))
                            {
                                foreach (char character in constantVal)
                                {
                                    if (!VALID_CHARS_NUMBERS.Contains(character))
                                        throw new IllegalCharacterException();
                                }

                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2)) * 2;

                                Int64 packConstantVal = Convert.ToInt64(constantVal);
                                if (constantVal.Length > MAX_PACK_LENGTH)
                                    throw new ExpressionTooLargeException();

                                objCode1 = constantVal;
                                if (packConstantVal >= 0)
                                    objCode1 += "C";
                                else
                                    objCode1 += "D";

                                if (objCode1.Length % 2 != 0)
                                    objCode1 = "0" + objCode1;

                                for (int i = objCode1.Length; i > constantLength; i--)
                                    objCode1 = objCode1.Remove(0, 1);

                                objCode1 = objCode1.PadLeft(constantLength, '0');
                            }

                            /*
                             ********************************************************
                             *        The constant is the Hex type.                 *
                             ********************************************************
                             */
                            else if (intermediateParam1.StartsWith("X'"))
                            {
                                if (constantVal.Length % 2 == 1)
                                    constantVal = "0" + constantVal;

                                for (int i = 0; i < constantVal.Length; i++)
                                {
                                    if (!VALID_CHARS_HEX.Contains(constantVal[i]))
                                        throw new IllegalCharacterException();

                                    objCode1 += constantVal[i];
                                }
                            }

                            else if (intermediateParam1.StartsWith("XL"))
                            {
                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2)) * 2;

                                if (constantVal.Length % 2 == 1)
                                    constantVal = "0" + constantVal;

                                for (int i = 0; i < constantVal.Length; i++)
                                {
                                    if (!VALID_CHARS_HEX.Contains(constantVal[i]))
                                        throw new IllegalCharacterException();

                                    objCode1 += constantVal[i];
                                }

                                for (int i = objCode1.Length; i > constantLength; i--)
                                    objCode1 = objCode1.Remove(0, 1);

                                objCode1 = objCode1.PadLeft(constantLength, '0');
                            }

                            /*
                             ********************************************************
                             *        The constant is the Zone type.                *
                             ********************************************************
                             */
                            else if (intermediateParam1.StartsWith("Z'"))
                            {
                                Int64 zoneConstantVal = -1;
                                Int64.TryParse(constantVal, out zoneConstantVal);

                                for (int i = 0; i < constantVal.Length - 1; i++)
                                {
                                    /* Uses NUMBERS because only contains digits. */
                                    if (!VALID_CHARS_NUMBERS.Contains(constantVal[i]))
                                        throw new IllegalCharacterException();

                                    if (constantVal[i] != '+' && constantVal[i] != '-')
                                        objCode1 += "F" + constantVal[i];
                                }

                                if (zoneConstantVal < 0)
                                    objCode1 += "D" + constantVal[constantVal.Length - 1];
                                else
                                    objCode1 += "C" + constantVal[constantVal.Length - 1];
                            }

                            else if (intermediateParam1.StartsWith("ZL"))
                            {
                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2)) * 2;

                                Int64 zoneConstantVal = -1;
                                Int64.TryParse(constantVal, out zoneConstantVal);

                                for (int i = 0; i < constantVal.Length - 1; i++)
                                {
                                    /* Uses NUMBERS because only contains digits. */
                                    if (!VALID_CHARS_NUMBERS.Contains(constantVal[i]))
                                        throw new IllegalCharacterException();

                                    if (constantVal[i] != '+' && constantVal[i] != '-')
                                        objCode1 += "F" + constantVal[i];
                                }

                                if (zoneConstantVal < 0)
                                    objCode1 += "D" + constantVal[constantVal.Length - 1];
                                else
                                    objCode1 += "C" + constantVal[constantVal.Length - 1];

                                for (int i = objCode1.Length; i > constantLength; i--)
                                    objCode1 = objCode1.Remove(0, 1);

                                objCode1 = objCode1.PadLeft(constantLength, '0');
                            }

                            /*
                             ********************************************************
                             *        The constant is the V-Address type.           *
                             ********************************************************
                             */
                            else if (intermediateParam1.StartsWith("V("))
                            {
                                /* The constant was only one address. */
                                if (symTable.IsSymbol(constantVal))
                                    objCode1 = symTable.GetAddress(constantVal).PadLeft(8, '0');
                                else if (Int32.TryParse(constantVal, out addressTypeValue) && constantVal.Length <= 6)
                                    objCode1 = addressTypeValue.ToString("X");

                                /* The constant is multiple labels. */
                                else if (intermediateParam1.Contains(','))
                                {
                                    string[] multiSymbols = constantVal.Split(',');

                                    if (multiSymbols.Length > 10)
                                        throw new TooManyOperandsInDCException();

                                    foreach (string symbol in multiSymbols)
                                    {
                                        if (symTable.IsSymbol(symbol))
                                            objCode1 += symTable.GetAddress(symbol).PadLeft(8, '0');
                                        else if (Int32.TryParse(symbol, out addressTypeValue) && symbol.Length <= 6)
                                            objCode1 = addressTypeValue.ToString("X");
                                        else
                                            throw new InvalidSymbolException();
                                    }
                                }

                                else
                                    objCode1 = Convert.ToInt32(constantVal).ToString("X").PadLeft(8, '0');

                            }

                            else if (intermediateParam1.StartsWith("VL"))
                            {
                                constantLength = Convert.ToInt32(intermediateParam1.Substring(2, openDelimiter - 2)) * 2;

                                /* The constant was only one address. */
                                if (symTable.IsSymbol(constantVal))
                                    objCode1 = symTable.GetAddress(constantVal).PadLeft(8, '0');

                                /* The constant is multiple labels. */
                                else if (intermediateParam1.Contains(','))
                                {
                                    string[] multiSymbols = constantVal.Split(',');

                                    if (multiSymbols.Length > 10)
                                        throw new TooManyOperandsInDCException();

                                    foreach (string symbol in multiSymbols)
                                    {
                                        if (symTable.IsSymbol(symbol))
                                            objCode1 += symTable.GetAddress(symbol).PadLeft(8, '0');
                                        else
                                            throw new InvalidSymbolException();
                                    }
                                }

                                else
                                    objCode1 = Convert.ToInt32(constantVal).ToString("X").PadLeft(8, '0');

                                for (int i = objCode1.Length; i > constantLength; i--)
                                    objCode1 = objCode1.Remove(0, 1);

                                objCode1 = objCode1.PadLeft(constantLength, '0');
                            }








                            ///* The constant is in hex representation. */
                            //else if (intermediateParam1.StartsWith("X'"))
                            //{
                            //    string tempCharString = intermediateParam1.Substring(openDelimiter + 1, intermediateParam1.Length - 3);

                            //    if (tempCharString.Length % 2 == 1)
                            //        tempCharString = "0" + tempCharString;
                               
                            //    for (int i = 0; i < tempCharString.Length; i++)
                            //    {
                            //        if (!VALID_CHARS_HEX.Contains(tempCharString[i]))
                            //            throw new IllegalCharacterException();

                            //        objCode1 += tempCharString[i];
                            //    }
                                    
                            //}

                            //else if (intermediateParam1.StartsWith("F'"))
                            //{
                            //    string tempCharString = intermediateParam1.Substring(openDelimiter + 1, intermediateParam1.Length - 3);

                            //    foreach (char character in tempCharString)
                            //    {
                            //        if (!VALID_CHARS_NUMBERS.Contains(character))
                            //            throw new IllegalCharacterException();
                            //    }

                            //    if ((ulong)Convert.ToInt64(tempCharString) > MAX_FULL)
                            //        throw new ExpressionTooLargeException();

                            //    tempCharString = (Convert.ToInt64(tempCharString)).ToString("X");
                            //    objCode1 = tempCharString.PadLeft(8, '0');
                            //}

                            ///* The constant is in packed representation. */
                            //else if (intermediateParam1.StartsWith("P'"))
                            //{
                            //    string packConstantString = intermediateParam1.Substring(openDelimiter + 1, intermediateParam1.Length - 3);

                            //    foreach (char character in packConstantString)
                            //    {
                            //        if (!VALID_CHARS_NUMBERS.Contains(character))
                            //            throw new IllegalCharacterException();
                            //    }
                                
                            //    Int64 packConstantVal = Convert.ToInt64(packConstantString);

                            //    if (packConstantString.Length > MAX_PACK_LENGTH)
                            //        throw new ExpressionTooLargeException();

                            //    objCode1 = packConstantString;
                            //    if (packConstantVal >= 0)
                            //        objCode1 += "C";
                            //    else
                            //        objCode1 += "D";

                            //    if (objCode1.Length % 2 != 0)
                            //        objCode1 = "0" + objCode1;
                            //}

                            ///* The constant is in zoned decimal representation. */
                            //else if (intermediateParam1.StartsWith("Z'"))
                            //{
                            //    string zoneConstantString = intermediateParam1.Substring(openDelimiter + 1, intermediateParam1.Length - 3);
                            //    Int64 zoneConstantVal = -1;
                            //    Int64.TryParse(zoneConstantString, out zoneConstantVal);

                            //    for (int i = 0; i < zoneConstantString.Length - 1; i++)
                            //    {
                            //        /* Uses WORDS because only contains digits. */
                            //        if (!VALID_CHARS_NUMBERS.Contains(zoneConstantString[i]))
                            //            throw new IllegalCharacterException();

                            //        if (zoneConstantString[i] != '+' && zoneConstantString[i] != '-')
                            //            objCode1 += "F" + zoneConstantString[i];
                            //    }

                            //    if (zoneConstantVal < 0)
                            //        objCode1 += "D" + zoneConstantString[zoneConstantString.Length-1];
                            //    else
                            //        objCode1 += "C" + zoneConstantString[zoneConstantString.Length - 1];
                            //}

                            ///* The constant is a halfword. */
                            //else if (intermediateParam1.StartsWith("H'"))
                            //{
                            //    string tempCharString = intermediateParam1.Substring(openDelimiter + 1, intermediateParam1.Length - 3);

                            //    foreach (char character in tempCharString)
                            //    {
                            //        if (!VALID_CHARS_NUMBERS.Contains(character))
                            //            throw new IllegalCharacterException();
                            //    }

                            //    if (Convert.ToInt32(tempCharString) > MAX_HALF)
                            //        throw new ExpressionTooLargeException();

                            //    tempCharString = Convert.ToInt64(tempCharString).ToString("X");

                            //    objCode1 = tempCharString.PadLeft(4, '0');
                            //}

                            ///* The constant is a fullword. */
                            //else if (intermediateParam1.StartsWith("F'"))
                            //{
                            //    string tempCharString = intermediateParam1.Substring(openDelimiter + 1, intermediateParam1.Length - 3);

                            //    foreach (char character in tempCharString)
                            //    {
                            //        if (!VALID_CHARS_NUMBERS.Contains(character))
                            //            throw new IllegalCharacterException();
                            //    }

                            //    if ((ulong)Convert.ToInt64(tempCharString) > MAX_FULL)
                            //        throw new ExpressionTooLargeException();

                            //    tempCharString = (Convert.ToInt64(tempCharString)).ToString("X");
                            //    objCode1 = tempCharString.PadLeft(8, '0');
                            //}

                            ///* The constant is a doubleword. */
                            //else if (intermediateParam1.StartsWith("D'"))
                            //{
                            //    string tempCharString = intermediateParam1.Substring(openDelimiter + 1, intermediateParam1.Length - 3);

                            //    foreach (char character in tempCharString)
                            //    {
                            //        if (!VALID_CHARS_NUMBERS.Contains(character))
                            //            throw new IllegalCharacterException();
                            //    }

                            //    if ((ulong)Convert.ToInt64(tempCharString) > MAX_DOUBLE)
                            //        throw new ExpressionTooLargeException();

                            //    tempCharString = Convert.ToInt64(tempCharString).ToString("X");
                            //    objCode1 = tempCharString.PadLeft(16, '0');
                            //}

                            ///* The constant is an address type. */
                            //else if (intermediateParam1.StartsWith("A(") || intermediateParam1.StartsWith("V("))
                            //{
                            //    /* Seperate the label(s) out of the parameter. */
                            //    string addressSymbol = intermediateParam1.Substring(2, intermediateParam1.Length - 3);

                            //    /* The constant was only one address. */
                            //    if (symTable.IsSymbol(addressSymbol))
                            //        objCode1 = symTable.GetAddress(addressSymbol).PadLeft(8, '0');

                            //    /* The constant is multiple labels. */
                            //    else if (intermediateParam1.Contains(','))
                            //    {
                            //        string[] multiSymbols = addressSymbol.Split(',');

                            //        if (multiSymbols.Length > 10)
                            //            throw new TooManyOperandsInDCException();

                            //        foreach (string symbol in multiSymbols)
                            //        {
                            //            if (symTable.IsSymbol(symbol))
                            //                objCode1 += symTable.GetAddress(symbol).PadLeft(8, '0');
                            //            else
                            //                throw new InvalidSymbolException();
                            //        }
                            //    }
                            //}

                            /* Format was not given. */
                            else
                                throw new IllegalConstantTypeException();

                        }

                        /* Instruction is DS. */
                        else if (intermediateInstruction == "DS")
                        {
                            int storageSize = 0;
                            int duplication = 1;

                            /*
                             ********************************************************
                             *        The storage is the Address type.              *
                             ********************************************************
                             */
                            if (intermediateParam1.EndsWith("A"))
                            {
                                if (intermediateParam1.IndexOf("A") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("A")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 1;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("AL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("AL") + 2;

                                if (intermediateParam1.IndexOf("AL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("AL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "AL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            /*
                             ********************************************************
                             *        The storage is the Binary type.               *
                             ********************************************************
                             */
                            else if (intermediateParam1.EndsWith("B"))
                            {
                                if (intermediateParam1.IndexOf("B") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("B")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 1;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("BL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("BL") + 2;

                                if (intermediateParam1.IndexOf("BL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("BL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "BL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            /*
                             ********************************************************
                             *        The storage is the Character type.            *
                             ********************************************************
                             */
                            else if (intermediateParam1.EndsWith("C"))
                            {
                                if (intermediateParam1.IndexOf("C") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("C")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 1;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("CL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("CL") + 2;

                                if (intermediateParam1.IndexOf("CL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("CL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "CL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            /*
                             ********************************************************
                             *        The storage is the Double type.               *
                             ********************************************************
                             */
                            else if (intermediateParam1.EndsWith("D"))
                            {
                                if (intermediateParam1.IndexOf("D") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("D")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 8;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("DL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("DL") + 2;

                                if (intermediateParam1.IndexOf("DL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("DL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "DL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            /*
                             ********************************************************
                             *        The storage is the Full type.                 *
                             ********************************************************
                             */
                            else if (intermediateParam1.EndsWith("F"))
                            {
                                if (intermediateParam1.IndexOf("F") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("F")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 4;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("FL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("FL") + 2;

                                if (intermediateParam1.IndexOf("FL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("FL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "FL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            /*
                             ********************************************************
                             *        The storage is the Half type.                 *
                             ********************************************************
                             */
                            else if (intermediateParam1.EndsWith("H"))
                            {
                                if (intermediateParam1.IndexOf("H") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("H")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 2;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("HL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("HL") + 2;

                                if (intermediateParam1.IndexOf("HL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("HL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "HL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            /*
                             ********************************************************
                             *        The storage is the Pack type.                 *
                             ********************************************************
                             */
                            else if (intermediateParam1.EndsWith("P"))
                            {
                                if (intermediateParam1.IndexOf("P") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("P")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 1;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("PL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("PL") + 2;

                                if (intermediateParam1.IndexOf("PL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("PL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "PL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            /*
                             ********************************************************
                             *        The storage is the V-Address type.            *
                             ********************************************************
                             */
                            else if (intermediateParam1.EndsWith("V"))
                            {
                                if (intermediateParam1.IndexOf("V") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("V")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 1;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("VL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("VL") + 2;

                                if (intermediateParam1.IndexOf("VL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("VL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "VL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                                /*
                             ********************************************************
                             *        The storage is the Hex type.                  *
                             ********************************************************
                             */
                            else if (intermediateParam1.EndsWith("X"))
                            {
                                if (intermediateParam1.IndexOf("X") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("X")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 1;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("XL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("XL") + 2;

                                if (intermediateParam1.IndexOf("XL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("XL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "XL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            /*
                             ********************************************************
                             *        The storage is the Zone type.                 *
                             ********************************************************
                             */
                            else if (intermediateParam1.EndsWith("Z"))
                            {
                                if (intermediateParam1.IndexOf("Z") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("Z")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                storageSize = 1;

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else if (intermediateParam1.Contains("ZL"))
                            {
                                instructionIndex = intermediateParam1.IndexOf("ZL") + 2;

                                if (intermediateParam1.IndexOf("ZL") > 0)
                                    duplication = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.IndexOf("ZL")));

                                if (duplication < 0 || duplication > MAX_DUPLICATION)
                                    throw new IllegalDuplicationFactorException();

                                if (intermediateParam1 == "ZL")
                                    storageSize = 1;
                                else
                                    storageSize = Convert.ToInt32(intermediateParam1.Substring(instructionIndex));

                                storageSize *= duplication;
                                for (int i = 0; i < storageSize; i++)
                                    objCode1 += "F5";

                                objCode2 = "";
                                objCode3 = "";
                            }

                            else
                                throw new FormatException();
                        }

                        /* Instruction was USING directive. */
                        else if (intermediateInstruction == "USING")
                        {
                            baseRegister = intermediateParam2;

                            if (symTable.IsSymbol(intermediateParam1))
                                baseRegisterContents = symTable.GetAddress(intermediateParam1);
                            else
                                throw new RelocatableExpressionRequiredException();
                        }

                        /* Instruction was LTORG or END directives. */
                        else if (intermediateInstruction == "LTORG" || intermediateInstruction == "END")
                        {
                            if (intermediateInstruction == "END")
                            {
                                if (!symTable.IsSymbol(intermediateParam1))
                                    throw new InvalidSymbolException();
                            }
                            
                            if (!literalPools[numLiteralPools].IsTableEmpty())
                            {
                                Int16 literalHalfNumericValue = 0;
                                Int32 literalFullNumericValue = 0;
                                Int64 literalDoubleNumericValue = 0;
                                string[] literals = literalPools[numLiteralPools].GetLiteralsList();
                                string[] addresses = literalPools[numLiteralPools].GetAddressesList();
                                int[] intAddresses = new int[addresses.Length];

                                for (int i = 0; i < addresses.Length; i++)
                                    intAddresses[i] = Convert.ToInt32(addresses[i], 16);

                                Array.Sort(intAddresses, literals);

                                string literalObjectCode;
                                string literalType = "";
                                string literalValue = "";
                                int addressTypeValue = 0;

                                /* Generate the literal object code. */
                                for (int i = 0; i < literals.Length; i++)
                                {
                                    literalObjectCode = "";
                                    literals[i] = literals[i].TrimEnd();

                                    line = String.Format("{0,12} {1}", " ", literals[i]);

                                    literalType = literals[i].Substring(0, 2);
                                    literalValue = literals[i].Substring(3, literals[i].Length - 4);

                                    switch (literalType)
                                    {
                                        case "=A":
                                            /* The constant is an address. */
                                            if (symTable.IsSymbol(literalValue))
                                                literalObjectCode = symTable.GetAddress(literalValue).PadLeft(8, '0');
                                            else if (Int32.TryParse(literalValue, out addressTypeValue) && literalValue.Length <= 6)
                                                literalObjectCode = addressTypeValue.ToString("X").PadLeft(8, '0');

                                            else if (literalValue.Contains(","))
                                            {
                                                string[] symbols = literalValue.Split(',');
                                                foreach (string sym in symbols)
                                                {
                                                    if (symTable.IsSymbol(sym))
                                                        literalObjectCode += symTable.GetAddress(sym).PadLeft(8, '0');
                                                    else if (Int32.TryParse(literalValue, out addressTypeValue) && literalValue.Length <= 6)
                                                        literalObjectCode = addressTypeValue.ToString("X").PadLeft(8, '0');
                                                    else
                                                        throw new InvalidSymbolException();
                                                }
                                            }
                                            break;

                                        case "=B":
                                            literalObjectCode = Convert.ToInt32(literalValue, 2).ToString("X");
                                            if (literalObjectCode.Length % 2 != 0)
                                                literalObjectCode = "0" + literalObjectCode;
                                            break;

                                        case "=C":
                                            /* The literal is a character string. */
                                            foreach (char character in literalValue)
                                                literalObjectCode += ToEBCDIC(character);
                                            break;

                                        case "=D":
                                            /* The constant is a doubleword. */
                                            Int64.TryParse(literalValue, out literalDoubleNumericValue);
                                            literalObjectCode = literalDoubleNumericValue.ToString("X");
                                            literalObjectCode = literalObjectCode.PadLeft(16, '0');
                                            break;

                                        case "=F":
                                            /* The constant is a fullword. */
                                            Int32.TryParse(literalValue, out literalFullNumericValue);
                                            literalObjectCode = literalFullNumericValue.ToString("X");
                                            literalObjectCode = literalObjectCode.PadLeft(8, '0');
                                            break;

                                        case "=H":
                                            /* The literal is a halfword. */
                                            Int16.TryParse(literalValue, out literalHalfNumericValue);
                                            literalObjectCode = literalHalfNumericValue.ToString("X");
                                            literalObjectCode = literalObjectCode.PadLeft(4, '0');
                                            break;

                                        case "=P":
                                            /* The literal is a packed value. */
                                            literalObjectCode = literalValue;
                                            Int64 temp;
                                            int objectCodeLength = literalObjectCode.Length;
                                            if (Int64.TryParse(literalValue, out temp))
                                            {
                                                if (temp >= 0)
                                                    literalObjectCode += "C";
                                                else
                                                {
                                                    literalObjectCode += "D";
                                                    literalObjectCode = literalObjectCode.Remove(0, 1);
                                                }
                                            }

                                            else
                                            {
                                                while (literalObjectCode.Length < literalValue.Length)
                                                    literalObjectCode += "F5";
                                            }
                                            
                                            if (literalObjectCode.Length % 2 != 0)
                                                literalObjectCode = "0" + literalObjectCode;
                                            break;

                                        case "=V":
                                            if (symTable.IsSymbol(literalValue))
                                                literalObjectCode = symTable.GetAddress(literalValue).PadLeft(8, '0');
                                            else if (Int32.TryParse(literalValue, out addressTypeValue) && literalValue.Length <= 6)
                                                literalObjectCode = addressTypeValue.ToString("X").PadLeft(8, '0');

                                            else if (literalValue.Contains(","))
                                            {
                                                string[] symbols = literalValue.Split('\'');
                                                foreach (string sym in symbols)
                                                {
                                                    if (symTable.IsSymbol(sym))
                                                        literalObjectCode = symTable.GetAddress(sym).PadLeft(8, '0');
                                                    else if (Int32.TryParse(literalValue, out addressTypeValue) && literalValue.Length <= 6)
                                                        literalObjectCode = addressTypeValue.ToString("X").PadLeft(8, '0');
                                                    else
                                                        throw new InvalidSymbolException();
                                                }
                                            }
                                            break;

                                        case "=X":
                                            /* The literal is hex type. */
                                            literalObjectCode = literalValue;
                                            if (literalObjectCode.Length % 2 == 1)
                                                literalObjectCode = "0" + literalObjectCode;
                                            break;

                                        case "=Z":
                                            /* The literal is a zone value. */
                                            objectCodeLength = literalValue.Length;
                                            if (Int64.TryParse(literalValue, out temp))
                                            {
                                                for (int j = 0; j < objectCodeLength - 1; j++)
                                                {
                                                    /* Uses WORDS because only contains digits. */
                                                    if (!VALID_CHARS_NUMBERS.Contains(literalValue[j]))
                                                        throw new IllegalCharacterException();

                                                    if (literalValue[j] != '+' && literalValue[j] != '-')
                                                        literalObjectCode += "F" + literalValue[j];
                                                }

                                                if (objectCodeLength < 0)
                                                    literalObjectCode += "D" + literalValue[objectCodeLength - 1];
                                                else
                                                    literalObjectCode += "C" + literalValue[objectCodeLength - 1];
                                            }

                                            else
                                            {
                                                while (literalObjectCode.Length < literalValue.Length)
                                                    literalObjectCode += "F5";
                                            }

                                            if (literalObjectCode.Length % 2 != 0)
                                                literalObjectCode = "0" + literalObjectCode;

                                            break;

                                        default:
                                            break;
                                    }
                                    int objCodeLength = objCode.Length;
                                    int literalLength = literalObjectCode.Length;

                                    while ((objCodeLength / 2) < (intAddresses[i]))
                                    {
                                        literalObjectCode = "F5" + literalObjectCode;
                                        objCodeLength += 2;
                                    }
                                        
                                    /* Add the object code of the literals. */
                                    objCode += literalObjectCode;
                                }
                            }

                            numLiteralPools++;
                        }

                        /*
                         * ORG is currently not supported. The code for the first attempt has just
                         * been commented out.
                         * 
                         * The original developers of this application's interpretation of the 
                         * functionality of ORG differed from the actual.
                         * 
                        else if (intermediateInstruction == "ORG")
                        {
                            orgFound = true;
                            //if (tempLength != interLocCtrVal)
                            //    objCode = objCode.Remove(interLocCtrVal, intermediateObjSize);     
                        }
                         * */
                    }

                    else
                        intermediateInstruction = "";
                }

                else
                    intermediateLocationCounter = "0";
            }

            /* Format is SS and did not include length. */
            catch (ArgumentOutOfRangeException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Invalid operand format", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            /* The expression given was too large for the format. */
            catch (ExpressionTooLargeException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Expression too large", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            /* The operands were not formatted correctly. */
            catch (FormatException)   
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Addressibility error", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            /* An invalid character was used for the given type. */
            catch (IllegalCharacterException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Illegal character", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            /* Constant type specified not valid. */
            catch (IllegalConstantTypeException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Illegal constant type", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            /* Duplication factor used w */
            catch (IllegalDuplicationFactorException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Illegal duplication factor", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            catch (IllegalUseOfLiteralException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Illegal use of literal", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            /* Symbol specified was not a valid symbol. */
            catch (InvalidConstantException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Invalid constant", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            /* Symbol specified was not a valid symbol. */
            catch (InvalidSymbolException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Undefined symbol", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            catch (MissingOperandException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Missing operand", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

             /* The expression given was too large for the format. */
            catch (OverflowException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Expression too large", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            /* Register specified was not between 0 and 15. */
            catch (RegisterOutOfRangeException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Invalid register specified", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            catch (RelocatableExpressionRequiredException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Relocatable expression required", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            catch (TooManyOperandsInDCException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Too many operands in DC", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            catch (UnresolvedExternalAddressException)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Unresolved external address", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            catch (Exception)
            {
                AddError(Convert.ToInt32(intermediateLineNumber), "Syntax", inputLine);

                intermediateInstructionOpCode = "";
                objCode1 = "";
                objCode2 = "";
                objCode3 = "";
            }

            /* Set object code. */
            finally
            {
                /* Add the object code for the instruction to the end of the object code string. */
                if (intermediateInstruction != "LTORG" && intermediateInstruction != "END")
                {
                    string tempObjCode = intermediateInstructionOpCode + objCode1 + objCode2 + objCode3;
                    int tempLength = objCode.Length;

                    int interLocCtrVal = (Convert.ToInt32(intermediateLocationCounter, 16) * 2);
/*       
                    int objectSizeCharacters = intermediateObjSize * 2;
*/               
                    /* 
                     * Add slack bytes if needed for boundary alignment. 
                     * This is determined by calculating the length of the existing object code 
                     * with the location counter and adding the bytes to reach it.
                     * 
                     * The lines commented out may be referenced later to fully implement the 
                     * ORG directive.
                     */
/*
                    if (!orgFound)
                    {
*/
                    while (tempLength < interLocCtrVal)
                    {
                        tempObjCode = "F5" + tempObjCode;
                        tempLength += 2;
                    }
                    objCode += tempObjCode;
/*                    }

                    else
                    {
                        if (objCode.Length >= interLocCtrVal + objectSizeCharacters)
                        {
                            objCode = objCode.Remove(interLocCtrVal, objectSizeCharacters);
                            objCode = objCode.Insert(interLocCtrVal, tempObjCode);
                        }
                        else
                        {
                            while (tempLength < interLocCtrVal)
                            {
                                tempObjCode = "F5" + tempObjCode;
                                tempLength += 2;
                            }
                            objCode += tempObjCode;
                        }
                    }
 */ 
                }
            }
        }

        /******************************************************************************************
         * 
         * Name:        ProcessLineSourceCode     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              Drew Aaron
         *              
         * Input:       Line from source code as string, streamwriter objects for prt and
         *              intermediate files.      
         * Return:      N/A 
         * Description: This method processes the source code line that is passed to it. The method
         *              pulls out the label, operation and parameters.
         *              
         *****************************************************************************************/
        private bool ProcessLineSourceCode(string inputLine) 
        {
            /* Reset variables. */
            line = inputLine.TrimEnd();
            label = "";
            instruction = "";
            objCode1 = "";
            objCode2 = "";
            parameters = "";
            previousLocation = locationCounter;

            int increment = 0;

            string[] parameterList = new string[3];
            for (int i = 0; i < 3; i++)
                parameterList[i] = "";

            string locationCounterHex = "";

            bool neededForIntermediate = false;

            /* Open the writing stream to the intermediate file. */
            StreamWriter intermediateOutStream = new StreamWriter(intermediateFile, true);

            try
            {
                /* The line is blank so flag error. */
                if (line == "")
                {
                    if (startCardFound)
                        throw new InvalidOperationException();
                    else
                        throw new IllegalStartCardException();
                }

                /* Makes sure the number of lines has not exceeded the limit. */
                if (numLines >= optionsLines)
                    throw new ExceededMaxLinesException();

                /* 
                 * If the first column contains a *, the row is commented out so ignore line. 
                 * If #, the row is an option line so ignore.
                 */
                if (line[0] != '*' && line[0] != '#')
                {
                    if (line.Length > 71)
                        throw new ContinuationCardColsNonblankException();
                    
                    neededForIntermediate = true;

                    /* Store the label portion of the line and validate. */
                    label = line.Substring(0, 8).TrimEnd();

                    if (label != "")
                        if (!ValidateLabel())
                            return false;

                    /* If column 9 is not a space, save the error. */
                    if (line[8] != ' ')
                        throw new InvalidLabelException();

                    /* Store the instruction portion of the line. */
                    instruction = line.Substring(9, 5).TrimEnd();

                    /* If the instruction field is not one of the the directives, handle normally. */
                    if (!assemblerDirectives.Contains(instruction))
                    {
                        numInstructions++;
                        increment = ValidateInstruction();

                        /* If column 15 is not a space, send the error. */
                        if ((line.Length > 14) && (line[14] != ' '))
                            throw new InvalidOperationException();

                        /* 
                         * Store the parameters portion of the line and validate.
                         * Only the parameters are stored (up to the first space character).
                         * This is so no comments are stored as a parameter.
                         * The parameterLastIndex is used as the length to get the parameters.
                         */
                        int parameterLastIndex = line.IndexOf(' ', 15);
                        parameterLastIndex -= 14;

                        /* There were no comments at the end of the line. */
                        if (parameterLastIndex < 0)
                            parameters = line.Substring(15);

                        /* 
                         * The first parameter is a character string literal. 
                         * This catch is here because of the presence of spaces in the strings. 
                         */
                        else if (line.Substring(15).Contains("=C"))
                        {
                            /* The first parameter is the character string. */
                            if (line.IndexOf(',', 15) > line.IndexOf('='))
                            {
                                parameterLastIndex = line.IndexOf(',', 15);
                                parameterLastIndex = line.IndexOf(' ', parameterLastIndex) - 14;

                                if (parameterLastIndex < 0)
                                    parameterLastIndex = line.Length - 15;
                            }

                            /* The second parameter is the character string. */
                            else
                            {
                                int openDelimit = line.IndexOf('\'', 15);
                                int closeDelimit = line.IndexOf('\'', openDelimit + 1);

                                parameterLastIndex = closeDelimit - 14;
                            }

                            /* Save the parameters. If the characters weren't found, save error. */
                            if (parameterLastIndex >= 0)
                                parameters = line.Substring(15, parameterLastIndex);
                            else
                                throw new FormatException();
                        }

                        /* 
                        * The first parameter is a character immediate. 
                        * This catch is here because of the presence of spaces in the strings. 
                        */
                        else if (line.Substring(15).Contains("C'"))
                        {
                            /* The first parameter is the character string. */
                            if (line.IndexOf(',', 15) > line.IndexOf("C'"))
                            {
                                parameterLastIndex = line.IndexOf(',', 15);
                                parameterLastIndex = line.IndexOf(' ', parameterLastIndex) - 14;

                                if (parameterLastIndex < 0)
                                    parameterLastIndex = line.Length - 15;
                            }

                            /* The second parameter is the character string. */
                            else
                            {
                                int openDelimit = line.IndexOf('\'', 15);
                                int closeDelimit = line.IndexOf('\'', openDelimit + 1);

                                parameterLastIndex = closeDelimit - 14;
                            }

                            /* Save the parameters. If the characters weren't found, save error. */
                            if (parameterLastIndex >= 0)
                                parameters = line.Substring(15, parameterLastIndex);
                            else
                                throw new FormatException();
                        }

                        /* The parameters have no extra spaces to worry about. */
                        else
                            parameters = line.Substring(15, parameterLastIndex);

                        ValidateParameters();
                    }

                    /* The field is a Declared Storage field. */
                    else if (instruction == "DS")
                    {
                        int index = 0;
                        increment = 0;
                        
                        if (line.IndexOf(' ', 15) < 0)
                            parameters = line.Substring(15);
                        else
                            parameters = line.Substring(15, line.IndexOf(' ', 15) - 14).TrimEnd();

                        /*
                         ********************************************************
                         *        The storage is the Address type.              *
                         ********************************************************
                         */
                        if (parameters.EndsWith("A"))
                        {
                            index = parameters.IndexOf("A");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 4;
                            increment *= duplication;

                            while (previousLocation % 4 != 0)
                                previousLocation++;
                            locationCounter = previousLocation + increment;
                        }

                        else if (parameters.Contains("AL"))
                        {
                            if (parameters == "AL" || parameters.IndexOf("AL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("AL");

                            int duplication = 1;
                            if (parameters.IndexOf("AL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            if (parameters == "AL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index + 2));

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        /*
                         ********************************************************
                         *        The storage is the Binary type.            *
                         ********************************************************
                         */
                        else if (parameters.EndsWith("B"))
                        {
                            index = parameters.IndexOf("B");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 1;

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        else if (parameters.Contains("BL"))
                        {
                            if (parameters == "BL" || parameters.IndexOf("BL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("BL") + 2;

                            int duplication = 1;
                            if (parameters.IndexOf("BL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index - 2));

                            if (parameters == "BL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index));

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        /*
                         ********************************************************
                         *        The storage is the Character type.            *
                         ********************************************************
                         */
                        else if (parameters.EndsWith("C"))
                        {
                            index = parameters.IndexOf("C");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 1;

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        else if (parameters.Contains("CL"))
                        {
                            if (parameters == "CL" || parameters.IndexOf("CL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("CL") + 2;

                            int duplication = 1;
                            if (parameters.IndexOf("CL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index - 2));

                            if (parameters == "CL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index));

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        /*
                         ********************************************************
                         *        The storage is the Double type.               *
                         ********************************************************
                         */
                        else if (parameters.EndsWith("D"))
                        {
                            index = parameters.IndexOf("D");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 8;
                            increment *= duplication;

                            while (previousLocation % 8 != 0)
                                previousLocation++;
                            locationCounter = previousLocation + increment;
                        }

                        else if (parameters.Contains("DL"))
                        {
                            if (parameters == "DL" || parameters.IndexOf("DL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("DL") + 2;

                            int duplication = 1;
                            if (parameters.IndexOf("DL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index - 2));

                            if (parameters == "DL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index));

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        /*
                         ********************************************************
                         *        The storage is the Full type.                 *
                         ********************************************************
                         */
                        else if (parameters.EndsWith("F"))
                        {
                            index = parameters.IndexOf("F");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 4;
                            increment *= duplication;

                            while (previousLocation % 4 != 0)
                                previousLocation++;
                            locationCounter = previousLocation + increment;
                        }

                        else if (parameters.Contains("FL"))
                        {
                            if (parameters == "FL" || parameters.IndexOf("FL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("FL") + 2;

                            int duplication = 1;
                            if (parameters.IndexOf("FL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index - 2));

                            if (parameters == "FL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index));

                            increment *= duplication;
                            locationCounter += increment;

                            //while (previousLocation % 4 != 0)
                            //    previousLocation++;
                            //locationCounter = previousLocation + increment;
                        }

                        /*
                         ********************************************************
                         *        The storage is the Half type.                 *
                         ********************************************************
                         */
                        else if (parameters.EndsWith("H"))
                        {
                            index = parameters.IndexOf("H");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 2;
                            increment *= duplication;

                            while (previousLocation % 2 != 0)
                                previousLocation++;
                            locationCounter = previousLocation + increment;
                        }

                        else if (parameters.Contains("HL"))
                        {
                            if (parameters == "HL" || parameters.IndexOf("HL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("HL") + 2;

                            int duplication = 1;
                            if (parameters.IndexOf("HL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index - 2));

                            if (parameters == "HL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index));

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        /*
                         ********************************************************
                         *        The storage is the Packed type.               *
                         ********************************************************
                         */
                        else if (parameters.EndsWith("P"))
                        {
                            index = parameters.IndexOf("P");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 1;
                            increment *= duplication;
                            locationCounter += increment;
                        }

                        else if (parameters.Contains("PL"))
                        {
                            if (parameters == "PL" || parameters.IndexOf("PL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("PL") + 2;

                            int duplication = 1;
                            if (parameters.IndexOf("PL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index - 2));

                            if (parameters == "PL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index));

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        /*
                         ********************************************************
                         *        The storage is the V-Address type.            *
                         ********************************************************
                         */
                        else if (parameters.EndsWith("V"))
                        {
                            index = parameters.IndexOf("V");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 4;
                            increment *= duplication;

                            while (previousLocation % 4 != 0)
                                previousLocation++;
                            locationCounter = previousLocation + increment;
                        }

                        else if (parameters.Contains("VL"))
                        {
                            if (parameters == "VL" || parameters.IndexOf("VL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("VL") + 2;

                            int duplication = 1;
                            if (parameters.IndexOf("VL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index - 2));

                            if (parameters == "VL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index));

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        /*
                         ********************************************************
                         *        The storage is the Hex type.                  *
                         ********************************************************
                         */
                        else if (parameters.EndsWith("X"))
                        {
                            index = parameters.IndexOf("X");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 1;
                            increment *= duplication;
                            locationCounter += increment;
                        }

                        else if (parameters.Contains("XL"))
                        {
                            if (parameters == "XL" || parameters.IndexOf("XL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("XL") + 2;

                            int duplication = 1;
                            if (parameters.IndexOf("XL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index - 2));

                            if (parameters == "XL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index));

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        /*
                         ********************************************************
                         *        The storage is the Zoned type.                *
                         ********************************************************
                         */
                        else if (parameters.EndsWith("Z"))
                        {
                            index = parameters.IndexOf("Z");

                            int duplication = 1;
                            if (index > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index));

                            increment = 1;
                            increment *= duplication;
                            locationCounter += increment;
                        }

                        else if (parameters.Contains("ZL"))
                        {
                            if (parameters == "ZL" || parameters.IndexOf("ZL") == parameters.Length - 2)
                                throw new AbsoluteExpressionRequiredException();

                            index = parameters.IndexOf("ZL") + 2;

                            int duplication = 1;
                            if (parameters.IndexOf("ZL") > 0)
                                duplication = Convert.ToInt32(parameters.Substring(0, index - 2));

                            if (parameters == "ZL")
                                increment = 1;
                            else
                                increment = Convert.ToInt32(parameters.Substring(index));

                            increment *= duplication;
                            locationCounter += increment;
                        }

                        else
                            throw new InvalidDelimiterException();

                        /* Need to throw an else and error here. */
                        if (symTable.IsSymbol(label))
                            symTable.UpdateAddress(label, previousLocation.ToString("X").PadLeft(6, '0'), increment);
                    }

                    /* Field is a Declared Constant field. */
                    else if (instruction == "DC")
                    {
                        int delimiterIndex = 0;
                        increment = 0;

                        /* Must determine if there is a missing delimiter. */
                        if (line.Contains("A(") || line.Contains("V(") ||
                            line.Substring(15).StartsWith("AL") || 
                            line.Substring(15).StartsWith("VL"))
                        {
                            delimiterIndex = line.IndexOf('(', 15) + 1;

                            if (line.IndexOf(')', delimiterIndex) >= 0)
                                parameters = line.Substring(15, line.IndexOf(')', delimiterIndex) - 14);
                            else
                                throw new MissingDelimiterException();

                            increment = 4;
                        }

                        else
                        {
                            delimiterIndex = line.IndexOf('\'', 15) + 1;

                            if (line.IndexOf('\'', delimiterIndex) >= 0)
                                parameters = line.Substring(15, line.IndexOf('\'', delimiterIndex) - 14);
                            else
                                throw new MissingDelimiterException();

                            parameters = line.Substring(15, line.IndexOf('\'', delimiterIndex) - 14);
                        }

                        /*
                         ********************************************************
                         *        The constant is the Address type.             *
                         ********************************************************
                         */
                        if (parameters.StartsWith("A("))
                        {
                            while (previousLocation % 4 != 0)
                                previousLocation++;

                            increment = 4;

                            foreach (char character in parameters)
                            {
                                if (character == ',')
                                    increment += 4;
                            }
                        }

                        else if (parameters.StartsWith("AL"))
                        {
                            delimiterIndex = parameters.IndexOf('(');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf(")", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("AL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }

                        /*
                         ********************************************************
                         *        The constant is the Binary type.              *
                         ********************************************************
                         */
                        else if (parameters.StartsWith("B'"))
                        {
                            for (delimiterIndex = 2; parameters[delimiterIndex] != '\''; delimiterIndex++)
                                increment++;
                            increment /= 4;

                            if (increment == 0)
                                increment++;
                        }

                        else if (parameters.StartsWith("BL"))
                        {                            
                            delimiterIndex = parameters.IndexOf('\'');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf("'", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("BL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }

                        /*
                         ********************************************************
                         *        The constant is the Character type.           *
                         ********************************************************
                         */
                        else if (parameters.StartsWith("C'"))
                        {
                            for (delimiterIndex = 2; parameters[delimiterIndex] != '\''; delimiterIndex++)
                                increment++;
                        }

                        else if (parameters.StartsWith("CL"))
                        {
                            delimiterIndex = parameters.IndexOf('\'');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf("'", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("CL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }

                        /*
                         ********************************************************
                         *        The constant is the Double type.              *
                         ********************************************************
                         */
                        else if (parameters.StartsWith("D'"))
                        {
                            while (previousLocation % 8 != 0)
                                previousLocation++;
                            increment = 8;

                            locationCounter = previousLocation;
                        }

                        else if (parameters.StartsWith("DL"))
                        {
                            delimiterIndex = parameters.IndexOf('\'');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf("'", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("DL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }

                        /*
                         ********************************************************
                         *        The constant is the Full type.                *
                         ********************************************************
                         */
                        else if (parameters.StartsWith("F'"))
                        {
                            while (previousLocation % 4 != 0)
                                previousLocation++;
                            increment = 4;

                            locationCounter = previousLocation;
                        }

                        else if (parameters.StartsWith("FL"))
                        {
                            delimiterIndex = parameters.IndexOf('\'');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf("'", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("FL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }

                        /*
                         ********************************************************
                         *        The constant is the Half type.                *
                         ********************************************************
                         */
                        else if (parameters.StartsWith("H'"))
                        {
                            while (previousLocation % 2 != 0)
                                previousLocation++;
                            increment = 2;

                            locationCounter = previousLocation;
                        }

                        else if (parameters.StartsWith("HL"))
                        {
                            delimiterIndex = parameters.IndexOf('\'');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf("'", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("HL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }

                        /*
                         ********************************************************
                         *        The constant is the Pack type.                *
                         ********************************************************
                         */
                        else if (parameters.StartsWith("P'"))
                        {
                            for (delimiterIndex = 2; parameters[delimiterIndex] != '\''; delimiterIndex++)
                                increment++;

                            if (increment % 2 == 0)
                                increment += 2;
                            else
                                increment++;

                            increment /= 2;
                        }

                        else if (parameters.StartsWith("PL"))
                        {
                            delimiterIndex = parameters.IndexOf('\'');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf("'", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("PL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }

                        /*
                         ********************************************************
                         *        The constant is the Hex type.                 *
                         ********************************************************
                         */
                        else if (parameters.StartsWith("X'"))
                        {
                            for (delimiterIndex = 2; parameters[delimiterIndex] != '\''; delimiterIndex++)
                                increment++;
                            if (increment != 1)
                                increment /= 2;
                        }

                        else if (parameters.StartsWith("XL"))
                        {
                            delimiterIndex = parameters.IndexOf('\'');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf("'", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("XL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }

                        /*
                         ********************************************************
                         *        The constant is the Zone type.                *
                         ********************************************************
                         */
                        else if (parameters.StartsWith("Z'"))
                        {
                            for (delimiterIndex = 2; parameters[delimiterIndex] != '\''; delimiterIndex++)
                                increment++;
                        }

                        else if (parameters.StartsWith("ZL"))
                        {
                            delimiterIndex = parameters.IndexOf('\'');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf("'", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("ZL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }

                        /*
                         ********************************************************
                         *        The constant is the V-Address type.           *
                         ********************************************************
                         */
                        else if (parameters.StartsWith("V("))
                        {
                            while (previousLocation % 4 != 0)
                                previousLocation++;
                            increment = 4;

                            foreach (char character in parameters)
                            {
                                if (character == ',')
                                    increment += 4;
                            }
                        }

                        else if (parameters.StartsWith("VL"))
                        {
                            delimiterIndex = parameters.IndexOf('(');
                            if (delimiterIndex < 0)
                                throw new AbsoluteExpressionRequiredException();
                            else if (parameters.IndexOf(")", delimiterIndex + 1) < 0)
                                throw new MissingDelimiterException();

                            if (delimiterIndex == parameters.IndexOf("VL") + 2)
                                throw new AbsoluteExpressionRequiredException();

                            increment = Convert.ToInt32(parameters.Substring(2, delimiterIndex - 2));
                        }


                        //else if (parameters.StartsWith("X'"))
                        //{
                        //    for (delimiterIndex = 2; parameters[delimiterIndex] != '\''; delimiterIndex++)
                        //        increment++;
                        //    if (increment != 1)
                        //        increment /= 2;
                        //}

                        //else if (parameters.StartsWith("Z'"))
                        //{
                        //    for (delimiterIndex = 2; parameters[delimiterIndex] != '\''; delimiterIndex++)
                        //        increment++;
                        //    //if (increment == 1)
                        //    //    increment = 2;
                        //}

                        //else if (parameters.StartsWith("P'"))
                        //{
                        //    for (delimiterIndex = 2; parameters[delimiterIndex] != '\''; delimiterIndex++)
                        //        increment++;

                        //    if (increment % 2 == 0)
                        //        increment += 2;
                        //    else
                        //        increment++;

                        //    increment /= 2;
                        //}

                        //else if (parameters.StartsWith("H'"))
                        //{
                        //    while (previousLocation % 2 != 0)
                        //        previousLocation++;
                        //    increment = 2;

                        //    locationCounter = previousLocation;
                        //}
                           
                        //else if (parameters.StartsWith("F'"))
                        //{
                        //    while (previousLocation % 4 != 0)
                        //        previousLocation++;
                        //    increment = 4;

                        //    locationCounter = previousLocation;
                        //}

                        //else if (parameters.StartsWith("D'"))
                        //{
                        //    while (previousLocation % 8 != 0)
                        //        previousLocation++;
                        //    increment = 8;

                        //    locationCounter = previousLocation;
                        //}

                        //else if (parameters.StartsWith("A(") || parameters.StartsWith("V("))
                        //{
                        //    increment = 4;

                        //    foreach (char character in parameters)
                        //    {
                        //        if (character == ',')
                        //            increment += 4;
                        //    }

                        //    while (previousLocation % 4 != 0)
                        //        previousLocation++;

                        //    locationCounter = previousLocation;
                        //}
                            

                        locationCounter += increment;

                        if (symTable.IsSymbol(label))
                            symTable.UpdateAddress(label, previousLocation.ToString("X").PadLeft(6, '0'), increment);

                    }

                    /* Field is a Using field. */
                    else if (instruction == "USING")
                    {
                        if (label != "")
                            throw new LabelNotAllowedException();

                        else
                        {
                            if (line.IndexOf(' ', 15) < 0)
                                parameters = line.Substring(15);
                            else
                                parameters = line.Substring(15, line.IndexOf(' ', 15) - 14).TrimEnd();
                            string[] usingParams = parameters.Split(',');

                            baseRegisterContents = usingParams[0];
                            baseRegister = usingParams[1];
                        }
                    }

                    else if (instruction == "END")
                    {
                        endCardFound = true;
                        if (line.IndexOf(" ", 15) < 0)
                            parameters = line.Substring(15);
                        else
                            parameters = line.Substring(15, line.IndexOf(" ", 15) - 15);

                        UpdateLiteralAddresses(previousLocation);
                    }
                        
                    else if (instruction == "LTORG")
                    {
                        UpdateLiteralAddresses(previousLocation);
                        numLiteralPools++;
                        literalPools.Add(new LiteralTable());
                    }

/*
 *                  This section of ORG can be uncommented 
                    else if (instruction == "ORG")
                    {
                        if (line.Length >= 16)
                        {
                            if (line.IndexOf(" ", 15) < 0)
                                parameters = line.Substring(15);
                            else
                                parameters = line.Substring(15, line.IndexOf(" ", 15) - 15);

                            parameterList = parameters.Split(',');

                            if (symTable.IsSymbol(parameterList[0]))
                                previousLocation = Convert.ToInt32(symTable.GetAddress(parameterList[0]), 16);
                            else if (parameterList[0] != "")
                                throw new OrgValueInWrongSectionException();

                            if (parameterList.Length > 1 && parameterList[1] != "")
                            {
                                int boundary = Convert.ToInt32(parameterList[1]);
                                while (previousLocation % boundary != 0)
                                    previousLocation++;
                            }
                            if (parameterList.Length > 2 && parameterList[2] != "")
                                previousLocation += Convert.ToInt32(parameterList[2]);
                        }

                        locationCounter = previousLocation;
                    }
*/
                    locationCounterHex = previousLocation.ToString("X").PadLeft(6, '0');

                    if(symTable.IsSymbol(label))
                        symTable.UpdateAddress(label, locationCounterHex, -1);

                    /* 
                    * Write the items to the intermediate file. 
                    * The instruction and parameters are saved.
                    * They are seperated by the Alert character  '\a'.
                    * Assembler formatting directives are ignored.
                    */
                    if (instruction != "TITLE" && instruction != "SPACE" && instruction != "EJECT")
                    {
                        neededForIntermediate = true;

                        parameters = parameters.TrimEnd();

                        if (instruction == "START" || instruction == "CSECT")
                        {
                            if (label == "")
                                throw new LabelRequiredException();

                            parameterList = null;
                            if (instruction == "START")
                            {
                                if (startCardFound)
                                    throw new IllegalStartCardException();
                                else
                                    startCardFound = true;
                            }

                            else if (instruction == "CSECT")
                            {
                                while (locationCounter % 2 != 0)
                                    locationCounter++;
                                locationCounterHex = locationCounter.ToString("X").PadLeft(6, '0');
                                symTable.UpdateAddress(label, locationCounterHex, 0);
                            }
                        }

                        else if (!startCardFound)
                            throw new IllegalStartCardException();

                        else if (instruction == "END")
                            parameterList = parameters.Split(' ');

                        /* The first parameter is a D(X,B). */
                        else if (parameters.IndexOf(',') > parameters.IndexOf('(') &&
                                 parameters.IndexOf(',') < parameters.IndexOf(')') &&
                                 parameters.IndexOf(',', parameters.IndexOf(',') + 1) < parameters.LastIndexOf('(') &&
                                 parameters.IndexOf(')') >= 0)
                        {
                            string[] tempArray = new string[1];
                            parameterList = parameters.Split(',');

                            if (parameterList.Length == 2)
                            {
                                parameterList[0] += "," + parameterList[1];

                                tempArray = new string[parameterList.Length - 1];
                                Array.Copy(parameterList, tempArray, parameterList.Length - 1);
                            }

                            else if (parameterList.Length == 3)
                            {
                                parameterList[0] += "," + parameterList[1];
                                parameterList[1] = parameterList[2];

                                tempArray = new string[parameterList.Length - 1];
                                Array.Copy(parameterList, tempArray, parameterList.Length - 1);
                            }

                            else if (parameterList.Length == 4)
                            {
                                parameterList[0] += "," + parameterList[1];
                                parameterList[1] = parameterList[2] + "," + parameterList[3];

                                tempArray = new string[parameterList.Length - 2];
                                Array.Copy(parameterList, tempArray, parameterList.Length - 2);
                            }

                            parameterList = tempArray;
                        }

                        else if (instruction == "DC" && 
                                 (parameters.Contains("A(") || parameters.Contains("V(")))
                        {
                            /* Just get the parameters in one entry of array. */
                            parameterList = parameters.Split('!');
                        }

                        /* The parameters are not character literals or D(X,B). */
                        else if (!parameters.Contains("=C") &&
                                    (parameters.LastIndexOf('(') < 0 || (parameters.LastIndexOf(',') < parameters.LastIndexOf('('))))
                        {
                            if (parameters != "")
                                parameterList = parameters.Split(',');
                            else
                                parameterList = null;
                        }
                        
                        /* The first parameter is a string of characters. */
                        else if (parameters.Contains("=C") && 
                                 (parameters.IndexOf("=C") < parameters.IndexOf(',') ||
                                  !parameters.Contains(",")))
                        {
                            string[] tempArray; 

                            parameterList[0] = parameters.Substring(0, parameters.IndexOf('\'', 3) + 1).TrimEnd();
                            if (parameters.IndexOf(',', parameters.IndexOf('\'')) > 0)
                            {
                                tempArray = new string[parameterList.Length - 1];
                                parameterList[1] = parameters.Substring(parameters.LastIndexOf(',') + 1).TrimEnd();
                                Array.Copy(parameterList, tempArray, parameterList.Length - 1);
                            }

                            else
                            {
                                tempArray = new string[parameterList.Length - 2];
                                Array.Copy(parameterList, tempArray, parameterList.Length - 2);
                            }   
                            
                            parameterList = tempArray;
                        }

                        /* The string of characters is the second parameter. */
                        else if (parameters.Contains("=C") && parameters.IndexOf("=C") > parameters.IndexOf(','))
                        {
                            parameterList[0] = parameters.Substring(0, parameters.IndexOf(',')).TrimEnd();
                            parameterList[1] = parameters.Substring(parameters.IndexOf(',') + 1).TrimEnd();

                            string[] tempArray = new string[parameterList.Length - 1];
                            Array.Copy(parameterList, tempArray, parameterList.Length - 1);

                            parameterList = tempArray;
                        }

                        /* The parameters contain =V type. */
                        else if (parameters.Contains("=V") && parameters.IndexOf("=V") > parameters.IndexOf(','))
                        {
                            parameterList[0] = parameters.Substring(0, parameters.IndexOf(',')).TrimEnd();
                            parameterList[1] = parameters.Substring(parameters.IndexOf(',') + 1).TrimEnd();

                            string[] tempArray = new string[parameterList.Length - 1];
                            Array.Copy(parameterList, tempArray, parameterList.Length - 1);

                            parameterList = tempArray;
                        }

                        /* The first parameter is D(X,B) or they're D(L,B) or D(B). */
                        else if (parameters.LastIndexOf(',') > parameters.LastIndexOf(')'))
                        {
                            parameterList = parameters.Split(',');

                            if (parameterList.Length == 2)
                            {
                                string first = parameterList[0];
                                string second = parameterList[1];

                                if (!symTable.IsSymbol(first.Substring(0, first.IndexOf('('))))
                                {

                                }
                                if (parameterList[1].StartsWith("=P"))
                                {

                                }
                            }

                            else if (parameterList.Length == 3)
                            {
                                if (parameters.Contains("C','"))
                                {
                                    parameterList[1] += "," + parameterList[2];
                                    parameterList[2] = "";
                                }

                                else
                                {
                                    parameterList[0] += "," + parameterList[1];
                                    parameterList[1] = parameterList[2];
                                }

                                string[] tempArray = new string[parameterList.Length - 1];
                                Array.Copy(parameterList, tempArray, parameterList.Length - 1);

                                parameterList = tempArray;
                            }
                        }

                        /* The last parameter is D(X,B). */
                        else
                        {
                            parameterList = parameters.Split(',');

                            if (parameterList.Length == 3)
                                parameterList[parameterList.Length - 2] += "," + parameterList[parameterList.Length - 1];
                            else if (parameterList.Length == 4)
                            {
                                parameterList[0] += "," + parameterList[1];
                                parameterList[1] = parameterList[2] + "," + parameterList[3];
                            }

                            string[] tempArray = new string[parameterList.Length - 1];
                            Array.Copy(parameterList, tempArray, parameterList.Length - 1);

                            parameterList = tempArray;
                        }
                    }

                    else
                        neededForIntermediate = false;
                }
            }

            catch (AbsoluteExpressionRequiredException)
            {
                AddError(lineNumber + 1, "Absolute expression required", line);
            }

            catch (ArgumentOutOfRangeException)
            {
                AddError(lineNumber + 1, "Syntax", line);
            }

            catch (ContinuationCardColsNonblankException)
            {
                AddError(lineNumber + 1, "Continuation card cols. 1-15 nonblank", line);
            }

            catch (FormatException)
            {
                AddError(lineNumber + 1, "Syntax", line);
            }

            catch (IllegalStartCardException)
            {
                AddError(lineNumber + 1, "Illegal start card", line);
            }

            catch (IndexOutOfRangeException)
            {
                AddError(lineNumber + 1, "Syntax", line);
            }

            catch (InvalidDelimiterException)
            {
                AddError(lineNumber + 1, "Invalid delimiter", line);
            }

            catch (InvalidLabelException)
            {
                AddError(lineNumber + 1, "Invalid symbol", line);
            }

            catch (InvalidOperationException)
            {
                AddError(lineNumber + 1, "Invalid op code", line);
            }

            catch (LabelNotAllowedException)
            {
                AddError(lineNumber + 1, "Label not allowed", line);
            }

            catch (LabelRequiredException)
            {
                AddError(lineNumber + 1, "Label required", line);
            }

            catch (MissingDelimiterException)
            {
                AddError(lineNumber + 1, "Missing delimiter", line);
            }

            catch (OrgValueInWrongSectionException)
            {
                AddError(lineNumber + 1, "Org value in wrong section", line);
            }

            catch (ExceededMaxLinesException)
            {
                MessageBox.Show(String.Format(
                    "The program has exceeded the max number ({0}) of lines.", optionsLines),
                    "Error - Exceeded Max Lines");
                return false;
            }

            catch (Exception)
            {
                AddError(lineNumber + 1, "Syntax", line);
            }

            finally
            {
                if (neededForIntermediate)
                {
                    /* 
                     * Choose how many items to print to intermediate file based on number of 
                     * parameters. 
                     */
                    if (parameterList == null)
                        intermediateOutStream.WriteLine("{0}\a{1}\a{2}\a{3}\a{4}", increment,
                            (lineNumber + 1), locationCounterHex, label, instruction.TrimEnd());

                    else if (parameterList.Length == 1 && parameterList[0] != "")
                        intermediateOutStream.WriteLine("{0}\a{1}\a{2}\a{3}\a{4}\a{5}", increment,
                            (lineNumber + 1), locationCounterHex, label, instruction.TrimEnd(),
                                                        parameterList[0].TrimEnd());

                    else if (parameterList.Length == 2 && parameterList[0] != "")
                        intermediateOutStream.WriteLine("{0}\a{1}\a{2}\a{3}\a{4}\a{5}\a{6}", increment,
                            (lineNumber + 1), locationCounterHex, label, instruction.TrimEnd(),
                            parameterList[0].TrimEnd(), parameterList[1].TrimEnd());

                    else if (parameterList.Length == 3 && parameterList[0] != "" &&
                             parameterList[1] != "" && parameterList[2] != "")
                        intermediateOutStream.WriteLine("{0}\a{1}\a{2}\a{3}\a{4}\a{5}\a{6}\a{7}", increment,
                        (lineNumber + 1), locationCounterHex, label, instruction.TrimEnd(),
                        parameterList[0].TrimEnd(), parameterList[1].TrimEnd(),
                        parameterList[2].TrimEnd());
                    else
                        intermediateOutStream.WriteLine("{0}\a{1}\a{2}\a{3}\a{4}", increment,
                        (lineNumber + 1), locationCounterHex, label, instruction.TrimEnd());

                }

                /* Increment the current line number, close writing stream to intermediate file. */
                lineNumber++;
                numLines++;
                intermediateOutStream.Close();
                programLength = locationCounter.ToString("X").PadLeft(6, '0');
            }
            return true;
        }

        /******************************************************************************************
         * 
         * Name:        ToEBCDIC   
         * 
         * Author(s):   Travis Hunt
         *                  
         * Input:       The value is a char to be converted. 
         * Return:      The result is a string of the EBCDIC character code.
         * Description: This method will return the EBCDIC character code for the input char.
         * 
         *              This code is an altered version based off the code found at the following:
         *              http://kodesharp.blogspot.com/2007/12/c-convert-ascii-to-ebcdic.html
         *              
         *****************************************************************************************/
        private string ToEBCDIC(char value)
        {
            string result = "";

            int[] a2e = new int[256]{
                                        0, 1, 2, 3, 55, 45, 46, 47, 22, 5, 37, 11, 12, 13, 14, 15,
                                        16, 17, 18, 19, 60, 61, 50, 38, 24, 25, 63, 39, 28, 29, 30, 31,
                                        64, 79,127,123, 91,108, 80,125, 77, 93, 92, 78,107, 96, 75, 97,
                                        240,241,242,243,244,245,246,247,248,249,122, 94, 76,126,110,111,
                                        124,193,194,195,196,197,198,199,200,201,209,210,211,212,213,214,
                                        215,216,217,226,227,228,229,230,231,232,233, 74,224, 90, 95,109,
                                        121,129,130,131,132,133,134,135,136,137,145,146,147,148,149,150,
                                        151,152,153,162,163,164,165,166,167,168,169,192,106,208,161, 7,
                                        32, 33, 34, 35, 36, 21, 6, 23, 40, 41, 42, 43, 44, 9, 10, 27,
                                        48, 49, 26, 51, 52, 53, 54, 8, 56, 57, 58, 59, 4, 20, 62,225,
                                        65, 66, 67, 68, 69, 70, 71, 72, 73, 81, 82, 83, 84, 85, 86, 87,
                                        88, 89, 98, 99,100,101,102,103,104,105,112,113,114,115,116,117,
                                        118,119,120,128,138,139,140,141,142,143,144,154,155,156,157,158,
                                        159,160,170,171,172,173,174,175,176,177,178,179,180,181,182,183,
                                        184,185,186,187,188,189,190,191,202,203,204,205,206,207,218,219,
                                        220,221,222,223,234,235,236,237,238,239,250,251,252,253,254,255
                                        };

            try
            {
                value = Convert.ToChar(a2e[(int)value]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

            result = Convert.ToInt32(value).ToString("X");
            return result;
        }

        /******************************************************************************************
         * 
         * Name:        UpdateLiteralAddresses     
         * 
         * Author(s):   Travis Hunt
         *                            
         * Input:       The current location counter as an int.   
         * Return:      N/A   
         * Description: This methods updates the addresses of the literals to the correct addresses.
         *              
         *****************************************************************************************/
        private void UpdateLiteralAddresses(int currentLocation)
        {
            try
            {
                /* Save the literals and addresses (order) as parallel arrays. */
                string[] literals = literalPools[numLiteralPools].GetLiteralsList();
                string[] addresses = literalPools[numLiteralPools].GetAddressesList();

                int[] intAddresses = new int[addresses.Length];

                /* Copy the addresses (order) into an integer array for sorting. */
                for (int i = 0; i < addresses.Length; i++)
                    intAddresses[i] = Convert.ToInt32(addresses[i]);

                /* Sort the literals based on the order they were declared. */
                Array.Sort(intAddresses, literals);

                /* Delete the table to empty it. */
                literalPools[numLiteralPools].ClearTable();

                /* Find the address of the location counter. */
                int currentAddress = currentLocation;

                foreach (string literal in literals)
                {
                    string type = literal.Substring(0, 2);

                    switch (type)
                    {
                        case "=H":
                            while (currentAddress % 2 != 0)
                                currentAddress++;
                            break;
                        case "=A":
                        case "=F":
                        case "=V":
                            while (currentAddress % 4 != 0)
                                currentAddress++;
                            break;
                        case "=D":
                            while (currentAddress % 8 != 0)
                                currentAddress++;
                            break;
                        default:
                            break;
                    }

                    /* Store the literal and address, then determine how much to increment. */
                    literalPools[numLiteralPools].Hash(literal, currentAddress.ToString("X").PadLeft(6, '0'));

                    currentAddress += literalPools[numLiteralPools].GetLiteralSize(literal);
                }
                locationCounter = currentAddress;
                previousLocation = currentAddress;
            }

            /* An error occurred, but is handled by a previous method. */
            catch (Exception)
            {
                
            }
        }

        /******************************************************************************************
         * 
         * Name:        ValidateInstruction     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       N/A    
         * Return:      True if instruction is formatted correctly, false if not.   
         * Description: This methods checks to see if the instruction is in the valid format.
         *              Used for Pass 1.             
         *              
         *****************************************************************************************/
        private int ValidateInstruction()
        {
            /* True if the instruction is a valid operation. */
            if (MachineOpTable.IsOpcode(instruction) >= 0)
            {
                int increment = 0;
                int index = MachineOpTable.IsOpcode(instruction);
                string opType = MachineOpTable.GetOpType(index);

                while (previousLocation % 2 != 0)
                    previousLocation++;
                locationCounter = previousLocation;

                /* Decide how much to increment the location counter from the instruction format. */
                switch (opType)
                {
                    case "RR":
                        increment = 2;
                        locationCounter += 2;
                        break;

                    case "RX":
                    case "RS":
                    case "SI":
                        increment = 4;
                        locationCounter += 4;
                        break;

                    case "X":
                    case "SS":
                        increment = 6;
                        locationCounter += 6;
                        break;
                }
                
                return increment;
            }

            /* Instruction is not valid. */
            else
                AddError(lineNumber + 1, "Invalid op code", line);

            return -1;
        }

        /******************************************************************************************
         * 
         * Name:        ValidateLabel     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       N/A    
         * Return:      True if the label is valid, false if otherwise.  
         * Description: This methods checks to see if the label is in the valid format.
         *              Used for Pass 1.
         *              
         *****************************************************************************************/
        private bool ValidateLabel()
        {
            try
            {
                /* 
             * Find the first space in the label, then make sure the rest of the string is spaces 
             * as well. 
             */
                int firstSpaceIndex = label.IndexOf(' ');

                if (!validChars.Substring(0, 26).Contains(label[0]))
                    throw new InvalidLabelException();

                /* Search the label for invalid characters and store the error if one is found. */
                foreach (char character in label)
                {
                    if (!validChars.Contains(character))
                        throw new InvalidLabelException();
                }

                /* If there is a label and it is not already in the symbol table, it's added. */
                if (label != "" && !label.StartsWith(" "))
                {
                    if (!symTable.IsSymbolFull())
                    {
                        if (!symTable.IsSymbol(label))
                            symTable.Hash(label, locationCounter.ToString("X").PadLeft(6, '0'));
                        else
                            AddError(lineNumber + 1, "Previously defined symbol", line);
                    }

                    else
                        throw new ExceededMaxSymbolsException();
                }

                return true;
            }

            /* Label was not valid. */
            catch (InvalidLabelException)
            {
                AddError(lineNumber + 1, "Invalid symbol", line);
            }

            catch (ExceededMaxSymbolsException)
            {
                MessageBox.Show(String.Format("Max number of symbols ({0}) exceeded, assembly " +
                                      "terminated.", MAX_SYMBOLS), "Error - Exceeded Max Symbols");
                return false;
            }
            return true;
        }

        /******************************************************************************************
         * 
         * Name:        ValidateParameters     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       N/A    
         * Return:      True if parameters are formatted correctly, false if not.  
         * Description: This methods checks to see if the parameters are in the valid format.
         *              Used for Pass 1.
         *              It also handles if there are literals in the parameters.
         *              
         *****************************************************************************************/
        private bool ValidateParameters()
        {
            try
            {
                string[] parameterList;

                parameterList = parameters.Split(',');

                int openParen = parameters.LastIndexOf('(');
                int comma = parameters.LastIndexOf(',');
                int closeParen = parameters.LastIndexOf(')');

                if (openParen > 0 && openParen < comma && comma < closeParen)
                {
                    parameterList[parameterList.Length - 2] += "," + parameterList[parameterList.Length - 1];
                    parameterList[parameterList.Length - 1] = "";
                }

                if (parameterList[0] == "" || (parameterList.Length > 1 && instruction != "END" &&
                    parameterList[1] == " "))
                {
                    AddError(lineNumber + 1, "Missing operand", line);
                    return false;
                }

                foreach (string param in parameterList)
                {
                    /* True if the parameter is a literal. */
                    if (param.StartsWith("="))
                    {
                        /* Check to be sure the delimiter required is there. */
                        if (((param.StartsWith("=C") || param.StartsWith("=X") || param.StartsWith("=P")
                              || param.StartsWith("=H") || param.StartsWith("=F") || param.StartsWith("=D")) &&
                              param.LastIndexOf('\'') <= param.IndexOf('\'')) ||
                            ((param.StartsWith("=A") || param.StartsWith("=V")) && param.IndexOf(')') <= param.IndexOf('(')))
                        {
                            AddError(lineNumber + 1, "Missing delimiter", line);
                            return false;
                        }

                        /* The table is not full. */
                        else if (!literalPools[numLiteralPools].IsLiteralFull())
                        {
                            if (param.Length <= CONSTANT_MAX_LENGTH)
                            {
                                if (!literalPools[numLiteralPools].IsLiteral(param.TrimEnd()))
                                {
                                    numLiterals++;
                                    literalPools[numLiteralPools].Hash(param.TrimEnd(), numLiterals.ToString());
                                }
                            }

                            else
                            {
                                AddError(lineNumber + 1, "Constant too long", line);
                                return false;
                            }
                        }

                        else
                            throw new ExceededMaxLiteralsException();
                    }
                }
                return true;
            }

            /* Number of literals exceeded max allowable amount. */
            catch (ExceededMaxLiteralsException)
            {
                MessageBox.Show(String.Format("Max number of literals ({0}) exceeded, assembly " +
                                              "terminated.", 
                                              (MAX_LITERALS + (MAX_LITERALS * numLiteralPools)), 
                                              "Error - Exceeded Max Literals"));
                return false;
            }

            /* Length of literal exceeded the max allowable length of 112. */
            catch (FormatException)
            {
                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                errorStream[numErrors, ERROR_DETAIL] = String.Format("Error: Literal exceeds max " +
                                                                     "allowable length ({0})", 
                                                                     CONSTANT_MAX_LENGTH);
                errorStream[numErrors, ERROR_SOURCE] = line;
                numErrors++;
                return false;
            }
        }
    }
}
