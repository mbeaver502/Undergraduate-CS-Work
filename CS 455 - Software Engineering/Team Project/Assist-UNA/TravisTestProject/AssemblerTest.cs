using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

/**************************************************************************************************
 * 
 * Name: AssemblerTest
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
 *  
 *************************************************************************************************/

namespace TravisTestProject
{
    class AssemblerTest
    {
        /* Constants. */
        private const int ERROR_COLUMNS = 3;
        private const int ERROR_DETAIL = 1;
        private const int ERROR_LINE = 0;
        private const int ERROR_SOURCE = 2;
        private const int MAX_LINES_PER_PAGE = 41;
        private const int MAX_ERRORS = 100;
        private const string PRT_FOOTER = "*** PROGRAM EXECUTION BEGINNING -\n" +
                        "ANY OUTPUT BEFORE EXECUTION COMPLETE MESSAGE IS PRODUCED BY USER PROGRAM ***\n";
        private const string PRT_HEADER = "  LOC     OBJECT CODE    STMT   SOURCE STATEMENT \t\t\t\t PAGE ";


        /* Private members. */
        private bool endCardFound;
        private bool entryCardFound;
        private bool titleFlag;
        private int currentObjectCodeIndex;
        private int lineNumber;
        private int locationCounter;
        private int numErrors;
        private int numInstructions;
        private int numLines;
        private int numLiterals;
        private int pageNumber;
        private int previousLocation;
        private int optionsInstructions;
        private int optionsLines;
        private int optionsPages;
        private LiteralTable litTable;
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
         * Name:        AssemblerTest (Constructor)     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       The source, intermediate and object file paths as strings, and external 
         *              SymbolTable and LiteralTable objects. 
         * Return:      N/A 
         * Description: The constructor for AssemblerTest.              
         *              
         *****************************************************************************************/
        public AssemblerTest(string id, string source, string prt, string intermediate, string obj,
                             int instructions, int lines, int pages)
        {
            /* Initialize all the data members to default values. */
            numErrors = 0;
            lineNumber = 0;
            locationCounter = 0;
            currentObjectCodeIndex = 0;
            numInstructions = 0;
            numLiterals = 0;

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
            baseRegisterContents = "";
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

            endCardFound = false;
            entryCardFound = false;

            /* Flag for when the TITLE directive is used. */
            titleFlag = false;

            /* Set the file paths. */
            sourceFile = source;
            prtFile = prt;
            intermediateFile = intermediate;
            objectFile = obj;

            /* Set the literal and symbol tables. */
            litTable = new LiteralTable();
            symTable = new SymbolTable();
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
         * Name:        Pass1     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method drives the assembling pass1 process. It will read through each
         *              line of the source code file and send it to the ProcessLine method.
         *              The intermediate file will be set during pass 1.
         *              
         *****************************************************************************************/
        public void Pass1()
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
            while (line == "" || line[0] == '#')
                line = inStream.ReadLine();

            /* Process each line of the source file. */
            while (!inStream.EndOfStream && !line.StartsWith("$ENTRY"))
            {
                ProcessLineSourceCode(line);
                line = inStream.ReadLine();
            }

            if (line.StartsWith("$ENTRY"))
            {
                if (inStream.EndOfStream && line.TrimEnd() != "$ENTRY")
                {
                    int spaceIndex = line.IndexOf(' ', 7);
                    if (spaceIndex >= 0)
                        inputFile = line.Substring(7, spaceIndex - 8);
                    else
                        inputFile = line.Substring(7);

                    string currDirectory = sourceFile.Substring(0, sourceFile.LastIndexOf('\\') + 1);
                    inputFile = currDirectory + inputFile;
                }

                else
                {
                    inputFile = "tempInputFile.txt";
                    inputFileOutStream = new StreamWriter(inputFile);

                    while(!inStream.EndOfStream)
                        inputFileOutStream.WriteLine(inStream.ReadLine());

                    inputFileOutStream.Close();
                }
            }
            

            

            UpdateLiteralAddresses();

            /* Close all the streams. */
            inStream.Close();
        }

