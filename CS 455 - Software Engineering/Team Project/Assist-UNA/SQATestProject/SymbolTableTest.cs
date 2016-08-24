using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: SymbolTable
 * 
 * ================================================================================================
 * 
 * Description: This class is to serve as the working test model of the final Symbol Table Class 
 *              for the ASSIST/UNA project.
 *                            
 *              It inherits from the abstract Table class.
 *              
 *              The Symbol Table is structured using the Hashtable class from the 
 *              System.Collections directory.
 *              
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/18/2014   THH     Original version.
 * 03/25/2014   THH     Fixed commenting issues to comply with standards.
 *  
 *************************************************************************************************/

namespace TravisTestProject
{
    class SymbolTable : Table
    {
        /* Constants. */
        private const int MAX_SYMBOLS = 100;
        private const int MAX_SIZE = 211;

        /* Private members. */
        private Hashtable symbolTable;
        private int numSymbols;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        SymbolTable (Constructor)     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      N/A 
         * Description: The constructor for SymbolTable. Initializes the Symbol table as a hash
         *              table and sets the initial number of Symbols to 0.
         *              
         *****************************************************************************************/
        public SymbolTable()
        {
            symbolTable = new Hashtable(MAX_SIZE);
            numSymbols = 0;
        }

        /******************************************************************************************
         * 
         * Name:        GetAddress     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The symbol as a string. 
         * Return:      The address of the symbol as a string. 
         * Description: This method uses the symbol as a key to find the value, which is the
         *              address of the symbol.
         *              
         *****************************************************************************************/
        override public string GetAddress(string key)
        {
            return (string)symbolTable[key];
        }

        /******************************************************************************************
         * 
         * Name:        Hash     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The symbol and its location as strings. 
         * Return:      N/A 
         * Description: This method takes the symbol as the key and the location as the value and
         *              uses the built in hashing function to store them in the hash table.
         *              
         *****************************************************************************************/
        override public void Hash(string key, string location)
        {
            symbolTable.Add(key, location);
            numSymbols++;
        }

        /******************************************************************************************
         * 
         * Name:        isSymbol   
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The symbol as a string, it is used at the key for the hash table. 
         * Return:      True if the symbol is in the table, false if otherwise. 
         * Description: This method checks the table for the existance of a symbol.
         *              
         *****************************************************************************************/
        public bool isSymbol(string key)
        {
            return symbolTable.ContainsKey(key);
        }

        /******************************************************************************************
         * 
         * Name:        isSymbolFull     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      True if number of symbols in table is 50. 
         * Description: This method checks the number of symbols in the table and returns whether
         *              or not the number is equal to the max number of symbols (50).
         *              
         *****************************************************************************************/
        public bool isSymbolFull()
        {
            return (symbolTable.Count == MAX_SYMBOLS);
        }

        /******************************************************************************************
         * 
         * Name:        isTableFull     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      True if table is full. 
         * Description: This method checks the number of symbols in the table and returns whether
         *              or not the number is equal to the max size of the table (101).
         *              
         *****************************************************************************************/
        override public bool IsTableFull()
        {
            return (symbolTable.Count == MAX_SIZE);
        }

        /******************************************************************************************
         * 
         * Name:        PrintTable     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      N/A 
         * Description: This method prints to the console the contents of the symbol table.
         *              Used for testing only.
         *              
         *****************************************************************************************/
        override public void PrintTable()
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("|     Symbol Table   |");
            Console.WriteLine("| Symbol   | Address |");
            Console.WriteLine("|--------------------|");

            foreach (var key in symbolTable.Keys)
                Console.WriteLine(String.Format("| {0,-8} | {1}  |", key, symbolTable[key]));

            Console.WriteLine("----------------------");
        }
    }
}
