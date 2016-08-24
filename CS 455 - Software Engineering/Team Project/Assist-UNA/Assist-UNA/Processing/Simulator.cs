using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**************************************************************************************************
 * 
 * Name: Simulator
 * 
 * ================================================================================================
 * 
 * Description: This class emulates the IBM/360 that the ASSIST/I language was used upon. This 
 *              class contains the methods to initialize the memory and the registers with their
 *              starting contents and also contains the driver that calls the library class to
 *              process the next instruction contained in the object code.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/15/2014   JMB     Created initial file. Added Memory initialization and display methods.
 *                          Updated the constructor. Added Register and PSW display methods.
 *              THH     Created initial version of the constructor.
 * 04/17/2014   CAF     Modified the Simulate method to call the SimulateInstruction method of the
 *                          library. Fixed end condition for Simulate while loop.
 *              JMB     Assisted in developing the logic and implementation for the Simulator loop.
 * 04/18/2014   JMB     Added preliminary documentation. Added data file processing.
 *              CAF     Created Initial entryStream of the List<string> type to contain the input
 *                          after $ENTRY.
 * 04/21/2014   JMB     Added Register 13-15 initial settings. See InitializeMemoryContents().
 * 04/22/2014   JMB     Updated to better conform to standards.
 * 04/24/2014   JMB     Added EXECUTION_COMPLETE string to end of PRT.
 * 04/25/2014   JMB     Fixed a fault in data entry from the input file.
 * 04/26/2014   JMB     Updated Simulator initialization fatal exceptions handling and reporting.
 * 04/26/2014   ACA     Updated debugging.
 * 04/27/2014   JMB     Added SetLibraryLinesOnLastPage mutator to facilitate printing of 
 *                          multiple-page PRTs.
 *              CAF     Edited the description in the class header.
 *                      
 *************************************************************************************************/

namespace Assist_UNA
{
    class Simulator
    {
        /* Constants. */
        /* 
         * END_DISPLACEMENT is 18 fullwords (4 * 18 = 72 = 0x48).
         * It will appear after the doubleword nearest the end of the program. 
         */
        private const string EXECUTION_COMPLETE = "*** EXECUTION COMPLETED ***";
        private const string MEMORY_INIT_ERROR = 
            "An error occurred while initializing Simulator memory.";
        private const uint DOUBLEWORD = 8;
        private const uint END_DISPLACEMENT = 0x48;
        private const uint NUM_REGISTERS = 16;
        private const uint REGISTER_SIZE = 3;


        /* Private members. */
        private bool debugMode = false;
        private bool hasError = false;
        private bool stopProcessor = false;
        private Library library;
        private List<string> entryStream;
        private MainForm mainForm;
        private Memory mainMemory;
        private PSW progStatWord;
        private Register[] registers;
        private string inputFile;
        private string objectFilePath;
        private string prtFilePath;
        private uint endCondition;
        private uint locationCounter;
        private uint programEnd;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        Simulator  
         * 
         * Author(s):   Travis Hunt
         *              Michael Beaver
         *              Chad Farley
         *              
         * Input:       N/A  
         * Return:      N/A
         * Description: This is the overloaded constructor that will initialize the Simulator
         *              object to a user-specified state. Do not use a default constructor.
         *              
         *****************************************************************************************/
        public Simulator(uint memorySize, string objFile, string prtFile, MainForm main, 
            string inputFileParameter, int inputFileLineCount)
        {
            StreamReader inputFileStream;
            string dataEntryLine;

            mainMemory = new Memory(memorySize);
            progStatWord = new PSW();
            registers = new Register[NUM_REGISTERS];
            inputFile = inputFileParameter;
            entryStream = new List<string>(inputFileLineCount);

            locationCounter = 0;
            mainForm = main;
            objectFilePath = objFile;
            prtFilePath = prtFile;
            debugMode = main.GetDebugMode();
            
            /* Create each Register object. */
            for (int i = 0; i < NUM_REGISTERS; i++)
                registers[i] = new Register();

            try
            {
                /* Read data from the data file into the data list. */
                inputFileStream = new StreamReader(inputFileParameter);

                for (int i = 0; i < inputFileLineCount; i++)
                {
                    dataEntryLine = inputFileStream.ReadLine();

                    try
                    {
                        /* Check for null and empty lines. */
                        if (!String.IsNullOrEmpty(dataEntryLine))
                            entryStream.Insert(i, dataEntryLine);

                        else if (inputFileStream.EndOfStream)
                            break;

                        else
                            i--;
                    }

                    catch (ArgumentOutOfRangeException)
                    {
                        hasError = true;
                    }
                }
                
                inputFileStream.Close();
            }
            
            catch (FileNotFoundException)
            {
                hasError = true;
            }

            if (hasError == false)
            {
                InitializeMemoryContents();

                library = new Library(mainMemory, registers, progStatWord,
                    entryStream, mainForm, endCondition);
            }
        }