        /******************************************************************************************
         * 
         * Name:        Pass2     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method drives the assembling pass2 process. 
         *              
         *****************************************************************************************/
        public void Pass2()
        {
            /* Open up streams to read from the source and intermediate files. */
            StreamReader inStream = new StreamReader(sourceFile);
            StreamReader intermediateInStream = new StreamReader(intermediateFile);
            StreamWriter prtOutStream = new StreamWriter(prtFile);
            StreamWriter objectOutStream = new StreamWriter(objectFile);

            /* Reset the line number and location counter values for writing to the prt. */
            lineNumber = 0;
            locationCounter = 0;

            /* Generate the object code from the intermediate file. */
            while (!intermediateInStream.EndOfStream)
                ProcessLineIntermediateFile(intermediateInStream.ReadLine());

            /* Initialize the prt file. */
            prtOutStream.WriteLine("ASSIST/UNA Version 1.0 {0,20}{1} \n\n", "GRADE RUN FOR: ", identifier);
            prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");

            /* Close the stream to the prt so it can be written to in the processing method. */
            prtOutStream.Close();

            line = inStream.ReadLine();
            while (!inStream.EndOfStream && !line.StartsWith("$ENTRY"))
            {
                GeneratePRT(line);
                line = inStream.ReadLine();
            }   

            /* Reopen the writing stream to the prt file to write the footer, then close it. */
            prtOutStream = new StreamWriter(prtFile, true);
            prtOutStream.WriteLine("\n\n*** {0} STATEMENTS FLAGGED - {0} ERRORS FOUND", numErrors);
            if (numErrors == 0)
                prtOutStream.WriteLine("\n\n{0}", PRT_FOOTER);

            /* Clear the seperating character in the object code and write to the object file. */
            objCode = objCode.Replace("|", "");
            objCode = objCode.Replace(" ", "");
            objectOutStream.WriteLine(objCode);

            /* Close the streams. */
            objectOutStream.Close();
            prtOutStream.Close();
            inStream.Close();
            intermediateInStream.Close();
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
         * Name:        GeneratePRT    
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       Line from source code as string.    
         * Return:      N/A 
         * Description: This method takes in a line from the source code and prints it to the prt.
         *              It also makes a call to the ProcessLineIntermediateFile method to generate
         *              the object machine code.
         *              
         *****************************************************************************************/
        private void GeneratePRT(string inputLine)
        {
            /* Reset variables. */
            line = inputLine.TrimEnd();
            label = "";
            instruction = "";
            objCode1 = "";
            objCode2 = "";
            objCode3 = "";
            parameters = "";

            /* Open writing stream to the prt and reading stream to intermediate file. */
            StreamWriter prtOutStream = new StreamWriter(prtFile, true);
            StreamReader intermediateInStream = new StreamReader(intermediateFile);

            /* If #, the row is an option line so ignore. */
            if (line != "" && line[0] != '#')
            {
                /* The line is a comment so just write the line. */
                if (line[0] == '*')
                {
                    prtOutStream.WriteLine("                        " +
                                       String.Format("{0,4} {1}", (lineNumber + 1), line));
                    //lineNumber++;
                }

                    /* Temporary? */

                /* The line is not a comment line. */
                else
                {
                    string[] tempIntermediateLine;
                    /* Store the label and instruction portions of the line. */
                    label = line.Substring(0, 8).TrimEnd();
                    instruction = line.Substring(9, 5).TrimEnd();

                    /* If the instruction is an assembler directive, handle the formatting. */
                    if (assemblerDirectives.Contains(instruction))
                    {
                        switch (instruction)
                        {
                            /*
                            * The instruction is START. Just print the line and line number, but
                            * but do not increment the location counter.
                            */
                            case "CSECT":
                            case "START":
                                string locationCounterHex = "000000";
                                line = String.Format("{0,11} {1}", (lineNumber + 1), line);
                                prtOutStream.WriteLine(String.Format("{0,6} {1,4} {2,4} {3}",
                                                                 locationCounterHex, objCode1,
                                                                 objCode2, line));
                                currentObjectCodeIndex++;
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

                                string[] literals = litTable.GetLiteralsList();
                                string[] addresses = litTable.GetAddressesList();
                                int[] intAddresses = new int[addresses.Length];

                                for (int i = 0; i < addresses.Length; i++)
                                    intAddresses[i] = Convert.ToInt32(addresses[i], 16);

                                Array.Sort(intAddresses, literals);

                                string literalObjectCode;
                                string literalType = "";
                                string literalValue = "";
                                string prtObjectCode;

                                for (int i = 0; i < literals.Length; i++)
                                {
                                    literalObjectCode = "";
                                    prtObjectCode = "";
                                    literals[i] = literals[i].TrimEnd();

                                    line = String.Format("{0,12} {1}", " ",
                                                         literals[i]);

                                    literalType = literals[i].Substring(0, 2);
                                    literalValue = literals[i].Substring(3, literals[i].Length - 4);

                                    switch (literalType)
                                    {
                                        case "=C":
                                            foreach (char character in literalValue)
                                                literalObjectCode += ToEBCDIC(character);

                                            break;
                                            
                                        case "=P":
                                            
                                            literalObjectCode = literalValue;

                                            if (Convert.ToInt32(literalValue) >= 0)
                                                literalObjectCode += "C";
                                            else
                                                literalObjectCode += "D";

                                            int objectCodeLength = literalObjectCode.Length;
                                            if (objectCodeLength % 2 != 0)
                                                literalObjectCode = "0" + literalObjectCode;

                                            break;

                                        default:
                                            break;
                                    }

                                    objCode += literalObjectCode.PadRight(12, ' ') + "|";

                                    for (int j = 0; j < 16 && j < literalObjectCode.Length; j++)
                                        prtObjectCode += literalObjectCode[j]; 
                                    

                                    prtOutStream.WriteLine(String.Format("{0,6} {1,-17} {2}",
                                                                 intAddresses[i].ToString("X").PadLeft(6,'0'),
                                                                 prtObjectCode, line));
                                    lineNumber++;
                                }

                                break;

                            /*
                             * The instruction is USING. 
                             * Set the base register and do not increment the location counter.
                             */
                            case "USING":
                                //locationCounterHex = locationCounter.ToString("X").PadLeft(6, '0');

                                tempIntermediateLine = intermediateInStream.ReadLine().
                                                                Split('|');
                                while ((lineNumber + 1).ToString() != tempIntermediateLine[0])
                                    tempIntermediateLine = intermediateInStream.ReadLine().
                                                                    Split('|');

                                locationCounterHex = tempIntermediateLine[1];

                                line = String.Format("{0,11} {1}", (lineNumber + 1), line);
                                prtOutStream.WriteLine(String.Format("{0,6} {1,4} {2,4} {3}",
                                                                 locationCounterHex, "",
                                                                 "", line));

                                baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                                currentObjectCodeIndex++;
                                break;

                            /* The instruction is DS. */
                            case "DS":

                                tempIntermediateLine = intermediateInStream.ReadLine().Split('|');


                                while ((lineNumber + 1).ToString() != tempIntermediateLine[0])
                                    tempIntermediateLine = intermediateInStream.ReadLine().
                                                                            Split('|');

                                locationCounterHex = tempIntermediateLine[1];


                                line = String.Format("{0,11} {1}", (lineNumber + 1), line);


                                prtOutStream.WriteLine(String.Format("{0,6} {1,4} {2,4} {3}",
                                                                 locationCounterHex, "",
                                                                 "", line));
                                currentObjectCodeIndex++;
                                break;

                            /* The instruction is DS. */
                            case "DC":

                                tempIntermediateLine = intermediateInStream.ReadLine().
                                                                Split('|');
                                while ((lineNumber + 1).ToString() != tempIntermediateLine[0])
                                    tempIntermediateLine = intermediateInStream.ReadLine().
                                                                    Split('|');

                                locationCounterHex = tempIntermediateLine[1];

                                line = String.Format("{0,3} {1}", (lineNumber + 1), line);

                                string[] objectCode = objCode.Split('|');

                                objCode1 = objectCode[currentObjectCodeIndex].Substring(0, 16);

                                prtOutStream.WriteLine(String.Format("{0,6} {1,-17} {2}",
                                                                 locationCounterHex, objCode1,
                                                                 line));

                                currentObjectCodeIndex++;
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
                                    if (line.IndexOf(' ', 15) < 0)
                                        spaces = Convert.ToInt32(line.Substring(15));
                                    else
                                        spaces = Convert.ToInt32(line.Substring(15, line.IndexOf(' ', 15) - 14));
                                }
                                for (int i = 0; i < spaces; i++)
                                    prtOutStream.WriteLine();

                                //lineNumber--;
                                break;

                            /* 
                             * The instruction is TITLE.
                             * Creates a title header and displays to the top of another page. 
                             */
                            case "TITLE":
                                int index = line.IndexOf('\'') + 1;
                                int lastIndex = line.LastIndexOf('\'');

                                pageNumber++;

                                /* Makes sure the number of pages does not exceed the limit. */
                                if (pageNumber < optionsPages)
                                {
                                    prtTitleHeader = line.Substring(index, lastIndex - index);

                                    prtOutStream.WriteLine("\f\n{0}", prtTitleHeader);
                                    prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");

                                    titleFlag = true;
                                }

                                /* The number of pages exceeded the limit. */
                                else
                                {
                                    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                    errorStream[numErrors, ERROR_DETAIL] = "Max number of pages " +
                                                                           "reached.";
                                    errorStream[numErrors, ERROR_SOURCE] = line;
                                    numErrors++;
                                    return;
                                }
                                break;

                            /*
                             * The instruction is EJECT.
                             * Starts writing to the prt on a new page. 
                             */
                            case "EJECT":
                                pageNumber++;

                                /* Makes sure the number of pages does not exceed the limit. */
                                if (pageNumber < optionsPages)
                                {
                                    prtOutStream.WriteLine("\f");
                                    if (titleFlag)
                                        prtOutStream.WriteLine(prtTitleHeader + "\n");
                                    prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");
                                    //lineNumber--;
                                }

                                /* The number of pages exceeded the limit. */
                                else
                                {
                                    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                    errorStream[numErrors, ERROR_DETAIL] = "Max number of pages " +
                                                                           "reached.";
                                    errorStream[numErrors, ERROR_SOURCE] = line;
                                    numErrors++;
                                    return;
                                }
                                break;

                            default:
                                break;
                        }
                    }

                    /* The instruction was not an assembler directive. */
                    else
                    {
                        tempIntermediateLine = intermediateInStream.ReadLine().
                                                                Split('|');
                        while ((lineNumber + 1).ToString() != tempIntermediateLine[0])
                            tempIntermediateLine = intermediateInStream.ReadLine().
                                                                    Split('|');

                        string locationCounterHex = tempIntermediateLine[1];

                        line = String.Format("{0,6} {1}", (lineNumber + 1), line);

                        string[] objectCode = objCode.Split('|');

                        objCode1 = objectCode[currentObjectCodeIndex].Substring(0, 4);
                        objCode2 = objectCode[currentObjectCodeIndex].Substring(4, 4);
                        objCode3 = objectCode[currentObjectCodeIndex].Substring(8, 4);

                        prtOutStream.WriteLine("{0,6} {1,4} {2,4} {3,4} {4}", locationCounterHex,
                                               objCode1, objCode2, objCode3, line);

                        bool errorPrintedForLine = false;
                        for (int i = 0; i < numErrors && !errorPrintedForLine; i++)
                        {
                            int errorLine = Convert.ToInt32(errorStream[i, ERROR_LINE]);
                            if (lineNumber + 1 == errorLine)
                            {
                                prtOutStream.WriteLine("{0,33} $\n{0,29}Error, {1}", " ",
                                                       errorStream[i, ERROR_DETAIL]);
                                errorPrintedForLine = true;
                            }
                                
                        }

                        currentObjectCodeIndex++;
                    }
                }
                lineNumber++;

                if (lineNumber % MAX_LINES_PER_PAGE == 0)
                {
                    pageNumber++;

                    /* Makes sure the number of pages does not exceed the limit. */
                    if (pageNumber < optionsPages)
                    {
                        prtOutStream.WriteLine("\f");
                        if (titleFlag)
                            prtOutStream.WriteLine(prtTitleHeader);
                        prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");
                    }

                    /* The number of pages exceeded the limit. */
                    else
                    {
                        errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                        errorStream[numErrors, ERROR_DETAIL] = "Max number of pages " +
                                                               "reached.";
                        errorStream[numErrors, ERROR_SOURCE] = line;
                        numErrors++;
                    }
                }
            }

            /* Close the prt and intermediate read/write streams. */
            prtOutStream.Close();
            intermediateInStream.Close();
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
            string[] items = inputLine.Split('|');
            string intermediateLabel = items[2];
            string intermediateInstruction = items[3];
            string intermediateParam1 = "";
            string intermediateParam2 = "";
            string intermediateParam3 = "";
            objCode1 = "";
            objCode2 = "";
            objCode3 = "";

            if (symTable.IsSymbol(baseRegisterContents))
                baseRegisterContents = symTable.GetAddress(baseRegisterContents);


            /* There's only one parameter. */
            if (items.Length == 5)
                intermediateParam1 = items[4];

            /* There's only two parameters. */
            else if (items.Length == 6)
            {
                intermediateParam1 = items[4];
                intermediateParam2 = items[5];
            }

            /* There's three parameters. */
            else if (items.Length == 7)
            {
                intermediateParam1 = items[4];
                intermediateParam2 = items[5];
                intermediateParam3 = items[6];
            }


            /* Will need to do machine op look up here. */
            int index = MachineOpTableTest.IsOpcode(intermediateInstruction);

            /* True if instruction is a valid operation. */
            if (index >= 0)
            {
                //int register;
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

                intermediateInstruction = MachineOpTableTest.GetObjectCode(index);

                string format = MachineOpTableTest.GetOpType(index);

                /* Generate the correct object code based on the instruction format. */
                switch (format)
                {
                    case "RR":
                        /* Convert the string decimal numbers to integers so they convert to hex. */
                        register1 = Convert.ToInt32(intermediateParam1);

                        /* Make sure the register specified is valid. */
                        if (register1 >= 0 || register1 <= 15)
                        {
                            objCode1 = register1.ToString("X");

                            /* 
                             * Checks against being BR (opcode is 07). 
                             * If the instruction was BR, set register2 to -1. 
                             */
                            if (intermediateInstruction == "07")
                                register2 = -1;
                            else
                                register2 = Convert.ToInt32(intermediateParam2);
                        }

                        else
                        {
                            errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                            errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format, " +
                                                                   "register is invalid.";
                            errorStream[numErrors, ERROR_SOURCE] = line;
                            numErrors++;
                        }

                        /* If register2 is negative, the instruction is BR with set mask of F. */
                        if (register2 < 0)
                            objCode1 = "F" + register1.ToString("X");
                        else
                            objCode1 = register1.ToString("X") + register2.ToString("X");
                        break;

                    case "RX":
                        /* The first parameter is a mask. */
                        if (intermediateParam1.StartsWith("B'"))
                            register1 = Convert.ToInt32(intermediateParam1.Substring(2, 4), 2);

                        /* The first parameter is a label for the B (opcode is 47). */
                        else if (intermediateInstruction == "47" && symTable.IsSymbol(intermediateParam1))
                        {
                            base2 = Convert.ToInt32(baseRegister);
                            register1 = -1;
                            index1 = 0;

                            displacement2 = Convert.ToInt32(symTable.GetAddress(intermediateParam1), 16);
                            displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                        }

                        /* The first parameter is a register. */
                        else
                        {
                            /* Convert the string decimal numbers to integers so they convert to hex. */
                            register1 = Convert.ToInt32(intermediateParam1);

                            /* Make sure the register specified is valid. */
                            if (register1 < 0 || register1 > 15)
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format, " +
                                                                       "register is invalid.";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }


                        if (intermediateParam2 != "")
                        {
                            /* The second parameter is a label (with no extra increment). */
                            if (symTable.IsSymbol(intermediateParam2))
                            {
                                index1 = 0;
                                base2 = Convert.ToInt32(baseRegister);
                                displacement2 = Convert.ToInt32(symTable.GetAddress(intermediateParam2), 16);
                                displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                            }

                            /* The second parameter is a literal. */
                            else if (litTable.IsLiteral(intermediateParam2.TrimEnd()))
                            {
                                index1 = 0;
                                base2 = Convert.ToInt32(baseRegister);
                                displacement2 = Convert.ToInt32(litTable.GetAddress(intermediateParam2), 16);
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
                                    displacement2 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                    displacement2 += Convert.ToInt32(second);
                                    displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                /* The label is second. */
                                else if (symTable.IsSymbol(second))
                                {
                                    displacement2 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                    displacement2 += Convert.ToInt32(first);
                                    displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                else
                                {
                                    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                    errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                    errorStream[numErrors, ERROR_SOURCE] = line;
                                    numErrors++;
                                }
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
                                    displacement2 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                    displacement2 -= Convert.ToInt32(second);
                                    displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                /* The label is second. */
                                else if (symTable.IsSymbol(second))
                                {
                                    displacement2 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                    displacement2 -= Convert.ToInt32(first);
                                    displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                else
                                {
                                    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                    errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                    errorStream[numErrors, ERROR_SOURCE] = line;
                                    numErrors++;
                                }
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
                                    displacement2 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                    displacement2 *= Convert.ToInt32(second);
                                    displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                /* The label is second. */
                                else if (symTable.IsSymbol(second))
                                {
                                    displacement2 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                    displacement2 *= Convert.ToInt32(first);
                                    displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                else
                                {
                                    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                    errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                    errorStream[numErrors, ERROR_SOURCE] = line;
                                    numErrors++;
                                }
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
                                    displacement2 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                    displacement2 /= Convert.ToInt32(second);
                                    displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                /* The label is second. */
                                else if (symTable.IsSymbol(second))
                                {
                                    displacement2 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                    displacement2 /= Convert.ToInt32(first);
                                    displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                }

                                else
                                {
                                    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                    errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                    errorStream[numErrors, ERROR_SOURCE] = line;
                                    numErrors++;
                                }
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
                                    if (openParanIndex == commaIndex + 1)
                                    {
                                        displacement2 = Convert.ToInt32(intermediateParam2.Substring(0, openParanIndex));
                                        base2 = Convert.ToInt32(intermediateParam2.Substring(openParanIndex + 1, closeParanIndex - commaIndex - 1));
                                        index1 = 0;
                                    }

                                    /* Format is D(X,B). */
                                    else
                                    {
                                        displacement2 = Convert.ToInt32(intermediateParam2.Substring(0, openParanIndex));
                                        index1 = Convert.ToInt32(intermediateParam2.Substring(openParanIndex + 1, commaIndex - openParanIndex - 1));
                                        base2 = Convert.ToInt32(intermediateParam2.Substring(commaIndex + 1, closeParanIndex - commaIndex - 1));
                                    }
                                }

                                /* Format is D(X). */
                                else
                                {
                                    displacement2 = Convert.ToInt32(intermediateParam2.Substring(0, openParanIndex));
                                    index1 = Convert.ToInt32(intermediateParam2.Substring(openParanIndex + 1, closeParanIndex - openParanIndex - 1));
                                    base2 = 0;
                                }
                            }

                            /* Format is D. */
                            else
                            {
                                displacement2 = Convert.ToInt32(intermediateParam2);
                                index1 = 0;
                                base2 = 0;
                            }
                        }

                        if (register1 < 0)
                            objCode1 = "F" + index1.ToString("X");
                        else
                            objCode1 = register1.ToString("X") + index1.ToString("X");

                        objCode2 = base2.ToString("X") + displacement2.ToString("X").PadLeft(3, '0');
                        objCode3 = "";
                        break;

                    case "RS":
                        register1 = Convert.ToInt32(intermediateParam1);
                        register3 = 0;

                        if (!symTable.IsSymbol(intermediateParam3) && !intermediateParam2.Contains("+") &&
                            !symTable.IsSymbol(intermediateParam2))
                        {
                            register3 = Convert.ToInt32(intermediateParam2);
                            displacement2 = Convert.ToInt32(intermediateParam3.Substring(0, intermediateParam3.IndexOf('(')));
                            base2 = Convert.ToInt32(intermediateParam3.Substring(intermediateParam3.IndexOf('(') + 1, intermediateParam3.IndexOf(')') - intermediateParam3.IndexOf('(') - 1));
                        }

                        else
                        {
                            if (!symTable.IsSymbol(intermediateParam2))
                                register3 = Convert.ToInt32(intermediateParam2);

                            base2 = Convert.ToInt32(baseRegister);

                            string symbolLocation = symTable.GetAddress(intermediateParam3);

                            if (symTable.IsSymbol(baseRegisterContents))
                                baseRegisterContents = symTable.GetAddress(baseRegisterContents);
                            displacement2 = Convert.ToInt32(symbolLocation, 16) - Convert.ToInt32(baseRegisterContents, 16);
                        }

                        objCode1 = register1.ToString("X") + register3.ToString("X");
                        objCode2 = base2.ToString("X") + displacement2.ToString("X").PadLeft(3, '0');
                        break;

                    case "SS":
                        /* D1(L1,B1),D2(L2,B2) */
                        base1 = 0;
                        base2 = 0;
                        displacement1 = 0;
                        displacement2 = 0;
                        length1 = 0;
                        length2 = 0;

                        /* The first parameter is a label and a length. */
                        if (symTable.IsSymbol(intermediateParam1.Substring(0, intermediateParam1.IndexOf('('))))
                        {
                            string symbolLocation = symTable.GetAddress(intermediateParam1.Substring(0, intermediateParam1.IndexOf('(')));
                            int openParen = intermediateParam1.IndexOf('(');
                            int closeParen = intermediateParam1.IndexOf(')');

                            if (symTable.IsSymbol(baseRegisterContents))
                                baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                            displacement1 = Convert.ToInt32(symbolLocation, 16) - Convert.ToInt32(baseRegisterContents, 16);
                            length1 = Convert.ToInt32(intermediateParam1.Substring(openParen + 1, closeParen - openParen - 1)) - 1;
                            base1 = Convert.ToInt32(baseRegister);
                        }

                       
                        /* The first parameter is a label (with extra increment). */
                        else if (intermediateParam1.Contains('+'))
                        {
                            int openParen = intermediateParam1.IndexOf('(');
                            int closeParen = intermediateParam1.IndexOf(')');

                            string tempIntermediateLabel = intermediateParam1.Substring(0, openParen);
                            string tempIntermediateLength = intermediateParam1.Substring(openParen + 1, closeParen - openParen - 1);

                            /* Save the length. */
                            length1 = Convert.ToInt32(tempIntermediateLength) - 1;
                            base1 = Convert.ToInt32(baseRegister);
                            
                            int plusIndex = tempIntermediateLabel.IndexOf('+');
                            string first = tempIntermediateLabel.Substring(0, plusIndex);
                            string second = tempIntermediateLabel.Substring(plusIndex + 1);

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
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* The second parameter is a label (with extra decrement). */
                        else if (intermediateParam1.Contains('-'))
                        {
                            objCode1 += "0";

                            string tempIntermediate = intermediateParam1.Substring(0, intermediateParam1.IndexOf('('));

                            int plusIndex = tempIntermediate.IndexOf('-');
                            string first = tempIntermediate.Substring(0, plusIndex);
                            string second = tempIntermediate.Substring(plusIndex + 1);

                            /* The label is first. */
                            if (symTable.IsSymbol(first))
                            {
                                int displacement = Convert.ToInt32(symTable.GetAddress(first), 16);
                                displacement -= Convert.ToInt32(second);
                                displacement -= Convert.ToInt32(baseRegisterContents, 16);
                                objCode2 = Convert.ToInt32(baseRegister).ToString("X") +
                                           displacement.ToString("X").PadLeft(3, '0');
                            }

                            /* The label is second. */
                            else if (symTable.IsSymbol(second))
                            {
                                int displacement = Convert.ToInt32(symTable.GetAddress(second), 16);
                                displacement -= Convert.ToInt32(first);
                                displacement -= Convert.ToInt32(baseRegisterContents, 16);
                                objCode2 = Convert.ToInt32(baseRegister).ToString("X") +
                                           displacement.ToString("X").PadLeft(3, '0');
                            }

                            else
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* The second parameter is a label (with extra multiplication). */
                        else if (intermediateParam1.Contains('*'))
                        {
                            objCode1 += "0";

                            string tempIntermediate = intermediateParam1.Substring(0, intermediateParam1.IndexOf('('));

                            int plusIndex = tempIntermediate.IndexOf('*');
                            string first = tempIntermediate.Substring(0, plusIndex);
                            string second = tempIntermediate.Substring(plusIndex + 1);

                            /* The label is first. */
                            if (symTable.IsSymbol(first))
                            {
                                int displacement = Convert.ToInt32(symTable.GetAddress(first), 16);
                                displacement *= Convert.ToInt32(second);
                                displacement -= Convert.ToInt32(baseRegisterContents, 16);
                                objCode2 = Convert.ToInt32(baseRegister).ToString("X") +
                                           displacement.ToString("X").PadLeft(3, '0');
                            }

                            /* The label is second. */
                            else if (symTable.IsSymbol(second))
                            {
                                int displacement = Convert.ToInt32(symTable.GetAddress(second), 16);
                                displacement *= Convert.ToInt32(first);
                                displacement -= Convert.ToInt32(baseRegisterContents, 16);
                                objCode2 = Convert.ToInt32(baseRegister).ToString("X") +
                                           displacement.ToString("X").PadLeft(3, '0');
                            }

                            else
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* The second parameter is a label (with extra division). */
                        else if (intermediateParam1.Contains('/'))
                        {
                            objCode1 += "0";

                            string tempIntermediate = intermediateParam1.Substring(0, intermediateParam1.IndexOf('('));

                            int plusIndex = tempIntermediate.IndexOf('/');
                            string first = tempIntermediate.Substring(0, plusIndex);
                            string second = tempIntermediate.Substring(plusIndex + 1);

                            /* The label is first. */
                            if (symTable.IsSymbol(first))
                            {
                                int displacement = Convert.ToInt32(symTable.GetAddress(first), 16);
                                displacement /= Convert.ToInt32(second);
                                displacement -= Convert.ToInt32(baseRegisterContents, 16);
                                objCode2 = Convert.ToInt32(baseRegister).ToString("X") +
                                           displacement.ToString("X").PadLeft(3, '0');
                            }

                            /* The label is second. */
                            else if (symTable.IsSymbol(second))
                            {
                                int displacement = Convert.ToInt32(symTable.GetAddress(second), 16);
                                displacement /= Convert.ToInt32(first);
                                displacement -= Convert.ToInt32(baseRegisterContents, 16);
                                objCode2 = Convert.ToInt32(baseRegister).ToString("X") +
                                           displacement.ToString("X").PadLeft(3, '0');
                            }

                            else
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /*-----------------------------------------------------------------------*/

                        /* Check the format of the second operand. */
                        if (!intermediateParam2.StartsWith("=") && intermediateParam2.Contains("(") &&
                            symTable.IsSymbol(intermediateParam2.Substring(0, intermediateParam2.IndexOf('('))))
                        {
                            string symbolLocation = symTable.GetAddress(intermediateParam2.Substring(0, intermediateParam2.IndexOf('(')));
                            int openParen = intermediateParam2.IndexOf('(');
                            int closeParen = intermediateParam2.IndexOf(')');

                            if (symTable.IsSymbol(baseRegisterContents))
                                baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                            displacement2 = Convert.ToInt32(symbolLocation, 16) - Convert.ToInt32(baseRegisterContents, 16);
                            length2 = Convert.ToInt32(intermediateParam2.Substring(openParen + 1, closeParen - openParen - 1)) - 1;
                            base2 = Convert.ToInt32(baseRegister);
                        }

                        else if (litTable.IsLiteral(intermediateParam2))
                        {
                            string literalLocation = litTable.GetAddress(intermediateParam2);
                            int openParen = intermediateParam2.IndexOf('(');
                            int closeParen = intermediateParam2.IndexOf(')');

                            if (symTable.IsSymbol(baseRegisterContents))
                                baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                            displacement2 = Convert.ToInt32(literalLocation, 16) - Convert.ToInt32(baseRegisterContents, 16);
                            base2 = Convert.ToInt32(baseRegister);
                        }

                        else if (symTable.IsSymbol(intermediateParam2))
                        {
                            string symbolLocation = symTable.GetAddress(intermediateParam2);
                           
                            if (symTable.IsSymbol(baseRegisterContents))
                                baseRegisterContents = symTable.GetAddress(baseRegisterContents);

                            displacement2 = Convert.ToInt32(symbolLocation, 16) - Convert.ToInt32(baseRegisterContents, 16);
                            length2 = -1;
                            base2 = Convert.ToInt32(baseRegister);
                        }

                        /* The second parameter is a label (with extra increment). */
                        else if (intermediateParam2.Contains('+'))
                        {
                            objCode1 += "0";

                            int openParen = intermediateParam2.IndexOf('(');
                            int closeParen = intermediateParam2.IndexOf(')');

                            string tempIntermediate = intermediateParam2.Substring(0, openParen);
                            length2 = Convert.ToInt32(intermediateParam2.Substring(openParen + 1, closeParen - openParen - 1)) - 1;

                            int plusIndex = tempIntermediate.IndexOf('+');
                            string first = tempIntermediate.Substring(0, plusIndex);
                            string second = tempIntermediate.Substring(plusIndex + 1);

                            /* The label is first. */
                            if (symTable.IsSymbol(first))
                            {
                                displacement2 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                displacement2 += Convert.ToInt32(second);
                                displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                base2 = Convert.ToInt32(baseRegister);
                            }

                            /* The label is second. */
                            else if (symTable.IsSymbol(second))
                            {
                                objCode1 += "0";

                                displacement2 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                displacement2 += Convert.ToInt32(first);
                                displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                base2 = Convert.ToInt32(baseRegister);
                            }

                            else
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* The second parameter is a label (with extra decrement). */
                        else if (intermediateParam2.Contains('-'))
                        {
                            objCode1 += "0";

                            int openParen = intermediateParam2.IndexOf('(');
                            int closeParen = intermediateParam2.IndexOf(')');

                            string tempIntermediate = intermediateParam2.Substring(0, openParen);
                            length2 = Convert.ToInt32(intermediateParam2.Substring(openParen + 1, closeParen - openParen - 1)) - 1;

                            int minusIndex = tempIntermediate.IndexOf('-');
                            string first = tempIntermediate.Substring(0, minusIndex);
                            string second = tempIntermediate.Substring(minusIndex + 1);

                            /* The label is first. */
                            if (symTable.IsSymbol(first))
                            {
                                displacement2 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                displacement2 -= Convert.ToInt32(second);
                                displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                base2 = Convert.ToInt32(baseRegister);
                            }

                            /* The label is second. */
                            else if (symTable.IsSymbol(second))
                            {
                                displacement2 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                displacement2 -= Convert.ToInt32(first);
                                displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                base2 = Convert.ToInt32(baseRegister);
                            }

                            else
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* The second parameter is a label (with extra multiplication). */
                        else if (intermediateParam2.Contains('*'))
                        {
                            objCode1 += "0";

                            int openParen = intermediateParam2.IndexOf('(');
                            int closeParen = intermediateParam2.IndexOf(')');

                            string tempIntermediate = intermediateParam2.Substring(0, openParen);
                            length2 = Convert.ToInt32(intermediateParam2.Substring(openParen + 1, closeParen - openParen - 1)) - 1;

                            int multiplyIndex = tempIntermediate.IndexOf('*');
                            string first = tempIntermediate.Substring(0, multiplyIndex);
                            string second = tempIntermediate.Substring(multiplyIndex + 1);

                            /* The label is first. */
                            if (symTable.IsSymbol(first))
                            {
                                displacement2 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                displacement2 *= Convert.ToInt32(second);
                                displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                base2 = Convert.ToInt32(baseRegister);
                            }

                            /* The label is second. */
                            else if (symTable.IsSymbol(second))
                            {
                                displacement2 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                displacement2 *= Convert.ToInt32(first);
                                displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                base2 = Convert.ToInt32(baseRegister);
                            }

                            else
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* The second parameter is a label (with extra division). */
                        else if (intermediateParam2.Contains('/'))
                        {
                            objCode1 += "0";

                            int openParen = intermediateParam2.IndexOf('(');
                            int closeParen = intermediateParam2.IndexOf(')');

                            string tempIntermediate = intermediateParam2.Substring(0, openParen);
                            length2 = Convert.ToInt32(intermediateParam2.Substring(openParen + 1, closeParen - openParen - 1)) - 1;
                            
                            int divideIndex = tempIntermediate.IndexOf('/');
                            string first = tempIntermediate.Substring(0, divideIndex);
                            string second = tempIntermediate.Substring(divideIndex + 1);

                            /* The label is first. */
                            if (symTable.IsSymbol(first))
                            {
                                displacement2 = Convert.ToInt32(symTable.GetAddress(first), 16);
                                displacement2 /= Convert.ToInt32(second);
                                displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                base2 = Convert.ToInt32(baseRegister);
                            }

                            /* The label is second. */
                            else if (symTable.IsSymbol(second))
                            {
                                displacement2 = Convert.ToInt32(symTable.GetAddress(second), 16);
                                displacement2 /= Convert.ToInt32(first);
                                displacement2 -= Convert.ToInt32(baseRegisterContents, 16);
                                base2 = Convert.ToInt32(baseRegister);
                            }

                            else
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }



                        }

                        /* The second parameter is in D(B) format. */
                        else if (intermediateParam2.Contains("(") &&
                                     !symTable.IsSymbol(intermediateParam2.Substring(0, intermediateParam2.IndexOf('('))))
                        {
                            int openParen = intermediateParam2.IndexOf('(');
                            int closeParen = intermediateParam2.IndexOf(')');

                            displacement2 = Convert.ToInt32(intermediateParam2.Substring(0, openParen));
                            base2 = Convert.ToInt32(intermediateParam2.Substring(openParen + 1, closeParen - openParen - 1));

                            int tempLength = length1;
                            length1 = length2;
                            length2 = tempLength;
                        }

                        /* 
                         * If the instruction is either ED, EDMK, CLC, or MVC there's only one 
                         * length, only use length1. 
                         * Else use both. 
                         */
                        if (intermediateInstruction == "DE" || intermediateInstruction == "DF" ||
                            intermediateInstruction == "D2" || intermediateInstruction == "D5")
                            objCode1 = length1.ToString("X").PadLeft(2,'0');
                        else
                            objCode1 = (length1.ToString("X") + length2.ToString("X")).PadLeft(2,'0');
                        
                        objCode2 = base1.ToString("X") + displacement1.ToString("X").PadLeft(3,'0');
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
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
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
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
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
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
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
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* Format is D(B). */
                        else if (intermediateParam1.Contains('('))
                        {
                            int openParanIndex = intermediateParam1.IndexOf('(');
                            int closeParanIndex = intermediateParam1.IndexOf(')');

                            displacement1 = Convert.ToInt32(intermediateParam1.Substring(0, openParanIndex));
                            base1 = Convert.ToInt32(intermediateParam1.Substring(openParanIndex + 1, closeParanIndex - openParanIndex - 1));

                        }

                        /* Format is D. */
                        else
                        {
                            displacement1 = Convert.ToInt32(intermediateParam1);
                            base1 = 0;
                        }

                        if (intermediateParam2.StartsWith("C"))
                            immediate = ToEBCDIC(intermediateParam2[2]);
                        else if (intermediateParam2.StartsWith("X"))
                            immediate = intermediateParam2.Substring(2, 2);
                        
                        objCode1 = Convert.ToInt32(immediate, 16).ToString("X").PadLeft(2, '0');
                        objCode2 = base1.ToString("X") + displacement1.ToString("X").PadLeft(3, '0');
                        objCode3 = "";
                        break;

                    case "X":

                        index1 = 0;
                        base1 = 0;
                        displacement1 = 0;

                        /* The first parameter is a label (with no extra increment). */
                        if (litTable.IsLiteral(intermediateParam1))
                        {
                            base1 = Convert.ToInt32(baseRegister);

                            displacement1 = Convert.ToInt32(litTable.GetAddress(intermediateParam1), 16);
                            displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                        }

                        /* The first parameter is a label (with no extra increment). */
                        else if (symTable.IsSymbol(intermediateParam1))
                        {
                            base1 = Convert.ToInt32(baseRegister);

                            displacement1 = Convert.ToInt32(symTable.GetAddress(intermediateParam1), 16);
                            displacement1 -= Convert.ToInt32(baseRegisterContents, 16);
                        }

                        /* The second parameter is a label (with extra increment). */
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
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* The second parameter is a label (with extra decrement). */
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
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* The second parameter is a label (with extra multiplication). */
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
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }

                        /* The second parameter is a label (with extra division). */
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
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
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
                                if (openParanIndex == commaIndex + 1)
                                {
                                    displacement1 = Convert.ToInt32(intermediateParam1.Substring(0, openParanIndex));
                                    base1 = Convert.ToInt32(intermediateParam1.Substring(openParanIndex + 1, closeParanIndex - commaIndex - 1));
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
                        else
                        {
                            displacement1 = Convert.ToInt32(intermediateParam1);
                            index1 = 0;
                            base1 = 0;
                        }

                        objCode1 = index1.ToString("X");
                        objCode2 = base1.ToString("X") + displacement1.ToString("X").PadLeft(3, '0');
                        objCode3 = Convert.ToInt32(intermediateParam2).ToString("X").PadLeft(4, '0');
                        break;

                    default:
                        break;
                }
            }

            else if (assemblerDirectives.Contains(intermediateInstruction))
            {
                //string[] usingParams = parameters.Split(',');
                if (intermediateInstruction == "DC")
                {
                    /* The constant is a character string. */
                    if (intermediateParam1.StartsWith("C'"))
                    {
                        string tempCharString = intermediateParam1.Substring(intermediateParam1.IndexOf('\'') + 1, intermediateParam1.Length - 3);

                        for (int i = 0; i < tempCharString.Length; i++)
                        {
                            objCode1 += ToEBCDIC(tempCharString[i]);
                        }
                    }

                    /* The constant is in hex representation. */
                    else if (intermediateParam1.StartsWith("X'"))
                    {
                        string tempCharString = intermediateParam1.Substring(intermediateParam1.IndexOf('\'') + 1, intermediateParam1.Length - 3);

                        for (int i = 0; i < tempCharString.Length; i++)
                        {
                            objCode1 += tempCharString[i];
                        }
                    }
                    objCode1 = objCode1.PadRight(16, ' ');
                    
                }

                /* Instruction is DS. */
                else if (intermediateInstruction == "DS")
                {
                    int storageSize = 0;

                    /* The storage is the Character Length type. */
                    if (intermediateParam1.StartsWith("CL"))
                    {
                        index = intermediateParam1.IndexOf("CL") + 2;
                        storageSize = Convert.ToInt32(intermediateParam1.Substring(index));

                        for (int i = 0; i < storageSize; i++)
                            objCode1 += "F5";

                        objCode2 = "";
                        objCode3 = "";
                    }

                    /* The storage is the Packed Length type. */
                    else if (intermediateParam1.StartsWith("PL"))
                    {
                        index = intermediateParam1.IndexOf("PL") + 2;
                        storageSize = Convert.ToInt32(intermediateParam1.Substring(index));

                        for (int i = 0; i < storageSize; i++)
                            objCode1 += "F5";

                        objCode2 = "";
                        objCode3 = "";
                    }

                    /* The storage is the Hex Length type. */
                    else if (intermediateParam1.StartsWith("XL"))
                    {
                        index = intermediateParam1.IndexOf("XL") + 2;
                        storageSize = Convert.ToInt32(intermediateParam1.Substring(index));

                        for (int i = 0; i < storageSize; i++)
                            objCode1 += "F5";

                        objCode2 = "";
                        objCode3 = "";
                    }

                    /* The storage is a halfword. */
                    else if (intermediateParam1.EndsWith("H"))
                    {
                        storageSize = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.Length - 1));
                        storageSize *= 2;

                        for (int i = 0; i < storageSize; i++)
                            objCode1 += "F5";

                        objCode2 = "";
                        objCode3 = "";
                    }

                    /* The storage is a fullword. */
                    else if (intermediateParam1.EndsWith("F"))
                    {
                        storageSize = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.Length - 1));
                        storageSize *= 4;

                        for (int i = 0; i < storageSize; i++)
                            objCode1 += "F5";

                        objCode2 = "";
                        objCode3 = "";
                    }

                    /* The storage is a doubleword. */
                    else if (intermediateParam1.EndsWith("D"))
                    {
                        storageSize = Convert.ToInt32(intermediateParam1.Substring(0, intermediateParam1.Length - 1));
                        storageSize *= 8;

                        for (int i = 0; i < storageSize; i++)
                            objCode1 += "F5";

                        objCode2 = "";
                        objCode3 = "";
                    }
                }

                intermediateInstruction = "";

            }

            else
            {
                /* Throw error? */
                intermediateInstruction = "";

            }


            /* Add the object code for the instruction to the end of the object code string. */
            objCode += (intermediateInstruction + objCode1 + objCode2 + objCode3).PadRight(12, ' ')
                        + "|";
        }

        /******************************************************************************************
         * 
         * Name:        ProcessLineSourceCode     
         * 
         * Author(s):   Travis Hunt
         *              Andrew Hamilton
         *              
         * Input:       Line from source code as string, streamwriter objects for prt and
         *              intermediate files.      
         * Return:      N/A 
         * Description: This method processes the source code line that is passed to it. The method
         *              pulls out the label, operation and parameters.
         *              
         *****************************************************************************************/
        private void ProcessLineSourceCode(string inputLine)
        {
            /* Reset variables. */
            line = inputLine;
            label = "";
            instruction = "";
            objCode1 = "";
            objCode2 = "";
            parameters = "";
            previousLocation = 0;

            /* Open the writing stream to the intermediate file. */
            StreamWriter intermediateOutStream = new StreamWriter(intermediateFile, true);

            /* 
             * If the first column contains a *, the row is commented out so ignore line. 
             * If #, the row is an option line so ignore.
             */
            if (line[0] != '*' && line[0] != '#')
            {
                /* Makes sure the number of lines has not exceeded the limit. */
                if (numLines >= optionsLines)
                {
                    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                    errorStream[numErrors, ERROR_DETAIL] = "Number of lines has " +
                                                           "exceeded the limit.";
                    errorStream[numErrors, ERROR_SOURCE] = line;
                    numErrors++;
                    return;
                }

                /* Store the label portion of the line and validate. */
                //if (line.Length >= 8)
                label = line.Substring(0, 8).TrimEnd();
                //else
                //    label = line.TrimEnd();

                /* Check for the $ENTRY indicator. */
                //if (label.StartsWith("$ENTRY"))
                //{
                    
                //    //entryCardFound = true;
                //    //if (line.TrimEnd() == "$ENTRY")
                //    //{
                //    //    inputFile = "(local)\\tempInputFile.txt";
                //    //}

                //    //else
                //    //{
                //    //    int firstSpaceIndex = line.IndexOf(' ');
                //    //    int secondSpaceIndex = line.IndexOf(' ', firstSpaceIndex + 1);

                //    //    inputFile = line.Substring(firstSpaceIndex + 1, secondSpaceIndex - firstSpaceIndex - 1);
                //    //}

                //}

                //else
                //{
                ValidateLabel();

                /* If column 9 is not a space, save the error. */
                if (line[8] != ' ')
                {
                    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                    errorStream[numErrors, ERROR_DETAIL] = "Invalid label format, column 9 must" +
                                                            " be blank";
                    errorStream[numErrors, ERROR_SOURCE] = line;
                    numErrors++;
                }

                /* Store the instruction portion of the line. */
                instruction = line.Substring(9, 5).TrimEnd();

                /* If the instruction field is not one of the the directives, handle normally. */
                if (!assemblerDirectives.Contains(instruction))
                {
                    numInstructions++;

                    /* Makes sure the number of instructions has not exceeded the limit. */
                    if (numInstructions >= optionsInstructions)
                    {
                        errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                        errorStream[numErrors, ERROR_DETAIL] = "Number of instructions has " +
                                                                "exceeded the limit.";
                        errorStream[numErrors, ERROR_SOURCE] = line;
                        numErrors++;
                        return;
                    }

                    previousLocation = locationCounter;

                    ValidateInstruction();

                    /* If column 15 is not a space, send the error. */
                    if (line[14] != ' ')
                    {
                        errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                        errorStream[numErrors, ERROR_DETAIL] = "Invalid instruction format, " +
                                                                "column 15 " + "must be blank";
                        errorStream[numErrors, ERROR_SOURCE] = line;
                        numErrors++;
                    }

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
                        {
                            errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                            errorStream[numErrors, ERROR_DETAIL] = "Error: syntax error.";
                            errorStream[numErrors, ERROR_SOURCE] = line;
                        }
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
                        {
                            errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                            errorStream[numErrors, ERROR_DETAIL] = "Error: syntax error.";
                            errorStream[numErrors, ERROR_SOURCE] = line;
                        }
                    }

                    /* The parameters have no extra spaces to worry about. */
                    else
                        parameters = line.Substring(15, parameterLastIndex);

                    ValidateParameters();
                }

                /* The field is a Declared Storage field. */
                else if (instruction == "DS")
                {
                    previousLocation = locationCounter;
                    int index = 0;
                    int increment = 0;

                    if (line.IndexOf(' ', 15) < 0)
                        parameters = line.Substring(15);
                    else
                        parameters = line.Substring(15, line.IndexOf(' ', 15) - 14).TrimEnd();

                    /* The storage is the Character Length type. */
                    if (parameters.StartsWith("CL"))
                    {
                        index = parameters.IndexOf("CL") + 2;
                        increment = Convert.ToInt32(parameters.Substring(index));
                        locationCounter += increment;
                    }

                    /* The storage is the Packed Length type. */
                    else if (parameters.StartsWith("PL"))
                    {
                        index = parameters.IndexOf("PL") + 2;
                        increment = Convert.ToInt32(parameters.Substring(index));
                        locationCounter += increment;
                    }

                    /* The storage is the Hex Length type. */
                    else if (parameters.StartsWith("XL"))
                    {
                        index = parameters.IndexOf("XL") + 2;
                        increment = Convert.ToInt32(parameters.Substring(index));
                        locationCounter += increment;
                    }

                    /* The storage is a halfword. */
                    else if (parameters.EndsWith("H"))
                    {
                        increment = Convert.ToInt32(parameters.Substring(0, parameters.Length - 1));
                        increment *= 2;

                        locationCounter += increment;

                        while (previousLocation % 2 != 0)
                            previousLocation++;

                        if (symTable.IsSymbol(label))
                            symTable.UpdateAddress(label, previousLocation.ToString("X").PadLeft(6, '0'));

                    }

                    /* The storage is a fullword. */
                    else if (parameters.EndsWith("F"))
                    {
                        while (previousLocation % 4 != 0)
                            previousLocation++;

                        if (symTable.IsSymbol(label))
                            symTable.UpdateAddress(label, previousLocation.ToString("X").PadLeft(6, '0'));

                        increment = Convert.ToInt32(parameters.Substring(0, parameters.Length - 1));
                        increment *= 4;

                        locationCounter += increment;

                        while (locationCounter % 4 != 0)
                            locationCounter++;
                    }

                    /* The storage is a doubleword. */
                    else if (parameters.EndsWith("D"))
                    {
                        increment = Convert.ToInt32(parameters.Substring(0, parameters.Length - 1));
                        increment *= 8;

                        locationCounter += increment;

                        while (previousLocation % 8 != 0)
                            previousLocation++;

                        if (symTable.IsSymbol(label))
                            symTable.UpdateAddress(label, previousLocation.ToString("X").PadLeft(6, '0'));

                    }
                    /* Need to throw an else and error here. */

                }

                /* Field is a Declared Constant field. */
                else if (instruction == "DC")
                {
                    previousLocation = locationCounter;
                    int index = 0;
                    int increment = 0;

                    index = line.IndexOf('\'', 15) + 1;
                    parameters = line.Substring(15, line.IndexOf('\'', index) - 14);

                    if (parameters.StartsWith("C'"))
                    {
                        for (index = 2; parameters[index] != '\''; index++)
                            increment++;
                    }

                    else if (parameters.StartsWith("X'"))
                    {
                        for (index = 2; parameters[index] != '\''; index++)
                            increment++;
                        increment /= 2;
                    }

                    locationCounter += increment;

                }

                /* Field is a Using field. */
                else if (instruction == "USING")
                {
                    previousLocation = locationCounter;

                    if (line.IndexOf(' ', 15) < 0)
                        parameters = line.Substring(15);
                    else
                        parameters = line.Substring(15, line.IndexOf(' ', 15) - 14).TrimEnd();
                    string[] usingParams = parameters.Split(',');
                    baseRegisterContents = usingParams[0];
                    baseRegister = usingParams[1];
                }

                else if (instruction == "END")
                {
                    previousLocation = locationCounter;
                    endCardFound = true;
                }

                string locationCounterHex = previousLocation.ToString("X").PadLeft(6, '0');

                /* 
                    * Write the items to the intermediate file. 
                    * The instruction and parameters are saved.
                    * They are seperated by the piping symbol "|".
                    * Assembler formatting directives are ignored.
                    */
                if (instruction != "TITLE" && instruction != "SPACE" && instruction != "EJECT")
                {
                    parameters = parameters.TrimEnd();
                    string[] parameterList = new string[3];

                    if (instruction == "START" || instruction == "END" || instruction == "CSECT")
                        parameterList = null;

                    /* The parameters are not character literals or D(X,B). */
                    else if (!parameters.Contains("=C") &&
                                (parameters.LastIndexOf('(') < 0 || (parameters.LastIndexOf(',') < parameters.LastIndexOf('('))))
                        parameterList = parameters.Split(',');

                    /* The first parameter is a string of characters. */
                    else if (parameters.Contains("=C") && parameters.IndexOf("=C") < parameters.IndexOf(','))
                    {
                        parameterList[0] = parameters.Substring(0, parameters.IndexOf('\'', 3) + 1).TrimEnd();
                        parameterList[1] = parameters.Substring(parameters.LastIndexOf(',') + 1).TrimEnd();

                        string[] tempArray = new string[parameterList.Length - 1];
                        Array.Copy(parameterList, tempArray, parameterList.Length - 1);

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
                        parameterList[parameterList.Length - 2] += "," + parameterList[parameterList.Length - 1];

                        string[] tempArray = new string[parameterList.Length - 1];
                        Array.Copy(parameterList, tempArray, parameterList.Length - 1);

                        parameterList = tempArray;
                    }

                    /* 
                        * Choose how many items to print to intermediate file based on number of 
                        * parameters. 
                        */
                    if (parameterList == null)
                        intermediateOutStream.WriteLine("{0}|{1}|{2}|{3}", (lineNumber + 1), locationCounterHex, label, instruction.TrimEnd());
                    else if (parameterList.Length == 1)
                        intermediateOutStream.WriteLine("{0}|{1}|{2}|{3}|{4}", (lineNumber + 1), locationCounterHex, label, instruction.TrimEnd(),
                                                        parameterList[0].TrimEnd());
                    else if (parameterList.Length == 2)
                        intermediateOutStream.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}", (lineNumber + 1), locationCounterHex, label, instruction.TrimEnd(),
                                                        parameterList[0].TrimEnd(), parameterList[1].TrimEnd());
                    else if (parameterList.Length == 3)
                        intermediateOutStream.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}", (lineNumber + 1), locationCounterHex, label, instruction.TrimEnd(),
                                                        parameterList[0].TrimEnd(), parameterList[1].TrimEnd(),
                                                        parameterList[2].TrimEnd());

                }
                //}
                

                


                //line = String.Format("{0,11} {1}", (lineNumber+1), line);

                //locationCounter += 4;
            }

