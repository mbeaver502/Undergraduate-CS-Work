using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: LiteralTable
 * 
 * ================================================================================================
 * 
 * Description: This class is to serve as the working test model of the final Literal Table Class 
 *              for the ASSIST/UNA project.
 *                            
 *              It inherits from the abstract Table class.
 *              
 *              The Literal Table is structured using the Hashtable class from the 
 *              System.Collections directory.
 *              
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/18/2014   THH     Original version.
 * 03/25/2014   THH     Fixed commenting issues to comply with standards.
 * 03/26/2014   THH     Added accessor for the number of items in the table.
 *                      Added accessors for the addresses and symbols.
 * 04/05/2014   THH     Changed to correct namespace.
 * 04/25/2014   THH     Added literal sizes hash table and accessor.
 *  
 *************************************************************************************************/

namespace Assist_UNA
{
    class LiteralTable : Table
    {
        /* Constants. */
        private const int MAX_LITERALS = 50;
        private const int MAX_SIZE = 101;

        /* Private members. */
        private int numLiterals;
        private Hashtable literalSizes;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        LiteralTable (Constructor)     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      N/A 
         * Description: The constructor for LiteralTable. Initializes the literal table as a hash
         *              table and sets the initial number of literals to 0.
         *              
         *****************************************************************************************/
        public LiteralTable()
        {
            table = new Hashtable(MAX_SIZE);
            literalSizes = new Hashtable(MAX_SIZE);
            numLiterals = 0;
        }

        /******************************************************************************************
         * 
         * Name:        ClearTable     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      N/A
         * Description: This method clears all values out of the literal table.
         *              
         *****************************************************************************************/
        public void ClearTable()
        {
            table = new Hashtable(MAX_SIZE);
            literalSizes = new Hashtable(MAX_SIZE);
            numLiterals = 0;
        }

        /******************************************************************************************
         * 
         * Name:        GetAddress     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The literal as a string. 
         * Return:      The address of the literal as a string. 
         * Description: This method uses the literal as a key to find the value, which is the
         *              address of the literal.
         *              
         *****************************************************************************************/
        override public string GetAddress(string key)
        {
            return (string)table[key];
        }

        /******************************************************************************************
         * 
         * Name:        GetAddressesList
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The keys (literals) of the table as an array of strings. 
         * Description: This returns as an array of strings the literals.
         *              
         *****************************************************************************************/
        public string[] GetAddressesList()
        {
            string[] addresses = new string[numLiterals];
            table.Values.CopyTo(addresses, 0);

            return addresses;
        }

        /******************************************************************************************
         * 
         * Name:        GetLiteralsList
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The keys (literals) of the table as an array of strings. 
         * Description: This returns as an array of strings the literals.
         *              
         *****************************************************************************************/
        public string[] GetLiteralsList()
        {
            string[] keys = new string[numLiterals];
            table.Keys.CopyTo(keys, 0);

            return keys;
        }

        /******************************************************************************************
         * 
         * Name:        GetLiteralSize
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The key (literal) to find the size of. 
         * Return:      Size of the literal's memory location as int (in bytes). 
         * Description: This returns as an int the number of bytes the literal's storage size is.
         *              
         *****************************************************************************************/
        public int GetLiteralSize(string key)
        {
            if (IsLiteral(key))
                return (int)literalSizes[key];
            return -1;
        }

        /******************************************************************************************
         * 
         * Name:        GetMaxSize   
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The max size. 
         * Description: This method returns the max number of literals.
         *              
         *****************************************************************************************/
        override public int GetMaxSize()
        {
            return MAX_LITERALS;
        }

        /******************************************************************************************
         * 
         * Name:        GetSize   
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The number of literals. 
         * Description: This method returns the number of literals that are stored in the table.
         *              
         *****************************************************************************************/
        override public int GetSize()
        {
            return numLiterals;
        }

        /******************************************************************************************
         * 
         * Name:        Hash     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The literal and its location as strings. 
         * Return:      N/A 
         * Description: This method takes the literal as the key and the location as the value and
         *              uses the built in hashing function to store them in the hash table.
         *              It will also store the memory size of the literal (as bytes) to the sizes
         *              hash table using the literal as the key.
         *              
         *****************************************************************************************/
        override public void Hash(string key, string location)
        {
            if (!IsLiteral(key))
            {
                int literalSize = 0;
                int delimiter = key.IndexOf('\'');

                if (delimiter < 0)
                    delimiter = key.IndexOf('(');

                string type = key.Substring(1, delimiter - 1);
                string value = key.Substring(delimiter + 1, key.Length - delimiter - 2);

                switch (type)
                {
                    case "A":
                    case "V":
                        
                        literalSize = 4;

                        foreach (char character in value)
                        {
                            if (character == ',')
                                literalSize += 4;
                        } 
                        break;

                    case "B":
                        literalSize = value.Length;
                        literalSize /= 4;
                        if (literalSize == 0)
                            literalSize++;
                        break;

                    case "C":
                        literalSize = value.Length;
                        break;

                    case "D":
                        literalSize = 8;
                        break;

                    case "F":
                        literalSize = 4;
                        break;
                    
                    case "H":
                        literalSize = 2;
                        break;
                    
                    case "P":
                        literalSize = value.Length;
                        if (literalSize % 2 == 0)
                            literalSize += 2;
                        else
                            literalSize++;
                        literalSize /= 2;
                        break;
                    
                    case "X":
                        literalSize = value.Length;
                        if (literalSize % 2 != 0)
                            literalSize++;

                        literalSize /= 2;
                        break;

                    case "Z":
                        literalSize = value.Length;
                        break;
                }

                literalSizes.Add(key, literalSize);
                table.Add(key, location);
                numLiterals++;
            }
        }

        /******************************************************************************************
         * 
         * Name:        isLiteral   
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The literal as a string, it is used at the key for the hash table. 
         * Return:      True if the literal is in the table, false if otherwise. 
         * Description: This method checks the table for the existance of a literal.
         *              
         *****************************************************************************************/
        public bool IsLiteral(string key)
        {
            return table.ContainsKey(key);
        }

        /******************************************************************************************
         * 
         * Name:        isLiteralFull     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      True if number of literals in table is 50. 
         * Description: This method checks the number of literals in the table and returns whether
         *              or not the number is equal to the max number of literals (50).
         *              
         *****************************************************************************************/
        public bool IsLiteralFull()
        {
            return (table.Count == MAX_LITERALS);
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
         * Name:        isTableFull     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      True if table is full. 
         * Description: This method checks the number of literals in the table and returns whether
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
         * Description: This method prints to the console the contents of the literal table.
         *              Used for testing only.
         *              
         *****************************************************************************************/
        override public void PrintTable()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("|     Literal Table    |");
            Console.WriteLine("| Literal   | Address  |");
            Console.WriteLine("|----------------------|");

            foreach (var key in table.Keys)
                Console.WriteLine(String.Format("| {0} | {1} ", key, table[key]));

            Console.WriteLine("------------------------");
        }

        /******************************************************************************************
         * 
         * Name:        UpdateAddress
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The key and the address to update. 
         * Return:      N/A
         * Description: This method updates the address of the given symbol to the address passed
         *              to it.
         *              
         *****************************************************************************************/
        public void UpdateAddress(string key, string address)
        {
            if (IsLiteral(key))
                table[key] = address;
        }

    }
}


