using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**************************************************************************************************
 * 
 * Name: MachineOpTableTest
 * 
 * ================================================================================================
 * 
 * Description: Instructions on modifications.
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
 *                       
 *************************************************************************************************/

static class MachineOpTableTest
    {
        /* Constants. */
        private const int NUM_INSTRUCTIONS = 48;
        private const int NUM_OP_TABLE_COLS = 5;
        private const int OBJECT_CODE_COL = 5;
        private const int OPCODE_COL = 0;
        private const int OPTYPE_COL = 1;
        private const int OPERAND1_COL = 2;
        private const int OPERAND2_COL = 3;
        private const int OPERAND3_COL = 4;

        /* Private members. */
        private static string[] opKey = new string[] {"A",
                                                      "AP",
                                                      "AR",
                                                      "B",
                                                      "BAL",
                                                      "BALR",
                                                      "BC",
                                                      "BCR",
                                                      "BCT",
                                                      "BCTR",
                                                      "BR",
                                                      "BXH",
                                                      "BXLE",
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
                                                         {"BR", "RR", "M", "DXB", "N", "07"},
                                                         {"BXH", "RS", "R", "R", "DB", "86"},
                                                         {"BXLE", "RS", "R", "R", "DB", "87"},
                                                         {"C", "RX", "R", "DXB", "N", "59"},
                                                         {"CLC", "RS", "DLB", "DB", "N", "D5"},
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

        //static int Main()
        //{
        //    MachineOpTableTest.Initialize();

        //    /* IsOpcode Tests. */
        //    Console.WriteLine("IsOpcode test cases:");
        //    Console.WriteLine("'A' index: " + IsOpcode("A"));
        //    Console.WriteLine("'ZAP' index: " + IsOpcode("ZAP"));
        //    Console.WriteLine("'L' index: " + IsOpcode("L"));
        //    Console.WriteLine("'D' index: " + IsOpcode("D"));
        //    Console.WriteLine("'OR' index: " + IsOpcode("OR"));
        //    Console.WriteLine("'CP' index: " + IsOpcode("CP"));
        //    Console.WriteLine("'ST' index: " + IsOpcode("ST"));
        //    Console.WriteLine();

        //    /* GetObject code test cases. */
        //    Console.WriteLine("GetObjectCode test cases:");
        //    Console.WriteLine("Index 0 (A) object code: " + GetObjectCode(0));
        //    Console.WriteLine("Index 45 (ZAP) object code: " + GetObjectCode(45));
        //    Console.WriteLine("Index 13 (CLI) object code: " + GetObjectCode(13));
        //    Console.WriteLine("Index 36 (SP) object code: " + GetObjectCode(36));
        //    Console.WriteLine("Index 25 (M) object code: " + GetObjectCode(25));
        //    Console.WriteLine("Index 10 (BXLE) object code: " + GetObjectCode(10));
        //    Console.WriteLine("Index -1 (invalid) object code: " + GetObjectCode(-1));
        //    Console.WriteLine();

        //    /* GetOperand1Type test cases. */
        //    Console.WriteLine("GetOperand1Type test cases:");
        //    Console.WriteLine("Index 0 (A) operand 1 type: " + GetOperand1Type(0));
        //    Console.WriteLine("Index 45 (ZAP) operand 1 type: " + GetOperand1Type(45));
        //    Console.WriteLine("Index 32 (O) operand 1 type: " + GetOperand1Type(32));
        //    Console.WriteLine("Index 44 (XREAD) operand 1 type: " + GetOperand1Type(44));
        //    Console.WriteLine("Index 2 (AR) operand 1 type: " + GetOperand1Type(2));
        //    Console.WriteLine("Index 39 (STM) operand 1 type: " + GetOperand1Type(39));
        //    Console.WriteLine("Index 46 (invalid) operand 1 type: " + GetOperand1Type(46));
        //    Console.WriteLine();

        //    /* GetOperand2Type test cases. */
        //    Console.WriteLine("GetOperand2Type test cases:");
        //    Console.WriteLine("Index 0 (A) operand 2 type: " + GetOperand2Type(0));
        //    Console.WriteLine("Index 45 (ZAP) operand 2 type: " + GetOperand2Type(45));
        //    Console.WriteLine("Index 35 (S) operand 2 type: " + GetOperand2Type(35));
        //    Console.WriteLine("Index 42 (XDECO) operand 2 type: " + GetOperand2Type(42));
        //    Console.WriteLine("Index 11 (C) operand 2 type: " + GetOperand2Type(11));
        //    Console.WriteLine("Index 23 (LM) operand 2 type: " + GetOperand2Type(23));
        //    Console.WriteLine("Index -5 (Invalid) operand 2 type: " + GetOperand2Type(-5));
        //    Console.WriteLine();

        //    /* GetOperand3Type test cases. */
        //    Console.WriteLine("GetOperand3Type test cases:");
        //    Console.WriteLine("Index 0 (A) operand 3 type: " + GetOperand3Type(0));
        //    Console.WriteLine("Index 45 (ZAP) operand 3 type: " + GetOperand3Type(45));
        //    Console.WriteLine("Index 3 (BAL) operand 3 type: " + GetOperand3Type(3));
        //    Console.WriteLine("Index 39 (SP) operand 3 type: " + GetOperand3Type(39));
        //    Console.WriteLine("Index 9 (BXH) operand 3 type: " + GetOperand3Type(9));
        //    Console.WriteLine("Index 10 (BXLE) operand 3 type: " + GetOperand3Type(10));
        //    Console.WriteLine("Index 100 (Invalid) operand 3 type: " + GetOperand3Type(100));
        //    Console.WriteLine();

        //    /* GetOpType test cases. */
        //    Console.WriteLine("GetOpType test cases:");
        //    Console.WriteLine("Index 0 (A) op type: " + GetOpType(0));
        //    Console.WriteLine("Index 45 (ZAP) op type: " + GetOpType(45));
        //    Console.WriteLine("Index 8 (BCTR) op type: " + GetOpType(8));
        //    Console.WriteLine("Index 13 (CLI) op type: " + GetOpType(13));
        //    Console.WriteLine("Index 23 (LM) op type: " + GetOpType(23));
        //    Console.WriteLine("Index 43 (XPRNT) op type: " + GetOpType(43));
        //    Console.WriteLine("Index -1 (Invalid) op type: " + GetOpType(-1));

        //    Console.ReadKey();
        //    return 0;
        //}

        ///******************************************************************************************
        // * 
        // * Name:        Initialize     
        // * 
        // * Author(s):   Andrew Hamilton
        // *              Michael Beaver
        // *              
        // *              
        // * Input:        N/A 
        // * Return:       N/A
        // * Description:  This method inserts each instruction's mnemonic into the opKey array and
        // *               inserts each instruction format and operands into the opTable array. This
        // *               method must be called before the opTable can be used.
        // *              
        // *              
        // *****************************************************************************************/

        //public static void Initialize()
        //{
        //    /* A. */
        //    opKey[0] = "A";
        //    opTable[0, 0] = "RX";
        //    opTable[0, 1] = "R";
        //    opTable[0, 2] = "DXB";
        //    opTable[0, 3] = "N";
        //    opTable[0, 4] = "5A";

        //    /* AP. */
        //    opKey[1] = "AP";
        //    opTable[1, 0] = "SS";
        //    opTable[1, 1] = "DLB";
        //    opTable[1, 2] = "DLB";
        //    opTable[1, 3] = "N";
        //    opTable[1, 4] = "FA";

        //    /* AR. */
        //    opKey[2] = "AR";
        //    opTable[2, 0] = "RR";
        //    opTable[2, 1] = "R";
        //    opTable[2, 2] = "R";
        //    opTable[2, 3] = "N";
        //    opTable[2, 4] = "1A";

        //    /* BAL. */
        //    opKey[3] = "BAL";
        //    opTable[3, 0] = "RX";
        //    opTable[3, 1] = "R";
        //    opTable[3, 2] = "DXB";
        //    opTable[3, 3] = "N";
        //    opTable[3, 4] = "45";

        //    /* BALR. */
        //    opKey[4] = "BALR";
        //    opTable[4, 0] = "RR";
        //    opTable[4, 1] = "R";
        //    opTable[4, 2] = "R";
        //    opTable[4, 3] = "N";
        //    opTable[4, 4] = "0D";

        //    /* BC. */
        //    opKey[5] = "BC";
        //    opTable[5, 0] = "RX";
        //    opTable[5, 1] = "M";
        //    opTable[5, 2] = "DXB";
        //    opTable[5, 3] = "N";
        //    opTable[5, 4] = "47";

        //    /* BCR. */
        //    opKey[6] = "BCR";
        //    opTable[6, 0] = "RR";
        //    opTable[6, 1] = "M";
        //    opTable[6, 2] = "R";
        //    opTable[6, 3] = "N";
        //    opTable[6, 4] = "07";

        //    /* BCT. */
        //    opKey[7] = "BCT";
        //    opTable[7, 0] = "RX";
        //    opTable[7, 1] = "R";
        //    opTable[7, 2] = "DXB";
        //    opTable[7, 3] = "N";
        //    opTable[7, 4] = "46";

        //    /* BCTR. */
        //    opKey[8] = "BCTR";
        //    opTable[8, 0] = "RR";
        //    opTable[8, 1] = "R";
        //    opTable[8, 2] = "R";
        //    opTable[8, 3] = "N";
        //    opTable[8, 4] = "06";

        //    /* BXH. */
        //    opKey[9] = "BXH";
        //    opTable[9, 0] = "RS";
        //    opTable[9, 1] = "R";
        //    opTable[9, 2] = "R";
        //    opTable[9, 3] = "DB";
        //    opTable[9, 4] = "86";

        //    /* BXLE. */
        //    opKey[10] = "BXLE";
        //    opTable[10, 0] = "RS";
        //    opTable[10, 1] = "R";
        //    opTable[10, 2] = "R";
        //    opTable[10, 3] = "DB";
        //    opTable[10, 4] = "87";

        //    /* C. */
        //    opKey[11] = "C";
        //    opTable[11, 0] = "RX";
        //    opTable[11, 1] = "R";
        //    opTable[11, 2] = "DXB";
        //    opTable[11, 3] = "N";
        //    opTable[11, 4] = "59";

        //    /* CLC. */
        //    opKey[12] = "CLC";
        //    opTable[12, 0] = "RS";
        //    opTable[12, 1] = "DLB";
        //    opTable[12, 2] = "DB";
        //    opTable[12, 3] = "N";
        //    opTable[12, 4] = "D5";

        //    /* CLI. */
        //    opKey[13] = "CLI";
        //    opTable[13, 0] = "SI";
        //    opTable[13, 1] = "DB";
        //    opTable[13, 2] = "I";
        //    opTable[13, 3] = "N";
        //    opTable[13, 4] = "95";

        //    /* CP. */
        //    opKey[14] = "CP";
        //    opTable[14, 0] = "SS";
        //    opTable[14, 1] = "DLB";
        //    opTable[14, 2] = "DLB";
        //    opTable[14, 3] = "N";
        //    opTable[14, 4] = "F9";

        //    /* CR. */
        //    opKey[15] = "CR";
        //    opTable[15, 0] = "RR";
        //    opTable[15, 1] = "R";
        //    opTable[15, 2] = "R";
        //    opTable[15, 3] = "N";
        //    opTable[15, 4] = "19";

        //    /* D. */
        //    opKey[16] = "D";
        //    opTable[16, 0] = "RX";
        //    opTable[16, 1] = "R";
        //    opTable[16, 2] = "DXB";
        //    opTable[16, 3] = "N";
        //    opTable[16, 4] = "5D";

        //    /* DP. */
        //    opKey[17] = "DP";
        //    opTable[17, 0] = "SS";
        //    opTable[17, 1] = "DLB";
        //    opTable[17, 2] = "DLB";
        //    opTable[17, 3] = "N";
        //    opTable[17, 4] = "FD";

        //    /* DR. */
        //    opKey[18] = "DR";
        //    opTable[18, 0] = "RR";
        //    opTable[18, 1] = "R";
        //    opTable[18, 2] = "R";
        //    opTable[18, 3] = "N";
        //    opTable[18, 4] = "1D";

        //    /* ED. */
        //    opKey[19] = "ED";
        //    opTable[19, 0] = "SS";
        //    opTable[19, 1] = "DLB";
        //    opTable[19, 2] = "DB";
        //    opTable[19, 3] = "N";
        //    opTable[19, 4] = "DE";

        //    /* EDMK. */
        //    opKey[20] = "EDMK";
        //    opTable[20, 0] = "SS";
        //    opTable[20, 1] = "DLB";
        //    opTable[20, 2] = "DB";
        //    opTable[20, 3] = "N";
        //    opTable[20, 4] = "DF";

        //    /* L. */
        //    opKey[21] = "L";
        //    opTable[21, 0] = "RX";
        //    opTable[21, 1] = "R";
        //    opTable[21, 2] = "DXB";
        //    opTable[21, 3] = "N";
        //    opTable[21, 4] = "58";

        //    /* LA. */
        //    opKey[22] = "LA";
        //    opTable[22, 0] = "RX";
        //    opTable[22, 1] = "R";
        //    opTable[22, 2] = "DXB";
        //    opTable[22, 3] = "N";
        //    opTable[22, 4] = "41";

        //    /* LM. */
        //    opKey[23] = "LM";
        //    opTable[23, 0] = "RS";
        //    opTable[23, 1] = "R";
        //    opTable[23, 2] = "DXB";
        //    opTable[23, 3] = "N";
        //    opTable[23, 4] = "98";

        //    /* LR. */
        //    opKey[24] = "LR";
        //    opTable[24, 0] = "RR";
        //    opTable[24, 1] = "R";
        //    opTable[24, 2] = "R";
        //    opTable[24, 3] = "N";
        //    opTable[24, 4] = "18";

        //    /* M. */
        //    opKey[25] = "M";
        //    opTable[25, 0] = "RX";
        //    opTable[25, 1] = "R";
        //    opTable[25, 2] = "DXB";
        //    opTable[25, 3] = "N";
        //    opTable[25, 4] = "5C";

        //    /* MP. */
        //    opKey[26] = "MP";
        //    opTable[26, 0] = "SS";
        //    opTable[26, 1] = "DLB";
        //    opTable[26, 2] = "DLB";
        //    opTable[26, 3] = "N";
        //    opTable[26, 4] = "FC";

        //    /* MR. */
        //    opKey[27] = "MR";
        //    opTable[27, 0] = "RR";
        //    opTable[27, 1] = "R";
        //    opTable[27, 2] = "R";
        //    opTable[27, 3] = "N";
        //    opTable[27, 4] = "1C";

        //    /* MVC. */
        //    opKey[28] = "MVC";
        //    opTable[28, 0] = "SS";
        //    opTable[28, 1] = "DLB";
        //    opTable[28, 2] = "DB";
        //    opTable[28, 3] = "N";
        //    opTable[28, 4] = "D2";

        //    /* MVI. */
        //    opKey[29] = "MVI";
        //    opTable[29, 0] = "SI";
        //    opTable[29, 1] = "DB";
        //    opTable[29, 2] = "I";
        //    opTable[29, 3] = "N";
        //    opTable[29, 4] = "92";

        //    /* N. */
        //    opKey[30] = "N";
        //    opTable[30, 0] = "RX";
        //    opTable[30, 1] = "R";
        //    opTable[30, 2] = "DXB";
        //    opTable[30, 3] = "N";
        //    opTable[30, 4] = "54";

        //    /* NR. */
        //    opKey[31] = "NR";
        //    opTable[31, 0] = "RR";
        //    opTable[31, 1] = "R";
        //    opTable[31, 2] = "R";
        //    opTable[31, 3] = "N";
        //    opTable[31, 4] = "14";

        //    /* O. */
        //    opKey[32] = "O";
        //    opTable[32, 0] = "RX";
        //    opTable[32, 1] = "R";
        //    opTable[32, 2] = "DXB";
        //    opTable[32, 3] = "N";
        //    opTable[32, 4] = "56";

        //    /* OR. */
        //    opKey[33] = "OR";
        //    opTable[33, 0] = "RR";
        //    opTable[33, 1] = "R";
        //    opTable[33, 2] = "R";
        //    opTable[33, 3] = "N";
        //    opTable[33, 4] = "16";

        //    /* PACK. */
        //    opKey[34] = "PACK";
        //    opTable[34, 0] = "SS";
        //    opTable[34, 1] = "DLB";
        //    opTable[34, 2] = "DLB";
        //    opTable[34, 3] = "N";
        //    opTable[34, 4] = "F2";

        //    /* S. */
        //    opKey[35] = "S";
        //    opTable[35, 0] = "RX";
        //    opTable[35, 1] = "R";
        //    opTable[35, 2] = "DXB";
        //    opTable[35, 3] = "N";
        //    opTable[35, 4] = "5B";

        //    /* SP. */
        //    opKey[36] = "SP";
        //    opTable[36, 0] = "SS";
        //    opTable[36, 1] = "DLB";
        //    opTable[36, 2] = "DLB";
        //    opTable[36, 3] = "N";
        //    opTable[36, 4] = "FB";

        //    /* SR. */
        //    opKey[37] = "SR";
        //    opTable[37, 0] = "RR";
        //    opTable[37, 1] = "R";
        //    opTable[37, 2] = "R";
        //    opTable[37, 3] = "N";
        //    opTable[37, 4] = "1B";

        //    /* ST. */
        //    opKey[38] = "ST";
        //    opTable[38, 0] = "RX";
        //    opTable[38, 1] = "R";
        //    opTable[38, 2] = "DXB";
        //    opTable[38, 3] = "N";
        //    opTable[38, 4] = "50";

        //    /* STM. */
        //    opKey[39] = "STM";
        //    opTable[39, 0] = "RS";
        //    opTable[39, 1] = "R";
        //    opTable[39, 2] = "DXB";
        //    opTable[39, 3] = "N";
        //    opTable[39, 4] = "90";

        //    /* UNPK. */
        //    opKey[40] = "UNPK";
        //    opTable[40, 0] = "SS";
        //    opTable[40, 1] = "DLB";
        //    opTable[40, 2] = "DLB";
        //    opTable[40, 3] = "N";
        //    opTable[40, 4] = "F3";

        //    /* XDECI. */
        //    opKey[41] = "XDECI";
        //    opTable[41, 0] = "RX";
        //    opTable[41, 1] = "R";
        //    opTable[41, 2] = "DXB";
        //    opTable[41, 3] = "N";
        //    opTable[41, 4] = "53";

        //    /* XDECO. */
        //    opKey[42] = "XDECO";
        //    opTable[42, 0] = "RX";
        //    opTable[42, 1] = "R";
        //    opTable[42, 2] = "DXB";
        //    opTable[42, 3] = "N";
        //    opTable[42, 4] = "52";

        //    /* XPRNT. */
        //    opKey[43] = "XPRNT";
        //    opTable[43, 0] = "X";
        //    opTable[43, 1] = "DXB";
        //    opTable[43, 2] = "L";
        //    opTable[43, 3] = "N";
        //    opTable[43, 4] = "";

        //    /* XREAD. */
        //    opKey[44] = "XREAD";
        //    opTable[44, 0] = "X";
        //    opTable[44, 1] = "A";
        //    opTable[44, 2] = "L";
        //    opTable[44, 3] = "N";
        //    opTable[44, 4] = "";

        //    /* ZAP. */
        //    opKey[45] = "ZAP";
        //    opTable[45, 0] = "SS";
        //    opTable[45, 1] = "DLB";
        //    opTable[45, 2] = "DLB";
        //    opTable[45, 3] = "N";
        //    opTable[45, 4] = "F8";

        //    Array.Sort(opKey);
        //}

        /******************************************************************************************
         * 
         * Name:        GetObjectCode      
         * 
         * Author(s):   Andrew Hamilton
         *              Travis Hunt
         *              
         *              
         * Input:          
         * Return:      
         * Description: 
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
         *              
         * Input:       A candidate string to be determined whether or not it is a valid opcode.      
         * Return:      The method will return the index of the given opcode if it is found in the 
         *              machine op table, or -1 if it is a invalid opcode.
         * Description: This method will determine whether or not a given string is a valid 
         *              instruction.
         *              
         *              
         *****************************************************************************************/
        public static int IsOpcode(string opcode)
        {
            int index;
            index = Array.BinarySearch(opKey, opcode);

            if (index >= 0)
                return index;

            return -1;
        }

    }