            /* Increment the current line number, close the writing stream to intermediate file. */
            lineNumber++;
            intermediateOutStream.Close();
            programLength = locationCounter.ToString("X").PadLeft(6, '0');
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
         * Input:       N/A    
         * Return:      N/A   
         * Description: This methods updates the addresses of the literals to the correct addresses
         *              at the end of the program.
         *              
         *****************************************************************************************/
        private void UpdateLiteralAddresses()
        {
            /* Save the literals and addresses (order) as parallel arrays. */
            string[] literals = litTable.GetLiteralsList();
            string[] addresses = litTable.GetAddressesList();
            
            int[] intAddresses = new int[addresses.Length];

            /* Copy the addresses (order) into an integer array for sorting. */
            for (int i = 0; i < addresses.Length; i++)
                intAddresses[i] = Convert.ToInt32(addresses[i]);
            
            /* Sort the literals based on the order they were declared. */
			Array.Sort(intAddresses, literals);

            /* Delete the table to empty it. */
            litTable.ClearTable();

            /* Find the address of the location counter. */
            int currentAddress = Convert.ToInt32(programLength, 16);

            foreach (string literal in literals)
            {
                string type = literal.Substring(0,2);
                
                /* Store the literal and address, then determine how much to increment. */
                litTable.Hash(literal, currentAddress.ToString("X").PadLeft(6, '0'));
                switch (type)
                {
                    case "=C":
                        currentAddress += literal.Length - 4;
                        break;

                    case "=P":
                        currentAddress += literal.Length - 4;
                        break;
                    
                    case "=X":
                        currentAddress += ((literal.Length - 4) / 2);
                        break;

                    default:
                        break;
                }
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
         *              Used for Pass 1             
         *              
         *****************************************************************************************/
        private bool ValidateInstruction()
        {
            /* True if the instruction is a valid operation. */
            if (MachineOpTableTest.IsOpcode(instruction) >= 0)
            {
                int index = MachineOpTableTest.IsOpcode(instruction);
                string opType = MachineOpTableTest.GetOpType(index);

                while (previousLocation % 2 != 0)
                    previousLocation++;
                locationCounter = previousLocation;

                /* Decide how much to increment the location counter from the instruction format. */
                switch (opType)
                {
                    case "RR":
                        //while (previousLocation % 2 != 0)
                        //    previousLocation++;
                        locationCounter += 2;
                        break;

                    case "RX":
                    case "RS":
                    case "SI":
                        //while (previousLocation % 4 != 0)
                        //    previousLocation++;
                        locationCounter += 4;
                        break;

                    case "X":
                    case "SS":
                        //while (previousLocation % 8 != 0)
                        //    previousLocation++;
                        locationCounter += 6;
                        break;
                }
                
                return true;
            }

            /* Instruction is not valid. */
            else
            {
                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                errorStream[numErrors, ERROR_DETAIL] = "Instruction is not valid.";
                errorStream[numErrors, ERROR_SOURCE] = line;
                numErrors++;
            }

            return false;
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
         *              Used for Pass 1
         *              
         *****************************************************************************************/
        private bool ValidateLabel()
        {
            /* 
             * Find the first space in the label, then make sure the rest of the string is spaces 
             * as well. 
             */
            int firstSpaceIndex = label.IndexOf(' ');
            //if (firstSpaceIndex >= 0)
            //{
            //    for (int i = firstSpaceIndex; i < label.Length; i++)
            //    {
            //if (label.Contains)
            //{
            //    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
            //    errorStream[numErrors, ERROR_DETAIL] = "Label is invalid, cannot contain" +
            //                                           " spaces inbetween characters";
            //    errorStream[numErrors, ERROR_SOURCE] = line;
            //    numErrors++;
            //    return false;
            //}
            //    }
            //}

            /* Search the label for invalid characters and store the error if one is found. */
            //for (int i = 0; i < firstSpaceIndex; i++)
            //{
            foreach (char character in label)
            {
                if (!validChars.Contains(character))
                {
                    errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                    errorStream[numErrors, ERROR_DETAIL] = "Error: Label is invalid, contains " +
                                                           " invalid character(s)";
                    errorStream[numErrors, ERROR_SOURCE] = line;
                    numErrors++;
                    return false;
                }
            }

            //}

            /* If there is a label and it is not already in the symbol table, it's added. */
            if (label != "" && !label.StartsWith(" "))
            {
                if (!symTable.IsSymbolFull())
                {
                    if (!symTable.IsSymbol(label))
                        symTable.Hash(label, locationCounter.ToString("X").PadLeft(6, '0'));
                    else
                    {
                        errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                        errorStream[numErrors, ERROR_DETAIL] = "Error: Label is invalid, already exists ";
                        errorStream[numErrors, ERROR_SOURCE] = line;
                        numErrors++;
                        return false;
                    }
                }
                else
                {
                    /* Need to set error for too many symbols. */
                    Console.Beep();
                }
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
            string[] parameterList;

            //if(!parameters.Contains('('))
            parameterList = parameters.Split(',');

            int openParen = parameters.LastIndexOf('(');
            int comma = parameters.LastIndexOf(',');
            int closeParen = parameters.LastIndexOf(')');

            if (openParen > 0 && openParen < comma && comma < closeParen)
            {
                parameterList[parameterList.Length - 2] += "," + parameterList[parameterList.Length - 1];
                parameterList[parameterList.Length - 1] = "";
            }

            foreach (string param in parameterList)
            {
                /* True if the parameter is a literal. */
                if (param.StartsWith("="))
                {
                    
                    if (!litTable.IsLiteralFull())
                    {
                        if (!litTable.IsLiteral(param.TrimEnd()))
                        {
                            numLiterals++;
                            litTable.Hash(param.TrimEnd(), numLiterals.ToString());
                        }
                            
                    }

                    else
                    {
                        /* Need to set an error for literal table overflow. */
                        Console.Beep();
                    }

                    /*
                    switch (param[1])
                    {
                        case '\'':
                        case 'C':
                        case 'X':
                            if (!litTable.isLiteral(param))
                                litTable.Hash(param, locationCounter.ToString("X").PadLeft(8, '0'));    
                            break;
                            
                        default:
                            break;
                    }
                    */
                }
            }


            return true;
        }
    }
}
