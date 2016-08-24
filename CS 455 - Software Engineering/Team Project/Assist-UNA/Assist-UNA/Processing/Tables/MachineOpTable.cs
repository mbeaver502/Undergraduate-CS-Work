using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: MachineOpTable
 * 
 * ================================================================================================
 * 
 * Description: The MachineOpTable class holds the information for each instruction implemented in
 *              ASSIST/UNA. Information for each instruction includes the instruction format type,
 *              first operand type, second operand type, third operand type, and object code for 
 *              the instruction. The MachineOpTable is a static class and has no methods for
 *              manipulating the contents of the table. To add instruction to the table, the opcode
 *              must be added to the opKey table as well as the opTable along with the instruction
 *              type, first operand, second operand, third operand, and object code. New 
 *              instruction must be added in alphabetical order by the opcode.
 *              
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/17/2014    AAH    Created class, added data members and set up functions and headers. 
 *                      Wrote the GetOperand1Type, GetOperand2Type, GetOperand3Type, and GetOpType
 *                          methods.
 * 03/20/2014    AAH    Created constructor.
 * 03/20/2014    JMB    Assisted in setting up MachineOpTable.
 * 03/22/2014    AAH    Finished constructor.
 *                      Created GetObjectCode method.
 *                      Wrote isOpcode method.
 *                      Created main method for testing.
 *                      Changed methods to static.
 *                      Deleted constructor and created Initialize method.  
 * 03/25/2014    AAH    Removed Initialize method and started hard coding the opKey and opTable 
 *                          in the declaration of the data members.
 * 03/26/2014    AAH    Continued hard coding the opTable.
 * 03/27/2014    AAH    Made changes to B, BR, XREAD, and XPRNT, worked with Travis on integrating                      
 *                          the new changes into the Assember.
 * 03/27/2014    THH    Added the DR instruction.
 *                      Replaced the static bounds on the accessor functions with the length of the
 *                          opTable.
 * 03/28/2014    AAH    Fixed SP object code.
 * 03/29/2014    THH    Fixed opcode for XPRNT and XREAD.
 * 04/10/2014    THH    Fixed operation format for CLC.
 * 04/13/2014    AAH    Added mnemonics.
 * 04/25/2014    JMB    Added GetMnemonicFromObjectCode method for use in runtime error report
 *                          generation. Modified IsOpcode method to not search for null or
 *                          empty string values.
 * 04/25/2014    AAH    Fixed class name along with some commenting and the modification 
 *                          instructions.
 * 04/26/2014    JMB    Updated GetMnemonicFromObjectCode to work properly with XPRNT and XREAD.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    static class MachineOpTable
    {
        /* Constants. */
        private const int NUM_INSTRUCTIONS = 78;//48;
        private const int NUM_OP_TABLE_COLS = 5;
        private const int OBJECT_CODE_COL = 5;
        private const int OPCODE_COL = 0;
        private const int OPTYPE_COL = 1;
        private const int OPERAND1_COL = 2;
        private const int OPERAND2_COL = 3;
        private const int OPERAND3_COL = 4;

        /* Private members. */
        private static string[] opKey = new string[]     {"A",
                                                    "AP",
                                                    "AR",
                                                    "B",
                                                    "BAL",
                                                    "BALR",
                                                    "BC",
                                                    "BCR",
                                                    "BCT",
                                                    "BCTR",
                                                    "BE",
                                                    "BER",
                                                    "BH",
                                                    "BHR",
                                                    "BL",
                                                    "BLR",
                                                    "BM",
                                                    "BMR",
                                                    "BNE",
                                                    "BNER",
                                                    "BNH",
                                                    "BNHR",
                                                    "BNL",
                                                    "BNLR",
                                                    "BNM",
                                                    "BNMR",
                                                    "BNO",
                                                    "BNOR",
                                                    "BNP",
                                                    "BNPR",
                                                    "BNZ",
                                                    "BNZR",
                                                    "BO",
                                                    "BOR",
                                                    "BP",
                                                    "BPR",
                                                    "BR",
                                                    "BXH",
                                                    "BXLE",
                                                    "BZ",
                                                    "BZR",
                                                    "C",
                                                    "CLC",
                                                    "CLI",
                                                    "CP",
                                                    "CR",
                                                    "D",
                                                    "DP",
                                                    "DR",
                                                    "ED",
                                                    "EDMK",
                                                    "L",
                                                    "LA",
                                                    "LM",
                                                    "LR",
                                                    "M",
                                                    "MP",
                                                    "MR",
                                                    "MVC",
                                                    "MVI",
                                                    "N",
                                                    "NOP",
                                                    "NOPR",
                                                    "NR",
                                                    "O",
                                                    "OR",
                                                    "PACK",
                                                    "S",
                                                    "SP",
                                                    "SR",
                                                    "ST",
                                                    "STM",
                                                    "UNPK",
                                                    "XDECI",
                                                    "XDECO",
                                                    "XPRNT",
                                                    "XREAD",
                                                    "ZAP"
                                                    };

        private static string[,] opTable = new string[,] {
                                                        {"A","RX","R","DXB","N","5A"},
                                                        {"AP", "SS", "DLB", "DLB", "N", "FA"},
                                                        {"AR", "RR", "R", "R", "N", "1A"},
                                                        {"B", "RX", "M", "DXB", "N", "47"},
                                                        {"BAL", "RX", "R", "DXB", "N", "45"},
                                                        {"BALR", "RR", "R", "R", "N", "05"},
                                                        {"BC", "RX", "M", "DXB", "N", "47"},
                                                        {"BCR", "RR", "M", "R", "N", "07"},
                                                        {"BCT", "RX", "R", "DXB", "N", "46"},
                                                        {"BCTR", "RR", "R", "R", "N", "06"},
                                                        {"BE", "RX", "M", "DXB", "N", "47"},
                                                        {"BER", "RR", "M", "DXB", "N", "07"},
                                                        {"BH", "RX", "M", "DXB", "N", "47"},
                                                        {"BHR", "RR", "M", "DXB", "N", "07"},
                                                        {"BL", "RX", "M", "DXB", "N", "47"},
                                                        {"BLR", "RR", "M", "DXB", "N", "07"},
                                                        {"BM", "RX", "M", "DXB", "N", "47"},
                                                        {"BMR", "RR", "M", "DXB", "N", "07"},
                                                        {"BNE", "RX", "M", "DXB", "N", "47"},
                                                        {"BNER", "RR", "M", "DXB", "N", "07"},
                                                        {"BNH", "RX", "M", "DXB", "N", "47"},
                                                        {"BNHR", "RR", "M", "DXB", "N", "07"},
                                                        {"BNL", "RX", "M", "DXB", "N", "47"},
                                                        {"BNLR", "RR", "M", "DXB", "N", "07"},
                                                        {"BNM", "RX", "M", "DXB", "N", "47"},
                                                        {"BNMR", "RR", "M", "DXB", "N", "07"},
                                                        {"BNP", "RX", "M", "DXB", "N", "47"},
                                                        {"BNPR", "RR", "M", "DXB", "N", "07"},
                                                        {"BNO", "RX", "M", "DXB", "N", "47"},
                                                        {"BNOR", "RR", "M", "DXB", "N", "07"},
                                                        {"BNZ", "RX", "M", "DXB", "N", "47"},
                                                        {"BNZR", "RR", "M", "DXB", "N", "07"},
                                                        {"BO", "RX", "M", "DXB", "N", "47"},
                                                        {"BOR", "RR", "M", "DXB", "N", "07"},
                                                        {"BP", "RX", "M", "DXB", "N", "47"},
                                                        {"BPR", "RR", "M", "DXB", "N", "07"},
                                                        {"BR", "RR", "M", "DXB", "N", "07"},
                                                        {"BXH", "RS", "R", "R", "DB", "86"},
                                                        {"BXLE", "RS", "R", "R", "DB", "87"},
                                                        {"BZ", "RX", "M", "DXB", "N", "47"},
                                                        {"BZR", "RR", "M", "DXB", "N", "07"},
                                                        {"C", "RX", "R", "DXB", "N", "59"},
                                                        {"CLC", "SS", "DLB", "DB", "N", "D5"},
                                                        {"CLI", "SI", "DB", "I", "N", "95"},
                                                        {"CP", "SS", "DLB", "DLB", "N", "F9"},
                                                        {"CR", "RR", "R", "R", "N", "19"},
                                                        {"D", "RX", "R", "DXB", "N", "5D"},
                                                        {"DP", "SS", "DLB", "DLB", "N", "FD"},
                                                        {"DR", "RR", "R", "R", "N", "1D"},
                                                        {"ED", "SS", "DLB", "DB", "N", "DE"},
                                                        {"EDMK", "SS", "DLB", "DB", "N", "DF"},
                                                        {"L", "RX", "R", "DXB", "N", "58"},
                                                        {"LA", "RX", "R", "DXB", "N", "41"},
                                                        {"LM", "RS", "R", "DXB", "N", "98"},
                                                        {"LR", "RR", "R", "R", "N", "18"},
                                                        {"M", "RX", "R", "DXB", "N", "5C"},
                                                        {"MP", "SS", "DLB", "DLB", "N", "FC"},
                                                        {"MR", "RR", "R", "R", "N", "1C"},
                                                        {"MVC", "SS", "DLB", "DB", "N", "D2"},
                                                        {"MVI", "SI", "DB", "I", "N", "92"},
                                                        {"N", "RX", "R", "DXB", "N", "54"},
                                                        {"NOP", "RX", "M", "DXB", "N", "47"},
                                                        {"NOPR", "RR", "M", "R", "N", "07"},
                                                        {"NR", "RR", "R", "R", "N", "14"},
                                                        {"O", "RX", "R", "DXB", "N", "56"},
                                                        {"OR", "RR", "R", "R", "N", "16"},
                                                        {"PACK", "SS", "DLB", "DLB", "N", "F2"},
                                                        {"S", "RX", "R", "DXB", "N", "5B"},
                                                        {"SP", "SS", "DLB", "DLB", "N", "FB"},
                                                        {"SR", "RR", "R", "R", "N", "1B"},
                                                        {"ST", "RX", "R", "DXB", "N", "50"},
                                                        {"STM", "RS", "R", "DXB", "N", "90"},
                                                        {"UNPK", "SS", "DLB", "DLB", "N", "F3"},
                                                        {"XDECI", "RX", "R", "DXB", "N", "53"},
                                                        {"XDECO", "RX", "R", "DXB", "N", "52"},
                                                        {"XPRNT", "X", "DXB", "L", "N", "E02"},
                                                        {"XREAD", "X", "DXB", "L", "N", "E00"},
                                                        {"ZAP", "SS", "DLB", "DLB", "N", "F8"}
                                                        };



        /* Public methods. */
        /******************************************************************************************
         * 
         * Name:        GetMnemonicFromObjectCode      
         * 
         * Author(s):   Michael Beaver
         *                      
         * Input:       The object code is a string, and xInstruction is a code denoting whether
         *              the object code is XPRNT or XREAD (0 and 1, respectively). An xInstruction
         *              other than 0 or 1 is assumed to not be an X-instruction.
         * Return:      The mnemonic is a string.
         * Description: This method will attempt to find the corresponding mnemonic (e.g., "AR")
         *              given a certain byte of object code. If the corresponding mnemonic is
         *              not found (i.e., the object code is not a valid instruction), then a
         *              null string is returned.
         *              
         *****************************************************************************************/
        public static string GetMnemonicFromObjectCode(string objectCode, int xInstruction)
        {
            string result = null;

            /* XPRNT. */
            if (xInstruction == 0)
                result = opKey[NUM_INSTRUCTIONS - 3];

            /* XREAD. */
            if (xInstruction == 1)
                result = opKey[NUM_INSTRUCTIONS - 2];

            else
            {
                for (int i = 0; i < NUM_INSTRUCTIONS; i++)
                {
                    /* Special case: XPRNT and XREAD. This will always return XPRNT. */
                    if (objectCode == "E0")
                    {
                        result = opKey[NUM_INSTRUCTIONS - 3];
                        break;
                    }

                    else if (objectCode == opTable[i, OBJECT_CODE_COL])
                    {
                        result = opKey[i];
                        break;
                    }
                }
            }

            return result;
        }

        /******************************************************************************************
            * 
            * Name:        GetObjectCode      
            * 
            * Author(s):   Andrew Hamilton
            *              Travis Hunt
            *              
            *              
            * Input:       Index of the Object code to be retrieved.  
            * Return:      The object code (as a String) of the given index.
            * Description: This method will return the object code of a given index in the MachineOpTable.
            *              
            *****************************************************************************************/
        public static string GetObjectCode(int index)
        {
            if (index < 0 || index > opTable.Length)
                return "";

            return opTable[index, OBJECT_CODE_COL];
        }

        /******************************************************************************************
            * 
            * Name:        GetOperand1Type        
            * 
            * Author(s):   Andrew Hamilton
            *              Travis Hunt
            *              
            *              
            * Input:       The index of the instruction.   
            * Return:      The instruction's first operand.
            * Description: This method will return the first operand of a given instruction in the 
            *              machine op table.
            *              
            *****************************************************************************************/
        public static string GetOperand1Type(int index)
        {
            if (index < 0 || index > opTable.Length)
                return "";

            return opTable[index, OPERAND1_COL];
        }

        /******************************************************************************************
            * 
            * Name:        GetOperand2Type        
            * 
            * Author(s):   Andrew Hamilton
            *              Travis Hunt
            *              
            *              
            * Input:       The index of the instruction.   
            * Return:      The instruction's second operand.
            * Description: This method will return the second operand of a given instruction in the 
            *              machine op table.
            *              
            *****************************************************************************************/
        public static string GetOperand2Type(int index)
        {
            if (index < 0 || index > opTable.Length)
                return "";

            return opTable[index, OPERAND2_COL];
        }

        /******************************************************************************************
            * 
            * Name:        GetOperand3Type        
            * 
            * Author(s):   Andrew Hamilton
            *              Travis Hunt
            *              
            *              
            * Input:       The index of the instruction.   
            * Return:      The instruction's third operand.
            * Description: This method will return the third operand of a given instruction in the 
            *              machine op table. If the given instruction does not have a third operand,
            *              then NULL will be returned.
            * 
            *****************************************************************************************/
        public static string GetOperand3Type(int index)
        {
            if (index < 0 || index > opTable.Length)
                return "";

            return opTable[index, OPERAND3_COL];
        }

        /******************************************************************************************
            * 
            * Name:        GetOpType       
            * 
            * Author(s):   Andrew Hamilton  
            *              Travis Hunt
            *              
            *              
            * Input:       The index of the instruction.      
            * Return:      The format of the given instruction (RR, RX, RS, SS, SI, X).
            * Description: This method will return the format of the given instruction.
            *              
            *****************************************************************************************/
        public static string GetOpType(int index)
        {
            if (index < 0 || index > opTable.Length)
                return "";

            return opTable[index, OPTYPE_COL];
        }

        /******************************************************************************************
            * 
            * Name:        IsOpcode   
            * 
            * Author(s):   Andrew Hamilton  
            *              Michael Beaver
            *              
            * Input:       A candidate string to be determined whether or not it is a valid opcode.      
            * Return:      The method will return the index of the given opcode if it is found in the 
            *              machine op table, or -1 if it is a invalid opcode.
            * Description: This method will determine whether or not a given string is a valid 
            *              instruction.        
            *              
            *****************************************************************************************/
        public static int IsOpcode(string opcode)
        {
            int index = -1;

            if (String.IsNullOrEmpty(opcode))
                index = -1;

            else
                index = Array.BinarySearch(opKey, opcode);

            if (index < 0)
                index = -1;

            return index;
        }
    }
}
