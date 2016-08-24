using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: ErrorDetection
 * 
 * ================================================================================================
 * 
 * Description: This class contains all the exceptions that can be thrown while performing the 
 *              instructioncs contained in Library.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/27/2014   JMB     Original version (file created). Code originally partly implemented by CAF.
 *                          Updated to allow PRTs to be printed across multiple pages if they 
 *                          include fatal exceptions. Migrated ErrorDetection class from Library.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    class ErrorDetection : Exception
    {
        /* Constants. */
        private const int MAX_BRANCH_TRACE = 10;
        private const int MAX_INSTRUCTION_TRACE = 10;
        private const int MAX_LINES_PER_PAGE = 45;
        private const string ELEVEN_SPACES = "           ";
        private const string EXECUTION_COMPLETE = "*** EXECUTION COMPLETED ***";
        private const string FIVE_SPACES = "     ";
        private const string FOUR_SPACES = "    ";
        private const string SIX_SPACES = "      ";
        private const string THREE_SPACES = "   ";
        private const string TWELVE_SPACES = "            ";
        private const uint DOUBLEWORD = 8;
        private const uint FULLWORD = 4;
        private const uint HALFWORD = 2;
        private const uint SS_SIZE = 6;


        /* Private members. */
        private MainForm display;
        private Memory mainMemory;
        private PSW progStatWord;
        private Queue<uint> branchTrace;
        private Queue<uint> instructionTrace;
        private Register[] registers;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        ErrorDetection        
         * 
         * Author(s):   Chad Farley     
         *              
         * Input:       displayDriver is a MainForm object, mainMemoryDriver is a Memory object,
         *              registersDriver is an array of Register objects, and progStatWord is 
         *              PSW object,
         * Return:      N/A
         * Description: This overloaded constructor sets the ErrorDetection object to a
         *              user-specified state.
         *              
         *****************************************************************************************/
        public ErrorDetection(MainForm displayDriver, Memory mainMemoryDriver,
            Register[] registersDriver, PSW progStatWordDriver)
        {
            display = displayDriver;
            mainMemory = mainMemoryDriver;
            registers = registersDriver;
            progStatWord = progStatWordDriver;
            branchTrace = new Queue<uint>(MAX_BRANCH_TRACE);
            instructionTrace = new Queue<uint>(MAX_INSTRUCTION_TRACE);
        }

        /******************************************************************************************
         * 
         * Name:        addBranch         
         * 
         * Author(s):   Chad Farley   
         *              
         * Input:       The address is an unsigned integer.
         * Return:      N/A
         * Description: This method will add a memory location (address) to the branchTree.
         *              
         *****************************************************************************************/
        public void addBranch(uint address)
        {
            /* Remove an entry if the trace is already full. */
            if (branchTrace.Count() == MAX_BRANCH_TRACE)
            {
                branchTrace.Dequeue();
                branchTrace.Enqueue(address);
            }

            else
                branchTrace.Enqueue(address);
        }

        /******************************************************************************************
         * 
         * Name:        addInstruction        
         * 
         * Author(s):   Chad Farley 
         *              
         * Input:       The address is an unsigned integer.
         * Return:      N/A
         * Description: This method will add a memory location (address) to the instructionTrace.
         *              
         *****************************************************************************************/
        public void addInstruction(uint address)
        {
            /* Remove an entry if the trace is already full. */
            if (instructionTrace.Count() == MAX_INSTRUCTION_TRACE)
            {
                instructionTrace.Dequeue();
                instructionTrace.Enqueue(address);
            }

            else
                instructionTrace.Enqueue(address);
        }

        /******************************************************************************************
         * 
         * Name:        throwException       
         * 
         * Author(s):   Chad Farley   
         *              
         * Input:       The error is a string.
         * Return:      N/A
         * Description: This method will call printError to report the appropriate exception. The
         *              error string should be from the ExceptionCodes class.
         *              
         *****************************************************************************************/
        public void throwException(string error)
        {
            switch (error)
            {
                /* Invalid object code. */
                case ExceptionCodes.OPERATION_EXCEPTION:
                    printError(error + " OPERATION");
                    break;

                /* Accessing out of program bounds. */
                case ExceptionCodes.PROTECTION_EXCEPTION:
                    printError(error + " PROTECTION");
                    break;

                /* Accessing out of memory bounds. */
                case ExceptionCodes.ADDRESSING_EXCEPTION:
                    printError(error + " ADDRESSING");
                    break;

                /* Address off of boundaries. */
                case ExceptionCodes.SPECIFICATION_EXCEPTION:
                    printError(error + " SPECIFICATION");
                    break;

                /* Invalid packed number. */
                case ExceptionCodes.DATA_EXCEPTION:
                    printError(error + " DATA");
                    break;

                /* Arithmetic operation too large/small. */
                case ExceptionCodes.FP_OVERFLOW_EXCEPTION:
                    printError(error + " FIXED POINT OVERFLOW");
                    break;

                /* Divide by zero/quotient too large. */
                case ExceptionCodes.FP_DIVIDE_EXCEPTION:
                    printError(error + " FIXED POINT DIVIDE");
                    break;

                /* Arithmetic PACKED operation too large/small. */
                case ExceptionCodes.DEC_OVERFLOW_EXCEPTION:
                    printError(error + " DECIMAL OVERFLOW");
                    break;

                /* Divide PACKED by zero/PACKED quotient too large. */
                case ExceptionCodes.DEC_DIVIDE_EXCEPTION:
                    printError(error + " DECIMAL DIVIDE");
                    break;
            }
        }


        /* Protected methods. */

        /******************************************************************************************
         * 
         * Name:        ToEBCDICChars  
         * 
         * Author(s):   Michael Beaver
         *                  
         * Input:       The value is a string. 
         * Return:      The result is a string.
         * Description: This method will return the EBCDIC character representation for a given
         *              hexadecimal value ("value"). All non-printable, non-viewable characters
         *              are represented by periods (".") as is done in ASSIST/I. If an error
         *              occurs, the returned result will be a period. Otherwise, the returned
         *              result will be the converted character.
         *              
         *****************************************************************************************/
        protected string ToEBCDICChars(string value)
        {
            int index;
            string[] EBCDICValues = {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".",
                ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".",
                ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".",
                ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", 
                ".", ".", ".", ".", " ", ".", ".", ".", ".", ".", ".", ".", ".", ".", "¢", ".",
                "<", "(", "+", "|", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "!", "$",
                "*", ")", ";", "¬", "-", "/", ".", ".", ".", ".", ".", ".", ".", ".", "¦", ",", 
                "%", "_", ">", "?", ".", ".", ".", ".", ".", ".", ".", ".", ".", "`", ":", "#",
                "@", "'", "=", "\"", ".", "a", "b", "c", "d", "e", "f", "g", "h", "i", ".", ".",
                ".", ".", ".", ".", ".", "j", "k", "l", "m", "n", "o", "p", "q", "r", ".", ".", 
                ".", ".", ".", ".", ".", "~", "s", "t", "u", "v", "w", "x", "y", "z", ".", ".",
                ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".",
                ".", ".", ".", ".", "{", "A", "B", "C", "D", "E", "F", "G", "H", "I", ".", ".", 
                ".", ".", ".", ".", "}", "J", "K", "L", "M", "N", "O", "P", "Q", "R", ".", ".",
                ".", ".", ".", ".", "\\", ".", "S", "T", "U", "V", "W", "X", "Y", "Z", ".", ".",
                ".", ".", ".", ".", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", ".", 
                ".", ".", ".", "."};
            string result = "";

            /* Convert to the EBCDIC character. */
            try
            {
                for (int i = 0; i < value.Length; i += 2)
                {
                    index = Convert.ToInt32(value[i].ToString() + value[i + 1].ToString(), 16);
                    result += EBCDICValues[index];
                }
            }

            /* The index was out of bounds. */
            catch (IndexOutOfRangeException)
            {
                result = ".";
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        getBranchTrace         
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The errorMessage is a string.
         * Return:      N/A
         * Description: This method updates the errorMessage string to contain the formatted
         *              branch trace.
         *              
         *****************************************************************************************/
        protected void getBranchTrace(ref string errorMessage)
        {
            int opCodeIndex = 0;
            uint[] branchList = branchTrace.ToArray();
            uint currentBranch = 0;

            /* Process the entire branch trace. */
            for (uint i = 0; i < branchList.Count(); i++)
            {
                currentBranch = branchList[i];

                /* Output the address of the branch. */
                errorMessage += TWELVE_SPACES + currentBranch.ToString("X").PadLeft(6, '0')
                    + FIVE_SPACES;

                /* Look up the address's object code in the MachineOpTable. */
                opCodeIndex = 
                    MachineOpTable.IsOpcode(
                        MachineOpTable.GetMnemonicFromObjectCode(
                            mainMemory.GetByteHex(currentBranch), 2));

                /* Output the appropriate number of bytes according to the instruction type. */
                switch (MachineOpTable.GetOpType(opCodeIndex))
                {
                    case "RR":
                        errorMessage += 
                            mainMemory.GetBytesString(currentBranch, currentBranch + HALFWORD - 1);
                        break;
                    case "RS":
                    case "RX":
                    case "X":
                        errorMessage += 
                            mainMemory.GetBytesString(currentBranch, currentBranch + HALFWORD - 1)
                                + " " + mainMemory.GetBytesString(currentBranch + HALFWORD, 
                                    currentBranch + FULLWORD - 1);
                        break;
                    case "SI":
                    case "SS":
                        errorMessage += 
                            mainMemory.GetBytesString(currentBranch, currentBranch + HALFWORD - 1)
                                + " " + mainMemory.GetBytesString(currentBranch + HALFWORD, 
                                    currentBranch + FULLWORD - 1) + " " + 
                                        mainMemory.GetBytesString(currentBranch + FULLWORD, 
                                            currentBranch + SS_SIZE - 1);
                        break;
                    default:
                        /* Invalid opcode. */
                        errorMessage += "Invalid Opcode";
                        break;
                }

                /* Last branch. */
                if (i == branchList.Count() - 1)
                    errorMessage += FOUR_SPACES + "<-- Last branch executed.";

                errorMessage += Environment.NewLine;
            }
        }

        /******************************************************************************************
         * 
         * Name:        getInstructionTrace         
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The errorMessage is a string.
         * Return:      N/A
         * Description: This method updates the errorMessage string to contain the formatted
         *              instruction trace.
         *              
         *****************************************************************************************/
        protected void getInstructionTrace(ref string errorMessage)
        {
            int opCodeIndex = 0;
            uint[] instructionList = instructionTrace.ToArray();
            uint currentInstruction = 0;

            /* Process the entire instruction trace. */
            for (uint i = 0; i < instructionList.Count(); i++)
            {
                currentInstruction = instructionList[i];

                /* Output the address of the instruction. */
                errorMessage += TWELVE_SPACES + currentInstruction.ToString("X").PadLeft(6, '0')
                    + FIVE_SPACES;

                /* Special cases: XPRNT and XREAD. */
                if (mainMemory.GetByteHex(currentInstruction) == "E0")
                {
                    /* XREAD. */
                    if (mainMemory.GetByteHex(currentInstruction + 1)[0] == '0')
                    {
                        opCodeIndex =
                            MachineOpTable.IsOpcode(
                                MachineOpTable.GetMnemonicFromObjectCode(
                                    mainMemory.GetByteHex(currentInstruction), 1));
                    }
                        
                    /* XPRNT. */
                    else if (mainMemory.GetByteHex(currentInstruction + 1)[0] == '2')
                    {
                        opCodeIndex = 
                            MachineOpTable.IsOpcode(
                                MachineOpTable.GetMnemonicFromObjectCode(
                                    mainMemory.GetByteHex(currentInstruction), 0));
                    }
                }

                else
                {
                    opCodeIndex = 
                        MachineOpTable.IsOpcode(
                            MachineOpTable.GetMnemonicFromObjectCode(
                                mainMemory.GetByteHex(currentInstruction), 2));
                }
 
                /* Output the appropriate number of bytes according to the instruction type. */
                switch (MachineOpTable.GetOpType(opCodeIndex))
                {
                    case "RR":
                        errorMessage += 
                            mainMemory.GetBytesString(currentInstruction, 
                                currentInstruction + HALFWORD - 1);
                        break;
                    case "RS":
                    case "RX":
                        errorMessage += 
                            mainMemory.GetBytesString(currentInstruction, 
                                currentInstruction + HALFWORD - 1) + " " + 
                                    mainMemory.GetBytesString(currentInstruction + HALFWORD, 
                                        currentInstruction + FULLWORD - 1);
                        break;
                    case "X":
                    case "SI":
                    case "SS":
                        errorMessage += 
                            mainMemory.GetBytesString(currentInstruction, 
                                currentInstruction + HALFWORD - 1) + " " + 
                                    mainMemory.GetBytesString(currentInstruction + HALFWORD, 
                                        currentInstruction + FULLWORD - 1) + " " + 
                                            mainMemory.GetBytesString(currentInstruction + FULLWORD, 
                                                currentInstruction + SS_SIZE - 1);
                        break;
                    default:
                        /* Invalid opcode. */
                        errorMessage += "Invalid Opcode";
                        break;
                }

                /* Indicate the last instruction executed. */
                if (i == instructionList.Count() - 1)
                    errorMessage += FOUR_SPACES + "<-- Last instruction executed.";

                errorMessage += Environment.NewLine;
            }
        }

        /******************************************************************************************
         * 
         * Name:        performMemoryDump         
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The errorMessage is a string.
         * Return:      N/A
         * Description: This method updates the errorMessage string to contain the formatted
         *              memory dump.
         *              
         *****************************************************************************************/
        protected void performMemoryDump(ref string errorMessage)
        {
            bool sameLineFlag = false;
            int firstSpaceIndex = 0;
            int numLines = 0;
            string memoryRow = "";
            string lastMemoryRow = "NULL";
            uint sameMemoryStart = 0;
            uint sameMemoryEnd = 0;
            uint maxMemory = mainMemory.GetMemorySize();
            uint lastLineLength = 0;

            /* 
             * Calculate the length of the last row in memory. This is used for memories that are
             * not evenly divisible by DOUBLEWORD * 2.
             */
            lastLineLength = maxMemory % (DOUBLEWORD * 2);

            /* Format each row in memory for output. */
            for (uint i = 0; i <= maxMemory; i += (DOUBLEWORD * 2))
            {

                if (numLines >= MAX_LINES_PER_PAGE)
                {
                    errorMessage += "\f";
                    numLines = 0;
                }

                /* This is a full-length line. */
                if (i + lastLineLength < maxMemory)
                    memoryRow = mainMemory.GetBytesString(i, i + (DOUBLEWORD * 2) - 1);

                /* This line is not full-length. */
                else
                    memoryRow = mainMemory.GetBytesString(i, i + lastLineLength - 1);

                /* Track whether contiguous lines are the same. */
                if (lastMemoryRow != "NULL" && lastMemoryRow == memoryRow)
                {
                    if (sameLineFlag == false)
                    {
                        sameLineFlag = true;
                        sameMemoryStart = i - (DOUBLEWORD * 2);
                    }
                }

                /* The lines are not similar, so output the range of similar preceding lines. */
                else if (sameLineFlag == true)
                {
                    sameMemoryEnd = i - (DOUBLEWORD * 2);

                    errorMessage += TWELVE_SPACES + "LINES:" + THREE_SPACES +
                        sameMemoryStart.ToString("X").PadLeft(6, '0') + " - " +
                            sameMemoryEnd.ToString("X").PadLeft(6, '0') + FOUR_SPACES +
                                "ARE IDENTICAL" + Environment.NewLine;

                    numLines += 2;

                    sameLineFlag = false;
                }

                /* Format and output the memory contents. */
                if (sameLineFlag == false && memoryRow != null)
                {
                    /* Format the address. */
                    errorMessage += i.ToString("X").PadLeft(6, '0');
                    errorMessage += SIX_SPACES;

                    /* The line is short, so pad it with spaces. */
                    if (memoryRow.Length < (DOUBLEWORD * 4))
                        memoryRow = memoryRow.PadRight((int)DOUBLEWORD * 4, ' ');

                    /* 
                     * Format the four fullwords on the row. 
                     * Note that DOUBLEWORD will select 8 characters. 
                     */
                    errorMessage += memoryRow.Substring(0, (int)DOUBLEWORD);
                    errorMessage += THREE_SPACES;
                    errorMessage += memoryRow.Substring((int)DOUBLEWORD, (int)DOUBLEWORD);
                    errorMessage += THREE_SPACES;
                    errorMessage += memoryRow.Substring((int)DOUBLEWORD * 2, (int)DOUBLEWORD);
                    errorMessage += THREE_SPACES;
                    errorMessage += memoryRow.Substring((int)DOUBLEWORD * 3, (int)DOUBLEWORD);
                    errorMessage += SIX_SPACES + "*";

                    /* For short rows, only output certain characters (not the padded spaces). */
                    if (memoryRow.Contains(' '))
                    {
                        firstSpaceIndex = memoryRow.IndexOf(' ', 0);
                        firstSpaceIndex /= 2;
                        errorMessage += ToEBCDICChars(mainMemory.GetBytesString(i,
                            i + (uint)firstSpaceIndex - 1));
                    }

                    /* Output the character representation for the whole row. */
                    else
                    {
                        errorMessage += ToEBCDICChars(mainMemory.GetBytesString(i,
                            i + (DOUBLEWORD * 2) - 1));
                    }

                    errorMessage += "*" + Environment.NewLine;

                    numLines++;
                }
                
                lastMemoryRow = memoryRow;
            }
        }

        /******************************************************************************************
         * 
         * Name:        performRegisterDump         
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The errorMessage is a string.
         * Return:      N/A
         * Description: This method updates the errorMessage string to contain the formatted
         *              contents of the registers.
         *              
         *****************************************************************************************/
        protected void performRegisterDump(ref string errorMessage)
        {
            errorMessage += "R0-7 :";

            /* Output the first 7 registers. */
            for (int i = 0; i <= 7; i++)
            {
                errorMessage += registers[i].GetBytesString(0, FULLWORD - 1);
                errorMessage += " ";
            }

            errorMessage += Environment.NewLine;

            errorMessage += "R8-15:";

            /* Output the remaining registers. */
            for (int i = 8; i <= 15; i++)
            {
                errorMessage += registers[i].GetBytesString(0, FULLWORD - 1);
                errorMessage += " ";
            }
        }


        /* Private methods. */

        /******************************************************************************************
         * 
         * Name:        printError         
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The errorMessage is a string.
         * Return:      N/A
         * Description: This method calls the appropriate helper methods to generate the core dump
         *              that is displayed in the Output window and in the PRT file.
         *              
         *****************************************************************************************/
        private void printError(string errorMessage)
        {
            StreamWriter prtFileOutput;
            string displayError;

            prtFileOutput = new StreamWriter(display.GetPathPRT(), true);

            /* End of Simulator simulation. */
            prtFileOutput.Write(Environment.NewLine + Environment.NewLine + EXECUTION_COMPLETE);
            display.AppendOutputText(Environment.NewLine + Environment.NewLine + 
                EXECUTION_COMPLETE);

            /* PSW error. */
            displayError = "\f"; // Environment.NewLine;
            displayError += "PSW AT ABEND:  XXXXXXXX ";

            switch (progStatWord.GetCondCodeInt())
            {
                case 0:
                    displayError += "00X";
                    break;
                case 1:
                    displayError += "01X";
                    break;
                case 2:
                    displayError += "10X";
                    break;
                case 3:
                    displayError += "11X";
                    break;
                default:
                    /* Invalid Condition Code. */
                    break;
            };

            /* The instruction address of PSW is 3 bytes (3 - 1 = 2 due to inclusive selection). */
            displayError += progStatWord.GetBytesString(0, 2);
            displayError += "      COMPLETION CODE: " + errorMessage +
                Environment.NewLine + Environment.NewLine;

            prtFileOutput.Write(displayError);
            display.AppendOutputText(displayError);

            /* Instruction trace start. */
            displayError = "** TRACE OF INSTRUCTIONS JUST BEFORE TERMINATION **"
                + Environment.NewLine + Environment.NewLine;

            /* Instruction trace header. */
            displayError += ELEVEN_SPACES + "LOCATION" + FOUR_SPACES + "INSTRUCTION"
                + Environment.NewLine;
            displayError += ELEVEN_SPACES + "========" + FOUR_SPACES + "==============="
                + Environment.NewLine;

            /* Instruction trace. */
            getInstructionTrace(ref displayError);
            displayError += Environment.NewLine + Environment.NewLine + Environment.NewLine;

            prtFileOutput.Write(displayError);
            display.AppendOutputText(displayError);

            /* Branch trace start. */
            displayError = "** TRACE OF LAST 10 BRANCH INSTRUCTIONS EXECUTED **"
                + Environment.NewLine + Environment.NewLine;

            /* Branch trace header. */
            displayError += ELEVEN_SPACES + "LOCATION" + FOUR_SPACES + "INSTRUCTION"
                + Environment.NewLine;
            displayError += ELEVEN_SPACES + "========" + FOUR_SPACES + "==============="
                + Environment.NewLine;

            /* Branch trace. */
            getBranchTrace(ref displayError);
            displayError += Environment.NewLine + Environment.NewLine +
                Environment.NewLine;

            prtFileOutput.Write(displayError);
            display.AppendOutputText(displayError);

            displayError = "";
            /* Register dump. */
            performRegisterDump(ref displayError);
            displayError += Environment.NewLine + Environment.NewLine +
                Environment.NewLine;

            prtFileOutput.Write(displayError);
            display.AppendOutputText(displayError);

            /* Memory dump. */
            displayError = "\fUSER STORAGE:   CORE ADDRESSES SPECIFIED - 000000 to ";
            displayError += 
                mainMemory.GetMemorySize().ToString("X").PadLeft(8, '0').Substring(2, 6);
            displayError += Environment.NewLine + Environment.NewLine;

            prtFileOutput.Write(displayError);
            display.AppendOutputText(displayError);

            displayError = "";
            performMemoryDump(ref displayError);
            displayError += Environment.NewLine + TWELVE_SPACES + FIVE_SPACES
                + "***  END OF USER MEMORY  ***" + Environment.NewLine;

            prtFileOutput.Write(displayError);
            display.AppendOutputText(displayError);

            prtFileOutput.Close();

            /* Exit condition to stop processor and return to Main Form. */
            throw new EndProcessingException();
        }
    }
}