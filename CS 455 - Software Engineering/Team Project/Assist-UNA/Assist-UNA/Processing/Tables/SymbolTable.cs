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
 * 03/26/2014   THH     Added accessor for the number of items in the table.
 * 03/27/2014   THH     Added accessors for the addresses and symbols.
 *                      Added the update address method.
 * 04/05/2014   THH     Changed to correct namespace.
 * 04/07/2014   THH     Added try/catch to get address.
 * 04/28/2014   THH     Final commenting and cleanup.
 *  
 *************************************************************************************************/

namespace Assist_UNA
{
    class SymbolTable : Table
    {
        /* Constants. */
        private const int MAX_SYMBOLS = 100;
        private const int MAX_SIZE = 211;

        /* Private members. */
        private Hashtable symbolSizes;
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
            table = new Hashtable(MAX_SIZE);
            numSymbols = 0;
            symbolSizes = new Hashtable(MAX_SIZE);
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
            try
            {
                return (string)table[key];
            }
            catch (ArgumentNullException)
            {
                return "";
            }
        }

        /******************************************************************************************
         * 
         * Name:        GetAddressesList
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The keys (symbols) of the table as an array of strings. 
         * Description: This returns as an array of strings the symbols.
         *              
         *****************************************************************************************/
        public string[] GetAddressesList()
        {
            string[] addresses = new string[numSymbols];
            table.Values.CopyTo(addresses, 0);

            return addresses;
        }

        /******************************************************************************************
         * 
         * Name:        GetSymbolsList
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The keys (symbols) of the table as an array of strings. 
         * Description: This returns as an array of strings the symbols.
         *              
         *****************************************************************************************/
        public string[] GetSymbolsList()
        {
            string[] keys = new string[numSymbols];
            table.Keys.CopyTo(keys, 0);

            return keys;
        }

        /******************************************************************************************
         * 
         * Name:        GetSizesList
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The sizes of the symbols as an array of ints. 
         * Description: This returns as an array of strings the sizes of the ints.
         *              
         *****************************************************************************************/
        public int[] GetSizesList()
        {
            int[] sizes = new int[numSymbols];
            symbolSizes.Values.CopyTo(sizes, 0);

            return sizes;
        }

        /******************************************************************************************
         * 
         * Name:        GetSizesListLabels
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The labels of the sizes of the symbols as an array of strings. 
         * Description: This returns as an array of strings the labels of the sizes.
         *              
         *****************************************************************************************/
        public string[] GetSizesListLabels()
        {
            string[] labels = new string[numSymbols];
            symbolSizes.Keys.CopyTo(labels, 0);

            return labels;
        }

        /******************************************************************************************
        * 
        * Name:        GetMaxSize   
        * 
        * Author(s):   Travis Hunt
        *              
        * Input:       N/A 
        * Return:      The max number of symbols. 
        * Description: This method returns the max number of symbols.
        *              
        *****************************************************************************************/
        override public int GetMaxSize()
        {
            return MAX_SIZE;
        }

        /******************************************************************************************
         * 
         * Name:        GetSize   
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The number of symbols. 
         * Description: This method returns the number of symbols that are stored in the table.
         *              
         *****************************************************************************************/
        override public int GetSize()
        {
            return numSymbols;
        }

        /******************************************************************************************
         * 
         * Name:        GetSymbolSize   
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The symbol as a string. 
         * Return:      The size of the memory at the symbol.
         * Description: This method returns the size of the symbol in memory.
         *              
         *****************************************************************************************/
        public int GetSymbolSize(string key)
        {
            if (symbolSizes.ContainsKey(key))
                return (int)symbolSizes[key];
            else
                return -1;
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
            if (!IsSymbol(key))
            {
                table.Add(key, location);
                numSymbols++;
            }
        }

        /******************************************************************************************
         * 
         * Name:        IsSymbol   
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The symbol as a string, it is used at the key for the hash table. 
         * Return:      True if the symbol is in the table, false if otherwise. 
         * Description: This method checks the table for the existance of a symbol.
         *              
         *****************************************************************************************/
        public bool IsSymbol(string key)
        {
            return table.ContainsKey(key);
        }

        /******************************************************************************************
         * 
         * Name:        IsSymbolFull     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      True if number of symbols in table is 50. 
         * Description: This method checks the number of symbols in the table and returns whether
         *              or not the number is equal to the max number of symbols (50).
         *              
         *****************************************************************************************/
        public bool IsSymbolFull()
        {
            return (table.Count == MAX_SYMBOLS);
        }

        /******************************************************************************************
        * 
        * Name:        IsTableEmpty     
        * 
        * Author(s):   Travis Hunt
        *              
        * Input:       N/A 
        * Return:      True if the table is empty. 
        * Description: This method return true if there are no items in the table.
        *              
        *****************************************************************************************/
        override public bool IsTableEmpty()
        {
            return (table.Count == 0);
        }

        /******************************************************************************************
         * 
         * Name:        IsTableFull     
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
            return (table.Count == MAX_SIZE);
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

            foreach (var key in table.Keys)
                Console.WriteLine(String.Format("| {0,-8} | {1}  |", key, table[key]));

            Console.WriteLine("----------------------");
        }

        /******************************************************************************************
         * 
         * Name:        UpdateAddress
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The key and the address to update as strings, the size of the storage as
         *              int. 
         * Return:      N/A
         * Description: This method updates the address of the given symbol to the address passed
         *              to it as well as the size. If the size is less than 0 however, the size 
         *              will not be added.
         *              
         *****************************************************************************************/
        public void UpdateAddress(string key, string address, int size)
        {
            if (IsSymbol(key))
            {
                table[key] = address;
                if (size >= 0)
                    symbolSizes.Add(key, size);
            }
        }
    }
}
