using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**************************************************************************************************
 * 
 * Name: Library
 *  
 * ================================================================================================
 * 
 * Description: This class represents all available instructions available to the ASSIST/UNA 
 *              assembly language. Interaction with this class should be done through the public
 *              method SimulateInstruction, which will then call the necessary methods to perform
 *              the intended operations as specified in the object code.  
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/04/2014   CAF     Created class LibraryTest class and created the initial switch statement
 * 04/05/2014   CAF     Created shell functions for each of the methods, added headers for the class
 *                          and each of its methods, assisted in the address calculation methods.
 *              JMB     Created the address calculation methods, started the test driver.
 * 04/07/2014   JMB     Removed unnecessary uint to int type conversions. Updated DetermineAddressDB
 *                          and DetermineAddressDXB methods. Added Console output for testing.
 * 04/08/2014   CAF     Created additional helper functions for processing the different types of 
 *                          instructions. Created a rough draft of the Add instruction to test the
 *                          test the new helper function layout. Completed several instruction
 *                          methods.
 *              JMB     Corrected a mistake in the DetermineAddressDB and DetermineAddressDXB 
 *                          methods. Added and modified Travis's ToEBCDIC method. Completed several
 *                          instruction methods. CONCERN: For DXB addresses, when the Base register
 *                          is Register 0, do we use its contents or the value zero?
 * 04/09/2014   JMB     Updated DetermineAddressDB and DetermineAddressDXB to always use the value
 *                          zero rather than the contents of Register 0.
 * 04/10/2014   CAF     Updated comment sections and cleaned up a few of the functions.
 *              JMB     Implemented instructions: BAL, BC, BCT, BCTR, C, L, N, NR, O, OR, S, ST.
 *                          Replaced "magic numbers" in SimulateInstruction with constants. Added 
 *                          boundary alignment checking to several RX-based instructions' methods. 
 *                          Added a TODO list.
 * 04/11/2014   JMB     Assisted Chad with instruction implementations and implemented various
 *                          instructions with Chad. Corrected various minor faults. Solely
 *                          implemented instructions: SP, UNPK.
 *              CAF     Implemented various methods with JMB. Cleaned up existing functions to
 *                          work with new methods.
 * 04/12/2014   JMB     Implemented instructions: MP, DP.
 * 04/13/2014   JMB     Implemented instructions: ED, EDMK.
 * 04/14/2014   JMB     Updated: ED, EDMK. CONCERN: What does the SEPARATOR (X'22') do, exactly?
 *              CAF     Began working on finishing up the missing condition code clauses within the
 *                          existing methods.
 * 04/15/2014   CAF     Finished up possible condtion code sections. CONCERN: Certain methods had 
 *                          unclear condition code requirements, ZAP in particular, will need review
 *                          with JMB. Added more content to some of the description fields of method
 *                          headers.
 *              JMB     Updated ZAP. There were three if-statements comparing strings to integers.
 *                          All instructions should now have condition code updates as necessary.
 * 04/17/2014   CAF     Finished the description fields of the methods, consolidated test to live,
 *                          worked on simulator end condition.
 *              JMB     Implemented XDECI with CAF.
 * 04/18/2014   JMB     Updated documentation. Corrected various minor defects. Began XREAD.
 *              CAF     Created a reference to the entryStream of the List<string> type and display
 *                          which references the front end for display updates.
 * 04/19/2014   JMB     Started implementing XPRNT. Ran into several integration issues that will
 *                          need to be resolved as soon as possible.
 *              CAF     Assisted JMB in implementing XPRNT. 
 * 04/20/2014   JMB     Implemented ChangeDisplayRegisterContents to update main form register 
 *                          contents from the contents of the Simulator's registers. See 
 *                          PerformInstructionAR for example usage. Implemented 
 *                          ChangeDisplayMemoryContents to update a row of the main form memory 
 *                          contents given a specific address. See PerformInstructionMVI for
 *                          example usage.
 * 04/21/2014   CAF     Implemented ErrorDetection class and and began display integration for 
 *                          methods that manipulate memory, implemented error handling on all 
 *                          methods except X methods. Implemented processing stop upon runtime 
 *                          error.
 *              JMB     Assisted in the implementations. Implemented ExceptionCodes class.
 * 04/22/2014   JMB     Updated to better conform to standards. Fixed defects in several 
 *                          instructions, including the X-instructions. PerformInstructionMVC now
 *                          handles attempts to cascade bytes (e.g., cascading a blank to "erase"
 *                          memory). Tested using THPROG1, THPROG2, and THPROG3. Output to the PRT
 *                          appears to be correct, but output to the Output textbox can be wonky.
 *              CAF     Updated XPRNT to handle all carriage control. Added branchTrace to 
 *                          errorDetection.
 * 04/23/2014   JMB     Corrected several faults in various instructions, including EDMK and BCTR.
 *                          Tested using THPROG5.
 *              CAF     Updated simulateInstruction to update instruction trace.
 * 04/24/2014   JMB     Fixed the data comparisons in CLC and CLI. Corrected several faults,
 *                          including faulty length checking in CP.
 * 04/25/2014   JMB     Fixed a fault in IsPackedNumber where the method would try to validate
 *                          null (or empty) strings. Refined and updated printError and its
 *                          associated helper methods to output to the Output window and the PRT.
 *              CAF     Updated ChangeDisplayMemoryContents to correctly update mainMemory. 
 *                          Updated all functions that modified mainMemory to reflect updates to
 *                          the display. Implemented the printError method and its associated
 *                          helper methods.
 * 04/26/2014   JMB     Fixed getInstructionTrace and getBranchTrace. Fixed an error in DR.
 * 04/27/2014   JMB     Added checked() operator to A, AP, AR, D, DP, DR, M, MP, MR, S, SP, SR, and
 *                          ZAP instructions to check for overflow. Packed addition/subtraction
 *                          are also checked in the AddPackedValues() helper method. Also
 *                          migrated ExceptionCodes and ErrorDetection to their own respective
 *                          class files. Added SetLinesOnLastPage mutator to facilitate printing of
 *                          PRTs with multiple-page output. Fixed code formatting.
 *                                  
 *************************************************************************************************/

namespace Assist_UNA
{
    class Library
    {
        /* Constants. */
        /* REGISTER_SIZE is Size-1 to offset the start at 0. Size is 4, thus 4 - 1 = 3. */
        private const int CONSOLE_SIZE = 15;
        private const int EBCDIC_INT_MINUS = 96;
        private const int EBCDIC_INT_NINE = -7;
        private const int EBCDIC_INT_ZERO = -16;
        private const int MAX_LINES_PER_PAGE = 45;
        private const int MAX_PACK_BYTES = 16;
        private const int REGISTER_SIZE = 3;
        private const int XREAD_MAX = 80;
        private const string DIGIT_SELECTOR = "20";
        private const string DIGIT_SELECTOR_SIGN_START = "21";
        private const string EBCDIC_ASTERISK = "5C";
        private const string EBCDIC_BLANK = "40";
        private const string EBCDIC_COMMA = "6B";
        private const string EBCDIC_MINUS = "60";
        private const string EBCDIC_PERIOD = "4B";
        private const string SEPARATOR = "22";
        private const uint DOUBLEWORD = 8;
        private const uint FULLWORD = 4;
        private const uint HALFWORD = 2;
        private const uint MAX_DP_LENGTH2 = 8;
        private const uint MAX_MP_LENGTH2 = 8;
        private const uint MAX_REGISTER = 15;
        private const uint MEMORY_COLUMNS = 16;
        private const uint SS_SIZE = 6;
        private const uint XPRNT_MAX = 133;


        /* Private members. */
        private ErrorDetection errorHandling;
        private int dataEntryIndex;
        private int linesPrinted;
        private List<string> entryStream;
        private MainForm display;
        private Memory mainMemory;
        private PSW progStatWord;
        private Register[] registers;
        private uint maxMemorySize;
        private uint maxProgramMemory;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        Library
         * 
         * Author(s):   Chad Farley     
         *              
         * Input:       The mainMemoryDriver is a Memory object, the registersDriver is an array
         *              of register objects, and the progStatWordDriver is a PSW object.
         * Return:      N/A
         * Description: This is the constructor for the LibraryTest class. This constructor will
         *              save references to the parameters so that the methods will be able to access
         *              these data structures and modify the contents.
         *              
         *****************************************************************************************/
        public Library(Memory mainMemoryDriver, Register[] registersDriver, PSW progStatWordDriver, 
            List<string> entryStreamDriver, MainForm displayDriver, uint programMemory)
        {
            mainMemory = mainMemoryDriver;
            registers = registersDriver;
            progStatWord = progStatWordDriver;
            entryStream = entryStreamDriver;
            display = displayDriver;
            dataEntryIndex = 0;
            errorHandling = new ErrorDetection(displayDriver, mainMemoryDriver, 
                registersDriver, progStatWordDriver);
            maxMemorySize = mainMemory.GetMemorySize();
            maxProgramMemory = programMemory;
            linesPrinted = 0;
        }

        /******************************************************************************************
         * 
         * Name:        SetLinesOnLastPage
         * 
         * Author(s):   Michael Beaver
         *                 
         * Input:       The lines is an integer.
         * Return:      N/A
         * Description: This method will set the linesPrinted counter to the number of lines
         *              printed on the last of the PRT (as given by the Assembler).
         *              
         *****************************************************************************************/
        public void SetLinesOnLastPage(int lines)
        {
            linesPrinted = lines;
        }

