using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryTest;

/**************************************************************************************************
 * 
 * Name: MemoryTest
 * 
 * ================================================================================================
 * 
 * Description: This class contains several methods for testing the functionality and correctness
 *              of the Memory class.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/28/2014    JMB     Created class, members, and methods.
 * 03/29/2014    JMB     Updated to work with the AssistByte modifications.
 * 04/03/2014    JMB     Corrected variables to conform to standard.
 * 04/06/2014    JMB     Updated certain integer values to be unsigned, which is more appropriate
 *                          to their roles and uses.
 *                      
 *************************************************************************************************/

namespace MemoryTest
{
    class MemoryTest
    {
        /* Constants. */
        private const int DEFAULT_INVALID_INT = -128;


        /* Private members. */
        Memory systemMem;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        DisplayMemoryContents
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will display all the contents in Memory, including the addresses
         *              actual contents (bytes), and the EBCDIC character representations.
         *           
         *****************************************************************************************/
        public void DisplayMemoryContents()
        {
            string temp;
            uint i = 0;
            uint sz = systemMem.GetMemorySize();
            
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine(" Addr          Memory Contents            EBCDIC Chars  ");
            Console.WriteLine("--------------------------------------------------------");

            /* Traverse the entire memory contents. */
            while (i < sz)
            {
                /* Format address. */
                temp = i.ToString("X").PadLeft(6, '0');
                temp += " ";

                /* Format memory contents. */
                if (i + 15 < sz)
                    temp += systemMem.GetBytesString(i, i + 15);
                else
                    temp += systemMem.GetBytesString(i, sz - 1).PadRight(32, ' ');

                temp += " ";

                /* Format EBCDIC character representation. */
                if (i + 15 < sz)
                    temp += systemMem.GetEBCDIC(i, i + 15);
                else
                    temp += systemMem.GetEBCDIC(i, sz - 1).PadRight(16, ' ');

                Console.WriteLine(temp);

                i += 16;
            }
        }

