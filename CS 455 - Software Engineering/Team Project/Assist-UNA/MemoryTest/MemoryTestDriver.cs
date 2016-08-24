using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryTest;

/**************************************************************************************************
 * 
 * Name: MemoryTestDriver
 * 
 * ================================================================================================
 * 
 * Description: This class contains the program for testing the correctness of the Data classes 
 *              (Memory, Register, and PSW).
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/28/2014    JMB     Created class, members, and methods.
 * 04/03/2014    JMB     Corrected variables to conform to standard.
 * 04/05/2014    JMB     Reordered methods to conform to standards.
 * 04/06/2014    JMB     Updated certain integer values to be unsigned, which is more appropriate
 *                          to their roles and uses.
 *                      
 *************************************************************************************************/

namespace MemoryTest
{
    class MemoryTestDriver
    {
        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        Menu
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      The menu selection is a string.
         * Description: This method displays a menu and prompts the user of a selection.
         * 
         *****************************************************************************************/
        static string Menu()
        {
            string choice;

            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("                            Main Menu                             ");
            Console.WriteLine("------------------------------------------------------------------");

            Console.WriteLine("   Memory Testing    |   Register Testing   |      PSW Testing    ");
            Console.WriteLine("---------------------|----------------------|---------------------");
            Console.WriteLine("01) Display Contents | 18) Display Contents | 28) Display Contents");
            Console.WriteLine("02) Get Byte         | 19) Get Bytes        | 29) Get Bytes       ");
            Console.WriteLine("03) Get Bytes        | 20) Get Byte Hex     | 30) Get Bytes Int   ");
            Console.WriteLine("04) Get Byte Hex     | 21) Get Byte Int     | 31) Get Bytes String");
            Console.WriteLine("05) Get Byte Int     | 22) Get Bytes Int    | 32) Get CC Int      ");
            Console.WriteLine("06) Get Bytes Int    | 23) Get Bytes String | 33) Set Byte Int    ");
            Console.WriteLine("07) Get Bytes String | 24) Set Byte Int     | 34) Set Byte String ");
            Console.WriteLine("08) Get EBCDIC Chars | 25) Set Byte String  | 35) Set Ints Contig.");
            Console.WriteLine("09) Get Memory Size  | 26) Set Ints Contig. | 36) Set Strs Contig.");
            Console.WriteLine("10) Set Byte Int     | 27) Set Strs Contig. | 37) Set CC Int      ");
            Console.WriteLine("11) Set Byte String  |                      | 38) Set CC Bits     ");
            Console.WriteLine("12) Set Ints Contig. |----------------------|---------------------");
            Console.WriteLine("13) Set Ints Noncon. |         KEY          |                     ");
            Console.WriteLine("14) Set Strs Contig. |                      |                     ");
            Console.WriteLine("15) Set Strs Noncon. | Contig. = Contiguous | 39) Quit            ");
            Console.WriteLine("16) Lock Byte        | Noncon. = Non-       |                     ");
            Console.WriteLine("17) Unlock Byte      |           Contiguous |                     ");
            Console.WriteLine("---------------------|----------------------|---------------------");

            Console.WriteLine("Choice: ");
            choice = Console.ReadLine();

            return choice;
        }

        /******************************************************************************************
         * 
         * Name:        Main
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       The args are command line parameters.
         * Return:      N/A
         * Description: This is the main driver method.
         * 
         *****************************************************************************************/
        static void Main(string[] args)
        {
            int sel;
            MemoryTest mt = new MemoryTest();
            PSWTest pt = new PSWTest();
            RegisterTest rt = new RegisterTest();
            string menuSel;
            string size;

            /* Get the memory size from the user. */
            Console.WriteLine("Memory size [256, 9999]: ");
            size = Console.ReadLine();

            /* Initialize the test objects. */
            mt.Initialize(Convert.ToUInt32(size));
            pt.Initialize();
            rt.Initialize();

            /* Loop until the user quits. */
            do
            {
                /* Get the user's menu selection. */
                Console.Clear();
                menuSel = Menu();

                if (menuSel != "")
                    sel = Convert.ToInt32(menuSel);

                else
                    sel = 0;

                Console.Clear();

                /* Memory Testing. */
                if (sel > 0 && sel < 18)
                    MemoryTestHandler(sel, mt);

                /* Register Testing. */
                else if (sel > 17 && sel < 28)
                    RegisterTestHandler(sel, rt);

                /* PSW Testing. */
                else if (sel > 27 && sel < 39)
                    PSWTestHandler(sel, pt);

                Console.ReadLine();
            } while (menuSel != "39");
        }

