using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryTest;

/**************************************************************************************************
 * 
 * Name: PSWTest
 * 
 * ================================================================================================
 * 
 * Description: This class contains several methods for testing the functionality and correctness
 *              of the PSW class.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/28/2014    JMB     Created class, members, and methods.
 * 03/29/2014    JMB     Updated to work with the AssistByte modifications.
 * 04/03/2014    JMB     Corrected variables to conform to standard.
 * 04/05/2014    JMB     Reordered methods to conform to standards.
 * 04/06/2014    JMB     Updated certain integer values to be unsigned, which is more appropriate
 *                          to their roles and uses.
 *                      
 *************************************************************************************************/

namespace MemoryTest
{
    class PSWTest
    {
        /* Constants. */
        private const int DEFAULT_INVALID_INT = -128;


        /* Private members. */
        PSW systemPSW;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        DisplayPSWContents
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method desplays the instruction address and the condition code.       
         * 
         *****************************************************************************************/
        public void DisplayPSWContents()
        {
            string tempLOC;
            uint cc;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                      PSW Contents                      ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the instruction address and condition code values. */
            tempLOC = systemPSW.GetBytesString(0, 2);
            cc = systemPSW.GetCondCodeInt();

            Console.WriteLine("Instruction Address: {0} \nCondition Code: {1}", 
                        tempLOC, 
                        cc.ToString());
        }

        /******************************************************************************************
         * 
         * Name:        GetBytesTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the GetBytes method.         
         * 
         *****************************************************************************************/
        public void GetBytesTest()
        {
            AssistByte[] myBytes;
            uint end;
            uint j = 0;
            uint start;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                  Testing PSW GetBytes                  ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the starting index. */
            Console.WriteLine("Starting Index: ");
            start = Convert.ToUInt32(Console.ReadLine());

            /* Get the ending index. */
            Console.WriteLine("Ending Index: ");
            end = Convert.ToUInt32(Console.ReadLine());

            /* Retrieve the bytes from the instruction address. */
            myBytes = new AssistByte[end - start + 1];
            myBytes = systemPSW.GetBytes(start, end);

            if (myBytes != null)
            {
                for (uint i = start; i <= end; i++)
                {
                    Console.WriteLine("Byte Address: {0} \t Byte Value: {1}",
                                  i.ToString("X").PadLeft(6, '0'),
                                  myBytes[j].GetHex());
                    j++;
                }
            }

            else
                Console.WriteLine("Error: An error was detected by GetBytes.");
        }

        /******************************************************************************************
         * 
         * Name:        GetBytesIntTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the GetBytesInt method.
         * 
         *****************************************************************************************/
        public void GetBytesIntTest()
        {
            int result;
            uint end;
            uint start;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                 Testing PSW GetBytesInt                ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the starting index. */
            Console.WriteLine("Starting Index: ");
            start = Convert.ToUInt32(Console.ReadLine());

            /* Get the ending index. */
            Console.WriteLine("Ending Index: ");
            end = Convert.ToUInt32(Console.ReadLine());

            /* Get the integer value of the bytes. */
            result = systemPSW.GetBytesInt(start, end);

            Console.WriteLine("First Byte Index: {0} \n Last Byte Index: {1} \n" +
                              "Byte Int Value: {2}",
                              start.ToString(),
                              end.ToString(),
                              result.ToString());
        }

        /******************************************************************************************
         * 
         * Name:        GetBytesStringTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the GetBytesString method.
         *           
         *****************************************************************************************/
        public void GetBytesStringTest()
        {
            string result;
            uint end;
            uint start;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                Testing PSW GetBytesString              ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the starting index. */
            Console.WriteLine("Starting Index: ");
            start = Convert.ToUInt32(Console.ReadLine());

            /* Get the ending index. */
            Console.WriteLine("Ending Index: ");
            end = Convert.ToUInt32(Console.ReadLine());

            /* Retrieve the hexadecimal representation of the bytes. */
            result = systemPSW.GetBytesString(start, end);

            if (result != null)
            {
                Console.WriteLine("First Byte Index: {0} \n Last Byte Index: {1} \n" +
                              "Bytes: {2}",
                              start.ToString(),
                              end.ToString(),
                              result);
            }

            else
                Console.WriteLine("Error: An error was detected by GetBytesString.");
        }