        /******************************************************************************************
         * 
         * Name:        GetByteTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the functionality of the GetByte method.
         * 
         *****************************************************************************************/
        public void GetByteTest()
        {
            AssistByte myByte;
            uint index;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                 Testing Memory GetByte                 ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the address of a byte in memory. */
            Console.WriteLine("Address: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Retrieve the byte from memory. */
            myByte = systemMem.GetByte(index);

            Console.WriteLine("Byte Address: {0} \t Byte Value: {1}",
                              index.ToString("X").PadLeft(6, '0'),
                              myByte.GetHex());
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
            Console.WriteLine("                 Testing Memory GetBytes                ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the inclusive beginning and ending addresses. */
            Console.WriteLine("Starting Address: ");
            start = Convert.ToUInt32(Console.ReadLine());

            Console.WriteLine("Ending Address: ");
            end = Convert.ToUInt32(Console.ReadLine());

            /* Retrieve the bytes from memory. */
            myBytes = new AssistByte[end - start + 1];
            myBytes = systemMem.GetBytes(start, end);

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
         * Name:        GetByteHexTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the GetByteHex method.
         *           
         *****************************************************************************************/
        public void GetByteHexTest()
        {
            string result;
            uint index;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                Testing Memory GetByteHex               ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the address of a byte in memory. */
            Console.WriteLine("Address: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Retrieve the hexadecimal representation of the byte. */
            result = systemMem.GetByteHex(index);

            if (result != null)
            {
                Console.WriteLine("Byte Address: {0} \t Byte Hex Value: {1}",
                              index.ToString("X").PadLeft(6, '0'),
                              result);
            }
            
            else
                Console.WriteLine("Error: An error was detected by GetByteHex.");
        }

        /******************************************************************************************
         * 
         * Name:        GetByteIntTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the GetByteInt method.
         * 
         *****************************************************************************************/
        public void GetByteIntTest()
        {
            int result;
            uint index;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                Testing Memory GetByteInt               ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the address of a byte in memory. */
            Console.WriteLine("Address: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Retrieve the integer value of the byte. */
            result = systemMem.GetByteInt(index);

            if (result != DEFAULT_INVALID_INT)
            {
                Console.WriteLine("Byte Address: {0} \t Byte Int Value: {1}",
                              index.ToString("X").PadLeft(6, '0'),
                              result.ToString());
            }

            else
                Console.WriteLine("Error: An error was detected by GetByteInt.");
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
            Console.WriteLine("               Testing Memory GetBytesInt               ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the inclusive beginning and ending addresses. */
            Console.WriteLine("Starting Address: ");
            start = Convert.ToUInt32(Console.ReadLine());

            Console.WriteLine("Ending Address: ");
            end = Convert.ToUInt32(Console.ReadLine());

            /* Only allow for four bytes (one fullword) to be used. */
            if ((end - start) > 3)
                return;

            /* Retrieve the integer value of the bytes. */
            result = systemMem.GetBytesInt(start, end);

             Console.WriteLine("First Byte Address: {0} \n Last Byte Address: {1} \n" +
                              "Byte Int Value: {2}",
                              start.ToString("X").PadLeft(6, '0'),
                              end.ToString("X").PadLeft(6, '0'),
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
            Console.WriteLine("              Testing Memory GetBytesString             ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the inclusive beginning and ending addresses. */
            Console.WriteLine("Starting Address: ");
            start = Convert.ToUInt32(Console.ReadLine());

            Console.WriteLine("Ending Address: ");
            end = Convert.ToUInt32(Console.ReadLine());

            /* Retrieve the string of hexadecimal values. */
            result = systemMem.GetBytesString(start, end);

            if (result != null)
            {
                Console.WriteLine("First Byte Address: {0} \n Last Byte Address: {1} \n" +
                              "Bytes: {2}",
                              start.ToString("X").PadLeft(6, '0'),
                              end.ToString("X").PadLeft(6, '0'),
                              result);
            }

            else
                Console.WriteLine("Error: An error was detected by GetBytesString.");
        }

        /******************************************************************************************
         * 
         * Name:        GetEBCDICTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the GetEBCDIC method.    
         * 
         *****************************************************************************************/
        public void GetEBCDICTest()
        {
            string result;
            uint end;
            uint start;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                Testing Memory GetEBCDIC                ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the inclusive beginning and ending addresses. */
            Console.WriteLine("Starting Address: ");
            start = Convert.ToUInt32(Console.ReadLine());

            Console.WriteLine("Ending Address: ");
            end = Convert.ToUInt32(Console.ReadLine());

            /* Retrieve the EBCDIC character representations. */
            result = systemMem.GetEBCDIC(start, end);

            if (result != null)
            {
                Console.WriteLine("First Byte Address: {0} \n Last Byte Address: {1} \n" +
                              "EBCDIC Chars: {2}",
                              start.ToString("X").PadLeft(6, '0'),
                              end.ToString("X").PadLeft(6, '0'),
                              result);
            }

            else
                Console.WriteLine("Error: An error was detected by GetEBCDIC.");
        }

        /******************************************************************************************
         * 
         * Name:        GetMemorySizeTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the GetMemorySize method.
         *           
         *****************************************************************************************/
        public void GetMemorySizeTest()
        {
            uint sz;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("              Testing Memory GetMemorySize              ");
            Console.WriteLine("--------------------------------------------------------");

            /* Retrieve the size of memory in bytes. */
            sz = systemMem.GetMemorySize();

            Console.WriteLine("Memory Size: {0} bytes", sz.ToString());
        }

        /******************************************************************************************
         * 
         * Name:        Initialize
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method initializes the Memory to a default state.
         *           
         *****************************************************************************************/
        public void Initialize()
        {
            systemMem = new Memory();
        }

        /******************************************************************************************
         * 
         * Name:        Initializes
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       The size is an unsigned integer.
         * Return:      N/A
         * Description: This method initializes the Memory to a user-specified size ("size").          
         * 
         *****************************************************************************************/
        public void Initialize(uint size)
        {
            systemMem = new Memory(size);
        }

        /******************************************************************************************
         * 
         * Name:        LockByteTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the LockByte method.
         * 
         *****************************************************************************************/
        public void LockByteTest()
        {
            bool result;
            uint index;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                 Testing Memory LockByte                ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the address of a byte in memory. */
            Console.WriteLine("Byte Address: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Lock the byte. */
            result = systemMem.LockByte(index);

            if (result)
            {
                Console.WriteLine("Byte Address: {0} \n Lock Status: {1}",
                             index.ToString("X").PadLeft(6, '0'),
                             systemMem.GetByte(index).IsLocked().ToString());
            }

            else
                Console.WriteLine("Error: There was an error detected by LockByte.");
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
            string newVal;
            string temp;
            uint index;
            
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("              Testing Memory SetByte (int)              ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the address of a byte in memory. */
            Console.WriteLine("Address: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the new integer value. */
            Console.WriteLine("New Byte Value (integer): ");
            value = Convert.ToInt32(Console.ReadLine());

            /* Keep the previous value. Set the new value. Retrieve the hexadecimal of the byte. */
            temp = systemMem.GetByteHex(index);
            result = systemMem.SetByte(index, value);
            newVal = systemMem.GetByteHex(index);

            if (result != false && temp != null)
            {
                Console.WriteLine("Byte Address: {0} \nOriginal Byte Value: {1} \n" +
                              "New Byte Value: {2}",
                              index.ToString("X").PadLeft(6, '0'),
                              temp,
                              newVal);
            }

            else
                Console.WriteLine("Error: An error was detected by SetByte and/or GetByteHex.");
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
            string newVal;
            string temp;
            string value;
            uint index;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("             Testing Memory SetByte (string)            ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the address of a byte in memory. */
            Console.WriteLine("Address: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the new hexadecimal value. */
            Console.WriteLine("New Byte Value (hexadecimal): ");
            value = Console.ReadLine();

            /* Keep the old value. Replace the value. Retrieve the new value from the byte. */
            temp = systemMem.GetByteHex(index);
            result = systemMem.SetByte(index, value);
            newVal = systemMem.GetByteHex(index);

            if (result != false && temp != null)
            {
                Console.WriteLine("Byte Address: {0} \nOriginal Byte Value: {1} \n" +
                              "New Byte Value: {2}",
                              index.ToString("X").PadLeft(6, '0'),
                              temp,
                              newVal);
            }

            else
                Console.WriteLine("Error: An error was detected by SetByte and/or GetByteHex.");
        }

        /******************************************************************************************
         * 
         * Name:        SetBytesContiguousIntsTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetBytes method given a set of integers to be
         *              placed into a contiguous set of bytes.       
         * 
         *****************************************************************************************/
        public void SetBytesContiguousIntsTest()
        {
            bool result;
            int[] values;
            string newVal;
            uint index;
            uint numValues;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("             Testing Memory SetBytes (ints)             ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the address of the beginning byte in memory. */
            Console.WriteLine("Starting Address: ");
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

            /* Replace the byte values in memory. */
            result = systemMem.SetBytes(index, values);

            if (result != false)
            {
                for (uint i = index; i < (index + values.Length); i++)
                {
                    newVal = systemMem.GetByteHex(i);

                    Console.WriteLine("Byte Address: {0} \nNew Byte Value: {1} \n",
                             i.ToString("X").PadLeft(6, '0'),
                             newVal);
                }
                    
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
         *              to be placed in contiguous bytes.         
         * 
         *****************************************************************************************/
        public void SetBytesContiguousStringsTest()
        {
            bool result;  
            string[] values;
            string newVal;
            uint index;
            uint numValues;   

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("            Testing Memory SetBytes (strings)           ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the address of the starting byte in memory. */
            Console.WriteLine("Starting Address: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the number of bytes to change. */
            Console.WriteLine("Number of bytes to change: ");
            numValues = Convert.ToUInt32(Console.ReadLine());

            /* Get the new hexadecimal values. */
            values = new string[numValues];
            for (uint i = 0; i < numValues; i++)
            {
                Console.WriteLine("New Value (hexadecimal) #{0}: ", i + 1);
                values[i] = Console.ReadLine();
            }

            /* Replace the values in memory. */
            result = systemMem.SetBytes(index, values);

            if (result != false)
            {
                for (uint i = index; i < (index + values.Length); i++)
                {
                    newVal = systemMem.GetByteHex(i);

                    Console.WriteLine("Byte Address: {0} \nNew Byte Value: {1} \n",
                                i.ToString("X").PadLeft(6, '0'),
                                newVal);
                }

            }

            else
                Console.WriteLine("Error: An error was detected by SetBytes.");
        }

        /******************************************************************************************
         * 
         * Name:        SetBytesNoncontiguousInts
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetBytes method given a set of integer values to
         *              be placed in a noncontiguous series of bytes.
         * 
         *****************************************************************************************/
        public void SetBytesNoncontiguousIntsTest()
        {
            bool result;
            int[] values;
            string newVal;
            uint currentIndex = 0;
            uint[] indices;
            uint numValues; 

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("             Testing Memory SetBytes (ints)             ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the number of bytes to change. */
            Console.WriteLine("Number of bytes to change: ");
            numValues = Convert.ToUInt32(Console.ReadLine());

            /* Get the addresses and the new integer values. */
            indices = new uint[numValues];
            values = new int[numValues];
            for (uint i = 0; i < numValues; i++)
            {
                Console.WriteLine("Address #{0}: ", i + 1);
                indices[i] = Convert.ToUInt32(Console.ReadLine());

                Console.WriteLine("New Value (integer) #{0}: ", i + 1);
                values[i] = Convert.ToInt32(Console.ReadLine());
            }

            /* Replace the values in memory. */
            result = systemMem.SetBytes(indices, values);

            if (result != false)
            {
                for (uint i = 0; i < indices.Length; i++) 
                {
                    currentIndex = indices[i];
                    newVal = systemMem.GetByteHex(currentIndex);
                    
                    Console.WriteLine("Byte Address: {0} \nNew Byte Value: {1} \n",
                             currentIndex.ToString("X").PadLeft(6, '0'),
                             newVal);                    
                }

            }

            else
                Console.WriteLine("Error: An error was detected by SetBytes.");
        }

        /******************************************************************************************
         * 
         * Name:        SetBytesNoncontiguousStringsTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetBytes method given a set of hexadecimal strings
         *              to be placed in a noncontiguous series of bytes. 
         * 
         *****************************************************************************************/
        public void SetBytesNoncontiguousStringsTest()
        {
            bool result;
            string newVal;
            string[] values;
            uint currentIndex = 0;
            uint[] indices;
            uint numValues;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("            Testing Memory SetBytes (strings)           ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the number of bytes to change. */
            Console.WriteLine("Number of bytes to change: ");
            numValues = Convert.ToUInt32(Console.ReadLine());

            /* Get the addresses and hexadecimal values. */
            indices = new uint[numValues];
            values = new string[numValues];
            for (uint i = 0; i < numValues; i++)
            {
                Console.WriteLine("Address #{0}: ", i + 1);
                indices[i] = Convert.ToUInt32(Console.ReadLine());

                Console.WriteLine("New Value (hexadecimal) #{0}: ", i + 1);
                values[i] = Console.ReadLine();
            }

            /* Replace the values in memory. */
            result = systemMem.SetBytes(indices, values);

            if (result != false)
            {
                for (uint i = 0; i < indices.Length; i++)
                {
                    currentIndex = indices[i];
                    newVal = systemMem.GetByteHex(currentIndex);

                    Console.WriteLine("Byte Address: {0} \nNew Byte Value: {1} \n",
                             currentIndex.ToString("X").PadLeft(6, '0'),
                             newVal);
                }

            }

            else
                Console.WriteLine("Error: An error was detected by SetBytes.");
        }

        /******************************************************************************************
         * 
         * Name:        UnlockByteTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the UnlockByte method.
         * 
         *****************************************************************************************/
        public void UnlockByteTest()
        {
            bool result;
            uint index;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                Testing Memory UnlockByte               ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the address of a byte in memory. */
            Console.WriteLine("Byte Address: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Unlock the byte. */
            result = systemMem.UnlockByte(index);

            if (result)
            {
                Console.WriteLine("Byte Address: {0} \n Lock Status: {1}",
                             index.ToString("X").PadLeft(6, '0'),
                             systemMem.GetByte(index).IsLocked().ToString());
            }

            else
                Console.WriteLine("Error: There was an error detected by UnlockByte.");
        }

    }

}