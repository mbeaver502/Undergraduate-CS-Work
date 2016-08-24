using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryTest;

/**************************************************************************************************
 * 
 * Name: RegisterTest
 * 
 * ================================================================================================
 * 
 * Description: This class contains several methods for testing the functionality and correctness
 *              of the Register class.
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
    class RegisterTest
    {
        /* Constants. */
        private const int DEFAULT_INVALID_INT = -128;
        private const uint NUM_REGISTERS = 16;
        

        /* Private members. */
        Register[] registers;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        DisplayRegisterContents
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method displays the contents of all the registers.
         * 
         *****************************************************************************************/
        public void DisplayRegisterContents()
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                   Register Contents                    ");
            Console.WriteLine("--------------------------------------------------------");

            /* Display the contents of all registers. */
            for (uint i = 0; i < NUM_REGISTERS; i++)
            {
                Console.WriteLine("Register {0}: {1}",
                            i.ToString("X"),
                            registers[i].GetBytesString(0, 3));
            }
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
            string temp = "";
            uint end;
            uint reg;
            uint start;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("                Testing Register GetBytes               ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the register number. */
            Console.WriteLine("Register Number: ");
            reg = Convert.ToUInt32(Console.ReadLine());

            /* Get the starting byte index. */
            Console.WriteLine("Starting Index: ");
            start = Convert.ToUInt32(Console.ReadLine());

            /* Get the ending byte index. */
            Console.WriteLine("Ending Index: ");
            end = Convert.ToUInt32(Console.ReadLine());

            try
            {
                /* Retrieve the bytes from the register. */
                myBytes = new AssistByte[end - start + 1];
                myBytes = registers[reg].GetBytes(start, end);

                if (myBytes != null)
                {
                    Console.WriteLine("Register: {0}", reg);

                    for (uint i = 0; i < myBytes.Length; i++)
                        temp += myBytes[i].GetHex();

                    Console.WriteLine("Bytes: {0}", temp);
                }

                else
                    Console.WriteLine("Error: An error was detected by GetBytes.");
            }

            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");
            }
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
            uint reg;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("               Testing Register GetByteHex              ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the register number. */
            Console.WriteLine("Register Number: ");
            reg = Convert.ToUInt32(Console.ReadLine());

            /* Get the byte index within the register. */
            Console.WriteLine("Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            try
            {
                /* Retrieve the hexadecimal value of the byte. */
                result = registers[reg].GetByteHex(index);

                if (result != null)
                {
                    Console.WriteLine("Register: {0} \nByte Index: {1} \nByte Hex Value: {2}",
                                reg.ToString("X"),
                                index.ToString(),
                                result);
                }

                else
                    Console.WriteLine("Error: An error was detected by GetByteHex.");
            }
            
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");
            }
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
            uint reg;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("               Testing Register GetByteInt              ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the register number. */
            Console.WriteLine("Register Number: ");
            reg = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of a byte within the register. */
            Console.WriteLine("Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            try
            {
                /* Retrieve the integer value of the byte. */
                result = registers[reg].GetByteInt(index);

                if (result != DEFAULT_INVALID_INT)
                {
                    Console.WriteLine("Register: {0} \nByte Index: {1} \nByte Int Value: {2}",
                                reg.ToString("X"),
                                index.ToString(),
                                result.ToString());
                }

                else
                    Console.WriteLine("Error: An error was detected by GetByteInt.");
            }
            
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");
            }    
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
            uint reg;
            uint start;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("              Testing Register GetBytesInt              ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the register number. */
            Console.WriteLine("Register Number: ");
            reg = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of a starting byte within the register. */
            Console.WriteLine("Starting Index: ");
            start = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of an ending byte within the register. */
            Console.WriteLine("Ending Index: ");
            end = Convert.ToUInt32(Console.ReadLine());

            try
            {
                /* Retrieve the integer value of the bytes. */
                result = registers[reg].GetBytesInt(start, end);

                Console.WriteLine("Register: {0} \n# of Bytes Selected: {1} \n" +
                                  "Bytes Int Value: {2}",
                                  reg.ToString("X"),
                                  (end - start + 1).ToString(),
                                  result.ToString());
            }
            
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");
            }
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
            uint reg;
            uint start;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("             Testing Register GetBytesString            ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the register number. */
            Console.WriteLine("Register Number: ");
            reg = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of a starting byte within the register. */
            Console.WriteLine("Starting Index: ");
            start = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of an ending byte within the register. */
            Console.WriteLine("Ending Index: ");
            end = Convert.ToUInt32(Console.ReadLine());

            try
            {
                /* Retrieve the hexadecimal string of bytes. */
                result = registers[reg].GetBytesString(start, end);

                if (result != null)
                {
                    Console.WriteLine("Register: {0} \nFirst Byte Index: {1} \n" +
                                "Last Byte Index: {2} \nBytes: {3}",
                                reg.ToString("X"),
                                start.ToString(),
                                end.ToString(),
                                result);
                }

                else
                    Console.WriteLine("Error: An error was detected by GetBytesString.");
            }
            
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");
            }
        }

        /******************************************************************************************
         * 
         * Name:        Initialize
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the set of registers to a default state. 
         * 
         *****************************************************************************************/
        public void Initialize()
        {
            /* Create the set of registers. */
            registers = new Register[NUM_REGISTERS];

            /* Initialize the registers. */
            for (uint i = 0; i < NUM_REGISTERS; i++)
                registers[i] = new Register();
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
            uint reg;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("             Testing Register SetByte (int)             ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the register number. */
            Console.WriteLine("Register Number: ");
            reg = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of a byte within the register. */
            Console.WriteLine("Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the new integer value. */
            Console.WriteLine("New Byte Value (integer): ");
            value = Convert.ToInt32(Console.ReadLine());

            try
            {
                /* Keep the old value. Set the new value. Retrieve the new value. */
                temp = registers[reg].GetByteHex(index);
                result = registers[reg].SetByte(index, value);
                newVal = registers[reg].GetByteHex(index);

                if (result != false && temp != null)
                {
                    Console.WriteLine("Register: {0} \nOriginal Byte Value: {1} \n" +
                                  "New Byte Value: {2}",
                                  reg.ToString("X"),
                                  temp,
                                  newVal);
                }

                else
                    Console.WriteLine("Error: An error was detected by SetByte and/or GetByteHex.");
            }
            
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");
            }
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
            uint reg;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("            Testing Register SetByte (string)           ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the register number. */
            Console.WriteLine("Register Number: ");
            reg = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of a byte within the register. */
            Console.WriteLine("Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the new hexadecimal value. */
            Console.WriteLine("New Byte Value (hexadecimal): ");
            value = Console.ReadLine();

            try
            {
                /* Keep the old value. Store the new value. Retrieve the new value. */
                temp = registers[reg].GetByteHex(index);
                result = registers[reg].SetByte(index, value);
                newVal = registers[reg].GetByteHex(index);

                if (result != false && temp != null)
                {
                    Console.WriteLine("Register: {0} \nOriginal Byte Value: {1} \n" +
                                  "New Byte Value: {2}",
                                  reg.ToString("X"),
                                  temp,
                                  newVal);
                }

                else
                    Console.WriteLine("Error: An error was detected by SetByte and/or GetByteHex.");
            }
            
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");
            }
        }

        /******************************************************************************************
         * 
         * Name:        SetBytesContiguousIntsTest
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A
         * Return:      N/A
         * Description: This method will test the SetBytes method given a set of integer values to
         *              be placed in a contiguous series of bytes.
         *           
         *****************************************************************************************/
        public void SetBytesContiguousIntsTest()
        {
            bool result;
            int[] values;
            string temp;
            uint index;
            uint numValues;
            uint reg;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("            Testing Register SetBytes (ints)            ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the register number. */
            Console.WriteLine("Register Number: ");
            reg = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of a starting byte within the register. */
            Console.WriteLine("Starting Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of an ending byte within the register. */
            Console.WriteLine("Number of bytes to change: ");
            numValues = Convert.ToUInt32(Console.ReadLine());

            /* Get the new integer values. */
            values = new int[numValues];
            for (uint i = 0; i < numValues; i++)
            {
                Console.WriteLine("New Value (integer) #{0}: ", i + 1);
                values[i] = Convert.ToInt32(Console.ReadLine());
            }

            try
            {
                /* Replace the integer values. */
                result = registers[reg].SetBytes(index, values);

                if (result != false)
                {
                    temp = registers[reg].GetBytesString(0, 3);

                    Console.WriteLine("Register: {0} \nContents: {1}",
                                reg.ToString("X"),
                                temp);
                }

                else
                    Console.WriteLine("Error: An error was detected by SetBytes.");
            }
            
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");
            }
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
            uint reg;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("           Testing Register SetBytes (strings)          ");
            Console.WriteLine("--------------------------------------------------------");

            /* Get the register number. */
            Console.WriteLine("Register Number: ");
            reg = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of a starting byte within the register. */
            Console.WriteLine("Starting Index: ");
            index = Convert.ToUInt32(Console.ReadLine());

            /* Get the index of an ending byte within the register. */
            Console.WriteLine("Number of bytes to change: ");
            numValues = Convert.ToUInt32(Console.ReadLine());

            /* Get the new hexadecimal values. */
            values = new string[numValues];
            for (uint i = 0; i < numValues; i++)
            {
                Console.WriteLine("New Value (hexadecimal) #{0}: ", i + 1);
                values[i] = Console.ReadLine();
            }

            try
            {
                /* Replace the hexadecimal values. */
                result = registers[reg].SetBytes(index, values);

                if (result != false)
                {
                    temp = registers[reg].GetBytesString(0, 3);

                    Console.WriteLine("Register: {0} \nContents: {1}",
                                reg.ToString("X"),
                                temp);
                }

                else
                    Console.WriteLine("Error: An error was detected by SetBytes.");
            }

            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");
            }
        }

    }

}