        /******************************************************************************************
         * 
         * Name:        GetCondCodeIntTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the GetCondCodeInt method.
         * 
         *****************************************************************************************/
        public void GetCondCodeIntTest()
        {
            uint result;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("               Testing PSW GetCondCodeInt               ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the integer value of the condition code. */
            result = systemPSW.GetCondCodeInt();

            Console.WriteLine("Condition Code: {0}", result.ToString());
        }

        /******************************************************************************************
         * 
         * Name:        Initialize
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method initializes the PSW to a default state.
         * 
         *****************************************************************************************/
        public void Initialize()
        {
            systemPSW = new PSW();
        }

        /******************************************************************************************
         * 
         * Name:        SetByteIntTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetByte method given a single integer value.   
         * 
         *****************************************************************************************/
        public void SetByteIntTest()
        {
            bool result;
            int value;
            string temp;
            uint index;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                Testing PSW SetByte (int)               ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the index of the byte in the instruction address. */
            Console.WriteLine("Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the new integer value. */
            Console.WriteLine("New Byte Value (integer): ");
            value = Convert.ToInt32(Console.ReadLine());

            /* Replace the integer value of the byte. */
            result = systemPSW.SetByte(index, value);
            temp = systemPSW.GetBytesString(0, 2);

            if (result != false && temp != null)
            {
                Console.WriteLine("Byte Index: {0} \nNew Byte Values: {1}",
                              index.ToString(),
                              temp);
            }

            else
                Console.WriteLine("Error: An error was detected by SetByte or GetBytesString.");
        }

        /******************************************************************************************
         * 
         * Name:        SetByteStringTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetByte method given a single hexadecimal string.        
         * 
         *****************************************************************************************/
        public void SetByteStringTest()
        {
            bool result;
            string temp;
            string value;
            uint index;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("               Testing PSW SetByte (string)             ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the index of the byte in the instruction address. */
            Console.WriteLine("Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the new hexadecimal value. */
            Console.WriteLine("New Byte Value (hexadecimal): ");
            value = Console.ReadLine();

            /* Replace the byte's hexadecimal value. */
            result = systemPSW.SetByte(index, value);
            temp = systemPSW.GetBytesString(0, 2);

            if (result != false && temp != null)
            {
                Console.WriteLine("Byte Index: {0} \nNew Byte Values: {1}",
                              index.ToString(),
                              temp);
            }

            else
                Console.WriteLine("Error: An error was detected by SetByte or GetBytesString.");
        }

        /******************************************************************************************
         * 
         * Name:        SetBytesContiguousIntsTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetBytes method given a set of integer vlaues to
         *              be placed in a series of contiguous bytes in the instruction address.    
         * 
         *****************************************************************************************/
        public void SetBytesContiguousIntsTest()
        {
            bool result;
            int[] values;
            string temp;
            uint index;
            uint numValues;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("               Testing PSW SetBytes (ints)              ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the starting index in the instruction address. */
            Console.WriteLine("Starting Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the number of bytes to change. */
            Console.WriteLine("Number of bytes to change: ");
            numValues = Convert.ToUInt32(Console.ReadLine());

            /* Get the new integer values. */
            values = new int[numValues];
            for (uint i = 0; i < numValues; i++)
            {
                Console.WriteLine("New Value (integer) #{0}: ", i + 1);
                values[i] = Convert.ToInt32(Console.ReadLine());
            }

            /* Replace the integer values of the bytes in the instruction address. */
            result = systemPSW.SetBytes(index, values);

            if (result != false)
            {
                temp = systemPSW.GetBytesString(0, 2);
                Console.WriteLine("New value: {0}", temp);
            }

            else
                Console.WriteLine("Error: An error was detected by SetBytes.");
        }

        /******************************************************************************************
         * 
         * Name:        SetBytesContiguousStringsTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetBytes method given a set of hexadecimal strings
         *              to be placed in a series of contiguous bytes.  
         * 
         *****************************************************************************************/
        public void SetBytesContiguousStringsTest()
        {
            bool result;
            string temp;
            string[] values;
            uint index;
            uint numValues;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("              Testing PSW SetBytes (strings)            ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the starting index in the instruction address. */
            Console.WriteLine("Starting Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the number of bytes to change. */
            Console.WriteLine("Number of bytes to change: ");
            numValues = Convert.ToUInt32(Console.ReadLine());

            /* Get the new hexadecimal values. */
            values = new string[numValues];
            for (uint i = 0; i < numValues; i++)
            {
                Console.WriteLine("New Value (integer) #{0}: ", i + 1);
                values[i] = Console.ReadLine();
            }

            /* Replace the hexadecimal values in the instruction address. */
            result = systemPSW.SetBytes(index, values);

            if (result != false)
            {
                temp = systemPSW.GetBytesString(0, 2);
                Console.WriteLine("New value: {0}", temp);
            }

            else
                Console.WriteLine("Error: An error was detected by SetBytes.");
        }

        /******************************************************************************************
         * 
         * Name:        SetCondCodeIntTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetCondCode method given a single integer value.
         * 
         *****************************************************************************************/
        public void SetCondCodeIntTest()
        {
            bool result;
            string temp;
            uint value;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("              Testing PSW SetCondCode (int)             ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the new condition code integer value. */
            Console.WriteLine("New Condition Code Value (integer): ");
            value = Convert.ToUInt32(Console.ReadLine());

            /* Replace the condition code value. */
            result = systemPSW.SetCondCode(value);

            if (result != false)
            {
                temp = systemPSW.GetCondCodeInt().ToString();
                Console.WriteLine("Condition Code: {0}", temp);
            }

            else
                Console.WriteLine("Error: An error was detected by SetCondCode.");
        }

        /******************************************************************************************
         * 
         * Name:        SetCondCodeBitsTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetCondCode method given the high and low bits.
         * 
         *****************************************************************************************/
        public void SetCondCodeBitsTest()
        {
            bool result;
            string temp;
            uint high;
            uint low;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("              Testing PSW SetCondCode (ints)            ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the new high bit. */
            Console.WriteLine("New Condition Code High Bit: ");
            high = Convert.ToUInt32(Console.ReadLine());

            /* Get the new low bit. */
            Console.WriteLine("New Condition Code Low Bit: ");
            low = Convert.ToUInt32(Console.ReadLine());

            /* Replace the high and low bits. */
            result = systemPSW.SetCondCode(high, low);

            if (result != false)
            {
                temp = systemPSW.GetCondCodeInt().ToString();
                Console.WriteLine("Condition Code: {0}", temp);
            }

            else
                Console.WriteLine("Error: An error was detected by SetCondCode.");
        }

    }

}
