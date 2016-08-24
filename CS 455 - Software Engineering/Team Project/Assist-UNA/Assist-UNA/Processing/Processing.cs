using Assist_UNA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**************************************************************************************************
 * 
 * Name: Processing
 * 
 * ================================================================================================
 * 
 * Description: This is a static class to take care of managing the backend processing. This 
 *              includes Assembling and Simulating the source code.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/05/2014   THH     Original version.
 * 04/08/2014   ACA     Added front end ASSIST option variables and worked on PRT with THH.
 * 04/10/2014   AAH     Uncommented lines so the obj and imf files will be deleted. These can
 *                          be added back for debugging if needed.
 *                      Moved to Processing folder.
 * 04/14/2014   THH     Added preliminary code for debugging and final run modes.
 * 04/15/2014   JMB     Added a new data member for memory size.
 * 04/18/2014   JMB     Fixed an error in Initialize() where it was accessing a null "main" object.
 *                          Added symbol table display updating to debug and final/run methods.
 * 04/19/2014   JMB     Added deletion of input files in assemble/debug and final/run.
 * 04/20/2014   JMB     The Assemble/Debug and Final/Run methods now check whether obj, prt, and 
 *                          input files exist before deleting them.
 * 04/21/2014   CAF     Added simulator execution abortion on fatal error.
 *              JMB     Assisted CAF with implementing fatal exception handling.
 * 04/22/2014   JMB     Updated to better conform to standards.
 * 04/24/2014   JMB     Added EXECUTION_COMPLETE string to end of PRT for Assemble/Final Run.
 * 04/25/2014   JMB     Assemble/Final Run will now delete Assembler-generated input files (.inp).
 * 04/26/2014   JMB     Updated Simulator and Assembler fatal exception reporting.
 * 04/26/2014   ACA     Updated debugging.
 *                      Added some instruction number output.
 * 04/27/2014   JMB     Updated to facilitate PRT printing of multiple pages.
 * 04/27/2014   THH     Added the exceed max lines failure to delete the .prt.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    static class Processing
    {
        /* Constants. */
        private const string SIMULATOR_ERROR = 
            "Cannot be executed! There were errors in Simulator initialization.";


        /* Private members. */
        private static Assembler assembler;
        private static int optionsInstructions;
        private static int optionsLines;
        private static int optionsPages;
        private static MainForm main;
        private static string identifier;
        private static string inputFilePath;
        private static string intermediateFilePath;
        private static string objectFilePath;
        private static string prtFilePath;
        private static string sourceFileDirectory;
        private static string sourceFileName;
        private static string sourceFilePath;
        private static SymbolTable symbolTable;
        private static uint optionsMemorySize;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        Assemble        
         * 
         * Author(s):   Travis Hunt
         *              
         *              
         * Input:       The main form.   
         * Return:      N/A
         * Description: This method will handle the duties of choosing the option to only assemble.
         *              
         *****************************************************************************************/
        public static void Assemble(MainForm mainForm)
        {
            bool inputFileFound;
            string pass1Return;
            string pass2Return;

            main = mainForm;
            main.UpdateStatusLabel("Assembling...");
            Cursor.Current = Cursors.AppStarting;

            Initialize(main);

            pass1Return = assembler.Pass1();
            pass2Return = assembler.Pass2();

            if (pass2Return == "IOException")
                main.SetPathPRT(String.Format(@"{0}\{1}.PRT", 
                    sourceFileDirectory, sourceFileName));

            else if (pass2Return == "UnauthorizedAccessException")
            {
                File.Delete(intermediateFilePath);
                return;
            }

            inputFileFound = (pass1Return != "FileNotFoundException");

            if (pass1Return == "ExceedMaxLinesException" && File.Exists(prtFilePath))
                File.Delete(prtFilePath);

            symbolTable = assembler.GetSymbolTable();
            UpdateSymbolTableDisplay(symbolTable, main);

            Cursor.Current = Cursors.Default;

            if (assembler.HasErrors())
            {
                main.UpdateStatusLabel("Assembly complete with errors.");
                MessageBox.Show("Your project has been assembled with errors.", "Assembly Error");
            }

            else
            {
                main.UpdateStatusLabel("Assembly complete.");
                MessageBox.Show("Your project has been assembled.", "Assembly Success");
            }          
  
           if (File.Exists(objectFilePath))
                File.Delete(objectFilePath);

           if (File.Exists(inputFilePath))
               File.Delete(inputFilePath);

           if (File.Exists(intermediateFilePath))
               File.Delete(intermediateFilePath);
            
        }

        /******************************************************************************************
         * 
         * Name:        AssembleDebug        
         * 
         * Author(s):   Travis Hunt
         *              Michael Beaver
         *              Drew Aaron
         *              
         * Input:       The main form.   
         * Return:      N/A
         * Description: This method will handle the duties of choosing the option to assemble and
         *              debug.
         *              
         *****************************************************************************************/
        public static void AssembleDebug(MainForm main)
        {
            bool inputFileFound;
            string generatedInputFile;
            string pass1Return;

            Cursor.Current = Cursors.AppStarting;
            main.UpdateStatusLabel("Assembling...");

            Initialize(main);

            pass1Return = assembler.Pass1();
            inputFileFound = (pass1Return != "FileNotFoundException");

            if (assembler.Pass2() == "IOException")
                main.SetPathPRT(String.Format(@"{0}\{1}.PRT", 
                    sourceFileDirectory, sourceFileName));

            Cursor.Current = Cursors.Default;

            if (File.Exists(intermediateFilePath))
                File.Delete(intermediateFilePath);

            if (pass1Return == "ExceedMaxLinesException" && File.Exists(prtFilePath))
                File.Delete(prtFilePath);

            symbolTable = assembler.GetSymbolTable();
            UpdateSymbolTableDisplay(symbolTable, main); 

            /* Simulator code follow here. */
            if (assembler.HasErrors())
            {
                main.UpdateStatusLabel("Assembly complete with errors.");
                MessageBox.Show("Cannot be executed! Assembler errors encountered.", 
                    "Assembly Error");
            }

            else if (inputFileFound)
            {
                main.UpdateStatusLabel("Assembly complete. Executing in debug...");
                
                inputFilePath = assembler.GetInputFile();
                objectFilePath = assembler.GetObjectFile();
                optionsMemorySize = Convert.ToUInt32(main.GetMaxSize());

                Simulator sim = new Simulator(optionsMemorySize, objectFilePath, prtFilePath,
                    main, inputFilePath, assembler.GetNumDataLines());

                sim.SetLibraryLinesOnLastPage(assembler.GetNumLinesOnLastPage());

                /* Attempt to simulate execution of the program. */
                try
                {
                    /* Only simulate if there are no errors in the setup of the Simulator. */
                    if (!sim.HasError())
                        sim.Simulate();

                    else
                        throw new OperationCanceledException();

                    Cursor.Current = Cursors.Default;
                    main.UpdateStatusLabel("Assembly and execution complete. "
                        + "Number of instructions executed: " + main.GetInstructionNumber());
                }

                catch
                {
                    /* 
                     * If a fatal exception is encountered in the simulator, it will be thrown.
                     * This catch block signifies the abortion of simulator execution.
                     */
                    Cursor.Current = Cursors.Default;
                    main.UpdateStatusLabel("Assembly and execution complete with errors. "
                        + "Last instruction number executed: " + main.GetInstructionNumber());

                    main.DisplayDebugMode(false);
                }
            }

            else
            {
                main.UpdateStatusLabel("Assembly complete. Execution terminated.");
                MessageBox.Show("Cannot be executed! Input file not found.", "Processing Error");
            }

            try
            {
            if (File.Exists(objectFilePath))
                File.Delete(objectFilePath);

            if (File.Exists(prtFilePath))
                File.Delete(prtFilePath);

            generatedInputFile = main.GetDirectory() + "\\" + main.GetFileNameProject() + ".inp";
            if (File.Exists(generatedInputFile))
                File.Delete(generatedInputFile);
            }

            catch
            {
                MessageBox.Show("Unable to delete temporary file.");
            }
        }

        /******************************************************************************************
         * 
         * Name:        AssembleFinalRun  
         * 
         * Author(s):   Travis Hunt
         *              Chad Farley
         *              Michael Beaver
         *              Drew Aaron
         *              
         * Input:       The main form.   
         * Return:      N/A
         * Description: This method will handle the duties of choosing the option to assemble and
         *              final run.
         *              
         *****************************************************************************************/
        public static void AssembleFinalRun(MainForm main)
        {
            bool inputFileFound;
            Simulator sim;
            string generatedInputFile;
            string pass1Return;

            Cursor.Current = Cursors.AppStarting;
            main.UpdateStatusLabel("Assembling...");

            Initialize(main);

            identifier = main.GetID();

            pass1Return = assembler.Pass1();
            inputFileFound = (pass1Return != "FileNotFoundException");

            if (assembler.Pass2() == "IOException")
                main.SetPathPRT(String.Format(@"{0}\{1}.PRT", 
                    sourceFileDirectory, sourceFileName));

            if (File.Exists(intermediateFilePath))
                File.Delete(intermediateFilePath);

            if (pass1Return == "ExceedMaxLinesException" && File.Exists(prtFilePath))
                File.Delete(prtFilePath);

            symbolTable = assembler.GetSymbolTable();
            UpdateSymbolTableDisplay(symbolTable, main); 

            /* Simulator code follow here. */
            if (assembler.HasErrors())
            {
                main.UpdateStatusLabel("Assembled with errors.");
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Cannot be executed! Assembler errors encountered.", 
                    "Assembly Error");
            }

            else if (inputFileFound)
            {
                main.UpdateStatusLabel("Assembly complete. Executing with final run...");

                inputFilePath = assembler.GetInputFile();
                objectFilePath = assembler.GetObjectFile();
                optionsMemorySize = Convert.ToUInt32(main.GetMaxSize());

                sim = new Simulator(optionsMemorySize, objectFilePath, prtFilePath, 
                    main, inputFilePath, assembler.GetNumDataLines());

                sim.SetLibraryLinesOnLastPage(assembler.GetNumLinesOnLastPage());

                /* Attempt to simulate execution of the program. */
                try
                {
                    /* Only simulate if there are no errors in the setup of the Simulator. */
                    if (!sim.HasError())
                    {
                        sim.Simulate();
                        main.ShowOutput();
                    }

                    else
                        throw new OperationCanceledException();

                    Cursor.Current = Cursors.Default;
                    main.UpdateStatusLabel("Assembly and final run execution complete. "
                        + "Number of instructions executed: " + main.GetInstructionNumber());
                }
                 
                catch
                { 
                    /* 
                     * If a fatal exception is encountered in the simulator, it will be thrown.
                     * This catch block signifies the abortion of simulator execution.
                     */
                    Cursor.Current = Cursors.Default;
                    main.UpdateStatusLabel("Assembly and final run execution complete with errors. "
                        + "Last instruction number executed: " + main.GetInstructionNumber());
                }
            }

            else
            {
                main.UpdateStatusLabel("Assembly complete. Execution terminated.");
                MessageBox.Show("Cannot be executed! Input file not found.", "Processing Error");
            }

            try
            {
                if (File.Exists(objectFilePath))
                    File.Delete(objectFilePath);

                generatedInputFile = main.GetDirectory() + "\\" + main.GetFileNameProject() + ".inp";
                if (File.Exists(generatedInputFile))
                    File.Delete(generatedInputFile);
            }

            catch
            {
                MessageBox.Show("Unable to delete temporary file.");
            }
        }


        /* Private methods. */

        /******************************************************************************************
         * 
         * Name:        Initialize        
         * 
         * Author(s):   Travis Hunt
         *              Michael Beaver
         *              Drew Aaron
         *              
         * Input:       main is the MainForm.
         * Return:      N/A
         * Description: This private method will initialize all the variables to the correct values
         *              sent from the main form.
         *              
         *****************************************************************************************/
        private static void Initialize(MainForm main)
        {
            identifier = main.GetID();
            inputFilePath = "";

            sourceFileDirectory = main.GetDirectory();
            sourceFileName = main.GetFileNameProject();

            sourceFilePath = String.Format(@"{0}\{1}.una", sourceFileDirectory, sourceFileName);
            intermediateFilePath = String.Format(@"{0}\{1}.imf", 
                sourceFileDirectory, sourceFileName);
            objectFilePath = String.Format(@"{0}\{1}.obj", sourceFileDirectory, sourceFileName);
            prtFilePath = main.GetPathPRT();

            optionsInstructions = main.GetMaxInstructions();
            optionsLines = main.GetMaxLines();
            optionsPages = main.GetMaxPages();

            assembler = new Assembler(identifier, sourceFilePath, prtFilePath,
                intermediateFilePath, objectFilePath, optionsInstructions, 
                optionsLines, optionsPages);
        }

        /******************************************************************************************
         * 
         * Name:        UpdateSymbolTableDisplay        
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The symbol table and main form.   
         * Return:      N/A
         * Description: This method will update the symbol table display on the main form.
         *              
         *****************************************************************************************/
        private static void UpdateSymbolTableDisplay(SymbolTable symTable, MainForm main)
        {
            /* Display the symbol table to the main display. */
            string[] symbols = symbolTable.GetSymbolsList();
            string[] addresses = symbolTable.GetAddressesList();

            Array.Sort(addresses, symbols);

            for (int i = 0; i < symbols.Length; i++)
                main.AddSymbolTableEntry(symbols[i], addresses[i]);
        }
    }
}
