using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryTest;

 /**************************************************************************************************
 * 
 * Name: LibraryTestDriver
 * 
 * ================================================================================================
 * 
 * Description: This class drives the test program for the Library.              
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/04/2014   CAF     Created test driver.
 * 04/04/2014   JMB     Assisted.
 * 04/07/2014   JMB     Added method to test the DB and DXB calculation methods.
 *                         CONCERN: In DXB, when X is 0, we are supposed to use the literal value
 *                        zero, not the contents of Register 0. Right? What about with DB?
 * 04/08/2014   CAF     Assisted JMB with general maintenance and testing conditions.
 * 04/08/2014   JMB     Maintained code and conducted testing.
 * 04/10/2014   CAF     Updated code to reflect function changes.
 * 04/11/2014   JMB     Removed unnecessary comments.
 * 04/13/2014   JMB     Removed more unnecessary comments.
 * 04/15/2014   JMB     Added a method to read object code from a file. Also added a method to
 *                      display memory contents.
 *                    
 *************************************************************************************************/

namespace LibraryTest
{
    class LibraryTestDriver
    {
        /* 
         * For testing purposes, we've decided to make the memory portion of this driver public
         * to reflect that the memory will be made public as part of the "processing" portion
         * of the back-end.
         */
        const string FILE_PATH = "..\\..\\..\\Test Object Codes\\";
        const uint MAIN_MEMORY_SIZE = 368;
        const uint NUM_REGISTERS = 16;

        static void Main(string[] args)
        {
            Memory mainMemory = new Memory(MAIN_MEMORY_SIZE);
            PSW progStatWord = new PSW();
            Register[] registers = new Register[NUM_REGISTERS];
            LibraryTest library = new LibraryTest(mainMemory, registers, progStatWord);
            uint locationCounter = 0;

            InitializeMemory(ref mainMemory, "THPROG1.obj");
            InitializeRegisters(ref registers);

            /* The stopping condition is not accurate. */
            while (locationCounter >= 0 && locationCounter < MAIN_MEMORY_SIZE)
            {
                Console.Clear();
                DisplayMemoryContents(ref mainMemory);
                library.SimulateInstruction(ref locationCounter);
            }
                

            Console.ReadLine();
        }

        /******************************************************************************************
         * 
         * Name:        DisplayMemoryContents
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       mainMemory is a Memory object.
         * Return:      N/A
         * Description: This method will display all the contents in Memory, including the addresses,
         *              actual contents (bytes), and the EBCDIC character representations.
         *           
         *****************************************************************************************/
        static void DisplayMemoryContents(ref Memory mainMemory)
        {
            string temp;
            uint i = 0;
            uint sz = mainMemory.GetMemorySize();

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
                    temp += mainMemory.GetBytesString(i, i + 15);
                else
                    temp += mainMemory.GetBytesString(i, sz - 1).PadRight(32, ' ');

                temp += " ";

                /* Format EBCDIC character representation. */
                if (i + 15 < sz)
                    temp += mainMemory.GetEBCDIC(i, i + 15);
                else
                    temp += mainMemory.GetEBCDIC(i, sz - 1).PadRight(16, ' ');

                Console.WriteLine(temp);

                i += 16;
            }
        }

        /******************************************************************************************
         * 
         * Name:        InitializeMemory 
         * 
         * Author(s):   Michael Beaver     
         *              
         * Input:       mainMemory is a Memory object.
         * Return:      N/A
         * Description: This method will initialize the contents of mainMemory to a series of bytes
         *              meant to simulate object code.
         *              
         *****************************************************************************************/
        static void InitializeMemory(ref Memory mainMemory)
        {
            string[] memoryData = {
                                    "40", "20", "6B", "20", "20", "20", "6B", "20", "21", "20", 
                                    "6B", "20", "20", "60", "41", "40", "00", "14", "18", "34", 
                                    "00", "00", "00", "05", "6C", "30", "C0", "6B", "58", "D0", 
                                    "C0", "26", "98", "EC", "D0", "0C", "07", "FE", "F5", "F5", 
                                    "DF", "0D", "00", "00", "00", "14", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "FF", "FF", "FF", "FD", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "40", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", "F5", 
                                    "F5", "F5", "F5", "F5", "F5", "F5"
                                   };

            /* Set each byte in memory. */
            for (uint i = 0; i < MAIN_MEMORY_SIZE; i++)
                mainMemory.SetByte(i, memoryData[i]);
        }

        /******************************************************************************************
        * 
        * Name:        InitializeMemory 
        * 
        * Author(s):   Michael Beaver     
        *              
        * Input:       mainMemory is a Memory object, and fileName is a string.
        * Return:      N/A
        * Description: This method will initialize the contents of mainMemory to a series of bytes
        *              taken from the specified file name. See the constants section of this file
        *              to update the file path (FILE_PATH) as necessary.
        *              
        *****************************************************************************************/
        static void InitializeMemory(ref Memory mainMemory, string fileName)
        {
            bool flag = true;
            StreamReader reader = new StreamReader(FILE_PATH + fileName);
            string fileContents;
            string tempByte;
            uint offset = 0;

            try
            {
                fileContents = reader.ReadLine();

                /* Attempt to initialize memory from the file contents. */
                for (int i = 0; i < fileContents.Length; i += 2)
                {
                    tempByte = fileContents[i].ToString() + fileContents[i + 1].ToString();
                    flag = mainMemory.SetByte(offset, tempByte);

                    /* Stop if overwriting memory fails. */
                    if (flag == false)
                        throw new OutOfMemoryException("Error reading into memory.");

                    offset++;
                }
            }

            catch
            {
                Console.WriteLine("There was an error in the file input!");
            }

            finally
            {
                reader.Close();
            }
        }

        /******************************************************************************************
         * 
         * Name:        InitializeRegisters
         * 
         * Author(s):   Michael Beaver     
         *              
         * Input:       registers is an array of Register objects.
         * Return:      N/A
         * Description: This method will initialize each Register object to a randomly generated
         *              value (in bytes). These values are meant to mimic actual register contents.
         *              
         *****************************************************************************************/
        static void InitializeRegisters(ref Register[] registers)
        {
            string[] registerContents = {"F4", "F4", "F4", "F4", 
                "F4", "F4", "F4", "F4", 
                "F4", "F4", "F4", "F4", 
                "F4", "F4", "F4", "F4", 
                "F4", "F4", "F4", "F4", 
                "F4", "F4", "F4", "F4",  
                "F4", "F4", "F4", "F4", 
                "F4", "F4", "F4", "F4", 
                "F4", "F4", "F4", "F4", 
                "F4", "F4", "F4", "F4",   
                "F4", "F4", "F4", "F4",  
                "F4", "F4", "F4", "F4",   
                "F4", "F4", "F4", "F4",  
                "F4", "F4", "F4", "F4",   
                "F4", "F4", "F4", "F4",  
                "F4", "F4", "F4", "F4"};  
            uint j = 0;

            for (uint i = 0; i < NUM_REGISTERS; i++)
            {
                registers[i] = new Register();

                /* Set each byte. */
                registers[i].SetByte(0, registerContents[j]);
                registers[i].SetByte(1, registerContents[j + 1]);
                registers[i].SetByte(2, registerContents[j + 2]);
                registers[i].SetByte(3, registerContents[j + 3]);

                j += 4;
            }
                
        }

    }

}