        /******************************************************************************************
         * 
         * Name:        HasError  
         * 
         * Author(s):   Michael Beaver
         *              
         *              
         * Input:       N/A 
         * Return:      The hasError flag is a boolean.
         * Description: This method will return the value of the hasError flag. If there are 
         *              errors, simulation should NOT contnue. Errors should be fatal.
         *              
         *****************************************************************************************/
        public bool HasError()
        {
            return hasError;
        }

        /******************************************************************************************
         * 
         * Name:        GetMemorySize  
         * 
         * Author(s):   Michael Beaver
         *              
         *              
         * Input:       N/A 
         * Return:      The memory size is an unsigned integer.
         * Description: This method will return the size of the main memory of the Simulator. 
         *              
         *****************************************************************************************/
        public uint GetMemorySize()
        {
            return mainMemory.GetMemorySize();
        }

        /******************************************************************************************
         * 
         * Name:        DisplayMemoryContents  
         * 
         * Author(s):   Michael Beaver    
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will update the memory listing on the main form to the contents
         *              of the memory in the Simulator.
         *              
         *****************************************************************************************/
        public void DisplayMemoryContents()
        {
            string address;
            string charRepresentation;
            string memoryContents;
            uint index = 0;
            uint size = mainMemory.GetMemorySize();

            while (index < size)
            {
                /* Format address. */
                address = index.ToString("X").PadLeft(6, '0');

                /* Format memory contents. */
                if (index + 15 < size)
                    memoryContents = mainMemory.GetBytesString(index, index + 15);

                else
                    memoryContents = mainMemory.GetBytesString(index, size - 1).PadRight(32, ' ');

                /* Add a space between each byte. 32 digits + 15 spaces = 47 total characters. */
                for (int i = 2; i < 47; i += 3)
                    memoryContents = memoryContents.Insert(i, " ");

                /* Format EBCDIC character representation. */
                if (index + 15 < size)
                    charRepresentation = mainMemory.GetEBCDIC(index, index + 15);

                else
                    charRepresentation = mainMemory.GetEBCDIC(index, size - 1).PadRight(16, ' ');

                mainForm.AddMemoryEntry(address, memoryContents, charRepresentation);

                index += 16;
            }
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
        public void DisplayPSWContents()
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

            mainForm.SetPSW(mask);
        }