        /******************************************************************************************
         * 
         * Name:        MemoryTestHandler
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       Sel is an integer, and mt is a MemoryTest object.
         * Return:      N/A
         * Description: This method selects the appropriate MemoryTest test method given the user's
         *              menu selection ("sel").
         * 
         *****************************************************************************************/
        static void MemoryTestHandler(int sel, MemoryTest mt)
        {
            switch(sel)
            {
                /* Display Contents. */
                case 1:
                    {
                        mt.DisplayMemoryContents();
                        break;
                    }

                /* Get Byte. */
                case 2:
                    {
                        mt.GetByteTest();
                        break;
                    }

                /* Get Bytes. */
                case 3:
                    {
                        mt.GetBytesTest();
                        break;
                    }

                /* Get Byte Hex. */
                case 4:
                    {
                        mt.GetByteHexTest();
                        break;
                    }

                /* Get Byte Int. */
                case 5:
                    {
                        mt.GetByteIntTest();
                        break;
                    }

                /* Get Bytes Int. */
                case 6:
                    {
                        mt.GetBytesIntTest();
                        break;
                    }

                /* Get Bytes String. */
                case 7:
                    {
                        mt.GetBytesStringTest();
                        break;
                    }

                /* Get EBCDIC Chars. */
                case 8:
                    {
                        mt.GetEBCDICTest();
                        break;
                    }

                /* Get Memory Size. */
                case 9:
                    {
                        mt.GetMemorySizeTest();
                        break;
                    }

                /* Set Byte Int. */
                case 10:
                    {
                        mt.SetByteIntTest();
                        break;
                    }
                
                /* Set Byte String. */
                case 11:
                    {
                        mt.SetByteStringTest();
                        break;
                    }

                /* Set Ints Contiguous. */
                case 12:
                    {
                        mt.SetBytesContiguousIntsTest();
                        break;
                    }

                /* Set Ints Noncontiguous. */
                case 13:
                    {
                        mt.SetBytesNoncontiguousIntsTest();
                        break;
                    }

                /* Set Strings Contiguous. */
                case 14:
                    {
                        mt.SetBytesContiguousStringsTest();
                        break;
                    }

                /* Set Strings Noncontiguous. */
                case 15:
                    {
                        mt.SetBytesNoncontiguousStringsTest();
                        break;
                    }

                /* Lock Byte. */
                case 16:
                    {
                        mt.LockByteTest();
                        break;
                    }

                /* Unlock Byte. */
                case 17:
                    {
                        mt.UnlockByteTest();
                        break;
                    }
            }
        }

        /******************************************************************************************
         * 
         * Name:        PSWTestHandler
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       Sel is an integer, and pt is a PSWTest object.
         * Return:      N/A
         * Description: This method selects the appropriate PSWTest test method given the user's
         *              menu selection ("sel").
         * 
         *****************************************************************************************/
        static void PSWTestHandler(int sel, PSWTest pt)
        {
            switch(sel)
            {
                /* Display Contents. */
                case 28:
                    {
                        pt.DisplayPSWContents();
                        break;
                    }

                /* Get Bytes. */
                case 29:
                    {
                        pt.GetBytesTest();
                        break;
                    }

                /* Get Bytes Int. */
                case 30:
                    {
                        pt.GetBytesIntTest();
                        break;
                    }

                /* Get Bytes String. */
                case 31:
                    {
                        pt.GetBytesStringTest();
                        break;
                    }

                /* Get Condition Code Int. */
                case 32:
                    {
                        pt.GetCondCodeIntTest();
                        break;
                    }

                /* Set Byte Int. */
                case 33:
                    {
                        pt.SetByteIntTest();
                        break;
                    }

                /* Set Byte String. */
                case 34:
                    {
                        pt.SetByteStringTest();
                        break;
                    }
                
                /* Set Ints Contiguous. */
                case 35:
                    {
                        pt.SetBytesContiguousIntsTest();
                        break;
                    }

                /* Set Strings Contiguous. */
                case 36:
                    {
                        pt.SetBytesContiguousStringsTest();
                        break;
                    }

                /* Set Condition Code Int. */
                case 37:
                    {
                        pt.SetCondCodeIntTest();
                        break;
                    }

                /* Set Condition Code Bits. */
                case 38:
                    {
                        pt.SetCondCodeBitsTest();
                        break;
                    }
            }
        }

        /******************************************************************************************
         * 
         * Name:        RegisterTestHandler
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       Sel is an integer, and rt is a RegisterTest object.
         * Return:      N/A
         * Description: This method selects the appropriate RegisterTest test method given the 
         *              user's menu selection ("sel").
         * 
         *****************************************************************************************/
        static void RegisterTestHandler(int sel, RegisterTest rt)
        {
            switch(sel)
            {
                /* Display Contents. */
                case 18:
                    {
                        rt.DisplayRegisterContents();
                        break;
                    }

                /* Get Bytes. */
                case 19:
                    {
                        rt.GetBytesTest();
                        break;
                    }

                /* Get Byte Hex. */
                case 20:
                    {
                        rt.GetByteHexTest();
                        break;
                    }

                /* Get Byte Int. */
                case 21:
                    {
                        rt.GetByteIntTest();
                        break;
                    }

                /* Get Bytes Int. */
                case 22:
                    {
                        rt.GetBytesIntTest();
                        break;
                    }

                /* Get Bytes String. */
                case 23:
                    {
                        rt.GetBytesStringTest();
                        break;
                    }

                /* Set Byte Int. */
                case 24:
                    {
                        rt.SetByteIntTest();
                        break;
                    }

                /* Set Byte String. */
                case 25:
                    {
                        rt.SetByteStringTest();
                        break;
                    }

                /* Set Ints Contiguous. */
                case 26:
                    {
                        rt.SetBytesContiguousIntsTest();
                        break;
                    }

                /* Set Strings Contiguous. */
                case 27:
                    {
                        rt.SetBytesContiguousStringsTest();
                        break;
                    }
            }
        }

    }

}
