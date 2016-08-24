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
 *  
 *************************************************************************************************/

namespace Assist_UNA
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
        private bool titleFlag;
        private int currentObjectCodeIndex;
        private int lineNumber;
        private int locationCounter;
        private int numErrors;
        private int pageNumber;
        private LiteralTable litTable;
        private string[] assemblerDirectives;
        private string baseRegister;
        private string baseRegisterContents;
        private string[,] errorStream;
        private string label;
        private string line;
        private string identifier;
        private string instruction;
        private string intermediateFile;
        private string objCode1;
        private string objCode2;
        private string objCode3;
        private string objCode;
        private string objectFile;
        private string parameters;
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
                             SymbolTable symbols, LiteralTable literals)
        {
            /* Initialize all the data members to default values. */
            numErrors = 0;
            lineNumber = 0;
            locationCounter = 0;
            currentObjectCodeIndex = 0;

            /* The size of this will need to figured out later... */
            errorStream = new string[MAX_ERRORS, ERROR_COLUMNS];

            /* Holds the characters that are valid for labels. */
            validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";

            /* Holds the directives the assembler uses, not for execution. */
            assemblerDirectives = new string[] {"START","END","CSPECT","LTORG","USING","SPACE",
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
            identifier = id;
            pageNumber = 1;

            /* Flag for when the TITLE directive is used. */
            titleFlag = false;

            /* Set the file paths. */
            sourceFile = source;
            prtFile = prt;
            intermediateFile = intermediate;
            objectFile = obj;

            /* Set the literal and symbol tables. */
            litTable = literals;
            symTable = symbols;
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
           
            /* Skip the options portion of the project file. */
            string line = inStream.ReadLine();
            while (line == "" || line[0] == '#')
                line = inStream.ReadLine();

            /* Process each line of the source file. */
            while (!inStream.EndOfStream)
            {
                ProcessLineSourceCode(line);
                line = inStream.ReadLine();
            }
                

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

            while (!inStream.EndOfStream)
                GeneratePRT(inStream.ReadLine());

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
            line = inputLine;
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
                                for (int i = 0; i < literals.Length; i++)
                                {

                                    line = String.Format("{0,11} {1,8} {2}", (lineNumber + 1), " ",
                                                         literals[i]);
                                    prtOutStream.WriteLine(String.Format("{0,6} {1,4} {2,4} {3}",
                                                                 addresses[i], " ",
                                                                 " ", line));
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
                                                                 locationCounterHex, objCode1,
                                                                 objCode2, line));
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

                                line = String.Format("{0,1} {1}", (lineNumber + 1), line);
                                
                                string[] objectCode = objCode.Split('|');

                                objCode1 = objectCode[currentObjectCodeIndex].Substring(0,16);

                                prtOutStream.WriteLine(String.Format("{0,6} {1,-18} {2}",
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

                                lineNumber--;
                                break;

                            /* 
                             * The instruction is TITLE.
                             * Creates a title header and displays to the top of another page. 
                             */
                            case "TITLE":
                                int index = line.IndexOf('\'') + 1;
                                int lastIndex = line.LastIndexOf('\'');

                                pageNumber++;

                                prtTitleHeader = line.Substring(index, lastIndex - index);
                                
                                prtOutStream.WriteLine("\f\n{0}\n", prtTitleHeader);
                                prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");
                                
                                titleFlag = true;
                                break;

                            /*
                             * The instruction is EJECT.
                             * Starts writing to the prt on a new page. 
                             */
                            case "EJECT":
                                pageNumber++;
                                prtOutStream.WriteLine("\f");
                                if (titleFlag)
                                    prtOutStream.WriteLine(prtTitleHeader + "\n");
                                prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");
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

                        for (int i = 0; i < numErrors; i++)
                        {
                            int errorLine = Convert.ToInt32(errorStream[i,ERROR_LINE]);
                            if (lineNumber+1 == errorLine)
                                prtOutStream.WriteLine("{0,33} $\n{0,29}Error, {1}", " ", 
                                                       errorStream[i,ERROR_DETAIL]);
                        }
                        
                        /* Call this to increment location counter by correct value. */
                        //ValidateInstruction();

                        currentObjectCodeIndex++;
                    }
                }
                lineNumber++;

                if (lineNumber == MAX_LINES_PER_PAGE)
                {
                    pageNumber++;
                    prtOutStream.WriteLine("\f");
                    if (titleFlag)
                        prtOutStream.WriteLine(prtTitleHeader + "\n");
                    prtOutStream.WriteLine(PRT_HEADER + pageNumber + "\n");
                    
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
                int register;

                intermediateInstruction = MachineOpTableTest.GetObjectCode(index);

                string format = MachineOpTableTest.GetOpType(index);

                /* Generate the correct object code based on the instruction format. */
                switch (format)
                {
                    case "RR":
                        /* Convert the string decimal numbers to integers so they convert to hex. */
                        register = Convert.ToInt32(intermediateParam1);

                        /* Make sure the register specified is valid. */
                        if (register >= 0 || register <= 15)
                        {
                            objCode1 = register.ToString("X");

                            /* Checks against being BR (opcode is 07). */
                            if (intermediateInstruction != "07")
                                objCode1 += Convert.ToInt32(intermediateParam2).ToString("X");

                            /* If the instruction was BR, add the mask (F, or 1111). */
                            else
                                objCode1 = "F" + objCode1;
                        }

                        else
                        {
                            errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                            errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format, " + 
                                                                   "register is invalid.";
                            errorStream[numErrors, ERROR_SOURCE] = line;
                            numErrors++;
                        }

                        break;

                    case "RX":

                        int d = 0;
                        int x = 0;
                        int b = 0;

                        /* The first parameter is a mask. */
                        if (intermediateParam1.StartsWith("B'"))
                            objCode1 = Convert.ToInt32(intermediateParam1.Substring(2, 4), 2).ToString("X");

                        /* The first parameter is a register. */
                        else
                        {
                            /* Convert the string decimal numbers to integers so they convert to hex. */
                            register = Convert.ToInt32(intermediateParam1);

                            /* Make sure the register specified is valid. */
                            if (register >= 0 || register <= 15)
                            {
                                objCode1 = register.ToString("X");
                            }

                            else
                            {
                                errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                                errorStream[numErrors, ERROR_DETAIL] = "Invalid operand format, " +
                                                                       "register is invalid.";
                                errorStream[numErrors, ERROR_SOURCE] = line;
                                numErrors++;
                            }
                        }
                            

                        /* The second parameter is a label (with no extra increment). */
                        if (symTable.IsSymbol(intermediateParam2))
                        {
                            objCode1 += "0";

                            int displacement = Convert.ToInt32(symTable.GetAddress(intermediateParam2), 16);
                            displacement -= Convert.ToInt32(baseRegisterContents, 16);
                            objCode2 = Convert.ToInt32(baseRegister).ToString("X") + displacement.ToString("X").PadLeft(3,'0');
                        }

                        /* The second parameter is a label (with extra increment). */
                        else if (intermediateParam2.Contains('+'))
                        {
                            objCode1 += "0";

                            int plusIndex = intermediateParam2.IndexOf('+');
                            string first = intermediateParam2.Substring(0, plusIndex);
                            string second = intermediateParam2.Substring(plusIndex + 1);

                            /* The label is first. */
                            if (symTable.IsSymbol(first))
                            {
                                int displacement = Convert.ToInt32(symTable.GetAddress(first), 16);
                                displacement += Convert.ToInt32(second);
                                displacement -= Convert.ToInt32(baseRegisterContents, 16);
                                objCode2 = Convert.ToInt32(baseRegister).ToString("X") +
                                           displacement.ToString("X").PadLeft(3, '0');
                            }

                            /* The label is second. */
                            else if (symTable.IsSymbol(second))
                            {
                                objCode1 += "0";

                                int displacement = Convert.ToInt32(symTable.GetAddress(second), 16);
                                displacement += Convert.ToInt32(first);
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

                        /* The second parameter is a label (with extra decrement). */
                        else if (intermediateParam2.Contains('-'))
                        {
                            objCode1 += "0";

                            int plusIndex = intermediateParam2.IndexOf('+');
                            string first = intermediateParam2.Substring(0, plusIndex);
                            string second = intermediateParam2.Substring(plusIndex + 1);

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
                        else if (intermediateParam2.Contains('*'))
                        {
                            objCode1 += "0";

                            int plusIndex = intermediateParam2.IndexOf('+');
                            string first = intermediateParam2.Substring(0, plusIndex);
                            string second = intermediateParam2.Substring(plusIndex + 1);

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
                        else if (intermediateParam2.Contains('/'))
                        {
                            objCode1 += "0";

                            int plusIndex = intermediateParam2.IndexOf('+');
                            string first = intermediateParam2.Substring(0, plusIndex);
                            string second = intermediateParam2.Substring(plusIndex + 1);

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
                                    d = Convert.ToInt32(intermediateParam2.Substring(0, openParanIndex));
                                    b = Convert.ToInt32(intermediateParam2.Substring(openParanIndex + 1, closeParanIndex - commaIndex - 1));
                                }

                                /* Format is D(X,B). */
                                else
                                {
                                    d = Convert.ToInt32(intermediateParam2.Substring(0, openParanIndex));
                                    x = Convert.ToInt32(intermediateParam2.Substring(openParanIndex + 1, commaIndex - openParanIndex - 1));
                                    b = Convert.ToInt32(intermediateParam2.Substring(commaIndex + 1, closeParanIndex - commaIndex - 1));
                                }
                            }

                            /* Format is D(X). */
                            else
                            {
                                d = Convert.ToInt32(intermediateParam2.Substring(0, openParanIndex));
                                x = Convert.ToInt32(intermediateParam2.Substring(openParanIndex + 1, closeParanIndex - openParanIndex - 1));
                            }

                            objCode1 += x.ToString("X");
                            objCode2 = b.ToString("X") + d.ToString("X").PadLeft(3, '0');
                        }

                        /* Format is D. */
                        else
                        {
                            d = Convert.ToInt32(intermediateParam2);
                            objCode1 += x.ToString("X");
                            objCode2 = b.ToString("X") + d.ToString("X").PadLeft(3, '0');
                        }
                                                
                        break;

                    case "RS":
                        int r1 = Convert.ToInt32(intermediateParam1);
                        int r3 = 0;
                        int d2, b2;

                        if (!symTable.IsSymbol(intermediateParam3) && !intermediateParam2.Contains("+") &&
                            !symTable.IsSymbol(intermediateParam2))
                        {
                            r3 = Convert.ToInt32(intermediateParam2);
                            d2 = Convert.ToInt32(intermediateParam3.Substring(0, intermediateParam3.IndexOf('(')));
                            b2 = Convert.ToInt32(intermediateParam3.Substring(intermediateParam3.IndexOf('(') + 1, intermediateParam3.IndexOf(')') - intermediateParam3.IndexOf('(') - 1));
                        }

                        else
                        {
                            b2 = Convert.ToInt32(baseRegister);

                            string symbolLocation = symTable.GetAddress(intermediateParam3);

                            if (symTable.IsSymbol(baseRegisterContents))
                                baseRegisterContents = symTable.GetAddress(baseRegisterContents);
                            d2 = Convert.ToInt32(symbolLocation, 16) - Convert.ToInt32(baseRegisterContents, 16);
                        }

                        objCode1 = r1.ToString("X") + r3.ToString("X");
                        objCode2 = b2.ToString("X") + d2.ToString("X").PadLeft(3,'0');
                        break;

                    case "X":

                        x = 0;
                        b = 0;
                        d = 0;

                        /* The first parameter is a label (with no extra increment). */
                        if (symTable.IsSymbol(intermediateParam1))
                        {
                            objCode1 += "0";

                            int displacement = Convert.ToInt32(symTable.GetAddress(intermediateParam1), 16);
                            displacement -= Convert.ToInt32(baseRegisterContents, 16);
                            objCode2 = Convert.ToInt32(baseRegister).ToString("X") + displacement.ToString("X").PadLeft(3, '0');
                        }

                        /* The second parameter is a label (with extra increment). */
                        else if (intermediateParam1.Contains('+'))
                        {
                           
                            int plusIndex = intermediateParam1.IndexOf('+');
                            string first = intermediateParam1.Substring(0, plusIndex);
                            string second = intermediateParam1.Substring(plusIndex + 1);

                            /* The label is first. */
                            if (symTable.IsSymbol(first))
                            {
                                int displacement = Convert.ToInt32(symTable.GetAddress(first), 16);
                                displacement += Convert.ToInt32(second);
                                displacement -= Convert.ToInt32(baseRegisterContents, 16);
                                objCode2 = Convert.ToInt32(baseRegister).ToString("X") +
                                           displacement.ToString("X").PadLeft(3, '0');
                            }

                            /* The label is second. */
                            else if (symTable.IsSymbol(second))
                            {
                                objCode1 += "0";

                                int displacement = Convert.ToInt32(symTable.GetAddress(second), 16);
                                displacement += Convert.ToInt32(first);
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

                        /* The second parameter is a label (with extra decrement). */
                        else if (intermediateParam1.Contains('-'))
                        {
                            objCode1 += "0";

                            int plusIndex = intermediateParam1.IndexOf('+');
                            string first = intermediateParam1.Substring(0, plusIndex);
                            string second = intermediateParam1.Substring(plusIndex + 1);

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

                            int plusIndex = intermediateParam1.IndexOf('+');
                            string first = intermediateParam1.Substring(0, plusIndex);
                            string second = intermediateParam1.Substring(plusIndex + 1);

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

                            int plusIndex = intermediateParam1.IndexOf('+');
                            string first = intermediateParam1.Substring(0, plusIndex);
                            string second = intermediateParam1.Substring(plusIndex + 1);

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
                                    d = Convert.ToInt32(intermediateParam1.Substring(0, openParanIndex));
                                    b = Convert.ToInt32(intermediateParam1.Substring(openParanIndex + 1, closeParanIndex - commaIndex - 1));
                                }

                                /* Format is D(X,B). */
                                else
                                {
                                    d = Convert.ToInt32(intermediateParam1.Substring(0, openParanIndex));
                                    x = Convert.ToInt32(intermediateParam1.Substring(openParanIndex + 1, commaIndex - openParanIndex - 1));
                                    b = Convert.ToInt32(intermediateParam1.Substring(commaIndex + 1, closeParanIndex - commaIndex - 1));
                                }
                            }

                            /* Format is D(X). */
                            else
                            {
                                d = Convert.ToInt32(intermediateParam1.Substring(0, openParanIndex));
                                x = Convert.ToInt32(intermediateParam1.Substring(openParanIndex + 1, closeParanIndex - openParanIndex - 1));
                            }

                            objCode1 += x.ToString("X");
                            objCode2 = b.ToString("X") + d.ToString("X").PadLeft(3, '0');
                        }

                        /* Format is D. */
                        else
                        {
                            d = Convert.ToInt32(intermediateParam1);
                            objCode1 += x.ToString("X");
                            objCode2 = b.ToString("X") + d.ToString("X").PadLeft(3, '0');
                        }

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
                        string tempCharString = intermediateParam1.Substring(intermediateParam1.IndexOf('\'') + 1, intermediateParam1.Length - 1);

                        for (int i = 0; i < tempCharString.Length; i++)
                        {
                            objCode1 += tempCharString[i];
                        }
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
            int previousLocation = 0;

            /* Open the writing stream to the intermediate file. */
            StreamWriter intermediateOutStream = new StreamWriter(intermediateFile, true);

            /* 
             * If the first column contains a *, the row is commented out so ignore line. 
             * If #, the row is an option line so ignore.
             */
            if (line[0] != '*' && line[0] != '#')
            {
                /* Store the label portion of the line and validate. */
                label = line.Substring(0, 8).TrimEnd();
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
                        if (line.IndexOf(',', 18) > line.IndexOf('\''))
                        {
                            parameterLastIndex = line.IndexOf(',', 18);
                            parameterLastIndex = line.IndexOf(' ', parameterLastIndex) - 14;

                            if (parameterLastIndex < 0)
                                parameterLastIndex = line.Length - 15;
                        }
                            
                        /* The second parameter is the character string. */
                        else
                            parameterLastIndex = line.IndexOf('\'', 18) - 14;

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
                        index = parameters.IndexOf("CL")+2;
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

                    //locationCounter += increment;
                }

                /* Field is a Declared Constant field. */
                else if (instruction == "DC")
                {
                    previousLocation = locationCounter;
                    int index = 0;
                    int increment = 0;

                    //if (line.IndexOf(' ', 15) < 0)
                    //    parameters = line.Substring(15);
                    //else
                    //    parameters = line.Substring(15, line.IndexOf(' ', 15) - 14).TrimEnd();

                    index = line.IndexOf('\'',15) + 1;
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
                }

                ///* If there is a label and it is not already in the symbol table, it's added. */
                //if (label != "" && !label.StartsWith(" "))
                //{
                //    if (!symTable.IsSymbolFull())
                //    {
                //        if (!symTable.IsSymbol(label))
                //            symTable.Hash(label, locationCounter.ToString("X").PadLeft(6, '0'));
                //        else
                //        {
                //            errorStream[numErrors, ERROR_LINE] = (lineNumber + 1).ToString();
                //            errorStream[numErrors, ERROR_DETAIL] = "Error: Label is invalid, already exists ";
                //            errorStream[numErrors, ERROR_SOURCE] = line;
                //            numErrors++;
                //            //return false;
                //        }
                //    }
                //    else
                //    {
                //        /* Need to set error for too many symbols. */
                //        Console.Beep();
                //    }
                //}

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

                    if (instruction == "START" || instruction == "END")
                        parameterList = null;

                    /* The parameters are not character literals or D(X,B). */
                    else if (!parameters.Contains("=C") && (parameters.LastIndexOf('(') < 0 || (parameters.LastIndexOf(',') < parameters.LastIndexOf('('))))
                        parameterList = parameters.Split(',');     

                    /* The first parameter is a string of characters. */
                    else if (parameters.Contains("=C") && parameters.IndexOf("=C") < parameters.IndexOf(','))
                    {
                        parameterList[0] = parameters.Substring(0, parameters.IndexOf('\'', 3) + 1).TrimEnd();
                        parameterList[1] = parameters.Substring(parameters.LastIndexOf(',')+1).TrimEnd();
                        parameterList[2] = "";
                    }
                    
                    /* The string of characters is the second parameter. */
                    else if (parameters.Contains("=C") && parameters.IndexOf("=C") >= parameters.IndexOf(','))
                    {
                        parameterList[0] = parameters.Substring(0, parameters.IndexOf(',') + 1).TrimEnd();
                        parameterList[1] = parameters.Substring(parameters.IndexOf(',') + 1).TrimEnd();
                        parameterList[2] = "";
                    }

                    /* The first parameter is D(X,B). */
                    else if (parameters.LastIndexOf(',') > parameters.LastIndexOf(')'))
                    {
                        parameterList = parameters.Split(',');
                        parameterList[0] += "," + parameterList[1];
                        parameterList[1] = parameterList[2];

                        string[] tempArray = new string[parameterList.Length - 1];
                        Array.Copy(parameterList, tempArray, parameterList.Length - 1);

                        parameterList = tempArray;
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
                        intermediateOutStream.WriteLine("{0}|{1}|{2}|{3}", (lineNumber+1), locationCounterHex, label, instruction.TrimEnd());
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
                

                //line = String.Format("{0,11} {1}", (lineNumber+1), line);

                //locationCounter += 4;
            }

            /* Increment the current line number, close the writing stream to intermediate file. */
            lineNumber++;
            intermediateOutStream.Close();
        }

        /******************************************************************************************
         * 
         * Name:        ToEBCDIC   
         * 
         * Author(s):   Travis Hunt
         *                  
         * Input:       The value is a char to be converted. 
         * Return:      The result is a string of the EBCDIC character code.
         * Description: This method will return the EBCDIC character code for the input string.
         *              
         *****************************************************************************************/
        private string ToEBCDIC(char value)
        {
            string valString = value.ToString();

            string[,] EBCDICValues = {{"0","F0"}, {"1","F1"}, {"2","F2"}, {"3","F3"}, {"4","F4"},
                                      {"5","F5"}, {"6","F6"}, {"7","F7"}, {"8","F8"}, {"9","F9"},
                                      {"A","C1"}, {"B","C2"}, {"C","C3"}, {"D","C4"}, {"E","C5"},
                                      {"F","C6"}, {"G","C7"}, {"H","C8"}, {"I","C9"}, {"J","D1"},
                                      {"K","D2"}, {"L","D3"}, {"M","D4"}, {"N","D5"}, {"O","D6"}, 
                                      {"P","D7"}, {"Q","D8"}, {"R","D9"}, {"S","E2"}, {"T","E3"}, 
                                      {"U","E4"}, {"V","E5"}, {"W","E6"}, {"X","E7"}, {"Y","E8"}, 
                                      {"Z","E9"}, {" ","40"}, {":","7A"}};

            for (int i = 0; i < EBCDICValues.Length / 2; i++)
            {
                if (EBCDICValues[i, 0] == valString)
                    return EBCDICValues[i, 1];
            }

            return "";
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

                /* Decide how much to increment the location counter from the instruction format. */
                switch (opType)
                {
                    case "RR":
                        locationCounter += 2;
                        break;

                    case "RX":
                    case "RS":
                    case "SI":
                        locationCounter += 4;
                        break;

                    case "X":
                    case "SS":
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
                      


            if (parameters.LastIndexOf('(') < parameters.LastIndexOf(',') && parameters.LastIndexOf('(') > 0)
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
                        if (!litTable.IsLiteral(param))
                            litTable.Hash(param, locationCounter.ToString("X").PadLeft(6, '0'));
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