        /******************************************************************************************
         * 
         * Name:        SimulateInstruction
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *                 
         * Input:       The locationCounter is an unsigned integer, andmainMemory is a 
         *              Memory object.
         * Return:      N/A
         * Description: This method will access in mainMemory at locationCounter to read in the 
         *              appropriate object code to determine which operation is to be performed,
         *              gather all pertinent information according to the object code, and pass
         *              this information to the appropriate method in accordance with the object
         *              code. 
         *              
         *****************************************************************************************/
        public void SimulateInstruction(ref uint locationCounter)
        {
            if (locationCounter > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (locationCounter > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            switch (mainMemory.GetByteHex(locationCounter))
            {
                /* A - Add. */
                case "5A":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionA(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* AP - Add Packed. */
                case "FA":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionAP(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* AR - Add Register. */
                case "1A":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionAR(locationCounter);
                    locationCounter += HALFWORD;
                    break;

                /* BAL - Branch and Link. */
                case "45":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    errorHandling.addBranch(locationCounter);
                    PerformInstructionBAL(ref locationCounter);
                    break;

                /* BALR - Branch and Link Register. */
                case "05":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    errorHandling.addBranch(locationCounter);
                    PerformInstructionBALR(ref locationCounter);
                    break;

                /* BC - Branch on Condition. */
                case "47":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    errorHandling.addBranch(locationCounter); 
                    PerformInstructionBC(ref locationCounter);
                    break;

                /* BCR - Branch on Condition Register. */
                case "07":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    errorHandling.addBranch(locationCounter);
                    PerformInstructionBCR(ref locationCounter);
                    break;

                /* BCT - Branch on Count. */
                case "46":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    errorHandling.addBranch(locationCounter);
                    PerformInstructionBCT(ref locationCounter);
                    break;

                /* BCTR - Branch on Count Register. */
                case "06":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    errorHandling.addBranch(locationCounter);
                    PerformInstructionBCTR(ref locationCounter);
                    break;

                /* BXH - Branch on Index High. */
                case "86":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    errorHandling.addBranch(locationCounter);
                    PerformInstructionBXH(ref locationCounter);
                    break;

                /* BXLE - Branch on Index Low or Equal. */
                case "87":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    errorHandling.addBranch(locationCounter);
                    PerformInstructionBXLE(ref locationCounter);
                    break;

                /* C - Compare. */
                case "59":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionC(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* CLC - Compare Logical Characters. */
                case "D5":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionCLC(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* CLI - Compare Logical Immediate. */
                case "95":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionCLI(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* CP - Compare Packed. */
                case "F9":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionCP(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* CR - Compare Register. */
                case "19":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionCR(locationCounter);
                    locationCounter += HALFWORD;
                    break;

                /* D - Divide. */
                case "5D":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionD(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* DP - Divide Packed. */
                case "FD":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionDP(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* DR - Divide Register. */
                case "1D":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionDR(locationCounter);
                    locationCounter += HALFWORD;
                    break;

                /* ED - Edit. */
                case "DE":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionED(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* EDMK - Edit and Mark. */
                case "DF":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionEDMK(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* L - Load. */
                case "58":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionL(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* LA - Load Address. */
                case "41":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionLA(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* LM - Load Multiple. */
                case "98":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionLM(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* LR - Load Register. */
                case "18":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionLR(locationCounter);
                    locationCounter += HALFWORD;
                    break;

                /* M - Multiply. */
                case "5C":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionM(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* MP - Multiply Packed. */
                case "FC":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionMP(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* MR - Multiply Register. */
                case "1C":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionMR(locationCounter);
                    locationCounter += HALFWORD;
                    break;

                /* MVC - Move Characters. */
                case "D2":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionMVC(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* MVI - Move Immediate */
                case "92":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionMVI(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* N - Bitwise AND. */
                case "54":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionN(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* NR - Bitwise AND Register. */
                case "14":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionNR(locationCounter);
                    locationCounter += HALFWORD;
                    break;

                /* O - Bitwise OR. */
                case "56":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionO(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* OR - Bitwise OR Register. */
                case "16":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionOR(locationCounter);
                    locationCounter += HALFWORD;
                    break;

                /* PACK - Pack. */
                case "F2":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionPACK(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* S - Subtract. */
                case "5B":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionS(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* SP - Subtract Packed. */
                case "FB":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionSP(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* SR - Subtract Register. */
                case "1B":
                    ChangeDisplayPSWContents(locationCounter, HALFWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionSR(locationCounter);
                    locationCounter += HALFWORD;
                    break;

                /* ST - Store. */
                case "50":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionST(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* STM - Store Multiple. */
                case "90":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionSTM(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* UNPK - Unpack. */
                case "F3":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionUNPK(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                /* XDECI - Convert Input to Decimal. */
                case "53":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionXDECI(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* XDECO - Convert Output to Decimal. */
                case "52":
                    ChangeDisplayPSWContents(locationCounter, FULLWORD);
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionXDECO(locationCounter);
                    locationCounter += FULLWORD;
                    break;

                /* XPRNT / XREAD. */
                case "E0":
                    /* XPRNT - Print Output. */
                    if (mainMemory.GetByteHex(locationCounter + 1)[0] == '2')
                    {
                        ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                        errorHandling.addInstruction(locationCounter);
                        PerformInstructionXPRNT(locationCounter);
                        locationCounter += SS_SIZE;
                    }

                    /* XREAD - Read Input. */
                    else if (mainMemory.GetByteHex(locationCounter + 1)[0] == '0')
                    {
                        ChangeDisplayPSWContents(locationCounter, SS_SIZE);
                        errorHandling.addInstruction(locationCounter);
                        PerformInstructionXREAD(locationCounter);
                        locationCounter += SS_SIZE;
                    }

                    /* ERROR. */
                    else
                        errorHandling.throwException(ExceptionCodes.OPERATION_EXCEPTION);

                    break;

                /* ZAP - Zero, Add Packed. */
                case "F8":
                    ChangeDisplayPSWContents(locationCounter, SS_SIZE); 
                    errorHandling.addInstruction(locationCounter);
                    PerformInstructionZAP(locationCounter);
                    locationCounter += SS_SIZE;
                    break;

                default:
                    errorHandling.throwException(ExceptionCodes.OPERATION_EXCEPTION);
                    break;
            };
        }


        /* Protected methods. */

        /******************************************************************************************
         * 
         * Name:        IsPackedNumber
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *                    
         * Input:       The packedNumber is a string.
         * Return:      Boolean the tells if packedNumber was a valid packed number or not.
         * Description: This methods takes a string that is then checked to determine if it is a 
         *              valid packed number. If the number is a valid packed number, the method 
         *              returns true, otherwise, it returns false.
         *              
         *****************************************************************************************/
        protected bool IsPackedNumber(string packedNumber)
        {
            bool result = false;
            char zone;
            int length;
            int temp;

            if (!String.IsNullOrEmpty(packedNumber))
            {
                length = packedNumber.Length;
                zone = packedNumber[length - 1];

                /* Maximum length is MAX_PACK_BYTES bytes. */
                if ((length / 2) > MAX_PACK_BYTES)
                    result = false;

                /* Check for valid zone character. */
                else if ((zone != 'A') && (zone != 'B') && (zone != 'C') &&
                         (zone != 'D') && (zone != 'E') && (zone != 'F'))
                    result = false;

                /* Check each digit for validity (i.e., in [0, 9]). */
                else
                {
                    for (int i = 0; i < length - 1; i++)
                    {
                        if (Int32.TryParse(packedNumber[i].ToString(), out temp))
                            result = true;

                        /* Immediately break if an invalid digit is detected. */
                        else
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            /* Null or empty string. */
            else
                result = false;

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        AddPackedValues
         * 
         * Author(s):   Michael Beaver
         *                    
         * Input:       The variables operand1 and operand2 are strings.
         * Return:      N/A
         * Description: Helper method that is used when attempted to add to packed values. This 
         *              method converts the strings from their packed states into their decimal
         *              equivalents and performs addition upon the values and returns a string
         *              containing the packed sum.
         *              
         *****************************************************************************************/
        protected string AddPackedValues(string operand1, string operand2)
        {
            int operand1Value = 0;
            int operand2Value = 0;
            int sum = 0;
            string result;

            /* Get integer values. */
            operand1Value = Convert.ToInt32(operand1.Substring(0, operand1.Length - 1), 10);
            operand2Value = Convert.ToInt32(operand2.Substring(0, operand2.Length - 1), 10);

            /* Checking for negative signs. */
            if (operand1[operand1.Length - 1] == 'B' || operand1[operand1.Length - 1] == 'D')
                operand1Value *= -1;

            if (operand2[operand2.Length - 1] == 'B' || operand2[operand2.Length - 1] == 'D')
                operand2Value *= -1;

            /* Guard against internal overflow. */
            try
            {
                sum = checked(operand1Value + operand2Value);
            }

            catch
            {
                result = null;
            }
            
            result = sum.ToString();

            /* Negative value. */
            if (sum < 0)
            {
                result += "D";
                result = result.Replace("-", "");
            }

            /* Zero or positive value. */
            else 
                result += "C";

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        ToEBCDIC   
         * 
         * Author(s):   Travis Hunt
         *              Michael Beaver
         *              Reference 3
         *                  
         * Input:       The value is a char to be converted. 
         * Return:      The result is a string of the EBCDIC character code.
         * Description: This method will return the EBCDIC character code for the input char.
         * 
         *              
         *****************************************************************************************/
        protected string ToEBCDIC(string value)
        {
            char tempChar;
            int[] a2e = new int[256]{0, 1, 2, 3, 55, 45, 46, 47, 22, 5, 37, 11, 12, 13, 14, 15, 16, 
                17, 18, 19, 60, 61, 50, 38, 24, 25, 63, 39, 28, 29, 30, 31, 64, 79, 127, 123, 91, 
                108, 80, 125, 77, 93, 92, 78, 107, 96, 75, 97, 240, 241, 242, 243, 244, 245, 246, 
                247, 248, 249, 122, 94, 76, 126, 110, 111, 124, 193, 194, 195, 196, 197, 198, 199, 
                200, 201, 209, 210, 211, 212, 213, 214, 215, 216, 217, 226, 227, 228, 229, 230, 231,
                232, 233, 74, 224, 90, 95, 109, 121, 129, 130, 131, 132, 133, 134, 135, 136, 137, 
                145, 146, 147, 148, 149, 150, 151, 152, 153, 162, 163, 164, 165, 166, 167, 168, 169,
                192, 106, 208, 161, 7, 32, 33, 34, 35, 36, 21, 6, 23, 40, 41, 42, 43, 44, 9, 10, 27, 
                48, 49, 26, 51, 52, 53, 54, 8, 56, 57, 58, 59, 4, 20, 62, 225, 65, 66, 67, 68, 69, 
                70, 71, 72, 73, 81, 82, 83, 84, 85, 86, 87, 88, 89, 98, 99, 100, 101, 102, 103, 104,
                105, 112, 113, 114, 115, 116, 117, 118, 119, 120, 128, 138, 139, 140, 141, 142, 143,
                144, 154, 155, 156, 157, 158, 159, 160, 170, 171, 172, 173, 174, 175, 176, 177, 178,
                179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 202, 203, 204, 205,
                206, 207, 218, 219, 220, 221, 222, 223, 234, 235, 236, 237, 238, 239, 250, 251, 252,
                253, 254, 255};
            string result = "";

            try
            {
                /* Convert to EBCDIC. */
                for (int i = 0; i < value.Length; i++)
                {
                    tempChar = Convert.ToChar(a2e[(int)value[i]]);
                    result += Convert.ToInt32(tempChar).ToString("X");
                }
            }

            catch
            {
                result = null;
            }

            return result;
        }

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
         * Name:        DetermineAddressDB
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The addressStart is an unsigned integer.
         * Return:      An unsigned integer value that is an index into mainMemory.
         * Description: This method is used to determine what address location in memory is to be
         *              accessed. This method will add the displacement and the contents of the
         *              base register to calculate the address in memory.
         *              
         *****************************************************************************************/
        protected uint DetermineAddressDB(uint addressStart)
        {
            string addressParameters;
            uint displacement = 0;
            uint index = 0;
            uint regNo = 0;
            uint regValue = 0;

            addressParameters = mainMemory.GetBytesString(addressStart, addressStart + 1);

            displacement = Convert.ToUInt32(addressParameters.Substring(1, REGISTER_SIZE), 16);
            regNo = Convert.ToUInt32(addressParameters[0].ToString(), 16);
            
            /* Do not use the contents of Register 0. */
            if (regNo == 0)
                regValue = 0;

            else
                regValue = Convert.ToUInt32(registers[regNo].GetBytesString(0, REGISTER_SIZE), 16);

            index = displacement + regValue;

            return index;
        }

        /******************************************************************************************
         * 
         * Name:        DetermineAddressDXB
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The addressStart is an unsigned integer.
         * Return:      An unsigned integer value that is an index into mainMemory.
         * Description: This method is used to determine what address location in memory is to be
         *              accessed. This method will add the displacement and the contents of the
         *              base register and the index register to calculate the address in memory.  
         *              
         *****************************************************************************************/
        protected uint DetermineAddressDXB(uint addressStart)
        {
            string addressParameters;
            uint baseRegNo = 0;
            uint baseRegValue = 0;
            uint displacement = 0;
            uint index = 0;
            uint indexRegNo = 0;
            uint indexRegValue = 0;

            addressParameters = mainMemory.GetBytesString(addressStart, addressStart + 2);

            indexRegNo = Convert.ToUInt32(addressParameters[1].ToString(), 16);
            baseRegNo = Convert.ToUInt32(addressParameters[2].ToString(), 16);

            displacement = Convert.ToUInt32(addressParameters.Substring(3, 3), 16);

            /* Do not use the contents of Register 0. */
            if (indexRegNo == 0)
                indexRegValue = 0;

            else
                indexRegValue = 
                    Convert.ToUInt32(registers[indexRegNo].GetBytesString(0, REGISTER_SIZE), 16);

            /* Do not use the contents of Register 0. */
            if (baseRegNo == 0)
                baseRegValue = 0;

            else
                baseRegValue = 
                    Convert.ToUInt32(registers[baseRegNo].GetBytesString(0, REGISTER_SIZE), 16);

            index = baseRegValue + indexRegValue + displacement;

            return index;
        }

        /******************************************************************************************
         * 
         * Name:        ChangeDisplayMemoryContents  
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver    
         *              
         * Input:       The memoryAddress is an unsigned integer.
         * Return:      N/A
         * Description: This method will update a row in the memory listing on the main form to the 
         *              contents of the memory in the Simulator at the specified memory address.
         *              
         *****************************************************************************************/
        protected void ChangeDisplayMemoryContents(uint memoryAddress, uint length)
        {
            int numSpaces;
            string address;
            string charRepresentation = "";
            string memoryContents = "";
            uint endRow;
            uint index = 0;
            uint memorySize = mainMemory.GetMemorySize();
            uint relativeAddress;
            uint startRow;
            
            startRow = memoryAddress / MEMORY_COLUMNS;
            endRow = (memoryAddress + length) / MEMORY_COLUMNS;

            do
            {
                display.RemoveMemoryEntry((int)startRow);
                relativeAddress = startRow * MEMORY_COLUMNS;

                /* Format the address. */
                address = relativeAddress.ToString("X").PadLeft(6, '0');
                index = relativeAddress + MEMORY_COLUMNS;

                /* Format memory contents and EBCDIC character representation. */
                if (index < memorySize)
                {
                    memoryContents = mainMemory.GetBytesString(relativeAddress, index - 1);
                    charRepresentation = mainMemory.GetEBCDIC(relativeAddress, index - 1);
                }

                else
                {
                    memoryContents = mainMemory.GetBytesString(relativeAddress, memorySize - 1);
                    charRepresentation = mainMemory.GetEBCDIC(relativeAddress, memorySize - 1);
                }

                memoryContents = memoryContents.PadRight(Convert.ToInt32(MEMORY_COLUMNS * 2), ' ');
                charRepresentation = 
                    charRepresentation.PadRight(Convert.ToInt32(MEMORY_COLUMNS), ' ');

                /* Add a space between each byte. */
                numSpaces = Convert.ToInt32((MEMORY_COLUMNS * 2) + (MEMORY_COLUMNS - 1));

                for (int i = 2; i < numSpaces; i += 3)
                    memoryContents = memoryContents.Insert(i, " ");

                /* Insert the new row of memory into the main form. */
                display.InsertMemoryEntry(Convert.ToInt32(startRow), address,
                    memoryContents, charRepresentation);
                startRow++;

            } while (startRow <= endRow);
        }

        /******************************************************************************************
         * 
         * Name:        DisplayPSWContents  
         * 
         * Author(s):   Michael Beaver    
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will update the PSW listing on the main form to the contents
         *              of the PSW in the Simulator.
         *              
         *****************************************************************************************/
        protected void ChangeDisplayPSWContents()
        {
            string mask = "XXXXXXXX ";
            uint cc;
            
            cc = progStatWord.GetCondCodeInt();

            switch (cc)
            {
                case 0:
                    mask += "00";
                    break;

                case 1:
                    mask += "01";
                    break;

                case 2:
                    mask += "10";
                    break;

                case 3:
                    mask += "11";
                    break;
            }

            mask += "X";
            mask += progStatWord.GetBytesString(0, REGISTER_SIZE - 1);

            display.SetPSW(mask);
        }

        /******************************************************************************************
         * 
         * Name:        ChangeDisplayPSWContents
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer, operandRegister is an unsigned
         *              integer, and operandAddress is an unsigned integer.
         * Return:      N/A
         * Description: This method will update the PSW listing on the main form to the contents
         *              of the PSW in the Simulator.
         *              
         *****************************************************************************************/
        protected void ChangeDisplayPSWContents(uint locationCounter, uint displacement)
        {
            string formatLocationCounter;
            string mask = "XXXXXXXX ";
            uint cc;

            cc = progStatWord.GetCondCodeInt();

            switch (cc)
            {
                case 0:
                    mask += "00";
                    break;

                case 1:
                    mask += "01";
                    break;

                case 2:
                    mask += "10";
                    break;

                case 3:
                    mask += "11";
                    break;
            }

            mask += "X";
            
            /* Point to the next instruction. The current instruction is HALFWORD in length. */
            if (displacement == HALFWORD)
                locationCounter += HALFWORD;

            /* Point to the next instruction. The current instruction is FULLWORD in length. */
            else if (displacement == FULLWORD)
                locationCounter += FULLWORD;

            /* Point to the next instruction. The current instruction is SS_SIZE in length. */
            else if (displacement == SS_SIZE)
                locationCounter += SS_SIZE;

            progStatWord.SetBytes(0, locationCounter);
            formatLocationCounter = locationCounter.ToString("X").PadLeft(6, '0');
            mask += formatLocationCounter;

            display.SetPSW(mask);
        }

        /******************************************************************************************
         * 
         * Name:        ChangeDisplayRegisterContents  
         * 
         * Author(s):   Michael Beaver    
         *              
         * Input:       The register is an unsigned integer.
         * Return:      N/A
         * Description: This method will update the specified main form register's contents from
         *              the contents of the specified register in the Simulator.
         *              
         *****************************************************************************************/
        protected void ChangeDisplayRegisterContents(uint register)
        {
            string registerValue = "";

            /* Attempt to retrieve the register's contents. */
            try
            {
                registerValue = registers[register].GetBytesString(0, REGISTER_SIZE);
            }
            
            catch (IndexOutOfRangeException)
            {
                /* Invalid register error. */
                return;
            }

            /* Update the appropriate register. */
            switch (register)
            {
                case 0:
                    display.SetRegister0(registerValue);
                    break;

                case 1:
                    display.SetRegister1(registerValue);
                    break;

                case 2:
                    display.SetRegister2(registerValue);
                    break;

                case 3:
                    display.SetRegister3(registerValue);
                    break;

                case 4:
                    display.SetRegister4(registerValue);
                    break;

                case 5:
                    display.SetRegister5(registerValue);
                    break;

                case 6:
                    display.SetRegister6(registerValue);
                    break;

                case 7:
                    display.SetRegister7(registerValue);
                    break;

                case 8:
                    display.SetRegister8(registerValue);
                    break;

                case 9:
                    display.SetRegister9(registerValue);
                    break;

                case 10:
                    display.SetRegister10(registerValue);
                    break;

                case 11:
                    display.SetRegister11(registerValue);
                    break;

                case 12:
                    display.SetRegister12(registerValue);
                    break;

                case 13:
                    display.SetRegister13(registerValue);
                    break;

                case 14:
                    display.SetRegister14(registerValue);
                    break;

                case 15:
                    display.SetRegister15(registerValue);
                    break;

                default:
                    /* Error. */
                    break;
            };
        }

        /******************************************************************************************
         * 
         * Name:        ProcessTypeRR
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer, operandRegister1 is an unsigned
         *              integer, and operandRegister2 is an unsigned integer.
         * Return:      N/A
         * Description: This method separates the registers from the register byte of an RR 
         *              instruction so that the calling method is accessing the appropriate 
         *              locations.
         *              
         *****************************************************************************************/
        protected void ProcessTypeRR(uint locationCounter, out uint operandRegister1, 
            out uint operandRegister2)
        {
            string byteParse;

            byteParse = mainMemory.GetBytesString(locationCounter + 1, locationCounter + 1);
            operandRegister1 = Convert.ToUInt32(byteParse[0].ToString(), 16);
            operandRegister2 = Convert.ToUInt32(byteParse[1].ToString(), 16);
        }

        /******************************************************************************************
         * 
         * Name:        ProcessTypeRS
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer, the operandRegister1 is an
         *              unsigned integer, the operandRegister2 is an unsigned integer, and
         *              the operand Address is an unsigned integer.
         * Return:      N/A
         * Description: This method maps the contents of the registers designated by the register 
         *              byte of an RS instruction to its contents by using an outbound parameter.
         *              The method then determines the memory location in mainMemory that is 
         *              being referenced by determining where the address provided in the
         *              instruction is referencing by calling the DetermineAddressDB method.
         *              
         *****************************************************************************************/
        protected void ProcessTypeRS(uint locationCounter, out uint operandRegister1, 
            out uint operandRegister2, out uint operandAddress)
        {
            string byteParse;

            byteParse = mainMemory.GetBytesString(locationCounter + 1, locationCounter + 1);
            operandRegister1 = Convert.ToUInt32(byteParse[0].ToString(), 16);
            operandRegister2 = Convert.ToUInt32(byteParse[1].ToString(), 16);
            operandAddress = DetermineAddressDB(locationCounter + 2);
        }

        /******************************************************************************************
         * 
         * Name:       ProcessTypeRX 
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer, operandRegister is an unsigned
         *              integer, and operandAddress is an unsigned integer.
         * Return:      N/A
         * Description: This method helps to decide what actual memory location is being accessed
         *              by calling the DetermineAddress methods and by separating the register
         *              from the byte containing a portion of the D(X,B) address.
         *              
         *****************************************************************************************/
        protected void ProcessTypeRX(uint locationCounter, out uint operandRegister, 
            out uint operandAddress)
        {
            string byteParse;

            byteParse = mainMemory.GetBytesString(locationCounter + 1, locationCounter + 1);
            operandAddress = DetermineAddressDXB(locationCounter + 1);
            operandRegister = Convert.ToUInt32(byteParse[0].ToString(), 16);
        }

        /******************************************************************************************
         * 
         * Name:        ProcessTypeSI
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer and the operandAddress is an
         *              unsigned integer.
         * Return:      N/A
         * Description: This method determines the memory location being referenced in mainMemory
         *              by using the DetermineAddressDB method and uses an outbound parameter
         *              to store the new address.
         *              
         *****************************************************************************************/
        protected void ProcessTypeSI(uint locationCounter, out uint operandAddress)
        {
            operandAddress = DetermineAddressDB(locationCounter + 2);
        }

        /******************************************************************************************
         * 
         * Name:        ProcessTypeSS
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer, the length1 is an unsigned
         *              integer, the length2 is an unsigned integer, the operandAddress1 is an
         *              unsigned integer, and the operandAddress2 is an unsigned integer.
         * Return:      N/A
         * Description: This method determines the address locations in mainMemory that are to be
         *              accessed by using the DetermineAddressDB functions and separates the two
         *              lengths from the length byte of an SS instruction.
         *              
         *****************************************************************************************/
        protected void ProcessTypeSS(uint locationCounter, out uint length1, out uint length2,
            out uint operandAddress1, out uint operandAddress2)
        {
            string byteParse;

            byteParse = mainMemory.GetBytesString(locationCounter + 1, locationCounter + 1);
            length1 = Convert.ToUInt32(byteParse[0].ToString(), 16) + 1;
            length2 = Convert.ToUInt32(byteParse[1].ToString(), 16) + 1;
            operandAddress1 = DetermineAddressDB(locationCounter + 2);
            operandAddress2 = DetermineAddressDB(locationCounter + 4);
        }

        /******************************************************************************************
         * 
         * Name:        ProcessTypeSSL
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer, the length is an unsigned 
         *              integer, the operandAddress1 is an unsigned integer, and the
         *              operandAddress2 is an unsigned integer.
         * Return:      N/A
         * Description: This method determines the address locations in mainMemory that are to be 
         *              accessed by using the DetermineAddressDB functions and stores the length
         *              from the length byte of the SS instruction. The SSL instruction refers to 
         *              an SS instruction that uses only one length rather than separating the 
         *              length byte into two separate lengths.
         *              
         *****************************************************************************************/
        protected void ProcessTypeSSL(uint locationCounter, out uint length, 
            out uint operandAddress1, out uint operandAddress2)
        {
            length = Convert.ToUInt32(mainMemory.GetByteInt(locationCounter + 1) + 1);
            operandAddress1 = DetermineAddressDB(locationCounter + 2);
            operandAddress2 = DetermineAddressDB(locationCounter + 4);
        }

        /******************************************************************************************
         * 
         * Name:        ProcessTypeX
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer and the operandAddress is an
         *              unsigned integer.
         * Return:      N/A
         * Description: This method determines the memory location being referenced in mainMemory
         *              by using the DetermineAddressDXB method and uses an outbound parameter
         *              to store the new address.
         *              
         *****************************************************************************************/
        protected void ProcessTypeX(uint locationCounter, out uint operandAddress, out uint length)
        {
            operandAddress = DetermineAddressDXB(locationCounter + 1);
            length = Convert.ToUInt32(mainMemory.GetBytesInt(locationCounter + 4, 
                locationCounter + 5));
        }


        /* Private methods. */

        /******************************************************************************************
         * 
         * Name:        PerformInstructionA
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver              
         *              
         * Input:       The locationCounter variable is an unsigned integer.
         * Return:      N/A
         * Description: This method takes in as a parameter a pointer into mainMemory that points 
         *              to the object code that contains the additional information needed to 
         *              process the intended operation. This method adds the contents at the address
         *              specified in the object code with the contents of register specified in the
         *              object code and stores it within the register. Upon completion, it updates
         *              the condition code.
         *              
         *****************************************************************************************/
        private void PerformInstructionA(uint locationCounter)
        {
            int addressValue;
            int registerValue;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            registerValue = registers[operandRegister].GetBytesInt(0, REGISTER_SIZE);

            /* Check for fullword boundary alignment. */
            if (operandAddress % FULLWORD == 0)
            {
                addressValue = mainMemory.GetBytesInt(operandAddress, operandAddress + 3);

                try
                {
                    registerValue = checked(registerValue + addressValue);
                }

                catch (OverflowException)
                {
                    progStatWord.SetCondCode(3);
                    ChangeDisplayPSWContents();
                    errorHandling.throwException(ExceptionCodes.FP_OVERFLOW_EXCEPTION);
                }

                /* Set the condition code as appropriate. */
                if (registerValue == 0)
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else if (registerValue < 0)
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                else if (registerValue > 0)
                {
                    progStatWord.SetCondCode(2);
                    ChangeDisplayPSWContents();
                }

                registers[operandRegister].SetBytes(0, registerValue);
                ChangeDisplayRegisterContents(operandRegister);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionAP
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method takes two packed numbers that are specified by the object code,
         *              unpacks the number through the use of a helper function, adds the unpacked
         *              numbers together, and then stores them into the location where the first
         *              packed number was stored. The condition code is updated based on the 
         *              contents of the sum after addition.
         *              
         *****************************************************************************************/
        private void PerformInstructionAP(uint locationCounter)
        {
            string address1Value;
            string address2Value;
            string packedSum;
            uint length1;
            uint length2;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSS(locationCounter, out length1, out length2,
                out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            address1Value = mainMemory.GetBytesString(operandAddress1,
                operandAddress1 + length1 - 1);
            address2Value = mainMemory.GetBytesString(operandAddress2,
                operandAddress2 + length2 - 1);

            /* The numbers must be valid packed. */
            if (IsPackedNumber(address1Value) && IsPackedNumber(address2Value))
            {
                packedSum = AddPackedValues(address1Value, address2Value);

                /* Avoid overflow. */
                if (packedSum.Length <= address1Value.Length && packedSum != null)
                {
                    packedSum = packedSum.PadLeft(address1Value.Length, '0');
                    mainMemory.SetBytes(operandAddress1, packedSum);
                    ChangeDisplayMemoryContents(operandAddress1, length1);

                    /* Negative result. */
                    if (packedSum[packedSum.Length - 1] == 'B' ||
                        packedSum[packedSum.Length - 1] == 'D')
                    {
                        progStatWord.SetCondCode(1);
                        ChangeDisplayPSWContents();
                    }

                    /* Positive result. */
                    else if (packedSum[packedSum.Length - 1] == 'A' ||
                        packedSum[packedSum.Length - 1] == 'C')
                    {
                        progStatWord.SetCondCode(2);
                        ChangeDisplayPSWContents();
                    }

                    /* Zero result. */
                    else
                    {
                        progStatWord.SetCondCode(0);
                        ChangeDisplayPSWContents();
                    }
                }

                /* Insert Decimal Overflow Error. */
                else
                {
                    progStatWord.SetCondCode(3);
                    ChangeDisplayPSWContents();
                    errorHandling.throwException(ExceptionCodes.DEC_OVERFLOW_EXCEPTION);
                }

            }

            else
                errorHandling.throwException(ExceptionCodes.DATA_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionAR
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method adds the values stored in two registers into the first register
         *              passed through the object code. After addition is performed, the condition
         *              code is updated based on the contents of the sum.
         *              
         *****************************************************************************************/
        private void PerformInstructionAR(uint locationCounter)
        {
            int register1Value;
            int register2Value;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            register1Value = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
            register2Value = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);

            try
            {
                register1Value = checked(register1Value + register2Value);
            }

            catch (OverflowException)
            {
                progStatWord.SetCondCode(3);
                ChangeDisplayPSWContents();
                errorHandling.throwException(ExceptionCodes.FP_OVERFLOW_EXCEPTION);
            }

            /* Set the condition code as appropriate. */
            if (register1Value == 0)
            {
                progStatWord.SetCondCode(0);
                ChangeDisplayPSWContents();
            }

            else if (register1Value < 0)
            {
                progStatWord.SetCondCode(1);
                ChangeDisplayPSWContents();
            }

            else if (register1Value > 0)
            {
                progStatWord.SetCondCode(2);
                ChangeDisplayPSWContents();
            }

            registers[operandRegister1].SetBytes(0, register1Value);
            ChangeDisplayRegisterContents(operandRegister1);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionBAL
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method stores the address of the next instruction within the object
         *              code through a reference parameter and then branches to the address that is
         *              specified through the object code.
         *              
         *****************************************************************************************/
        private void PerformInstructionBAL(ref uint locationCounter)
        {
            string branchAddress;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            registers[operandRegister].SetBytes(0, (locationCounter + FULLWORD).ToString("X"));
            ChangeDisplayRegisterContents(operandRegister);

            /* Perform the branch. */
            branchAddress = operandAddress.ToString("X").PadLeft(6, '0');
            locationCounter = Convert.ToUInt32(branchAddress, 16);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionBALR
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method stores into operandRegister1 the next address in the object code
         *              and braches to the location in the object code based on the contents of 
         *              operandRegister2.
         *              
         *****************************************************************************************/
        private void PerformInstructionBALR(ref uint locationCounter)
        {
            string branchAddress;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            registers[operandRegister1].SetBytes(0, (locationCounter + HALFWORD).ToString("X"));
            ChangeDisplayRegisterContents(operandRegister1);

            /* Branch only if the second register is not zero. */
            if (operandRegister2 != 0)
            {
                branchAddress = registers[operandRegister2].GetBytesString(1, REGISTER_SIZE);
                locationCounter = Convert.ToUInt32(branchAddress, 16);
            }

            else
                locationCounter += HALFWORD;
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionBC
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *                    
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method uses the condition code and a mask that is specified in the 
         *              object code to determine if the locationCounter should move to the next 
         *              instruction in the object code or if it should be placed at the address
         *              specified in the instruction. If the mask matches the condition code, the 
         *              location counter is placed at the address specified, otherwise, it moves to 
         *              the next instruction in the object code.
         *              
         *****************************************************************************************/
        private void PerformInstructionBC(ref uint locationCounter)
        {
            string binaryMask;
            uint operandAddress;
            uint operandMask;

            ProcessTypeRX(locationCounter, out operandMask, out operandAddress);

            binaryMask = Convert.ToString(operandMask, 2).PadLeft(4, '0');

            /* Check the binary mask against the condition code. */
            if (progStatWord.GetCondCodeInt() == 0)
            {
                if (binaryMask[0] == '1')
                    locationCounter = operandAddress;

                else
                    locationCounter += FULLWORD;
            }

            else if (progStatWord.GetCondCodeInt() == 1)
            {
                if (binaryMask[1] == '1')
                    locationCounter = operandAddress;

                else
                    locationCounter += FULLWORD;
            }

            else if (progStatWord.GetCondCodeInt() == 2)
            {
                if (binaryMask[2] == '1')
                    locationCounter = operandAddress;

                else
                    locationCounter += FULLWORD;
            }

            else
            {
                if (binaryMask[3] == '1')
                    locationCounter = operandAddress;

                else
                    locationCounter += FULLWORD;
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionBCR
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method uses the condition code and a mask that is specified in the 
         *              object code to determine if the locationCounter should move to the next 
         *              instruction in the object code or if it should be placed at the address
         *              specified in the instruction. If the mask matches the condition code, the 
         *              location counter is placed at the address stored in the register, otherwise, 
         *              it moves to the next instruction in the object code.
         *              
         *****************************************************************************************/
        private void PerformInstructionBCR(ref uint locationCounter)
        {
            string binaryMask;
            uint operandMask;
            uint operandRegister;

            ProcessTypeRR(locationCounter, out operandMask, out operandRegister);

            binaryMask = Convert.ToString(operandMask, 2).PadLeft(4, '0');

            /* Check the binary mask against the condition code. */
            if (progStatWord.GetCondCodeInt() == 0)
            {
                if (binaryMask[0] == '1')
                    locationCounter = 
                        Convert.ToUInt32(registers[operandRegister].GetBytesInt(1, REGISTER_SIZE));

                else
                    locationCounter += HALFWORD;
            }

            else if (progStatWord.GetCondCodeInt() == 1)
            {
                if (binaryMask[1] == '1')
                    locationCounter = 
                        Convert.ToUInt32(registers[operandRegister].GetBytesInt(1, REGISTER_SIZE));

                else
                    locationCounter += HALFWORD;
            }

            else if (progStatWord.GetCondCodeInt() == 2)
            {
                if (binaryMask[2] == '1')
                    locationCounter = 
                        Convert.ToUInt32(registers[operandRegister].GetBytesInt(1, REGISTER_SIZE));

                else
                    locationCounter += HALFWORD;
            }

            else
            {
                if (binaryMask[3] == '1')
                    locationCounter = 
                        Convert.ToUInt32(registers[operandRegister].GetBytesInt(1, REGISTER_SIZE));

                else
                    locationCounter += HALFWORD;
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionBCT  
         * 
         * Author(s):   Michael Beaver
         *                      
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method subtracts 1 from the register and then moves the location counter
         *              to the address specified in the instruction if the contents of the register 
         *              is 0. Otherwise, the location counter is placed at the next instruction in 
         *              the object code.
         *              
         *****************************************************************************************/
        private void PerformInstructionBCT(ref uint locationCounter)
        {
            int controlRegisterContents;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            /* Decrement the control register by 1. */
            controlRegisterContents = registers[operandRegister].GetBytesInt(0, REGISTER_SIZE) - 1;
            registers[operandRegister].SetBytes(0, controlRegisterContents);

            /* Branch if the control register is not zero. */
            if (controlRegisterContents != 0)
                locationCounter = operandAddress;

            else
                locationCounter += FULLWORD;
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionBCTR
         * 
         * Author(s):   Michael Beaver
         *                  
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method subtracts 1 from the first register and then moves the location 
         *              counter to the address stored in the second register if the contents of the 
         *              first register is 0. Otherwise, the location counter is placed at the next 
         *              instruction in the object code.
         *              
         *****************************************************************************************/
        private void PerformInstructionBCTR(ref uint locationCounter)
        {
            int controlRegisterContents;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            /* Decrement the control register by 1. */
            controlRegisterContents = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
            controlRegisterContents--;

            registers[operandRegister1].SetBytes(0, controlRegisterContents);

            /* 
             * Branch if the control register is not zero.
             * Do not branch if the second register is Register 0.
             */
            if (controlRegisterContents != 0 && operandRegister2 != 0)
                locationCounter =
                    Convert.ToUInt32(registers[operandRegister2].GetBytesInt(1, REGISTER_SIZE));

            else
                locationCounter += HALFWORD;
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionBXH
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method uses two registers where the second register must be an odd
         *              numbered register and the first register can be either an odd numbered 
         *              register or an even numbered register where both the even numbered register
         *              and the odd numbered register following it are used. The second register is
         *              added to the first register, stored at the first register, and then the sum 
         *              is compared to the second register if the second register is odd or the odd 
         *              register following the second register if it was an even register. If the 
         *              sum is greater than the register being compared, the location counter 
         *              is moved to the address provided. Otherwise, the location counter is placed
         *              at the next instruction in the object code.
         *              
         *****************************************************************************************/
        private void PerformInstructionBXH(ref uint locationCounter)
        {
            int compareValue;
            int incrementValue;
            int registerValue;
            uint operandAddress;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRS(locationCounter, out operandRegister1, 
                out operandRegister2, out operandAddress);

            /* The second register is even. */
            if (operandRegister2 % 2 == 0)
            {
                incrementValue = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);
                compareValue = registers[operandRegister2 + 1].GetBytesInt(0, REGISTER_SIZE);
                registerValue = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
                registerValue += incrementValue;
                registers[operandRegister1].SetBytes(0, registerValue);
                ChangeDisplayRegisterContents(operandRegister1);

                if (registerValue > compareValue)
                    locationCounter = operandAddress;

                else
                    locationCounter += FULLWORD;
            }

            /* The second register is odd. */
            else
            {
                compareValue = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);
                registerValue = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
                registerValue += compareValue;

                if (registerValue > compareValue)
                    locationCounter = operandAddress;

                else
                    locationCounter += FULLWORD;
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionBXLE
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method uses two registers where the second register must be an odd
         *              numbered register and the first register can be either an odd numbered 
         *              register or an even numbered register where both the even numbered register
         *              and the odd numbered register following it are used. The second register is
         *              added to the first register, stored at the first register, and then the sum 
         *              is compared to the second register if the second register is odd or the odd 
         *              register following the second register if it was an even register. If the 
         *              sum is less than or equal to the register being compared, the location counter 
         *              is moved to the address provided. Otherwise, the location counter is placed
         *              at the next instruction in the object code.
         *              
         *****************************************************************************************/
        private void PerformInstructionBXLE(ref uint locationCounter)
        {
            int compareValue;
            int incrementValue;
            int registerValue;
            uint operandAddress;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRS(locationCounter, out operandRegister1,
                out operandRegister2, out operandAddress);

            /* The second register is even. */
            if (operandRegister2 % 2 == 0)
            {
                incrementValue = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);
                compareValue = registers[operandRegister2 + 1].GetBytesInt(0, REGISTER_SIZE);
                registerValue = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
                registerValue += incrementValue;
                registers[operandRegister1].SetBytes(0, registerValue);
                ChangeDisplayRegisterContents(operandRegister1);

                if (registerValue <= compareValue)
                    locationCounter = operandAddress;

                else
                    locationCounter += FULLWORD;
            }

            /* The second register is odd. */
            else
            {
                compareValue = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);
                registerValue = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
                registerValue += compareValue;

                if (registerValue <= compareValue)
                    locationCounter = operandAddress;

                else
                    locationCounter += FULLWORD;
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionC
         * 
         * Author(s):   Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method compares the contents of the register to the contents of 
         *              mainMemory reference by the address. The condition code is changed based
         *              upon this comparison.
         *              
         *****************************************************************************************/
        private void PerformInstructionC(uint locationCounter)
        {
            int memoryContents;
            int registerContents;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            registerContents = registers[operandRegister].GetBytesInt(0, REGISTER_SIZE);

            /* Fullword boundary alignment check. */
            if (operandAddress % FULLWORD == 0)
            {
                memoryContents = mainMemory.GetBytesInt(operandAddress, 
                    operandAddress + FULLWORD - 1);

                /* Perform the comparisons and update the CC accordingly. */
                if (registerContents == memoryContents)
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else if (registerContents < memoryContents)
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                else
                {
                    progStatWord.SetCondCode(2);
                    ChangeDisplayPSWContents();
                }
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionCLC
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method compares to locations in mainMemory referenced by the two 
         *              addresses provided by the instruction. The condition code is changed to 
         *              based upon the comparison.
         *              
         *****************************************************************************************/
        private void PerformInstructionCLC(uint locationCounter)
        {
            bool flag = true;
            int address1Value;
            int address2Value;
            uint length;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSSL(locationCounter, out length, out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            /* Traverse the data strings. */
            for (uint i = 0; i < length; i++)
            {
                address1Value = Convert.ToInt32(mainMemory.GetByteHex(operandAddress1 + i), 16);
                address2Value = Convert.ToInt32(mainMemory.GetByteHex(operandAddress2 + i), 16);

                /* Update the condition code accordingly. */
                if (address1Value < address2Value)
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                    flag = false;
                    break;
                }

                else if (address1Value > address2Value)
                {
                    progStatWord.SetCondCode(2);
                    ChangeDisplayPSWContents();
                    flag = false;
                    break;
                }
            }

            /* Equality. */
            if (flag)
            {
                progStatWord.SetCondCode(0);
                ChangeDisplayPSWContents();
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionCLI
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method compares a byte in mainMemory referenced by the address with a
         *              byte in the instruction parameters. The condition code is changed based upon
         *              this comparison.
         *              
         *****************************************************************************************/
        private void PerformInstructionCLI(uint locationCounter)
        {
            int addressValue;
            int data;
            uint operandAddress;

            ProcessTypeSI(locationCounter, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            addressValue = Convert.ToInt32(mainMemory.GetByteHex(operandAddress), 16);
            data = Convert.ToInt32(mainMemory.GetByteHex(locationCounter + 1), 16);

            /* Update the condition code accordingly. */
            if (addressValue < data)
            {
                progStatWord.SetCondCode(1);
                ChangeDisplayPSWContents();
            }

            else if (addressValue > data)
            {
                progStatWord.SetCondCode(2);
                ChangeDisplayPSWContents();
            }

            else
            {
                progStatWord.SetCondCode(0);
                ChangeDisplayPSWContents();
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionCP
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver      
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method compares to packed numbers in mainMemory reference by the 
         *              addresses provided in the instruction parameters. The condition code is
         *              changed based upon this comparison.
         *              
         *****************************************************************************************/
        private void PerformInstructionCP(uint locationCounter)
        {
            bool flag = true;
            char zoneChar1;
            char zoneChar2;
            string address1Value;
            string address2Value;
            int compareVal1;
            int compareVal2;
            int value1Length;
            int value2Length;
            uint operandAddress1;
            uint operandAddress2;
            uint operandLength1;
            uint operandLength2;

            ProcessTypeSS(locationCounter, out operandLength1, out operandLength2,
                out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            address1Value = mainMemory.GetBytesString(operandAddress1,
                operandAddress1 + operandLength1 - 1);
            address2Value = mainMemory.GetBytesString(operandAddress2,
                operandAddress2 + operandLength2 - 1);

            /* Check for valid packed. */
            if (IsPackedNumber(address1Value) && IsPackedNumber(address2Value))
            {
                value1Length = address1Value.Length;
                value2Length = address2Value.Length;

                /* Pad zeros to make traversal easier. */
                if (value1Length < value2Length)
                {
                    address1Value = address1Value.PadLeft(value2Length, '0');
                    value1Length = value2Length;
                }

                else if (value1Length > value2Length)
                {
                    address2Value = address2Value.PadLeft(value1Length, '0');
                    value2Length = value1Length;
                }

                zoneChar1 = address1Value[value1Length - 1];
                zoneChar2 = address2Value[value2Length - 1];

                /* The first packed number is positive. The second packed number is negative. */
                if ((zoneChar1 == 'A' || zoneChar1 == 'C' || zoneChar1 == 'E' ||
                    zoneChar1 == 'F') && (zoneChar2 == 'B' || zoneChar2 == 'D'))
                {
                    progStatWord.SetCondCode(2);
                    ChangeDisplayPSWContents();
                }

                /* The first packed number is negative, and the second packed number is positive. */
                else if ((zoneChar1 == 'B' || zoneChar1 == 'D') && (zoneChar2 == 'A'
                    || zoneChar2 == 'C' || zoneChar2 == 'E' || zoneChar2 == 'F'))
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                /* Both packed numbers are positive. */
                else if ((zoneChar1 == 'A' || zoneChar1 == 'C' ||
                    zoneChar1 == 'E' || zoneChar1 == 'F') &&
                    (zoneChar2 == 'A' || zoneChar2 == 'C' ||
                    zoneChar2 == 'E' || zoneChar2 == 'F'))
                {
                    /* Traverse the packed numbers. */
                    for (int i = 0; i < value1Length - 1; i++)
                    {
                        compareVal1 = Convert.ToInt32(address1Value[i].ToString(), 16);
                        compareVal2 = Convert.ToInt32(address2Value[i].ToString(), 16);

                        /* Update the condition code accordingly. */
                        if (compareVal1 < compareVal2)
                        {
                            progStatWord.SetCondCode(1);
                            ChangeDisplayPSWContents();
                            flag = false;
                            break;
                        }

                        else if (compareVal1 > compareVal2)
                        {
                            progStatWord.SetCondCode(2);
                            ChangeDisplayPSWContents();
                            flag = false;
                            break;
                        }
                    }

                    /* Equality. */
                    if (flag)
                    {
                        progStatWord.SetCondCode(0);
                        ChangeDisplayPSWContents();
                    }
                }

                /* Both packed numbers are negative. */
                else if ((zoneChar1 == 'B' || zoneChar1 == 'D') &&
                    (zoneChar2 == 'B' || zoneChar2 == 'D'))
                {
                    /* Traverse the packed numbers. */
                    for (int i = 0; i < value1Length - 1; i++)
                    {
                        compareVal1 = Convert.ToInt32(address1Value[i].ToString(), 16);
                        compareVal2 = Convert.ToInt32(address2Value[i].ToString(), 16);

                        /* Update the condition code accordingly. */
                        if (compareVal1 > compareVal2)
                        {
                            progStatWord.SetCondCode(1);
                            ChangeDisplayPSWContents();
                            flag = false;
                            break;
                        }

                        else if (compareVal1 < compareVal2)
                        {
                            progStatWord.SetCondCode(2);
                            ChangeDisplayPSWContents();
                            flag = false;
                            break;
                        }
                    }

                    /* Equality. */
                    if (flag)
                    {
                        progStatWord.SetCondCode(0);
                        ChangeDisplayPSWContents();
                    }
                }

                /* Equality. */
                else
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }
            }

            else
                errorHandling.throwException(ExceptionCodes.DATA_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionCR  
         * 
         * Author(s):   Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method compares the contents of two registers provided by the instruction
         *              parameters. The condition code is updated based upon this comparison.
         *              
         *****************************************************************************************/
        private void PerformInstructionCR(uint locationCounter)
        {
            int registerContents1;
            int registerContents2;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            registerContents1 = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
            registerContents2 = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);

            /* The contents of R1 are equal to the contents of R2, so the CC is zero. */
            if (registerContents1 == registerContents2)
            {
                progStatWord.SetCondCode(0);
                ChangeDisplayPSWContents();
            }

            /* The contents of R1 are less than the contents of R2, so the CC is one. */
            else if (registerContents1 < registerContents2)
            {
                progStatWord.SetCondCode(1);
                ChangeDisplayPSWContents();
            }

            /* The contents of R1 are greater than the contents of R2, so the CC is one. */
            else
            {
                progStatWord.SetCondCode(2);
                ChangeDisplayPSWContents();
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionD
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method divides a number stored within an even numbered register and 
         *              divides that number with the memory contents stored in mainMemory at the 
         *              location provided by the address. The remainder from the division is stored
         *              in the even numbered register and the actual quotient is stored in the odd
         *              numbered register that follows the even numbered register that was passed 
         *              through via the instruction.
         *              
         *****************************************************************************************/
        private void PerformInstructionD(uint locationCounter)
        {
            int addressValue;
            int registerValue;
            int remainder = 0;
            int quotient = 0;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            /* The second register must be even, and the memory  must be on a fullword boundary. */
            if (operandRegister % 2 == 0 && operandAddress % FULLWORD == 0)
            {
                /* Load the values from the register. */
                registerValue = registers[operandRegister].GetBytesInt(0, REGISTER_SIZE);
                addressValue = mainMemory.GetBytesInt(operandAddress, 
                    operandAddress + FULLWORD - 1);

                /* Perform arithmetic upon values. */
                if (addressValue != 0)
                {
                    try
                    {
                        if (registerValue != 0)
                            throw new Exception("Fixed-point divide exception.");

                        quotient = checked(registerValue / addressValue);
                        remainder = checked(registerValue % addressValue);
                    }

                    catch
                    {
                        errorHandling.throwException(ExceptionCodes.FP_DIVIDE_EXCEPTION);
                    }

                    /* Update the sign of the remainder, if necessary. */
                    if ((quotient < 0 && remainder > 0) || (quotient > 0 && remainder < 0))
                        remainder *= -1;

                    /* Store values into appropriate register. */
                    registers[operandRegister + 1].SetBytes(0, quotient);
                    registers[operandRegister].SetBytes(0, remainder);
                    ChangeDisplayRegisterContents(operandRegister + 1);
                    ChangeDisplayRegisterContents(operandRegister);
                }

                /* Division by zero. */
                else
                    errorHandling.throwException(ExceptionCodes.FP_DIVIDE_EXCEPTION);
            }

            else
                errorHandling.throwException(ExceptionCodes.DATA_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionDP
         * 
         * Author(s):   Michael Beaver
         *               
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method will unpack two packed numbers stored in mainMemory at the 
         *              addresses passed as parameters from the object code and divide them. After
         *              division, the quotient is placed at the first address that was provided as 
         *              a parameter.
         *              
         *****************************************************************************************/
        private void PerformInstructionDP(uint locationCounter)
        {
            char zoneChar1;
            char zoneChar2;
            int operand1IntValue;
            int operand2IntValue;
            int quotient = 0;
            int remainder = 0;
            string operand1Value;
            string operand2Value;
            string quotientString;
            string remainderString;
            uint length1;
            uint length2;
            uint lengthDiff;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSS(locationCounter, out length1, out length2,
                out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            operand1Value = mainMemory.GetBytesString(operandAddress1, 
                operandAddress1 + length1 - 1);
            operand2Value = mainMemory.GetBytesString(operandAddress2, 
                operandAddress2 + length2 - 1);

            /* Numbers must be valid packed. */
            if (IsPackedNumber(operand1Value) && IsPackedNumber(operand2Value))
            {
                /* Lengths must be valid. */
                if (length1 > length2 && length2 <= MAX_DP_LENGTH2)
                {
                    operand1IntValue = 
                        Convert.ToInt32(operand1Value.Substring(0, ((int)length1 * 2) - 1), 10);
                    operand2IntValue = 
                        Convert.ToInt32(operand2Value.Substring(0, ((int)length2 * 2) - 1), 10);

                    /* Make the value(s) negative as appropriate. */
                    zoneChar1 = operand1Value[((int)length1 * 2) - 1];
                    zoneChar2 = operand2Value[((int)length2 * 2) - 1];

                    if (zoneChar1 == 'B' || zoneChar1 == 'D')
                        operand1IntValue *= -1;

                    if (zoneChar2 == 'B' || zoneChar2 == 'D')
                        operand2IntValue *= -1;

                    /* Avoid division by zero. */
                    if (operand2IntValue != 0)
                    {
                        try
                        {
                            quotient = checked(operand1IntValue / operand2IntValue);
                            remainder = checked(operand1IntValue % operand2IntValue);
                        }

                        catch
                        {
                            errorHandling.throwException(ExceptionCodes.DEC_DIVIDE_EXCEPTION);
                        }

                        /* The remainder has the same sign as the dividend. */
                        if ((operand1IntValue < 0 && remainder > 0) ||
                            (operand1IntValue > 0 && remainder < 0))
                            remainder *= -1;

                        remainderString = remainder.ToString().PadLeft((int)length2 * 2 - 1, '0');
                        quotientString = quotient.ToString();

                        /* Append the appropriate zone character to the quotient. */
                        if (quotient < 0)
                        {
                            quotientString = quotientString.Replace("-", "0");
                            quotientString += "D";
                        }

                        else if (quotient == 0)
                            quotientString += "F";

                        else
                            quotientString += "C";

                        /* Append the appropriate zone character to the remainder. */
                        if (remainder < 0)
                        {
                            remainderString = remainderString.Replace("-", "0");
                            remainderString += "D";
                        }

                        else if (remainder == 0)
                            remainderString += "F";

                        else
                            remainderString += "C";

                        lengthDiff = (length1 * 2) - (length2 * 2);

                        /* Quotient must fit within the first length1 - length2 bytes. */
                        if (quotientString.Length <= (int)lengthDiff)
                        {
                            quotientString = quotientString.PadLeft((int)lengthDiff, '0');

                            mainMemory.SetBytes(operandAddress1, quotientString);
                            mainMemory.SetBytes(operandAddress1 + length1 - length2,
                                remainderString);
                            ChangeDisplayMemoryContents(operandAddress1, length1);
                        }

                        else
                            errorHandling.throwException(ExceptionCodes.DEC_DIVIDE_EXCEPTION);
                    }

                    else
                        errorHandling.throwException(ExceptionCodes.DEC_DIVIDE_EXCEPTION);
                }

                else
                    errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionDR
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method will divide the contents of two registers where the first 
         *              register passed as a parameter must be an even numbered register. The 
         *              contents of the first register is divided by the second register and the 
         *              remainder is stored in the even numbered first register and the quotient is
         *              stored in the odd numbered register that immediately follows the even 
         *              numbered first register.
         *              
         *****************************************************************************************/
        private void PerformInstructionDR(uint locationCounter)
        {
            int quotient = 0;
            int register1Value;
            int register2Value;
            int remainder = 0;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            /* First register is even. */
            if (operandRegister1 % 2 == 0)
            {
                /* Load the values from the register. */
                register1Value = registers[operandRegister1 + 1].GetBytesInt(0, REGISTER_SIZE);
                register2Value = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);

                /* Perform arithmetic upon values. */
                if (register2Value != 0)
                {
                    try
                    {
                        quotient = checked(register1Value / register2Value);
                        remainder = checked(register1Value % register2Value);
                    }

                    catch
                    {
                        errorHandling.throwException(ExceptionCodes.FP_DIVIDE_EXCEPTION);
                    }
                               
                    /* Update the sign of the remainder, if necessary. */
                    if ((quotient < 0 && remainder > 0) || (quotient > 0 && remainder < 0))
                        remainder *= -1;

                    /* Store values into appropriate register. */
                    registers[operandRegister1 + 1].SetBytes(0, quotient);
                    registers[operandRegister1].SetBytes(0, remainder);
                    ChangeDisplayRegisterContents(operandRegister1 + 1);
                    ChangeDisplayRegisterContents(operandRegister1);
                }

                else
                    errorHandling.throwException(ExceptionCodes.FP_DIVIDE_EXCEPTION);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionED
         * 
         * Author(s):   Michael Beaver        
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method will read a packed number into a specified pattern in memory
         *              made up of a fill byte and digit selectors (X'20' and X'21'). The number of 
         *              digit selectors is used to determine the length of the packed number in
         *              memory to select. The packed number is parsed into the pattern stream and
         *              then saved back into memory. The condition code is set appropriately.
         *              
         *****************************************************************************************/
        private void PerformInstructionED(uint locationCounter)
        {
            bool significanceIndicator = false;
            char tempDigit;
            int patternOffset;
            string fillByte;
            string operand1Value;
            string operand2Value;
            string tempByte;
            uint length1;
            uint length2 = 0;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSSL(locationCounter, out length1, out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            operand1Value = mainMemory.GetBytesString(operandAddress1, 
                operandAddress1 + length1 - 1);

            /* Calculate the number of selector bytes. */
            for (int i = 0; i < length1 * 2; i += 2)
            {
                tempByte = operand1Value[i].ToString() + operand1Value[i + 1].ToString();

                if (tempByte == DIGIT_SELECTOR || tempByte == DIGIT_SELECTOR_SIGN_START)
                    length2++;
            }

            /* The length of the second operand is the number of selector bytes +1 divided by 2. */
            length2 = (length2 + 1) / 2;
            operand2Value = mainMemory.GetBytesString(operandAddress2, 
                operandAddress2 + length2 - 1);

            /* The second operand must be a valid packed number. */
            if (IsPackedNumber(operand2Value))
            {
                fillByte = operand1Value[0].ToString() + operand1Value[1].ToString();
                patternOffset = 2;

                for (int i = 0; i < (length2 * 2) - 1; i++)
                {
                    tempDigit = operand2Value[i];
                    tempByte = operand1Value[patternOffset].ToString() +
                        operand1Value[patternOffset + 1].ToString();

                    /* Skip non-digit selector bytes. */
                    if (tempByte != DIGIT_SELECTOR && 
                        tempByte != DIGIT_SELECTOR_SIGN_START &&
                        significanceIndicator == true)
                        i--;

                    /* Replace a digit selector with the fill byte, if the sign. ind. is off. */
                    else if (tempByte != DIGIT_SELECTOR && 
                        tempByte != DIGIT_SELECTOR_SIGN_START
                        && significanceIndicator == false)
                    {
                        operand1Value = operand1Value.Remove(patternOffset, 2);
                        operand1Value = operand1Value.Insert(patternOffset, fillByte);
                        i--;
                    }

                    /* Replace zeros with the fill byte. */
                    else if (tempDigit == '0' && significanceIndicator == false)
                    {
                        operand1Value = operand1Value.Remove(patternOffset, 2);
                        operand1Value = operand1Value.Insert(patternOffset, fillByte);
                    }

                    /* Treat all digits as significant, including zeros. */
                    else
                    {
                        operand1Value = operand1Value.Remove(patternOffset, 2);
                        operand1Value = operand1Value.Insert(patternOffset,
                            ToEBCDIC(tempDigit.ToString()));
                    }

                    /* Turn on the significance indicator for significance starter byte. */
                    if (tempByte == DIGIT_SELECTOR_SIGN_START && significanceIndicator == false)
                        significanceIndicator = true;

                    /* Turn on the significance indicator for any nonzero digit. */
                    if (tempDigit != '0' && significanceIndicator == false)
                        significanceIndicator = true;

                    patternOffset += 2;
                }

                /* Positive signed or unsigned zone will turn off the significance indicator. */
                tempDigit = operand2Value[((int)length2 * 2) - 1];

                if (tempDigit == 'A' || tempDigit == 'C' || tempDigit == 'E' || tempDigit == 'F')
                    significanceIndicator = false;

                /* Replace message characters after the pattern, if necessary. */
                for (int i = patternOffset; i < length1 * 2; i += 2)
                {
                    tempByte = operand1Value[i].ToString() + operand1Value[i + 1].ToString();

                    /* Replace the message characters only if the significance indicator is on. */
                    if (tempByte != EBCDIC_BLANK && significanceIndicator == false)
                    {
                        operand1Value = operand1Value.Remove(i, 2);
                        operand1Value = operand1Value.Insert(i, fillByte);
                    }
                }

                mainMemory.SetBytes(operandAddress1, operand1Value);
                ChangeDisplayMemoryContents(operandAddress1, length1);

                tempDigit = operand2Value[operand2Value.Length - 1];

                /* Set the condition code accordingly. */
                if (Convert.ToInt32(operand2Value.Substring(0, operand2Value.Length - 1)) == 0)
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else if (tempDigit == 'B' || tempDigit == 'D')
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                else
                {
                    progStatWord.SetCondCode(2);
                    ChangeDisplayPSWContents();
                }
            }

            else
                errorHandling.throwException(ExceptionCodes.DATA_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionEDMK
         * 
         * Author(s):   Michael Beaver      
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method will read a packed number into a specified pattern in memory
         *              made up of a fill byte and digit selectors (X'20' and X'21'). The number of 
         *              digit selectors is used to determine the length of the packed number in
         *              memory to select. The packed number is parsed into the pattern stream and
         *              then saved back into memory. When an X'21' digit selector is encountered,
         *              Register 1 will be set to point to that byte. The condition code is set 
         *              appropriately.    
         *              
         *****************************************************************************************/
        private void PerformInstructionEDMK(uint locationCounter)
        {
            bool significanceIndicator = false;
            char tempDigit;
            int patternOffset;
            string fillByte;
            string operand1Value;
            string operand2Value;
            string tempByte;
            uint length1;
            uint length2 = 0;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSSL(locationCounter, out length1, out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            operand1Value = mainMemory.GetBytesString(operandAddress1, 
                operandAddress1 + length1 - 1);

            /* Calculate the number of selector bytes. */
            for (int i = 0; i < length1 * 2; i += 2)
            {
                tempByte = operand1Value[i].ToString() + operand1Value[i + 1].ToString();
                if (tempByte == DIGIT_SELECTOR || tempByte == DIGIT_SELECTOR_SIGN_START)
                    length2++;
            }

            /* The length of the second operand is the number of selector bytes +1 divided by 2. */
            length2 = (length2 + 1) / 2;
            operand2Value = mainMemory.GetBytesString(operandAddress2, 
                operandAddress2 + length2 - 1);

            /* The second operand must be a valid packed number. */
            if (IsPackedNumber(operand2Value))
            {
                fillByte = operand1Value[0].ToString() + operand1Value[1].ToString();
                patternOffset = 2;

                for (int i = 0; i < (length2 * 2) - 1; i++)
                {
                    tempDigit = operand2Value[i];
                    tempByte = operand1Value[patternOffset].ToString() +
                        operand1Value[patternOffset + 1].ToString();

                    /* Skip non-digit selector bytes. */
                    if (tempByte != DIGIT_SELECTOR && 
                        tempByte != DIGIT_SELECTOR_SIGN_START &&
                        significanceIndicator == true)
                        i--;

                    /* Replace a digit selector with the fill byte, if the sign. ind. is off. */
                    else if (tempByte != DIGIT_SELECTOR && 
                        tempByte != DIGIT_SELECTOR_SIGN_START
                        && significanceIndicator == false)
                    {
                        operand1Value = operand1Value.Remove(patternOffset, 2);
                        operand1Value = operand1Value.Insert(patternOffset, fillByte);
                        i--;
                    }

                    /* Replace zeros with the fill byte. */
                    else if (tempDigit == '0' && significanceIndicator == false)
                    {
                        operand1Value = operand1Value.Remove(patternOffset, 2);
                        operand1Value = operand1Value.Insert(patternOffset, fillByte);
                    }

                    /* Treat all digits as significant, including zeros. */
                    else
                    {
                        operand1Value = operand1Value.Remove(patternOffset, 2);
                        operand1Value = operand1Value.Insert(patternOffset,
                            ToEBCDIC(tempDigit.ToString()));
                    }

                    /* 
                     * Turn on the significance indicator for any nonzero digit. 
                     * Register 1 points to this byte. 
                     */
                    if (tempDigit != '0' && significanceIndicator == false && 
                        (tempByte == DIGIT_SELECTOR_SIGN_START || tempByte == DIGIT_SELECTOR))
                    {
                        significanceIndicator = true;
                        registers[1].SetBytes(0, (int)operandAddress1 + (patternOffset / 2));
                        ChangeDisplayRegisterContents(1);
                    }

                    /* Turn on the significance indicator for significance starter byte. */
                    if (tempByte == DIGIT_SELECTOR_SIGN_START && significanceIndicator == false)
                        significanceIndicator = true;

                    patternOffset += 2;
                }

                /* Positive signed or unsigned zone will turn off the significance indicator. */
                tempDigit = operand2Value[((int)length2 * 2) - 1];

                if (tempDigit == 'A' || tempDigit == 'C' || tempDigit == 'E' || tempDigit == 'F')
                    significanceIndicator = false;

                /* Replace message characters after the pattern, if necessary. */
                for (int i = patternOffset; i < length1 * 2; i += 2)
                {
                    tempByte = operand1Value[i].ToString() + operand1Value[i + 1].ToString();

                    /* Replace the message characters only if the significance indicator is on. */
                    if (tempByte != EBCDIC_BLANK && significanceIndicator == false)
                    {
                        operand1Value = operand1Value.Remove(i, 2);
                        operand1Value = operand1Value.Insert(i, fillByte);
                    }
                }

                mainMemory.SetBytes(operandAddress1, operand1Value);
                ChangeDisplayMemoryContents(operandAddress1, length1);

                tempDigit = operand2Value[operand2Value.Length - 1];

                /* Set the condition code accordingly. */
                if (Convert.ToInt32(operand2Value.Substring(0, operand2Value.Length - 1)) == 0)
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else if (tempDigit == 'B' || tempDigit == 'D')
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                else
                {
                    progStatWord.SetCondCode(2);
                    ChangeDisplayPSWContents();
                }
            }

            else
                errorHandling.throwException(ExceptionCodes.DATA_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionL 
         * 
         * Author(s):   Michael Beaver
         *                
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method stores the contents in mainMemory at the location provided by 
         *              the address into the register provided.
         *              
         *****************************************************************************************/
        private void PerformInstructionL(uint locationCounter)
        {
            string memoryContents;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            /* Fullword boundary alignment check. */
            if (operandAddress % FULLWORD == 0)
            {
                memoryContents = mainMemory.GetBytesString(operandAddress, 
                    operandAddress + FULLWORD - 1);
                registers[operandRegister].SetBytes(0, memoryContents);
                ChangeDisplayRegisterContents(operandRegister);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionLA
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: The address parameter, after calculation using DetermineAddressDXB is stored
         *              into the register provided.
         *              
         *****************************************************************************************/
        private void PerformInstructionLA(uint locationCounter)
        {
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            registers[operandRegister].SetBytes(0,
                operandAddress.ToString("X").PadLeft((REGISTER_SIZE + 1) * 2, '0'));
            ChangeDisplayRegisterContents(operandRegister);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionLM
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method loads from mainMemory at the location provided by the address 
         *              into the contents of the registers starting with the first register going 
         *              to the second register into mainMemory at the location provided by the 
         *              address. This method is circular, meaning that if the first register is 
         *              register 10 and the second register is register 1, this method will store 
         *              the registers 10, 11, 12, 13, 14, 15, 0, and 1 into mainMemory.
         *              
         *****************************************************************************************/
        private void PerformInstructionLM(uint locationCounter)
        {
            string registerContent;
            uint operandAddress;
            uint operandRegister1;
            uint operandRegister2;
            uint registerIndex;
            uint storageIndex = 0;

            ProcessTypeRS(locationCounter, out operandRegister1,
                out operandRegister2, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            registerIndex = operandRegister1;

            /* Traverse the registers and load their contents from memory. */
            if (operandAddress % FULLWORD == 0)
            {
                do
                {
                    registerContent = mainMemory.GetBytesString(operandAddress + storageIndex,
                        operandAddress + storageIndex + FULLWORD - 1);

                    registers[registerIndex].SetBytes(0, registerContent);
                    storageIndex += FULLWORD;
                    ChangeDisplayRegisterContents(registerIndex);

                    /* Special condition: Registers are the same. */
                    if (registerIndex == operandRegister2)
                        registerIndex = operandRegister2 + 1;

                    else if (registerIndex == MAX_REGISTER)
                        registerIndex = 0;

                    else
                        registerIndex++;

                } while (registerIndex != operandRegister2 + 1);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionLR
         * 
         * Author(s):   Michael Beaver       
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method stores the contents of the second register into the first 
         *              register.
         *              
         *****************************************************************************************/
        private void PerformInstructionLR(uint locationCounter)
        {
            int register1Value;
            int register2Value;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            register1Value = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
            register2Value = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);

            registers[operandRegister1].SetBytes(0,
                register2Value.ToString("X").PadLeft((REGISTER_SIZE + 1) * 2, '0'));
            ChangeDisplayRegisterContents(operandRegister1);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionM
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method requires the register provided to be an even numbered register.
         *              This method multiplies the contents of register with the memory stored in
         *              mainMemory provided by the address. The product is stored using both the 
         *              even numbered register provided and the odd numbered register immediately
         *              following the even numbered register to account for the size of the product.
         *              
         *****************************************************************************************/
        private void PerformInstructionM(uint locationCounter)
        {
            int addressValue;
            int registerValue;
            long product = 0;
            string productString;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            /* The second register is even, and the memory is on a fullword boundary. */
            if (operandRegister % 2 == 0 && operandAddress % FULLWORD == 0)
            {
                /* Load the values from the register. */
                registerValue = registers[operandRegister + 1].GetBytesInt(0, REGISTER_SIZE);
                addressValue = mainMemory.GetBytesInt(operandAddress, 
                    operandAddress + FULLWORD - 1);

                /* Perform arithmetic upon values. */
                try
                {
                    product = checked(registerValue * addressValue);
                }
                
                catch
                {
                    errorHandling.throwException(ExceptionCodes.FP_OVERFLOW_EXCEPTION);
                }

                if (product > 0)
                    productString = product.ToString("X").PadLeft(16, '0');

                else
                    productString = product.ToString("X").PadLeft(16, 'F');

                /* Store values into appropriate register. */
                registers[operandRegister].SetBytes(0, productString.Substring(0, 8));
                registers[operandRegister + 1].SetBytes(0, productString.Substring(8, 8));
                ChangeDisplayRegisterContents(operandRegister);
                ChangeDisplayRegisterContents(operandRegister + 1);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionMP
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method unpacks the two packed numbers provided by the instruction from
         *              mainMemory and multiplies them. The product is packed and then stored into 
         *              mainMemory at the location provided by the first address.
         *              
         *****************************************************************************************/
        private void PerformInstructionMP(uint locationCounter)
        {
            char zoneChar1;
            char zoneChar2;
            int operand1IntValue;
            int operand2IntValue;
            int product = 0;
            string operand1Value;
            string operand2Value;
            string result;
            uint length1;
            uint length2;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSS(locationCounter, out length1, out length2,
                out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            operand1Value = mainMemory.GetBytesString(operandAddress1,
                operandAddress1 + length1 - 1);
            operand2Value = mainMemory.GetBytesString(operandAddress2,
                operandAddress2 + length2 - 1);

            /* Numbers must be valid packed. */
            if (IsPackedNumber(operand1Value) && IsPackedNumber(operand2Value))
            {
                /* Lengths must be valid. */
                if (length1 > length2 && length2 <= MAX_MP_LENGTH2)
                {
                    /* The first length2 bytes of Operand1 must be zero. */
                    if (Convert.ToInt32(operand1Value.Substring(0, (int)length2 * 2), 10) == 0)
                    {
                        operand1IntValue = 
                            Convert.ToInt32(operand1Value.Substring(0, ((int)length1 * 2) - 1), 10);
                        operand2IntValue = 
                            Convert.ToInt32(operand2Value.Substring(0, ((int)length2 * 2) - 1), 10);

                        /* Make the value(s) negative as appropriate. */
                        zoneChar1 = operand1Value[((int)length1 * 2) - 1];
                        zoneChar2 = operand2Value[((int)length2 * 2) - 1];

                        if (zoneChar1 == 'B' || zoneChar1 == 'D')
                            operand1IntValue *= -1;

                        if (zoneChar2 == 'B' || zoneChar2 == 'D')
                            operand2IntValue *= -1;

                        try
                        {
                            product = checked(operand1IntValue * operand2IntValue);
                        }
                        
                        catch
                        {
                            progStatWord.SetCondCode(3);
                            ChangeDisplayPSWContents();
                            errorHandling.throwException(ExceptionCodes.DEC_OVERFLOW_EXCEPTION);
                        }

                        result = product.ToString().PadLeft((int)length1 * 2 - 1, '0');

                        /* Test for overflow. */
                        if ((product.ToString().Length) >= (length1 * 2 - 1))
                        {
                            progStatWord.SetCondCode(3);
                            ChangeDisplayPSWContents();
                            errorHandling.throwException(ExceptionCodes.DEC_OVERFLOW_EXCEPTION);
                        }

                        /* Append the appropriate zone character. */
                        if (product < 0)
                        {
                            result = result.Replace("-", "0");
                            result += "D";
                            progStatWord.SetCondCode(1);
                            ChangeDisplayPSWContents();
                        }

                        else if (product == 0)
                        {
                            result += "C";
                            progStatWord.SetCondCode(0);
                            ChangeDisplayPSWContents();
                        }

                        else
                        {
                            result += "C";
                            progStatWord.SetCondCode(2);
                            ChangeDisplayPSWContents();
                        }

                        mainMemory.SetBytes(operandAddress1, result);
                        ChangeDisplayMemoryContents(operandAddress1, length1);
                    }

                    else
                        errorHandling.throwException(ExceptionCodes.DATA_EXCEPTION);
                }

                else
                    errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
            }

            else
                errorHandling.throwException(ExceptionCodes.DATA_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionMR
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method requires the first register to be an even numbered register. 
         *              This method multiplies the content from the first register and the second 
         *              register. The product is then stored in the even numbered first register
         *              and the odd numbered immediately following the even numbered register to 
         *              account for the size of the product.
         *              
         *****************************************************************************************/
        private void PerformInstructionMR(uint locationCounter)
        {
            int register1Value;
            int register2Value;
            long product = 0;
            string productString;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            /* The first register is even. */
            if (operandRegister1 % 2 == 0)
            {
                /* Load the values from the register. */
                register1Value = registers[operandRegister1 + 1].GetBytesInt(0, REGISTER_SIZE);
                register2Value = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);

                /* Perform arithmetic upon values. */
                try
                {
                    product = checked(register1Value * register2Value);
                }
                
                catch
                {
                    errorHandling.throwException(ExceptionCodes.FP_OVERFLOW_EXCEPTION);
                }

                if (product > 0)
                    productString = product.ToString("X").PadLeft(16, '0');

                else
                    productString = product.ToString("X").PadLeft(16, 'F');

                /* Store values into appropriate register. */
                registers[operandRegister1].SetBytes(0, productString.Substring(0, 8));
                registers[operandRegister1 + 1].SetBytes(0, productString.Substring(8, 8));
                ChangeDisplayRegisterContents(operandRegister1);
                ChangeDisplayRegisterContents(operandRegister1 + 1);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionMVC
         * 
         * Author(s):   Chad Farley
         *                      
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method moves the contents of mainMemory referenced by the second address
         *              to the location in mainMemory provided by the second address.
         *              
         *****************************************************************************************/
        private void PerformInstructionMVC(uint locationCounter)
        {
            string address2Content;
            uint length;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSSL(locationCounter, out length, out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            address2Content = mainMemory.GetBytesString(operandAddress2,
                operandAddress2 + (uint)length - 1);

            /* Special case: Cascading a byte. */
            if (operandAddress1 == operandAddress2 + 1)
            {
                address2Content = address2Content.Remove(2);

                for (uint i = 0; i < length - 1; i++)
                    address2Content += address2Content.Substring(0, 2);
            }

            mainMemory.SetBytes(operandAddress1, address2Content);
            ChangeDisplayMemoryContents(operandAddress1, length);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionMVI
         * 
         * Author(s):   Chad Farley          
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method stores the data provided by the data byte of the instruction to
         *              the location in mainMemory provided by the address.
         *              
         *****************************************************************************************/
        private void PerformInstructionMVI(uint locationCounter)
        {
            string data;
            uint operandAddress;

            ProcessTypeSI(locationCounter, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            data = mainMemory.GetByteHex(locationCounter + 1);

            mainMemory.SetByte(operandAddress, data);
            ChangeDisplayMemoryContents(operandAddress, 1);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionN 
         * 
         * Author(s):   Michael Beaver 
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method performs a bitwise AND to the contents of the register and the
         *              contents of mainMemory provided by the address. The result is stored in the
         *              register.
         *              
         *****************************************************************************************/
        private void PerformInstructionN(uint locationCounter)
        {
            int memoryContents;
            int registerContents;
            int result;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            registerContents = registers[operandRegister].GetBytesInt(0, REGISTER_SIZE);

            /* Check for fullword boundary alignment. */
            if (operandAddress % FULLWORD == 0)
            {
                memoryContents = mainMemory.GetBytesInt(operandAddress, 
                    operandAddress + FULLWORD - 1);

                /* Perform logical bitwise AND and update the CC. */
                result = registerContents & memoryContents;

                if (result == 0)
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                registers[operandRegister].SetBytes(0, result);
                ChangeDisplayRegisterContents(operandRegister);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionNR
         * 
         * Author(s):   Michael Beaver
         *                         
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method performs a bitwise AND to the contents of the first register and 
         *              the contents of the second register. The result is stored in the first
         *              register.
         *              
         *****************************************************************************************/
        private void PerformInstructionNR(uint locationCounter)
        {
            int registerContents1;
            int registerContents2;
            int result;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            registerContents1 = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
            registerContents2 = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);

            /* Perform logical bitwise AND and update the CC. */
            result = registerContents1 & registerContents2;

            if (result == 0)
            {
                progStatWord.SetCondCode(0);
                ChangeDisplayPSWContents();
            }

            else
            {
                progStatWord.SetCondCode(1);
                ChangeDisplayPSWContents();
            }

            registers[operandRegister1].SetBytes(0, result);
            ChangeDisplayRegisterContents(operandRegister1);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionO
         * 
         * Author(s):   Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method performs a bitwise OR to the contents of the register and the 
         *              contents of mainMemory at the location provided by the address. The results
         *              are stored in the register.
         *              
         *****************************************************************************************/
        private void PerformInstructionO(uint locationCounter)
        {
            int memoryContents;
            int registerContents;
            int result;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            registerContents = registers[operandRegister].GetBytesInt(0, REGISTER_SIZE);

            /* Check for fullword boundary alignment. */
            if (operandAddress % FULLWORD == 0)
            {
                memoryContents = mainMemory.GetBytesInt(operandAddress, 
                    operandAddress + FULLWORD - 1);

                /* Perform logical bitwise OR and update the CC. */
                result = registerContents | memoryContents;

                if (result == 0)
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                registers[operandRegister].SetBytes(0, result);
                ChangeDisplayRegisterContents(operandRegister);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionOR
         * 
         * Author(s):   Michael Beaver      
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method performs a bitwise OR to the contents of the first register and 
         *              the contents of the second register. The result is stored in the first
         *              register.
         *              
         *****************************************************************************************/
        private void PerformInstructionOR(uint locationCounter)
        {
            int registerContents1;
            int registerContents2;
            int result;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            registerContents1 = registers[operandRegister1].GetBytesInt(0, REGISTER_SIZE);
            registerContents2 = registers[operandRegister2].GetBytesInt(0, REGISTER_SIZE);

            /* Perform logical bitwise OR and update the CC. */
            result = registerContents1 | registerContents2;

            if (result == 0)
            {
                progStatWord.SetCondCode(0);
                ChangeDisplayPSWContents();
            }

            else
            {
                progStatWord.SetCondCode(1);
                ChangeDisplayPSWContents();
            }

            registers[operandRegister1].SetBytes(0, result);
            ChangeDisplayRegisterContents(operandRegister1);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionPACK
         * 
         * Author(s):   Michael Beaver    
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method takes the contents of mainMemory at the location provided by 
         *              the second address and "packs" the information. After packing the content, 
         *              it is stored in mainMemory at the locaton provided by the first address.
         *              
         *****************************************************************************************/
        private void PerformInstructionPACK(uint locationCounter)
        {
            string operandValue2;
            string result = "";
            string zone;
            uint length1;
            uint length2;
            uint lengthDiff;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSS(locationCounter, out length1, out length2,
                out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            operandValue2 = mainMemory.GetBytesString(operandAddress2,
                operandAddress2 + length2 - 1);

            /* Reverse the zone byte. */
            for (int i = 0; i < (length2 * 2) - 2; i++)
                result += operandValue2[i].ToString();

            zone = operandValue2[((int)length2 * 2) - 2].ToString();
            result = result.Insert(((int)length2 * 2) - 2,
                operandValue2[((int)length2 * 2) - 1].ToString());

            /* Strip zone characters and append final zone. */
            result = result.Replace(EBCDIC_BLANK, "");
            for (int i = 0; i < result.Length - 1; i++)
            {
                result = result.Remove(i, 1);
            }
            result += zone;
            result = result.PadLeft((int)length2 * 2, '0');

            /* Case 1: First operand is too small. Lose most significant digits without error. */
            if (length1 < length2)
            {
                lengthDiff = (length2 * 2) - (length1 * 2) + 1;

                /* Truncate the most significant digits. */
                if (lengthDiff % 2 == 1)
                    result = result.Substring((int)lengthDiff - 1);

                else
                    result = result.Substring((int)lengthDiff);
            }

            /* Case 2: First operand is too large. Pad with leading zeros. */
            else if (length1 > length2)
                result = result.PadLeft((int)length1 * 2, '0');

            mainMemory.SetBytes(operandAddress1, result);
            ChangeDisplayMemoryContents(operandAddress1, length1);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionS
         * 
         * Author(s):   Michael Beaver          
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method subtracts the contents in mainMemory at the location provided by
         *              the address from the contents of the register. The result is stored in the
         *              register.
         *              
         *****************************************************************************************/
        private void PerformInstructionS(uint locationCounter)
        {
            int addressValue;
            int registerValue;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            registerValue = registers[operandRegister].GetBytesInt(0, 3);

            /* Check for fullword boundary alignment. */
            if (operandAddress % FULLWORD == 0)
            {
                addressValue = mainMemory.GetBytesInt(operandAddress, operandAddress + 3);

                try
                {
                    registerValue = checked(registerValue - addressValue);
                }

                /* Overflow Error. */
                catch
                {
                    progStatWord.SetCondCode(3);
                    ChangeDisplayPSWContents();
                    errorHandling.throwException(ExceptionCodes.FP_OVERFLOW_EXCEPTION);
                }

                /* Update the condition code accordingly. */
                if (registerValue == 0)
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else if (registerValue < 0)
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                else if (registerValue > 0)
                {
                    progStatWord.SetCondCode(2);
                    ChangeDisplayPSWContents();
                }

                registers[operandRegister].SetBytes(0, registerValue);
                ChangeDisplayRegisterContents(operandRegister);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionSP
         * 
         * Author(s):   Michael Beaver
         *                    
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method unpacks the two packed numbers stored in mainMemory provided
         *              by the addresses and subtracts the packed number at the second address from
         *              the packed number at the first address. The result is stored in mainMemory
         *              at the location provided by the first address.
         *              
         *****************************************************************************************/
        private void PerformInstructionSP(uint locationCounter)
        {
            char zone;
            string address1Value;
            string address2Value;
            string packedDifference;
            uint length1;
            uint length2;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSS(locationCounter, out length1, out length2,
                out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            address1Value = mainMemory.GetBytesString(operandAddress1, 
                operandAddress1 + length1 - 1);
            address2Value = mainMemory.GetBytesString(operandAddress2, 
                operandAddress2 + length2 - 1);

            /* Both numbers must be valid packed. */
            if (IsPackedNumber(address1Value) && IsPackedNumber(address2Value))
            {
                /* If the second operand is positive, change it to negative. */
                zone = address2Value[address2Value.Length - 1];

                if (zone == 'A' || zone == 'C' || zone == 'E' || zone == 'F')
                    address2Value = address2Value.Replace(zone.ToString(), "D");

                packedDifference = AddPackedValues(address1Value, address2Value);

                /* Avoid overflow. */
                if (packedDifference.Length <= address1Value.Length && packedDifference != null)
                {
                    packedDifference = packedDifference.PadLeft(address1Value.Length, '0');
                    mainMemory.SetBytes(operandAddress1, packedDifference);
                    ChangeDisplayMemoryContents(operandAddress1, length1);

                    /* Negative result. */
                    if (packedDifference[packedDifference.Length - 1] == 'B' ||
                        packedDifference[packedDifference.Length - 1] == 'D')
                    {
                        progStatWord.SetCondCode(1);
                        ChangeDisplayPSWContents();
                    }

                    /* Positive result. */
                    else if (packedDifference[packedDifference.Length - 1] == 'A' ||
                        packedDifference[packedDifference.Length - 1] == 'C')
                    {
                        progStatWord.SetCondCode(2);
                        ChangeDisplayPSWContents();
                    }

                    /* Zero result. */
                    else
                    {
                        progStatWord.SetCondCode(0);
                        ChangeDisplayPSWContents();
                    }
                }

                else
                {
                    progStatWord.SetCondCode(3);
                    ChangeDisplayPSWContents();
                    errorHandling.throwException(ExceptionCodes.DEC_OVERFLOW_EXCEPTION);
                }
            }

            else
                errorHandling.throwException(ExceptionCodes.DATA_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionSR
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *                    
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method subtracts the contents of the second register from the contents
         *              of the first register. The result is stored in the first register.
         *              
         *****************************************************************************************/
        private void PerformInstructionSR(uint locationCounter)
        {
            int register1Value;
            int register2Value;
            uint operandRegister1;
            uint operandRegister2;

            ProcessTypeRR(locationCounter, out operandRegister1, out operandRegister2);

            register1Value = registers[operandRegister1].GetBytesInt(0, 3);
            register2Value = registers[operandRegister2].GetBytesInt(0, 3);

            try
            {
                register1Value = checked(register1Value - register2Value);
            }

            catch
            {
                progStatWord.SetCondCode(3);
                ChangeDisplayPSWContents();
                errorHandling.throwException(ExceptionCodes.FP_OVERFLOW_EXCEPTION);
            }

            /* Update the condition code accordingly. */
            if (register1Value == 0)
            {
                progStatWord.SetCondCode(0);
                ChangeDisplayPSWContents();
            }

            else if (register1Value < 0)
            {
                progStatWord.SetCondCode(1);
                ChangeDisplayPSWContents();
            }

            else if (register1Value > 0)
            {
                progStatWord.SetCondCode(2);
                ChangeDisplayPSWContents();
            }

            registers[operandRegister1].SetBytes(0, register1Value);
            ChangeDisplayRegisterContents(operandRegister1);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionST
         * 
         * Author(s):   Michael Beaver  
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method stores the contents of the register into mainMemory at the
         *              location provided by the address.
         *              
         *****************************************************************************************/
        private void PerformInstructionST(uint locationCounter)
        {
            string registerContents;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            /* Fullword boundary alignment check. */
            if (operandAddress % FULLWORD == 0)
            {
                registerContents = registers[operandRegister].GetBytesString(0, REGISTER_SIZE);
                mainMemory.SetBytes(operandAddress, registerContents);
                ChangeDisplayMemoryContents(operandAddress, FULLWORD);
            }

            /* Error. */
            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionSTM
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method stores the contents of the registers starting with the first 
         *              register going to the second register into mainMemory at the location 
         *              provided by the address. This method is circular, meaning that if the first
         *              register is register 10 and the second register is register 1, this method
         *              will store the registers 10, 11, 12, 13, 14, 15, 0, and 1 into mainMemory.
         *              
         *              
         *****************************************************************************************/
        private void PerformInstructionSTM(uint locationCounter)
        {
            string registerContent;
            uint operandAddress;
            uint operandRegister1;
            uint operandRegister2;
            uint registerIndex;
            uint storageIndex = 0;

            ProcessTypeRS(locationCounter, out operandRegister1,
                out operandRegister2, out operandAddress);

            if (operandAddress > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            registerIndex = operandRegister1;

            /* Traverse the registers and save their contents to memory. */
            if (operandAddress % FULLWORD == 0)
            {
                do
                {
                    registerContent = registers[registerIndex].GetBytesString(0, REGISTER_SIZE);
                    mainMemory.SetBytes(operandAddress + storageIndex, registerContent);
                    storageIndex += FULLWORD;

                    /* Special condition: Registers are the same. */
                    if (registerIndex == operandRegister2)
                        registerIndex = operandRegister2 + 1;

                    else if (registerIndex == MAX_REGISTER)
                        registerIndex = 0;

                    else
                        registerIndex++;

                } while (registerIndex != operandRegister2 + 1);

                ChangeDisplayMemoryContents(operandAddress, storageIndex);
            }

            else
                errorHandling.throwException(ExceptionCodes.SPECIFICATION_EXCEPTION);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionUNPK
         * 
         * Author(s):   Michael Beaver    
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method takes the packed data stored in mainMemory at the location 
         *              provided by the second address and unpacks it and then stores the unpacked
         *              data into mainMemory at the location provided by the first address.
         *              
         *****************************************************************************************/
        private void PerformInstructionUNPK(uint locationCounter)
        {
            int index = 0;
            string operandValue2;
            string result = "";
            string zone;
            uint length1;
            uint length2;
            uint lengthDiff;
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSS(locationCounter, out length1, out length2,
                out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            operandValue2 = mainMemory.GetBytesString(operandAddress2,
                operandAddress2 + length2 - 1);

            /* Reverse the zone byte. */
            for (int i = 0; i < operandValue2.Length - 2; i++)
                result += operandValue2[i].ToString();

            zone = operandValue2[operandValue2.Length - 2].ToString();
            result = result.Insert(operandValue2.Length - 2,
                operandValue2[operandValue2.Length - 1].ToString());
            result += zone;

            /* Insert the new zone characters. */
            for (int i = 0; i < result.Length - 2; i += 2)
                result = result.Insert(i, "F");

            /* Truncate most significant digits. */
            if (result.Length > (length1 * 2))
            {
                lengthDiff = Convert.ToUInt32(result.Length) - (length1 * 2);
                result = result.Substring((int)lengthDiff);
            }

            /* Pad with zero to fill space. */
            else if (result.Length < (length1 * 2))
            {
                while (result.Length < (length1 * 2))
                {
                    result = result.Insert(index, "F0");
                    index += 2;
                }
            }

            mainMemory.SetBytes(operandAddress1, result);
            ChangeDisplayMemoryContents(operandAddress1, length1);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionXDECI
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method will convert the first decimal number found starting at a
         *              memory location into hexadecimal and store it into the specified register.
         *              
         *              
         *****************************************************************************************/
        private void PerformInstructionXDECI(uint locationCounter)
        {
            bool loopControl = true;
            bool negativeValue = false;
            int currentIndexValue;
            int finalValue = 0;
            string decimalString = "";
            uint maxMemory = mainMemory.GetMemorySize();
            uint numEnd;
            uint numStart;
            uint offset = 0;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            while (loopControl)
            {
                /* Memory bounds check. */
                if (operandAddress + offset > maxMemory)
                {
                    loopControl = false;
                    progStatWord.SetCondCode(3);
                    ChangeDisplayPSWContents();
                    errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);
                }

                currentIndexValue = mainMemory.GetByteInt(operandAddress + offset);

                /* Detect negative signs. */
                if (currentIndexValue == EBCDIC_INT_MINUS)
                    negativeValue = true;

                /* Only act on values in memory that are digits (X'F0' to X'F9'). */
                if (currentIndexValue >= EBCDIC_INT_ZERO && currentIndexValue <= EBCDIC_INT_NINE)
                {
                    numStart = operandAddress + offset;
                    offset++;
                    currentIndexValue = mainMemory.GetByteInt(operandAddress + offset);

                    /* Find the end of the number, if it is longer than one digit. */
                    while (currentIndexValue >= EBCDIC_INT_ZERO && 
                        currentIndexValue <= EBCDIC_INT_NINE)
                    {
                        offset++;
                        currentIndexValue = mainMemory.GetByteInt(operandAddress + offset);
                    }
                        
                    /* Set Register 1 to the byte immediately after the found number. */
                    registers[1].SetBytes(0, Convert.ToInt32(operandAddress + offset));
                    ChangeDisplayRegisterContents(1);

                    /* Save the number in the specified register. */
                    numEnd = operandAddress + offset - 1;
                    decimalString = mainMemory.GetBytesString(numStart, numEnd);
                    decimalString = ToEBCDICChars(decimalString);
                    finalValue = Convert.ToInt32(decimalString, 10);

                    /* Set the number to negative. */
                    if (negativeValue)
                    {
                        finalValue *= -1;
                        negativeValue = false;
                    }

                    registers[operandRegister].SetBytes(0, finalValue);
                    ChangeDisplayRegisterContents(operandRegister);

                    /* Exit the loop as soon as a number is found and converted. */
                    loopControl = false;
                }

                /* Continue traversing memory. */
                else
                    offset++;
            }

            /* Set the condition code appropriately. */
            try
            {
                if (finalValue == 0)
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else if (finalValue < 0)
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                else if (finalValue > 0)
                {
                    progStatWord.SetCondCode(2);
                    ChangeDisplayPSWContents();
                }
            }
            
            catch
            {
                progStatWord.SetCondCode(3);
                ChangeDisplayPSWContents();
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionXDECO
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method converts the contents of the register from hexadecimal to its
         *              decimal representation and stores it into mainMemory at the location provided
         *              by the address.
         *              
         *****************************************************************************************/
        private void PerformInstructionXDECO(uint locationCounter)
        {
            int registerValue;
            string convertedValue;
            uint operandAddress;
            uint operandRegister;

            ProcessTypeRX(locationCounter, out operandRegister, out operandAddress);

            registerValue = registers[operandRegister].GetBytesInt(0, REGISTER_SIZE);
            convertedValue = registerValue.ToString();
            convertedValue = convertedValue.PadLeft(12, ' ');
            convertedValue = ToEBCDIC(convertedValue);

            mainMemory.SetBytes(operandAddress, convertedValue);

            /* 
             * FULLWORD * 3 is 12 characters. 
             * XDECO takes a register and converts it into 12 characters. 
             */
            ChangeDisplayMemoryContents(operandAddress, FULLWORD * 3);
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionXPRNT
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method reads contents equal to the length provided by the instruction 
         *              from mainMemory referenced by the address provided in the instruction. The
         *              length must be under 133. 
         *              
         *****************************************************************************************/
        private void PerformInstructionXPRNT(uint locationCounter)
        {
            StreamWriter prtOutput = null;
            string operandContents;
            string printer = "";
            uint length;
            uint operandAddress;

            ProcessTypeX(locationCounter, out operandAddress, out length);

            /* XPRNT will not print more than XPRNT_MAX (including carriage control) characters. */
            if (length > XPRNT_MAX)
                length = XPRNT_MAX;
            
            operandContents = mainMemory.GetBytesString(operandAddress, 
                operandAddress + length - 1);
            operandContents = ToEBCDICChars(operandContents);

            /*
             * NOTE: Currently, the cls carriage control will print the string at the BOTTOM
             * of the console. We need the printed string to appear at the TOP of the console.
             */

            /* New screen carriage control. */
            if (operandContents[0] == '1')
            {
                for (int i = 0; i < CONSOLE_SIZE; i++)
                    printer += Environment.NewLine;
                printer += operandContents.Substring(1);
                display.AppendOutputText(printer);
            }
            
            else
            {
                /* Double space carriage control. */
                if (operandContents[0] == '0')
                {
                    printer += Environment.NewLine + Environment.NewLine;
                    linesPrinted += 2;
                }

                /* Suppress carriage control. */
                else if (operandContents[0] == '+')
                {
                    /* Do nothing. */
                }

                /* Single space carriage control. */
                else
                {
                    printer += Environment.NewLine;
                    linesPrinted++;
                }

                printer += operandContents.Substring(1);
                display.AppendOutputText(printer);
            }

            /* Replace all but one new line with the new page character for the PRT. */
            if (operandContents[0] == '1')
            {
                printer = printer.Remove(0, (CONSOLE_SIZE - 1) * 2);
                
                /* New page (form feed). */
                printer = printer.Insert(0, "\f");
                linesPrinted = 0;
            }

            /* Write out to the PRT. */
            try
            {
                prtOutput = new StreamWriter(display.GetPathPRT(), true);

                if (linesPrinted == (MAX_LINES_PER_PAGE - 1) && operandContents[0] != '1')
                {

                    printer += "\f";
                    
                    linesPrinted = 0;
                }

                prtOutput.Write(printer);
            }
            
            catch
            {
                MessageBox.Show("Unable to write to the .PRT file!", "XPRNT Error");
            }

            finally
            {
                prtOutput.Close();
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionXREAD
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver
         *              
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method will read data from a "card" (a memory location) into the
         *              specified memory location. The maximum bytes read is XREAD_MAX.           
         *              
         *****************************************************************************************/
        private void PerformInstructionXREAD(uint locationCounter)
        {
            int lengthDelta = 0;
            string dataLine;
            uint length;
            uint operandAddress;

            ProcessTypeX(locationCounter, out operandAddress, out length);

            /* Invalid data "card" or we have reached end of data. */
            if (entryStream.Count() == 0 || dataEntryIndex >= entryStream.Count())
            {
                progStatWord.SetCondCode(1);
                ChangeDisplayPSWContents();
            }

            else 
            {
                dataLine = entryStream[dataEntryIndex];
                dataLine = ToEBCDIC(dataLine);

                /* XREAD only reads up to XREAD_MAX characters. */
                if (dataLine.Length > XREAD_MAX)
                    dataLine = dataLine.Substring(0, XREAD_MAX);

                /* Pad EBCDIC zeroes. */
                lengthDelta = (int)length - (dataLine.Length / 2);
                for (int i = 0; i < lengthDelta; i++)
                    dataLine += EBCDIC_BLANK;

                /* Check for read and save success or failure. */
                if (mainMemory.SetBytes(operandAddress, dataLine))
                {
                    ChangeDisplayMemoryContents(operandAddress, length);
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                dataEntryIndex++;
            }
        }

        /******************************************************************************************
         * 
         * Name:        PerformInstructionZAP
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       The locationCounter is an unsigned integer.
         * Return:      N/A
         * Description: This method sets the contents of mainMemory at the location provided by the
         *              first address to zero and then stores the packed number from mainMemory at 
         *              the location provided by the second address into mainMemory at the location
         *              provided by the first address.
         *              
         *****************************************************************************************/
        private void PerformInstructionZAP(uint locationCounter)
        {
            char zone;
            string operandValue1;
            string operandValue2;
            string result;
            uint length1;
            uint length2; 
            uint operandAddress1;
            uint operandAddress2;

            ProcessTypeSS(locationCounter, out length1, out length2,
                out operandAddress1, out operandAddress2);

            if (operandAddress1 > maxMemorySize || operandAddress2 > maxMemorySize)
                errorHandling.throwException(ExceptionCodes.ADDRESSING_EXCEPTION);

            if (operandAddress1 > maxProgramMemory || operandAddress2 > maxProgramMemory)
                errorHandling.throwException(ExceptionCodes.PROTECTION_EXCEPTION);

            /* Zero out the memory location. */
            for (uint i = 0; i < length1 - 1; i++)
                mainMemory.SetByte(operandAddress1 + i, 0);
            mainMemory.SetByte(operandAddress1 + length1 - 1, "0F");
            ChangeDisplayMemoryContents(operandAddress1, length1);


            /* This value DOES NOT need to be valid packed. */
            operandValue1 = mainMemory.GetBytesString(operandAddress1,
                operandAddress1 + length1 - 1);

            /* This value MUST be valid packed. */
            operandValue2 = mainMemory.GetBytesString(operandAddress2,
                operandAddress2 + length2 - 1);

            if (IsPackedNumber(operandValue2))
            {
                result = AddPackedValues(operandValue1, operandValue2);

                if (result == null)
                {
                    progStatWord.SetCondCode(3);
                    ChangeDisplayPSWContents();
                    errorHandling.throwException(ExceptionCodes.DEC_OVERFLOW_EXCEPTION);
                }

                result = result.PadLeft(operandValue1.Length, '0');
                zone = result[result.Length - 1];

                /* Update the condition code accordingly. */
                if (Convert.ToInt32(result.Substring(0, result.Length - 1)) == 0)
                {
                    progStatWord.SetCondCode(0);
                    ChangeDisplayPSWContents();
                }

                else if (zone == 'B' || zone == 'D')
                {
                    progStatWord.SetCondCode(1);
                    ChangeDisplayPSWContents();
                }

                else if (zone == 'A' || zone == 'C' || zone == 'E' || zone == 'F')
                {
                    progStatWord.SetCondCode(2);
                    ChangeDisplayPSWContents();
                }

                /* Overflow. */
                if (length1 < length2)
                {
                    progStatWord.SetCondCode(3);
                    ChangeDisplayPSWContents();
                    errorHandling.throwException(ExceptionCodes.DEC_OVERFLOW_EXCEPTION);
                }

                mainMemory.SetBytes(operandAddress1, result);
                ChangeDisplayMemoryContents(operandAddress1, length1);
            }

            else
                errorHandling.throwException(ExceptionCodes.DATA_EXCEPTION);
        }
    }
}