        /******************************************************************************************
         * 
         * Name:        DisplayRegisterContents  
         * 
         * Author(s):   Michael Beaver    
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will update the registers listing on the main form to the contents
         *              of the registers in the Simulator.
         *              
         *****************************************************************************************/
        public void DisplayRegisterContents()
        {
            mainForm.SetRegister0(registers[0].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister1(registers[1].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister2(registers[2].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister3(registers[3].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister4(registers[4].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister5(registers[5].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister6(registers[6].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister7(registers[7].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister8(registers[8].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister9(registers[9].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister10(registers[10].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister11(registers[11].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister12(registers[12].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister13(registers[13].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister14(registers[14].GetBytesString(0, REGISTER_SIZE));
            mainForm.SetRegister15(registers[15].GetBytesString(0, REGISTER_SIZE));
        }

        /******************************************************************************************
         * 
         * Name:        SetLibraryLinesOnLastPage  
         * 
         * Author(s):   Michael Beaver    
         *                  
         * Input:       The lines is an integer.
         * Return:      N/A
         * Description: This method will update the appropriate member in the Library object to
         *              facilitate printing of multiple-page PRTs.
         *              
         *****************************************************************************************/
        public void SetLibraryLinesOnLastPage(int lines)
        {
            library.SetLinesOnLastPage(lines);
        }

        /******************************************************************************************
         * 
         * Name:        Simulate  
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver    
         *              Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method simulates the execution loop. Instructions at the location
         *              counter will be executed via the appropriate library method.
         *              
         *****************************************************************************************/
        public void Simulate()
        {
            int instructionProcessed = 0;
            int maxInstructions = mainForm.GetMaxInstructions();
            StreamWriter prtFileOutput;

            DisplayMemoryContents();
            DisplayRegisterContents();
            DisplayPSWContents();

            /* Program execution loop. */
            do
            {
                library.SimulateInstruction(ref locationCounter);
                instructionProcessed++;

                if (debugMode && instructionProcessed <= maxInstructions)
                {
                    if (locationCounter == endCondition)
                        stopProcessor = 
                            mainForm.WaitForDebugClick(instructionProcessed, true, locationCounter);

                    else
                        stopProcessor = 
                            mainForm.WaitForDebugClick(instructionProcessed, false, locationCounter);

                    debugMode = mainForm.GetDebugMode();
                }

                else if (instructionProcessed > maxInstructions) 
                {
                    MessageBox.Show("Maximum number of instructions reached.", "Error");
                    mainForm.UpdateStatusLabel("Maximum number of instructions reached.");
                }

                else if (!debugMode && instructionProcessed <= maxInstructions)
                    mainForm.SetInstructionNumber(instructionProcessed);

            } while (locationCounter != endCondition && 
                instructionProcessed <= maxInstructions && stopProcessor == false);

            try
            {
                if (File.Exists(prtFilePath))
                {
                    prtFileOutput = new StreamWriter(prtFilePath, true);

                    prtFileOutput.Write(Environment.NewLine + 
                        Environment.NewLine + EXECUTION_COMPLETE);

                    prtFileOutput.Close();
                }
            }

            catch (FileNotFoundException)
            {
                MessageBox.Show(mainForm, ".PRT file not found!");
            }

            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(mainForm, ".PRT file access denied!");
            }
        }


        /* Private methods. */

        /******************************************************************************************
         * 
         * Name:        InitializeMemoryContents  
         * 
         * Author(s):   Chad Farley
         *              Michael Beaver    
         *                  
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the memory contents of the Simulator to the
         *              object code provided by the Assembler. This method will also update 
         *              Register 14 to hold the program termination condition.
         *              
         *****************************************************************************************/
        private void InitializeMemoryContents()
        {
            bool flag = true;
            StreamReader reader = new StreamReader(objectFilePath);
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
                        throw new OutOfMemoryException(MEMORY_INIT_ERROR);

                    offset++;
                }

                programEnd = Convert.ToUInt32(fileContents.Length) / 2;

                /* Get to the nearest doubleword boundary after the end of the object code. */
                while (programEnd % DOUBLEWORD != 0)
                    programEnd++;

                endCondition = programEnd + END_DISPLACEMENT;

                /* Check memory bounds. */
                if (programEnd > GetMemorySize() || endCondition > GetMemorySize())
                {
                    hasError = true;
                    throw new OutOfMemoryException(MEMORY_INIT_ERROR);
                }
                    

                else
                {
                    /* 
                     * Initialize the registers. 
                     * Register 13 marks the save area.
                     * Register 14 marks the location to which control shall be returned.
                     * Register 15 marks the entry point.
                     * The instruction address defaults to the entry point.
                     */
                    registers[13].SetBytes(0, Convert.ToInt32(programEnd));
                    registers[14].SetBytes(0, Convert.ToInt32(endCondition));
                    registers[15].SetBytes(0, 0);
                    progStatWord.SetBytes(0, 0);
                }
            }

            catch (Exception ex)
            {
                if (ex is OutOfMemoryException || ex is FileNotFoundException || 
                    ex is OutOfMemoryException)
                    MessageBox.Show(ex.Message, "Simulator Error");

                hasError = true;
            }

            finally
            {
                reader.Close();
            }
        }
    }
}